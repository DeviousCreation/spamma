using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.Queries;

public record ListDomainsQuery(int Skip, int Take) : IQuery<ListDomainsQueryResult>;