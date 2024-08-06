using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

public record CreateSubdomainCommand(Guid SubdomainId, string SubdomainName, Guid DomainId) : ICommand;