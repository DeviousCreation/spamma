using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Querying.Domain.QueryProcessor;

internal class GetDomainByNameQueryProcessor(SpammaDataContext dbContext)
    : IQueryProcessor<GetDomainByNameQuery, GetDomainByNameQueryResult>
{
    public Task<QueryResult<GetDomainByNameQueryResult>> Handle(
        GetDomainByNameQuery request, CancellationToken cancellationToken)
    {
        return dbContext.Set<DomainQueryEntity>()
            .Where(x => x.Name == request.Name)
            .Select(x => new
            {
                x.Id,
                x.Name,
            })
            .SingleOrDefaultAsync(cancellationToken: cancellationToken)
            .ContinueWith(
                task =>
                {
                    var data = task.Result;
                    return data == null
                        ? QueryResult<GetDomainByNameQueryResult>.Failed()
                        : QueryResult<GetDomainByNameQueryResult>.Succeeded(
                            new GetDomainByNameQueryResult(data.Id, data.Name));
                }, cancellationToken);
    }
}