using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record EmailAuthCompletedIntegrationEvent(Guid UserId, DateTime WhenCompleted)
    : IIntegrationEvent
{
    public string EventName => EventNames.EmailDeleted;
}