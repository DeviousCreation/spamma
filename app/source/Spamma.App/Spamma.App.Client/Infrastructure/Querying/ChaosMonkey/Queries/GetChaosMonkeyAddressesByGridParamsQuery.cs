using Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.Queries;

public record GetChaosMonkeyAddressesByGridParamsQuery(int Skip, int Take) :
    GridParams<GetChaosMonkeyAddressesByGridParamsQueryResult>(Skip, Take);