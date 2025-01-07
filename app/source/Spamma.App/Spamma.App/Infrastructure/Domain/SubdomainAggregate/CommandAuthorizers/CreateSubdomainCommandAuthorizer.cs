using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class CreateSubdomainCommandAuthorizer : AbstractRequestAuthorizer<CreateSubdomainCommand>
{
    public override void BuildPolicy(CreateSubdomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "sub-domain-management",
        });
    }
}