using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying;

public abstract record GridParams<TQueryResult>(int Skip, int Take) : IQuery<TQueryResult>
    where TQueryResult : IQueryResult;