using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

public record GetUserByIdQueryResult(
    Guid Id, string EmailAddress, DateTime WhenCreated, DateTime? WhenVerified, DateTime? LastLoggedIn, DateTime? WhenDisabled)
    : IQueryResult;