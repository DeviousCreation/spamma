using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class EnableUserCommandAuthorizer : AbstractRequestAuthorizer<EnableUserCommand>
{
    public override void BuildPolicy(EnableUserCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "user-management",
        });
    }
}