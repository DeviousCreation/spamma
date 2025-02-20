using MediatR;
using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Contracts.Querying;

public interface IQueryProcessor<in TQuery, TResult>
    : IRequestHandler<TQuery, QueryResult<TResult>>
    where TQuery : IQuery<TResult>
    where TResult : IQueryResult;