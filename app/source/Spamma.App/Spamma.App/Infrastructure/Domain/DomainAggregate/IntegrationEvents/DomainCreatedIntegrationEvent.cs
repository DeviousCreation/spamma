using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

internal record DomainCreatedIntegrationEvent(Guid DomainId, string DomainName)
    : IIntegrationEvent
{
    public string EventName => EventNames.DomainCreated;
}