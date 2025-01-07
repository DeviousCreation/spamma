using System.Diagnostics.CodeAnalysis;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class EmailQueryEntity : IQueryEntity
{
    private EmailQueryEntity()
    {
    }

    internal Guid Id { get; private set; }

    internal Guid SubdomainId { get; private set; }

    internal string EmailAddress { get; private set; } = null!;

    internal string Subject { get; private set; } = null!;

    internal DateTimeOffset WhenSent { get; private set; }

    internal SubdomainQueryEntity? Subdomain { get; private set; }
}