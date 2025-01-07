// See https://aka.ms/new-console-template for more information

//using FluentEmail.Core;
//using FluentEmail.Core.Models;

using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;
using Spamma.App.Infrastructure.Querying.Entities;
using Tynamix.ObjectFiller;

// CONFIGURE HOST
using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((builder, services) =>
    {
        services
            .AddFluentEmail("test@spamma.dev")
            .AddSmtpSender("localhost", 9025);
        services.AddDbContextFactory<SpammaDataContext>(opts =>
        {
            opts.UseNpgsql("Server=127.0.0.1;Port=5432;Database=spamma;User Id=spamma_user;Password=spamma_password;");
            opts.LogTo(Console.WriteLine);
            opts.EnableSensitiveDataLogging();
            opts.UseSnakeCaseNamingConvention();
        });
    })
    .ConfigureLogging((_, logging) =>
    {
        logging.ClearProviders();
        logging.AddSimpleConsole(options => options.IncludeScopes = true);
    })
    .Build();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var fluentEmail = host.Services.GetRequiredService<IFluentEmail>();
var dbContextFactory = host.Services.GetRequiredService<IDbContextFactory<SpammaDataContext>>();

// LOAD DATA
var context = await dbContextFactory.CreateDbContextAsync();
var domains = await context.Set<DomainQueryEntity>()
    .SelectMany(d => d.Subdomains.Select(s => $"{s.Name}.{d.Name}")).ToListAsync();

// PREP DATA
var toEmails = EmailGenerator.GenerateRandomEmails(domains, 5);
var ccEmails = EmailGenerator.GenerateRandomEmails(domains, 3);
var bccEmails = EmailGenerator.GenerateRandomEmails(domains, 2);

var subject = Tynamix.ObjectFiller.Randomizer<string>.Create();
var body = Tynamix.ObjectFiller.Randomizer<string>.Create();

// CREATE AND SEND EMAIL
var response = await fluentEmail
    .To(toEmails)
    .CC(ccEmails)
    .BCC(bccEmails)
    .Body(body)
    .Subject(subject)
    .SendAsync();
logger.LogInformation("Response from smtp call: {@response}", response.Successful);

public static class EmailGenerator
{
    private static readonly Random Random = new Random();

    public static List<Address> GenerateRandomEmails(List<string> domains, int count)
    {
        var emails = new List<Address>();
        for (var i = 0; i < count; i++)
        {
            emails.Add(new Address($"user{Random.Next(1000, 9999)}@{domains[Random.Next(domains.Count)]}"));
        }
        return emails;
    }
}