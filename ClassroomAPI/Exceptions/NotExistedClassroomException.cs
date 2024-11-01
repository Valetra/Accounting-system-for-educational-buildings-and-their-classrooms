namespace Exceptions;

public class NotExistedClassroomException(Guid id) : Exception
{
	public readonly Guid ClassroomId = id;
}
