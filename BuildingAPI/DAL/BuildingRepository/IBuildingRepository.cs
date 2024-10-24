using DAL.Models;

namespace DAL.Repository;

public interface IBuildingRepository
{
	IQueryable<Building> GetAll();
	Task<Building?> Get(Guid id);
	Task<Building> Create(Building model);
	Task<Building?> Update(Building model);
	Task<bool> Delete(Guid id);
}
