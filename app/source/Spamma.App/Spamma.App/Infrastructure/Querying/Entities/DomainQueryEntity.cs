using System.Diagnostics.CodeAnalysis;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class DomainQueryEntity : IQueryEntity
{
    private DomainQueryEntity()
    {
        this.Subdomains = new List<SubdomainQueryEntity>();
        this.DomainAccessPolicies = new List<DomainAccessPolicyQueryEntity>();
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = null!;

    internal Guid CreatedUserId { get; private set; }

    internal DateTime WhenCreated { get; private set; }

    internal UserQueryEntity? CreatedUser { get; private set; }

    internal IReadOnlyList<SubdomainQueryEntity> Subdomains { get; private set; }

    internal IReadOnlyList<DomainAccessPolicyQueryEntity> DomainAccessPolicies { get; private set; }
}