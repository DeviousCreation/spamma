using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.QueryResults;

public record GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult(
    IReadOnlyList<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult.Item> Items, int TotalItems) :
    ListQueryResultOf<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(Guid ChaosMonkeyAddressId, string EmailAddress, ChaosMonkeyType ChaosMonkeyType);
}