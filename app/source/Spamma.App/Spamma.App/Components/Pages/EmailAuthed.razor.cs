using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.CommandResults;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Components.Pages;

public partial class EmailAuthed : ComponentBase
{
    [SupplyParameterFromQuery]
    public string Token { get; set; } = string.Empty;

    [Inject]
    private ICommander Commander { get; set; } = default!;

    [Inject]
    private IHttpContextAccessor HttpContextAccessor { get; set; } = default!;

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var result = await this.Commander.Send<CompleteAuthViaEmailCommand, CompleteAuthViaEmailCommandResult>(new CompleteAuthViaEmailCommand(this.Token));

        if (result.Status == CommandResultStatus.Succeeded)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, result.Data.UserId.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await this.HttpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
    }
}