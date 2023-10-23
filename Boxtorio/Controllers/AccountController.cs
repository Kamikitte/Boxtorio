using AutoMapper;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public sealed class AccountController : ControllerBase
{
	private readonly AccountService accountService;
	private readonly IMapper mapper;
	public AccountController(AccountService accountService, IMapper mapper)
	{
		this.accountService = accountService;
		this.mapper = mapper;
	}

	[HttpGet]
	[Authorize]
	public async Task<AccountModel> GetCurrentUser()
	{
		var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
		if (Guid.TryParse(userIdString, out var userId))
		{
			return await accountService.GetAccount(userId);
		}

        throw new ArgumentException("you are not authorized");
    }

	[HttpGet]
	[Authorize]
	public async Task<IEnumerable<WorkerModel>> GetWorkers()
	{
		var workers = await accountService.GetAccounts<Worker>();
		return mapper.Map<IEnumerable<WorkerModel>>(workers);
	}
}
