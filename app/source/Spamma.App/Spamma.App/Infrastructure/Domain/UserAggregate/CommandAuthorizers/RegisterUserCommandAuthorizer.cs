using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class RegisterUserCommandAuthorizer : AbstractRequestAuthorizer<RegisterUserCommand>
{
    public override void BuildPolicy(RegisterUserCommand request)
    {
        this.UseRequirement(new MustNotBeAuthenticatedRequirement());
    }
}