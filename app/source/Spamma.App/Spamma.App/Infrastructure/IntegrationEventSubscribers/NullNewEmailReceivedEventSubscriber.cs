using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.EmailAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullNewEmailReceivedEventSubscriber(
    ILogger<NullNewEmailReceivedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.NewEmailReceived)]
    public Task JobResultMerge(NewEmailReceivedIntegrationEvent ev)
    {
        logger.LogInformation("Email.NewEmailReceived {@Event}", ev);
        return Task.CompletedTask;
    }
}