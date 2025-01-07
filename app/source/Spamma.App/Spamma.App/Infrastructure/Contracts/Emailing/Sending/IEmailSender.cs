using System.Collections.Immutable;
using ResultMonad;
using Spamma.App.Infrastructure.Constants;

namespace Spamma.App.Infrastructure.Contracts.Emailing.Sending;

internal interface IEmailSender
{
    Task<Result> SendEmailAsync(string name, string emailAddress, string subject,
        List<Tuple<EmailTemplateSection, ImmutableArray<string>>> body, CancellationToken cancellationToken = default);
}