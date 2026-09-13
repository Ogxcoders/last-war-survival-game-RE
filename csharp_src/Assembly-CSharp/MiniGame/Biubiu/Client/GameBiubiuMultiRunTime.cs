using Box2DSharp.Common;
using Joker;
using Leopotam.EcsLite;
using MiniGame.Core;
using MiniGame.Core.Server;
using UnityEngine;

namespace MiniGame.Biubiu.Client;

public class GameBiubiuMultiRunTime : GameBiuBiuRunTime
{
	public const float WAITGOWIFTTIME = 30f;

	private float _curWaitWifiTime;

	private void Start()
	{
		GlobalMonobehaviourDispatcher.onApplicationPause += OnAppPauseHandle;
	}

	protected override void OnDestroy()
	{
		GlobalMonobehaviourDispatcher.onApplicationPause -= OnAppPauseHandle;
		base.OnDestroy();
	}

	public override void InitRunTime(GameWorld gameWorld, GameLevel gameLevel, bool verify = true)
	{
		SharedRuntime env = GetEnv(gameWorld);
		GameBiuBiuPlayerBase player = GetPlayer(gameWorld);
		env.InitData.PlayerID = player.PlayerID;
		gameWorld.Env.LogicTickLockStep = -1;
		GameBiubiuClient.InitClient(gameWorld, env.GameType, gameLevel, verify: false, input: false);
		gameWorld.ViewSystems.Add(new SystemClientLockStepInput());
	}

	public new void Update()
	{
		if (GameEntry.GlobalData != null && GameEntry.GlobalData.isInBackGround)
		{
			return;
		}
		ProcessMessages();
		base.Update();
		GameWorld gameWorld = base.Loader?.GameWorld;
		if (gameWorld == null || GetEnv(gameWorld).GameType != EGameType.PvpClient)
		{
			return;
		}
		GameBiuBiuPlayerBase player = ((GameBiubiuPlayerEnv)base.LoadEnv).Player;
		if (player == null)
		{
			return;
		}
		GameObject gameObject = (gameWorld.Env.ResourceLoader as GameLoader)?.LoaderEnv.WifiGo;
		if (!player.IsConnected)
		{
			if (!player.RoomEnd)
			{
				if (gameObject != null && !gameObject.activeSelf)
				{
					gameObject.SetActive(value: true);
				}
				_curWaitWifiTime += Time.unscaledDeltaTime;
			}
			else
			{
				if (gameObject != null && gameObject.activeSelf)
				{
					gameObject.SetActive(value: false);
				}
				_curWaitWifiTime = 0f;
			}
		}
		else
		{
			if (gameObject != null && gameObject.activeSelf)
			{
				gameObject.SetActive(value: false);
			}
			_curWaitWifiTime = 0f;
		}
		if (_curWaitWifiTime >= 30f)
		{
			_curWaitWifiTime = -99999f;
			player.WifiTimeOut();
		}
	}

	private void OnAppPauseHandle(bool pauseStatus)
	{
		GameWorld gameWorld = base.Loader?.GameWorld;
		if (gameWorld == null)
		{
			return;
		}
		GameBiuBiuPlayerBase player = ((GameBiubiuPlayerEnv)base.LoadEnv).Player;
		if (player != null && GetEnv(gameWorld).GameType == EGameType.PvpClient)
		{
			if (!pauseStatus)
			{
				player.AppResume();
			}
			else
			{
				player.AppPause();
			}
		}
	}

	private GameBiuBiuPlayerBase GetPlayer(GameWorld world)
	{
		_ = GetEnv(world).Commands;
		return ((GameBiubiuPlayerEnv)base.LoadEnv).Player;
	}

	private SharedRuntime GetEnv(GameWorld world)
	{
		return (SharedRuntime)world.Env;
	}

	private void QuickSyncServerFrame(int tickFrame)
	{
		if (tickFrame < 60)
		{
			return;
		}
		GameWorld gameWorld = base.Loader?.GameWorld;
		if (gameWorld == null)
		{
			return;
		}
		gameWorld.Update(gameWorld.Env.LogicTickDelta.AsFloat * (float)tickFrame);
		EcsFilter ecsFilter = gameWorld.World.Filter<ComponentMovablePrediction>().End();
		EcsPool<ComponentMovablePrediction> pool = gameWorld.World.GetPool<ComponentMovablePrediction>();
		foreach (int item in ecsFilter)
		{
			pool.Del(item);
		}
	}

	private void ProcessMessages()
	{
		GameWorld gameWorld = base.Loader?.GameWorld;
		if (gameWorld == null)
		{
			return;
		}
		ServiceCommand commands = ((SharedRuntime)gameWorld.Env).Commands;
		GameBiuBiuPlayerBase player = ((GameBiubiuPlayerEnv)base.LoadEnv).Player;
		(IMessage, object) message;
		while (player.TryDequeueMessage(out message))
		{
			if (!(message.Item1 is RoomMessage roomMessage))
			{
				Debug.LogError($"Unsupported message type {message.Item1?.GetType()}");
			}
			else if (roomMessage.Data is GameBiubiuFrameSyncResp gameBiubiuFrameSyncResp)
			{
				int logicTickLockStep = gameWorld.Env.LogicTickLockStep;
				gameWorld.Env.LogicTickLockStep = gameBiubiuFrameSyncResp.LogicTickLockStep;
				if (gameBiubiuFrameSyncResp.Commands != null)
				{
					for (int i = 0; i < gameBiubiuFrameSyncResp.Commands.Count; i++)
					{
						GameBiubiuFrameSyncResp.CmdCreateBullet cmdCreateBullet = gameBiubiuFrameSyncResp.Commands[i];
						CommandCreateBullet commandCreateBullet = new CommandCreateBullet
						{
							FrameIndex = cmdCreateBullet.FrameIndex,
							EntityID = cmdCreateBullet.EntityID,
							BodyPosition = new FVector2(FP.FromRaw(cmdCreateBullet.BodyPosX), FP.FromRaw(cmdCreateBullet.BodyPosY)),
							Position = new FVector2(FP.FromRaw(cmdCreateBullet.PositionX), FP.FromRaw(cmdCreateBullet.PositionY)),
							Direction = new FVector2(FP.FromRaw(cmdCreateBullet.DirectionX), FP.FromRaw(cmdCreateBullet.DirectionY))
						};
						commands.TryQueueEvent(commandCreateBullet);
					}
				}
				int tickFrame = gameBiubiuFrameSyncResp.LogicTickLockStep - logicTickLockStep - 1;
				QuickSyncServerFrame(tickFrame);
			}
			else if (roomMessage.Data is GameBiubiuFrameSyncResp.ViewGunAim viewGunAim)
			{
				EcsFilter ecsFilter = gameWorld.World.Filter<ComponentPlayer>().End();
				EcsPool<ComponentControllerClient> pool = gameWorld.World.GetPool<ComponentControllerClient>();
				foreach (int item in ecsFilter)
				{
					if (gameWorld.World.GetPool<ComponentPlayer>().Get(item).PlayerID == (EPlayerID)viewGunAim.PlayerID)
					{
						if (!pool.Has(item))
						{
							return;
						}
						DataUIPlayerController controller = pool.Get(item).GetController(gameWorld.World, item);
						if (controller == null)
						{
							return;
						}
						controller.GunAimPositionSyn = new Vector3(viewGunAim.X, viewGunAim.Y, viewGunAim.Z);
					}
				}
			}
			else if (!(roomMessage.Data is GameBiubiuEndResp) && !(roomMessage.Data is GameBiubiuEnterResp))
			{
				Debug.LogError($"Unsupported message type {message.Item1?.GetType()}");
			}
		}
	}
}
