using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying;

public abstract record ListQueryResultOf<TItem>(IReadOnlyList<TItem> Items, int TotalItems) : IQueryResult
    where TItem : notnull;