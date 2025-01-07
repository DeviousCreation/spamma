using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserDetailsUpdatedEventSubscriber(
    ILogger<NullUserDetailsUpdatedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserDetailsUpdated)]
    public Task JobResultMerge(UserDetailsUpdatedIntegrationEvent ev)
    {
        logger.LogInformation("User.UserDetailsUpdated {@Event}", ev);
        return Task.CompletedTask;
    }
}