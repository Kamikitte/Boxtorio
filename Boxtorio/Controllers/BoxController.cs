using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class BoxController : ControllerBase
{
	private readonly BoxService _boxService;
	public BoxController(BoxService boxService)
	{
		_boxService = boxService;
	}

	[HttpPost]
	public async Task AddBox(CreateBoxModel model)
		=> await _boxService.AddBox(model);

	[HttpPost]
	public async Task<IEnumerable<BoxModel>> GetBoxesFromDP(Guid dpid)
		=> await _boxService.GetBoxesFromDP(dpid);

	[HttpPost]
	public async Task ChangeBox(BoxModel newModel)
		=> await _boxService.ChangeBox(newModel);

	[HttpPost]
	public async Task RemoveBox(Guid boxid)
		=> await _boxService.RemoveBox(boxid);
}