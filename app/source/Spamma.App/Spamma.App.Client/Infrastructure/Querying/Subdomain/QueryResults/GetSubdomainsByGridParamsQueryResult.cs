namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

public record GetSubdomainsByGridParamsQueryResult(
    IReadOnlyList<GetSubdomainsByGridParamsQueryResult.Item> Items, int TotalItems) :
    ListQueryResultOf<GetSubdomainsByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(
        Guid SubdomainId,
        string SubdomainName,
        Guid DomainId,
        string DomainName,
        DateTime WhenCreated);
}