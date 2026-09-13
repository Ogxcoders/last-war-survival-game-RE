namespace System;

[Serializable]
internal enum InternalGCCollectionMode
{
	NonBlocking = 1,
	Blocking = 2,
	Optimized = 4,
	Compacting = 8
}
