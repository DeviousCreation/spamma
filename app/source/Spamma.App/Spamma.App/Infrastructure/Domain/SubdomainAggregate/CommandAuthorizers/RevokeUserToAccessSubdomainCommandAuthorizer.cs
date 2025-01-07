using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.SubdomainAggregate.CommandAuthorizers;

public class RevokeUserToAccessSubdomainCommandAuthorizer : AbstractRequestAuthorizer<RevokeUserToAccessSubdomainCommand>
{
    public override void BuildPolicy(RevokeUserToAccessSubdomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "subdomain-user-management",
        });
    }
}