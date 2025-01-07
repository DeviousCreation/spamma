using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;

public record GetSubdomainByIdQuery(Guid Id) : IQuery<GetSubdomainByIdQueryResult>;