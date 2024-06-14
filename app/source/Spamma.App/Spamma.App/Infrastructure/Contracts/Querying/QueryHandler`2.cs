using Dapper;
using MaybeMonad;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Infrastructure.Contracts.Database;

namespace Spamma.App.Infrastructure.Contracts.Querying;

public abstract class QueryHandler<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    private readonly IConnectionProvider _connectionProvider;

    protected QueryHandler(IConnectionProvider connectionProvider)
    {
        this._connectionProvider = connectionProvider;
    }

    public async Task<QueryResult<TResult>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var commandMaybe = this.GenerateCommand(request);

        if (commandMaybe.HasNoValue)
        {
            return QueryResult<TResult>.Failed();
        }

        await using var connection = this._connectionProvider.GetConnection();
        await connection.OpenAsync(cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(commandMaybe.Value);
        return this.ProcessData(multi);
    }

    protected abstract QueryResult<TResult> ProcessData(SqlMapper.GridReader gridReader);

    protected abstract Maybe<CommandDefinition> GenerateCommand(TQuery request);
}