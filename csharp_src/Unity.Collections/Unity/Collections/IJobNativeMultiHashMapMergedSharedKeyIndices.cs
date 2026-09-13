using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections;

[JobProducerType(typeof(JobNativeMultiHashMapUniqueHashExtensions.NativeMultiHashMapUniqueHashJobStruct<>))]
public interface IJobNativeMultiHashMapMergedSharedKeyIndices
{
	void ExecuteFirst(int index);

	void ExecuteNext(int firstIndex, int index);
}
