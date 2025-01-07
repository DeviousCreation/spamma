using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class StartAuthViaEmailCommandAuthorizer : AbstractRequestAuthorizer<StartAuthViaEmailCommand>
{
    public override void BuildPolicy(StartAuthViaEmailCommand request)
    {
        this.UseRequirement(new MustNotBeAuthenticatedRequirement());
    }
}