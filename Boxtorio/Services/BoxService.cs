using AutoMapper;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;

namespace Boxtorio.Services;

public sealed class BoxService
{
	private readonly IMapper mapper;
	private readonly DataContext dataContext;
	public BoxService(IMapper mapper, DataContext dataContext)
	{
		this.mapper = mapper;
		this.dataContext = dataContext;
	}

	public async Task AddBox(CreateBoxModel model)
	{
		var box = mapper.Map<Box>(model);
		await dataContext.Boxes.AddAsync(box);
		await dataContext.SaveChangesAsync();
	}
	public async Task<IEnumerable<BoxModel>> GetBoxesFromDp(Guid dpid)
	{
		_ = await dataContext.DeliveryPoints.FindAsync(dpid) ??
            throw new Exception("Неверное значение id пункта");

		var list = dataContext.Boxes
			.Where(x => x.DeliveryPointId == dpid);
		return mapper.Map<IEnumerable<BoxModel>>(list);
	}

	public async Task<IEnumerable<BoxModel>> GetBoxesFromPlace(Guid placeid)
	{
		_ = await dataContext.Places.FindAsync(placeid) ??
            throw new Exception("Неверное значение id места");

		var list = dataContext.Boxes
			.Where(x => x.PlaceId == placeid);
		return mapper.Map<IEnumerable<BoxModel>>(list);
	}

	public async Task ChangeBox(BoxModel newModel)
	{
		var boxEntity = await dataContext.Boxes.FindAsync(newModel.Id) ??
		                throw new Exception("Неверный ID посылки");

		mapper.Map(newModel, boxEntity);

		await dataContext.SaveChangesAsync();
	}

	public async Task RemoveBox(Guid id)
	{
		var boxEntity = await dataContext.Boxes.FindAsync(id) ??
		                throw new Exception("Неверный ID посылки");

		dataContext.Boxes.Remove(boxEntity);

		await dataContext.SaveChangesAsync();
	}
}
