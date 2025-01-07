using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserInvitedEventSubscriber(
    ILogger<NullUserInvitedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserInvited)]
    public Task JobResultMerge(UserInvitedIntegrationEvent ev)
    {
        logger.LogInformation("User.UserInvited {@Event}", ev);
        return Task.CompletedTask;
    }
}