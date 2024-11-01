using Microsoft.EntityFrameworkCore;

using DAL.Models;
using DAL.Contexts;

namespace DAL.Repository;

public class ShortBuildingInfoRepository(ClassroomContext context) : IShortBuildingInfoRepository
{
	protected readonly DbContext Context = context;
	protected readonly DbSet<ShortBuildingInfo> Entities = context.Set<ShortBuildingInfo>();

	public IQueryable<ShortBuildingInfo> GetAll() => Entities.Where(e => !e.IsDeleted);

	public Task<ShortBuildingInfo?> Get(Guid id) => Entities.FirstOrDefaultAsync(m => Equals(m.Id, id) && !m.IsDeleted);

	public async Task<ShortBuildingInfo> Create(ShortBuildingInfo model)
	{
		await Entities.AddAsync(model);
		await Context.SaveChangesAsync();

		return model;
	}

	public async Task<ShortBuildingInfo?> Update(ShortBuildingInfo model)
	{
		ShortBuildingInfo? toUpdate = await Entities.AsNoTracking().FirstOrDefaultAsync(m => Equals(m.Id, model.Id));

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
		ShortBuildingInfo? toDelete = await Entities.FirstOrDefaultAsync(m => Equals(m.Id, id));

		if (toDelete is null || toDelete.IsDeleted)
		{
			return false;
		}

		toDelete.IsDeleted = true;
		Context.Update(toDelete);

		await Context.SaveChangesAsync();

		return true;
	}

	public async Task<bool> Has(Guid id) => await Entities.AnyAsync(m => Equals(m.Id, id) && !m.IsDeleted);
}
