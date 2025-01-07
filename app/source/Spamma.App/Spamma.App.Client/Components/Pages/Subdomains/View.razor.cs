using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Subdomains;

public partial class View : ComponentBase
{
    private Guid _subdomainId;
    private ConfirmDialog _confirmDialog;
    private GetSubdomainByIdQueryResult _subdomain;

    [Inject]
    private ICommander Commander { get; set; } = default!;

    [Inject]
    private IQuerier Querier { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!;

    [Inject]
    private IClock Clock { get; set; } = default!;
    
    [Parameter]
    public string SubdomainId { get; set; } = default!;
    
    protected override async Task OnParametersSetAsync()
    {
        this._subdomainId = Guid.Parse(this.SubdomainId);
        var query = new GetSubdomainByIdQuery(this._subdomainId);
        var result = await this.Querier.Send(query);
        if (result.Status == QueryResultStatus.Succeeded)
        {
            this._subdomain = result.Data;
        }

        this.SetupSubdomainAccess();
        this.SetupChaosMonkey();

        await base.OnParametersSetAsync();
    }
}