using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Subdomain;

internal class GetSubdomainsForDomainByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> dbContextFactory) :
    IQueryProcessor<GetSubdomainsForDomainByGridParamsQuery, GetSubdomainsForDomainByGridParamsQueryResult>
{
    public async Task<QueryResult<GetSubdomainsForDomainByGridParamsQueryResult>> Handle(GetSubdomainsForDomainByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(subdomain => subdomain.DomainId == request.DomainId && subdomain.SubdomainAccessPolicies
                .Any(sap => sap.UserId == userId && !sap.IsRevoked))
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.WhenCreated,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(subdomain => subdomain.DomainId == request.DomainId && subdomain.SubdomainAccessPolicies
                .Any(sap => sap.UserId == userId && !sap.IsRevoked))
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.WhenCreated,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetSubdomainsForDomainByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetSubdomainsForDomainByGridParamsQueryResult>.Succeeded(new GetSubdomainsForDomainByGridParamsQueryResult(
            data.Select(x => new GetSubdomainsForDomainByGridParamsQueryResult.Item(
                x.Id,
                x.Name)).ToList(), count));
    }
}