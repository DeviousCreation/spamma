using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandAuthorizers;

public class DisableDomainCommandAuthorizer : AbstractRequestAuthorizer<DisableDomainCommand>
{
    public override void BuildPolicy(DisableDomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "domain-management",
        });
    }
}