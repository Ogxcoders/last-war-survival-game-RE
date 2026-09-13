using System;
using System.Runtime.CompilerServices;
using Unity.Jobs;
using UnityEngine.Bindings;

namespace UnityEngine.Experimental.AI;

[StaticAccessor("NavMeshWorldBindings", StaticAccessorType.DoubleColon)]
public struct NavMeshWorld
{
	internal IntPtr world;

	public bool IsValid()
	{
		return world != IntPtr.Zero;
	}

	public static NavMeshWorld GetDefaultWorld()
	{
		GetDefaultWorld_Injected(out var ret);
		return ret;
	}

	private static void AddDependencyInternal(IntPtr navmesh, JobHandle handle)
	{
		AddDependencyInternal_Injected(navmesh, ref handle);
	}

	public void AddDependency(JobHandle job)
	{
		AddDependencyInternal(world, job);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetDefaultWorld_Injected(out NavMeshWorld ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void AddDependencyInternal_Injected(IntPtr navmesh, ref JobHandle handle);
}
