using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

public class ChaosMonkeyAddress : Entity
{
    public ChaosMonkeyAddress(Guid id, string address, ChaosMonkeyType type)
    {
        this.Id = id;
        this.Address = address;
        this.Type = type;
    }

    private ChaosMonkeyAddress()
    {
    }

    public string Address { get; private set; }

    public ChaosMonkeyType Type { get; private set; }

    public ResultWithError<ErrorData> ChangeType(ChaosMonkeyType type)
    {
        this.Type = type;

        return ResultWithError.Ok<ErrorData>();
    }
}