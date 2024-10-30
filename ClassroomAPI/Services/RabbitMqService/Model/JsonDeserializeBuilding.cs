namespace RabbitMq;

public class JsonDeserializeBuilding
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";
	public string Address { get; set; } = "";
	public byte FloorsCount { get; set; }
	public string? Other { get; set; }
	public bool IsDeleted { get; set; }
}
