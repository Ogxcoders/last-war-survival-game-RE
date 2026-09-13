using Unity.IL2CPP.CompilerServices;

namespace Leopotam.EcsLite.ExtendedSystems;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public static class Extensions
{
	public static IEcsSystems AddGroup(this IEcsSystems systems, string groupName, bool defaultState, string eventWorldName, params IEcsSystem[] nestedSystems)
	{
		return systems.Add(new EcsGroupSystem(groupName, defaultState, eventWorldName, nestedSystems));
	}

	public static IEcsSystems DelHere<T>(this IEcsSystems systems, string worldName = null) where T : struct
	{
		return systems.Add(new DelHereSystem<T>(systems.GetWorld(worldName)));
	}
}
