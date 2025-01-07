using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Querying.Processing.Subdomain;

internal class GetSubdomainByIdQueryProcessor(IDbContextFactory<SpammaDataContext> dbContextFactory) :
    IQueryProcessor<GetSubdomainByIdQuery, GetSubdomainByIdQueryResult>
{
    public async Task<QueryResult<GetSubdomainByIdQueryResult>> Handle(GetSubdomainByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var data = await dataContext.Set<SubdomainQueryEntity>()
            .Where(subdomain => subdomain.Id == request.Id && subdomain.SubdomainAccessPolicies
                .Any(sapInner => sapInner.UserId == userId && !sapInner.IsRevoked))
            .Select(x => new
            {
                x.Id,
                x.Name,
            })
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        return data == null ?
            QueryResult<GetSubdomainByIdQueryResult>.Failed() :
            QueryResult<GetSubdomainByIdQueryResult>.Succeeded(new GetSubdomainByIdQueryResult(data.Id, data.Name));
    }
}