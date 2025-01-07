namespace Spamma.App.Client.Infrastructure.Querying.Email.QueryResults;

public record GetEmailsByGridParamsQueryResult(IReadOnlyList<GetEmailsByGridParamsQueryResult.Item> Items, int TotalItems)
    : ListQueryResultOf<GetEmailsByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(Guid Id, string To, string Subject, DateTimeOffset WhenReceived);
}