using System;
using System.Runtime.InteropServices;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs;

[StructLayout(LayoutKind.Sequential, Size = 1)]
internal struct ParticleSystemJobStruct<T> where T : struct, IJobParticleSystem
{
	public delegate void ExecuteJobFunction(ref T data, IntPtr listDataPtr, IntPtr unusedPtr, ref JobRanges ranges, int jobIndex);

	public static IntPtr jobReflectionData;

	public static IntPtr Initialize()
	{
		if (jobReflectionData == IntPtr.Zero)
		{
			jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(T), JobType.Single, new ExecuteJobFunction(Execute));
		}
		return jobReflectionData;
	}

	public unsafe static void Execute(ref T data, IntPtr listDataPtr, IntPtr unusedPtr, ref JobRanges ranges, int jobIndex)
	{
		NativeListData* ptr = (NativeListData*)(void*)listDataPtr;
		ParticleSystem.CopyManagedJobData(ptr->system, out var particleData);
		ParticleSystemJobData jobData = new ParticleSystemJobData(ref particleData);
		data.Execute(jobData);
	}
}
