using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResultMonad;
using Spamma.App.Client.Infrastructure.Constants;
using Spamma.App.Client.Infrastructure.Contracts.Domain;

namespace Spamma.App.Infrastructure.Contracts;

internal interface IAuthTokenProvider
{
    internal Task<Result<string>> GetToken(Guid userId, Guid securityStamp, DateTime whenCreated);

    internal Task<Result<TokenResult, ErrorData>> ProcessToken(string token);

    internal record TokenResult(Guid UserId, Guid SecurityStamp, IReadOnlyDictionary<string, string> OtherData);
}

internal class AuthTokenProvider(IOptions<Settings> settings) : IAuthTokenProvider
{
    private readonly Settings _settings = settings.Value;
    private const string UserIdClaimType = "spamma-user-id";
    private const string SecurityTokenClaimType = "spamma-security-token";

    public Task<Result<string>> GetToken(Guid userId, Guid securityStamp, DateTime whenCreated)
    {
        var signingKey = new SymmetricSecurityKey(Convert.FromBase64String(this._settings.SigningKeyBase64));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(UserIdClaimType, userId.ToString()),
                new Claim(SecurityTokenClaimType, securityStamp.ToString()),
            }),
            Expires = whenCreated.AddHours(1),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Embed GUIDs in the token payload
        var jwtToken = tokenHandler.WriteToken(token);

        return Task.FromResult(Result.Ok(jwtToken));
    }

    public Task<Result<IAuthTokenProvider.TokenResult, ErrorData>> ProcessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String(this._settings.SigningKeyBase64);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
        };

        ClaimsPrincipal claimsPrincipal;
        try
        {
            claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch
        {
            return Task.FromResult(Result.Fail<IAuthTokenProvider.TokenResult, ErrorData>(new ErrorData(ErrorCodes.TokenNotValid, "Token not valid")));
        }

        if (claimsPrincipal.Claims.All(x => x.Type != UserIdClaimType) || claimsPrincipal.Claims.All(x => x.Type != SecurityTokenClaimType))
        {
            return Task.FromResult(Result.Fail<IAuthTokenProvider.TokenResult, ErrorData>(new ErrorData(ErrorCodes.TokenNotValid, "Token not valid")));
        }

        if (!Guid.TryParse(claimsPrincipal.Claims.Single(x => x.Type == UserIdClaimType).Value, out var userId) ||
            !Guid.TryParse(claimsPrincipal.Claims.Single(x => x.Type == SecurityTokenClaimType).Value, out var securityStamp))
        {
            return Task.FromResult(Result.Fail<IAuthTokenProvider.TokenResult, ErrorData>(new ErrorData(ErrorCodes.TokenNotValid, "Token not valid")));
        }

        return Task.FromResult(Result.Ok<IAuthTokenProvider.TokenResult, ErrorData>(
            new IAuthTokenProvider.TokenResult(userId, securityStamp,
                claimsPrincipal.Claims
                    .Where(x => x.Type != UserIdClaimType && x.Type != SecurityTokenClaimType)
                    .ToDictionary(x => x.Type, x => x.Value))));
    }
}