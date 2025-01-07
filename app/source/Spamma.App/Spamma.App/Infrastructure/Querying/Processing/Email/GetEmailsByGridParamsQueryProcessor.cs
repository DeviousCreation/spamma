using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Email.Queries;
using Spamma.App.Client.Infrastructure.Querying.Email.QueryResults;
using Spamma.App.Client.Infrastructure.Web;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.Email;

internal class GetEmailsByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> contextFactory, ICurrentUserServiceFactory currentUserServiceFactory) :
    IQueryProcessor<GetEmailsByGridParamsQuery, GetEmailsByGridParamsQueryResult>
{
    public async Task<QueryResult<GetEmailsByGridParamsQueryResult>> Handle(GetEmailsByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var currentUserService = currentUserServiceFactory.Create();

        var currentUser = await currentUserService.GetCurrentUserAsync();

        if (currentUser.HasNoValue)
        {
            return QueryResult<GetEmailsByGridParamsQueryResult>.Failed();
        }

        var userId = currentUser.Value.Id;

        await using var dataContext = await contextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(subdomain => subdomain.SubdomainAccessPolicies
                .Any(sapInner => sapInner.UserId == userId && !sapInner.IsRevoked))
            .SelectMany(x => x.Emails.Select(y => new
            {
                y.Id,
                y.EmailAddress,
                y.Subject,
                y.WhenSent,
            }))
            .OrderBy(x => x.WhenSent)
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<SubdomainQueryEntity>()
            .Where(subdomain => subdomain.SubdomainAccessPolicies
                .Any(sapInner => sapInner.UserId == userId && !sapInner.IsRevoked))
            .SelectMany(x => x.Emails.Select(y => new
            {
                y.Id,
                y.EmailAddress,
                y.Subject,
                y.WhenSent,
            }))
            .OrderBy(x => x.WhenSent)
            .DeferredCount();
        
        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetEmailsByGridParamsQueryResult>.Failed();
        }
        
        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetEmailsByGridParamsQueryResult>.Succeeded(new GetEmailsByGridParamsQueryResult(
            data.Select(x => new GetEmailsByGridParamsQueryResult.Item(
                x.Id, x.EmailAddress, x.Subject, x.WhenSent)).ToList(), count));
    }
}