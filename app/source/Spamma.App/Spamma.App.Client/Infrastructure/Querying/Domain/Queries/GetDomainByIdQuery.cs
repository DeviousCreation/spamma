using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.Queries;

public record GetDomainByIdQuery(Guid Id) : IQuery<GetDomainByIdQueryResult>;