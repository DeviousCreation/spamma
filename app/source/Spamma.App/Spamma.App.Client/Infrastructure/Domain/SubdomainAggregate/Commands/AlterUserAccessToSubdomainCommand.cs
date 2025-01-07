using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;

public record AlterUserAccessToSubdomainCommand(
        Guid SubdomainId, Guid UserId, SubdomainAccessPolicyType SubdomainAccessPolicyType)
        : ICommand;