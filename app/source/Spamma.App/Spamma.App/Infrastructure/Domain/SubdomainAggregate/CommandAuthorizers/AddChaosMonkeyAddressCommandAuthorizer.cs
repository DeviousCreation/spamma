using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class AddChaosMonkeyAddressCommandAuthorizer : AbstractRequestAuthorizer<AddChaosMonkeyAddressCommand>
{
    public override void BuildPolicy(AddChaosMonkeyAddressCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "chaos-monkey-address-management",
        });
    }
}