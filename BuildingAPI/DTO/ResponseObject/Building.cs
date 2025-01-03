namespace ResponseObjects;

public class Building
{
	public Guid Id { get; set; }
	public required string Name { get; set; }
	public required string Address { get; set; }
	public required int FloorsCount { get; set; }
	public string? Other { get; set; }
}
