using BlazorBootstrap;
using Microsoft.AspNetCore.Components.QuickGrid;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.SubdomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Users;

public partial class View
{
    private QuickGrid<GetSubdomainsForUserByGridParamsQueryResult.Item> _subdomainGrid;
    private GridItemsProvider<GetSubdomainsForUserByGridParamsQueryResult.Item>? _subdomainItemProvider;
    private PaginationState _subdomainPagination = new PaginationState { ItemsPerPage = 2 };
    private ChangeAccessToSubdomainModel _changeAccessToSubdomainModel;
    private Offcanvas _changeAccessToSubdomainOffcanvas;

    private void SetupSubdomainProvider()
    {
        this._subdomainItemProvider = async req =>
        {
            var query = new GetSubdomainsForUserByGridParamsQuery(
                this._userId,
                req.StartIndex,
                req.Count ?? this._subdomainPagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: result.Data.Items.ToList(),
                totalItemCount: result.Data.TotalItems);
        };
    }

    private Task OnChangeAccessClick(GetSubdomainsForUserByGridParamsQueryResult.Item context)
    {
        this._changeAccessToSubdomainModel = new ChangeAccessToSubdomainModel
        {
            DomainName = context.DomainName,
            SubdomainName = context.SubdomainName,
            SubdomainId = context.SubdomainId,
            Type = context.PolicyType,
        };
        return this._changeAccessToSubdomainOffcanvas.ShowAsync();
    }

    private async Task OnRemoveAccessClick(GetSubdomainsForUserByGridParamsQueryResult.Item context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to remove access to this subdomain?",
            message1: "This will remove their access from this subdomain. This will stop them accessing emails sent to this subdomain.",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new RevokeUserToAccessSubdomainCommand(context.SubdomainId, this._userId));

        if (result.Status == CommandResultStatus.Succeeded)
        {
            await this._subdomainGrid.RefreshDataAsync();
            this.ToastService.Notify(new(ToastType.Success, $"User access revoked successfully."));
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to revoke user access."));
        }
    }

    private async Task OnChangeAccessToSubdomainSubmit()
    {
        var result = await this.Commander.Send(new AlterUserAccessToSubdomainCommand(
            this._changeAccessToSubdomainModel.SubdomainId, this._userId, this._changeAccessToSubdomainModel.Type));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"Access policy changed successfully."));
            await this._subdomainGrid.RefreshDataAsync();
            await this._changeAccessToSubdomainOffcanvas.HideAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to change access policy."));
        }
    }

    private class ChangeAccessToSubdomainModel
    {
        public string SubdomainName { get; init; }

        public SubdomainAccessPolicyType Type { get; set; }

        public Guid SubdomainId { get; init; }

        public string DomainName { get; init; }
    }
}