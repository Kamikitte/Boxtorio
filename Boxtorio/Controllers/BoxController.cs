using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public sealed class BoxController : ControllerBase
{
	private readonly BoxService boxService;
	public BoxController(BoxService boxService)
	{
		this.boxService = boxService;
	}

	[HttpPost]
	public async Task AddBox(CreateBoxModel model)
		=> await boxService.AddBox(model);

	[HttpPost]
	public async Task<IEnumerable<BoxModel>> GetBoxesFromDp(Guid dpid)
		=> await boxService.GetBoxesFromDp(dpid);

	[HttpPost]
	public async Task ChangeBox(BoxModel newModel)
		=> await boxService.ChangeBox(newModel);

	[HttpPost]
	public async Task RemoveBox(Guid boxid)
		=> await boxService.RemoveBox(boxid);
}
