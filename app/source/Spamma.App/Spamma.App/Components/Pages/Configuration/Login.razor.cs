using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Spamma.App.Infrastructure.Constants;
using Spamma.App.Infrastructure.Contracts.Web;

namespace Spamma.App.Components.Pages.Configuration;

public partial class Login(NavigationManager navigationManager, ICodeLoginProvider loginProvider, IHttpContextAccessor httpContextAccessor)
{
    [SupplyParameterFromForm]
    public Model FormModel { get; set; } = new();

    private async Task ValidateCode()
    {
        if (loginProvider.ValidateCode(this.FormModel.Code))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, AuthConstants.ConfigScheme);

            var authProperties = new AuthenticationProperties();

            await httpContextAccessor.HttpContext!.SignInAsync(
                AuthConstants.ConfigScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            navigationManager.NavigateTo("/config/secret-key");
        }
        else
        {
            // Handle invalid code
        }
    }

    public class Model
    {
        public string Code { get; set; } = string.Empty;
    }
}