using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

public record UserDetailsUpdatedIntegrationEvent : IIntegrationEvent
{
    public string EventName => EventNames.UserDetailsUpdated;
}