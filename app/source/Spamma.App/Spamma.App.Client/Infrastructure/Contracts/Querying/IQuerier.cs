namespace Spamma.App.Client.Infrastructure.Contracts.Querying;

public interface IQuerier
{
    Task<QueryResult<TQueryResult>> Send<TQueryResult>(
        IQuery<TQueryResult> request, CancellationToken cancellationToken = default)
        where TQueryResult : IQueryResult;
}