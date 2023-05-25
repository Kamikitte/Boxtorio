using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private readonly AccountService _accountService;
		public AccountController(AccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet]
		[Authorize]
		public async Task<AccountModel> GetCurrentUser()
		{
			var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
			if (Guid.TryParse(userIdString, out var userId))
			{
				return await _accountService.GetAccount(userId);
			}
			else
				throw new Exception("you are not authorized");
		}
	}
}