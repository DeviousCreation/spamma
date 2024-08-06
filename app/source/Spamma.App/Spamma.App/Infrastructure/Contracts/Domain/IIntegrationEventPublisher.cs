using DotNetCore.CAP;

namespace Spamma.App.Infrastructure.Contracts.Domain;

internal interface IIntegrationEventPublisher
{
    Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent;
}

internal class IntegrationEventPublisher(ICapPublisher capPublisher) : IIntegrationEventPublisher
{
    public Task PublishAsync<TIntegrationEvent>(TIntegrationEvent @event, CancellationToken cancellationToken = default)
        where TIntegrationEvent : IIntegrationEvent
    {
        return capPublisher.PublishAsync(@event.EventName, @event, cancellationToken: cancellationToken);
    }
}