using AutoMapper;

namespace Mapper;

public class AppMappingProfile : Profile
{
	public AppMappingProfile()
	{
		CreateMap<DAL.Models.Classroom, ResponseObjects.Classroom>();
		CreateMap<RequestObjects.Classroom, DAL.Models.Classroom>();
		CreateMap<RabbitMq.JsonDeserializeBuilding, DAL.Models.ShortBuildingInfo>();
	}
}
