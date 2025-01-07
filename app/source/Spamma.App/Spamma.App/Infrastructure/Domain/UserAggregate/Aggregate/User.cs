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
    private DateTime? _whenDisabled;

    internal User(
        Guid id,
        string name,
        string emailAddress,
        DateTime whenCreated)
    {
        this.Id = id;
        this.Name = name;
        this.EmailAddress = emailAddress;
        this.WhenCreated = whenCreated;

        this._recordedUserEvents = new List<RecordedUserEvent>();
    }

    private User()
    {
        this._recordedUserEvents = new List<RecordedUserEvent>();
    }

    internal string Name { get; private set; } = null!;

    internal string EmailAddress { get; private set; } = null!;

    internal DateTime WhenCreated { get; private set; }

    internal Guid SecurityStamp { get; private set; }

    internal bool IsVerified => this._recordedUserEvents.Exists(
        x => x.ActionType == UserActionType.AccountVerified);

    internal DateTime WhenVerified
    {
        get
        {
            var verifiedEvent = this._recordedUserEvents.Find(
                x => x.ActionType == UserActionType.AccountVerified);

            if (verifiedEvent == null)
            {
                throw new InvalidOperationException("User has not been verified.");
            }

            return verifiedEvent.WhenHappened;
        }
    }

    internal bool IsDisabled => this._whenDisabled != default;

    internal DateTime WhenDisabled
    {
        get
        {
            if (this._whenDisabled == default)
            {
                throw new InvalidOperationException("User is not disabled.");
            }

            return this._whenDisabled.Value;
        }
    }

    internal IReadOnlyCollection<RecordedUserEvent> RecordedUserEvents => this._recordedUserEvents.AsReadOnly();

    internal ResultWithError<ErrorData> InitializeEmailChange(DateTime whenHappened)
    {
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.EmailAddressChangedInitialized, whenHappened));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> ChangeEmailAddress(string newEmailAddress, DateTime whenChanged)
    {
        this.EmailAddress = newEmailAddress;
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.EmailAddressChanged, whenChanged));
        return ResultWithError.Ok<ErrorData>();
    }

    public ResultWithError<ErrorData> RequestVerification(DateTime whenHappened)
    {
        if (this.IsVerified)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.UserAlreadyVerified, "User already verified."));
        }

        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.AccountVerficationRequested, whenHappened));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> ConfirmVerification(DateTime whenHappened)
    {
        if (this.IsVerified)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.UserAlreadyVerified, "User already verified."));
        }

        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.AccountVerified, whenHappened));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> UpdateDetails(string name, DateTime whenHappened)
    {
        this.Name = name;
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.DetailsUpdated, whenHappened));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> StartAuthViaEmail(DateTime whenHappened)
    {
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.LoginInitiated, whenHappened));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> CompleteAuthViaEmail(DateTime whenHappened)
    {
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.SuccessfulLogin, whenHappened));
        this.SecurityStamp = Guid.NewGuid();
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> Disable(DateTime whenDisabled)
    {
        if (this.IsDisabled)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.DomainAlreadyDisabled, "User already enabled."));
        }

        this._whenDisabled = whenDisabled;
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.AccountDisabled, whenDisabled));
        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> Enable(DateTime whenEnable)
    {
        if (!this.IsDisabled)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.DomainAlreadyDisabled, "User already enabled."));
        }

        this._whenDisabled = null;
        this._recordedUserEvents.Add(new RecordedUserEvent(UserActionType.AccountDisabled, whenEnable));
        return ResultWithError.Ok<ErrorData>();
    }
}