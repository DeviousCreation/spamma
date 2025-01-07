using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.User.Queries;
using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.User;

internal class GetUsersByParamsQueryProcessor(IDbContextFactory<SpammaDataContext> contextFactory) :
    IQueryProcessor<GetUsersByGridParamsQuery, GetUsersByGridParamsQueryResult>
{
    public async Task<QueryResult<GetUsersByGridParamsQueryResult>> Handle(GetUsersByGridParamsQuery request, CancellationToken cancellationToken)
    {
        await using var dataContext = await contextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<UserQueryEntity>()
            .Skip(request.Skip)
            .Take(request.Take)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.EmailAddress,
                x.WhenCreated,
                x.WhenVerified,
                x.WhenDisabled,
                x.LastLoggedIn,
                x.DomainCount,
                x.SubdomainCount,
            }).Future();

        var countFuture = dataContext.Set<UserQueryEntity>()
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetUsersByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetUsersByGridParamsQueryResult>.Succeeded(new GetUsersByGridParamsQueryResult(
            data.Select(x => new GetUsersByGridParamsQueryResult.Item(
                x.Id,
                x.Name,
                x.EmailAddress,
                x.WhenCreated,
                x.WhenVerified,
                x.LastLoggedIn,
                x.WhenDisabled,
                x.DomainCount,
                x.SubdomainCount)).ToList(), count));
    }
}