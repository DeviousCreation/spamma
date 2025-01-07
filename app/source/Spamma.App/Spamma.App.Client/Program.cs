using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NodaTime;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Contracts.Web;
using Spamma.App.Client.Infrastructure.Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorBootstrap();
builder.Services.AddMediatR(
    configuration => configuration.RegisterServicesFromAssemblies(
        typeof(Program).Assembly));
builder.Services.AddAuthorizationCore();
//builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<ICommander, Commander>();
builder.Services.AddScoped<IQuerier, Querier>();

builder.Services.AddSingleton<IClock>(SystemClock.Instance);

builder.Services.AddHttpClient("spamma", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

await builder.Build().RunAsync();