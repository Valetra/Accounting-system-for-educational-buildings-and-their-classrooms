using DAL.Models;
using DAL.Repository;

namespace Service;

public class ClassroomService(IClassroomRepository classroomRepository, IShortBuildingInfoRepository shortBuildingInfoRepository) : IClassroomService
{
	public Task<List<Classroom>> GetAll() => Task.FromResult(classroomRepository.GetAll().ToList());

	public Task<Classroom?> Get(Guid id) => classroomRepository.Get(id);

	public Task<Classroom> Create(Classroom model) => classroomRepository.Create(model);

	public Task<Classroom?> Update(Classroom model) => classroomRepository.Update(model);

	public async Task<bool> Delete(Guid id) => await classroomRepository.Delete(id);

	public Task<ShortBuildingInfo?> GetShortBuildingInfo(Guid id) => shortBuildingInfoRepository.Get(id);
}
