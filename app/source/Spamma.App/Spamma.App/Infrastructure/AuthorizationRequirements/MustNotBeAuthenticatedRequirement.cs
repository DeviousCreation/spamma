using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Web;

namespace Spamma.App.Infrastructure.AuthorizationRequirements;

internal class MustNotBeAuthenticatedRequirement : IAuthorizationRequirement
{
    private class MustNotBeAuthenticatedRequirementHandler(ICurrentUserServiceFactory currentUserServiceFactory) : IAuthorizationHandler<MustNotBeAuthenticatedRequirement>
    {
        public async Task<AuthorizationResult> Handle(MustNotBeAuthenticatedRequirement requirement, CancellationToken cancellationToken = default)
        {
            var currentUserService = currentUserServiceFactory.Create();
            var user = await currentUserService.GetCurrentUserAsync();
            return user.HasNoValue ? AuthorizationResult.Succeed() : AuthorizationResult.Fail();
        }
    }
}