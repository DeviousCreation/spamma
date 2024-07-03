using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Infrastructure;

internal class GetDomainByIdQueryProcessor(IHttpClientFactory factory)
    : GenericQueryProcessor<GetDomainByIdQuery, GetDomainByIdQueryResult>(factory);