using System.ComponentModel;

namespace System.Net.Sockets;

[Obsolete("This API supports the .NET Framework infrastructure and is not intended to be used directly from your code.", true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public enum SocketClientAccessPolicyProtocol
{
	Tcp,
	Http
}
