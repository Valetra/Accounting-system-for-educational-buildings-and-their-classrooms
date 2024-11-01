namespace Exceptions;

public class NotExistedBuildingException(Guid id) : Exception
{
	public readonly Guid BuildingId = id;
}
