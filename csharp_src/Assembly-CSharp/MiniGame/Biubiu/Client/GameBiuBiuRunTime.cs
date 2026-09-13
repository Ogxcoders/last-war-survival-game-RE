using MiniGame.Core;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameBiuBiuRunTime : MonoBehaviour
{
	private GameBiuBiuLoader _loader;

	public GameBiuBiuLoader Loader => _loader;

	protected IBindUI BindUI { get; set; }

	protected DataResourceLoaderEnv LoadEnv { get; set; }

	public IGameSerializer Serializer => GameBiubiuClient.Serializer;

	public void BindRunEnv(IBindUI bindUI, DataResourceLoaderEnv loadEnv)
	{
		BindUI = bindUI;
		LoadEnv = loadEnv;
	}

	public virtual IGameSharedEnv GetSharedEnv()
	{
		return GameBiubiuClient.InitSharedEnv(null, new GameLoader(LoadEnv, BindUI, Serializer));
	}

	public virtual void InitRunTime(GameWorld gameWorld, GameLevel gameLevel, bool verify = true)
	{
		SharedRuntime sharedRuntime = (SharedRuntime)gameWorld.Env;
		sharedRuntime.InitData.PlayerID = EPlayerID.ID_1P;
		GameBiubiuClient.InitClient(gameWorld, sharedRuntime.GameType, gameLevel, verify);
	}

	public void Update()
	{
		_loader?.GameWorld?.Update(Time.deltaTime);
	}

	public void Enter(EGameType gameType, string levelPath, string gameLevelJson = null, bool verify = true)
	{
		if (_loader != null)
		{
			_loader.Dispose();
			_loader = null;
		}
		_loader = new GameBiuBiuLoader();
		StartCoroutine(_loader.StartGame(gameType, levelPath, gameLevelJson, this, updateTemplates: true, verify));
	}

	public void Exit()
	{
		if (_loader != null)
		{
			_loader.ExitGame();
			_loader = null;
		}
	}

	public void Dispose()
	{
		StopAllCoroutines();
		if (_loader != null)
		{
			_loader.Dispose();
			_loader = null;
		}
	}

	protected virtual void OnDestroy()
	{
		Dispose();
	}
}
