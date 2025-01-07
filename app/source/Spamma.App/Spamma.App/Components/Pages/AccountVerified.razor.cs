using Microsoft.AspNetCore.Components;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Components.Pages;

public partial class AccountVerified
{
    [SupplyParameterFromQuery]
    public string Token { get; set; } = string.Empty;

    [Inject]
    private ICommander Commander { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        var result = await this.Commander.Send(new VerifyUserCommand(this.Token));
    }
}