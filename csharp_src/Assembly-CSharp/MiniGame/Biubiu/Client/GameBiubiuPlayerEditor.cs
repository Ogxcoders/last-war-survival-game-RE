using System;
using System.Net;
using Joker;
using MiniGame.Core.Server;

namespace MiniGame.Biubiu.Client;

public class GameBiubiuPlayerEditor : GameBiuBiuPlayerBase
{
	public string[] Players { get; private set; }

	public GameBiubiuPlayerEditor(string ip, string room)
		: base(ip, room)
	{
	}

	public override void Startup()
	{
		base.PlayerSessionId = base.Name + "  " + DateTime.Now.ToString("MM-dd_HH-mm-ss");
		base.Startup();
		IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(_ip), _port);
		Log.Info(iPEndPoint.ToString());
		base.Network.Connect(iPEndPoint);
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
		else
		{
			EnqueueMessage(message, channel);
		}
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
				LogicVersion = "",
				ResourceVersion = "",
				UID = base.PlayerSessionId,
				PlayerName = base.PlayerSessionId
			};
			messageEnter.LevelPath = base.LevelPath;
			messageEnter.MaxPlayers = _maxPlayers;
		}
		else if (message is IMessageLeave)
		{
			roomMessage = new C2SGameRoomLeave
			{
				RoomSessionID = _room,
				SID = "",
				UUID = ""
			};
		}
		else if (message is IMessageVerify data)
		{
			base.Sid = Guid.NewGuid().ToString();
			base.Uid = Guid.NewGuid().ToString();
			base.UUID = Guid.NewGuid().ToString();
			roomMessage = new C2SGameRoomVerify
			{
				LogicType = "GameLogicBiubiuVerify",
				LogicVersion = "",
				ResourceVersion = "",
				Data = data
			};
		}
		else
		{
			roomMessage = message as RoomMessage;
		}
		base.SendMessage(message, roomMessage);
	}
}
