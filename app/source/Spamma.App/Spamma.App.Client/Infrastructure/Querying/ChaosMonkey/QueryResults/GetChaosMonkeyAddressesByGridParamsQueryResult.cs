using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.QueryResults;

public record GetChaosMonkeyAddressesByGridParamsQueryResult(
    IReadOnlyList<GetChaosMonkeyAddressesByGridParamsQueryResult.Item> Items, int TotalItems) :
    ListQueryResultOf<GetChaosMonkeyAddressesByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(
        Guid ChaosMonkeyAddressId,
        string EmailAddress,
        ChaosMonkeyType ChaosMonkeyType,
        Guid SubdomainId);
}