using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security;

namespace System;

public static class GC
{
	private enum StartNoGCRegionStatus
	{
		Succeeded,
		NotEnoughMemory,
		AmountTooLarge,
		AlreadyInProgress
	}

	private enum EndNoGCRegionStatus
	{
		Succeeded,
		NotInProgress,
		GCInduced,
		AllocationExceeded
	}

	internal static readonly object EPHEMERON_TOMBSTONE = get_ephemeron_tombstone();

	public static int MaxGeneration
	{
		[SecuritySafeCritical]
		get
		{
			return GetMaxGeneration();
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetCollectionCount(int generation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern int GetMaxGeneration();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalCollect(int generation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void RecordPressure(long bytesAllocated);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void register_ephemeron_array(Ephemeron[] array);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern object get_ephemeron_tombstone();

	internal static void GetMemoryInfo(out uint highMemLoadThreshold, out ulong totalPhysicalMem, out uint lastRecordedMemLoad, out UIntPtr lastRecordedHeapSize, out UIntPtr lastRecordedFragmentation)
	{
		highMemLoadThreshold = 0u;
		totalPhysicalMem = ulong.MaxValue;
		lastRecordedMemLoad = 0u;
		lastRecordedHeapSize = UIntPtr.Zero;
		lastRecordedFragmentation = UIntPtr.Zero;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern long GetAllocatedBytesForCurrentThread();

	[SecurityCritical]
	public static void AddMemoryPressure(long bytesAllocated)
	{
		if (bytesAllocated <= 0)
		{
			throw new ArgumentOutOfRangeException("bytesAllocated", Environment.GetResourceString("Positive number required."));
		}
		if (4 == IntPtr.Size && bytesAllocated > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("pressure", Environment.GetResourceString("Value must be non-negative and less than or equal to Int32.MaxValue."));
		}
		RecordPressure(bytesAllocated);
	}

	[SecurityCritical]
	public static void RemoveMemoryPressure(long bytesAllocated)
	{
		if (bytesAllocated <= 0)
		{
			throw new ArgumentOutOfRangeException("bytesAllocated", Environment.GetResourceString("Positive number required."));
		}
		if (4 == IntPtr.Size && bytesAllocated > int.MaxValue)
		{
			throw new ArgumentOutOfRangeException("bytesAllocated", Environment.GetResourceString("Value must be non-negative and less than or equal to Int32.MaxValue."));
		}
		RecordPressure(-bytesAllocated);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SecuritySafeCritical]
	public static extern int GetGeneration(object obj);

	public static void Collect(int generation)
	{
		Collect(generation, GCCollectionMode.Default);
	}

	[SecuritySafeCritical]
	public static void Collect()
	{
		InternalCollect(MaxGeneration);
	}

	[SecuritySafeCritical]
	public static void Collect(int generation, GCCollectionMode mode)
	{
		Collect(generation, mode, blocking: true);
	}

	[SecuritySafeCritical]
	public static void Collect(int generation, GCCollectionMode mode, bool blocking)
	{
		Collect(generation, mode, blocking, compacting: false);
	}

	[SecuritySafeCritical]
	public static void Collect(int generation, GCCollectionMode mode, bool blocking, bool compacting)
	{
		if (generation < 0)
		{
			throw new ArgumentOutOfRangeException("generation", Environment.GetResourceString("Value must be positive."));
		}
		if (mode < GCCollectionMode.Default || mode > GCCollectionMode.Optimized)
		{
			throw new ArgumentOutOfRangeException("mode", Environment.GetResourceString("Enum value was out of legal range."));
		}
		int num = 0;
		if (mode == GCCollectionMode.Optimized)
		{
			num |= 4;
		}
		if (compacting)
		{
			num |= 8;
		}
		if (blocking)
		{
			num |= 2;
		}
		else if (!compacting)
		{
			num |= 1;
		}
		InternalCollect(generation);
	}

	[SecuritySafeCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static int CollectionCount(int generation)
	{
		if (generation < 0)
		{
			throw new ArgumentOutOfRangeException("generation", Environment.GetResourceString("Value must be positive."));
		}
		return GetCollectionCount(generation);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static void KeepAlive(object obj)
	{
	}

	[SecuritySafeCritical]
	public static int GetGeneration(WeakReference wo)
	{
		return GetGeneration(wo.Target ?? throw new ArgumentException());
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void WaitForPendingFinalizers();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	private static extern void _SuppressFinalize(object o);

	[SecuritySafeCritical]
	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	public static void SuppressFinalize(object obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		_SuppressFinalize(obj);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void _ReRegisterForFinalize(object o);

	[SecuritySafeCritical]
	public static void ReRegisterForFinalize(object obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException("obj");
		}
		_ReRegisterForFinalize(obj);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern long GetTotalMemory(bool forceFullCollection);

	private static bool _RegisterForFullGCNotification(int maxGenerationPercentage, int largeObjectHeapPercentage)
	{
		throw new NotImplementedException();
	}

	private static bool _CancelFullGCNotification()
	{
		throw new NotImplementedException();
	}

	private static int _WaitForFullGCApproach(int millisecondsTimeout)
	{
		throw new NotImplementedException();
	}

	private static int _WaitForFullGCComplete(int millisecondsTimeout)
	{
		throw new NotImplementedException();
	}

	[SecurityCritical]
	public static void RegisterForFullGCNotification(int maxGenerationThreshold, int largeObjectHeapThreshold)
	{
		if (maxGenerationThreshold <= 0 || maxGenerationThreshold >= 100)
		{
			throw new ArgumentOutOfRangeException("maxGenerationThreshold", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument must be between {0} and {1}."), 1, 99));
		}
		if (largeObjectHeapThreshold <= 0 || largeObjectHeapThreshold >= 100)
		{
			throw new ArgumentOutOfRangeException("largeObjectHeapThreshold", string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument must be between {0} and {1}."), 1, 99));
		}
		if (!_RegisterForFullGCNotification(maxGenerationThreshold, largeObjectHeapThreshold))
		{
			throw new InvalidOperationException(Environment.GetResourceString("This API is not available when the concurrent GC is enabled."));
		}
	}

	[SecurityCritical]
	public static void CancelFullGCNotification()
	{
		if (!_CancelFullGCNotification())
		{
			throw new InvalidOperationException(Environment.GetResourceString("This API is not available when the concurrent GC is enabled."));
		}
	}

	[SecurityCritical]
	public static GCNotificationStatus WaitForFullGCApproach()
	{
		return (GCNotificationStatus)_WaitForFullGCApproach(-1);
	}

	[SecurityCritical]
	public static GCNotificationStatus WaitForFullGCApproach(int millisecondsTimeout)
	{
		if (millisecondsTimeout < -1)
		{
			throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
		}
		return (GCNotificationStatus)_WaitForFullGCApproach(millisecondsTimeout);
	}

	[SecurityCritical]
	public static GCNotificationStatus WaitForFullGCComplete()
	{
		return (GCNotificationStatus)_WaitForFullGCComplete(-1);
	}

	[SecurityCritical]
	public static GCNotificationStatus WaitForFullGCComplete(int millisecondsTimeout)
	{
		if (millisecondsTimeout < -1)
		{
			throw new ArgumentOutOfRangeException("millisecondsTimeout", Environment.GetResourceString("Number must be either non-negative and less than or equal to Int32.MaxValue or -1."));
		}
		return (GCNotificationStatus)_WaitForFullGCComplete(millisecondsTimeout);
	}

	[SecurityCritical]
	private static bool StartNoGCRegionWorker(long totalSize, bool hasLohSize, long lohSize, bool disallowFullBlockingGC)
	{
		throw new NotImplementedException();
	}

	[SecurityCritical]
	public static bool TryStartNoGCRegion(long totalSize)
	{
		return StartNoGCRegionWorker(totalSize, hasLohSize: false, 0L, disallowFullBlockingGC: false);
	}

	[SecurityCritical]
	public static bool TryStartNoGCRegion(long totalSize, long lohSize)
	{
		return StartNoGCRegionWorker(totalSize, hasLohSize: true, lohSize, disallowFullBlockingGC: false);
	}

	[SecurityCritical]
	public static bool TryStartNoGCRegion(long totalSize, bool disallowFullBlockingGC)
	{
		return StartNoGCRegionWorker(totalSize, hasLohSize: false, 0L, disallowFullBlockingGC);
	}

	[SecurityCritical]
	public static bool TryStartNoGCRegion(long totalSize, long lohSize, bool disallowFullBlockingGC)
	{
		return StartNoGCRegionWorker(totalSize, hasLohSize: true, lohSize, disallowFullBlockingGC);
	}

	[SecurityCritical]
	private static EndNoGCRegionStatus EndNoGCRegionWorker()
	{
		throw new NotImplementedException();
	}

	[SecurityCritical]
	public static void EndNoGCRegion()
	{
		EndNoGCRegionWorker();
	}
}
