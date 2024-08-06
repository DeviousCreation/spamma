using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record UserInvitationConfirmedIntegrationEvent(Guid UserId, DateTime WhenConfirmed) : IIntegrationEvent
{
    public string EventName => EventNames.UserInvitationConfirmed;
}