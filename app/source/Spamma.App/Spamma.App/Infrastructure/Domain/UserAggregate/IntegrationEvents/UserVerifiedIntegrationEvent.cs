using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record UserVerifiedIntegrationEvent(Guid UserId, DateTime WhenVerified) : IIntegrationEvent
{
    public string EventName => EventNames.UserVerified;
}