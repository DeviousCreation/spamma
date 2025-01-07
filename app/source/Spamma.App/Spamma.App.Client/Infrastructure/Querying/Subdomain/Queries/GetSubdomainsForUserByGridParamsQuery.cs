using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;

public record GetSubdomainsForUserByGridParamsQuery(Guid UserId, int Skip, int Take) :
    GridParams<GetSubdomainsForUserByGridParamsQueryResult>(Skip, Take);