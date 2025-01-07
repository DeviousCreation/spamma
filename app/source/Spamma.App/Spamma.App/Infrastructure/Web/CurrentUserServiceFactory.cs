using Spamma.App.Client.Infrastructure.Contracts.Web;
using Spamma.App.Client.Infrastructure.Web;
using ApiCurrentUserService = Spamma.App.Infrastructure.Web.CurrentUserService;
using BlazorCurrentUserService = Spamma.App.Client.Infrastructure.Web.CurrentUserService;

namespace Spamma.App.Infrastructure.Web;

public class CurrentUserServiceFactory(IServiceProvider serviceProvider) : ICurrentUserServiceFactory
{
    public ICurrentUserService Create()
    {
        var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
        if (httpContextAccessor?.HttpContext != null)
        {
            return serviceProvider.GetRequiredService<ApiCurrentUserService>();
        }

        return serviceProvider.GetRequiredService<BlazorCurrentUserService>();
    }
}