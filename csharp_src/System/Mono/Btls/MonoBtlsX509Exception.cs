using System;

namespace Mono.Btls;

internal class MonoBtlsX509Exception : Exception
{
	public MonoBtlsX509Error ErrorCode { get; private set; }

	public string ErrorMessage { get; private set; }

	public MonoBtlsX509Exception(MonoBtlsX509Error code, string message)
		: base(message)
	{
		ErrorCode = code;
		ErrorMessage = message;
	}

	public override string ToString()
	{
		return $"[MonoBtlsX509Exception: ErrorCode={ErrorCode}, ErrorMessage={ErrorMessage}]";
	}
}
