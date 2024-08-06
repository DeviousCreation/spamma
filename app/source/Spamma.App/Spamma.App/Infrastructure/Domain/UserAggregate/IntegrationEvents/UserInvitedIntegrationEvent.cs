using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record UserInvitedIntegrationEvent(Guid UserId, string EmailAddress, Guid SecurityStamp, DateTime WhenInvited)
    : IIntegrationEvent
{
    public string EventName => EventNames.UserInvited;
}