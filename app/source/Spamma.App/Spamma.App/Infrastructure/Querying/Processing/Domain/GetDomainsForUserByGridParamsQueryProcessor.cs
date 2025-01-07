using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Domain;

internal class GetDomainsForUserByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> contextFactory) :
    IQueryProcessor<GetDomainsForUserByGridParamsQuery, GetDomainsForUserByGridParamsQueryResult>
{
    public async Task<QueryResult<GetDomainsForUserByGridParamsQueryResult>> Handle(
        GetDomainsForUserByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await contextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<DomainQueryEntity>()
            .Where(domain => domain.DomainAccessPolicies
                .Any(dapInner => 
                    (dapInner.UserId == userId && !dapInner.IsRevoked) &&
                    (dapInner.User.Id == request.UserId && !dapInner.IsRevoked)))
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.DomainAccessPolicies.Single(dap => dap.UserId == request.UserId).PolicyType,
                d.DomainAccessPolicies.Single(dap => dap.UserId == request.UserId).WhenAssigned,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<UserQueryEntity>()
            .Where(domain => domain.DomainAccessPolicies
                .Any(dapInner => 
                    (dapInner.UserId == userId && !dapInner.IsRevoked) &&
                    (dapInner.User.Id == request.UserId && !dapInner.IsRevoked)))
            .Select(d => new
            {
                d.Id,
                d.Name,
                d.DomainAccessPolicies.Single(dap => dap.UserId == request.UserId).PolicyType,
                d.DomainAccessPolicies.Single(dap => dap.UserId == request.UserId).WhenAssigned,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetDomainsForUserByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetDomainsForUserByGridParamsQueryResult>.Succeeded(new GetDomainsForUserByGridParamsQueryResult(
            data.Select(x => new GetDomainsForUserByGridParamsQueryResult.DomainItem(
                x.Id,
                x.Name,
                x.WhenAssigned,
                x.PolicyType)).ToList(), count));
    }
}