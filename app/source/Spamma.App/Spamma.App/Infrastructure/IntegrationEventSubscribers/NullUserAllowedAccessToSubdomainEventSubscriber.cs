using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserAllowedAccessToSubdomainEventSubscriber(
    ILogger<NullUserAllowedAccessToSubdomainEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserAllowedAccessToSubdomain)]
    public Task JobResultMerge(UserAllowedAccessToSubdomainIntegrationEvent ev)
    {
        logger.LogInformation(EventNames.UserAllowedAccessToSubdomain + " " + "{@Event}", ev);
        return Task.CompletedTask;
    }
}