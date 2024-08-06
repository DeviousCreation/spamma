using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.Aggregate;

internal class SubdomainAccessPolicy : Entity
{
    private DateTime? _whenRevoked;

    internal SubdomainAccessPolicy(Guid userId, SubdomainAccessPolicyType policyType, DateTime whenAssigned)
    {
        this.Id = userId;
        this.PolicyType = policyType;
        this.WhenAssigned = whenAssigned;
    }

    private SubdomainAccessPolicy()
    {
    }

    internal SubdomainAccessPolicyType PolicyType { get; private set; }

    internal DateTime WhenAssigned { get; private set; }

    internal bool IsRevoked => this._whenRevoked != null;

    internal DateTime WhenRevoked
    {
        get
        {
            if (this._whenRevoked == null)
            {
                throw new InvalidOperationException("Domain access policy has not been revoked.");
            }

            return this._whenRevoked.Value;
        }
    }

    internal ResultWithError<ErrorData> Revoke(DateTime whenRevoked)
    {
        if (this._whenRevoked != null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.DomainAccessPolicyAlreadyRevoked, "Domain access policy has already been revoked."));
        }

        this._whenRevoked = whenRevoked;

        return ResultWithError.Ok<ErrorData>();
    }
}