using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

public record UserEnabledIntegrationEvent(Guid UserId, string EmailAddress, DateTime WhenEnabled) : IIntegrationEvent
{
    public string EventName => EventNames.UserEnabled;
}