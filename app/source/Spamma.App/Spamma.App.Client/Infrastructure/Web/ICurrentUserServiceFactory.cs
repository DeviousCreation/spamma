using Spamma.App.Client.Infrastructure.Contracts.Web;

namespace Spamma.App.Client.Infrastructure.Web;

public interface ICurrentUserServiceFactory
{
    ICurrentUserService Create();
}