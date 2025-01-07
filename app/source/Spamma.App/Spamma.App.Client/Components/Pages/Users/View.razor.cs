using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.User.Queries;
using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

namespace Spamma.App.Client.Components.Pages.Users;

public partial class View : ComponentBase
{
    private Guid _userId;
    private GetUserByIdQueryResult? _user;
    private ConfirmDialog _confirmDialog;

    [Inject]
    private ICommander Commander { get; set; } = default!;

    [Inject]
    private IQuerier Querier { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!;

    [Inject]
    private IClock Clock { get; set; } = default!;

    [Parameter]
    public string UserId { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        this._userId = Guid.Parse(this.UserId);
        var query = new GetUserByIdQuery(this._userId);
        var result = await this.Querier.Send(query);
        if (result.Status == QueryResultStatus.Succeeded)
        {
            this._user = result.Data;
        }

        this.SetupDomainProvider();
        this.SetupSubdomainProvider();
        await base.OnParametersSetAsync();
    }
}