using GameKit.Base;

public class NetConnectLostState : NetStateBase
{
	public NetConnectLostState(TcpNetManager tcpNetManager)
		: base(NetState.CONNECTLOST, tcpNetManager)
	{
	}

	public override void OnEnter(params object[] param)
	{
		string reason = (string)param[0];
		INetProxy proxy = (INetProxy)param[1];
		_netManager.callOnConnectionLost(reason, proxy);
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
