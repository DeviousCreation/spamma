using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class VerifyUserCommandAuthorizer : AbstractRequestAuthorizer<VerifyUserCommand>
{
    public override void BuildPolicy(VerifyUserCommand request)
    {
        this.UseRequirement(new MustNotBeAuthenticatedRequirement());
    }
}