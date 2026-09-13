using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MiniGame.Core;
using Newtonsoft.Json;

namespace MiniGame.Biubiu;

public static class GameBiubiuShare
{
	private static bool _inited = false;

	private static ConcurrentDictionary<Type, int> _typeToOp = new ConcurrentDictionary<Type, int>();

	private static ConcurrentDictionary<int, Type> _opToType = new ConcurrentDictionary<int, Type>();

	private static JsonSerializerSettings _messageSerializerSettings = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.None,
		NullValueHandling = NullValueHandling.Ignore,
		DefaultValueHandling = DefaultValueHandling.Ignore
	};

	public const int kRuntimeDumpStep = 10;

	public const int kRuntimeDumpMaxSize = 8388608;

	public static GameSerializer Serializer = new GameSerializer(new GameBiubiuJsonTypeBinder(), new List<JsonConverter>
	{
		new S5GameConverter()
	}, new List<IEcsPoolDelegate>
	{
		new ComponentPrefabDelegate(),
		new ComponentPhysicsDelegate(),
		new ComponentPhysicsWorldDelegate(),
		new ComponentUIDelegate()
	});

	public static List<Type> Incs = new List<Type>
	{
		typeof(ComponentResource),
		typeof(ComponentPosition),
		typeof(ComponentRotation),
		typeof(ComponentData)
	};

	public static List<Type> IncsFrame0 = new List<Type>
	{
		typeof(ComponentResource),
		typeof(ComponentPosition),
		typeof(ComponentRotation),
		typeof(ComponentData),
		typeof(ComponentPhysics)
	};

	private static void Init()
	{
		if (!_inited)
		{
			_inited = true;
			RegisterTypeOp(typeof(GameCheckValidation));
			RegisterTypeOp(typeof(GameCheckValidationResp));
			RegisterTypeOp(typeof(GameBiubiuFrameSyncResp));
			RegisterTypeOp(typeof(GameBiubiuFrameSyncResp.CmdCreateBullet));
			RegisterTypeOp(typeof(GameBiubiuFrameSyncResp.ViewGunAim));
			RegisterTypeOp(typeof(GameBiubiuStartResp));
			RegisterTypeOp(typeof(GameBiubiuEndResp));
			RegisterTypeOp(typeof(GameBiubiuEnter));
			RegisterTypeOp(typeof(GameBiubiuEnterResp));
			RegisterTypeOp(typeof(GameBiubiuLeavel));
			RegisterTypeOp(typeof(GameBiubiuLeaveResp));
		}
	}

	public static bool IsOpCode(Type type, int op)
	{
		if (op == 0)
		{
			return false;
		}
		return type.IsAssignableFrom(GetMessageType(op));
	}

	public static bool IsOpCode<T>(int op)
	{
		return IsOpCode(typeof(T), op);
	}

	public static int GetMessageOpCode(Type type)
	{
		if (_typeToOp.TryGetValue(type, out var value))
		{
			return value;
		}
		throw new ArgumentException($"Unknown message type: {type}");
	}

	public static Type GetMessageType(int op)
	{
		if (_opToType.TryGetValue(op, out var value))
		{
			return value;
		}
		throw new ArgumentException($"Unknown message type: {op}");
	}

	private static void RegisterTypeOp(Type type)
	{
		int opCode = GetOpCode(type);
		_typeToOp.TryAdd(type, opCode);
		_opToType.TryAdd(opCode, type);
	}

	private static int GetOpCode(Type type)
	{
		return GetOpCode(type.FullName);
	}

	private static int GetOpCode(string str)
	{
		return GetOpCode(Encoding.UTF8.GetBytes(str));
	}

	private static int GetOpCode(byte[] buf)
	{
		uint num = 2166136261u;
		num = 2166136261u;
		for (int i = 0; i < buf.Length; i++)
		{
			num = (buf[i] ^ num) * 16777619;
		}
		return (int)num;
	}

	public static bool PackMessage(object message, out int opCode, out object data)
	{
		Init();
		if (message == null)
		{
			opCode = 0;
			data = null;
		}
		else
		{
			Type type = message.GetType();
			opCode = GetMessageOpCode(type);
			data = JsonConvert.SerializeObject(message, _messageSerializerSettings);
		}
		return true;
	}

	public static bool UnpackMessage(int opCode, object message, out object data)
	{
		Init();
		if (message != null)
		{
			Type messageType = GetMessageType(opCode);
			data = JsonConvert.DeserializeObject(message as string, messageType, _messageSerializerSettings);
		}
		else
		{
			data = null;
		}
		return true;
	}

	public static SharedRuntime InitSharedEnv(List<ISyncCommand> commands, IResourceLoader resource, SharedRuntime shared = null)
	{
		if (shared == null)
		{
			shared = new SharedRuntime();
		}
		shared.InitData = new SharedInitData
		{
			PlayerInfos = new List<InitPlayerInfo>()
		};
		shared.ResourceLoader = resource;
		shared.Commands = new ServiceCommand(commands);
		shared.SharedSnapshotType = SharedSnapshotType.Shared;
		return shared;
	}

	public static void InitComponents(GameWorld world)
	{
		world.World.InitPool<ComponentTemplate>().InitPool<ComponentUniqueID>().InitPool<ComponentActivedUniqueID>()
			.InitPool<ComponentUniqueIDManager>()
			.InitPool<ComponentActivitySnapshot>()
			.InitPool<ComponentActivityChanged>()
			.InitPool<ComponentPhysics>()
			.InitPool<ComponentPhysicsWorld>()
			.InitPool<ComponentPhysicsSync>()
			.InitPool<ComponentTriggers>()
			.InitPool<ComponentActivedTriggers>()
			.InitPool<ComponentEventManager>()
			.InitPool<ComponentPosition>()
			.InitPool<ComponentRotation>()
			.InitPool<ComponentScale>()
			.InitPool<ComponentTime>()
			.InitPool<ComponentComposeEntityRoot>()
			.InitPool<ComponentComposeEntity>()
			.InitPool<ComponentResource>()
			.InitPool<ComponentPrefabClient>()
			.InitPool<ComponentGunReload>()
			.InitPool<ComponentControllerClient>()
			.InitPool<ComponentPlayer>()
			.InitPool<ComponentEnemy>()
			.InitPool<ComponentTile>()
			.InitPool<ComponentBullet>()
			.InitPool<ComponentDecoration>()
			.InitPool<ComponentVelocity>()
			.InitPool<ComponentPathVelocity>()
			.InitPool<ComponentStop>()
			.InitPool<ComponentUIClient>()
			.InitPool<ComponentMovablePrediction>()
			.InitPool<ComponentData>()
			.InitPool<ComponentPhysicsWorldSimulate>()
			.InitPool<ComponentPhysicsSimulate>()
			.InitPool<ComponentVelocityTarget>();
	}

	public static void InitSystems(GameWorld world)
	{
		world.LogicSystems.Add(new SystemTemplateInstantiate()).Add(new SystemPlayerInit()).Add(new SystemActivity())
			.Add(new SystemUniqueID())
			.Add(new SystemCommand())
			.Add(new SystemGun())
			.Add(new SystemTime())
			.Add(new SystemTrigger())
			.Add(new SystemEvent())
			.Add(new SystemLevel())
			.Add(new SystemGameOver());
		world.PhysicsSystems.Add(new SystemPhysics()).Add(new SystemBulletPhysicsKeepSpeed()).Add(new SystemMove());
	}

	public static GameBiubiuVerify GetValidationResult(EcsWorld world)
	{
		return new GameBiubiuVerify
		{
			AliveEntities = world.TakeEntitiesStates<Inc<ComponentResource>, Exc<ComponentUIClient, ComponentTile>>(Incs),
			LogicTickCount = world.GetShared<IGameSharedEnv>().LogicTickCount,
			PhysicsTickCount = world.GetShared<IGameSharedEnv>().PhysicsTickCount
		};
	}

	public static string GetValidationResultJson(EcsWorld world)
	{
		return world.GetShared<IGameSharedEnv>().ResourceLoader.GetSerializer().ToJson(GetValidationResult(world));
	}

	public static string GetValidationResultMD5(EcsWorld world)
	{
		IGameSerializer serializer = world.GetShared<IGameSharedEnv>().ResourceLoader.GetSerializer();
		string validationResultJson = GetValidationResultJson(world);
		return serializer.GetMD5(validationResultJson);
	}

	public static SystemRuntimeDumpFile CreateRuntimeDumpSystem(string file)
	{
		return new SystemRuntimeDumpFile(file, Incs, null, (default(Inc<ComponentResource>), default(Exc<ComponentUIClient, ComponentTile>)), IncsFrame0, null, (default(Inc<ComponentResource>), default(Exc<ComponentUIClient>)), 10, 8388608);
	}
}
