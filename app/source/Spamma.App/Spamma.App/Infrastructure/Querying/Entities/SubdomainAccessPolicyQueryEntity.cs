using System.Diagnostics.CodeAnalysis;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class SubdomainAccessPolicyQueryEntity : IQueryEntity
{
    private SubdomainAccessPolicyQueryEntity()
    {
    }

    internal Guid UserId { get; private set; }

    internal Guid SubdomainId { get; private set; }

    internal SubdomainAccessPolicyType PolicyType { get; private set; }

    internal DateTime WhenAssigned { get; private set; }

    internal bool IsRevoked { get; private set; }

    internal DateTime? WhenRevoked { get; private set; }

    internal UserQueryEntity? User { get; private set; }

    internal SubdomainQueryEntity? Subdomain { get; private set; }
}