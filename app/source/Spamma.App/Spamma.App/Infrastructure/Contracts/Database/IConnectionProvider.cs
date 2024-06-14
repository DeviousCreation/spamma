using Npgsql;

namespace Spamma.App.Infrastructure.Contracts.Database;

public interface IConnectionProvider
{
    NpgsqlConnection GetConnection();
}