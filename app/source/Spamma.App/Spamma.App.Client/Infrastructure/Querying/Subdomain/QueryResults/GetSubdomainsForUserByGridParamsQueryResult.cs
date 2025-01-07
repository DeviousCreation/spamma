using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

public record GetSubdomainsForUserByGridParamsQueryResult(
    IReadOnlyList<GetSubdomainsForUserByGridParamsQueryResult.Item> Items, int TotalItems) :
    ListQueryResultOf<GetSubdomainsForUserByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(
        Guid DomainId, string DomainName, Guid SubdomainId, string SubdomainName, DateTime WhenAssigned,
        SubdomainAccessPolicyType PolicyType);
}