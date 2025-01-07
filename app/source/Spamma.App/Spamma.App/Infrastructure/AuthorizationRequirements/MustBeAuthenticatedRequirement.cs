using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Web;

namespace Spamma.App.Infrastructure.AuthorizationRequirements;

internal class MustBeAuthenticatedRequirement : IAuthorizationRequirement
{
    private class MustBeAuthenticatedRequirementHandler(ICurrentUserServiceFactory currentUserServiceFactory) : IAuthorizationHandler<MustBeAuthenticatedRequirement>
    {
        public async Task<AuthorizationResult> Handle(MustBeAuthenticatedRequirement requirement, CancellationToken cancellationToken = default)
        {
            var currentUserService = currentUserServiceFactory.Create();
            var user = await currentUserService.GetCurrentUserAsync();
            return user.HasValue ? AuthorizationResult.Succeed() : AuthorizationResult.Fail();
        }
    }
}