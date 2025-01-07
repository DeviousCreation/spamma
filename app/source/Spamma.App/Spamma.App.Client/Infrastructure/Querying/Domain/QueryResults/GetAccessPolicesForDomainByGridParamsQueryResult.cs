using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Querying;

namespace Spamma.App.Client.Infrastructure.Querying.Domain.QueryResults;

public record GetAccessPolicesForDomainByGridParamsQueryResult(
    IReadOnlyList<GetAccessPolicesForDomainByGridParamsQueryResult.Item> Items, int TotalItems)
    : ListQueryResultOf<GetAccessPolicesForDomainByGridParamsQueryResult.Item>(Items, TotalItems)
{
    public record Item(
        Guid UserId,
        string EmailAddress,
        string Name,
        DomainAccessPolicyType Type,
        DateTime WhenAssigned);
}