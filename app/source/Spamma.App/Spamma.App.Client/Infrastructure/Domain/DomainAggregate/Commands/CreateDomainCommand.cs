using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;

public record CreateDomainCommand(Guid DomainId, string DomainName) : ICommand;