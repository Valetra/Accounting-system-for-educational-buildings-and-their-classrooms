namespace Exceptions;

public class NotExistedBuildingException(Guid id) : Exception
{

	public override string Message => $"Building with id = `{id}` does not exists.";
}
