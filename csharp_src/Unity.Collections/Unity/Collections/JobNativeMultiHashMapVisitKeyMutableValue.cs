using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections;

public static class JobNativeMultiHashMapVisitKeyMutableValue
{
	internal struct NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue> where TJob : struct, IJobNativeMultiHashMapVisitKeyMutableValue<TKey, TValue> where TKey : struct, IEquatable<TKey> where TValue : struct
	{
		public delegate void ExecuteJobFunction(ref NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue> fullData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

		[NativeDisableContainerSafetyRestriction]
		public NativeMultiHashMap<TKey, TValue> HashMap;

		public TJob JobData;

		private static IntPtr jobReflectionData;

		public static IntPtr Initialize()
		{
			if (jobReflectionData == IntPtr.Zero)
			{
				jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue>), typeof(TJob), JobType.ParallelFor, new ExecuteJobFunction(Execute));
			}
			return jobReflectionData;
		}

		public unsafe static void Execute(ref NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue> fullData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
		{
			int beginIndex;
			int endIndex;
			while (JobsUtility.GetWorkStealingRange(ref ranges, jobIndex, out beginIndex, out endIndex))
			{
				int* buckets = (int*)fullData.HashMap.m_Buffer->buckets;
				int* next = (int*)fullData.HashMap.m_Buffer->next;
				byte* keys = fullData.HashMap.m_Buffer->keys;
				byte* values = fullData.HashMap.m_Buffer->values;
				for (int i = beginIndex; i < endIndex; i++)
				{
					for (int num = buckets[i]; num != -1; num = next[num])
					{
						TKey key = UnsafeUtility.ReadArrayElement<TKey>(keys, num);
						fullData.JobData.ExecuteNext(key, ref UnsafeUtilityEx.ArrayElementAsRef<TValue>(values, num));
					}
				}
			}
		}
	}

	public unsafe static JobHandle Schedule<TJob, TKey, TValue>(this TJob jobData, NativeMultiHashMap<TKey, TValue> hashMap, int minIndicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where TJob : struct, IJobNativeMultiHashMapVisitKeyMutableValue<TKey, TValue> where TKey : struct, IEquatable<TKey> where TValue : struct
	{
		NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue> output = new NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue>
		{
			HashMap = hashMap,
			JobData = jobData
		};
		JobsUtility.JobScheduleParameters parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref output), NativeMultiHashMapVisitKeyMutableValueJobStruct<TJob, TKey, TValue>.Initialize(), dependsOn, ScheduleMode.Batched);
		return JobsUtility.ScheduleParallelFor(ref parameters, hashMap.m_Buffer->bucketCapacityMask + 1, minIndicesPerJobCount);
	}
}
