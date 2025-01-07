using Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.Queries;

public record GetChaosMonkeyAddressesForSubdomainByGridParamsQuery(Guid SubdomainId, int Skip, int Take) :
    GridParams<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult>(Skip, Take);