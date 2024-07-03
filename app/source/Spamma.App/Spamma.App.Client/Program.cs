using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddBlazorBootstrap();

builder.Services.AddMediatR(
    configuration => configuration.RegisterServicesFromAssemblies(
        typeof(Program).Assembly));

builder.Services.AddHttpClient("spamma", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

await builder.Build().RunAsync();