using System.Runtime.InteropServices;

namespace BestHTTP.SignalRCore.Messages;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct PingMessage
{
	public MessageTypes type => MessageTypes.Ping;
}
