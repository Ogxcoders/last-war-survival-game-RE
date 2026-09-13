using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine;

internal static class Internal_SubsystemDescriptors
{
	internal static List<ISubsystemDescriptorImpl> s_IntegratedSubsystemDescriptors = new List<ISubsystemDescriptorImpl>();

	internal static List<ISubsystemDescriptor> s_StandaloneSubsystemDescriptors = new List<ISubsystemDescriptor>();

	[RequiredByNativeCode]
	internal static bool Internal_AddDescriptor(SubsystemDescriptor descriptor)
	{
		foreach (ISubsystemDescriptor s_StandaloneSubsystemDescriptor in s_StandaloneSubsystemDescriptors)
		{
			if (s_StandaloneSubsystemDescriptor == descriptor)
			{
				return false;
			}
		}
		s_StandaloneSubsystemDescriptors.Add(descriptor);
		SubsystemManager.ReportSingleSubsystemAnalytics(descriptor.id);
		return true;
	}

	[RequiredByNativeCode]
	internal static void Internal_InitializeManagedDescriptor(IntPtr ptr, ISubsystemDescriptorImpl desc)
	{
		desc.ptr = ptr;
		s_IntegratedSubsystemDescriptors.Add(desc);
	}

	[RequiredByNativeCode]
	internal static void Internal_ClearManagedDescriptors()
	{
		foreach (ISubsystemDescriptorImpl s_IntegratedSubsystemDescriptor in s_IntegratedSubsystemDescriptors)
		{
			s_IntegratedSubsystemDescriptor.ptr = IntPtr.Zero;
		}
		s_IntegratedSubsystemDescriptors.Clear();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern IntPtr Create(IntPtr descriptorPtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string GetId(IntPtr descriptorPtr);
}
