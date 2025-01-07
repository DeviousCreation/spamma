using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

public record GetSubdomainsForDomainByGridParamsQueryResult(
    IReadOnlyList<GetSubdomainsForDomainByGridParamsQueryResult.Item> Items, int TotalItems) :
    ListQueryResultOf<GetSubdomainsForDomainByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(Guid SubdomainId, string SubdomainName);
}