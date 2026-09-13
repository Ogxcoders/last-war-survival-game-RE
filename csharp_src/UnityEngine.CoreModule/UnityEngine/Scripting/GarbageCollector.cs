using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Scripting;

[NativeHeader("Runtime/Scripting/GarbageCollector.h")]
public static class GarbageCollector
{
	public enum Mode
	{
		Disabled,
		Enabled
	}

	public static Mode GCMode
	{
		get
		{
			return GetMode();
		}
		set
		{
			if (value != GetMode())
			{
				SetMode(value);
				if (GarbageCollector.GCModeChanged != null)
				{
					GarbageCollector.GCModeChanged(value);
				}
			}
		}
	}

	public static extern bool isIncremental
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetIncrementalEnabled")]
		get;
	}

	public static extern ulong incrementalTimeSliceNanoseconds
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public static event Action<Mode> GCModeChanged;

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeThrows]
	private static extern void SetMode(Mode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern Mode GetMode();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("CollectIncrementalWrapper")]
	[NativeThrows]
	public static extern bool CollectIncremental(ulong nanoseconds);
}
