using System;
using System.Collections.Generic;
using Joker;

namespace MiniGame.Core.Server;

public class GameRoomSessionService : IService
{
	public static GameRoomSessionService Instance { get; private set; }

	public static string AccessKeyId { get; set; }

	public static string AccessKeySecret { get; set; }

	public string RoomSessionId { get; protected set; }

	public string RoomGameId { get; protected set; }

	public string RoomFleetID { get; protected set; }

	public string RoomProcessID { get; protected set; }

	public string RoomInstanceID { get; protected set; }

	public GameRoomSessionService()
	{
		if (Instance != null)
		{
			throw new Exception("IRuntimeService is already instantiated");
		}
		Instance = this;
	}

	public virtual void ProcessReady(int port)
	{
	}

	public virtual Dictionary<string, string> ActivateGameSession(string gameSessionId)
	{
		return null;
	}

	public virtual void TerminateGameSession(string gameSessionId)
	{
	}

	public virtual bool AcceptPlayerSession(string playerSessionId)
	{
		return true;
	}

	public virtual bool RemovePlayerSession(string playerSessionId)
	{
		return true;
	}

	public virtual void Awake()
	{
	}

	public virtual void Startup()
	{
	}

	public virtual void Shutdown()
	{
	}

	public virtual void Destroy()
	{
	}
}
