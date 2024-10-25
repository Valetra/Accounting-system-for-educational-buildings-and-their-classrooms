using System.ComponentModel.DataAnnotations;

namespace RequestObjects;

public class Building
{
	[Required]
	public required string Name { get; set; }
	[Required]
	public required string Address { get; set; }
	[Required]
	public required byte FloorsCount { get; set; }
	public string? Other { get; set; }
}
