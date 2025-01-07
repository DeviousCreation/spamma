using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandAuthorizers;

public class AlterUserAccessToDomainCommandAuthorizer : AbstractRequestAuthorizer<AllowUserToAccessDomainCommand>
{
    public override void BuildPolicy(AllowUserToAccessDomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "domain-user-management",
        });
    }
}