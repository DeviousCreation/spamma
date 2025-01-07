using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.IntegrationEvents;

internal record UserAllowedAccessToSubdomainIntegrationEvent(
    Guid SubdomainId, Guid UserId, SubdomainAccessPolicyType SubdomainAccessPolicyType, DateTime WhenAllowed)
    : IIntegrationEvent
{
    public string EventName => EventNames.UserAllowedAccessToSubdomain;
}