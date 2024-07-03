using BlazorBootstrap;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Pages.Domains;

public partial class Listing
{
    private AddDomainModel _addDomainModel = new AddDomainModel();
    PaginationState pagination = new PaginationState { ItemsPerPage = 2 };

    [Inject]
    private ISender Sender { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!;

    GridItemsProvider<ListDomainsQueryResult.DomainItem>? _domainItemProvider;
    private Offcanvas _addDomainOffCanvas;
    private QuickGrid<ListDomainsQueryResult.DomainItem> _grid;

    protected override Task OnInitializedAsync()
    {
        this._domainItemProvider = async req =>
        {
            var query = new ListDomainsQuery(
                req.StartIndex, 
                req.Count ?? pagination.ItemsPerPage);
            var result = await this.Sender.Send(query, req.CancellationToken);
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
        await this.Sender.Send(new CreateDomainCommand(Guid.NewGuid(), this._addDomainModel.DomainName));
        this.ToastService.Notify(new(ToastType.Success, $"Employee details saved successfully."));
        await this._addDomainOffCanvas.HideAsync();
        await this._grid.RefreshDataAsync();
    }

    public class AddDomainModel
    {
        public string DomainName { get; set; } = string.Empty;
    }
}