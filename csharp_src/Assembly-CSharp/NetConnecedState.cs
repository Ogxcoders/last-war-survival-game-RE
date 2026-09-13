using GameFramework;
using GameKit.Base;

public class NetConnecedState : NetStateBase
{
	private INetProxy curLine;

	public NetConnecedState(TcpNetManager tcpNetManager)
		: base(NetState.CONNECTED, tcpNetManager)
	{
	}

	public override void OnEnter(params object[] param)
	{
		curLine = (INetProxy)param[0];
		_netManager.OnConnectionEvent?.Invoke("E000", null);
	}

	public override void OnExit()
	{
		curLine.Disconnect();
		curLine = null;
	}

	public override void OnUpdate()
	{
		curLine.UpdateSmartFoxClient();
	}

	public override string getCurlineName()
	{
		return curLine.proxyName;
	}

	public override void OnConnectionLost(string reason, INetProxy proxy)
	{
		Log.Error("Connection {0} was lost, Reason: {1} cur line {2}", proxy.proxyName, reason, getCurlineName());
		if (curLine != null && !curLine.proxyName.Equals(proxy.proxyName))
		{
			Log.Info("line {0} lost not cur {1} line do nothing", proxy.proxyName, getCurlineName());
			return;
		}
		_netManager.changeState(NetState.CONNECTLOST, reason, proxy);
	}
}
