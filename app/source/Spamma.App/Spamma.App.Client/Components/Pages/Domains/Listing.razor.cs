using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Domains;

public partial class Listing
{
    private AddDomainModel _addDomainModel = new AddDomainModel();
    PaginationState pagination = new PaginationState { ItemsPerPage = 2 };

    [Inject]
    private ICommander Commander { get; set; } = default!;

    [Inject]
    private IQuerier Querier { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IClock Clock { get; set; } = default!;

    GridItemsProvider<GetDomainsByGridParamsQueryResult.Item>? _domainItemProvider;
    private Offcanvas _addDomainOffCanvas;
    private QuickGrid<GetDomainsByGridParamsQueryResult.Item> _grid;
    private ConfirmDialog _confirmDialog;

    protected override Task OnInitializedAsync()
    {
        this._domainItemProvider = async req =>
        {
            var query = new GetDomainsByGridParamsQuery(
                req.StartIndex,
                req.Count ?? this.pagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: result.Data.Items.ToList(),
                totalItemCount: result.Data.TotalItems);
        };

        return Task.CompletedTask;
    }

    private async Task OnAddClick()
    {
        this._addDomainModel = new AddDomainModel();
        await this._addDomainOffCanvas.ShowAsync();
    }

    private async Task OnAddSubmit()
    {
        await this.Commander.Send(new CreateDomainCommand(this._addDomainModel.DomainName));
        this.ToastService.Notify(new(ToastType.Success, $"Employee details saved successfully."));
        await this._addDomainOffCanvas.HideAsync();
        await this._grid.RefreshDataAsync();
    }

    public class AddDomainModel
    {
        public string DomainName { get; set; } = string.Empty;
    }

    private void OnViewClick(GetDomainsByGridParamsQueryResult.Item context)
    {
        this.NavigationManager.NavigateTo($"/settings/domains/{context.DomainId}");
    }

    private async Task OnDisableClick(GetDomainsByGridParamsQueryResult.Item context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to remove access to this domain?",
            message1: "This will remove their access from this domain. This doesn't affect their access to any subdomains",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new DisableDomainCommand(context.DomainId));

        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"User access revoked successfully."));
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to revoke user access."));
        }
    }
}