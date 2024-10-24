using Microsoft.EntityFrameworkCore;

using DAL.Models;
using DAL.Contexts;

namespace DAL.Repository;

public class BuildingRepository(BuildingContext context) : IBuildingRepository
{
	protected readonly DbContext Context = context;
	protected readonly DbSet<Building> Entities = context.Set<Building>();

	public IQueryable<Building> GetAll() => Entities.Where(e => !e.IsDeleted);

	public Task<Building?> Get(Guid id) => Entities.FirstOrDefaultAsync(m => Equals(m.Id, id) && !m.IsDeleted);

	public async Task<Building> Create(Building model)
	{
		await Entities.AddAsync(model);
		await Context.SaveChangesAsync();

		return model;
	}

	public async Task<Building?> Update(Building model)
	{
		Building? toUpdate = await Entities.AsNoTracking().FirstOrDefaultAsync(m => Equals(m.Id, model.Id));

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
		Building? toDelete = await Entities.FirstOrDefaultAsync(m => Equals(m.Id, id));

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
