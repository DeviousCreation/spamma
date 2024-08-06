using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

internal class ChaosMonkeyAddress : Entity
{
    internal ChaosMonkeyAddress(Guid id, string emailAddress, ChaosMonkeyType type)
    {
        this.Id = id;
        this.EmailAddress = emailAddress;
        this.Type = type;
    }

    private ChaosMonkeyAddress()
    {
    }

    public string EmailAddress { get; private set; } = null!;

    public ChaosMonkeyType Type { get; private set; }

    internal ResultWithError<ErrorData> ChangeType(ChaosMonkeyType type)
    {
        this.Type = type;

        return ResultWithError.Ok<ErrorData>();
    }
}