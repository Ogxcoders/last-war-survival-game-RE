using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mono;

internal static class DependencyInjector
{
	private const string TypeName = "Mono.SystemDependencyProvider, System";

	private static object locker = new object();

	private static ISystemDependencyProvider systemDependency;

	internal static ISystemDependencyProvider SystemProvider
	{
		get
		{
			if (systemDependency != null)
			{
				return systemDependency;
			}
			lock (locker)
			{
				if (systemDependency != null)
				{
					return systemDependency;
				}
				systemDependency = ReflectionLoad();
				if (systemDependency == null)
				{
					throw new PlatformNotSupportedException("Cannot find 'Mono.SystemDependencyProvider, System' dependency");
				}
				return systemDependency;
			}
		}
	}

	internal static void Register(ISystemDependencyProvider provider)
	{
		lock (locker)
		{
			if (systemDependency != null && systemDependency != provider)
			{
				throw new InvalidOperationException();
			}
			systemDependency = provider;
		}
	}

	[PreserveDependency("get_Instance()", "Mono.SystemDependencyProvider", "System")]
	private static ISystemDependencyProvider ReflectionLoad()
	{
		Type type = Type.GetType("Mono.SystemDependencyProvider, System");
		if (type == null)
		{
			return null;
		}
		PropertyInfo property = type.GetProperty("Instance", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
		if (property == null)
		{
			return null;
		}
		return (ISystemDependencyProvider)property.GetValue(null);
	}
}
