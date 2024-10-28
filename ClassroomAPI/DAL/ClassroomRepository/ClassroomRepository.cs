using Microsoft.EntityFrameworkCore;

using DAL.Models;
using DAL.Contexts;

namespace DAL.Repository;

public class ClassroomRepository(ClassroomContext context) : IClassroomRepository
{
	protected readonly DbContext Context = context;
	protected readonly DbSet<Classroom> Entities = context.Set<Classroom>();

	public IQueryable<Classroom> GetAll() => Entities.Where(e => !e.IsDeleted);

	public Task<Classroom?> Get(Guid id) => Entities.FirstOrDefaultAsync(m => Equals(m.Id, id) && !m.IsDeleted);

	public async Task<Classroom> Create(Classroom model)
	{
		await Entities.AddAsync(model);
		await Context.SaveChangesAsync();

		return model;
	}

	public async Task<Classroom?> Update(Classroom model)
	{
		Classroom? toUpdate = await Entities.AsNoTracking().FirstOrDefaultAsync(m => Equals(m.Id, model.Id));

		if (toUpdate is null || toUpdate.IsDeleted)
		{
			return null;
		}

		Context.Update(model);
		await Context.SaveChangesAsync();

		return model;
	}

	public async Task<bool> Delete(Guid id)
	{
		Classroom? toDelete = await Entities.FirstOrDefaultAsync(m => Equals(m.Id, id));

		if (toDelete is null || toDelete.IsDeleted)
		{
			return false;
		}

		toDelete.IsDeleted = true;
		Context.Update(toDelete);

		await Context.SaveChangesAsync();

		return true;
	}
}
