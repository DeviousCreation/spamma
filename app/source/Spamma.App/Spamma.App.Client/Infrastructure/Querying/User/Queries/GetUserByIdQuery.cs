using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.User.Queries;

public record GetUserByIdQuery(Guid Id) : IQuery<GetUserByIdQueryResult>;