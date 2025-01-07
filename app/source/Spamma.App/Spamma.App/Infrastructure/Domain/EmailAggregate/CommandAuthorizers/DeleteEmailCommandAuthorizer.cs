using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.EmailAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.EmailAggregate.CommandAuthorizers;

public class DeleteEmailCommandAuthorizer : AbstractRequestAuthorizer<DeleteEmailCommand>
{
    public override void BuildPolicy(DeleteEmailCommand request)
    {
        this.UseRequirement(new MustBeInRoleRequirement
        {
            Role = "email-management",
        });
    }
}