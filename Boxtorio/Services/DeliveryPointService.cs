using AutoMapper;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;
using Microsoft.EntityFrameworkCore;

namespace Boxtorio.Services
{
	public class DeliveryPointService
	{
		private readonly IMapper _mapper;
		private readonly DataContext _context;
		private readonly AccountService _accountService;
		public DeliveryPointService(IMapper mapper, DataContext context, AccountService accountService)
		{
			_mapper = mapper;
			_context = context;
			_accountService = accountService;
		}

		public async Task CreateDeliveryPoint(CreateDeliveryPointModel model)
		{
			var deliveryPoint = _mapper.Map<DeliveryPoint>(model);
			await _context.DeliveryPoints.AddAsync(deliveryPoint);
			await _context.SaveChangesAsync();
		} 
		public async Task<DeliveryPoint> GetDeliveryPoint(Guid id)
		{
			var point = await _context.DeliveryPoints
				.Include(x => x.Workers)
				.FirstOrDefaultAsync(x => x.Id == id);
			if (point == null)
			{
				throw new Exception("Delivery point not found");
			}

			return point;
		}

		public async Task<IEnumerable<DeliveryPoint>> GetDeliveryPoints()
		{
			return _context.DeliveryPoints.Include(x => x.Workers);
		}

		public async Task AssignWorker(Guid workerid, Guid deliverypointid)
		{
			var point = await GetDeliveryPoint(deliverypointid);
			var worker = await _accountService.GetAccount<Worker>(workerid);
			worker.DeliveryPointId = deliverypointid;
			await _context.SaveChangesAsync();
		}
		public async Task<IEnumerable<WorkerModel>> GetWorkersFromDP(Guid deliverypointid)
		{
			var point = await GetDeliveryPoint(deliverypointid);
			return point.Workers.Select(_mapper.Map<WorkerModel>);
		}
	}
}
