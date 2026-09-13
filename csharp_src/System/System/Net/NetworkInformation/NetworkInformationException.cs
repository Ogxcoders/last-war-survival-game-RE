using System.ComponentModel;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Net.NetworkInformation;

[Serializable]
public class NetworkInformationException : Win32Exception
{
	public override int ErrorCode => base.NativeErrorCode;

	public NetworkInformationException()
		: base(Marshal.GetLastWin32Error())
	{
	}

	public NetworkInformationException(int errorCode)
		: base(errorCode)
	{
	}

	internal NetworkInformationException(SocketError socketError)
		: base((int)socketError)
	{
	}

	protected NetworkInformationException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}
}
