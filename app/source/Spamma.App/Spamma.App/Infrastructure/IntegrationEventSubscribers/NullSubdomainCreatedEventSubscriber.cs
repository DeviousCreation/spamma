using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullSubdomainCreatedEventSubscriber(
    ILogger<NullSubdomainCreatedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.SubdomainCreated)]
    public Task JobResultMerge(SubdomainCreatedIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.SubdomainCreated {@Event}", ev);
        return Task.CompletedTask;
    }
}