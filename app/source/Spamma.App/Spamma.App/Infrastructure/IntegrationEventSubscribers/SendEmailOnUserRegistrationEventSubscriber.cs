using System.Collections.Immutable;
using System.Net;
using DotNetCore.CAP;
using Microsoft.Extensions.Options;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Contracts.Emailing;
using Spamma.App.Infrastructure.Contracts.Emailing.Sending;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class SendEmailOnUserRegistrationEventSubscriber(
    IAuthTokenProvider authTokenProvider, IEmailSender emailSender, IOptions<Settings> settings) : ICapSubscribe
{
    private readonly Settings _settings = settings.Value;

    [CapSubscribe(EventNames.UserRegistered)]
    public async Task JobResultMerge(UserRegisteredIntegrationEvent ev)
    {
        var token = await authTokenProvider.GetToken(ev.UserId, ev.SecurityStamp, ev.WhenRegistered);
        if (token.IsFailure)
        {
            return;
        }

        var encodedString = WebUtility.UrlEncode(token.Value);
        var emailBody = new List<Tuple<EmailTemplateSection, ImmutableArray<string>>>
        {
            Tuple.Create(EmailTemplateSection.ActionLink, new ImmutableArray<string>
            {
                string.Format(this._settings.RegistrationUri, encodedString),
                "Finish registration",
            }),
        };
        await emailSender.SendEmailAsync(ev.Name, ev.EmailAddress, "Register", emailBody);
    }
}