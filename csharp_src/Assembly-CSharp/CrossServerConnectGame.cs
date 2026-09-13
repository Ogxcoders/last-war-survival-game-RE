using GameFramework;
using ProtoBufNet;
using UnityEngine;

public class CrossServerConnectGame : FsmBaseState
{
	private float _timeout = 6f;

	private float _elapseTime;

	public override int id => 3;

	public CrossServerConnectGame(CrossServerFsmManager mgr)
		: base(mgr)
	{
	}

	public override void OnEnter(params object[] args)
	{
		_elapseTime = 0f;
		string[] array = ((string)args[0]).Split(new char[1] { '|' });
		GameEntry.Network.GameServerUrlList = array;
		if (array.Length == 0)
		{
			Log.Error("ConnectGameState::OnEnter GameServerUrlList length 0 ！");
		}
		else
		{
			GameEntry.Network.GameServerUrl = array[0];
		}
		Log.Info("[Net] [cross] switch to main line,disconnect cur line");
		if (NetPacketConst.useForwardServerId)
		{
			GameEntry.Network.GameServerPort = (int)args[1];
			GameEntry.Network.ZoneName = (string)args[2];
			GameEntry.Network.Uid = (string)args[3];
			Log.Info("[AT]SetNetUID_CrossConEnter1:" + GameEntry.Network.Uid);
			GameEntry.Network.GameServerConnectionType = (int)args[4];
			OnGameConnection("E000", "");
			_mgr.crossing = true;
			if (GameEntry.Network.ZoneName.StartsWith("APS"))
			{
				GameEntry.Network.ZoneName.Substring(3);
			}
			return;
		}
		Log.Info("[Net] [cross] switch to main line,disconnect cur line");
		GameEntry.Network.Disconnect();
		GameEntry.Network.GameServerPort = (int)args[1];
		GameEntry.Network.ZoneName = (string)args[2];
		GameEntry.Network.Uid = (string)args[3];
		Log.Info("[AT]SetNetUID_CrossConEnter2:" + GameEntry.Network.Uid);
		GameEntry.Network.GameServerConnectionType = (int)args[4];
		GameEntry.Network.OnConnectionEvent = OnGameConnection;
		Log.Info($"[Net] [cross] ConnectGameState::{GameEntry.Network.GameServerUrl},port:{GameEntry.Network.GameServerPort},zone:{GameEntry.Network.ZoneName},uid:{GameEntry.Network.Uid}");
		GameEntry.Network.Connect();
		_mgr.crossing = true;
		string text = GameEntry.Network.ZoneName;
		if (text.StartsWith("APS"))
		{
			text = GameEntry.Network.ZoneName.Substring(3);
		}
		CrossServerUtil.ShowCloud(text);
	}

	private void OnGameConnection(string err, string errorMessage)
	{
		if (string.IsNullOrEmpty(err) || err == "E000")
		{
			_mgr.SetState(4);
		}
		else
		{
			GotoLoadingError(7);
		}
	}

	public override void OnExit()
	{
	}

	public override void OnUpdate()
	{
		_elapseTime += Time.deltaTime;
		if (_elapseTime > _timeout)
		{
			GotoLoadingError(8);
		}
	}
}
