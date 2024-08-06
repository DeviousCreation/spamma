using System.Diagnostics.CodeAnalysis;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class UserQueryEntity : IQueryEntity
{
    private UserQueryEntity()
    {
        this.RecordedUserEventQueryEntities = new List<RecordedUserEventQueryEntity>();
        this.SubdomainAccessPolicies = new List<SubdomainAccessPolicyQueryEntity>();
        this.DomainAccessPolicies = new List<DomainAccessPolicyQueryEntity>();
    }

    internal Guid Id { get; private set; }

    internal string EmailAddress { get; private set; } = null!;

    internal DateTime WhenCreated { get; private set; }

    internal IReadOnlyList<RecordedUserEventQueryEntity> RecordedUserEventQueryEntities { get; private set; }

    internal IReadOnlyList<SubdomainAccessPolicyQueryEntity> SubdomainAccessPolicies { get; private set; }

    internal IReadOnlyList<DomainAccessPolicyQueryEntity> DomainAccessPolicies { get; private set; }
}