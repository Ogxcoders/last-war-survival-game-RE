namespace System.Runtime.Serialization;

[Serializable]
public class InvalidDataContractException : Exception
{
	public InvalidDataContractException()
	{
	}

	public InvalidDataContractException(string message)
		: base(message)
	{
	}

	public InvalidDataContractException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected InvalidDataContractException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
