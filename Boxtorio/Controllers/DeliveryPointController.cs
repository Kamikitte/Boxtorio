using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Boxtorio.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boxtorio.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public sealed class DeliveryPointController : ControllerBase
{
    private readonly DeliveryPointService dpservice;

    public DeliveryPointController(DeliveryPointService dpservice)
    {
        this.dpservice = dpservice;
    }

    [HttpPost]
    public async Task Create(CreateDeliveryPointModel model)
        => await dpservice.CreateDeliveryPoint(model);

    [HttpPost]
    public async Task<DeliveryPoint> GetDeliveryPoint(Guid id)
        => await dpservice.GetDeliveryPoint(id);

    [HttpGet]
    public async Task<IEnumerable<DeliveryPoint>> GetDeliveryPoints()
        => await dpservice.GetDeliveryPoints();

    [HttpPost]
    public async Task AssignWorker(Guid workerid, Guid deliverypointid)
        => await dpservice.AssignWorker(workerid, deliverypointid);

    [HttpPost]
    public async Task<IEnumerable<WorkerModel>> GetWorkers(Guid deliverypoint)
        => await dpservice.GetWorkersFromDp(deliverypoint);


    [HttpPost]
    public async Task AddNewPlace(CreatePlaceModel model)
        => await dpservice.AddNewPlace(model);
}
