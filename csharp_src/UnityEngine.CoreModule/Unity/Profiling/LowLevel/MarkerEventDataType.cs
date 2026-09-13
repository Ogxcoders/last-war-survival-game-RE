namespace Unity.Profiling.LowLevel;

internal enum MarkerEventDataType : byte
{
	None,
	InstanceId,
	Int32,
	UInt32,
	Int64,
	UInt64,
	Float,
	Double,
	String,
	String16,
	Vec3,
	Blob8
}
