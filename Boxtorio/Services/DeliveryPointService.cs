using AutoMapper;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Microsoft.EntityFrameworkCore;

namespace Boxtorio.Services;

public sealed class DeliveryPointService
{
	private readonly IMapper mapper;
	private readonly DataContext context;
	private readonly AccountService accountService;
	public DeliveryPointService(IMapper mapper, DataContext context, AccountService accountService)
	{
		this.mapper = mapper;
		this.context = context;
		this.accountService = accountService;
	}

	public async Task CreateDeliveryPoint(CreateDeliveryPointModel model)
	{
		var deliveryPoint = mapper.Map<DeliveryPoint>(model);
		await context.DeliveryPoints.AddAsync(deliveryPoint);
		await context.SaveChangesAsync();
	}
	public async Task<DeliveryPoint> GetDeliveryPoint(Guid id)
	{
		var point = await context.DeliveryPoints
			.Include(x => x.Workers)
			.Include(x => x.Places)
			.Include(x => x.Boxes)
			.FirstOrDefaultAsync(x => x.Id == id);
		if (point == null)
		{
			throw new ArgumentException("Delivery point not found");
		}

		return point;
	}

	public Task<IEnumerable<DeliveryPoint>> GetDeliveryPoints()
	{
		return Task.FromResult<IEnumerable<DeliveryPoint>>(context.DeliveryPoints
            .Include(x => x.Workers)
            .Include(x => x.Places)
            .Include(x => x.Boxes));
	}

	public async Task AssignWorker(Guid workerid, Guid deliverypointid)
	{
		await GetDeliveryPoint(deliverypointid);
		var worker = await accountService.GetAccount<Worker>(workerid);
		worker.DeliveryPointId = deliverypointid;
		await context.SaveChangesAsync();
	}
	public async Task<IEnumerable<WorkerModel>> GetWorkersFromDp(Guid deliverypointid)
    {
        var point = await GetDeliveryPoint(deliverypointid);
        return point.Workers != null ? point.Workers.Select(mapper.Map<WorkerModel>) : new List<WorkerModel>();
    }

	public async Task AddNewPlace(CreatePlaceModel model)
	{
		var place = mapper.Map<Place>(model);
		await context.Places.AddAsync(place);
		await context.SaveChangesAsync();
	}
}
