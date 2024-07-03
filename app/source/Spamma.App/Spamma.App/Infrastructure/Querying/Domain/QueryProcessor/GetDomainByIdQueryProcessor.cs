using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Querying.Domain.QueryProcessor;

public class GetDomainByIdQueryProcessor : IQueryProcessor<GetDomainByIdQuery, GetDomainByIdQueryResult>
{
    private readonly SpammaDataContext _dbContext;

    public GetDomainByIdQueryProcessor(SpammaDataContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<QueryResult<GetDomainByIdQueryResult>> Handle(GetDomainByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await this._dbContext.Set<DomainQueryEntity>()
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