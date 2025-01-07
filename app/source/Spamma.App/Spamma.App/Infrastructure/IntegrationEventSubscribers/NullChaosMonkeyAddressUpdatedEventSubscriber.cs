using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullChaosMonkeyAddressUpdatedEventSubscriber(
    ILogger<NullChaosMonkeyAddressUpdatedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.ChaosMonkeyAddressUpdated)]
    public Task JobResultMerge(ChaosMonkeyAddressUpdatedIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.ChaosMonkeyAddressUpdated {@Event}", ev);
        return Task.CompletedTask;
    }
}