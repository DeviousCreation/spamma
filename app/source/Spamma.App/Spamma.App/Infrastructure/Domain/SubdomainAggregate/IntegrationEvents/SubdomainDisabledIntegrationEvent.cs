using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

internal record SubdomainDisabledIntegrationEvent(Guid SubdomainId, DateTime WhenDisabled)
    : IIntegrationEvent
{
    public string EventName => EventNames.SubdomainDisabled;
}