using AutoMapper;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
	private readonly AccountService _accountService;
	private readonly IMapper _mapper;
	public AccountController(AccountService accountService, IMapper mapper)
	{
		_accountService = accountService;
		_mapper = mapper;
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

	[HttpGet]
	[Authorize]
	public async Task<IEnumerable<WorkerModel>> GetWorkers()
	{
		var workers = await _accountService.GetAccounts<Worker>();
		return _mapper.Map<IEnumerable<WorkerModel>>(workers);
	}
}