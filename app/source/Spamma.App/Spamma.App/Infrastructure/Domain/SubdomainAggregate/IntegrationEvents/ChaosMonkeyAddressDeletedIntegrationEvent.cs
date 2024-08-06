using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

internal record ChaosMonkeyAddressDeletedIntegrationEvent(Guid SubdomainId, Guid ChaosMonkeyAddressId)
    : IIntegrationEvent
{
    public string EventName => EventNames.ChaosMonkeyAddressDeleted;
}