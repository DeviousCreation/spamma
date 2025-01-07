using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;

public record GetSubdomainsForDomainByGridParamsQuery(Guid DomainId, int Skip, int Take) :
    GridParams<GetSubdomainsForDomainByGridParamsQueryResult>(Skip, Take);