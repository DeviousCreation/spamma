using Microsoft.AspNetCore.Components;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Components.Pages;

public partial class RequestVerification
{
    [Inject]
    private ICommander Commander { get; set; } = default!;

    [SupplyParameterFromForm]
    private RequestVerificationModel Model { get; set; } = new RequestVerificationModel();

    private async Task OnSubmit()
    {
        var result = await this.Commander.Send(new RequestUserVerificationCommand(this.Model.EmailAddress));
    }

    public class RequestVerificationModel
    {
        public string EmailAddress { get; set; } = string.Empty;
    }
}