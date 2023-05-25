using Boxtorio.Common;
using Boxtorio.Configs;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Boxtorio.Services
{
	public class AuthService
	{
		private readonly DataContext _context;
		private readonly AuthConfig _config;
		public AuthService(DataContext context, IOptions<AuthConfig> config)
		{
			_context = context;
			_config = config.Value;
		}

		private TokenModel GenerateTokens(AccountSession session)
		{
			var dtNow = DateTime.Now;
			if (session.User == null)
				throw new Exception("wtf");

			var jwt = new JwtSecurityToken(
				issuer: _config.Issuer,
				audience: _config.Audience,
				notBefore: dtNow,
				claims: new Claim[]
				{
					new Claim(ClaimsIdentity.DefaultNameClaimType, session.User.Name),
					new Claim(ClaimTypes.Role, session.User.Role),
					new Claim("id", session.User.Id.ToString()),
					new Claim("sessionId", session.Id.ToString())
				},
				expires: DateTime.Now.AddMinutes(_config.LifeTime),
				signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
				);
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var refresh = new JwtSecurityToken(
				notBefore: dtNow,
				claims: new Claim[]
				{
					new Claim("refreshToken", session.RefreshToken.ToString())
				},
				expires: DateTime.Now.AddHours(_config.LifeTime),
				signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
				);
			var encodedRefresh = new JwtSecurityTokenHandler().WriteToken(refresh);
			return new TokenModel(encodedJwt, encodedRefresh);
		}

		private async Task<Account> GetUserByCredention(string login, string password)
		{
			var user = await _context.Accounts.FirstOrDefaultAsync(x => x.Email.ToLower() == login.ToLower());
			if (user == null)
				throw new Exception("user not found");

			if (!HashHelper.Verify(password, user.PasswordHash))
				throw new Exception("password is incorrect");

			return user;
		}

		public async Task<TokenModel> GetToken(string login, string password)
		{
			var user = await GetUserByCredention(login, password);
			var session = await _context.AccountSessions.AddAsync(new AccountSession
			{
				Id = Guid.NewGuid(),
				User = user,
				RefreshToken = Guid.NewGuid(),
				Created = DateTime.UtcNow,

			});
			await _context.SaveChangesAsync();
			return GenerateTokens(session.Entity);
		}

		public async Task<AccountSession> GetSessionById(Guid id)
		{
			var session = await _context.AccountSessions.FirstOrDefaultAsync(x => x.Id == id);
			if (session == null)
				throw new Exception("session not found");

			return session;
		}

		private async Task<AccountSession> GetSessionByRefreshToken(Guid id)
		{
			var session = await _context.AccountSessions.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshToken == id);
			if (session == null)
				throw new Exception("session not found");

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
				IssuerSigningKey = _config.SymmetricSecurityKey()
			};
			var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validParams, out var securityToken);

			if (securityToken is not JwtSecurityToken jwtToken
				|| !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
				StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("invalid token");

			if (principal.Claims.FirstOrDefault(x => x.Type == "refreshToken")?.Value is String refreshIdString
				&& Guid.TryParse(refreshIdString, out var refreshId))
			{
				var session = await GetSessionByRefreshToken(refreshId);
				if (!session.IsActive)
					throw new Exception("session is not active");

				session.RefreshToken = Guid.NewGuid();
				await _context.SaveChangesAsync();

				return GenerateTokens(session);
			}
			else
				throw new SecurityTokenException("invalid token");
		}
	}
}
