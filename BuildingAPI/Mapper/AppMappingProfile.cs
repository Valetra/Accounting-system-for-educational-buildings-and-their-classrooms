using AutoMapper;

namespace Mapper;

public class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<DAL.Models.Building, ResponseObjects.Building>();
		CreateMap<RequestObjects.Building, DAL.Models.Building>();
		CreateMap<DAL.Models.Building, MessageContracts.Building>();
	}
}
