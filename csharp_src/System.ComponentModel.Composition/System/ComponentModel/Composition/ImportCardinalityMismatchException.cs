using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security;

namespace System.ComponentModel.Composition;

[Serializable]
[DebuggerTypeProxy(typeof(ImportCardinalityMismatchExceptionDebuggerProxy))]
[DebuggerDisplay("{Message}")]
public class ImportCardinalityMismatchException : Exception
{
	public ImportCardinalityMismatchException()
		: this(null, null)
	{
	}

	public ImportCardinalityMismatchException(string message)
		: this(message, null)
	{
	}

	public ImportCardinalityMismatchException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	[SecuritySafeCritical]
	protected ImportCardinalityMismatchException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
