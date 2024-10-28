using DAL.Models;
using DAL.Repository;

namespace Service;

public class BuildingService(BuildingRepository studyBuildingRepository) : IBuildingService
{
	public Task<List<Building>> GetAll() => Task.FromResult(studyBuildingRepository.GetAll().ToList());

	public Task<Building?> Get(Guid id) => studyBuildingRepository.Get(id);

	public Task<Building> Create(Building model) => studyBuildingRepository.Create(model);

	public Task<Building?> Update(Building model) => studyBuildingRepository.Update(model);

	public async Task<bool> Delete(Guid id) => await studyBuildingRepository.Delete(id);
}
