namespace Spamma.App.Infrastructure.Contracts.Domain;

internal interface IIntegrationEvent
{
    string EventName { get; }
}