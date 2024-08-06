using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

   public record GetDomainByNameQueryResult(Guid Id, string Name)
        : IQueryResult;