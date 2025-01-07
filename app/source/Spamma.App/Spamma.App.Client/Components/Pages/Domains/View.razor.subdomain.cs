using BlazorBootstrap;
using Microsoft.AspNetCore.Components.QuickGrid;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Domains;

public partial class View
{
    private PaginationState? _subdomainPagination;
    private GridItemsProvider<GetSubdomainsForDomainByGridParamsQueryResult.Item>? _subdomainItemProvider;
    private Offcanvas _addSubdomainOffCanvas;
    private AddSubdomainModel _addSubdomainModel = new();
    private QuickGrid<GetSubdomainsForDomainByGridParamsQueryResult.Item> _subdomainGrid;

    private void SetupSubdomain()
    {
        this._subdomainItemProvider = async req =>
        {
            var query = new GetSubdomainsForDomainByGridParamsQuery(
                this._domainId,
                req.StartIndex,
                req.Count ?? this._subdomainPagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: Enumerable.ToList<GetSubdomainsForDomainByGridParamsQueryResult.Item>(result.Data.Items),
                totalItemCount: result.Data.TotalItems);
        };
    }

    private async Task OnAddSubdomainClick()
    {
        this._addSubdomainModel = new AddSubdomainModel();
        await this._addSubdomainOffCanvas.ShowAsync();
    }

    private async Task OnAddSubdomainSubmit()
    {
        var result = await this.Commander.Send(new CreateSubdomainCommand(this._addSubdomainModel.Name, this._domainId));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"User invited successfully."));
            await this._addSubdomainOffCanvas.HideAsync();
            await this._subdomainGrid.RefreshDataAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to invite user."));
        }
    }

    private class AddSubdomainModel
    {
        public string Name { get; set; } = string.Empty;
    }

    private void OnSubdomainViewClick(GetSubdomainsForDomainByGridParamsQueryResult.Item context)
    {
        this.NavigationManager.NavigateTo($"/subdomains/{context.SubdomainId}");
    }

    private async Task OnSubdomainDisableClick(GetSubdomainsForDomainByGridParamsQueryResult.Item context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to disable this subdomain?",
            message1: "This will stop any emails coming to this subdomain.",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new DisableSubdomainCommand(context.SubdomainId));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"Subdomain disabled successfully."));
            await this._subdomainGrid.RefreshDataAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to disable subdomain."));
        }
    }
}