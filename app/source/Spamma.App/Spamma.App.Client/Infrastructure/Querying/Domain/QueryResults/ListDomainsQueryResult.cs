namespace Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

public record ListDomainsQueryResult(IReadOnlyList<ListDomainsQueryResult.DomainItem> Items, int TotalItems)
{
    public record DomainItem(Guid Id, string Name);
}