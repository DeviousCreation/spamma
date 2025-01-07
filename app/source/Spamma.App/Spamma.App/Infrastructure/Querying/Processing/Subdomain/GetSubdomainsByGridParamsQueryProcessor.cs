using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Subdomain;

internal class GetSubdomainsByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> dbContextFactory) :
    IQueryProcessor<GetSubdomainsByGridParamsQuery, GetSubdomainsByGridParamsQueryResult>
{
    public async Task<QueryResult<GetSubdomainsByGridParamsQueryResult>> Handle(GetSubdomainsByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(domain => domain.SubdomainAccessPolicies
                .Any(sap => sap.UserId == userId && !sap.IsRevoked))
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.DomainId,
                DomainName = s.Domain!.Name,
                s.WhenCreated,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(domain => domain.SubdomainAccessPolicies
                .Any(sap => sap.UserId == userId && !sap.IsRevoked))
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.DomainId,
                DomainName = s.Domain!.Name,
                s.WhenCreated,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetSubdomainsByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetSubdomainsByGridParamsQueryResult>.Succeeded(new GetSubdomainsByGridParamsQueryResult(
            data.Select(x => new GetSubdomainsByGridParamsQueryResult.Item(
                x.Id,
                x.Name,
                x.DomainId,
                x.DomainName,
                x.WhenCreated)).ToList(), count));
    }
}