using System.Runtime.Serialization;
using System.Security;

namespace System.ComponentModel.Composition;

[Serializable]
public class CompositionContractMismatchException : Exception
{
	public CompositionContractMismatchException()
		: this(null, null)
	{
	}

	public CompositionContractMismatchException(string message)
		: this(message, null)
	{
	}

	public CompositionContractMismatchException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[SecuritySafeCritical]
	protected CompositionContractMismatchException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
