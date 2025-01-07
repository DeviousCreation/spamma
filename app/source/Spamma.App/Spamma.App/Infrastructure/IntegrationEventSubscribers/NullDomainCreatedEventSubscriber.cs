using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullDomainCreatedEventSubscriber(ILogger<NullDomainCreatedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.DomainCreated)]
    public Task JobResultMerge(DomainCreatedIntegrationEvent ev)
    {
        logger.LogInformation("Domain.DomainCreated {@Event}", ev);
        return Task.CompletedTask;
    }
}