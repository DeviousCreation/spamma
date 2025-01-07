using System.Buffers;
using Louw.PublicSuffix;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NuGet.Versioning;
using SmtpServer;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Infrastructure.Contracts.Emailing.Receiving;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Emailing.Receiving;

public class SpammaMessageStore : MessageStore
{
    public override async Task<SmtpResponse> SaveAsync(
        ISessionContext context,
        IMessageTransaction transaction,
        ReadOnlySequence<byte> buffer,
        CancellationToken cancellationToken)
    {
        var scope = context.ServiceProvider.CreateScope();
        var messageStoreProvider = scope.ServiceProvider.GetRequiredService<IMessageStoreProvider>();
        var commander = scope.ServiceProvider.GetRequiredService<ICommander>();
        var dataProviderFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<SpammaDataContext>>();

        await using var stream = new MemoryStream();

        var position = buffer.GetPosition(0);
        while (buffer.TryGet(ref position, out var memory))
        {
            stream.Write(memory.Span);
        }

        stream.Position = 0;

        var message = await MimeKit.MimeMessage.LoadAsync(stream, cancellationToken);

        var domainGroups = message.To.Mailboxes.Select(x => new { Domain = x.Domain.ToLower(), x.Address })
            .Concat(message.Cc.Mailboxes.Select(x => new { Domain = x.Domain.ToLower(), x.Address }))
            .Concat(message.Bcc.Mailboxes.Select(x => new { Domain = x.Domain.ToLower(), x.Address }))
            .GroupBy(x => x.Domain);

        await using var dataProvider = await dataProviderFactory.CreateDbContextAsync(cancellationToken);

        var domainsInSystem = await dataProvider.Set<DomainQueryEntity>()
            .Select(domain => new
            {
                domain.Id,
                domain.Name,
                Subdomains = domain.Subdomains.Select(subdomain => new
                {
                    subdomain.Id,
                    subdomain.Name,
                    ChaosMonkeyAddresses = subdomain.ChaosMonkeyAddresses.Select(chaos => new
                    {
                        chaos.Id,
                        chaos.EmailAddress,
                        chaos.Type,
                    }),
                }),
            }).ToListAsync(cancellationToken: cancellationToken);

        var domainParser = new DomainParser(new WebTldRuleProvider());
        string? receivingAddress = null;
        Guid discoveredSubdomainId = Guid.Empty;
        foreach (var domainGroup in domainGroups)
        {
            var domainInfo = await domainParser.ParseAsync(domainGroup.Key);
            var foundDomain = domainsInSystem.Find(d =>
                string.Equals(d.Name, domainInfo.RegistrableDomain, StringComparison.InvariantCultureIgnoreCase));
            if (foundDomain == null)
            {
                continue;
            }

            var foundSubdomain =
                foundDomain.Subdomains.FirstOrDefault(s => string.Equals(s.Name, domainInfo.SubDomain));

            if (foundSubdomain == null)
            {
                continue;
            }

            receivingAddress = domainGroup.Select(x => x.Address).First();
            discoveredSubdomainId = foundSubdomain.Id;

            var foundChaos = foundSubdomain.ChaosMonkeyAddresses
                .FirstOrDefault(c => domainGroup.Select(dg => dg.Address.ToLower()).Contains(c.EmailAddress.ToLower()));
            if (foundChaos != null)
            {
                return foundChaos.Type switch
                {
                    ChaosMonkeyType.NotAllowed => SmtpResponse.MailboxNameNotAllowed,
                    ChaosMonkeyType.AuthFailed => SmtpResponse.AuthenticationFailed,
                    ChaosMonkeyType.AuthRequired => SmtpResponse.AuthenticationRequired,
                    ChaosMonkeyType.Unavailable => SmtpResponse.MailboxUnavailable,
                    ChaosMonkeyType.SizeLimitExceeded => SmtpResponse.SizeLimitExceeded,
                    ChaosMonkeyType.NoValidRecipients => SmtpResponse.NoValidRecipientsGiven,
                };
            }

            break;
        }

        if (string.IsNullOrWhiteSpace(receivingAddress) || discoveredSubdomainId == Guid.Empty)
        {
            return SmtpResponse.MailboxNameNotAllowed;
        }

        var messageId = Guid.NewGuid();
        var saveFileResult = await messageStoreProvider.StoreMessageContentAsync(messageId, message, cancellationToken);
        if (!saveFileResult.IsSuccess)
        {
            return SmtpResponse.TransactionFailed;
        }

        var saveDataResult = await commander.Send(
            new EmailReceivedCommand(
                messageId,
                discoveredSubdomainId,
                receivingAddress,
                message.Subject,
                message.Date.DateTime), cancellationToken);

        if (saveDataResult.Status != CommandResultStatus.Failed)
        {
            return SmtpResponse.Ok;
        }

        await messageStoreProvider.DeleteMessageContentAsync(messageId, cancellationToken);
        return SmtpResponse.TransactionFailed;
    }
}