using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;

public record GetSubdomainsByGridParamsQuery(int Skip, int Take) :
    GridParams<GetSubdomainsByGridParamsQueryResult>(Skip, Take);