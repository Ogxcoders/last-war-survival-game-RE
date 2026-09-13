using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace UnityEngine.ParticleSystemJobs;

public static class IParticleSystemJobExtensions
{
	public unsafe static JobHandle Schedule<T>(this T jobData, ParticleSystem ps, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParticleSystem
	{
		JobsUtility.JobScheduleParameters parameters = CreateScheduleParams(ref jobData, ps, dependsOn, ParticleSystemJobStruct<T>.Initialize());
		JobHandle jobHandle = ParticleSystem.ScheduleManagedJob(ref parameters, ps.GetManagedJobData());
		ps.SetManagedJobHandle(jobHandle);
		return jobHandle;
	}

	public unsafe static JobHandle Schedule<T>(this T jobData, ParticleSystem ps, int minIndicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParticleSystemParallelFor
	{
		JobsUtility.JobScheduleParameters parameters = CreateScheduleParams(ref jobData, ps, dependsOn, ParticleSystemParallelForJobStruct<T>.Initialize());
		JobHandle jobHandle = JobsUtility.ScheduleParallelForDeferArraySize(ref parameters, minIndicesPerJobCount, ps.GetManagedJobData(), null);
		ps.SetManagedJobHandle(jobHandle);
		return jobHandle;
	}

	public unsafe static JobHandle ScheduleBatch<T>(this T jobData, ParticleSystem ps, int innerLoopBatchCount, JobHandle dependsOn = default(JobHandle)) where T : struct, IJobParticleSystemParallelForBatch
	{
		JobsUtility.JobScheduleParameters parameters = CreateScheduleParams(ref jobData, ps, dependsOn, ParticleSystemParallelForBatchJobStruct<T>.Initialize());
		JobHandle jobHandle = JobsUtility.ScheduleParallelForDeferArraySize(ref parameters, innerLoopBatchCount, ps.GetManagedJobData(), null);
		ps.SetManagedJobHandle(jobHandle);
		return jobHandle;
	}

	private unsafe static JobsUtility.JobScheduleParameters CreateScheduleParams<T>(ref T jobData, ParticleSystem ps, JobHandle dependsOn, IntPtr jobReflectionData) where T : struct
	{
		dependsOn = JobHandle.CombineDependencies(ps.GetManagedJobHandle(), dependsOn);
		return new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref jobData), jobReflectionData, dependsOn, ScheduleMode.Batched);
	}
}
