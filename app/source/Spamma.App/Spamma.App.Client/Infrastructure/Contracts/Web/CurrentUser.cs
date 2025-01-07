using System.Security.Claims;
using MaybeMonad;

namespace Spamma.App.Client.Infrastructure.Contracts.Web;

public record CurrentUser
{
    private CurrentUser(Guid id, string email, string name, string[] role)
    {
        this.Id = id;
        this.Email = email;
        this.Name = name;
        this.Role = role;
    }

    public Guid Id { get;  }

    public string Email { get;  }

    public string Name { get; }

    public string[] Role { get; }

    public static Maybe<CurrentUser> FromClaimsPrincipal(ClaimsPrincipal user)
    {
        if (user.Identity?.IsAuthenticated != true)
        {
            return Maybe<CurrentUser>.Nothing;
        }

        if (user.Claims.All(x => x.Type != ClaimTypes.Sid) ||
            user.Claims.All(x => x.Type != ClaimTypes.Email) ||
            user.Claims.All(x => x.Type != ClaimTypes.Name) ||
            user.Claims.All(x => x.Type != ClaimTypes.Role))
        {
            return Maybe<CurrentUser>.Nothing;
        }

        if (!Guid.TryParse(user.Claims.First(x => x.Type == ClaimTypes.Sid).Value, out var id))
        {
            return Maybe<CurrentUser>.Nothing;
        }

        return new CurrentUser(
            id,
            user.Claims.First(x => x.Type == ClaimTypes.Email).Value,
            user.Claims.First(x => x.Type == ClaimTypes.Name).Value,
            user.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray());
    }
}