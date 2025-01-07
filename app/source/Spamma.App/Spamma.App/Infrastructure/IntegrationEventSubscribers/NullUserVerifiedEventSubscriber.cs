using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullUserVerifiedEventSubscriber(
    ILogger<NullUserVerifiedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.UserVerified)]
    public Task JobResultMerge(UserVerifiedIntegrationEvent ev)
    {
        logger.LogInformation("User.UserVerified {@Event}", ev);
        return Task.CompletedTask;
    }
}