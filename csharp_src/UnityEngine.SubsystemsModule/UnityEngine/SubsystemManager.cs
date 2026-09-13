using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeType(Header = "Modules/Subsystems/SubsystemManager.h")]
public static class SubsystemManager
{
	public static event Action reloadSubsytemsStarted;

	public static event Action reloadSubsytemsCompleted;

	static SubsystemManager()
	{
		StaticConstructScriptingClassMap();
	}

	public static void GetAllSubsystemDescriptors(List<ISubsystemDescriptor> descriptors)
	{
		descriptors.Clear();
		foreach (ISubsystemDescriptorImpl s_IntegratedSubsystemDescriptor in Internal_SubsystemDescriptors.s_IntegratedSubsystemDescriptors)
		{
			descriptors.Add(s_IntegratedSubsystemDescriptor);
		}
		foreach (ISubsystemDescriptor s_StandaloneSubsystemDescriptor in Internal_SubsystemDescriptors.s_StandaloneSubsystemDescriptors)
		{
			descriptors.Add(s_StandaloneSubsystemDescriptor);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void ReportSingleSubsystemAnalytics(string id);

	public static void GetSubsystemDescriptors<T>(List<T> descriptors) where T : ISubsystemDescriptor
	{
		descriptors.Clear();
		foreach (ISubsystemDescriptorImpl s_IntegratedSubsystemDescriptor in Internal_SubsystemDescriptors.s_IntegratedSubsystemDescriptors)
		{
			if (s_IntegratedSubsystemDescriptor is T)
			{
				descriptors.Add((T)s_IntegratedSubsystemDescriptor);
			}
		}
		foreach (ISubsystemDescriptor s_StandaloneSubsystemDescriptor in Internal_SubsystemDescriptors.s_StandaloneSubsystemDescriptors)
		{
			if (s_StandaloneSubsystemDescriptor is T)
			{
				descriptors.Add((T)s_StandaloneSubsystemDescriptor);
			}
		}
	}

	public static void GetInstances<T>(List<T> instances) where T : ISubsystem
	{
		instances.Clear();
		foreach (ISubsystem s_IntegratedSubsystemInstance in Internal_SubsystemInstances.s_IntegratedSubsystemInstances)
		{
			if (s_IntegratedSubsystemInstance is T)
			{
				instances.Add((T)s_IntegratedSubsystemInstance);
			}
		}
		foreach (ISubsystem s_StandaloneSubsystemInstance in Internal_SubsystemInstances.s_StandaloneSubsystemInstances)
		{
			if (s_StandaloneSubsystemInstance is T)
			{
				instances.Add((T)s_StandaloneSubsystemInstance);
			}
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void DestroyInstance_Internal(IntPtr instancePtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void StaticConstructScriptingClassMap();

	[RequiredByNativeCode]
	private static void Internal_ReloadSubsystemsStarted()
	{
		if (SubsystemManager.reloadSubsytemsStarted != null)
		{
			SubsystemManager.reloadSubsytemsStarted();
		}
	}

	[RequiredByNativeCode]
	private static void Internal_ReloadSubsystemsCompleted()
	{
		if (SubsystemManager.reloadSubsytemsCompleted != null)
		{
			SubsystemManager.reloadSubsytemsCompleted();
		}
	}
}
