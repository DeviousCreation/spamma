using MaybeMonad;
using Spamma.App.Client.Infrastructure.Contracts.Web;

namespace Spamma.App.Infrastructure.Web;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    
    public Task<Maybe<CurrentUser>> GetCurrentUserAsync()
    {
        if(httpContextAccessor.HttpContext == null)
        {
            return Task.FromResult(Maybe<CurrentUser>.Nothing);
        }

        if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
        {
            return Task.FromResult(Maybe<CurrentUser>.Nothing);
        }

        return Task.FromResult(CurrentUser.FromClaimsPrincipal(httpContextAccessor.HttpContext.User));
    }
}