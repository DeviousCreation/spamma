using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

internal record UserRevokedAccessToSubdomainIntegrationEvent(Guid SubdomainId, Guid UserId, DateTime WhenRevoked)
    : IIntegrationEvent
{
    public string EventName => EventNames.UserRevokedAccessToSubdomain;
}