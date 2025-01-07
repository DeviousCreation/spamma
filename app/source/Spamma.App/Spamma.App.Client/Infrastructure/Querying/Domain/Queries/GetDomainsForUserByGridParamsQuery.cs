using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.Queries;

public record GetDomainsForUserByGridParamsQuery(Guid UserId, int Skip, int Take) :
    GridParams<GetDomainsForUserByGridParamsQueryResult>(Skip, Take);