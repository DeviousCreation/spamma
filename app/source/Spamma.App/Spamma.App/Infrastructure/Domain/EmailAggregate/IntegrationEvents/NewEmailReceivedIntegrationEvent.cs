using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate.IntegrationEvents;

internal record NewEmailReceivedIntegrationEvent(Guid SubdomainId, Guid EmailId, string Sender, string Receiver, string Subject)
    : IIntegrationEvent
{
    public string EventName => EventNames.NewEmailReceived;
}