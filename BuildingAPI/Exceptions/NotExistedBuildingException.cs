namespace Exceptions;

public class NotExistedBuildingException(Guid id) : Exception
{
	public readonly string Id = id.ToString();
}
