using AutoMapper;
using Boxtorio.Data;
using Boxtorio.Data.Entities;
using Boxtorio.Models;

namespace Boxtorio.Services;

public class BoxService
{
	private readonly IMapper _mapper;
	private readonly DataContext _dataContext;
	public BoxService(IMapper mapper, DataContext dataContext)
	{
		_mapper = mapper;
		_dataContext = dataContext;
	}

	public async Task AddBox(CreateBoxModel model)
	{
		var box = _mapper.Map<Box>(model);
		await _dataContext.Boxes.AddAsync(box);
		await _dataContext.SaveChangesAsync();
	}
	public async Task<IEnumerable<BoxModel>> GetBoxesFromDP(Guid dpid)
	{
		var dp = await _dataContext.DeliveryPoints.FindAsync(dpid) ??
		         throw new Exception("Неверное значение id пункта");

		var list = _dataContext.Boxes
			.Where(x => x.DeliveryPointId == dpid);
		return _mapper.Map<IEnumerable<BoxModel>>(list);
	}

	public async Task<IEnumerable<BoxModel>> GetBoxesFromPlace(Guid placeid)
	{
		var dp = await _dataContext.Places.FindAsync(placeid) ??
		         throw new Exception("Неверное значение id места");

		var list = _dataContext.Boxes
			.Where(x => x.PlaceId == placeid);
		return _mapper.Map<IEnumerable<BoxModel>>(list);
	}

	public async Task ChangeBox(BoxModel newModel)
	{
		var boxEntity = await _dataContext.Boxes.FindAsync(newModel.Id) ?? 
		                throw new Exception("Неверный ID посылки");

		_mapper.Map(newModel, boxEntity);

		await _dataContext.SaveChangesAsync();
	}

	public async Task RemoveBox(Guid id)
	{
		var boxEntity = await _dataContext.Boxes.FindAsync(id) ??
		                throw new Exception("Неверный ID посылки");

		_dataContext.Boxes.Remove(boxEntity);

		await _dataContext.SaveChangesAsync();
	}
}