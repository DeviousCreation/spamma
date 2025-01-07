using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

internal record EmailAddressChangedIntegrationEvent(Guid UserId, string EmailAddress, DateTime WhenChanged)
    : IIntegrationEvent
{
    public string EventName => EventNames.EmailAddressChanged;
}