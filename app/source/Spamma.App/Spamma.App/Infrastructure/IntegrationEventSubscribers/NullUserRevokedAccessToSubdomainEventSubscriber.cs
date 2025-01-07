using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserRevokedAccessToSubdomainEventSubscriber(
    ILogger<NullUserRevokedAccessToSubdomainEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserRevokedAccessToSubdomain)]
    public Task JobResultMerge(UserRevokedAccessToSubdomainIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.UserRevokedAccessToSubdomain {@Event}", ev);
        return Task.CompletedTask;
    }
}