using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

public record UserDisabledIntegrationEvent(Guid UserId, string EmailAddress, DateTime WhenDisabled) : IIntegrationEvent
{
    public string EventName => EventNames.UserDisabled;
}