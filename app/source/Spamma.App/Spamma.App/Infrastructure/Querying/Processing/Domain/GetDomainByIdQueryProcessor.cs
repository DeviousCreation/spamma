using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;

namespace Spamma.App.Infrastructure.Querying.Processing.Domain;

internal class GetDomainByIdQueryProcessor(
    IDbContextFactory<SpammaDataContext> dbContextFactory, ICurrentUserServiceFactory currentUserServiceFactory)
    : IQueryProcessor<GetDomainByIdQuery, GetDomainByIdQueryResult>
{
    public async Task<QueryResult<GetDomainByIdQueryResult>> Handle(
        GetDomainByIdQuery request, CancellationToken cancellationToken)
    {
        var currentUserService = currentUserServiceFactory.Create();

        var currentUser = await currentUserService.GetCurrentUserAsync();

        if (currentUser.HasNoValue)
        {
            return QueryResult<GetDomainByIdQueryResult>.Failed();
        }

        var userId = currentUser.Value.Id;
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var data = await dataContext.Set<DomainQueryEntity>()
            .Where(domain => domain.Id == request.Id && domain.DomainAccessPolicies
                .Any(dapInner => dapInner.UserId == userId && !dapInner.IsRevoked))
            .Select(x => new
            {
                x.Id,
                x.Name,
            })
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        return data == null ?
            QueryResult<GetDomainByIdQueryResult>.Failed() :
            QueryResult<GetDomainByIdQueryResult>.Succeeded(new GetDomainByIdQueryResult(data.Id, data.Name));
    }
}