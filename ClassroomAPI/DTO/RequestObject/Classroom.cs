using System.ComponentModel.DataAnnotations;

namespace RequestObjects;

public class Classroom
{
	[Required]
	public required Guid BuildingId { get; set; }
	[Required]
	public required string Name { get; set; }
	[AllowedValues(["Lecture", "ForPractice", "Gym", "Other"])]
	[Required]
	public required string Type { get; set; }
	[Required]
	public required int Floor { get; set; }
	[Required]
	public required int Number { get; set; }
	public required int Capacity { get; set; }
}
