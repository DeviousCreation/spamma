using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.IntegrationEvents;

internal record UserAllowedAccessToDomainIntegrationEvent(
    Guid DomainId,
    Guid UserId,
    DomainAccessPolicyType DomainAccessPolicyType)
    : IIntegrationEvent
{
    public string EventName => EventNames.UserAllowedAccessToDomain;
}