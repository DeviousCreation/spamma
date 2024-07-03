using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

public class Subdomain : Entity, IAggregateRoot
{
    private readonly List<ChaosMonkeyAddress> _chaosMonkeyAddresses;

    public Subdomain(Guid id, string name, Guid domainId)
    {
        this.Id = id;
        this.Name = name;
        this.DomainId = domainId;
        this._chaosMonkeyAddresses = new List<ChaosMonkeyAddress>();
    }

    private Subdomain()
    {
        this._chaosMonkeyAddresses = new List<ChaosMonkeyAddress>();
    }

    public string Name { get; private set; }

    public Guid DomainId { get; private set; }

    public IReadOnlyCollection<ChaosMonkeyAddress> ChaosMonkeyAddresses => this._chaosMonkeyAddresses.AsReadOnly();

    public ResultWithError<ErrorData> AddChaosMonkeyAddress(ChaosMonkeyAddress chaosMonkeyAddress)
    {
        if (this._chaosMonkeyAddresses.Exists(x => x.Address == chaosMonkeyAddress.Address))
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.ChaosMonkeyAddressAlreadyExists, "Chaos monkey address already exists."));
        }

        this._chaosMonkeyAddresses.Add(chaosMonkeyAddress);

        return ResultWithError.Ok<ErrorData>();
    }

    public ResultWithError<ErrorData> RemoveChaosMonkeyAddress(string address)
    {
        var chaosMonkeyAddress = this._chaosMonkeyAddresses.Find(x => x.Address == address);

        if (chaosMonkeyAddress == null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.ChaosMonkeyAddressNotFound, "Chaos monkey address not found."));
        }

        this._chaosMonkeyAddresses.Remove(chaosMonkeyAddress);

        return ResultWithError.Ok<ErrorData>();
    }

    public ResultWithError<ErrorData> UpdateChaosMonkeyAddress(ChaosMonkeyAddress chaosMonkeyAddress)
    {
        var existingChaosMonkeyAddress = this._chaosMonkeyAddresses.Find(x => x.Address == chaosMonkeyAddress.Address);

        if (existingChaosMonkeyAddress == null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.ChaosMonkeyAddressNotFound, "Chaos monkey address not found."));
        }

        existingChaosMonkeyAddress.ChangeType(chaosMonkeyAddress.Type);

        return ResultWithError.Ok<ErrorData>();
    }
}