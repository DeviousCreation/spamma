using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record UserRegisteredIntegrationEvent(
    Guid UserId, string Name, string EmailAddress, Guid SecurityStamp, DateTime WhenRegistered)
    : IIntegrationEvent
{
    public string EventName => EventNames.UserRegistered;
}