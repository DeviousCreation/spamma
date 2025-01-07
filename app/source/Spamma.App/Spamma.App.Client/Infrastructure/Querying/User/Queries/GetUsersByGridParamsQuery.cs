using Spamma.App.Client.Infrastructure.Querying.User.QueryResults;

namespace Spamma.App.Client.Infrastructure.Querying.User.Queries;

public record GetUsersByGridParamsQuery(int Skip, int Take, string EmailAddress = "", string Name = "") :
    GridParams<GetUsersByGridParamsQueryResult>(Skip, Take);