namespace Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

public record GetUsersByGridParamsQueryResult(IReadOnlyList<GetUsersByGridParamsQueryResult.Item> Items, int TotalItems) :
    ListQueryResultOf<GetUsersByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(
        Guid Id,
        string Name,
        string EmailAddress,
        DateTime WhenInvited,
        DateTime? WhenVerified,
        DateTime? LastLoginAt,
        DateTime? WhenDisabled,
        int DomainCount,
        int SubdomainCount);
}