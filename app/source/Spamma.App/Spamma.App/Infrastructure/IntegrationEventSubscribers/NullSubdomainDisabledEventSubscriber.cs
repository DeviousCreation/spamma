using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullSubdomainDisabledEventSubscriber(
    ILogger<NullSubdomainDisabledEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.SubdomainDisabled)]
    public Task JobResultMerge(SubdomainDisabledIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.SubdomainDisabled {@Event}", ev);
        return Task.CompletedTask;
    }
}