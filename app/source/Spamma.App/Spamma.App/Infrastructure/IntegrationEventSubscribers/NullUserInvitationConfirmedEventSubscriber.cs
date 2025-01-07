using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserInvitationConfirmedEventSubscriber(
    ILogger<NullUserInvitationConfirmedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserInvitationConfirmed)]
    public Task JobResultMerge(UserInvitationConfirmedIntegrationEvent ev)
    {
        logger.LogInformation("User.UserInvitationConfirmed {@Event}", ev);
        return Task.CompletedTask;
    }
}