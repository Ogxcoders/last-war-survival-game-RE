using System;
using Joker;

namespace MiniGame.Core.Server;

public class TGameLogicService<T> : GameLogicService, IService where T : IGameLogicFactory, new()
{
	private class LogicLogger : IGameLogicLogger
	{
		public void Debug(string message)
		{
			Log.Debug(message);
		}

		public void Info(string message)
		{
			Log.Info(message);
		}

		public void Warning(string message)
		{
			Log.Warning(message);
		}

		public void Error(string message)
		{
			Log.Error(message);
		}

		public void Exception(Exception ex)
		{
			Log.Exception(ex);
		}
	}

	private IGameLogicFactory _factory;

	public TGameLogicService()
	{
		_factory = new T();
		_factory.InitLogger(new LogicLogger());
		_factory.InitWorkspace(null);
	}

	public void Awake()
	{
	}

	public void Startup()
	{
	}

	public void Shutdown()
	{
	}

	public void Destroy()
	{
	}

	public override IGameLogic CreateGame(string type, string logicVersion, string resourceVersion = null)
	{
		return _factory.CreateGame(type);
	}
}
