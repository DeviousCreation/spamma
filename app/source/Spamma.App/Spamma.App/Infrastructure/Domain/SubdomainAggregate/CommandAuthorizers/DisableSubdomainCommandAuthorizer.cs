using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class DisableSubdomainCommandAuthorizer : AbstractRequestAuthorizer<DisableSubdomainCommand>
{
    public override void BuildPolicy(DisableSubdomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "sub-domain-management",
        });
    }
}