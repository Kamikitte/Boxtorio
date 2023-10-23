using AutoMapper;
using Boxtorio.Data.Entities;
using Boxtorio.Models;

namespace Boxtorio.Common;

public sealed class MapperProile : Profile
{
	public MapperProile()
	{
		CreateMap<CreateAccountModel, Account>()
			.ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()))
			.ForMember(d => d.PasswordHash, m => m.MapFrom(s => HashHelper.GetHash(s.Password)));

		CreateMap<CreateAccountModel, Admin>()
			.IncludeBase<CreateAccountModel, Account>();

		CreateMap<CreateAccountModel, Worker>()
			.IncludeBase<CreateAccountModel, Account>();

		CreateMap<Account, AccountModel>();

		CreateMap<Worker, WorkerModel>();

		CreateMap<CreateDeliveryPointModel, DeliveryPoint>()
			.ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));

		CreateMap<DeliveryPoint, DeliveryPointModel>();

		CreateMap<CreatePlaceModel, Place>()
			.ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));

		CreateMap<Place, PlaceModel>();

		CreateMap<CreateBoxModel, Box>()
			.ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));

		CreateMap<Box, BoxModel>();

		CreateMap<BoxModel, Box>();
	}
}
