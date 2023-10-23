namespace Boxtorio.Models;

public sealed class TokenRequestModel
{
	public string Login { get; set; }
	public string Password { get; set; }

	public TokenRequestModel(string login, string password)
	{
		Login = login;
		Password = password;
	}
}

public sealed class RefreshTokenRequestModel
{
	public string RefreshToken { get; set; }

	public RefreshTokenRequestModel(string refreshToken)
	{
		RefreshToken = refreshToken;
	}
}
