using DotNetCore.CAP;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Domain.UserAggregate.IntegrationEvents;

namespace Spamma.App.Infrastructure.IntegrationEventSubscribers;

internal class NullEmailAddressChangedEventSubscriber(
    ILogger<NullEmailAddressChangedEventSubscriber> logger)
    : ICapSubscribe
{
    [CapSubscribe(EventNames.EmailAddressChanged)]
    public Task JobResultMerge(EmailAddressChangedIntegrationEvent ev)
    {
        logger.LogInformation("user.EmailAddressChanged {@Event}", ev);
        return Task.CompletedTask;
    }
}