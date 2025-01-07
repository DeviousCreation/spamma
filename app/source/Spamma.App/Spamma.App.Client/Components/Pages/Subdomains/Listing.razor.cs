using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Subdomains;

public partial class Listing : ComponentBase
{
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

    [Parameter]
    public string? DomainId { get; set; } = default!;

    private GridItemsProvider<GetSubdomainsByGridParamsQueryResult.Item>? _subdomainItemProvider;
    private QuickGrid<GetSubdomainsByGridParamsQueryResult.Item> _grid;
    private ConfirmDialog _confirmDialog;
    private AddSubdomainModel _addSubdomainModel = new AddSubdomainModel();
    private PaginationState pagination = new PaginationState { ItemsPerPage = 2 };
    private bool _isAddDomainLevel;
    private Guid _domainId;
    private GetDomainByIdQueryResult _domain;
    private IReadOnlyList<GetDomainsByGridParamsQueryResult.Item>? _domains;
    private Offcanvas _addSubdomainOffCanvas;

    protected override async Task OnInitializedAsync()
    {
        if (!string.IsNullOrWhiteSpace(this.DomainId))
        {
            this._isAddDomainLevel = true;
            this._domainId = Guid.Parse(this.DomainId);
            var query = new GetDomainByIdQuery(this._domainId);
            var result = await this.Querier.Send(query);
            if (result.Status == QueryResultStatus.Succeeded)
            {
                this._domain = result.Data;
            }
        }

        this._subdomainItemProvider = async req =>
        {
            var query = new GetSubdomainsByGridParamsQuery(
                req.StartIndex,
                req.Count ?? this.pagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: result.Data.Items.ToList(),
                totalItemCount: result.Data.TotalItems);
        };
    }

    private async Task OnAddClick()
    {
        if (this._isAddDomainLevel && this._domains == null)
        {
            var domainResult = await this.Querier.Send(new GetDomainsByGridParamsQuery(0, int.MaxValue));
            if (domainResult.Status != QueryResultStatus.Succeeded)
            {
                return;
            }

            this._domains = domainResult.Data.Items;
        }

        this._addSubdomainModel = new AddSubdomainModel();
        await this._addSubdomainOffCanvas.ShowAsync();
    }

    private async Task OnAddSubmit()
    {
        await this.Commander.Send(new CreateSubdomainCommand(this._addSubdomainModel.SubdomainName, this._addSubdomainModel.DomainId));
        this.ToastService.Notify(new(ToastType.Success, $"Employee details saved successfully."));
        await this._addSubdomainOffCanvas.HideAsync();
        await this._grid.RefreshDataAsync();
    }

    private void OnViewClick(GetSubdomainsByGridParamsQueryResult.Item context)
    {
        if (string.IsNullOrWhiteSpace(this.DomainId))
        {
            this.NavigationManager.NavigateTo($"/subdomains/{context.SubdomainId}");
        }
        else
        {
            this.NavigationManager.NavigateTo($"/domains/{this.DomainId}/subdomains/{context.SubdomainId}");    
        }
    }

    private async Task OnDisableClick(GetSubdomainsByGridParamsQueryResult.Item context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to remove access to this domain?",
            message1: "This will remove their access from this domain. This doesn't affect their access to any subdomains",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new DisableSubdomainCommand(context.SubdomainId));

        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"User access revoked successfully."));
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to revoke user access."));
        }
    }

    private class AddSubdomainModel
    {
        public string SubdomainName { get; set; } = string.Empty;

        public Guid DomainId { get; set; }
    }
}