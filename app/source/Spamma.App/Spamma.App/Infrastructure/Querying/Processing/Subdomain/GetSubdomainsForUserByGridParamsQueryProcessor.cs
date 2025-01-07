using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Subdomain;

internal class GetSubdomainsForUserByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> dbContextFactory) :
    IQueryProcessor<GetSubdomainsForUserByGridParamsQuery, GetSubdomainsForUserByGridParamsQueryResult>
{
    public async Task<QueryResult<GetSubdomainsForUserByGridParamsQueryResult>> Handle(GetSubdomainsForUserByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(domain => domain.SubdomainAccessPolicies
                .Any(sap => 
                    (sap.UserId == userId && !sap.IsRevoked) &&
                    (sap.User.Id == request.UserId && !sap.IsRevoked)))
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.DomainId,
                DomainName = d.Domain!.Name,
                d.SubdomainAccessPolicies.Single(sap => sap.UserId == request.UserId).PolicyType,
                d.SubdomainAccessPolicies.Single(sap => sap.UserId == request.UserId).WhenAssigned,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(domain => domain.SubdomainAccessPolicies
                .Any(sap => 
                    (sap.UserId == userId && !sap.IsRevoked) &&
                    (sap.User.Id == request.UserId && !sap.IsRevoked)))
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.DomainId,
                DomainName = d.Domain!.Name,
                d.SubdomainAccessPolicies.Single(sap => sap.UserId == request.UserId).PolicyType,
                d.SubdomainAccessPolicies.Single(sap => sap.UserId == request.UserId).WhenAssigned,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetSubdomainsForUserByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetSubdomainsForUserByGridParamsQueryResult>.Succeeded(new GetSubdomainsForUserByGridParamsQueryResult(
            data.Select(x => new GetSubdomainsForUserByGridParamsQueryResult.Item(
                x.DomainId,
                x.DomainName,
                x.Id,
                x.Name,
                x.WhenAssigned,
                x.PolicyType)).ToList(), count));
    }
}