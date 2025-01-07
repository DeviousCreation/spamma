using System.Reflection;
using System.Runtime.CompilerServices;
using MaintenanceModeMiddleware.Configuration.Enums;
using MaintenanceModeMiddleware.Extensions;
using MediatR.Behaviors.Authorization.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Savorboard.CAP.InMemoryMessageQueue;
using SmtpServer;
using SmtpServer.Storage;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Contracts.Web;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Components;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Emailing;
using Spamma.App.Infrastructure.Contracts.Emailing.Receiving;
using Spamma.App.Infrastructure.Contracts.Emailing.Sending;
using Spamma.App.Infrastructure.Contracts.SutWrappers;
using Spamma.App.Infrastructure.Contracts.Web;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.DomainAggregate;
using Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.EmailAggregate;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.SettingAggregate;
using Spamma.App.Infrastructure.Domain.SettingAggregate.Aggregate;
using Spamma.App.Infrastructure.Emailing;
using Spamma.App.Infrastructure.Emailing.Receiving;
using Spamma.App.Infrastructure.Emailing.Sending;
using Spamma.App.Infrastructure.Extensions;
using Spamma.App.Infrastructure.Web;

using ApiCurrentUserService = Spamma.App.Infrastructure.Web.CurrentUserService;
using BlazorCurrentUserService = Spamma.App.Client.Infrastructure.Web.CurrentUserService;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(opt => builder.Configuration.GetSection("Settings").Bind(opt));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
    {
        
        opt.LoginPath = "/sign-in";
    });
builder.Services.AddAuthentication(AuthConstants.ConfigScheme)
    .AddCookie(AuthConstants.ConfigScheme, opt =>
    {
        opt.LoginPath = "/config/sign-in";
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazorBootstrap();
builder.Services.AddLogging();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
// Adds the transient pipeline behavior and additionally registers all `IAuthorizationHandlers` for a given assembly
builder.Services.AddMediatorAuthorization(Assembly.GetExecutingAssembly());
// Register all `IAuthorizer` implementations for a given assembly
builder.Services.AddAuthorizersFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IRepository<Domain>, DomainRepository>();
builder.Services.AddScoped<IRepository<Email>, EmailRepository>();
builder.Services.AddScoped<IRepository<Subdomain>, SubdomainRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Setting>, SettingRepository>();
builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddScoped<IAuthTokenProvider, AuthTokenProvider>();
builder.Services.AddScoped<ICommander, Commander>();
builder.Services.AddScoped<IQuerier, Querier>();
builder.Services.AddSingleton<ICodeLoginProvider, CodeLoginProvider>();
builder.Services.AddSingleton<ICodeGenerator, CodeGenerator>();

builder.Services.AddScoped<ICurrentUserServiceFactory, CurrentUserServiceFactory>();
builder.Services.AddScoped<ApiCurrentUserService>();
builder.Services.AddMaintenance();
//builder.Services.AddScoped<BlazorCurrentUserService>();
builder.Services.AddSingleton<IClock>(SystemClock.Instance);

builder.Services.AddCap(x =>
{
    x.UseInMemoryStorage();
    x.UseInMemoryMessageQueue();
});

builder.Services.AddDbContextFactory<SpammaDataContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["Settings:ConnectionString"]);
    opts.LogTo(Console.WriteLine);
    opts.EnableSensitiveDataLogging();
    opts.UseSnakeCaseNamingConvention();
});

builder.Services.AddTransient<IMessageStore, SpammaMessageStore>();
builder.Services.AddSingleton(
    provider =>
    {
        var options = new SmtpServerOptionsBuilder()
            .ServerName("SMTP Server")
            .Port(9025)
            .Build();

        return new SmtpServer.SmtpServer(options, provider.GetRequiredService<IServiceProvider>());
    });
builder.Services.AddHostedService<SmtpHostedService>();
builder.Services.AddSingleton<IMessageStoreProvider, LocalMessageStoreProvider>();
builder.Services.AddSingleton<IDirectoryWrapper, DirectoryWrapper>();
builder.Services.AddSingleton<IFileWrapper, FileWrapper>();

builder.Services
    .AddFluentEmail(
        builder.Configuration["Settings:FromEmailAddress"],
        builder.Configuration["Settings:FromName"])
    .AddRazorRenderer()
    .AddSmtpSender(
        builder.Configuration["Settings:EmailSmtpHost"],
        int.Parse(builder.Configuration["Settings:EmailSmtpPort"]!));
builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.RegisterEventSubscribers();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IHostedService, StartupHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseMaintenance(options =>
{
    options.UseNoDefaultValues();
    options.BypassFileExtensions(new List<string>{".css", ".js", ".png", ".jpg", ".jpeg", ".gif", ".svg", ".ico", ".woff", ".woff2", ".ttf", ".eot", ".otf", ".map"});
    options.UseResponseFromFile("redirect.html", EnvDirectory.ContentRootPath);
    options.BypassUrlPath("/config", StringComparison.OrdinalIgnoreCase);
    options.BypassUrlPath("/app-offline", StringComparison.OrdinalIgnoreCase);
});

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Spamma.App.Client._Imports).Assembly);

app.ConfigureApi();

await app.RunAsync();