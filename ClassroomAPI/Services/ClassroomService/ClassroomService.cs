using DAL.Models;
using DAL.Repository;
using Exceptions;

namespace Service;

public class ClassroomService(IClassroomRepository classroomRepository, IShortBuildingInfoRepository shortBuildingInfoRepository) : IClassroomService
{
	public Task<List<Classroom>> GetAll() => Task.FromResult(classroomRepository.GetAll().ToList());

	public Task<Classroom?> Get(Guid id) => classroomRepository.Get(id);

	public async Task<Classroom> Create(Classroom model)
	{
		bool isBuildingExists = await shortBuildingInfoRepository.Has(model.BuildingId);

		return isBuildingExists ? await classroomRepository.Create(model) : throw new NotExistedBuildingException();
	}

	public Task<Classroom?> Update(Classroom model) => classroomRepository.Update(model);

	public async Task<bool> Delete(Guid id) => await classroomRepository.Delete(id);

	public Task<ShortBuildingInfo?> GetShortBuildingInfo(Guid id) => shortBuildingInfoRepository.Get(id);
}
