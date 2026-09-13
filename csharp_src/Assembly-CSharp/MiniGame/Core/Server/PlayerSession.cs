using System.Net;
using Joker;

namespace MiniGame.Core.Server;

public class PlayerSession : NetworkSession
{
	protected string _playerSessionID;

	protected string _roomSessionID;

	protected string _sid;

	protected string _uid;

	protected string _uuid;

	protected readonly IGameServer _server;

	protected ILogger Log;

	public string PlayerSessionID => _playerSessionID;

	public string RoomSessionID => _roomSessionID;

	public string SID => _sid;

	public string UID => _uid;

	public string UUID => _uuid;

	public PlayerSession(IGameServer server, NetworkSystem owner, long channelID, IPEndPoint ipEndPoint)
		: base(owner, channelID, ipEndPoint)
	{
		_server = server;
		_server.ConnectPlayer(this);
		Log = Joker.Log.GetLogger<PlayerSession>();
	}

	public void Logout()
	{
		if (!string.IsNullOrEmpty(PlayerSessionID))
		{
			_server?.LogoutPlayer(this);
		}
		_playerSessionID = null;
		_roomSessionID = null;
		_sid = null;
		_uuid = null;
		_uuid = null;
	}

	public override void HandleMessage(IMessage message)
	{
		if (message is RoomMessage roomMessage)
		{
			if (roomMessage is C2SGameRoomSync)
			{
				roomMessage.RoomSessionID = RoomSessionID;
				roomMessage.PlayerSessionID = PlayerSessionID;
				roomMessage.SID = SID;
				roomMessage.UUID = UUID;
				roomMessage.UID = UID;
				Log.Debug($"C2SGameRoomSync({UID}):  {roomMessage.Data}");
				if (string.IsNullOrEmpty(roomMessage.RoomSessionID) || string.IsNullOrEmpty(roomMessage.PlayerSessionID))
				{
					Log.Error("C2SGameRoomSync with none room " + roomMessage.RoomSessionID + " or " + roomMessage.PlayerSessionID);
					return;
				}
			}
			else
			{
				Log.Debug($"Received Room Message: room:{roomMessage.RoomSessionID} player:{roomMessage.PlayerSessionID} sid:{roomMessage.SID} uuid:{roomMessage.UUID} uid:{roomMessage.UID} code:{roomMessage.OpCode} data:{roomMessage.Data}");
			}
			if (string.IsNullOrEmpty(PlayerSessionID))
			{
				if (string.IsNullOrEmpty(roomMessage.PlayerSessionID))
				{
					Log.Error("First message must contain SessionID.");
					return;
				}
				_roomSessionID = roomMessage.RoomSessionID;
				_playerSessionID = roomMessage.PlayerSessionID;
				_sid = roomMessage.SID;
				_uid = roomMessage.UUID;
				_uid = roomMessage.UID;
				_server.LoginPlayer(this);
			}
			else if (PlayerSessionID != roomMessage.PlayerSessionID)
			{
				Log.Error("Session id from one connect must same " + PlayerSessionID + " " + roomMessage.PlayerSessionID);
				return;
			}
			base.HandleMessage(message);
		}
		else
		{
			Log.Error($"Received unexpected message type: {message.GetType()}");
		}
	}

	public override void SendMessage(IMessage message)
	{
		if (message is RoomMessage roomMessage)
		{
			if (roomMessage is S2CGameRoomSync)
			{
				roomMessage.PlayerSessionID = null;
				roomMessage.RoomSessionID = null;
				roomMessage.SID = null;
				roomMessage.UUID = null;
				roomMessage.UID = null;
				Log.Debug($"S2CGameRoomSync({UID}): {roomMessage.Data}");
			}
			else
			{
				roomMessage.RoomSessionID = RoomSessionID;
				roomMessage.PlayerSessionID = PlayerSessionID;
				roomMessage.SID = SID;
				roomMessage.UUID = UUID;
				roomMessage.UID = UID;
				Log.Debug($"Send Room Message: room{roomMessage.RoomSessionID} {roomMessage.RoomSessionID} player:{roomMessage.PlayerSessionID} sid:{roomMessage.SID} uuid:{roomMessage.UUID} uid:{roomMessage.UID} code:{roomMessage.OpCode} data:{roomMessage.Data}");
			}
		}
		base.Owner.Send(base.ChannelID, message);
	}

	public override void Dispose()
	{
		if (base.IsValid)
		{
			base.Dispose();
			Logout();
			_server.DisconnectPlayer(this);
		}
	}
}
