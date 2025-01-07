using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;

public record AllowUserToAccessDomainCommand(
    Guid DomainId, Guid UserId, DomainAccessPolicyType DomainAccessPolicyType)
    : ICommand;