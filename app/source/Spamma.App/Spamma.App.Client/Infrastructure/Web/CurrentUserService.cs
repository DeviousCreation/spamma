using MaybeMonad;
using Microsoft.AspNetCore.Components.Authorization;
using Spamma.App.Client.Infrastructure.Contracts.Web;

namespace Spamma.App.Client.Infrastructure.Web;

public class CurrentUserService(AuthenticationStateProvider authenticationStateProvider) : ICurrentUserService
{
    public async Task<Maybe<CurrentUser>> GetCurrentUserAsync()
    {
        var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
        return CurrentUser.FromClaimsPrincipal(authState.User);
    }
}