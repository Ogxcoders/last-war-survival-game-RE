using System;
using System.Collections;
using Joker;
using UnityEngine;
using XLua;

namespace MiniGame.Biubiu.Client;

public class UIBootPvpConnect
{
	public int ServerID;

	public PvpPlayerRuntime PvpPlayerRuntime;

	private Action<string> OnError;

	private Action OnFinish;

	private float ConnectGameLifgTimeOut = 6f;

	private float ConnectWaitPlayerTimeOut = 6f;

	private float CreatePlayerTimeOut = 6f;

	private float OpTime;

	private Coroutine co;

	public void ConnectGameLift(int serverId, LuaTable data, int maxPlayers, string levelPath, Action<string> onError = null, Action onFinish = null)
	{
		OnError = onError;
		OnFinish = onFinish;
		ServerID = serverId;
		AppBiubiu.InitApp();
		if (co != null)
		{
			YieldUtils.StopCoroutine(co);
			co = null;
		}
		co = YieldUtils.StartCoroutine(Connect(serverId, data, maxPlayers, levelPath));
	}

	private IEnumerator Connect(int serverId, LuaTable data, int maxPlayers, string levelPath)
	{
		string uid = data.Get<string>("uid");
		string uuid = data.Get<string>("uuid");
		string ip = data.Get<string>("ip");
		string port = data.Get<string>("port");
		string sid = data.Get<string>("sid");
		string playerName = data.Get<string>("playname");
		string playerSessionId = data.Get<string>("sessionidplayer");
		string sessionGame = data.Get<string>("sessionidgame");
		int p = data.Get<int>("p");
		string logicVersion = data.Get<string>("logicversion");
		string resVersion = data.Get<string>("resversion");
		bool reenter = data.Get<bool>("reconnect");
		float enterRoomTime = data.Get<float>("time");
		float time = Time.realtimeSinceStartup;
		Singleton<WorldService>.Instance.Create<GameBiuBiuPlayerPvp>(ESchedulerType.Main, -1, playerName, new object[2]
		{
			ip + ":" + port,
			sessionGame
		});
		GameBiuBiuPlayerPvp player = null;
		OpTime = CreatePlayerTimeOut;
		while (player == null)
		{
			yield return null;
			player = Singleton<WorldService>.Instance.GetWorld(playerName) as GameBiuBiuPlayerPvp;
			if (player != null)
			{
				PvpPlayerRuntime = new PvpPlayerRuntime
				{
					Player = player
				};
			}
			OpTime -= Time.unscaledDeltaTime;
			if (OpTime < 0f)
			{
				OnError?.Invoke("FIGHT_CREATE_PLAYER_FAIR");
				yield break;
			}
		}
		OpTime = ConnectGameLifgTimeOut;
		while (!player.IsConnected)
		{
			yield return null;
			OpTime -= Time.unscaledDeltaTime;
			if (OpTime < 0f)
			{
				OnError?.Invoke("FIGHT_CONNECT_GAME_LIFT");
				yield break;
			}
		}
		Log.Info("[BiuBiu] Connect GameLift Time: " + (Time.realtimeSinceStartup - time));
		player.SetUpVersion(logicVersion, resVersion);
		player.SetUpPlayer(serverId, uid, playerName, playerSessionId, sessionGame, (EPlayerID)p);
		player.SetupRoom(levelPath, string.Empty, sid, uuid, maxPlayers, reenter ? (-1f) : (Time.realtimeSinceStartup - enterRoomTime));
		if (reenter)
		{
			player.SetIsReEnter();
		}
		PvpPlayerRuntime.Prepare();
		Log.Info("[BiuBiu]:" + ip + ":" + port + ":" + playerSessionId + ":" + sessionGame);
		bool flag = false;
		OpTime = ConnectWaitPlayerTimeOut;
		while (!flag)
		{
			yield return null;
			flag = PvpPlayerRuntime.IsPrepared();
			if (!flag)
			{
				OpTime -= Time.unscaledDeltaTime;
				if (OpTime < 0f)
				{
					OnError?.Invoke("FIGHT_WAIT_PLAYER");
					yield break;
				}
			}
		}
		OnFinish?.Invoke();
	}

	public int ReEnter()
	{
		if (PvpPlayerRuntime != null && PvpPlayerRuntime.Player != null)
		{
			if (!PvpPlayerRuntime.Player.ReEnter)
			{
				return 0;
			}
			return 1;
		}
		return -1;
	}

	public void Dispose()
	{
		if (PvpPlayerRuntime != null)
		{
			PvpPlayerRuntime.Dispose();
			PvpPlayerRuntime = null;
		}
		if (co != null)
		{
			YieldUtils.StopCoroutine(co);
			co = null;
		}
		OnFinish = null;
		OnError = null;
		ServerID = -1;
	}
}
