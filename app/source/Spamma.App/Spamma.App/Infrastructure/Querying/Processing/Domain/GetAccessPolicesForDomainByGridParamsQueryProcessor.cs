using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Domain;

internal class GetAccessPolicesForDomainByGridParamsQueryProcessor(
    IDbContextFactory<SpammaDataContext> dbContextFactory, ICurrentUserServiceFactory currentUserServiceFactory) :
    IQueryProcessor<GetAccessPolicesForDomainByGridParamsQuery, GetAccessPolicesForDomainByGridParamsQueryResult>
{
    public async Task<QueryResult<GetAccessPolicesForDomainByGridParamsQueryResult>> Handle(GetAccessPolicesForDomainByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var currentUserService = currentUserServiceFactory.Create();

        var currentUser = await currentUserService.GetCurrentUserAsync();

        if (currentUser.HasNoValue)
        {
            return QueryResult<GetAccessPolicesForDomainByGridParamsQueryResult>.Failed();
        }

        var userId = currentUser.Value.Id;

        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<DomainAccessPolicyQueryEntity>()
            .Where(dap => !dap.IsRevoked && dap.DomainId == request.DomainId && dap.Domain!.DomainAccessPolicies
                .Any(dapInner => dapInner.UserId == userId && !dapInner.IsRevoked))
            .Select(dap => new
            {
                UserId = dap.User!.Id,
                dap.User.EmailAddress,
                dap.User.Name,
                dap.PolicyType,
                dap.WhenAssigned,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<DomainAccessPolicyQueryEntity>()
            .Where(dap => !dap.IsRevoked && dap.DomainId == request.DomainId && dap.Domain!.DomainAccessPolicies
                .Any(dapInner => dapInner.UserId == userId && !dapInner.IsRevoked))
            .Select(dap => new
            {
                dap.User!.Id,
                dap.User.EmailAddress,
                dap.User.Name,
                dap.PolicyType,
                dap.WhenAssigned,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetAccessPolicesForDomainByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetAccessPolicesForDomainByGridParamsQueryResult>.Succeeded(new GetAccessPolicesForDomainByGridParamsQueryResult(
            data.Select(x => new GetAccessPolicesForDomainByGridParamsQueryResult.Item(
                x.UserId,
                x.EmailAddress,
                x.Name,
                x.PolicyType,
                x.WhenAssigned)).ToList(), count));
    }
}