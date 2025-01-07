using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

public record DisableSubdomainCommand(Guid SubdomainId) : ICommand;

public record EnableSubdomainCommand(Guid SubdomainId) : ICommand;