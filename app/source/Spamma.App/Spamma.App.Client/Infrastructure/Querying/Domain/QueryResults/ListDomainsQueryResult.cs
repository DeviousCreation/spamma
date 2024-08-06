using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

public record ListDomainsQueryResult(IReadOnlyList<ListDomainsQueryResult.DomainItem> Items, int TotalItems)
    : IQueryResult
{
    public record DomainItem(Guid Id, string Name);
}