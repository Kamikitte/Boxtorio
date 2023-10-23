using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Boxtorio.Configs;

public sealed class AuthConfig
{
	public const string Position = "auth";
	public string Issuer { get; set; } = null!;
	public string Audience { get; set; } = null!;
	public string Key { get; set; } = null!;
	public int LifeTime { get; set; }
	public SymmetricSecurityKey SymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}
