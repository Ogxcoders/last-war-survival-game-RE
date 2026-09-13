namespace System.Net.NetworkInformation;

internal class SystemTcpConnectionInformation : TcpConnectionInformation
{
	private IPEndPoint localEndPoint;

	private IPEndPoint remoteEndPoint;

	private TcpState state;

	public override TcpState State => state;

	public override IPEndPoint LocalEndPoint => localEndPoint;

	public override IPEndPoint RemoteEndPoint => remoteEndPoint;

	public SystemTcpConnectionInformation(IPEndPoint local, IPEndPoint remote, TcpState state)
	{
		localEndPoint = local;
		remoteEndPoint = remote;
		this.state = state;
	}
}
