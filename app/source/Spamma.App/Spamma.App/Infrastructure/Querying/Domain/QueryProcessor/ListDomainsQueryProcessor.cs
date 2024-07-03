using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Domain.QueryProcessor;

public class ListDomainsQueryProcessor : IQueryProcessor<ListDomainsQuery, ListDomainsQueryResult>
{
    private readonly SpammaDataContext _dbContext;

    public ListDomainsQueryProcessor(SpammaDataContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<QueryResult<ListDomainsQueryResult>> Handle(ListDomainsQuery request, CancellationToken cancellationToken)
    {
        var futureData = this._dbContext.Set<DomainQueryEntity>().Skip(request.Skip).Take(request.Take).Future();
        var futureCount = this._dbContext.Set<DomainQueryEntity>().DeferredCount().FutureValue();

        var data = await futureData.ToListAsync(cancellationToken);
        var count = await futureCount.ValueAsync(cancellationToken);

        return QueryResult<ListDomainsQueryResult>.Succeeded(
            new ListDomainsQueryResult(
                data.Select(x => new ListDomainsQueryResult.DomainItem(
            x.Id,
            x.Name)).ToList(), count));
    }
}