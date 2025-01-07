using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record EmailAddressChangeInitializedIntegrationEvent(
    Guid UserId, string EmailAddress, Guid SecurityStamp, DateTime WhenInitialized)
    : IIntegrationEvent
{
    public string EventName => EventNames.EmailAddressChangeInitialized;
}