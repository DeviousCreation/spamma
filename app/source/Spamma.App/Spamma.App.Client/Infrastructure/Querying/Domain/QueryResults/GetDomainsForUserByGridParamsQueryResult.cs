using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

public record GetDomainsForUserByGridParamsQueryResult(
    IReadOnlyList<GetDomainsForUserByGridParamsQueryResult.DomainItem> Items, int TotalItems) :
    ListQueryResultOf<GetDomainsForUserByGridParamsQueryResult.DomainItem>(Items, TotalItems)
{
    public record DomainItem(Guid DomainId, string Name, DateTime WhenAssigned, DomainAccessPolicyType PolicyType);
}