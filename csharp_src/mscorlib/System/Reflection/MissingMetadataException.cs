namespace System.Reflection;

public sealed class MissingMetadataException : TypeAccessException
{
	public MissingMetadataException()
	{
	}

	public MissingMetadataException(string message)
		: base(message)
	{
	}
}
