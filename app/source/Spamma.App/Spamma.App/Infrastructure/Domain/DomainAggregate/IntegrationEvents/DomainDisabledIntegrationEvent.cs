using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

public record DomainDisabledIntegrationEvent(Guid DomainId) : IIntegrationEvent
{
    public string EventName => EventNames.DomainDisabled;
}