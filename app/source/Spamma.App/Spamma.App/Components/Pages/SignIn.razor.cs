using Microsoft.AspNetCore.Components;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Components.Pages;

public partial class SignIn : ComponentBase
{
    [Inject]
    private ICommander Commander { get; set; } = default!;

    [SupplyParameterFromForm]
    private SignInModel Model { get; set; } = new SignInModel();

    private async Task Login()
    {
        var result = await this.Commander.Send(new StartAuthViaEmailCommand(this.Model.EmailAddress));
    }

    public class SignInModel
    {
        public string EmailAddress { get; set; } = string.Empty;
    }
}