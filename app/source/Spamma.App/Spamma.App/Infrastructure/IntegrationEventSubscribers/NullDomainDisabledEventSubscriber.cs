using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullDomainDisabledEventSubscriber(
    ILogger<NullDomainDisabledEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.DomainDisabled)]
    public Task JobResultMerge(DomainDisabledIntegrationEvent ev)
    {
        logger.LogInformation("Domain.DomainDisabled {@Event}", ev);
        return Task.CompletedTask;
    }
}