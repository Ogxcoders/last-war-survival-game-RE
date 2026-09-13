using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Net.NetworkInformation;

internal sealed class LinuxNetworkChange : INetworkChange, IDisposable
{
	[Flags]
	private enum EventType
	{
		Availability = 1,
		Address = 2
	}

	private object _lock = new object();

	private Socket nl_sock;

	private SocketAsyncEventArgs nl_args;

	private EventType pending_events;

	private Timer timer;

	private NetworkAddressChangedEventHandler AddressChanged;

	private NetworkAvailabilityChangedEventHandler AvailabilityChanged;

	public bool HasRegisteredEvents
	{
		get
		{
			if (AddressChanged == null)
			{
				return AvailabilityChanged != null;
			}
			return true;
		}
	}

	public event NetworkAddressChangedEventHandler NetworkAddressChanged
	{
		add
		{
			Register(value);
		}
		remove
		{
			Unregister(value);
		}
	}

	public event NetworkAvailabilityChangedEventHandler NetworkAvailabilityChanged
	{
		add
		{
			Register(value);
		}
		remove
		{
			Unregister(value);
		}
	}

	public void Dispose()
	{
	}

	private bool EnsureSocket()
	{
		lock (_lock)
		{
			if (nl_sock != null)
			{
				return true;
			}
			IntPtr preexistingHandle = CreateNLSocket();
			if (preexistingHandle.ToInt64() == -1)
			{
				return false;
			}
			SafeSocketHandle safe_handle = new SafeSocketHandle(preexistingHandle, ownsHandle: true);
			nl_sock = new Socket(AddressFamily.Unspecified, SocketType.Raw, ProtocolType.Udp, safe_handle);
			nl_args = new SocketAsyncEventArgs();
			nl_args.SetBuffer(new byte[8192], 0, 8192);
			nl_args.Completed += OnDataAvailable;
			nl_sock.ReceiveAsync(nl_args);
		}
		return true;
	}

	private void MaybeCloseSocket()
	{
		if (nl_sock != null && AvailabilityChanged == null && AddressChanged == null)
		{
			CloseNLSocket(nl_sock.Handle);
			GC.SuppressFinalize(nl_sock);
			nl_sock = null;
			nl_args = null;
		}
	}

	private bool GetAvailability()
	{
		NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
		foreach (NetworkInterface networkInterface in allNetworkInterfaces)
		{
			if (networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback && networkInterface.OperationalStatus == OperationalStatus.Up)
			{
				return true;
			}
		}
		return false;
	}

	private void OnAvailabilityChanged(object unused)
	{
		AvailabilityChanged?.Invoke(null, new NetworkAvailabilityEventArgs(GetAvailability()));
	}

	private void OnAddressChanged(object unused)
	{
		AddressChanged?.Invoke(null, EventArgs.Empty);
	}

	private void OnEventDue(object unused)
	{
		EventType eventType;
		lock (_lock)
		{
			eventType = pending_events;
			pending_events = (EventType)0;
			timer.Change(-1, -1);
		}
		if ((eventType & EventType.Availability) != 0)
		{
			ThreadPool.QueueUserWorkItem(OnAvailabilityChanged);
		}
		if ((eventType & EventType.Address) != 0)
		{
			ThreadPool.QueueUserWorkItem(OnAddressChanged);
		}
	}

	private void QueueEvent(EventType type)
	{
		lock (_lock)
		{
			if (timer == null)
			{
				timer = new Timer(OnEventDue);
			}
			if (pending_events == (EventType)0)
			{
				timer.Change(150, -1);
			}
			pending_events |= type;
		}
	}

	private unsafe void OnDataAvailable(object sender, SocketAsyncEventArgs args)
	{
		if (nl_sock != null)
		{
			EventType eventType;
			fixed (byte* buffer = args.Buffer)
			{
				eventType = ReadEvents(nl_sock.Handle, new IntPtr(buffer), args.BytesTransferred, 8192);
			}
			nl_sock.ReceiveAsync(nl_args);
			if (eventType != 0)
			{
				QueueEvent(eventType);
			}
		}
	}

	private void Register(NetworkAddressChangedEventHandler d)
	{
		EnsureSocket();
		AddressChanged = (NetworkAddressChangedEventHandler)Delegate.Combine(AddressChanged, d);
	}

	private void Register(NetworkAvailabilityChangedEventHandler d)
	{
		EnsureSocket();
		AvailabilityChanged = (NetworkAvailabilityChangedEventHandler)Delegate.Combine(AvailabilityChanged, d);
	}

	private void Unregister(NetworkAddressChangedEventHandler d)
	{
		lock (_lock)
		{
			AddressChanged = (NetworkAddressChangedEventHandler)Delegate.Remove(AddressChanged, d);
			MaybeCloseSocket();
		}
	}

	private void Unregister(NetworkAvailabilityChangedEventHandler d)
	{
		lock (_lock)
		{
			AvailabilityChanged = (NetworkAvailabilityChangedEventHandler)Delegate.Remove(AvailabilityChanged, d);
			MaybeCloseSocket();
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr CreateNLSocket();

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern EventType ReadEvents(IntPtr sock, IntPtr buffer, int count, int size);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern IntPtr CloseNLSocket(IntPtr sock);
}
