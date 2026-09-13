using System;
using System.Collections.Generic;
using System.Net;
using Joker;

namespace MiniGame.Core.Server;

public class GameRoomServer : World, IGameServer, IStartupSync, IShutdownSync, IUpdate, ILateUpdate
{
	private Dictionary<PlayerSession, string> _sessionToId = new Dictionary<PlayerSession, string>();

	private Dictionary<string, PlayerSession> _idToSession = new Dictionary<string, PlayerSession>();

	private Dictionary<string, GameRoom> _rooms = new Dictionary<string, GameRoom>();

	private ILogger Log;

	public int PortRangeMin { get; set; } = 7758;

	public int PortRangeMax { get; set; } = 8758;

	public NetworkSessionSystem Network { get; private set; }

	public int Port { get; private set; } = -1;

	public void Startup()
	{
		Log = Joker.Log.GetLogger<GameRoomServer>();
		if (GameServerProcessLock.SafeRunWithDefault(GetType().Name, InitNetwork, def: false))
		{
			GameRoomSessionService.Instance.ProcessReady(Port);
			return;
		}
		Log.Error("Startup failed because InitNetwork failed.");
		App.Exit();
	}

	private bool InitNetwork()
	{
		Log.Info("Game room server starting...");
		Port = -1;
		for (int i = PortRangeMin; i < PortRangeMax; i++)
		{
			if (!NetworkSystem.IsPortUsed(i))
			{
				Port = i;
				break;
			}
		}
		if (Port <= 0)
		{
			Log.Error("Game room can started failed because the port is invalid.");
			App.Exit();
			return false;
		}
		Network = AddSystem<NetworkSessionSystem, TCreateNetworkSession, int>(CreatePlayerSession, Port);
		Log.Info("Game room server start with port: " + Port);
		return true;
	}

	public void Shutdown()
	{
		foreach (KeyValuePair<string, GameRoom> room in _rooms)
		{
			room.Value.Dispose();
		}
		_rooms.Clear();
		Log.Info("Game room server shutting down...");
	}

	public void Update()
	{
		using (ProfilerService.Instance?.CreateSample("GameRoomServer"))
		{
			(IMessage, object) message;
			while (TryDequeueMessage(out message))
			{
				try
				{
					HandleMessage(message.Item1, message.Item2 as NetworkSession);
				}
				catch (Exception e)
				{
					Log.Exception(e);
				}
			}
		}
	}

	public void LateUpdate()
	{
		using (ProfilerService.Instance?.CreateSample("GameRoomServer"))
		{
			float deltaTime = Singleton<TimeService>.Instance.DeltaTime;
			foreach (KeyValuePair<string, GameRoom> room in _rooms)
			{
				try
				{
					room.Value.Update(deltaTime);
				}
				catch (Exception ex)
				{
					Log.Error(ex.ToString());
				}
			}
		}
	}

	public void ConnectPlayer(PlayerSession session)
	{
		Log.Info("ConnectPlayer session:" + session.PlayerSessionID + " sid:" + session.SID + " uuid:" + session.UUID + " uid:" + session.UID);
		_sessionToId.Add(session, null);
	}

	public void LoginPlayer(PlayerSession session)
	{
		Log.Info("LoginPlayer session:" + session.PlayerSessionID + " sid:" + session.SID + " uuid:" + session.UUID + " uid:" + session.UID);
		if (_idToSession.ContainsKey(session.PlayerSessionID))
		{
			Log.Error("LoginPlayer session:" + session.PlayerSessionID + " already exists.");
			session.Dispose();
		}
		else
		{
			_sessionToId[session] = session.PlayerSessionID;
			_idToSession.Add(session.PlayerSessionID, session);
		}
	}

	public void LogoutPlayer(PlayerSession session)
	{
		if (_sessionToId.ContainsKey(session))
		{
			Log.Info("LogoutPlayer session:" + session.PlayerSessionID + " sid:" + session.SID + " uuid:" + session.UUID + " uid:" + session.UID);
			LeaveRoom(session);
			_sessionToId.Remove(session);
			_idToSession.Remove(session.PlayerSessionID);
		}
	}

	public void DisconnectPlayer(PlayerSession session)
	{
		Log.Info("DisconnectPlayer session:" + session.PlayerSessionID + " sid:" + session.SID + " uuid:" + session.UUID + " uid:" + session.UID);
		LeaveRoom(session);
		_sessionToId.Remove(session);
	}

	private PlayerSession GetPlayer(string sessionId)
	{
		return _idToSession[sessionId];
	}

	private void EnterRoom(PlayerSession session, C2SGameRoomEnter msg)
	{
		if (!GameRoomSessionService.Instance.AcceptPlayerSession(msg.PlayerSessionID))
		{
			Log.Warning("GameRoom can not accept player session: " + msg.PlayerSessionID);
			Log.Warning("Kicked invalid player session: " + msg.PlayerSessionID);
			session.Dispose();
			return;
		}
		TryAddRoomByEnter(msg, session);
		if (TryGetRoom(msg.RoomSessionID, out var room))
		{
			room.AddPlayer(msg, session);
		}
		else
		{
			Log.Error("EnterRoom not exists " + msg.RoomSessionID + " player:" + session.PlayerSessionID);
		}
	}

	private void LeaveRoom(PlayerSession session, C2SGameRoomLeave msg = null)
	{
		GameRoom room = null;
		if (!string.IsNullOrEmpty(session.RoomSessionID) && !TryGetRoom(session.RoomSessionID, out room))
		{
			Log.Error("LeaveRoom not exists " + session.RoomSessionID + " player:" + session.PlayerSessionID);
		}
		else if (room != null && room.HasPlayer(session))
		{
			if (msg == null)
			{
				msg = new C2SGameRoomLeave
				{
					SID = room.SID,
					UUID = room.UUID,
					PlayerSessionID = session.PlayerSessionID,
					RoomSessionID = room.RoomSessionID
				};
			}
			room.DelPlayer(msg, session);
		}
	}

	private bool TryGetPlayer(string sessionId, out PlayerSession session)
	{
		return _idToSession.TryGetValue(sessionId, out session);
	}

	public bool TryGetRoom(string roomID, out GameRoom room)
	{
		return _rooms.TryGetValue(roomID, out room);
	}

	public void TryAddRoomByEnter(C2SGameRoomEnter msg, NetworkSession networkSession)
	{
		if (!_rooms.ContainsKey(msg.RoomSessionID))
		{
			C2SGameRoomCreate c2SGameRoomCreate = new C2SGameRoomCreate();
			c2SGameRoomCreate.SID = msg.SID;
			c2SGameRoomCreate.UUID = msg.UUID;
			c2SGameRoomCreate.RoomSessionID = msg.RoomSessionID;
			c2SGameRoomCreate.PlayerSessionID = msg.PlayerSessionID;
			c2SGameRoomCreate.LogicType = msg.LogicType;
			c2SGameRoomCreate.LogicVersion = msg.LogicVersion;
			c2SGameRoomCreate.ResourceVersion = msg.ResourceVersion;
			AddRoom(c2SGameRoomCreate, networkSession);
		}
	}

	private void AddRoom(C2SGameRoomCreate msg, NetworkSession session)
	{
		if (_rooms.ContainsKey(msg.RoomSessionID))
		{
			Log.Error("AddRoom already exists " + msg.RoomSessionID);
			return;
		}
		Log.Info("AddRoom session:" + msg.RoomSessionID + " sid:" + msg.SID + " uuid:" + msg.UUID + " level:" + msg.LevelPath);
		Dictionary<string, string> dictionary = GameRoomSessionService.Instance.ActivateGameSession(msg.RoomSessionID);
		GameRoom gameRoom = CreateRoom(msg, session);
		if (dictionary != null)
		{
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				gameRoom.RoomProperties[item.Key] = item.Value;
			}
		}
		_rooms.Add(gameRoom.RoomSessionID, gameRoom);
		Log.Info($"AddRoom rooms count {_rooms.Count}");
	}

	private void DelRoom(C2SGameRoomDestroy msg, NetworkSession session)
	{
		if (!_rooms.TryGetValue(msg.RoomSessionID, out var value))
		{
			Log.Error("Room not found " + msg.RoomSessionID);
			return;
		}
		Log.Info("DelRoom session:" + msg.RoomSessionID + " sid:" + msg.SID + " uuid:" + msg.UUID);
		_rooms.Remove(msg.RoomSessionID);
		value.Dispose();
		GameRoomSessionService.Instance.TerminateGameSession(msg.RoomSessionID);
		Log.Info($"DelRoom rooms count {_rooms.Count}");
	}

	private void VerifyRoom(C2SGameRoomVerify msg, NetworkSession session)
	{
		if (string.IsNullOrEmpty(msg.RoomSessionID) || string.IsNullOrEmpty(msg.SID) || string.IsNullOrEmpty(msg.UUID))
		{
			Log.Error("Room ID and Session ID can't be null or empty");
			return;
		}
		if (_rooms.ContainsKey(msg.RoomSessionID))
		{
			Log.Error("VerifyRoom already exists " + msg.RoomSessionID);
			return;
		}
		C2SGameRoomCreate msg2 = new C2SGameRoomCreate
		{
			RoomSessionID = msg.RoomSessionID,
			SID = msg.SID,
			UUID = msg.UUID
		};
		GameRoom gameRoom = CreateRoom(msg2, session);
		_rooms.Add(gameRoom.RoomSessionID, gameRoom);
		if (session == null)
		{
			Log.Error("VerifyRoom session is null");
		}
		else
		{
			gameRoom.VerifyGame(msg, session);
		}
	}

	public virtual GameRoom CreateRoom(C2SGameRoomCreate msg, NetworkSession session)
	{
		return new GameRoomLocal(this, session, msg);
	}

	private void EnqueueRoomMessage(RoomMessage msg, NetworkSession session)
	{
		if (TryGetRoom(msg.RoomSessionID, out var room))
		{
			room.EnqueueMessage(msg, session);
			return;
		}
		Log.Error("Enqueue RoomMessage " + msg.GetType().Name + " but not exists room:" + msg.RoomSessionID + " player:" + msg.PlayerSessionID);
	}

	private void HandleMessage(IMessage message, NetworkSession session)
	{
		if (!(message is RoomMessage roomMessage))
		{
			Log.Error($"Unknown message type {message.GetType()}");
			return;
		}
		PlayerSession session2 = null;
		if (message != null)
		{
			if (message is C2SGameRoomCreate c2SGameRoomCreate)
			{
				C2SGameRoomCreate msg = c2SGameRoomCreate;
				AddRoom(msg, session);
				return;
			}
			if (message is C2SGameRoomDestroy c2SGameRoomDestroy)
			{
				C2SGameRoomDestroy msg2 = c2SGameRoomDestroy;
				DelRoom(msg2, session);
				return;
			}
			if (message is C2SGameRoomEnter c2SGameRoomEnter)
			{
				C2SGameRoomEnter msg3 = c2SGameRoomEnter;
				if (!TryGetPlayer(roomMessage.PlayerSessionID, out session2))
				{
					Log.Error("Player not found " + roomMessage.PlayerSessionID);
				}
				else
				{
					EnterRoom(session2, msg3);
				}
				return;
			}
			if (message is C2SGameRoomLeave c2SGameRoomLeave)
			{
				C2SGameRoomLeave msg4 = c2SGameRoomLeave;
				if (!TryGetPlayer(roomMessage.PlayerSessionID, out session2))
				{
					Log.Error("Player not found " + roomMessage.PlayerSessionID);
				}
				else
				{
					LeaveRoom(session2, msg4);
				}
				return;
			}
			if (message is C2SGameRoomVerify c2SGameRoomVerify)
			{
				C2SGameRoomVerify msg5 = c2SGameRoomVerify;
				VerifyRoom(msg5, session);
				return;
			}
			if (message is C2SGameRoomSync || message is C2SGameRoomStart || message is C2SGameRoomEnd)
			{
				EnqueueRoomMessage(roomMessage, session);
				return;
			}
		}
		Log.Error($"Invalid message type {message.GetType()}");
	}

	protected virtual NetworkSession CreatePlayerSession(long sessionId, IPEndPoint endPoint)
	{
		return new PlayerSession(this, Network, sessionId, endPoint);
	}
}
