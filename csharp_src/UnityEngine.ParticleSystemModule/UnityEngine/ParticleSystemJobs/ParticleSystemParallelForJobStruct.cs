using System;
using System.Runtime.InteropServices;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct ParticleSystemParallelForJobStruct<T> where T : struct, IJobParticleSystemParallelFor
{
	public delegate void ExecuteJobFunction(ref T data, IntPtr listDataPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

	public static IntPtr jobReflectionData;

	public static IntPtr Initialize()
	{
		if (jobReflectionData == IntPtr.Zero)
		{
			jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(T), JobType.ParallelFor, new ExecuteJobFunction(Execute));
		}
		return jobReflectionData;
	}

	public unsafe static void Execute(ref T data, IntPtr listDataPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
	{
		NativeListData* ptr = (NativeListData*)(void*)listDataPtr;
		ParticleSystem.CopyManagedJobData(ptr->system, out var particleData);
		ParticleSystemJobData jobData = new ParticleSystemJobData(ref particleData);
		int beginIndex;
		int endIndex;
		while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out beginIndex, out endIndex))
		{
			for (int i = beginIndex; i < endIndex; i++)
			{
				data.Execute(jobData, i);
			}
		}
	}
}
