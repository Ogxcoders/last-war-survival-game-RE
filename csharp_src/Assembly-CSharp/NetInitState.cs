public class NetInitState : NetStateBase
{
	public NetInitState(TcpNetManager tcpNetManager)
		: base(NetState.INIT, tcpNetManager)
	{
	}

	public override void OnEnter(params object[] param)
	{
		_netManager.changeState(NetState.CHOOSELINE);
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
