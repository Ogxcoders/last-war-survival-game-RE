using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using MiniGame.Core;

namespace MiniGame.Test.Client;

public class GameTestLoader : IResourceLoader
{
	private GameWorld _world;

	private GameLevel _level;

	private IGameSerializer _serializer;

	private List<IResourceHolder> _holders = new List<IResourceHolder>();

	private Stopwatch _sw = new Stopwatch();

	public float Progress { get; private set; }

	public bool IsDone { get; private set; }

	public bool IsLoading { get; private set; }

	public bool IsError { get; private set; }

	public float RunningTime => (float)_sw.ElapsedMilliseconds * 0.001f;

	public GameWorld GameWorld
	{
		get
		{
			if (!IsDone)
			{
				return null;
			}
			return _world;
		}
	}

	public GameTestRuntime Runtime { get; private set; }

	public IGameSerializer GetSerializer()
	{
		return _serializer;
	}

	public IResourceHolder LoadAsset<T>(string name)
	{
		throw new NotImplementedException();
	}

	public IResourceHolder LoadAssetAsync<T>(string name, Action<IResourceHolder> callback = null)
	{
		if (typeof(T) == typeof(GameTestLevelHolder))
		{
			GameTestLevelHolder gameTestLevelHolder = new GameTestLevelHolder(name);
			gameTestLevelHolder.OnDone = callback;
			_holders.Add(gameTestLevelHolder);
			return gameTestLevelHolder;
		}
		return null;
	}

	public object LoadConfig(int id, Type type)
	{
		throw new NotImplementedException();
	}

	public T LoadConfig<T>(int id) where T : class
	{
		throw new NotImplementedException();
	}

	public IEnumerator StartGame(params object[] args)
	{
		IsLoading = true;
		_sw.Reset();
		_sw.Start();
		IResourceHolder holder = LoadAssetAsync<GameTestLevelHolder>(args[0] as string);
		while (!holder.IsDone)
		{
			yield return null;
		}
		GameTestEnvClient gameTestEnvClient = new GameTestEnvClient();
		gameTestEnvClient.ResourceLoader = this;
		Runtime = args[1] as GameTestRuntime;
		_level = holder.As<GameLevel>();
		_world = new GameWorld(_level.GetEnv<GameTestEnvClient>() ?? gameTestEnvClient, _serializer);
		GameTestClient.InitSystems(_world);
		_world.Init();
		Progress = 0f;
		for (int i = 0; i < 100; i++)
		{
			Progress += (float)(i + 1) * 0.01f;
			yield return null;
		}
		IsDone = true;
		IsLoading = false;
		IsError = false;
	}

	public IEnumerator ExitGame()
	{
		_sw.Stop();
		_world?.Dispose();
		_world = null;
		foreach (IResourceHolder holder in _holders)
		{
			holder.OnDone = null;
		}
		yield return null;
	}

	public void Dispose()
	{
		ExitGame();
		foreach (IResourceHolder holder in _holders)
		{
			holder.Dispose();
		}
		_holders.Clear();
	}
}
