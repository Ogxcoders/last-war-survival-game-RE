using System;
using Joker;

namespace MiniGame.Core.Server;

public class GameRoomPlayer : IGameLogicSession, IGameLogicMessageSender
{
	private readonly GameRoom _room;

	private readonly NetworkSession _session;

	private IGameLogic _logic;

	private bool _isObserver;

	private string _playerName;

	private string _uid;

	private string _sessionId;

	public GameRoomPlayer(GameRoom room, C2SGameRoomEnter msg, NetworkSession session)
	{
		_room = room;
		_playerName = msg.PlayerName;
		_uid = msg.UID;
		_sessionId = msg.PlayerSessionID;
		_session = session;
	}

	public void AddToLogic(IGameLogic logic, bool isObserver)
	{
		_logic = logic;
		_isObserver = isObserver;
		if (isObserver)
		{
			logic.AddOBPlayer(this);
		}
		else
		{
			logic.AddPlayer(this);
		}
	}

	public void DelFromLogic(IGameLogic logic)
	{
		if (_isObserver)
		{
			_logic.DelOBPlayer(this);
		}
		else
		{
			_logic.DelPlayer(this);
		}
		_logic = null;
	}

	public void SendRaw(int opCode, object data, object oriData)
	{
		RoomMessage roomMessage = null;
		if (_logic.IsOpCode<IMessageVerify>(opCode))
		{
			roomMessage = new S2CGameRoomVerify();
		}
		else if (_logic.IsOpCode<IMessageSync>(opCode))
		{
			roomMessage = new S2CGameRoomSync();
		}
		else if (_logic.IsOpCode<IMessageStart>(opCode))
		{
			IMessageStart messageStart = oriData as IMessageStart;
			roomMessage = new S2CGameRoomStart
			{
				PlayerSessionIDs = (messageStart.PlayerSessionIDs.Clone() as string[])
			};
		}
		else if (_logic.IsOpCode<IMessageEnd>(opCode))
		{
			roomMessage = new S2CGameRoomEnd();
		}
		else
		{
			if (!_logic.IsOpCode<IMessageEnter>(opCode))
			{
				throw new NotImplementedException("Unknown opcode: " + opCode);
			}
			roomMessage = new S2CGameRoomEnter
			{
				ProcessID = GameRoomSessionService.Instance.RoomProcessID,
				FleetID = GameRoomSessionService.Instance.RoomFleetID,
				InstanceID = GameRoomSessionService.Instance.RoomInstanceID
			};
		}
		roomMessage.RoomSessionID = _room.RoomSessionID;
		roomMessage.SID = _room.SID;
		roomMessage.UUID = _room.UUID;
		roomMessage.OpCode = opCode;
		roomMessage.Data = data;
		_session.SendMessage(roomMessage);
	}

	public string GetName()
	{
		return _playerName;
	}

	public string GetUID()
	{
		return _uid;
	}

	public string GetSessionID()
	{
		return _sessionId;
	}

	public object GetProperty(string key)
	{
		return null;
	}
}
