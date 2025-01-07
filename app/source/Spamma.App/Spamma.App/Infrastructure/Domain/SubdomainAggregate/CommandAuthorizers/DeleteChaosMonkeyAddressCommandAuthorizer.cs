using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class DeleteChaosMonkeyAddressCommandAuthorizer : AbstractRequestAuthorizer<DeleteChaosMonkeyAddressCommand>
{
    public override void BuildPolicy(DeleteChaosMonkeyAddressCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "chaos-monkey-address-management",
        });
    }
}