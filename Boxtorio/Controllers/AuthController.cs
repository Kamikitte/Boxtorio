using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly AuthService _authService;
	private readonly AccountService _accountService;

	public AuthController(AuthService authService, AccountService accountService)
	{
		_authService = authService;
		_accountService = accountService;
	}

	[HttpPost]
	public async Task CreateUser(CreateAccountModel model)
	{
		if (await _accountService.CheckAccountExist(model.Email))
			throw new Exception("user is exist");
		await _accountService.CreateAccount(model);
	}

	[HttpPost]
	public async Task<TokenModel> Token(TokenRequestModel model)
		=> await _authService.GetToken(model.Login, model.Password);

	[HttpPost]
	public async Task<TokenModel> RefreshToken(RefreshTokenRequestModel model)
		=> await _authService.GetTokenByRefreshToken(model.RefreshToken);
}