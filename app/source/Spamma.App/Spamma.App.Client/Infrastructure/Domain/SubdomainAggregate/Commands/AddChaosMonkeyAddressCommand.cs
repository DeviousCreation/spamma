using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

public record AddChaosMonkeyAddressCommand(Guid SubdomainId, Guid AddressId, string Address, ChaosMonkeyType Type) : ICommand;