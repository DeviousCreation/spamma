using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

public record DeleteChaosMonkeyAddressCommand(Guid SubdomainId, Guid AddressId) : ICommand;