using BlazorBootstrap;
using Microsoft.AspNetCore.Components.QuickGrid;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Users;

public partial class View
{
    private QuickGrid<GetDomainsForUserByGridParamsQueryResult.DomainItem> _domainGrid;
    private GridItemsProvider<GetDomainsForUserByGridParamsQueryResult.DomainItem>? _domainItemProvider;
    private PaginationState _domainPagination = new PaginationState { ItemsPerPage = 2 };
    private Offcanvas _changeAccessToDomainOffcanvas;
    private ChangeAccessToDomainModel _changeAccessToDomainModel;

    private void SetupDomainProvider()
    {
        this._domainItemProvider = async req =>
        {
            var query = new GetDomainsForUserByGridParamsQuery(
                this._userId,
                req.StartIndex,
                req.Count ?? this._domainPagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: result.Data.Items.ToList(),
                totalItemCount: result.Data.TotalItems);
        };
    }

    private Task OnChangeAccessClick(GetDomainsForUserByGridParamsQueryResult.DomainItem context)
    {
        this._changeAccessToDomainModel = new ChangeAccessToDomainModel
        {
            DomainName = context.Name,
            DomainId = context.DomainId,
            Type = context.PolicyType,
        };
        return this._changeAccessToDomainOffcanvas.ShowAsync();
    }

    private async Task OnRemoveAccessClick(GetDomainsForUserByGridParamsQueryResult.DomainItem context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to remove access to this domain?",
            message1: "This will remove their access from this domain. This doesn't affect their access to any subdomains",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new RevokeUserToAccessDomainCommand(context.DomainId, this._userId));

        if (result.Status == CommandResultStatus.Succeeded)
        {
            await this._domainGrid.RefreshDataAsync();
            this.ToastService.Notify(new(ToastType.Success, $"User access revoked successfully."));
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to revoke user access."));
        }
    }

    private async Task OnChangeAccessToDomainSubmit()
    {
        var result = await this.Commander.Send(new AlterUserAccessToDomainCommand(
            this._changeAccessToDomainModel.DomainId, this._userId, this._changeAccessToDomainModel.Type));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"Access policy changed successfully."));
            await this._domainGrid.RefreshDataAsync();
            await this._changeAccessToDomainOffcanvas.HideAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to change access policy."));
        }
    }

    private class ChangeAccessToDomainModel
    {
        public string DomainName { get; init; }

        public DomainAccessPolicyType Type { get; set; }

        public Guid DomainId { get; init; }
    }
}

