using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullChaosMonkeyAddressDeletedEventSubscriber(
    ILogger<NullChaosMonkeyAddressDeletedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.ChaosMonkeyAddressDeleted)]
    public Task JobResultMerge(SubdomainCreatedIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.ChaosMonkeyAddressDeleted {@Event}", ev);
        return Task.CompletedTask;
    }
}