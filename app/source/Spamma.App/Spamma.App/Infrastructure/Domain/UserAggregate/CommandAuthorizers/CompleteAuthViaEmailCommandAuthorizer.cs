using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Infrastructure.AuthorizationRequirements;

namespace Spamma.App.Infrastructure.Domain.UserAggregate.CommandAuthorizers;

public class CompleteAuthViaEmailCommandAuthorizer : AbstractRequestAuthorizer<CompleteAuthViaEmailCommand>
{
    public override void BuildPolicy(CompleteAuthViaEmailCommand request)
    {
        this.UseRequirement(new MustNotBeAuthenticatedRequirement());
    }
}