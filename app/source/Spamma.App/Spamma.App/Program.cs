using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Components;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.DomainAggregate;
using Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Settings>(opt => builder.Configuration.GetSection("Settings").Bind(opt));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazorBootstrap();
builder.Services.AddLogging();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddScoped<IRepository<Domain>, DomainRepository>();

builder.Services.AddDbContext<SpammaDataContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration["Settings:ConnectionString"]);
    opts.LogTo(Console.WriteLine);
    opts.EnableSensitiveDataLogging();
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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Spamma.App.Client._Imports).Assembly);

app.ConfigureApi();

await app.RunAsync();