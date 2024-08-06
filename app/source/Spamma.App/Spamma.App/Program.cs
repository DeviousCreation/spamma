using System.Runtime.CompilerServices;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Savorboard.CAP.InMemoryMessageQueue;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Components;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.DomainAggregate;
using Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.EmailAggregate;
using Spamma.App.Infrastructure.Domain.EmailAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate;
using Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;
using Spamma.App.Infrastructure.IntegrationEventSubscribers;
using Spamma.App.Infrastructure.Web;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(opt => builder.Configuration.GetSection("Settings").Bind(opt));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/sign-in";
    });
builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazorBootstrap();
builder.Services.AddLogging();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddScoped<IRepository<Domain>, DomainRepository>();
builder.Services.AddScoped<IRepository<Email>, EmailRepository>();
builder.Services.AddScoped<IRepository<Subdomain>, SubdomainRepository>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();
builder.Services.AddScoped<IAuthTokenProvider, AuthTokenProvider>();
builder.Services.AddScoped<ICommander, Commander>();
builder.Services.AddScoped<IQuerier, Querier>();
//builder.Services.AddScoped<ICapSubscribe, SendEmailOnLoginAttemptEventSubscriber>();
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