using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index(nameof(Name))]
[Index(nameof(Address))]
public class Building
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Address { get; set; }
	public required byte FloorsCount { get; set; }
	public string? Other { get; set; }
	public bool IsDeleted { get; set; }
}
