using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserEnabledEventSubscriber(
    ILogger<NullUserEnabledEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserEnabled)]
    public Task JobResultMerge(UserEnabledIntegrationEvent ev)
    {
        logger.LogInformation("User.UserEnabled {@Event}", ev);
        return Task.CompletedTask;
    }
}