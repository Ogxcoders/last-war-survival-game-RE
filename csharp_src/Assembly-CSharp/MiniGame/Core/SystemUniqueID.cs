using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGame.Core;

public class SystemUniqueID : IEcsInitSystem, IEcsSystem, IEcsRunSystem
{
	private EcsSharedInject<IGameSharedEnv> _shared;

	private EcsFilterInject<Inc<ComponentUniqueID>, Exc<ComponentActivedUniqueID>> _filterRegister;

	private EcsPoolInject<ComponentUniqueIDManager> _poolManager;

	private EcsPoolInject<ComponentUniqueID> _poolUniqueID;

	private EcsPoolInject<ComponentActivedUniqueID> _poolActivedUniqueID;

	public void Init(IEcsSystems systems)
	{
		EcsWorld world = systems.GetWorld();
		if (!_shared.Value.UniqueIDManager.Unpack(world, out var _))
		{
			int entity2 = world.NewEntity();
			_poolManager.Value.Add(entity2).UniqueIDToEntity = new Dictionary<int, int>();
			_shared.Value.UniqueIDManager = world.PackEntity(entity2);
		}
	}

	public void Run(IEcsSystems systems)
	{
		if (!_shared.Value.UniqueIDManager.Unpack(systems.GetWorld(), out var entity))
		{
			throw new Exception("UniqueIDManager not init");
		}
		ref ComponentUniqueIDManager mgr = ref _poolManager.Value.Get(entity);
		foreach (int item in _filterRegister.Value)
		{
			ref ComponentActivedUniqueID reference = ref _poolActivedUniqueID.Value.Add(item);
			ref ComponentUniqueID reference2 = ref _poolUniqueID.Value.Get(item);
			reference.ID = reference2.ID;
			FuncUniqueID.Register(ref mgr, item, reference2.ID);
		}
	}
}
