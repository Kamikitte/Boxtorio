using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly AccountService accountService;
    private readonly AuthService authService;

    public AuthController(AuthService authService, AccountService accountService)
    {
        this.authService = authService;
        this.accountService = accountService;
    }

    [HttpPost]
    public async Task CreateUser(CreateAccountModel model)
    {
        if (await accountService.CheckAccountExist(model.Email))
        {
            throw new ArgumentException("user is exist");
        }

        await accountService.CreateAccount(model);
    }

    [HttpPost]
    public async Task<TokenModel> Token(TokenRequestModel model)
        => await authService.GetToken(model.Login, model.Password);

    [HttpPost]
    public async Task<TokenModel> RefreshToken(RefreshTokenRequestModel model)
        => await authService.GetTokenByRefreshToken(model.RefreshToken);
}
