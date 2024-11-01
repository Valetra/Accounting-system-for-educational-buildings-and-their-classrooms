using DAL.Models;

namespace DAL.Repository;

public interface IClassroomRepository
{
	IQueryable<Classroom> GetAll();
	Task<Classroom?> Get(Guid id);
	Task<Classroom> Create(Classroom model);
	Task<Classroom?> Update(Classroom model);
	Task<bool> Delete(Guid id);
	Task<bool> Has(Guid id);
}
