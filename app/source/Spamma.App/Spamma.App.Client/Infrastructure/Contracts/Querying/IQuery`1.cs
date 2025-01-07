using MediatR;

namespace Spamma.App.Client.Infrastructure.Contracts.Querying;

public interface IQuery<T> : IRequest<QueryResult<T>>
where T : IQueryResult;