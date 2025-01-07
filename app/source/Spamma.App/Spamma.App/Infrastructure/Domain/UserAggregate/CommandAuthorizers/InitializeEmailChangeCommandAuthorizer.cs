using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class InitializeEmailChangeCommandAuthorizer : AbstractRequestAuthorizer<InitializeEmailChangeCommand>
{
    public override void BuildPolicy(InitializeEmailChangeCommand request)
    {
        this.UseRequirement(new MustBeAuthenticatedRequirement());
    }
}