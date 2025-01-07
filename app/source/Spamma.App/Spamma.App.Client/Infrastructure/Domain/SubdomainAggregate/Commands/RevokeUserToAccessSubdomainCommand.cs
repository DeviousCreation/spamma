using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

public record RevokeUserToAccessSubdomainCommand(Guid SubdomainId, Guid UserId) : ICommand;