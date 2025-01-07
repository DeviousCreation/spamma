using Microsoft.AspNetCore.Components;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Components.Pages;

public partial class Register
{
    [Inject]
    private ICommander Commander { get; set; } = default!;

    [SupplyParameterFromForm]
    private RegisterModel Model { get; set; } = new RegisterModel();

    private async Task OnSubmit()
    {
        var result = await this.Commander.Send(new RegisterUserCommand(this.Model.Name, this.Model.EmailAddress));
    }

    public class RegisterModel
    {
        public string EmailAddress { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}