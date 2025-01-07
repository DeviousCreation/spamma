using BlazorBootstrap;
using Microsoft.AspNetCore.Components.QuickGrid;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.Queries;
using Spamma.App.Client.Infrastructure.Querying.ChaosMonkey.QueryResults;

namespace Spamma.App.Client.Components.Pages.Subdomains;

public partial class View
{
    private PaginationState? _chaosMonkeyPagination;
    private GridItemsProvider<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult.Item>? _chaosMonkeyItemProvider;
    private Offcanvas _addChaosMonkeyOffCanvas;
    private AddChaosMonkeyModel _addChaosMonkeyModel = new();
    private QuickGrid<GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult.Item> _chaosMonkeyGrid;

    private void SetupChaosMonkey()
    {
        this._chaosMonkeyItemProvider = async req =>
        {
            var query = new GetChaosMonkeyAddressesForSubdomainByGridParamsQuery(
                this._subdomainId,
                req.StartIndex,
                req.Count ?? this._chaosMonkeyPagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: result.Data.Items.ToList(),
                totalItemCount: result.Data.TotalItems);
        };
    }

    private async Task OnAddChaosMonkeyClick()
    {
        this._addChaosMonkeyModel = new AddChaosMonkeyModel();
        await this._addChaosMonkeyOffCanvas.ShowAsync();
    }

    private async Task OnAddChaosMonkeySubmit()
    {
        var result = await this.Commander.Send(new AddChaosMonkeyAddressCommand(
            this._subdomainId, Guid.NewGuid(), _addChaosMonkeyModel.EmailAddress, _addChaosMonkeyModel.ChaosMonkeyType));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"User invited successfully."));
            await this._addChaosMonkeyOffCanvas.HideAsync();
            await this._chaosMonkeyGrid.RefreshDataAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to invite user."));
        }
    }

    private class AddChaosMonkeyModel
    {
        public string EmailAddress { get; set; } = string.Empty;
        public ChaosMonkeyType ChaosMonkeyType { get; set; }
    }

    private async Task OnChaosMonkeyAddressDisableClick(GetChaosMonkeyAddressesForSubdomainByGridParamsQueryResult.Item context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to disable this subdomain?",
            message1: "This will stop any emails coming to this subdomain.",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new DeleteChaosMonkeyAddressCommand(this._subdomainId, context.ChaosMonkeyAddressId));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"Subdomain disabled successfully."));
            await this._chaosMonkeyGrid.RefreshDataAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to disable subdomain."));
        }
    }
}