using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class DeliveryPointController : ControllerBase
	{
		private readonly DeliveryPointService _dpservice;
		public DeliveryPointController(DeliveryPointService dpservice)
		{
			_dpservice = dpservice;
		}

		[HttpPost]
		public async Task Create(CreateDeliveryPointModel model)
			=> await _dpservice.CreateDeliveryPoint(model);

		[HttpPost]
		public async Task AssignWorker(Guid workerid, Guid deliverypointid)
			=> await _dpservice.AssignWorker(workerid, deliverypointid);

		[HttpPost]
		public async Task<IEnumerable<WorkerModel>> GetWorkers(Guid deliverypoint)
			=> await _dpservice.GetWorkersFromDP(deliverypoint);

		[HttpGet]
		public async Task<IEnumerable<DeliveryPoint>> GetDeliveryPoints()
			=> await _dpservice.GetDeliveryPoints();

		[HttpPost]
		public async Task AddNewPlace(CreatePlaceModel model)
			=> await _dpservice.AddNewPlace(model);
	}
}
