using DAL.Models;

namespace Service;

public interface IClassroomService
{
	Task<List<Classroom>> GetAll();
	Task<Classroom?> Get(Guid id);
	Task<Classroom> Create(Classroom model);
	Task<Classroom> Update(Classroom model);
	Task<bool> Delete(Guid id);
	Task<ShortBuildingInfo?> GetShortBuildingInfo(Guid id);
}
