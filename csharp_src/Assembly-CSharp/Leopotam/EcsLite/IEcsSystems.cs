using System;
using System.Collections.Generic;

namespace Leopotam.EcsLite;

public interface IEcsSystems
{
	IEcsSystems AddWorld(EcsWorld world, string name);

	EcsWorld GetWorld(string name = null);

	Dictionary<string, EcsWorld> GetAllNamedWorlds();

	IEcsSystems Add(IEcsSystem system);

	List<IEcsSystem> GetAllSystems();

	void Init();

	void Run();

	void Destroy();

	void RunProfiler(Action<Type> begin, Action<Type> end);
}
