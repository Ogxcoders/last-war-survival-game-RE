using System;
using System.Collections.Generic;
using Joker;

namespace MiniGame.Core.Server;

public class GameRoomThreadServer : World, IStartupSync, IShutdownSync, IUpdate, ILateUpdate
{
	private class FakePlayerSession : NetworkSession
	{
		private readonly World _gateWorld;

		private readonly string _sessionId;

		public FakePlayerSession(World gateWorld, string sessionId)
			: base(null, 0L, null)
		{
			_gateWorld = gateWorld;
			_sessionId = sessionId;
		}

		public override void SendMessage(IMessage message)
		{
			if (message is RoomMessage roomMessage)
			{
				roomMessage.PlayerSessionID = _sessionId;
				_gateWorld.EnqueueMessage(message, _sessionId);
			}
			else
			{
				Log.Error("Player message type " + message.GetType().Name + " is not RoomMessage");
			}
		}

		public override void HandleMessage(IMessage message)
		{
			throw new NotSupportedException("This method is not supported");
		}

		public override void Dispose()
		{
			if (!base.IsValid)
			{
				base.Dispose();
			}
		}
	}

	private readonly int _gateId;

	private Dictionary<string, FakePlayerSession> _players = new Dictionary<string, FakePlayerSession>();

	private Dictionary<string, GameRoom> _rooms = new Dictionary<string, GameRoom>();

	public GameRoomThreadServer(int gateId)
	{
		_gateId = gateId;
	}

	public void Startup()
	{
	}

	public void Shutdown()
	{
		foreach (KeyValuePair<string, GameRoom> room in _rooms)
		{
			room.Value.Dispose();
		}
		_rooms.Clear();
	}

	public void Update()
	{
		(IMessage, object) message;
		while (TryDequeueMessage(out message))
		{
			if (!(message.Item2 is string))
			{
				Log.Error("message sender is not a string");
			}
			else
			{
				HandleMessage(message.Item1, GetOrAddPlayer((string)message.Item2));
			}
		}
	}

	public void LateUpdate()
	{
		float deltaTime = Singleton<TimeService>.Instance.DeltaTime;
		foreach (KeyValuePair<string, GameRoom> room in _rooms)
		{
			room.Value.Update(deltaTime);
		}
	}

	private FakePlayerSession GetOrAddPlayer(string id)
	{
		if (!_players.TryGetValue(id, out var value))
		{
			value = new FakePlayerSession(Singleton<WorldService>.Instance.GetWorld(_gateId), id);
			_players[id] = value;
		}
		return value;
	}

	private void RemovePlayer(string id)
	{
		_players.Remove(id);
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
			c2SGameRoomCreate.RoomSessionID = msg.RoomSessionID;
			c2SGameRoomCreate.SID = msg.SID;
			c2SGameRoomCreate.UUID = msg.UUID;
			c2SGameRoomCreate.LogicType = msg.LogicType;
			c2SGameRoomCreate.LogicVersion = msg.LogicVersion;
			c2SGameRoomCreate.ResourceVersion = msg.ResourceVersion;
			AddRoom(c2SGameRoomCreate, networkSession);
		}
	}

	public void TryDelRoomByLeave(C2SGameRoomLeave msg, NetworkSession networkSession)
	{
		if (_players.ContainsKey(msg.PlayerSessionID))
		{
			RemovePlayer(msg.PlayerSessionID);
		}
	}

	private void AddRoom(C2SGameRoomCreate msg, NetworkSession session)
	{
		if (_rooms.ContainsKey(msg.RoomSessionID))
		{
			Log.Error("Room already exists {0}", msg.RoomSessionID);
		}
		else
		{
			GameRoom gameRoom = CreateRoom(msg, session);
			_rooms.Add(gameRoom.RoomSessionID, gameRoom);
			GameRoomSessionService.Instance.ActivateGameSession(msg.PlayerSessionID);
		}
	}

	private void DelRoom(C2SGameRoomDestroy msg, NetworkSession session)
	{
		if (!_rooms.TryGetValue(msg.RoomSessionID, out var value))
		{
			Log.Error("Room not found {0}", msg.RoomSessionID);
		}
		else
		{
			_rooms.Remove(msg.RoomSessionID);
			value.Dispose();
			GameRoomSessionService.Instance.TerminateGameSession(msg.RoomSessionID);
		}
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
			Log.Error("Room already exists {0}", msg.RoomSessionID);
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
			gameRoom.EnqueueMessage(msg, session);
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
		Log.Error("Room not exists {0}", msg.RoomSessionID);
	}

	private void HandleMessage(IMessage message, NetworkSession session)
	{
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
				TryAddRoomByEnter(msg3, session);
				EnqueueRoomMessage(msg3, session);
				return;
			}
			if (message is C2SGameRoomLeave c2SGameRoomLeave)
			{
				C2SGameRoomLeave msg4 = c2SGameRoomLeave;
				TryDelRoomByLeave(msg4, session);
				EnqueueRoomMessage(msg4, session);
				return;
			}
			if (message is C2SGameRoomVerify c2SGameRoomVerify)
			{
				C2SGameRoomVerify msg5 = c2SGameRoomVerify;
				VerifyRoom(msg5, session);
				return;
			}
			if (message is RoomMessage roomMessage)
			{
				RoomMessage msg6 = roomMessage;
				EnqueueRoomMessage(msg6, session);
				return;
			}
		}
		Log.Error("Unknown message type {0}", message.GetType());
	}
}
