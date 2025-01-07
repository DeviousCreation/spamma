using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;

public record GetAccessPolicesForSubdomainByGridParamsQuery(Guid SubdomainId, int Skip, int Take) :
    GridParams<GetAccessPolicesForSubdomainByGridParamsQueryResult>(Skip, Take);