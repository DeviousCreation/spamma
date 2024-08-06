using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

internal class Subdomain : Entity, IAggregateRoot
{
    private readonly List<ChaosMonkeyAddress> _chaosMonkeyAddresses;
    private readonly List<SubdomainAccessPolicy> _subdomainAccessPolicies;

    internal Subdomain(Guid id, string name, Guid domainId)
    {
        this.Id = id;
        this.Name = name;
        this.DomainId = domainId;
        this._chaosMonkeyAddresses = new List<ChaosMonkeyAddress>();
        this._subdomainAccessPolicies = new List<SubdomainAccessPolicy>();
    }

    private Subdomain()
    {
        this._chaosMonkeyAddresses = new List<ChaosMonkeyAddress>();
        this._subdomainAccessPolicies = new List<SubdomainAccessPolicy>();
    }

    internal string Name { get; private set; } = null!;

    internal Guid DomainId { get; private set; }

    internal IReadOnlyCollection<ChaosMonkeyAddress> ChaosMonkeyAddresses => this._chaosMonkeyAddresses.AsReadOnly();

    internal IReadOnlyCollection<SubdomainAccessPolicy> SubdomainAccessPolicies => this._subdomainAccessPolicies.AsReadOnly();

    internal ResultWithError<ErrorData> AddChaosMonkeyAddress(ChaosMonkeyAddress chaosMonkeyAddress)
    {
        if (this._chaosMonkeyAddresses.Exists(x => x.EmailAddress == chaosMonkeyAddress.EmailAddress))
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.ChaosMonkeyAddressAlreadyExists, "Chaos monkey address already exists."));
        }

        this._chaosMonkeyAddresses.Add(chaosMonkeyAddress);

        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> RemoveChaosMonkeyAddress(Guid addressId)
    {
        var chaosMonkeyAddress = this._chaosMonkeyAddresses.Find(x => x.Id == addressId);

        if (chaosMonkeyAddress == null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.ChaosMonkeyAddressNotFound, "Chaos monkey address not found."));
        }

        this._chaosMonkeyAddresses.Remove(chaosMonkeyAddress);

        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> UpdateChaosMonkeyAddress(Guid addressId, ChaosMonkeyType chaosMonkeyType)
    {
        var existingChaosMonkeyAddress = this._chaosMonkeyAddresses.Find(x => x.Id == addressId);

        if (existingChaosMonkeyAddress == null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.ChaosMonkeyAddressNotFound, "Chaos monkey address not found."));
        }

        existingChaosMonkeyAddress.ChangeType(chaosMonkeyType);

        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> SetUserAccess(Guid userId, SubdomainAccessPolicyType policyType, DateTime whenAllowed)
    {
        var existingDomainAccessPolicy = this._subdomainAccessPolicies.Find(x => x.Id == userId && !x.IsRevoked);

        if (existingDomainAccessPolicy != null)
        {
            if (existingDomainAccessPolicy.PolicyType == policyType)
            {
                return ResultWithError.Fail(
                    new ErrorData(
                        ErrorCodes.DomainAccessPolicyAlreadyExists, "Domain access policy already exists."));
            }

            existingDomainAccessPolicy.Revoke(whenAllowed);
        }

        this._subdomainAccessPolicies.Add(new SubdomainAccessPolicy(userId, policyType, whenAllowed));

        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> RevokeUserAccess(Guid userId, DateTime whenRevoked)
    {
        var domainAccessPolicy = this._subdomainAccessPolicies.Find(x =>
            x.Id == userId && !x.IsRevoked);

        if (domainAccessPolicy == null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.DomainAccessPolicyNotFound, "Domain access policy not found."));
        }

        var result = domainAccessPolicy.Revoke(whenRevoked);

        return result.IsFailure ? ResultWithError.Fail(result.Error) : ResultWithError.Ok<ErrorData>();
    }
}