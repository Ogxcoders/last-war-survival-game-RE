using System;
using System.Net;
using Joker;
using Joker.Client;
using MiniGame.Core.Server;

namespace MiniGame.Biubiu.Client;

public abstract class GameBiuBiuPlayerBase : GameClient
{
	protected long EnterBackGroundTime;

	protected long _channel = -1L;

	protected readonly string _ip;

	protected readonly int _port;

	protected readonly string _room;

	protected int _maxPlayers;

	protected bool AppPauseState { get; private set; }

	protected NetworkSystem Network { get; private set; }

	public GameBiuBiuRunTime GameBiuBiuRunTime { get; private set; }

	public bool IsMultiPlayerGame => !string.IsNullOrEmpty(_room);

	public EGameMultiPlayerState MultiPlayerState { get; set; }

	public string LevelPath { get; private set; }

	public string LevelJson { get; private set; }

	public bool IsConnected { get; set; }

	public bool ReEnter { get; set; }

	public bool RoomEnd { get; set; }

	public EPlayerID PlayerID { get; set; } = EPlayerID.ID_None;

	public int ServerId { get; set; }

	public string SessionGame { get; set; }

	public string Uid { get; set; }

	protected string UUID { get; set; }

	public string PlayerSessionId { get; set; }

	protected string PlayerName { get; set; }

	protected string Sid { get; set; }

	public string LogicVersion { get; protected set; }

	public string ResVersion { get; protected set; }

	public float EnterRoomTime { get; protected set; }

	public GameBiuBiuPlayerBase()
	{
	}

	public GameBiuBiuPlayerBase(string ip, string room)
	{
		_ip = "127.0.0.1";
		_port = 7758;
		string[] array = ip.Split(new char[1] { ':' });
		if (array.Length == 1)
		{
			_ip = array[0].Trim();
			_ip = (IPAddress.TryParse(_ip, out var _) ? _ip : "127.0.0.1");
		}
		else if (array.Length == 2)
		{
			_ip = array[0].Trim();
			_port = (int.TryParse(array[1].Trim(), out _port) ? _port : 7758);
		}
		_room = room;
	}

	protected override void InitSystems()
	{
		Network = AddSystem<NetworkSystem>();
		NetworkSystem network = Network;
		network.OnConnect = (Action<long>)Delegate.Combine(network.OnConnect, new Action<long>(OnConnect));
		NetworkSystem network2 = Network;
		network2.OnDisconnect = (Action<long>)Delegate.Combine(network2.OnDisconnect, new Action<long>(OnDisconnect));
		NetworkSystem network3 = Network;
		network3.OnMessage = (Action<long, IMessage>)Delegate.Combine(network3.OnMessage, new Action<long, IMessage>(OnMessage));
		NetworkSystem network4 = Network;
		network4.OnError = (Action<long, int>)Delegate.Combine(network4.OnError, new Action<long, int>(OnError));
	}

	private void OnConnect(long channel)
	{
		_channel = channel;
		Log.Info($"OnConnect client game biubiu {_channel}");
		IsConnected = true;
	}

	protected void ConnectGameLiftServer(IPEndPoint address, bool force)
	{
		if (force)
		{
			_channel = -1L;
			IsConnected = false;
			NetworkSystem network = Network;
			network.OnConnect = (Action<long>)Delegate.Remove(network.OnConnect, new Action<long>(OnConnect));
			NetworkSystem network2 = Network;
			network2.OnDisconnect = (Action<long>)Delegate.Remove(network2.OnDisconnect, new Action<long>(OnDisconnect));
			NetworkSystem network3 = Network;
			network3.OnMessage = (Action<long, IMessage>)Delegate.Remove(network3.OnMessage, new Action<long, IMessage>(OnMessage));
			NetworkSystem network4 = Network;
			network4.OnError = (Action<long, int>)Delegate.Remove(network4.OnError, new Action<long, int>(OnError));
			DelSystem(Network);
			Network = AddSystem<NetworkSystem>();
			NetworkSystem network5 = Network;
			network5.OnConnect = (Action<long>)Delegate.Combine(network5.OnConnect, new Action<long>(OnConnect));
			NetworkSystem network6 = Network;
			network6.OnDisconnect = (Action<long>)Delegate.Combine(network6.OnDisconnect, new Action<long>(OnDisconnect));
			NetworkSystem network7 = Network;
			network7.OnMessage = (Action<long, IMessage>)Delegate.Combine(network7.OnMessage, new Action<long, IMessage>(OnMessage));
			NetworkSystem network8 = Network;
			network8.OnError = (Action<long, int>)Delegate.Combine(network8.OnError, new Action<long, int>(OnError));
		}
		Network.Connect(address);
	}

	protected virtual void OnDisconnect(long channel)
	{
		Log.Info($"OnDisconnect client game biubiu {_channel}");
		IsConnected = false;
	}

	protected virtual void OnMessage(long channel, IMessage message)
	{
	}

	private void OnError(long channel, int error)
	{
		string errorDesc = NetworkSystem.GetErrorDesc(error);
		Log.Info($"Network channel lost {channel} with resion:{errorDesc}({error})");
	}

	public abstract void Send(object message);

	public virtual void SendMessage(object message, RoomMessage room)
	{
		if (!(room is C2SGameRoomSync))
		{
			room.RoomSessionID = _room;
			room.PlayerSessionID = PlayerSessionId;
			room.SID = Sid;
			room.UUID = UUID;
			room.UID = Uid;
		}
		else
		{
			room.RoomSessionID = null;
			room.PlayerSessionID = null;
			room.SID = null;
			room.UID = null;
			room.UUID = null;
		}
		GameBiubiuShare.PackMessage(message, out room.OpCode, out room.Data);
		Network.Send(_channel, room);
		Log.Debug($"BiuBiu Send:{room.GetType()} RoomSessionID{_room}PlayerSessionId:{PlayerSessionId}UUID{UUID}UID{Uid}SID{Sid}");
	}

	public void SetupRoom(string level, string levelJson, string sid, string uuid, int maxPlayers, float enterRoomTime = -1f)
	{
		LevelPath = level;
		LevelJson = levelJson;
		_maxPlayers = maxPlayers;
		UUID = uuid;
		Sid = sid;
		EnterRoomTime = enterRoomTime;
	}

	public void SetUpPlayer(int serverId, string uid, string playerName, string playerSessionId, string sessionGame, EPlayerID ePlayerID)
	{
		SessionGame = sessionGame;
		ServerId = serverId;
		Uid = uid;
		PlayerName = playerName;
		PlayerSessionId = playerSessionId;
		PlayerID = ePlayerID;
	}

	public void SetUpVersion(string logicVersion, string resVersion)
	{
		LogicVersion = logicVersion;
		ResVersion = resVersion;
	}

	public void SendEnterRoom()
	{
		MultiPlayerState = EGameMultiPlayerState.EnterRoom;
		GameBiubiuEnter message = new GameBiubiuEnter();
		Send(message);
	}

	public void SetupRunTime(GameBiuBiuRunTime biuBiuRunTime)
	{
		GameBiuBiuRunTime = biuBiuRunTime;
	}

	public void SetIsReEnter()
	{
		ReEnter = true;
	}

	public virtual void WifiTimeOut()
	{
	}

	protected bool IsNeedReConnect()
	{
		long localSeconds = GameEntry.Timer.GetLocalSeconds();
		if (!IsConnected || localSeconds - EnterBackGroundTime >= 60)
		{
			return true;
		}
		return false;
	}

	public virtual void AppPause()
	{
		AppPauseState = true;
		EnterBackGroundTime = GameEntry.Timer?.GetLocalSeconds() ?? 0;
		Log.Info("[BiuBiu]:AppPause");
	}

	public virtual void AppResume()
	{
		AppPauseState = false;
		Log.Info("[BiuBiu]:AppResume");
	}
}
