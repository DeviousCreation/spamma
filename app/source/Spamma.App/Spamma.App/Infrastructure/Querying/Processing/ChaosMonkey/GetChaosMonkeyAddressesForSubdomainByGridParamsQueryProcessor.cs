using Microsoft.EntityFrameworkCore;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.Queries;
using Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.QueryResults;
using Spamma.App.Infrastructure.Database;
using Spamma.App.Infrastructure.Querying.Entities;
using Z.EntityFramework.Plus;

namespace Spamma.App.Infrastructure.Querying.Processing.ChaosMonkey;

internal class GetChaosMonkeyAddressesForSubdomainByGridParamsQueryProcessor(IDbContextFactory<SpammaDataContext> dbContextFactory)
    : IQueryProcessor<GetChaosMonkeyAddressesForSubdomainByGridParamsQuery, GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult>
{
    public async Task<QueryResult<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult>> Handle(
        GetChaosMonkeyAddressesForSubdomainByGridParamsQuery request, CancellationToken cancellationToken)
    {
        var userId = Guid.NewGuid();
        await using var dataContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var dataFuture = dataContext.Set<ChaosMonkeyAddressQueryEntity>()
            .Where(cma => cma.Subdomain!.WhenDisabled == null
                          && cma.SubdomainId == request.SubdomainId
                          && cma.Subdomain.SubdomainAccessPolicies.Any(sap => sap.UserId == userId && !sap.IsRevoked)
                          && cma.Subdomain!.Domain!.WhenDisabled == null)
            .Select(cma => new
            {
                ChoasMonkeyAddressId = cma.Id,
                cma.EmailAddress,
                ChaosMonkeyType = cma.Type,
                cma.SubdomainId,
            })
            .Skip(request.Skip)
            .Take(request.Take)
            .Future();

        var countFuture = dataContext.Set<ChaosMonkeyAddressQueryEntity>()
            .Where(cma => cma.Subdomain!.WhenDisabled == null
                          && cma.Subdomain.SubdomainAccessPolicies.Any(sap => sap.UserId == userId && !sap.IsRevoked)
                          && cma.Subdomain!.Domain!.WhenDisabled == null)
            .Select(cma => new
            {
                cma.Id,
                cma.EmailAddress,
                cma.Type,
                cma.SubdomainId,
            })
            .DeferredCount();

        var data = await dataFuture.ToListAsync(cancellationToken);

        if (data == null)
        {
            return QueryResult<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult>.Failed();
        }

        var count = await countFuture.ExecuteAsync(cancellationToken);

        return QueryResult<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult>.Succeeded(new GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult(
            data.Select(x => new GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult.Item(
                x.ChoasMonkeyAddressId,
                x.EmailAddress,
                x.ChaosMonkeyType)).ToList(), count));
    }
}