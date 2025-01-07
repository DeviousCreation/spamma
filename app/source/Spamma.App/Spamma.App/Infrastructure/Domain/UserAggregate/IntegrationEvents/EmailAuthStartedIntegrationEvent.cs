using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record EmailAuthStartedIntegrationEvent(
    Guid UserId, string Name, string EmailAddress, Guid SecurityStamp, DateTime WhenStarted)
    : IIntegrationEvent
{
    public string EventName => EventNames.SignInProcessStarted;
}