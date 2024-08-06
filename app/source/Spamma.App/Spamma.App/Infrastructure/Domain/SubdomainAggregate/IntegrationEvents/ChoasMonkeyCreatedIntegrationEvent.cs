using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

internal record ChoasMonkeyCreatedIntegrationEvent(Guid SubdomainId, Guid ChaosMonkeyAddressId, string Name, string Description)
    : IIntegrationEvent
{
    public string EventName => EventNames.ChaosMonkeyAddressCreated;
}