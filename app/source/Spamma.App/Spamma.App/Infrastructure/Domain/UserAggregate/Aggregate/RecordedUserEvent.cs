using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;

internal class RecordedUserEvent : Entity
{
    internal RecordedUserEvent(
        UserActionType actionType,
        DateTime whenHappened)
    {
        this.Id = Guid.NewGuid();
        this.ActionType = actionType;
        this.WhenHappened = whenHappened;
    }

    private RecordedUserEvent()
    {
    }

    internal UserActionType ActionType { get; private set; }

    internal DateTime WhenHappened { get; private set; }
}