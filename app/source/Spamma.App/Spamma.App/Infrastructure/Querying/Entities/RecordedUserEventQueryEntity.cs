using System.Diagnostics.CodeAnalysis;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Infrastructure.Querying.Entities;

[SuppressMessage("SonarAnalyzer.CSharp", "S3453", Justification = "Create via Entity Framework")]
[SuppressMessage("SonarAnalyzer.CSharp", "S1144", Justification = "Create via Entity Framework")]
internal class RecordedUserEventQueryEntity : IQueryEntity
{
    private RecordedUserEventQueryEntity()
    {
    }

    internal Guid Id { get; private set; }

    internal Guid UserId { get; private set; }

    internal UserActionType ActionType { get; private set; }

    internal DateTime WhenHappened { get; private set; }

    internal UserQueryEntity? User { get; private set; }
}