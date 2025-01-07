using MaybeMonad;

namespace Spamma.App.Client.Infrastructure.Contracts.Web;

public interface ICurrentUserService
{
    Task<Maybe<CurrentUser>> GetCurrentUserAsync();
}