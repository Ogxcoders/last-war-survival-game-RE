using System;
using Unity.Jobs.LowLevel.Unsafe;

namespace Unity.Collections;

[JobProducerType(typeof(JobNativeMultiHashMapVisitKeyMutableValue.NativeMultiHashMapVisitKeyMutableValueJobStruct<, , >))]
public interface IJobNativeMultiHashMapVisitKeyMutableValue<TKey, TValue> where TKey : struct, IEquatable<TKey> where TValue : struct
{
	void ExecuteNext(TKey key, ref TValue value);
}
