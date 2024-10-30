using DAL.Models;

namespace ResponseObjects;

public class Classroom
{
	public Guid Id { get; set; }
	public required ShortBuildingInfo ShortBuildingInfo { get; set; }
	public required string Name { get; set; }
	public required string Type { get; set; }
	public required int Floor { get; set; }
	public required byte Number { get; set; }
	public required int Capacity { get; set; }
}
