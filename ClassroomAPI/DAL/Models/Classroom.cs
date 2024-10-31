using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index(nameof(Name))]
public class Classroom
{
	public Guid Id { get; set; }
	public required Guid BuildingId { get; set; }
	public required string Name { get; set; }
	public required string Type { get; set; }
	public required int Capacity { get; set; }
	public required int Floor { get; set; }
	public required int Number { get; set; }
	public bool IsDeleted { get; set; }
}
