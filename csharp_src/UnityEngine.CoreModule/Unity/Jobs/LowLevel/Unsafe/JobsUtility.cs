using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace Unity.Jobs.LowLevel.Unsafe;

[NativeHeader("Runtime/Jobs/JobSystem.h")]
[NativeType(Header = "Runtime/Jobs/ScriptBindings/JobsBindings.h")]
public static class JobsUtility
{
	public struct JobScheduleParameters
	{
		public JobHandle Dependency;

		public int ScheduleMode;

		public IntPtr ReflectionData;

		public IntPtr JobDataPtr;

		public unsafe JobScheduleParameters(void* i_jobData, IntPtr i_reflectionData, JobHandle i_dependency, ScheduleMode i_scheduleMode)
		{
			Dependency = i_dependency;
			JobDataPtr = (IntPtr)i_jobData;
			ReflectionData = i_reflectionData;
			ScheduleMode = (int)i_scheduleMode;
		}
	}

	public const int MaxJobThreadCount = 128;

	public const int CacheLineSize = 64;

	public static extern bool IsExecutingJob
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod(IsFreeFunction = true, IsThreadSafe = true)]
		get;
	}

	public static extern bool JobDebuggerEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction]
		set;
	}

	public static extern bool JobCompilerEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction]
		set;
	}

	public static extern int JobWorkerMaximumCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[FreeFunction("JobSystem::GetJobQueueMaximumThreadCount")]
		get;
	}

	public static int JobWorkerCount
	{
		get
		{
			return GetJobQueueWorkerThreadCount();
		}
		set
		{
			if (value < 1 || value > JobWorkerMaximumCount)
			{
				throw new ArgumentOutOfRangeException("JobWorkerCount", $"Invalid JobWorkerCount {value} must be in the range 1 -> {JobWorkerMaximumCount}");
			}
			SetJobQueueMaximumActiveThreadCount(value);
		}
	}

	public unsafe static void GetJobRange(ref JobRanges ranges, int jobIndex, out int beginIndex, out int endIndex)
	{
		int* ptr = (int*)(void*)ranges.StartEndIndex;
		beginIndex = ptr[jobIndex * 2];
		endIndex = ptr[jobIndex * 2 + 1];
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsFreeFunction = true, IsThreadSafe = true)]
	public static extern bool GetWorkStealingRange(ref JobRanges ranges, int jobIndex, out int beginIndex, out int endIndex);

	[FreeFunction("ScheduleManagedJob")]
	public static JobHandle Schedule(ref JobScheduleParameters parameters)
	{
		Schedule_Injected(ref parameters, out var ret);
		return ret;
	}

	[FreeFunction("ScheduleManagedJobParallelFor")]
	public static JobHandle ScheduleParallelFor(ref JobScheduleParameters parameters, int arrayLength, int innerloopBatchCount)
	{
		ScheduleParallelFor_Injected(ref parameters, arrayLength, innerloopBatchCount, out var ret);
		return ret;
	}

	[FreeFunction("ScheduleManagedJobParallelForDeferArraySize")]
	public unsafe static JobHandle ScheduleParallelForDeferArraySize(ref JobScheduleParameters parameters, int innerloopBatchCount, void* listData, void* listDataAtomicSafetyHandle)
	{
		ScheduleParallelForDeferArraySize_Injected(ref parameters, innerloopBatchCount, listData, listDataAtomicSafetyHandle, out var ret);
		return ret;
	}

	[FreeFunction("ScheduleManagedJobParallelForTransform")]
	public static JobHandle ScheduleParallelForTransform(ref JobScheduleParameters parameters, IntPtr transfromAccesssArray)
	{
		ScheduleParallelForTransform_Injected(ref parameters, transfromAccesssArray, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod(IsThreadSafe = true, IsFreeFunction = true)]
	[Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
	public unsafe static extern void PatchBufferMinMaxRanges(IntPtr bufferRangePatchData, void* jobdata, int startIndex, int rangeSize);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction]
	private static extern IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, JobType jobType, object managedJobFunction0, object managedJobFunction1, object managedJobFunction2);

	public static IntPtr CreateJobReflectionData(Type type, JobType jobType, object managedJobFunction0, object managedJobFunction1 = null, object managedJobFunction2 = null)
	{
		return CreateJobReflectionData(type, type, jobType, managedJobFunction0, managedJobFunction1, managedJobFunction2);
	}

	public static IntPtr CreateJobReflectionData(Type wrapperJobType, Type userJobType, JobType jobType, object managedJobFunction0)
	{
		return CreateJobReflectionData(wrapperJobType, userJobType, jobType, managedJobFunction0, null, null);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("JobSystem::GetJobQueueWorkerThreadCount")]
	private static extern int GetJobQueueWorkerThreadCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("JobSystem::ForceSetJobQueueWorkerThreadCount")]
	private static extern void SetJobQueueMaximumActiveThreadCount(int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[FreeFunction("JobSystem::ResetJobQueueWorkerThreadCount")]
	public static extern void ResetJobWorkerCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Schedule_Injected(ref JobScheduleParameters parameters, out JobHandle ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ScheduleParallelFor_Injected(ref JobScheduleParameters parameters, int arrayLength, int innerloopBatchCount, out JobHandle ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void ScheduleParallelForDeferArraySize_Injected(ref JobScheduleParameters parameters, int innerloopBatchCount, void* listData, void* listDataAtomicSafetyHandle, out JobHandle ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void ScheduleParallelForTransform_Injected(ref JobScheduleParameters parameters, IntPtr transfromAccesssArray, out JobHandle ret);
}
