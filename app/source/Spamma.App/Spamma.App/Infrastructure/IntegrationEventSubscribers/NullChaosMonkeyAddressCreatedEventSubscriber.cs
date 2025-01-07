using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullChaosMonkeyAddressCreatedEventSubscriber(
    ILogger<NullChaosMonkeyAddressCreatedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.ChaosMonkeyAddressCreated)]
    public Task JobResultMerge(ChaosMonkeyAddressCreatedIntegrationEvent ev)
    {
        logger.LogInformation("Subdomain.ChaosMonkeyAddressCreated {@Event}", ev);
        return Task.CompletedTask;
    }
}