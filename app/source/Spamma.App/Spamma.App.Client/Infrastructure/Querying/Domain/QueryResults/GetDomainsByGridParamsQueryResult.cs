using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

public record GetDomainsByGridParamsQueryResult(
    IReadOnlyList<GetDomainsByGridParamsQueryResult.Item> Items, int TotalItems)
    : IQueryResult
{
    public record Item(Guid DomainId, string Name);
}