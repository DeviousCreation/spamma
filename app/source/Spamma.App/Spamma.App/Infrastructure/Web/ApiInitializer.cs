using Spamma.App.Client.Infrastructure.Contracts.Domain;
using Spamma.App.Client.Infrastructure.Domain.UserAggregate.Commands;

namespace Spamma.App.Infrastructure.Web;

public static partial class ApiInitializer
{
    public static partial void ConfigureApi(this WebApplication app);
}