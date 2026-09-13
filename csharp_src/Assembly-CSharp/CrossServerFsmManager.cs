using System.Collections.Generic;
using GameFramework;
using ProtoBufNet;

public class CrossServerFsmManager
{
	private class GSLResult
	{
		public int port;

		public string url;

		public string ZoneName;

		public int connectionType;
	}

	private FsmBaseState _state;

	private Dictionary<int, FsmBaseState> _stateMap = new Dictionary<int, FsmBaseState>();

	public bool crossing;

	private GSLResult _gslResult;

	public void Start()
	{
		crossing = false;
		_stateMap = new Dictionary<int, FsmBaseState>();
		if (NetPacketConst.useForwardServerId)
		{
			RegisterState(new CrossServerBeforeState(this));
			RegisterState(new CrossServerCheckResVersion(this));
			RegisterState(new CrossServerGetServerList(this));
			RegisterState(new CrossServerConnectGame(this));
			RegisterState(new CrossServerLogin(this));
			RegisterState(new CrossServerPushInit(this));
			RegisterState(new CrossServerAfterCross(this));
			RegisterState(new CrossServerLoadingError(this));
			SetState(0);
		}
		else
		{
			RegisterState(new CrossServerCheckResVersion(this));
			RegisterState(new CrossServerGetServerList(this));
			RegisterState(new CrossServerConnectGame(this));
			RegisterState(new CrossServerLogin(this));
			RegisterState(new CrossServerPushInit(this));
			RegisterState(new CrossServerAfterCross(this));
			RegisterState(new CrossServerLoadingError(this));
			SetState(1);
		}
	}

	private void RegisterState(FsmBaseState state)
	{
		_stateMap.Add(state.id, state);
	}

	public void SetState(int stateId, params object[] arg)
	{
		if (_stateMap.TryGetValue(stateId, out var value))
		{
			Log.Info("[Cross Loading] " + _state?.GetType()?.Name + " -> " + value.GetType().Name);
			_state?.OnExit();
			_state = value;
			_state.OnEnter(arg);
		}
		else
		{
			Log.Error($"[Cross Loading] {stateId} not found");
		}
	}

	public void Update()
	{
		_state?.OnUpdate();
	}

	public void Dispose()
	{
		_state?.OnExit();
		_state = null;
		_gslResult = null;
	}

	public bool IsCrossingOver()
	{
		return !crossing;
	}

	public void SyncServerListResult()
	{
		_gslResult = new GSLResult
		{
			port = GameEntry.Network.GameServerPort,
			url = GameEntry.Network.GameServerUrl,
			ZoneName = GameEntry.Network.ZoneName,
			connectionType = GameEntry.Network.GameServerConnectionType
		};
	}

	public bool TryGetGSLResult(out string url, out int port, out string zoneName, out int connectionType)
	{
		if (_gslResult == null)
		{
			url = null;
			port = 0;
			zoneName = null;
			connectionType = 0;
			return false;
		}
		url = _gslResult.url;
		port = _gslResult.port;
		zoneName = _gslResult.ZoneName;
		connectionType = _gslResult.connectionType;
		return true;
	}
}
