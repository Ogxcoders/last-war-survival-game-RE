using System.Runtime.ConstrainedExecution;

namespace System.Runtime;

public static class GCSettings
{
	[MonoTODO("Always returns false")]
	public static bool IsServerGC => false;

	[MonoTODO("Always returns GCLatencyMode.Interactive and ignores set")]
	public static GCLatencyMode LatencyMode
	{
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		get
		{
			return GCLatencyMode.Interactive;
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		set
		{
		}
	}

	public static GCLargeObjectHeapCompactionMode LargeObjectHeapCompactionMode
	{
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		get;
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		set;
	}
}
