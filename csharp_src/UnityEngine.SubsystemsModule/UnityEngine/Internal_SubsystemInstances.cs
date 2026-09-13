using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UnityEngine;

internal static class Internal_SubsystemInstances
{
	internal static List<ISubsystem> s_IntegratedSubsystemInstances = new List<ISubsystem>();

	internal static List<ISubsystem> s_StandaloneSubsystemInstances = new List<ISubsystem>();

	[RequiredByNativeCode]
	internal static void Internal_InitializeManagedInstance(IntPtr ptr, IntegratedSubsystem inst)
	{
		inst.m_Ptr = ptr;
		inst.SetHandle(inst);
		s_IntegratedSubsystemInstances.Add(inst);
	}

	[RequiredByNativeCode]
	internal static void Internal_ClearManagedInstances()
	{
		foreach (ISubsystem s_IntegratedSubsystemInstance in s_IntegratedSubsystemInstances)
		{
			((IntegratedSubsystem)s_IntegratedSubsystemInstance).m_Ptr = IntPtr.Zero;
		}
		s_IntegratedSubsystemInstances.Clear();
		s_StandaloneSubsystemInstances.Clear();
	}

	[RequiredByNativeCode]
	internal static void Internal_RemoveInstanceByPtr(IntPtr ptr)
	{
		for (int num = s_IntegratedSubsystemInstances.Count - 1; num >= 0; num--)
		{
			if (((IntegratedSubsystem)s_IntegratedSubsystemInstances[num]).m_Ptr == ptr)
			{
				((IntegratedSubsystem)s_IntegratedSubsystemInstances[num]).m_Ptr = IntPtr.Zero;
				s_IntegratedSubsystemInstances.RemoveAt(num);
			}
		}
	}

	internal static IntegratedSubsystem Internal_GetInstanceByPtr(IntPtr ptr)
	{
		foreach (IntegratedSubsystem s_IntegratedSubsystemInstance in s_IntegratedSubsystemInstances)
		{
			if (s_IntegratedSubsystemInstance.m_Ptr == ptr)
			{
				return s_IntegratedSubsystemInstance;
			}
		}
		return null;
	}

	internal static void Internal_AddStandaloneSubsystem(Subsystem inst)
	{
		s_StandaloneSubsystemInstances.Add(inst);
	}

	internal static Subsystem Internal_FindStandaloneSubsystemInstanceGivenDescriptor(SubsystemDescriptor descriptor)
	{
		foreach (Subsystem s_StandaloneSubsystemInstance in s_StandaloneSubsystemInstances)
		{
			if (s_StandaloneSubsystemInstance.m_subsystemDescriptor == descriptor)
			{
				return s_StandaloneSubsystemInstance;
			}
		}
		return null;
	}
}
