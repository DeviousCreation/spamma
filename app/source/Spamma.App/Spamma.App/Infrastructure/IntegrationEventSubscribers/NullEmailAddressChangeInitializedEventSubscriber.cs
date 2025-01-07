using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullEmailAddressChangeInitializedEventSubscriber(
    ILogger<NullEmailAddressChangeInitializedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.EmailAddressChangeInitialized)]
    public Task JobResultMerge(EmailAddressChangeInitializedIntegrationEvent ev)
    {
        logger.LogInformation("User.EmailAddressChangeInitialized {@Event}", ev);
        return Task.CompletedTask;
    }
}