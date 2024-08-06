using System.Diagnostics.CodeAnalysis;
using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.Aggregate;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class User : Entity, IAggregateRoot
{
    private readonly List<RecordedUserEvent> _recordedUserEvents;

    internal User(
        Guid id,
        string emailAddress,
        DateTime whenCreated)
    {
        this.Id = id;
        this.EmailAddress = emailAddress;
        this.WhenCreated = whenCreated;

        this._recordedUserEvents = new List<RecordedUserEvent>();
    }

    private User()
    {
        this._recordedUserEvents = new List<RecordedUserEvent>();
    }

    internal string EmailAddress { get; private set; } = null!;

    internal DateTime WhenCreated { get; private set; }

    internal Guid SecurityStamp { get; private set; }

    internal IReadOnlyCollection<RecordedUserEvent> RecordedUserEvents => this._recordedUserEvents.AsReadOnly();

    internal ResultWithError<ErrorData> ChangeEmailAddress(string newEmailAddress, DateTime whenChanged)
    {
        this.EmailAddress = newEmailAddress;
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.EmailAddressChanged, whenChanged));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> RecordSuccessfulLogin(DateTime whenHappened)
    {
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.SuccessfulLogin, whenHappened));
        this.SecurityStamp = Guid.NewGuid();
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> ConfirmInvitation(DateTime toDateTimeUtc)
    {
        throw new NotImplementedException();
    }

    internal ResultWithError<ErrorData> InitializeEmailChange(string requestEmailAddress, DateTime now)
    {
        throw new NotImplementedException();
    }

    internal ResultWithError<ErrorData> UpdateDetails(string firstName, string lastName)
    {
        throw new NotImplementedException();
    }

    internal ResultWithError<ErrorData> StartAuthViaEmail(DateTime now)
    {
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.LoginInitiated, now));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> CompleteAuthViaEmail(DateTime now)
    {
        throw new NotImplementedException();
    }
}