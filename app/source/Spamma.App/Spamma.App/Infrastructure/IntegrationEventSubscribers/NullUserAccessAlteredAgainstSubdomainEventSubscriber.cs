using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserAccessAlteredAgainstSubdomainEventSubscriber(
    ILogger<NullUserAccessAlteredAgainstSubdomainEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserAccessAlteredAgainstSubdomain)]
    public Task JobResultMerge(UserAccessAlteredAgainstSubdomainIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.UserAccessAlteredAgainstSubdomain {@Event}", ev);
        return Task.CompletedTask;
    }
}