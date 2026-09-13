using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections;

[JobProducerType(typeof(JobNativeMultiHashMapVisitKeyValue.NativeMultiHashMapVisitKeyValueJobStruct<, , >))]
public interface IJobNativeMultiHashMapVisitKeyValue<TKey, TValue> where TKey : struct, IEquatable<TKey> where TValue : struct
{
	void ExecuteNext(TKey key, TValue value);
}
