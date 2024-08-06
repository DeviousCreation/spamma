using System.Diagnostics.CodeAnalysis;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class SubdomainQueryEntity : IQueryEntity
{
    private SubdomainQueryEntity()
    {
        this.ChaosMonkeyAddresses = new List<ChaosMonkeyAddressQueryEntity>();
        this.SubdomainAccessPolicies = new List<SubdomainAccessPolicyQueryEntity>();
        this.Emails = new List<EmailQueryEntity>();
    }

    internal Guid Id { get; private set; }

    internal string Name { get; private set; } = null!;

    internal Guid DomainId { get; private set; }

    internal DomainQueryEntity? Domain { get; private set; }

    internal IReadOnlyList<ChaosMonkeyAddressQueryEntity> ChaosMonkeyAddresses { get; private set; }

    internal IReadOnlyList<SubdomainAccessPolicyQueryEntity> SubdomainAccessPolicies { get; private set; }

    internal IReadOnlyList<EmailQueryEntity> Emails { get; private set; }
}