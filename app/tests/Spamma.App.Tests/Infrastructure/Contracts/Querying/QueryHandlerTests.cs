using System.Data;
using Dapper;
using MaybeMonad;
using Moq;
using Moq.Dapper;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Infrastructure.Contracts.Database;
using Spamma.App.Infrastructure.Contracts.Querying;

namespace Spamma.App.Tests.Infrastructure.Contracts.Querying;

public class QueryHandlerTests
{
    // command fails to generate
    [Fact]
    public async Task Handle_WhenCommandFailsToGenerate_ReturnsFailedQueryResult()
    {
        // Arrange
        var connectionProvider = new Mock<IConnectionProvider>();
        var handler = new StubQueryHandler(connectionProvider.Object);
        var query = new StubQuery(false);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        await Verify(new
        {
            result.Status,
        });
    }

    private class StubQueryHandler : QueryHandler<StubQuery, string>
    {
        public StubQueryHandler(IConnectionProvider connectionProvider)
            : base(connectionProvider)
        {
        }

        protected override QueryResult<string> ProcessData(SqlMapper.GridReader gridReader)
        {
            return gridReader.ReadAsync<dynamic>().Result.First().IsSuccess
                ? QueryResult<string>.Succeeded("Success")
                : QueryResult<string>.Failed();
        }

        protected override Maybe<CommandDefinition> GenerateCommand(StubQuery request)
        {
            return request.ShouldGenerate
                ? new CommandDefinition("SELECT 1")
                : Maybe<CommandDefinition>.Nothing;
        }
    }

    private record StubQuery(bool ShouldGenerate) : IQuery<string>;
}