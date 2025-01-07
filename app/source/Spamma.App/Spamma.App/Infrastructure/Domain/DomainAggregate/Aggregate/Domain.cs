using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;

internal class Domain : Entity, IAggregateRoot
{
    private readonly List<DomainAccessPolicy> _domainAccessPolicies;
    private DateTime? _whenDisabled;

    internal Domain(Guid id, string name, Guid createdUserId, DateTime whenCreated)
    {
        this.Id = id;
        this.Name = name;
        this.CreatedUserId = createdUserId;
        this.WhenCreated = whenCreated;
        this._domainAccessPolicies =
        [
            new DomainAccessPolicy(createdUserId, DomainAccessPolicyType.Administrator, whenCreated)
        ];
    }

    private Domain()
    {
        this._domainAccessPolicies = [];
    }

    internal string Name { get; private set; } = null!;

    internal Guid CreatedUserId { get; private set; }

    internal DateTime WhenCreated { get; private set; }

    internal bool IsDisabled => this._whenDisabled != default;

    internal DateTime WhenDisabled
    {
        get
        {
            if (this._whenDisabled == null)
            {
                throw new InvalidOperationException("Domain is not disabled.");
            }

            return this._whenDisabled.Value;
        }
    }

    internal IReadOnlyList<DomainAccessPolicy> DomainAccessPolicies => this._domainAccessPolicies.AsReadOnly();

    internal ResultWithError<ErrorData> SetUserAccess(Guid userId, DomainAccessPolicyType policyType, DateTime whenAllowed)
    {
        var existingDomainAccessPolicy = this._domainAccessPolicies.Find(x => x.Id == userId && !x.IsRevoked);

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

        this._domainAccessPolicies.Add(new DomainAccessPolicy(userId, policyType, whenAllowed));

        return ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> RevokeUserAccess(Guid userId, DateTime whenRevoked)
    {
        var domainAccessPolicy = this._domainAccessPolicies.Find(x =>
            x.Id == userId && !x.IsRevoked);

        if (domainAccessPolicy == null)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.DomainAccessPolicyNotFound, "Domain access policy not found."));
        }

        var result = domainAccessPolicy.Revoke(whenRevoked);

        return result.IsFailure ? ResultWithError.Fail(result.Error) : ResultWithError.Ok<ErrorData>();
    }

    internal ResultWithError<ErrorData> Disable(DateTime whenDisabled)
    {
        if (this.IsDisabled)
        {
            return ResultWithError.Fail(new ErrorData(ErrorCodes.DomainAlreadyDisabled, "Domain already disabled."));
        }

        this._whenDisabled = whenDisabled;
        return ResultWithError.Ok<ErrorData>();
    }
}