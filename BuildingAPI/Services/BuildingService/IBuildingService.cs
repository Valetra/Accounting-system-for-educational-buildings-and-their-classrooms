using DAL.Models;

namespace Service;

public interface IBuildingService
{
	Task<List<Building>> GetAll();
	Task<Building?> Get(Guid id);
	Task<Building> Create(Building model);
	Task<Building?> Update(Building model);
	Task<bool> Delete(Guid id);
}
