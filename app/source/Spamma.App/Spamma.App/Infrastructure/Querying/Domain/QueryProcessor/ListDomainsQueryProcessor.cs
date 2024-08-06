using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Domain.QueryProcessor;

internal class ListDomainsQueryProcessor(SpammaDataContext dbContext)
    : IQueryProcessor<ListDomainsQuery, ListDomainsQueryResult>
{
    public async Task<QueryResult<ListDomainsQueryResult>> Handle(ListDomainsQuery request, CancellationToken cancellationToken)
    {
        var futureData = dbContext.Set<DomainQueryEntity>().Skip(request.Skip).Take(request.Take).Future();
        var futureCount = dbContext.Set<DomainQueryEntity>().DeferredCount().FutureValue();

        var data = await futureData.ToListAsync(cancellationToken);
        var count = await futureCount.ValueAsync(cancellationToken);

        return QueryResult<ListDomainsQueryResult>.Succeeded(
            new ListDomainsQueryResult(
                data.Select(x => new ListDomainsQueryResult.DomainItem(
            x.Id,
            x.Name)).ToList(), count));
    }
}