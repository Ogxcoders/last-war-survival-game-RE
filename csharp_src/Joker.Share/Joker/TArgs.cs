using System.Net.Sockets;

namespace Joker;

public struct TArgs
{
	public TcpOp Op;

	public long ChannelId;

	public SocketAsyncEventArgs SocketAsyncEventArgs;
}
