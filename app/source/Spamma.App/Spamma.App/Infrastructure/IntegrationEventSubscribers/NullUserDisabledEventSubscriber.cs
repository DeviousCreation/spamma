using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserDisabledEventSubscriber(
    ILogger<NullUserDisabledEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserDisabled)]
    public Task JobResultMerge(UserDisabledIntegrationEvent ev)
    {
        logger.LogInformation("User.UserDisabled {@Event}", ev);
        return Task.CompletedTask;
    }
}