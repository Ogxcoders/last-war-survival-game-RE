using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.Profiling;

[UsedByNativeCode]
[NativeHeader("Runtime/Profiler/ScriptBindings/ProfilerMarker.bindings.h")]
public struct ProfilerMarker
{
	[UsedByNativeCode]
	public struct AutoScope : IDisposable
	{
		[NativeDisableUnsafePtrRestriction]
		internal readonly IntPtr m_Ptr;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal AutoScope(IntPtr markerPtr)
		{
			m_Ptr = markerPtr;
			Internal_Begin(markerPtr);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			Internal_End(m_Ptr);
		}
	}

	[NativeDisableUnsafePtrRestriction]
	internal readonly IntPtr m_Ptr;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ProfilerMarker(string name)
	{
		m_Ptr = Internal_Create(name, 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("ENABLE_PROFILER")]
	public void Begin()
	{
		Internal_Begin(m_Ptr);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("ENABLE_PROFILER")]
	public void Begin(UnityEngine.Object contextUnityObject)
	{
		Internal_BeginWithObject(m_Ptr, contextUnityObject);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("ENABLE_PROFILER")]
	public void End()
	{
		Internal_End(m_Ptr);
	}

	[Conditional("ENABLE_PROFILER")]
	internal void GetName(ref string name)
	{
		name = Internal_GetName(m_Ptr);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public AutoScope Auto()
	{
		return new AutoScope(m_Ptr);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	[NativeConditional("ENABLE_PROFILER")]
	internal static extern IntPtr Internal_Create(string name, ushort flags);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_PROFILER")]
	[ThreadSafe]
	internal static extern void Internal_Begin(IntPtr markerPtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_PROFILER")]
	[ThreadSafe]
	internal static extern void Internal_BeginWithObject(IntPtr markerPtr, UnityEngine.Object contextUnityObject);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	[NativeConditional("ENABLE_PROFILER")]
	internal static extern void Internal_End(IntPtr markerPtr);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	[NativeConditional("ENABLE_PROFILER")]
	internal unsafe static extern void Internal_Emit(IntPtr markerPtr, ushort eventType, int metadataCount, void* metadata);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_PROFILER")]
	[ThreadSafe]
	private static extern string Internal_GetName(IntPtr markerPtr);
}
