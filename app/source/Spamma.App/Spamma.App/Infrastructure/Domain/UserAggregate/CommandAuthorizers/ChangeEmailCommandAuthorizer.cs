using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class ChangeEmailCommandAuthorizer : AbstractRequestAuthorizer<ChangeEmailCommand>
{
    public override void BuildPolicy(ChangeEmailCommand request)
    {
        this.UseRequirement(new MustBeAuthenticatedRequirement());
    }
}