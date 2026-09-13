using System;
using System.Collections;
using System.Net;
using Joker;
using MiniGame.Core.Server;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameBiuBiuPlayerPvp : GameBiuBiuPlayerBase
{
	private int _reConnectCount;

	private bool _isReconnecting;

	private const int MaxReconnectAttempts = 3;

	private float ReconnectTimeOut = 5f;

	private Action OnReConnectSuccess;

	private Action OnReConnectFair;

	private int JoinKickCount = 5;

	private int CurJoinKickCount;

	private Coroutine ReConnectCoroutine;

	public string[] Players { get; private set; }

	public GameBiuBiuPlayerPvp(string ip, string room)
		: base(ip, room)
	{
	}

	public override void Startup()
	{
		base.Startup();
		CurJoinKickCount = 0;
		IPEndPoint address = new IPEndPoint(IPAddress.Parse(_ip), _port);
		base.Network.Connect(address);
	}

	public override void Send(object message)
	{
		RoomMessage roomMessage = null;
		if (message is IMessageSync)
		{
			roomMessage = new C2SGameRoomSync();
		}
		else if (message is IMessageEnter messageEnter)
		{
			roomMessage = new C2SGameRoomEnter
			{
				LevelPath = base.LevelPath,
				MaxPlayers = _maxPlayers,
				IsObserver = false,
				LogicType = "GameLogicBiubiuRealtime",
				LogicVersion = base.LogicVersion
			};
			messageEnter.LevelPath = base.LevelPath;
			messageEnter.MaxPlayers = _maxPlayers;
		}
		else
		{
			roomMessage = ((message is IMessageLeave) ? new C2SGameRoomLeave() : ((!(message is IMessageVerify data)) ? (message as RoomMessage) : new C2SGameRoomVerify
			{
				LogicType = "GameLogicBiubiuVerify",
				LogicVersion = base.LogicVersion,
				Data = data
			}));
		}
		base.SendMessage(message, roomMessage);
	}

	protected override void OnMessage(long channel, IMessage message)
	{
		if (message is RoomMessage roomMessage)
		{
			GameBiubiuShare.UnpackMessage(roomMessage.OpCode, roomMessage.Data, out roomMessage.Data);
		}
		if (message is S2CGameRoomStart s2CGameRoomStart)
		{
			base.MultiPlayerState = EGameMultiPlayerState.Prepared;
			Players = s2CGameRoomStart.PlayerSessionIDs.Clone() as string[];
			base.PlayerID = EPlayerID.ID_None;
			for (int i = 0; i < Players.Length; i++)
			{
				if (Players[i] == base.PlayerSessionId)
				{
					base.PlayerID = (EPlayerID)i;
					break;
				}
			}
			base.RoomEnd = false;
		}
		else if (message is S2CGameRoomEnd s2CGameRoomEnd)
		{
			GameBiubiuEndResp gameBiubiuEndResp = s2CGameRoomEnd.Data as GameBiubiuEndResp;
			if (base.GameBiuBiuRunTime.Loader != null && base.GameBiuBiuRunTime.Loader.GameWorld != null)
			{
				SharedRuntime shared = base.GameBiuBiuRunTime.Loader.GameWorld._world.GetShared<SharedRuntime>();
				shared.ServerGameOver = true;
				shared.GameResult.Statistics = gameBiubiuEndResp.Statistics;
				Log.Info($" [BiuBiu]: S2CGameRoomEnd ExceptionCount:{gameBiubiuEndResp.Statistics.ExceptionCount} BattleTimeMills:{gameBiubiuEndResp.Statistics.BattleTimeMills} BulletNum1:{gameBiubiuEndResp.Statistics.BulletNum[0]} BulletNum2:{gameBiubiuEndResp.Statistics.BulletNum[1]} WinPlayerID:{gameBiubiuEndResp.Statistics.WinPlayerID} GameEndFrame{gameBiubiuEndResp.Statistics.GameEndFrame}");
			}
			base.RoomEnd = true;
		}
		else if (message is S2CGameRoomEnter s2CGameRoomEnter)
		{
			Log.Info("[BiuBiu]:FleetID:" + s2CGameRoomEnter.FleetID + " InstanceID:" + s2CGameRoomEnter.InstanceID + " ProcessID:" + s2CGameRoomEnter.ProcessID);
		}
		else
		{
			EnqueueMessage(message, channel);
		}
	}

	protected override void OnDisconnect(long channel)
	{
		base.OnDisconnect(channel);
		if (base.AppPauseState)
		{
			return;
		}
		if (CurJoinKickCount >= JoinKickCount)
		{
			OnReconnectFailed();
			return;
		}
		CurJoinKickCount++;
		if (!base.RoomEnd)
		{
			StopCurReconnectCoroutine();
			ReConnectCoroutine = YieldUtils.StartCoroutine(TryReconnect());
		}
	}

	private IEnumerator TryReconnect()
	{
		if (_reConnectCount > 0 || _isReconnecting || base.GameBiuBiuRunTime == null || base.GameBiuBiuRunTime.Loader == null || base.GameBiuBiuRunTime.Loader.GameWorld == null)
		{
			yield break;
		}
		_reConnectCount = 3;
		IPEndPoint address = new IPEndPoint(IPAddress.Parse(_ip), _port);
		if (base.GameBiuBiuRunTime.Loader != null && base.GameBiuBiuRunTime.Loader.GameWorld != null)
		{
			base.GameBiuBiuRunTime.Loader.GameWorld._world.GetShared<SharedRuntime>().GameResult.Statistics.Retry_Connect_Count++;
		}
		float timeOut = ReconnectTimeOut;
		while (_reConnectCount > 0)
		{
			if (!_isReconnecting)
			{
				ConnectGameLiftServer(address, force: true);
				_isReconnecting = true;
				_reConnectCount--;
				timeOut = ReconnectTimeOut;
			}
			yield return null;
			if (base.IsConnected)
			{
				_isReconnecting = false;
				_reConnectCount = -1;
				SendEnterRoom();
				OnReconnectSuccess();
				yield break;
			}
			timeOut -= Time.deltaTime;
			if (timeOut < 0f)
			{
				_isReconnecting = false;
				if (_reConnectCount <= 0)
				{
					break;
				}
			}
		}
		_isReconnecting = false;
		_reConnectCount = -1;
		OnReconnectFailed();
	}

	private void StopCurReconnectCoroutine()
	{
		_isReconnecting = false;
		_reConnectCount = 0;
		base.IsConnected = false;
		if (ReConnectCoroutine != null)
		{
			YieldUtils.StopCoroutine(ReConnectCoroutine);
			ReConnectCoroutine = null;
		}
	}

	private void OnReconnectSuccess()
	{
		Log.Info("[BiuBiu]:Reconnection succeeded.");
		OnReConnectSuccess?.Invoke();
	}

	private void OnReconnectFailed()
	{
		Log.Info("[BiuBiu]:Reconnection attempts exhausted. Failed to reconnect.");
		OnReConnectFair?.Invoke();
	}

	public override void WifiTimeOut()
	{
		Log.Info("[BiuBiu]:WifiTimeOut");
		OnReConnectFair?.Invoke();
	}

	public void SetupNetWork(Action reSuccess, Action reFair)
	{
		OnReConnectSuccess = reSuccess;
		OnReConnectFair = reFair;
	}

	public override void AppResume()
	{
		base.AppResume();
		Log.Info("Need ReConnect");
		StopCurReconnectCoroutine();
		ReConnectCoroutine = YieldUtils.StartCoroutine(TryReconnect());
	}

	public override void Shutdown()
	{
		base.Shutdown();
		StopCurReconnectCoroutine();
	}
}
