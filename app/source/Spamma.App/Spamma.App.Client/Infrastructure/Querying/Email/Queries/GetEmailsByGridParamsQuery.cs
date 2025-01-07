using Spamma.App.Client.Infrastructure.Contracts.Querying;
using Spamma.App.Client.Infrastructure.Querying.Email.QueryResults;
using Spamma.App.Client.Infrastructure.Querying.Subdomain.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.Email.Queries;

public record GetEmailsByGridParamsQuery(int Skip, int Take) : GridParams<GetEmailsByGridParamsQueryResult>(Skip, Take);