using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index(nameof(Name))]
[Index(nameof(Address))]
public class ShortBuildingInfo
{
	public required Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Address { get; set; }
	public bool IsDeleted { get; set; }
}
