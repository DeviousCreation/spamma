using BlazorBootstrap;
using Microsoft.AspNetCore.Components.QuickGrid;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Domain.DomainAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.User.Queries;
using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

namespace Spamma.App.Client.Components.Pages.Domains;

public partial class View
{
    private AddUserModel _addUserModel = new();
    private Offcanvas _addUserOffcanvas;
    private ChangeAccessModel _changeAccessModel = new();
    private Offcanvas _changeAccessOffcanvas;
    private PaginationState? _domainAccessPagination;
    private GridItemsProvider<GetAccessPolicesForDomainByGridParamsQueryResult.Item>? _domainAccessItemProvider;
    private QuickGrid<GetAccessPolicesForDomainByGridParamsQueryResult.Item> _domainAccessGrid;

    private async Task<AutoCompleteDataProviderResult<UserLookup>> UserLookupDataProvider(AutoCompleteDataProviderRequest<UserLookup> request)
    {
        var data = await this.Querier.Send(new GetUsersByGridParamsQuery(0, int.MaxValue, request.Filter.Value, request.Filter.Value));
        if (data.Status == QueryResultStatus.Succeeded)
        {
            var result = Enumerable.Select<GetUsersByGridParamsQueryResult.Item, UserLookup>(data.Data.Items, x => new UserLookup(x.Id, x.Name, x.EmailAddress));
            return new AutoCompleteDataProviderResult<UserLookup>
            {
                Data = result,
                TotalCount = data.Data.TotalItems,
            };
        }

        return new AutoCompleteDataProviderResult<UserLookup>();
    }

    private async Task OnAddUserSubmit()
    {
        var result = await this.Commander.Send(new AllowUserToAccessDomainCommand(
            this._domainId, this._addUserModel.UserId, this._addUserModel.Type));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"User invited successfully."));
            await this._addUserOffcanvas.HideAsync();
            await this._domainAccessGrid.RefreshDataAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to invite user."));
        }
    }

    private void SetupDomainAccess()
    {
        this._domainAccessItemProvider = async req =>
        {
            var query = new GetAccessPolicesForDomainByGridParamsQuery(
                this._domainId,
                req.StartIndex,
                req.Count ?? this._domainAccessPagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: Enumerable.ToList<GetAccessPolicesForDomainByGridParamsQueryResult.Item>(result.Data.Items),
                totalItemCount: result.Data.TotalItems);
        };
    }

    private void UserLookupChanged(UserLookup arg)
    {
        this._addUserModel.UserId = arg.Id;
    }

    private Task OnAddUserClick()
    {
        this._addUserModel = new AddUserModel();
        return this._addUserOffcanvas.ShowAsync();
    }
    
    private Task OnChangeAccessClick(GetAccessPolicesForDomainByGridParamsQueryResult.Item item)
    {
        this._changeAccessModel = new ChangeAccessModel
        {
            Name = item.Name,
            Type = item.Type,
            UserId = item.UserId,
        };
        return this._changeAccessOffcanvas.ShowAsync();
    }
    
    private async Task OnChangeAccessSubmit()
    {
        var result = await this.Commander.Send(new AlterUserAccessToDomainCommand(
            this._domainId, this._changeAccessModel.UserId, this._changeAccessModel.Type));
        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"Access policy changed successfully."));
            await this._domainAccessGrid.RefreshDataAsync();
            await this._changeAccessOffcanvas.HideAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to change access policy."));
        }
    }

    private class AddUserModel
    {
        public DomainAccessPolicyType Type { get; set; } = DomainAccessPolicyType.Unknown;

        public string SelectedUserId { get; set; } = string.Empty;

        public Guid UserId { get; set; }
    }
    
    private class ChangeAccessModel
    {
        public Guid UserId { get; init; }

        public DomainAccessPolicyType Type { get; set; }

        public string Name { get; init; } = string.Empty;
    }

    internal record UserLookup(Guid Id, string Name, string EmailAddress)
    {
        public override string ToString()
        {
            return $"{this.Name} - {this.EmailAddress}";
        }
    }

    private async Task OnRemoveAccessClick(GetAccessPolicesForDomainByGridParamsQueryResult.Item context)
    {
        var confirmation = await this._confirmDialog.ShowAsync(
            title: "Are you sure you want to remove access to this domain?",
            message1: "This will remove their access from this domain. This doesn't affect their access to any subdomains",
            message2: "Do you want to proceed?");

        if (!confirmation)
        {
            return;
        }

        var result = await this.Commander.Send(new RevokeUserToAccessDomainCommand(this._domainId, context.UserId));
        
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