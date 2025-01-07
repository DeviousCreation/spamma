using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

public record GetAccessPolicesForSubdomainByGridParamsQueryResult(
    IReadOnlyList<GetAccessPolicesForSubdomainByGridParamsQueryResult.Item> Items, int TotalItems)
    : ListQueryResultOf<GetAccessPolicesForSubdomainByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(
        Guid Id,
        string Name,
        string EmailAddress,
        SubdomainAccessPolicyType Type,
        DateTime WhenAssigned);
}