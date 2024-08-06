using System.Net;
using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class SendEmailOnLoginAttemptEventSubscriber(
    IAuthTokenProvider authTokenProvider) : ICapSubscribe
{
    [CapSubscribe(EventNames.SignInProcessStarted)]
    public async Task JobResultMerge(EmailAuthStartedIntegrationEvent ev)
    {
        var token = await authTokenProvider.GetToken(ev.UserId, ev.SecurityStamp, ev.WhenStarted);
        if (token.IsFailure)
        {
            return;
        }

        var encodedString = WebUtility.UrlEncode(token.Value);
        Console.WriteLine(encodedString);
    }
}