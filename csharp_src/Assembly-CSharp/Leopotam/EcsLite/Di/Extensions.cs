using System;
using System.Reflection;

namespace Leopotam.EcsLite.Di;

public static class Extensions
{
	public static IEcsSystems Inject(this IEcsSystems systems, params object[] injects)
	{
		if (injects == null)
		{
			injects = Array.Empty<object>();
		}
		foreach (IEcsSystem allSystem in systems.GetAllSystems())
		{
			InjectToSystem(allSystem, systems, injects);
		}
		return systems;
	}

	public static void InjectToSystem(IEcsSystem system, IEcsSystems systems, object[] injects)
	{
		FieldInfo[] fields = system.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!fieldInfo.IsStatic && !InjectBuiltIns(fieldInfo, system, systems))
			{
				InjectCustoms(fieldInfo, system, injects);
			}
		}
	}

	private static bool InjectBuiltIns(FieldInfo fieldInfo, IEcsSystem system, IEcsSystems systems)
	{
		if (typeof(IEcsDataInject).IsAssignableFrom(fieldInfo.FieldType))
		{
			IEcsDataInject ecsDataInject = (IEcsDataInject)fieldInfo.GetValue(system);
			ecsDataInject.Fill(systems);
			fieldInfo.SetValue(system, ecsDataInject);
			return true;
		}
		return false;
	}

	private static bool InjectCustoms(FieldInfo fieldInfo, IEcsSystem system, object[] injects)
	{
		if (typeof(IEcsCustomDataInject).IsAssignableFrom(fieldInfo.FieldType))
		{
			IEcsCustomDataInject ecsCustomDataInject = (IEcsCustomDataInject)fieldInfo.GetValue(system);
			ecsCustomDataInject.Fill(injects);
			fieldInfo.SetValue(system, ecsCustomDataInject);
			return true;
		}
		return false;
	}

	public static ref T NewEntity<T>(this in EcsPoolInject<T> poolInject, out int entity) where T : struct
	{
		entity = poolInject.Value.GetWorld().NewEntity();
		return ref poolInject.Value.Add(entity);
	}
}
