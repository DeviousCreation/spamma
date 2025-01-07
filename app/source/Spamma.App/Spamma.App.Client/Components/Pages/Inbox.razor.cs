using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Humanizer;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MimeKit;
using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Email.Queries;
using Spamma.App.Client.Infrastructure.Querying.Email.QueryResults;

namespace Spamma.App.Client.Components.Pages;

public partial class Inbox : ComponentBase
{
    // private Guid _selectedId;
    // private IReadOnlyList<GetEmailsByGridParamsQueryResult.Item> _emailMetaDataItems = new List<GetEmailsByGridParamsQueryResult.Item>();
    // //private string? _continuationToken;
    // private Tab _currentTab;
    //
    // private MimeMessage _mailMessage;
    //
    // private enum Tab
    // {
    //     NotSelected = 0,
    //     Html = 1,
    //     Text = 2,
    //     Attachments = 3,
    //     Source = 4,
    // }
    //
    //     [Inject]
    //     protected IJSRuntime JsRuntime { get; set; } = default!;
    //     
    //     [Inject]
    //     private IQuerier Querier { get; set; } = default!;
    //
    //     protected override async Task OnParametersSetAsync()
    //     {
    //         var result = await this.Querier.Send(new GetEmailsByGridParamsQuery(0, int.MaxValue));
    //         if (result != null)
    //         {
    //             //this._continuationToken = result.ContinuationToken;
    //             this._emailMetaDataItems = result.Data.Items;
    //         }
    //     }
    //
    //     private static string HumanizeEpochMilliseconds(DateTimeOffset timestamp, bool inWords = false)
    //     {
    //         return inWords ? timestamp.DateTime.ToOrdinalWords() : timestamp.DateTime.Humanize();
    //     }
    //
    //     private async Task OnMessageSelected(Guid id)
    //     {
    //         this._currentTab = Tab.NotSelected;
    //         this.StateHasChanged();
    //         var meta = this._emailMetaDataItems.Single(x => x.Id == id);
    //         var result = await Querier.Send(new GetEmailDataByIdQuery(id));
    //         
    //         var emlBytes = Convert.FromBase64String(result.Data.FileData);
    //         var emlStream = new MemoryStream(emlBytes);
    //
    //         // Load the MimeMessage from the stream
    //         this._mailMessage = await MimeMessage.LoadAsync(emlStream);
    //         
    //         if (!string.IsNullOrWhiteSpace(_mailMessage.HtmlBody))
    //         {
    //             this._currentTab = Tab.Html;
    //         }
    //         else if (!string.IsNullOrWhiteSpace(_mailMessage.TextBody))
    //         {
    //             this._currentTab = Tab.Text;
    //         }
    //         else
    //         {
    //             this._currentTab = Tab.Source;
    //         }
    //     }

        
    }