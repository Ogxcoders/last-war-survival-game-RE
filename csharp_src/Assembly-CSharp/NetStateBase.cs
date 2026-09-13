using GameFramework;
using GameKit.Base;
using Sfs2X.Core;

public abstract class NetStateBase
{
	protected TcpNetManager _netManager;

	private NetState _state;

	public NetStateBase(NetState netState, TcpNetManager tcpNetManager)
	{
		_netManager = tcpNetManager;
		_state = netState;
	}

	public abstract void OnEnter(params object[] param);

	public abstract void OnExit();

	public abstract void OnUpdate();

	public NetState getState()
	{
		return _state;
	}

	public virtual string getCurlineName()
	{
		return "none";
	}

	public virtual bool OnConnection(INetProxy proxy, BaseEvent e)
	{
		Log.Error("error state {0} call OnConnection", _state);
		return false;
	}

	public virtual void OnConnectionLost(string reason, INetProxy proxy)
	{
		Log.Error("error state {0} call OnConnectionLost", _state);
	}
}
