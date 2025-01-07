using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record UserVerificationRequestedIntegrationEvent(
    Guid UserId, string Name, string EmailAddress, Guid SecurityStamp, DateTime WhenHappened) : IIntegrationEvent
{
    public string EventName => EventNames.UserRequestedVerification;
}