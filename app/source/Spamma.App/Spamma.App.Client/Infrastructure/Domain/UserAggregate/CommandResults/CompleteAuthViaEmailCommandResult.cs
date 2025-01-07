using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.UserAggregate.CommandResults;

public record CompleteAuthViaEmailCommandResult(Guid UserId, string Email, string Name) : ICommandResult;