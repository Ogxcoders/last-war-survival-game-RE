using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Threading;

[ComVisible(true)]
public class Overlapped
{
	private IAsyncResult ares;

	private int offsetL;

	private int offsetH;

	private int evt;

	private IntPtr evt_ptr;

	public IAsyncResult AsyncResult
	{
		get
		{
			return ares;
		}
		set
		{
			ares = value;
		}
	}

	[Obsolete("Not 64bit compatible.  Use EventHandleIntPtr instead.")]
	public int EventHandle
	{
		get
		{
			return evt;
		}
		set
		{
			evt = value;
		}
	}

	[ComVisible(false)]
	public IntPtr EventHandleIntPtr
	{
		get
		{
			return evt_ptr;
		}
		set
		{
			evt_ptr = value;
		}
	}

	public int OffsetHigh
	{
		get
		{
			return offsetH;
		}
		set
		{
			offsetH = value;
		}
	}

	public int OffsetLow
	{
		get
		{
			return offsetL;
		}
		set
		{
			offsetL = value;
		}
	}

	public Overlapped()
	{
	}

	[Obsolete("Not 64bit compatible.  Please use the constructor that takes IntPtr for the event handle")]
	public Overlapped(int offsetLo, int offsetHi, int hEvent, IAsyncResult ar)
	{
		offsetL = offsetLo;
		offsetH = offsetHi;
		evt = hEvent;
		ares = ar;
	}

	public Overlapped(int offsetLo, int offsetHi, IntPtr hEvent, IAsyncResult ar)
	{
		offsetL = offsetLo;
		offsetH = offsetHi;
		evt_ptr = hEvent;
		ares = ar;
	}

	[CLSCompliant(false)]
	public unsafe static void Free(NativeOverlapped* nativeOverlappedPtr)
	{
		if ((IntPtr)nativeOverlappedPtr == IntPtr.Zero)
		{
			throw new ArgumentNullException("nativeOverlappedPtr");
		}
		Marshal.FreeHGlobal((IntPtr)nativeOverlappedPtr);
	}

	[CLSCompliant(false)]
	public unsafe static Overlapped Unpack(NativeOverlapped* nativeOverlappedPtr)
	{
		if ((IntPtr)nativeOverlappedPtr == IntPtr.Zero)
		{
			throw new ArgumentNullException("nativeOverlappedPtr");
		}
		return new Overlapped
		{
			offsetL = nativeOverlappedPtr->OffsetLow,
			offsetH = nativeOverlappedPtr->OffsetHigh,
			evt = (int)nativeOverlappedPtr->EventHandle
		};
	}

	[CLSCompliant(false)]
	[Obsolete("Use Pack(iocb, userData) instead")]
	[MonoTODO("Security - we need to propagate the call stack")]
	public unsafe NativeOverlapped* Pack(IOCompletionCallback iocb)
	{
		NativeOverlapped* ptr = (NativeOverlapped*)(void*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeOverlapped)));
		ptr->OffsetLow = offsetL;
		ptr->OffsetHigh = offsetH;
		ptr->EventHandle = (IntPtr)evt;
		return ptr;
	}

	[CLSCompliant(false)]
	[ComVisible(false)]
	[MonoTODO("handle userData")]
	public unsafe NativeOverlapped* Pack(IOCompletionCallback iocb, object userData)
	{
		NativeOverlapped* ptr = (NativeOverlapped*)(void*)Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NativeOverlapped)));
		ptr->OffsetLow = offsetL;
		ptr->OffsetHigh = offsetH;
		ptr->EventHandle = evt_ptr;
		return ptr;
	}

	[CLSCompliant(false)]
	[Obsolete("Use UnsafePack(iocb, userData) instead")]
	[SecurityPermission(SecurityAction.Demand, ControlEvidence = true, ControlPolicy = true)]
	public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb)
	{
		return Pack(iocb);
	}

	[ComVisible(false)]
	[CLSCompliant(false)]
	public unsafe NativeOverlapped* UnsafePack(IOCompletionCallback iocb, object userData)
	{
		return Pack(iocb, userData);
	}
}
