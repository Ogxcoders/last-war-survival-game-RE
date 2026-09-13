using System;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Jobs;

public static class IJobForExtensions
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct ForJobStruct<T> where T : struct, IJobFor
	{
		public delegate void ExecuteJobFunction(ref T data, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

		public static IntPtr jobReflectionData;

		public static IntPtr Initialize(bool asParallel)
		{
			if (jobReflectionData == IntPtr.Zero)
			{
				jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(T), JobType.ParallelFor, new ExecuteJobFunction(Execute));
			}
			return jobReflectionData;
		}

		public static void Execute(ref T jobData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
		{
			int beginIndex;
			int endIndex;
			while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out beginIndex, out endIndex))
			{
				int num = endIndex;
				for (int i = beginIndex; i < num; i++)
				{
					jobData.Execute(i);
				}
			}
		}
	}

	public unsafe static JobHandle Schedule<T>(this T jobData, int arrayLength, JobHandle dependency) where T : struct, IJobFor
	{
		JobsUtility.JobScheduleParameters parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), ForJobStruct<T>.Initialize(asParallel: false), dependency, ScheduleMode.Batched);
		return JobsUtility.ScheduleParallelFor(ref parameters, arrayLength, arrayLength);
	}

	public unsafe static JobHandle ScheduleParallel<T>(this T jobData, int arrayLength, int innerloopBatchCount, JobHandle dependency) where T : struct, IJobFor
	{
		JobsUtility.JobScheduleParameters parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), ForJobStruct<T>.Initialize(asParallel: true), dependency, ScheduleMode.Batched);
		return JobsUtility.ScheduleParallelFor(ref parameters, arrayLength, innerloopBatchCount);
	}

	public unsafe static void Run<T>(this T jobData, int arrayLength) where T : struct, IJobFor
	{
		JobsUtility.JobScheduleParameters parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), ForJobStruct<T>.Initialize(asParallel: false), default(JobHandle), ScheduleMode.Run);
		JobsUtility.ScheduleParallelFor(ref parameters, arrayLength, arrayLength);
	}
}
