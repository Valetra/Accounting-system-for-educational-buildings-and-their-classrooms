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

	public async Task<Classroom> Update(Classroom model)
	{
		Classroom? existingClassroom = await classroomRepository.Get(model.Id)
			?? throw new NotExistedClassroomException();

		bool isBuildingExists = await shortBuildingInfoRepository.Has(model.BuildingId);

		if (!isBuildingExists)
		{
			throw new NotExistedBuildingException();
		}

		await classroomRepository.Update(model);

		return existingClassroom;
	}

	public async Task<bool> Delete(Guid id) => await classroomRepository.Delete(id);

	public Task<ShortBuildingInfo?> GetShortBuildingInfo(Guid id) => shortBuildingInfoRepository.Get(id);
}
