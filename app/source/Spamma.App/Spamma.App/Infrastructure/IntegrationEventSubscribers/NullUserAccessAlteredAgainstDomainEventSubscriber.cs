using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserAccessAlteredAgainstDomainEventSubscriber(
    ILogger<NullUserAccessAlteredAgainstDomainEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserAllowedAccessToDomain)]
    public Task JobResultMerge(UserAccessAlteredAgainstDomainIntegrationEvent ev)
    {
        logger.LogInformation("Domain.UserAllowedAccessToDomain {@Event}", ev);
        return Task.CompletedTask;
    }
}