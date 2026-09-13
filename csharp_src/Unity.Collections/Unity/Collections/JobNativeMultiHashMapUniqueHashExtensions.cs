using System;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections;

public static class JobNativeMultiHashMapUniqueHashExtensions
{
	public struct NativeMultiHashMapUniqueHashJobStruct<TJob> where TJob : struct, IJobNativeMultiHashMapMergedSharedKeyIndices
	{
		private delegate void ExecuteJobFunction(ref NativeMultiHashMapUniqueHashJobStruct<TJob> fullData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex);

		[ReadOnly]
		public NativeMultiHashMap<int, int> HashMap;

		public TJob JobData;

		private static IntPtr jobReflectionData;

		public static IntPtr Initialize()
		{
			if (jobReflectionData == IntPtr.Zero)
			{
				jobReflectionData = JobsUtility.CreateJobReflectionData(typeof(NativeMultiHashMapUniqueHashJobStruct<TJob>), typeof(TJob), JobType.ParallelFor, new ExecuteJobFunction(Execute));
			}
			return jobReflectionData;
		}

		public unsafe static void Execute(ref NativeMultiHashMapUniqueHashJobStruct<TJob> fullData, IntPtr additionalPtr, IntPtr bufferRangePatchData, ref JobRanges ranges, int jobIndex)
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
						int key = UnsafeUtility.ReadArrayElement<int>(keys, num);
						int index = UnsafeUtility.ReadArrayElement<int>(values, num);
						fullData.HashMap.TryGetFirstValue(key, out var item, out var it);
						if (num == it.EntryIndex)
						{
							fullData.JobData.ExecuteFirst(index);
						}
						else
						{
							fullData.JobData.ExecuteNext(item, index);
						}
					}
				}
			}
		}
	}

	public unsafe static JobHandle Schedule<TJob>(this TJob jobData, NativeMultiHashMap<int, int> hashMap, int minIndicesPerJobCount, JobHandle dependsOn = default(JobHandle)) where TJob : struct, IJobNativeMultiHashMapMergedSharedKeyIndices
	{
		NativeMultiHashMapUniqueHashJobStruct<TJob> output = new NativeMultiHashMapUniqueHashJobStruct<TJob>
		{
			HashMap = hashMap,
			JobData = jobData
		};
		JobsUtility.JobScheduleParameters parameters = new JobsUtility.JobScheduleParameters(UnsafeUtility.AddressOf(ref output), NativeMultiHashMapUniqueHashJobStruct<TJob>.Initialize(), dependsOn, ScheduleMode.Batched);
		return JobsUtility.ScheduleParallelFor(ref parameters, hashMap.m_Buffer->bucketCapacityMask + 1, minIndicesPerJobCount);
	}
}
