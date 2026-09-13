using System.Collections.Generic;
using Leopotam.EcsLite;
using MiniGame.Core;
using Newtonsoft.Json;

namespace MiniGame.Biubiu.Client;

public class GameBiubiuClient
{
	public static GameSerializer Serializer = new GameSerializer(new GameBiubiuJsonTypeBinder(), new List<JsonConverter>
	{
		new GameObjectConverter(),
		new S5GameConverter()
	}, new List<IEcsPoolDelegate>
	{
		new ComponentPrefabClientDelegate(),
		new ComponentControllerClientDelegate(),
		new ComponentPhysicsWorldDelegate(),
		new ComponentPhysicsDelegate(),
		new ComponentUIClientDelegate()
	});

	public static GameBiubiuEnvClient InitSharedEnv(List<ISyncCommand> commands, IResourceLoader resource)
	{
		GameBiubiuEnvClient gameBiubiuEnvClient = new GameBiubiuEnvClient();
		GameBiubiuShare.InitSharedEnv(commands, resource, gameBiubiuEnvClient);
		return gameBiubiuEnvClient;
	}

	public static GameBiubiuEnvClient InitSharedEnvEditable(IResourceLoader resource)
	{
		return new GameBiubiuEnvClient
		{
			ResourceLoader = resource,
			SharedSnapshotType = SharedSnapshotType.Map
		};
	}

	public static void InitClient(GameWorld world, EGameType gameType, GameLevel gameLevel = null, bool verify = false, bool input = true)
	{
		GameBiubiuShare.InitComponents(world);
		gameLevel?.LoadLevel(world);
		GameBiubiuShare.InitSystems(world);
		FuncTCClient.InitAction();
		world.ViewSystems.Add(new SystemClientGame());
		if (gameType == EGameType.PvpClient)
		{
			world.ViewSystems.Add(new SystemClientPhysicsSimulate()).Add(new SystemClientPrefabMovablePredictionSimulate());
		}
		else
		{
			world.ViewSystems.Add(new SystemClientPrefabMovablePrediction());
		}
		world.ViewSystems.Add(new SystemClientPhysicsSync()).Add(new SystemClientUIAdapt()).Add(new SystemClientPlayerSync())
			.Add(new SystemClientPrefab())
			.Add(new SystemClientPlayerInit());
		if (input)
		{
			world.ViewSystems.Add(new SystemClientInput());
		}
		if (SharedRuntime.Dump)
		{
			world.LogicSystems.Add(GameBiubiuShare.CreateRuntimeDumpSystem("DumpClientRuntime.json"));
		}
		if (verify)
		{
			world.SettlementSystems.Add(new SystemClientValidation());
		}
	}

	public static void InitClientEditable(GameWorld world, GameLevel gameLevel = null, SharedConfigData sharedConfigData = null)
	{
		GameBiubiuShare.InitComponents(world);
		world.Env.RestoreSnapshot(sharedConfigData);
		gameLevel?.LoadLevel(world);
		world.PhysicsSystems.Add(new SystemEditorPhysics()).Add(new SystemPhysicsDrawDebug());
		world.LogicSystems.Add(new SystemTemplateEditable());
	}
}
