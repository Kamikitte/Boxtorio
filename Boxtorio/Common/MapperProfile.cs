using AutoMapper;
using Boxtorio.Data.Entities;
using Boxtorio.Models;

namespace Boxtorio.Common
{
	public class MapperProile : Profile
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

			CreateMap<CreatePlaceModel, Place>()
				.ForMember(d => d.Id, m => m.MapFrom(s => Guid.NewGuid()));
		}
	}
}