using AutoMapper;

namespace Mapper;

public class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<DAL.Models.Classroom, ResponseObjects.Classroom>();
		CreateMap<RequestObjects.Classroom, DAL.Models.Classroom>();

		CreateMap<DAL.Models.ShortBuildingInfo, ResponseObjects.ShortBuildingInfo>();
		CreateMap<MessageContracts.Building, DAL.Models.ShortBuildingInfo>();
	}
}
