using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.DomainAggregate.CommandAuthorizers;

public class CreateDomainCommandAuthorizer : AbstractRequestAuthorizer<CreateDomainCommand>
{
    public override void BuildPolicy(CreateDomainCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "domain-management",
        });
    }
}