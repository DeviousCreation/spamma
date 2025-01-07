using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Subdomain;

internal class GetAccessPolicesForSubdomainByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> dbContextFactory)
    : IQueryProcessor<GetAccessPolicesForSubdomainByGridParamsQuery, GetAccessPolicesForSubdomainByGridParamsQueryResult>
{
    public async Task<QueryResult<GetAccessPolicesForSubdomainByGridParamsQueryResult>> Handle(GetAccessPolicesForSubdomainByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var dataFuture = dataContext.Set<SubdomainAccessPolicyQueryEntity>()
            .Where(sap => !sap.IsRevoked && sap.SubdomainId == request.SubdomainId && sap.Subdomain!.SubdomainAccessPolicies
                .Any(sapInner => sapInner.UserId == userId && !sapInner.IsRevoked))
            .Select(sap => new
            {
                UserId = sap.User!.Id,
                sap.User.EmailAddress,
                sap.User.Name,
                sap.PolicyType,
                sap.WhenAssigned,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<SubdomainAccessPolicyQueryEntity>()
            .Where(sap => !sap.IsRevoked && sap.SubdomainId == request.SubdomainId && sap.Subdomain!.SubdomainAccessPolicies
                .Any(sapInner => sapInner.UserId == userId && !sapInner.IsRevoked))
            .Select(sap => new
            {
                UserId = sap.User!.Id,
                sap.User.EmailAddress,
                sap.User.Name,
                sap.PolicyType,
                sap.WhenAssigned,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetAccessPolicesForSubdomainByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetAccessPolicesForSubdomainByGridParamsQueryResult>.Succeeded(new GetAccessPolicesForSubdomainByGridParamsQueryResult(
            data.Select(x => new GetAccessPolicesForSubdomainByGridParamsQueryResult.Item(
                x.UserId,
                x.Name,
                x.EmailAddress,
                x.PolicyType,
                x.WhenAssigned)).ToList(), count));
    }
}