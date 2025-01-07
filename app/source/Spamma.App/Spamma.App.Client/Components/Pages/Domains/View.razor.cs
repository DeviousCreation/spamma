using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using NodaTime;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Domain.Queries;
using Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

namespace Spamma.App.Client.Components.Pages.Domains;

public partial class View
{
    private Guid _domainId;
    private GetDomainByIdQueryResult? _domain;
    private ConfirmDialog _confirmDialog;

    [Inject]
    private ICommander Commander { get; set; } = default!;

    [Inject]
    private IQuerier Querier { get; set; } = default!;

    [Inject]
    private ToastService ToastService { get; set; } = default!;

    [Inject]
    private IClock Clock { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter]
    public string DomainId { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        this._domainId = Guid.Parse(this.DomainId);
        var query = new GetDomainByIdQuery(this._domainId);
        var result = await this.Querier.Send(query);
        if (result.Status == QueryResultStatus.Succeeded)
        {
            this._domain = result.Data;
            this.SetupDomainAccess();
            this.SetupSubdomain();
        }

        await base.OnParametersSetAsync();
    }
}