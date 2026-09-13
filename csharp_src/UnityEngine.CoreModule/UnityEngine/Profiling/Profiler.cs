using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Profiling;

[NativeHeader("Runtime/Profiler/Profiler.h")]
[UsedByNativeCode]
[NativeHeader("Runtime/Utilities/MemoryUtilities.h")]
[NativeHeader("Runtime/Profiler/ScriptBindings/Profiler.bindings.h")]
[MovedFrom("UnityEngine")]
[NativeHeader("Runtime/Allocator/MemoryManager.h")]
[NativeHeader("Runtime/ScriptingBackend/ScriptingApi.h")]
public sealed class Profiler
{
	internal const uint invalidProfilerArea = uint.MaxValue;

	public static extern bool supported
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "profiler_is_available", IsFreeFunction = true)]
		get;
	}

	[StaticAccessor("ProfilerBindings", StaticAccessorType.DoubleColon)]
	public static extern string logFile
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public static extern bool enableBinaryLog
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::IsBinaryLogEnabled", IsFreeFunction = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::SetBinaryLogEnabled", IsFreeFunction = true)]
		set;
	}

	public static extern int maxUsedMemory
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::GetMaxUsedMemory", IsFreeFunction = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::SetMaxUsedMemory", IsFreeFunction = true)]
		set;
	}

	public static extern bool enabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeConditional("ENABLE_PROFILER")]
		[NativeMethod(Name = "profiler_is_enabled", IsFreeFunction = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::SetProfilerEnabled", IsFreeFunction = true)]
		set;
	}

	public static extern bool enableAllocationCallstacks
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::IsAllocationCallstackCaptureEnabled", IsFreeFunction = true)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "ProfilerBindings::SetAllocationCallstackCaptureEnabled", IsFreeFunction = true)]
		set;
	}

	public static int areaCount => Enum.GetNames(typeof(ProfilerArea)).Length;

	[Obsolete("maxNumberOfSamplesPerFrame has been depricated. Use maxUsedMemory instead")]
	public static int maxNumberOfSamplesPerFrame
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	[Obsolete("usedHeapSize has been deprecated since it is limited to 4GB. Please use usedHeapSizeLong instead.")]
	public static uint usedHeapSize => (uint)usedHeapSizeLong;

	public static extern long usedHeapSizeLong
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(Name = "GetUsedHeapSize", IsFreeFunction = true)]
		get;
	}

	private Profiler()
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("profiler_set_area_enabled")]
	[Conditional("ENABLE_PROFILER")]
	public static extern void SetAreaEnabled(ProfilerArea area, bool enabled);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_PROFILER")]
	[FreeFunction("profiler_is_area_enabled")]
	public static extern bool GetAreaEnabled(ProfilerArea area);

	[Conditional("UNITY_EDITOR")]
	public static void AddFramesFromFile(string file)
	{
		if (string.IsNullOrEmpty(file))
		{
			Debug.LogError("AddFramesFromFile: Invalid or empty path");
		}
		else
		{
			AddFramesFromFile_Internal(file, keepExistingFrames: true);
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeHeader("Modules/ProfilerEditor/Public/ProfilerSession.h")]
	[StaticAccessor("profiling::GetProfilerSessionPtr()", StaticAccessorType.Arrow)]
	[NativeMethod(Name = "LoadFromFile")]
	[NativeConditional("ENABLE_PROFILER && UNITY_EDITOR")]
	private static extern void AddFramesFromFile_Internal(string file, bool keepExistingFrames);

	[Conditional("ENABLE_PROFILER")]
	public static void BeginThreadProfiling(string threadGroupName, string threadName)
	{
		if (string.IsNullOrEmpty(threadGroupName))
		{
			throw new ArgumentException("Argument should be a valid string", "threadGroupName");
		}
		if (string.IsNullOrEmpty(threadName))
		{
			throw new ArgumentException("Argument should be a valid string", "threadName");
		}
		BeginThreadProfilingInternal(threadGroupName, threadName);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_PROFILER")]
	[NativeMethod(Name = "ProfilerBindings::BeginThreadProfiling", IsFreeFunction = true, IsThreadSafe = true)]
	private static extern void BeginThreadProfilingInternal(string threadGroupName, string threadName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ProfilerBindings::EndThreadProfiling", IsFreeFunction = true, IsThreadSafe = true)]
	[NativeConditional("ENABLE_PROFILER")]
	public static extern void EndThreadProfiling();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("ENABLE_PROFILER")]
	public static void BeginSample(string name)
	{
		ValidateArguments(name);
		BeginSampleImpl(name, null);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	[Conditional("ENABLE_PROFILER")]
	public static void BeginSample(string name, Object targetObject)
	{
		ValidateArguments(name);
		BeginSampleImpl(name, targetObject);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ValidateArguments(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("Argument should be a valid string.", "name");
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ProfilerBindings::BeginSample", IsFreeFunction = true, IsThreadSafe = true)]
	private static extern void BeginSampleImpl(string name, Object targetObject);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ProfilerBindings::EndSample", IsFreeFunction = true, IsThreadSafe = true)]
	[Conditional("ENABLE_PROFILER")]
	public static extern void EndSample();

	[Obsolete("GetRuntimeMemorySize has been deprecated since it is limited to 2GB. Please use GetRuntimeMemorySizeLong() instead.")]
	public static int GetRuntimeMemorySize(Object o)
	{
		return (int)GetRuntimeMemorySizeLong(o);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ProfilerBindings::GetRuntimeMemorySizeLong", IsFreeFunction = true)]
	public static extern long GetRuntimeMemorySizeLong([NotNull] Object o);

	[Obsolete("GetMonoHeapSize has been deprecated since it is limited to 4GB. Please use GetMonoHeapSizeLong() instead.")]
	public static uint GetMonoHeapSize()
	{
		return (uint)GetMonoHeapSizeLong();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "scripting_gc_get_heap_size", IsFreeFunction = true)]
	public static extern long GetMonoHeapSizeLong();

	[Obsolete("GetMonoUsedSize has been deprecated since it is limited to 4GB. Please use GetMonoUsedSizeLong() instead.")]
	public static uint GetMonoUsedSize()
	{
		return (uint)GetMonoUsedSizeLong();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "scripting_gc_get_used_size", IsFreeFunction = true)]
	public static extern long GetMonoUsedSizeLong();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetMemoryManager()", StaticAccessorType.Dot)]
	[NativeConditional("ENABLE_MEMORY_MANAGER")]
	public static extern bool SetTempAllocatorRequestedSize(uint size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_MEMORY_MANAGER")]
	[StaticAccessor("GetMemoryManager()", StaticAccessorType.Dot)]
	public static extern uint GetTempAllocatorSize();

	[Obsolete("GetTotalAllocatedMemory has been deprecated since it is limited to 4GB. Please use GetTotalAllocatedMemoryLong() instead.")]
	public static uint GetTotalAllocatedMemory()
	{
		return (uint)GetTotalAllocatedMemoryLong();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_MEMORY_MANAGER")]
	[NativeMethod(Name = "GetTotalAllocatedMemory")]
	[StaticAccessor("GetMemoryManager()", StaticAccessorType.Dot)]
	public static extern long GetTotalAllocatedMemoryLong();

	[Obsolete("GetTotalUnusedReservedMemory has been deprecated since it is limited to 4GB. Please use GetTotalUnusedReservedMemoryLong() instead.")]
	public static uint GetTotalUnusedReservedMemory()
	{
		return (uint)GetTotalUnusedReservedMemoryLong();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_MEMORY_MANAGER")]
	[StaticAccessor("GetMemoryManager()", StaticAccessorType.Dot)]
	[NativeMethod(Name = "GetTotalUnusedReservedMemory")]
	public static extern long GetTotalUnusedReservedMemoryLong();

	[Obsolete("GetTotalReservedMemory has been deprecated since it is limited to 4GB. Please use GetTotalReservedMemoryLong() instead.")]
	public static uint GetTotalReservedMemory()
	{
		return (uint)GetTotalReservedMemoryLong();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetMemoryManager()", StaticAccessorType.Dot)]
	[NativeConditional("ENABLE_MEMORY_MANAGER")]
	[NativeMethod(Name = "GetTotalReservedMemory")]
	public static extern long GetTotalReservedMemoryLong();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetMemoryManager()", StaticAccessorType.Dot)]
	[NativeConditional("ENABLE_PROFILER")]
	[NativeMethod(Name = "GetRegisteredGFXDriverMemory")]
	public static extern long GetAllocatedMemoryForGraphicsDriver();

	[Conditional("ENABLE_PROFILER")]
	public static void EmitFrameMetaData(Guid id, int tag, Array data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		Type elementType = data.GetType().GetElementType();
		if (!UnsafeUtility.IsBlittable(elementType))
		{
			throw new ArgumentException($"{elementType} type used in Profiler.ReportFrameStats must be blittable");
		}
		Internal_EmitFrameMetaData_Array(id.ToByteArray(), tag, data, data.Length, UnsafeUtility.SizeOf(elementType));
	}

	[Conditional("ENABLE_PROFILER")]
	public static void EmitFrameMetaData<T>(Guid id, int tag, List<T> data) where T : struct
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		Type typeFromHandle = typeof(T);
		if (!UnsafeUtility.IsBlittable(typeof(T)))
		{
			throw new ArgumentException($"{typeFromHandle} type used in Profiler.ReportFrameStats must be blittable");
		}
		Internal_EmitFrameMetaData_Array(id.ToByteArray(), tag, NoAllocHelpers.ExtractArrayFromList(data), data.Count, UnsafeUtility.SizeOf(typeFromHandle));
	}

	[Conditional("ENABLE_PROFILER")]
	public unsafe static void EmitFrameMetaData<T>(Guid id, int tag, NativeArray<T> data) where T : struct
	{
		Internal_EmitFrameMetaData_Native(id.ToByteArray(), tag, (IntPtr)data.GetUnsafeReadOnlyPtr(), data.Length, UnsafeUtility.SizeOf<T>());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(Name = "ProfilerBindings::Internal_EmitFrameMetaData_Array", IsFreeFunction = true)]
	[ThreadSafe]
	[NativeConditional("ENABLE_PROFILER")]
	private static extern void Internal_EmitFrameMetaData_Array(byte[] id, int tag, Array data, int count, int elementSize);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_PROFILER")]
	[NativeMethod(Name = "ProfilerBindings::Internal_EmitFrameMetaData_Native", IsFreeFunction = true)]
	[ThreadSafe]
	private static extern void Internal_EmitFrameMetaData_Native(byte[] id, int tag, IntPtr data, int count, int elementSize);
}
