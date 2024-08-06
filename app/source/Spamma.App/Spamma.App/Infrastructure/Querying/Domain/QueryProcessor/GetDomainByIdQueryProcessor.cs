using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Querying.Domain.QueryProcessor;

internal class GetDomainByIdQueryProcessor(SpammaDataContext dbContext)
    : IQueryProcessor<GetDomainByIdQuery, GetDomainByIdQueryResult>
{
    public async Task<QueryResult<GetDomainByIdQueryResult>> Handle(GetDomainByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await dbContext.Set<DomainQueryEntity>()
            .Where(x => x.Id == request.Id)
            .Select(x => new
            {
                x.Id,
                x.Name,
            })
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        return data == null ?
            QueryResult<GetDomainByIdQueryResult>.Failed() :
            QueryResult<GetDomainByIdQueryResult>.Succeeded(new GetDomainByIdQueryResult(data.Id, data.Name));
    }
}