using System.Collections.Generic;
using System.Net;
using Joker;

namespace MiniGame.Core.Server;

public class GameRoomPingDebugServer : GameRoomServer
{
	private List<int> _pings;

	private float _pingJitter;

	private int _idx;

	public GameRoomPingDebugServer(List<int> pings, float pingJitter)
	{
		_pings = pings;
		_pingJitter = pingJitter;
	}

	protected override NetworkSession CreatePlayerSession(long sessionId, IPEndPoint endPoint)
	{
		int num = 0;
		if (_pings != null && _pings.Count > 0)
		{
			int index = _idx++ % _pings.Count;
			num = _pings[index];
		}
		Log.Info($"PlayerPingDebugSession with ping {num}");
		return new PlayerPingDebugSession(num, _pingJitter, this, base.Network, sessionId, endPoint);
	}
}
