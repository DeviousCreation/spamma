using System.Diagnostics.CodeAnalysis;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class DomainAccessPolicyQueryEntity : IQueryEntity
{
    private DomainAccessPolicyQueryEntity()
    {
    }

    internal Guid DomainId { get; private set; }

    internal Guid UserId { get; private set; }

    internal DomainAccessPolicyType PolicyType { get; private set; }

    internal DateTime WhenAssigned { get; private set; }

    internal bool IsRevoked { get; private set; }

    internal DateTime? WhenRevoked { get; private set; }

    internal DomainQueryEntity? Domain { get; private set; }

    internal UserQueryEntity? User { get; private set; }
}