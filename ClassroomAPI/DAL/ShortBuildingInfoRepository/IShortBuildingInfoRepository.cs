using DAL.Models;

namespace DAL.Repository;

public interface IShortBuildingInfoRepository
{
	IQueryable<ShortBuildingInfo> GetAll();
	Task<ShortBuildingInfo?> Get(Guid id);
	Task<ShortBuildingInfo> Create(ShortBuildingInfo model);
	Task<ShortBuildingInfo?> Update(ShortBuildingInfo model);
	Task<bool> Delete(Guid id);
	Task<bool> Has(Guid id);
}
