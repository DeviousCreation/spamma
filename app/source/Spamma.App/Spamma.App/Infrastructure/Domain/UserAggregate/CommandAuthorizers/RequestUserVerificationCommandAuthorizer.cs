using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class RequestUserVerificationCommandAuthorizer : AbstractRequestAuthorizer<RequestUserVerificationCommand>
{
    public override void BuildPolicy(RequestUserVerificationCommand request)
    {
        this.UseRequirement(new MustNotBeAuthenticatedRequirement());
    }
}