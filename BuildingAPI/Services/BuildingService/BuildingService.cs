using DAL.Models;
using DAL.Repository;

namespace Service;

public class BuildingService(IBuildingRepository buildingRepository) : IBuildingService
{
	public Task<List<Building>> GetAll() => Task.FromResult(buildingRepository.GetAll().ToList());

	public Task<Building?> Get(Guid id) => buildingRepository.Get(id);

	public Task<Building> Create(Building model) => buildingRepository.Create(model);

	public Task<Building?> Update(Building model) => buildingRepository.Update(model);

	public async Task<bool> Delete(Guid id) => await buildingRepository.Delete(id);
}
