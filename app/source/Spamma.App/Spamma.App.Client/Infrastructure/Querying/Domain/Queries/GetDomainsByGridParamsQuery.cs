using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.Queries;

public record GetDomainsByGridParamsQuery(int Skip, int Take) : GridParams<GetDomainsByGridParamsQueryResult>(Skip, Take);