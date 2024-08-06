using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

internal record SubdomainCreatedIntegrationEvent(Guid SubdomainId, string Name, Guid DomainId)
    : IIntegrationEvent
{
    public string EventName => EventNames.SubdomainCreated;
}