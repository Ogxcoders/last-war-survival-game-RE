using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net;

[Serializable]
public class HttpListenerException : Win32Exception
{
	public override int ErrorCode => base.NativeErrorCode;

	public HttpListenerException()
		: base(Marshal.GetLastWin32Error())
	{
	}

	public HttpListenerException(int errorCode)
		: base(errorCode)
	{
	}

	public HttpListenerException(int errorCode, string message)
		: base(errorCode, message)
	{
	}

	protected HttpListenerException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
