using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Domain;

internal class GetDomainsByGridParamsQueryProcessor(
    IDbContextFactory<SpammaDataContext> dbContextFactory, ICurrentUserServiceFactory currentUserServiceFactory) :
    IQueryProcessor<GetDomainsByGridParamsQuery, GetDomainsByGridParamsQueryResult>
{
    public async Task<QueryResult<GetDomainsByGridParamsQueryResult>> Handle(
        GetDomainsByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var currentUserService = currentUserServiceFactory.Create();

        var currentUser = await currentUserService.GetCurrentUserAsync();

        if (currentUser.HasNoValue)
        {
            return QueryResult<GetDomainsByGridParamsQueryResult>.Failed();
        }

        var userId = currentUser.Value.Id;
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<DomainQueryEntity>()
            .Where(domain => domain.DomainAccessPolicies
                .Any(dapInner => dapInner.UserId == userId && !dapInner.IsRevoked))
            .Select(d => new
            {
                d.Id,
                d.Name,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<DomainQueryEntity>()
            .Where(domain => domain.DomainAccessPolicies
                .Any(dapInner => dapInner.UserId == userId && !dapInner.IsRevoked))
            .Select(d => new
            {
                d.Id,
                d.Name,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetDomainsByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetDomainsByGridParamsQueryResult>.Succeeded(new GetDomainsByGridParamsQueryResult(
            data.Select(x => new GetDomainsByGridParamsQueryResult.Item(
                x.Id,
                x.Name)).ToList(), count));
    }
}