namespace MessageContracts;

public class Building
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Address { get; set; } = "";
	public int FloorsCount { get; set; }
	public string? Other { get; set; }
	public bool IsDeleted { get; set; }
}
