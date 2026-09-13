using System;
using System.Collections.Generic;
using System.Diagnostics;
using Joker;

namespace MiniGame.Core.Server;

public abstract class GameRoom : IDisposable
{
	public enum EState
	{
		Init,
		Running,
		Settlement,
		Invalid
	}

	protected const int kKeepSettlementDelay = 10;

	protected const int kKeepAliveDelay = 20;

	protected World _ownerServer;

	protected readonly string _roomSessionID;

	protected readonly string _SID;

	protected readonly string _UUID;

	protected readonly int _lifeSeconds;

	protected EState _state;

	protected string _logicType;

	protected Stopwatch _sw;

	protected IGameLogic _logic;

	protected string _resourceVersion;

	protected string _logicVersion;

	protected string _levelPath;

	protected int _maxPlayers;

	protected ILogger Log;

	protected Queue<(IMessage msg, NetworkSession sender)> _queue = new Queue<(IMessage, NetworkSession)>();

	protected DoubleMap<NetworkSession, GameRoomPlayer> _players = new DoubleMap<NetworkSession, GameRoomPlayer>();

	public string SID => _SID;

	public string UUID => _UUID;

	public string RoomSessionID => _roomSessionID;

	public EState State => _state;

	public bool IsValid => _state != EState.Invalid;

	public Dictionary<string, string> RoomProperties { get; private set; } = new Dictionary<string, string>();

	public GameRoom(World server, NetworkSession roomOwner, C2SGameRoomCreate msg)
	{
		Log = Joker.Log.GetLogger(GetType().Name);
		_ownerServer = server;
		_roomSessionID = msg.RoomSessionID;
		_SID = msg.SID;
		_UUID = msg.UUID;
		_lifeSeconds = ((msg.LifeSeconds > 0) ? msg.LifeSeconds : 600);
		_sw = new Stopwatch();
		_sw.Start();
		_logicType = msg.LogicType;
		_resourceVersion = msg.ResourceVersion;
		_logicVersion = msg.LogicVersion;
		_levelPath = msg.LevelPath;
		_maxPlayers = msg.MaxPlayers;
		Log.Info($"Create game room sid:{SID} UUID:{UUID} LifeSeconds:{_lifeSeconds} LogicType:{_logicType} LogicVersion:{_logicVersion} ResourceVersion:{_resourceVersion} LevelPath:{_levelPath} MaxPlayers:{_maxPlayers}");
		TryInitGame();
	}

	public bool HasPlayer(NetworkSession session)
	{
		GameRoomPlayer value;
		return _players.TryGetValueByKey(session, out value);
	}

	public void AddPlayer(C2SGameRoomEnter msg, NetworkSession session)
	{
		if (!IsValid)
		{
			return;
		}
		if (_logic == null)
		{
			Log.Error("GameRoom can not accept logic because it is null.");
			App.Exit();
			return;
		}
		if (!string.IsNullOrEmpty(_levelPath))
		{
			if (msg.LevelPath != _levelPath)
			{
				Log.Error("AppPlayer with invalid level path: " + msg.LevelPath);
			}
			msg.LevelPath = _levelPath;
		}
		else
		{
			AssignDefaultMessageValue(ref msg.LevelPath, _levelPath);
		}
		AssignDefaultMessageValue(ref msg.LogicType, _logicType);
		AssignDefaultMessageValue(ref msg.LogicVersion, _logicVersion);
		AssignDefaultMessageValue(ref msg.ResourceVersion, _resourceVersion);
		AssignDefaultMessageValue(ref msg.MaxPlayers, _maxPlayers);
		GameRoomPlayer gameRoomPlayer = new GameRoomPlayer(this, msg, session);
		_players.Add(session, gameRoomPlayer);
		gameRoomPlayer.AddToLogic(_logic, msg.IsObserver);
		HandleLogicMessage(gameRoomPlayer, msg);
	}

	public void DelPlayer(C2SGameRoomLeave msg, NetworkSession session)
	{
		if (!IsValid)
		{
			return;
		}
		if (!_players.TryGetValueByKey(session, out var value))
		{
			Log.Error("RemovePlayer failed! can not find player.");
			return;
		}
		if (msg != null && msg.Data != null)
		{
			HandleLogicMessage(value, msg);
		}
		value.DelFromLogic(_logic);
		_players.RemoveByKey(session);
	}

	public void VerifyGame(C2SGameRoomVerify msg, NetworkSession session)
	{
		if (IsValid)
		{
			_logicType = msg.LogicType;
			_resourceVersion = msg.ResourceVersion;
			_logicVersion = msg.LogicVersion;
			TryInitGame();
			AddPlayer(new C2SGameRoomEnter
			{
				SID = msg.SID,
				UUID = msg.UUID,
				RoomSessionID = msg.RoomSessionID,
				PlayerSessionID = msg.PlayerSessionID,
				IsObserver = true
			}, session);
			HandleLogicMessage(session, msg);
		}
	}

	public void EnqueueMessage(IMessage msg, NetworkSession session)
	{
		_queue.Enqueue((msg, session));
	}

	public virtual void Update(float elapsed)
	{
		if (IsValid)
		{
			ProcessMessages();
			if (_logic != null && _logic.IsValid())
			{
				_logic.Update(elapsed);
			}
			if (State != EState.Settlement && (IsSettlementTimeout() || (_logic != null && _logic.IsSettlement())))
			{
				_state = EState.Settlement;
				NotifySettlement();
			}
			if (IsRoomTimeout() || (_logic != null && !_logic.IsValid()) || (State == EState.Settlement && !HasAlivePlayers()))
			{
				Dispose();
			}
		}
	}

	public void Dispose()
	{
		if (!IsValid)
		{
			return;
		}
		try
		{
			_sw.Stop();
			_state = EState.Invalid;
			_ownerServer.EnqueueMessage(new C2SGameRoomDestroy
			{
				RoomSessionID = _roomSessionID
			}, null);
			_logic?.Dispose();
			KickPlayers();
		}
		catch (Exception e)
		{
			Log.Exception(e);
		}
		finally
		{
			_state = EState.Invalid;
		}
		Log.Info("GameRoom disposed: " + GetType().Name);
	}

	protected void TryInitGame()
	{
		if (_logic == null && !string.IsNullOrEmpty(_logicType) && IsValid)
		{
			Log.Info("Create room: " + _logicType + " " + _logicVersion + " " + _resourceVersion);
			_logic = GameLogicService.Instance.CreateGame(_logicType, _logicVersion, _resourceVersion);
			if (_logic == null)
			{
				Log.Error("GameLogic could not be instantiated: " + _logicType);
				App.Exit();
			}
		}
	}

	protected virtual void ProcessMessages()
	{
		while (_queue.Count > 0)
		{
			(IMessage, NetworkSession) tuple = _queue.Dequeue();
			HandleMessage(tuple.Item1, tuple.Item2);
			TryInitGame();
		}
	}

	protected virtual void HandleMessage(IMessage message, NetworkSession session)
	{
		if (message is RoomMessage message2)
		{
			HandleLogicMessage(session, message2);
		}
		else
		{
			Log.Error("HandleMessage failed: " + message.GetType().Name);
		}
	}

	protected void HandleLogicMessage(GameRoomPlayer player, RoomMessage message)
	{
		try
		{
			_logic?.HandleMessage(player, message.OpCode, message.Data);
		}
		catch (Exception e)
		{
			Log.Exception(e);
			try
			{
				NotifySettlement();
			}
			finally
			{
				Dispose();
			}
		}
	}

	protected virtual void HandleLogicMessage(NetworkSession session, RoomMessage message)
	{
		if (!_players.TryGetValueByKey(session, out var value))
		{
			Log.Error("HandleLogicMessage failed! cannot find player. " + message.PlayerSessionID);
		}
		HandleLogicMessage(value, message);
	}

	protected bool IsSettlementTimeout()
	{
		return _sw.Elapsed.TotalSeconds > (double)(_lifeSeconds + 10);
	}

	protected bool IsRoomTimeout()
	{
		return _sw.Elapsed.TotalSeconds > (double)(_lifeSeconds + 20);
	}

	protected bool HasAlivePlayers()
	{
		return _players.Count > 0;
	}

	protected void KickPlayers()
	{
		Log.Info($"Kicking players: {_players.Count}");
		_players.ForEach(delegate(NetworkSession session, GameRoomPlayer player)
		{
			if (session is PlayerSession playerSession)
			{
				playerSession.Logout();
			}
		});
	}

	protected void AssignDefaultMessageValue(ref string val, string def)
	{
		if (string.IsNullOrEmpty(val) && !string.IsNullOrEmpty(def))
		{
			val = def;
		}
	}

	protected void AssignDefaultMessageValue(ref int val, int def)
	{
		if (val == 0)
		{
			val = def;
		}
	}

	public abstract void NotifySettlement();
}
