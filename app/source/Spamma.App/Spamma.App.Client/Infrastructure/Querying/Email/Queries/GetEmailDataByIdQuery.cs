using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Email.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Email.Queries;

public record GetEmailDataByIdQuery(Guid EmailId) : IQuery<GetEmailDataByIdQueryResult>;