using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Boxtorio.Common;
using Boxtorio.Configs;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Boxtorio.Services;

public sealed class AuthService
{
    private readonly AuthConfig config;
    private readonly DataContext context;

    public AuthService(DataContext context, IOptions<AuthConfig> config)
    {
        this.context = context;
        this.config = config.Value;
    }

    private TokenModel GenerateTokens(AccountSession session)
    {
        var dtNow = DateTime.Now;
        if (session.User == null)
        {
            throw new ArgumentException("wtf");
        }

        var jwt = new JwtSecurityToken(
            config.Issuer,
            config.Audience,
            notBefore: dtNow,
            claims: new Claim[]
            {
                new(ClaimsIdentity.DefaultNameClaimType, session.User.Name),
                new(ClaimTypes.Role, session.User.Role),
                new("id", session.User.Id.ToString()),
                new("sessionId", session.Id.ToString())
            },
            expires: DateTime.Now.AddMinutes(config.LifeTime),
            signingCredentials: new SigningCredentials(config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var refresh = new JwtSecurityToken(
            notBefore: dtNow,
            claims: new Claim[] { new("refreshToken", session.RefreshToken.ToString()) },
            expires: DateTime.Now.AddHours(config.LifeTime),
            signingCredentials: new SigningCredentials(config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
        );
        var encodedRefresh = new JwtSecurityTokenHandler().WriteToken(refresh);
        return new TokenModel(encodedJwt, encodedRefresh);
    }

    private async Task<Account> GetUserByCredention(string login, string password)
    {
        var user = await context.Accounts.FirstOrDefaultAsync(x => string.Equals(x.Email, login, StringComparison.CurrentCultureIgnoreCase));
        if (user == null)
        {
            throw new ArgumentException("user not found");
        }

        if (!HashHelper.Verify(password, user.PasswordHash))
        {
            throw new ArgumentException("password is incorrect");
        }

        return user;
    }

    public async Task<TokenModel> GetToken(string login, string password)
    {
        var user = await GetUserByCredention(login, password);
        var session = await context.AccountSessions.AddAsync(new AccountSession
        {
            Id = Guid.NewGuid(),
            User = user,
            RefreshToken = Guid.NewGuid(),
            Created = DateTime.UtcNow
        });
        await context.SaveChangesAsync();
        return GenerateTokens(session.Entity);
    }

    public async Task<AccountSession> GetSessionById(Guid id)
    {
        var session = await context.AccountSessions.FirstOrDefaultAsync(x => x.Id == id);
        if (session == null)
        {
            throw new ArgumentException("session not found");
        }

        return session;
    }

    private async Task<AccountSession> GetSessionByRefreshToken(Guid id)
    {
        var session = await context.AccountSessions.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshToken == id);
        if (session == null)
        {
            throw new ArgumentException("session not found");
        }

        return session;
    }

    public async Task<TokenModel> GetTokenByRefreshToken(string refreshToken)
    {
        var validParams = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = config.SymmetricSecurityKey()
        };
        var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validParams, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtToken
            || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("invalid token");
        }

        if (principal.Claims.FirstOrDefault(x => x.Type == "refreshToken")?.Value is not { } refreshIdString
            || !Guid.TryParse(refreshIdString, out var refreshId))
        {
            throw new SecurityTokenException("invalid token");
        }

        var session = await GetSessionByRefreshToken(refreshId);
        if (!session.IsActive)
        {
            throw new ArgumentException("session is not active");
        }

        session.RefreshToken = Guid.NewGuid();
        await context.SaveChangesAsync();

        return GenerateTokens(session);
    }
}
