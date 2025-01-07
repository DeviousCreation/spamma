using System.Net.Http.Json;
using System.Text.Json;

namespace Spamma.App.Client.Infrastructure.Contracts.Querying;

public class GenericQueryProcessor<TQuery, TResult>(IHttpClientFactory httpClientFactory)
    : IQueryProcessor<TQuery, TResult>
    where TQuery : IQuery<TResult>
    where TResult : IQueryResult
{
    public async Task<QueryResult<TResult>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var client = httpClientFactory.CreateClient("spamma");

        var response = await client.PostAsJsonAsync(
            $"api/query/{typeof(TQuery).FullName}",
            request,
            cancellationToken: cancellationToken);

        var content = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return QueryResult<TResult>.Failed();
        }

        var resp = JsonSerializer.Deserialize<QueryResult<TResult>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        return resp ?? QueryResult<TResult>.Failed();
    }
}