using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.Queries;

public record GetAccessPolicesForDomainByGridParamsQuery(Guid DomainId, int Skip, int Take) :
    GridParams<GetAccessPolicesForDomainByGridParamsQueryResult>(Skip, Take);