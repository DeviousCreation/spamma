using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.QuickGrid;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;
using Spamma.App.Client.Infrastructure.Querying.User.Queries;
using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

namespace Spamma.App.Client.Components.Pages.Users;

public partial class Listing : ComponentBase
{
    private InviteUserModel _inviteUserModel = new InviteUserModel();
    PaginationState pagination = new PaginationState { ItemsPerPage = 2 };
    private GridItemsProvider<GetUsersByGridParamsQueryResult.Item>? _userItemProvider;
    private Offcanvas _inviteUserOffCanvas;
    private QuickGrid<GetUsersByGridParamsQueryResult.Item> _grid;

    [Inject]
    private ICommander Commander { get; set; } = default!;

    [Inject]
    private IQuerier Querier { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!;

    [Inject]
    private IClock Clock { get; set; } = default!;

    protected override Task OnInitializedAsync()
    {
        this._userItemProvider = async req =>
        {
            var query = new GetUsersByGridParamsQuery(
                req.StartIndex,
                req.Count ?? this.pagination.ItemsPerPage);

            var result = await this.Querier.Send(query, req.CancellationToken);
            return GridItemsProviderResult.From(
                items: result.Data.Items.ToList(),
                totalItemCount: result.Data.TotalItems);
        };

        return Task.CompletedTask;
    }

    private async Task OnInviteClick()
    {
        this._inviteUserModel = new InviteUserModel();
        await this._inviteUserOffCanvas.ShowAsync();
    }

    private async Task OnInviteSubmit()
    {
        await this.Commander.Send(new RegisterUserCommand(this._inviteUserModel.Name, this._inviteUserModel.EmailAddress));
        this.ToastService.Notify(new(ToastType.Success, $"User invited successfully."));
        await this._inviteUserOffCanvas.HideAsync();
        await this._grid.RefreshDataAsync();
    }

    private void NavigateToUser(Guid userId)
    {
        this.NavigationManager.NavigateTo($"/users/{userId}");
    }

    private async Task ToggleUserState(Guid userId, bool isActive)
    {
        CommandResult result;
        if (isActive)
        {
            result = await this.Commander.Send(new DisableUserCommand(userId));
        }
        else
        {
            result = await this.Commander.Send(new EnableUserCommand(userId));
        }

        if (result.Status == CommandResultStatus.Succeeded)
        {
            this.ToastService.Notify(new(ToastType.Success, $"User state updated successfully."));
            await this._grid.RefreshDataAsync();
        }
        else
        {
            this.ToastService.Notify(new(ToastType.Danger, $"Failed to update user state."));
        }
    }

    public class InviteUserModel
    {
        public string EmailAddress { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}