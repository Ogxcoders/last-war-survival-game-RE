using System;
using GameFramework;
using Sfs2X.Core;

public class NetConnectErrorState : NetStateBase
{
	public NetConnectErrorState(TcpNetManager tcpNetManager)
		: base(NetState.CONNECTERROR, tcpNetManager)
	{
	}

	public override void OnEnter(params object[] param)
	{
		BaseEvent obj = (BaseEvent)param[0];
		_ = obj.Params["errorCode"];
		string text = (string)obj.Params["errorMessage"];
		try
		{
			text = text.Split(new char[1] { '\n' })[0];
			text = text.Substring(text.LastIndexOf(':'));
			Log.Error("Connect to server failed:{0}", text);
			if (text.Contains("Network is unreachable"))
			{
				_netManager.OnConnectionEvent?.Invoke("E105", text);
			}
			else
			{
				_netManager.OnConnectionEvent?.Invoke("E101", text);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex.Message);
		}
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
	}
}
