namespace System.Runtime;

[Serializable]
public enum GCLatencyMode
{
	Batch,
	Interactive,
	LowLatency,
	SustainedLowLatency,
	NoGCRegion
}
