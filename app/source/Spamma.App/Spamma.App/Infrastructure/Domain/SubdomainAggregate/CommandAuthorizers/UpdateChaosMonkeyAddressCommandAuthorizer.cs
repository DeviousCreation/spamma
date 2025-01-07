using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class UpdateChaosMonkeyAddressCommandAuthorizer : AbstractRequestAuthorizer<UpdateChaosMonkeyAddressCommand>
{
    public override void BuildPolicy(UpdateChaosMonkeyAddressCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "chaos-monkey-address-management",
        });
    }
}