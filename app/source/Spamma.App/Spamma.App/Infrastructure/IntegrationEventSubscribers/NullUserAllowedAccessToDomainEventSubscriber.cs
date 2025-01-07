using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserAllowedAccessToDomainEventSubscriber(
    ILogger<NullUserAllowedAccessToDomainEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserAllowedAccessToDomain)]
    public Task JobResultMerge(UserAllowedAccessToDomainIntegrationEvent ev)
    {
        logger.LogInformation("Domain.UserAllowedAccessToDomain {@Event}", ev);
        return Task.CompletedTask;
    }
}