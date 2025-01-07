using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.Email.QueryResults;

public record GetEmailDataByIdQueryResult(string FileData) : IQueryResult;