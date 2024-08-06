using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

internal record UserRevokedAccessToDomainIntegrationEvent(Guid DomainId, Guid UserId)
    : IIntegrationEvent
{
    public string EventName => EventNames.UserRevokedAccessToDomain;
}