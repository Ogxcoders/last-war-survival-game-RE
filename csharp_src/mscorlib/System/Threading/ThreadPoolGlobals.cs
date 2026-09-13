using System.Security;

namespace System.Threading;

internal static class ThreadPoolGlobals
{
	public const uint tpQuantum = 30u;

	public static int processorCount = Environment.ProcessorCount;

	public static volatile bool vmTpInitialized;

	public static bool enableWorkerTracking;

	[SecurityCritical]
	public static readonly ThreadPoolWorkQueue workQueue = new ThreadPoolWorkQueue();

	public static bool tpHosted => ThreadPool.IsThreadPoolHosted();
}
