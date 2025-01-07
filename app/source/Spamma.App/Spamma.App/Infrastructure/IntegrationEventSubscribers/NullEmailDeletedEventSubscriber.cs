using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.EmailAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullEmailDeletedEventSubscriber(
    ILogger<NullEmailDeletedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.EmailDeleted)]
    public Task JobResultMerge(NewEmailReceivedIntegrationEvent ev)
    {
        logger.LogInformation("Email.EmailDeleted {@Event}", ev);
        return Task.CompletedTask;
    }
}