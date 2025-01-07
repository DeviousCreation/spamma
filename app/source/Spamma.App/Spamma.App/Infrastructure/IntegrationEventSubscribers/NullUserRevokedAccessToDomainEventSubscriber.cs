using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserRevokedAccessToDomainEventSubscriber(
    ILogger<NullUserRevokedAccessToDomainEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserRevokedAccessToDomain)]
    public Task JobResultMerge(UserRevokedAccessToDomainIntegrationEvent ev)
    {
        logger.LogInformation("Domain.UserRevokedAccessToDomain {@Event}", ev);
        return Task.CompletedTask;
    }
}