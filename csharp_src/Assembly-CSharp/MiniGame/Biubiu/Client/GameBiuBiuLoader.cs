using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameBiuBiuLoader
{
	private GameWorld _world;

	private GameLevel _level;

	private Stopwatch _sw = new Stopwatch();

	private Dictionary<string, IResourceHolder> _holders = new Dictionary<string, IResourceHolder>();

	private const string GameRootName = "Assets/Main/MiniGameRes/BiuBiu/Prefab/Env/UIS5BiuBiuEnv.prefab";

	private const string ConfigPath = "Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt";

	private List<ISyncCommand> _lastCommands;

	public float Progress { get; private set; }

	public bool IsDone { get; private set; }

	public bool IsLoading { get; private set; }

	public bool IsError { get; private set; }

	private IResourceLoader ResourceLoader { get; set; }

	public GameWorld World => _world;

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

	public Action LoadingEnd { get; set; }

	public IEnumerator StartGame(EGameType gameType, string levelPath, string gameLevelJson, GameBiuBiuRunTime runTime, bool updateTemplates, bool verify = true)
	{
		IsLoading = true;
		_sw.Reset();
		_sw.Start();
		IGameSharedEnv sharedEnv = runTime.GetSharedEnv();
		SharedRuntime shared = sharedEnv as SharedRuntime;
		shared.InitData.LevelPath = levelPath;
		shared.GameType = gameType;
		ResourceLoader = shared?.ResourceLoader;
		IResourceHolder gameLevelHolder = null;
		if (string.IsNullOrEmpty(gameLevelJson) && !string.IsNullOrEmpty(levelPath) && !_holders.TryGetValue(levelPath, out gameLevelHolder))
		{
			gameLevelHolder = ResourceLoader.LoadAsset<GameLevel>(levelPath);
			_holders.Add(levelPath, gameLevelHolder);
		}
		if (!_holders.TryGetValue("Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt", out var gameConfigHolder))
		{
			gameConfigHolder = ResourceLoader.LoadAsset<SharedConfigData>("Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt");
			_holders.Add("Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt", gameConfigHolder);
		}
		if (!string.IsNullOrEmpty(gameLevelJson))
		{
			_level = runTime.Serializer.FromJson<GameLevel>(gameLevelJson);
		}
		else if (_level == null && gameLevelHolder != null)
		{
			_level = gameLevelHolder.As<GameLevel>();
		}
		updateTemplates = false;
		if (_level != null && updateTemplates)
		{
			_level.UpdateTemplates(ResourceLoader);
		}
		SharedConfigData configData = ((gameConfigHolder != null && gameConfigHolder.IsDone) ? gameConfigHolder.As<SharedConfigData>() : null);
		_world = new GameWorld(sharedEnv, runTime.Serializer, new GameUnityDebugger());
		_holders.TryGetValue("Assets/Main/MiniGameRes/BiuBiu/Prefab/Env/UIS5BiuBiuEnv.prefab", out var gameRootHolder);
		if (gameRootHolder == null)
		{
			gameRootHolder = ResourceLoader.LoadAssetAsync<GameObjectHolder>("Assets/Main/MiniGameRes/BiuBiu/Prefab/Env/UIS5BiuBiuEnv.prefab", delegate(IResourceHolder holder)
			{
				if (holder.IsError || !holder.IsValid)
				{
					UnityEngine.Debug.LogError("asset name :Assets/Main/MiniGameRes/BiuBiu/Prefab/Env/UIS5BiuBiuEnv.prefab load error");
				}
				else
				{
					((GameLoader)ResourceLoader).LoaderEnv.SetGameGO(holder.As<GameObject>());
				}
			});
			((GameObjectHolder)gameRootHolder).With(GameObjectHolder.InstanceExtra.Default);
			_holders.Add("Assets/Main/MiniGameRes/BiuBiu/Prefab/Env/UIS5BiuBiuEnv.prefab", gameRootHolder);
		}
		while ((gameLevelHolder != null && !gameLevelHolder.IsDone) || (gameConfigHolder != null && !gameConfigHolder.IsDone) || (gameRootHolder != null && !gameRootHolder.IsDone))
		{
			yield return null;
		}
		_holders["Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt"].Dispose();
		_holders["Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt"] = new ConfigHolder(configData);
		(_world.Env.ResourceLoader as GameLoader)?.BindConfig(_holders["Assets/Main/MiniGameRes/BiuBiu/Config/Config.txt"] as ConfigHolder);
		runTime.InitRunTime(_world, _level, verify);
		if (ResourceLoader != null && ((GameLoader)ResourceLoader).LoaderEnv is GameBiubiuPlayerEnv gameBiubiuPlayerEnv)
		{
			shared.InitData.ServerId = gameBiubiuPlayerEnv.Player.ServerId;
			shared.InitData.ReEnter = gameBiubiuPlayerEnv.Player.ReEnter;
			FuncPostLog.PostPrepareTime(gameBiubiuPlayerEnv.Player.EnterRoomTime, gameBiubiuPlayerEnv.Player.SessionGame);
		}
		else
		{
			FuncPostLog.PostPrepareTime(-1f);
		}
		_world.Init();
		Progress = 0f;
		float allProgress = _holders.Count;
		bool preFinish = false;
		while (!preFinish)
		{
			preFinish = true;
			float curProgress = 0f;
			foreach (IResourceHolder value in _holders.Values)
			{
				preFinish = preFinish && value.IsDone;
				curProgress += value.Progress;
				yield return null;
			}
			Progress = curProgress / allProgress;
		}
		IsDone = true;
		IsLoading = false;
		IsError = false;
		LoadingEnd?.Invoke();
	}

	public void ExitGame()
	{
		_sw.Stop();
		_world?.Dispose();
		_world = null;
		if (_level != null)
		{
			_level.Dispose();
			_level = null;
		}
		ResourceLoader = null;
		LoadingEnd = null;
		foreach (KeyValuePair<string, IResourceHolder> holder in _holders)
		{
			holder.Value.OnDone = null;
		}
		FuncTCClient.ClearAction();
	}

	public void Dispose()
	{
		_sw.Stop();
		_world?.Dispose();
		_world = null;
		if (_level != null)
		{
			_level.Dispose();
			_level = null;
		}
		List<string> list = _holders.Keys.ToList();
		for (int num = list.Count - 1; num >= 0; num--)
		{
			_holders[list[num]].Dispose();
		}
		_holders.Clear();
		ResourceLoader = null;
		LoadingEnd = null;
		FuncTCClient.ClearAction();
	}
}
