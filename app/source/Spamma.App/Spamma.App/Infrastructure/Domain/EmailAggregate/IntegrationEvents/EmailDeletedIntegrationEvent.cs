using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate.IntegrationEvents;

internal record EmailDeletedIntegrationEvent(Guid SubdomainId, Guid EmailId)
    : IIntegrationEvent
{
    public string EventName => EventNames.EmailDeleted;
}