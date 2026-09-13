using System.Runtime.InteropServices;

namespace System.Threading;

public sealed class ThreadPoolBoundHandle : IDisposable
{
	public SafeHandle Handle
	{
		get
		{
			throw new PlatformNotSupportedException();
		}
	}

	internal ThreadPoolBoundHandle()
	{
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* AllocateNativeOverlapped(IOCompletionCallback callback, object state, object pinData)
	{
		throw new PlatformNotSupportedException();
	}

	[CLSCompliant(false)]
	public unsafe NativeOverlapped* AllocateNativeOverlapped(PreAllocatedOverlapped preAllocated)
	{
		throw new PlatformNotSupportedException();
	}

	public static ThreadPoolBoundHandle BindHandle(SafeHandle handle)
	{
		throw new PlatformNotSupportedException();
	}

	public void Dispose()
	{
	}

	[CLSCompliant(false)]
	public unsafe void FreeNativeOverlapped(NativeOverlapped* overlapped)
	{
		throw new PlatformNotSupportedException();
	}

	[CLSCompliant(false)]
	public unsafe static object GetNativeOverlappedState(NativeOverlapped* overlapped)
	{
		throw new PlatformNotSupportedException();
	}
}
