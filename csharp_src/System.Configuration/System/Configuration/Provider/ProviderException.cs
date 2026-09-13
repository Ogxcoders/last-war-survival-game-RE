using System.Runtime.Serialization;
using Unity;

namespace System.Configuration.Provider;

[Serializable]
public class ProviderException : Exception
{
	public ProviderException()
	{
		ThrowStub.ThrowNotSupportedException();
	}

	protected ProviderException(SerializationInfo info, StreamingContext context)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ProviderException(string message)
	{
		ThrowStub.ThrowNotSupportedException();
	}

	public ProviderException(string message, Exception innerException)
	{
		ThrowStub.ThrowNotSupportedException();
	}
}
