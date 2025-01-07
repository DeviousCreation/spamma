using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

public record GetSubdomainByIdQueryResult(Guid Id, string Name)
    : IQueryResult;