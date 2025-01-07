using MediatR.Behaviors.Authorization;
using Spamma.App.Client.Infrastructure.Web;

namespace Spamma.App.Infrastructure.AuthorizationRequirements;

internal class MustBeInRoleRequirement : IAuthorizationRequirement
{
    internal string Role { get; init; }

    private class MustBeInRoleRequirementHandler(ICurrentUserServiceFactory currentUserServiceFactory) : IAuthorizationHandler<MustBeInRoleRequirement>
    {
        public async Task<AuthorizationResult> Handle(MustBeInRoleRequirement requirement, CancellationToken cancellationToken = default)
        {
            var currentUserService = currentUserServiceFactory.Create();
            var user = await currentUserService.GetCurrentUserAsync();
            if (user.HasNoValue)
            {
                return AuthorizationResult.Fail();
            }

            return user.Value.Role.Contains(requirement.Role) ? AuthorizationResult.Succeed() : AuthorizationResult.Fail();
        }
    }
}