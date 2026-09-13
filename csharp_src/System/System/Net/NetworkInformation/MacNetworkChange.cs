using System.Runtime.InteropServices;

namespace System.Net.NetworkInformation;

internal sealed class MacNetworkChange : INetworkChange, IDisposable
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	private delegate void SCNetworkReachabilityCallback(IntPtr target, NetworkReachabilityFlags flags, IntPtr info);

	[StructLayout(LayoutKind.Explicit, Size = 28)]
	private struct sockaddr_in
	{
		[FieldOffset(0)]
		public byte sin_len;

		[FieldOffset(1)]
		public byte sin_family;

		public static sockaddr_in Create()
		{
			return new sockaddr_in
			{
				sin_len = 28,
				sin_family = 2
			};
		}
	}

	private struct SCNetworkReachabilityContext
	{
		public IntPtr version;

		public IntPtr info;

		public IntPtr retain;

		public IntPtr release;

		public IntPtr copyDescription;
	}

	[Flags]
	private enum NetworkReachabilityFlags
	{
		None = 0,
		TransientConnection = 1,
		Reachable = 2,
		ConnectionRequired = 4,
		ConnectionOnTraffic = 8,
		InterventionRequired = 0x10,
		ConnectionOnDemand = 0x20,
		IsLocalAddress = 0x10000,
		IsDirect = 0x20000,
		IsWWAN = 0x40000,
		ConnectionAutomatic = 8
	}

	private const string DL_LIB = "/usr/lib/libSystem.dylib";

	private const string CORE_SERVICES_LIB = "/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration";

	private const string CORE_FOUNDATION_LIB = "/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation";

	private IntPtr handle;

	private IntPtr runLoopMode;

	private SCNetworkReachabilityCallback callback;

	private bool scheduledWithRunLoop;

	private NetworkReachabilityFlags flags;

	private bool IsAvailable
	{
		get
		{
			if ((flags & NetworkReachabilityFlags.Reachable) != NetworkReachabilityFlags.None)
			{
				return (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;
			}
			return false;
		}
	}

	public bool HasRegisteredEvents
	{
		get
		{
			if (this.networkAddressChanged == null)
			{
				return this.networkAvailabilityChanged != null;
			}
			return true;
		}
	}

	private event NetworkAddressChangedEventHandler networkAddressChanged;

	private event NetworkAvailabilityChangedEventHandler networkAvailabilityChanged;

	public event NetworkAddressChangedEventHandler NetworkAddressChanged
	{
		add
		{
			value(null, EventArgs.Empty);
			networkAddressChanged += value;
		}
		remove
		{
			networkAddressChanged -= value;
		}
	}

	public event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
	{
		add
		{
			value(null, new NetworkAvailabilityEventArgs(IsAvailable));
			networkAvailabilityChanged += value;
		}
		remove
		{
			networkAvailabilityChanged -= value;
		}
	}

	[DllImport("/usr/lib/libSystem.dylib")]
	private static extern IntPtr dlopen(string path, int mode);

	[DllImport("/usr/lib/libSystem.dylib")]
	private static extern IntPtr dlsym(IntPtr handle, string symbol);

	[DllImport("/usr/lib/libSystem.dylib")]
	private static extern int dlclose(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern void CFRelease(IntPtr handle);

	[DllImport("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation")]
	private static extern IntPtr CFRunLoopGetMain();

	[DllImport("/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration")]
	private static extern IntPtr SCNetworkReachabilityCreateWithAddress(IntPtr allocator, ref sockaddr_in sockaddr);

	[DllImport("/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration")]
	private static extern bool SCNetworkReachabilityGetFlags(IntPtr reachability, out NetworkReachabilityFlags flags);

	[DllImport("/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration")]
	private static extern bool SCNetworkReachabilitySetCallback(IntPtr reachability, SCNetworkReachabilityCallback callback, ref SCNetworkReachabilityContext context);

	[DllImport("/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration")]
	private static extern bool SCNetworkReachabilityScheduleWithRunLoop(IntPtr reachability, IntPtr runLoop, IntPtr runLoopMode);

	[DllImport("/System/Library/Frameworks/SystemConfiguration.framework/SystemConfiguration")]
	private static extern bool SCNetworkReachabilityUnscheduleFromRunLoop(IntPtr reachability, IntPtr runLoop, IntPtr runLoopMode);

	public MacNetworkChange()
	{
		sockaddr_in sockaddr = sockaddr_in.Create();
		handle = SCNetworkReachabilityCreateWithAddress(IntPtr.Zero, ref sockaddr);
		if (handle == IntPtr.Zero)
		{
			throw new Exception("SCNetworkReachabilityCreateWithAddress returned NULL");
		}
		callback = HandleCallback;
		SCNetworkReachabilityContext context = new SCNetworkReachabilityContext
		{
			info = GCHandle.ToIntPtr(GCHandle.Alloc(this))
		};
		SCNetworkReachabilitySetCallback(handle, callback, ref context);
		scheduledWithRunLoop = LoadRunLoopMode() && SCNetworkReachabilityScheduleWithRunLoop(handle, CFRunLoopGetMain(), runLoopMode);
		SCNetworkReachabilityGetFlags(handle, out flags);
	}

	private bool LoadRunLoopMode()
	{
		IntPtr intPtr = dlopen("/System/Library/Frameworks/CoreFoundation.framework/CoreFoundation", 0);
		if (intPtr == IntPtr.Zero)
		{
			return false;
		}
		try
		{
			runLoopMode = dlsym(intPtr, "kCFRunLoopDefaultMode");
			if (runLoopMode != IntPtr.Zero)
			{
				runLoopMode = Marshal.ReadIntPtr(runLoopMode);
				return runLoopMode != IntPtr.Zero;
			}
		}
		finally
		{
			dlclose(intPtr);
		}
		return false;
	}

	public void Dispose()
	{
		lock (this)
		{
			if (!(handle == IntPtr.Zero))
			{
				if (scheduledWithRunLoop)
				{
					SCNetworkReachabilityUnscheduleFromRunLoop(handle, CFRunLoopGetMain(), runLoopMode);
				}
				CFRelease(handle);
				handle = IntPtr.Zero;
				callback = null;
				flags = NetworkReachabilityFlags.None;
				scheduledWithRunLoop = false;
			}
		}
	}

	private static void HandleCallback(IntPtr reachability, NetworkReachabilityFlags flags, IntPtr info)
	{
		if (!(info == IntPtr.Zero) && GCHandle.FromIntPtr(info).Target is MacNetworkChange macNetworkChange && macNetworkChange.flags != flags)
		{
			macNetworkChange.flags = flags;
			macNetworkChange.networkAddressChanged?.Invoke(null, EventArgs.Empty);
			macNetworkChange.networkAvailabilityChanged?.Invoke(null, new NetworkAvailabilityEventArgs(macNetworkChange.IsAvailable));
		}
	}
}
