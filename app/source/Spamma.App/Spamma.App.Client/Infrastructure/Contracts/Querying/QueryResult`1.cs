using System.Text.Json.Serialization;
using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Querying;

public class QueryResult<T>
{
    private readonly T? _data;

    private QueryResult()
    {
        this.Status = QueryResultStatus.Failed;
    }

    private QueryResult(T data)
    {
        this._data = data;
        this.Status = QueryResultStatus.Succeeded;
    }

    [JsonConstructor]
    private QueryResult(T? data, QueryResultStatus status)
    {
        this._data = data;
        this.Status = status;
    }

    public QueryResultStatus Status { get; }

    public T Data
    {
        get
        {
            if (this.Status != QueryResultStatus.Succeeded)
            {
                throw new System.InvalidOperationException("Data is only available when the status is Succeeded");
            }

            return this._data!;
        }
    }

    public static QueryResult<T> Failed()
    {
        return new QueryResult<T>();
    }

    public static QueryResult<T> Succeeded(T data)
    {
        return new QueryResult<T>(data);
    }
}