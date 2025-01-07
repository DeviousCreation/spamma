using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.User.Queries;
using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Querying.Processing.User;

internal class GetUserByIdQueryProcessor(IDbContextFactory<SpammaDataContext> contextFactory) :
    IQueryProcessor<GetUserByIdQuery, GetUserByIdQueryResult>
{
    public async Task<QueryResult<GetUserByIdQueryResult>> Handle(
        GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await using var dataContext = await contextFactory.CreateDbContextAsync(cancellationToken);
        var data = await dataContext.Set<UserQueryEntity>()
            .Where(x => x.Id == request.Id)
            .Select(x => new
            {
                x.Id,
                x.EmailAddress,
                x.WhenCreated,
                x.WhenVerified,
                x.LastLoggedIn,
                x.WhenDisabled,
            }).SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (data == null)
        {
            return QueryResult<GetUserByIdQueryResult>.Failed();
        }

        return QueryResult<GetUserByIdQueryResult>.Succeeded(new GetUserByIdQueryResult(
            data.Id, data.EmailAddress, data.WhenCreated, data.WhenVerified, data.LastLoggedIn, data.WhenDisabled));
    }
}