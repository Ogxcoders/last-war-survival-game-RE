using System.Security.Permissions;
using System.Threading.Tasks;

namespace System.Net.Sockets;

public class UdpClient : IDisposable
{
	private const int MaxUDPSize = 65536;

	private Socket m_ClientSocket;

	private bool m_Active;

	private byte[] m_Buffer = new byte[65536];

	private AddressFamily m_Family = AddressFamily.InterNetwork;

	private bool m_CleanedUp;

	private bool m_IsBroadcast;

	public Socket Client
	{
		get
		{
			return m_ClientSocket;
		}
		set
		{
			m_ClientSocket = value;
		}
	}

	protected bool Active
	{
		get
		{
			return m_Active;
		}
		set
		{
			m_Active = value;
		}
	}

	public int Available => m_ClientSocket.Available;

	public short Ttl
	{
		get
		{
			return m_ClientSocket.Ttl;
		}
		set
		{
			m_ClientSocket.Ttl = value;
		}
	}

	public bool DontFragment
	{
		get
		{
			return m_ClientSocket.DontFragment;
		}
		set
		{
			m_ClientSocket.DontFragment = value;
		}
	}

	public bool MulticastLoopback
	{
		get
		{
			return m_ClientSocket.MulticastLoopback;
		}
		set
		{
			m_ClientSocket.MulticastLoopback = value;
		}
	}

	public bool EnableBroadcast
	{
		get
		{
			return m_ClientSocket.EnableBroadcast;
		}
		set
		{
			m_ClientSocket.EnableBroadcast = value;
		}
	}

	public bool ExclusiveAddressUse
	{
		get
		{
			return m_ClientSocket.ExclusiveAddressUse;
		}
		set
		{
			m_ClientSocket.ExclusiveAddressUse = value;
		}
	}

	public UdpClient()
		: this(AddressFamily.InterNetwork)
	{
	}

	public UdpClient(AddressFamily family)
	{
		if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
		{
			throw new ArgumentException(global::SR.GetString("'{0}' Client can only accept InterNetwork or InterNetworkV6 addresses.", "UDP"), "family");
		}
		m_Family = family;
		createClientSocket();
	}

	public UdpClient(int port)
		: this(port, AddressFamily.InterNetwork)
	{
	}

	public UdpClient(int port, AddressFamily family)
	{
		if (!ValidationHelper.ValidateTcpPort(port))
		{
			throw new ArgumentOutOfRangeException("port");
		}
		if (family != AddressFamily.InterNetwork && family != AddressFamily.InterNetworkV6)
		{
			throw new ArgumentException(global::SR.GetString("'{0}' Client can only accept InterNetwork or InterNetworkV6 addresses."), "family");
		}
		m_Family = family;
		IPEndPoint localEP = ((m_Family != AddressFamily.InterNetwork) ? new IPEndPoint(IPAddress.IPv6Any, port) : new IPEndPoint(IPAddress.Any, port));
		createClientSocket();
		Client.Bind(localEP);
	}

	public UdpClient(IPEndPoint localEP)
	{
		if (localEP == null)
		{
			throw new ArgumentNullException("localEP");
		}
		m_Family = localEP.AddressFamily;
		createClientSocket();
		Client.Bind(localEP);
	}

	public UdpClient(string hostname, int port)
	{
		if (hostname == null)
		{
			throw new ArgumentNullException("hostname");
		}
		if (!ValidationHelper.ValidateTcpPort(port))
		{
			throw new ArgumentOutOfRangeException("port");
		}
		Connect(hostname, port);
	}

	public void AllowNatTraversal(bool allowed)
	{
		if (allowed)
		{
			m_ClientSocket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);
		}
		else
		{
			m_ClientSocket.SetIPProtectionLevel(IPProtectionLevel.EdgeRestricted);
		}
	}

	public void Close()
	{
		Dispose(disposing: true);
	}

	private void FreeResources()
	{
		if (!m_CleanedUp)
		{
			Socket client = Client;
			if (client != null)
			{
				client.InternalShutdown(SocketShutdown.Both);
				client.Close();
				Client = null;
			}
			m_CleanedUp = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			FreeResources();
			GC.SuppressFinalize(this);
		}
	}

	public void Connect(string hostname, int port)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (hostname == null)
		{
			throw new ArgumentNullException("hostname");
		}
		if (!ValidationHelper.ValidateTcpPort(port))
		{
			throw new ArgumentOutOfRangeException("port");
		}
		IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
		Exception ex = null;
		Socket socket = null;
		Socket socket2 = null;
		try
		{
			if (m_ClientSocket == null)
			{
				if (Socket.OSSupportsIPv4)
				{
					socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
				}
				if (Socket.OSSupportsIPv6)
				{
					socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
				}
			}
			IPAddress[] array = hostAddresses;
			foreach (IPAddress iPAddress in array)
			{
				try
				{
					if (m_ClientSocket == null)
					{
						if (iPAddress.AddressFamily == AddressFamily.InterNetwork && socket2 != null)
						{
							socket2.Connect(iPAddress, port);
							m_ClientSocket = socket2;
							socket?.Close();
						}
						else if (socket != null)
						{
							socket.Connect(iPAddress, port);
							m_ClientSocket = socket;
							socket2?.Close();
						}
						m_Family = iPAddress.AddressFamily;
						m_Active = true;
						break;
					}
					if (iPAddress.AddressFamily == m_Family)
					{
						Connect(new IPEndPoint(iPAddress, port));
						m_Active = true;
						break;
					}
				}
				catch (Exception ex2)
				{
					if (NclUtilities.IsFatal(ex2))
					{
						throw;
					}
					ex = ex2;
				}
			}
		}
		catch (Exception ex3)
		{
			if (NclUtilities.IsFatal(ex3))
			{
				throw;
			}
			ex = ex3;
		}
		finally
		{
			if (!m_Active)
			{
				socket?.Close();
				socket2?.Close();
				if (ex != null)
				{
					throw ex;
				}
				throw new SocketException(SocketError.NotConnected);
			}
		}
	}

	public void Connect(IPAddress addr, int port)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (addr == null)
		{
			throw new ArgumentNullException("addr");
		}
		if (!ValidationHelper.ValidateTcpPort(port))
		{
			throw new ArgumentOutOfRangeException("port");
		}
		IPEndPoint endPoint = new IPEndPoint(addr, port);
		Connect(endPoint);
	}

	public void Connect(IPEndPoint endPoint)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (endPoint == null)
		{
			throw new ArgumentNullException("endPoint");
		}
		CheckForBroadcast(endPoint.Address);
		Client.Connect(endPoint);
		m_Active = true;
	}

	private void CheckForBroadcast(IPAddress ipAddress)
	{
		if (Client != null && !m_IsBroadcast && IsBroadcast(ipAddress))
		{
			m_IsBroadcast = true;
			Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
		}
	}

	private static bool IsBroadcast(IPAddress address)
	{
		if (address.AddressFamily == AddressFamily.InterNetworkV6)
		{
			return false;
		}
		return address.Equals(IPAddress.Broadcast);
	}

	public int Send(byte[] dgram, int bytes, IPEndPoint endPoint)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (dgram == null)
		{
			throw new ArgumentNullException("dgram");
		}
		if (m_Active && endPoint != null)
		{
			throw new InvalidOperationException(global::SR.GetString("Cannot send packets to an arbitrary host while connected."));
		}
		if (endPoint == null)
		{
			return Client.Send(dgram, 0, bytes, SocketFlags.None);
		}
		CheckForBroadcast(endPoint.Address);
		return Client.SendTo(dgram, 0, bytes, SocketFlags.None, endPoint);
	}

	public int Send(byte[] dgram, int bytes, string hostname, int port)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (dgram == null)
		{
			throw new ArgumentNullException("dgram");
		}
		if (m_Active && (hostname != null || port != 0))
		{
			throw new InvalidOperationException(global::SR.GetString("Cannot send packets to an arbitrary host while connected."));
		}
		if (hostname == null || port == 0)
		{
			return Client.Send(dgram, 0, bytes, SocketFlags.None);
		}
		IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
		int i;
		for (i = 0; i < hostAddresses.Length && hostAddresses[i].AddressFamily != m_Family; i++)
		{
		}
		if (hostAddresses.Length == 0 || i == hostAddresses.Length)
		{
			throw new ArgumentException(global::SR.GetString("None of the discovered or specified addresses match the socket address family."), "hostname");
		}
		CheckForBroadcast(hostAddresses[i]);
		IPEndPoint remoteEP = new IPEndPoint(hostAddresses[i], port);
		return Client.SendTo(dgram, 0, bytes, SocketFlags.None, remoteEP);
	}

	public int Send(byte[] dgram, int bytes)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (dgram == null)
		{
			throw new ArgumentNullException("dgram");
		}
		if (!m_Active)
		{
			throw new InvalidOperationException(global::SR.GetString("The operation is not allowed on non-connected sockets."));
		}
		return Client.Send(dgram, 0, bytes, SocketFlags.None);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginSend(byte[] datagram, int bytes, IPEndPoint endPoint, AsyncCallback requestCallback, object state)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (datagram == null)
		{
			throw new ArgumentNullException("datagram");
		}
		if (bytes > datagram.Length || bytes < 0)
		{
			throw new ArgumentOutOfRangeException("bytes");
		}
		if (m_Active && endPoint != null)
		{
			throw new InvalidOperationException(global::SR.GetString("Cannot send packets to an arbitrary host while connected."));
		}
		if (endPoint == null)
		{
			return Client.BeginSend(datagram, 0, bytes, SocketFlags.None, requestCallback, state);
		}
		CheckForBroadcast(endPoint.Address);
		return Client.BeginSendTo(datagram, 0, bytes, SocketFlags.None, endPoint, requestCallback, state);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginSend(byte[] datagram, int bytes, string hostname, int port, AsyncCallback requestCallback, object state)
	{
		if (m_Active && (hostname != null || port != 0))
		{
			throw new InvalidOperationException(global::SR.GetString("Cannot send packets to an arbitrary host while connected."));
		}
		IPEndPoint endPoint = null;
		if (hostname != null && port != 0)
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(hostname);
			int i;
			for (i = 0; i < hostAddresses.Length && hostAddresses[i].AddressFamily != m_Family; i++)
			{
			}
			if (hostAddresses.Length == 0 || i == hostAddresses.Length)
			{
				throw new ArgumentException(global::SR.GetString("None of the discovered or specified addresses match the socket address family."), "hostname");
			}
			CheckForBroadcast(hostAddresses[i]);
			endPoint = new IPEndPoint(hostAddresses[i], port);
		}
		return BeginSend(datagram, bytes, endPoint, requestCallback, state);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginSend(byte[] datagram, int bytes, AsyncCallback requestCallback, object state)
	{
		return BeginSend(datagram, bytes, null, requestCallback, state);
	}

	public int EndSend(IAsyncResult asyncResult)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (m_Active)
		{
			return Client.EndSend(asyncResult);
		}
		return Client.EndSendTo(asyncResult);
	}

	public byte[] Receive(ref IPEndPoint remoteEP)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		EndPoint remoteEP2 = ((m_Family != AddressFamily.InterNetwork) ? IPEndPoint.IPv6Any : IPEndPoint.Any);
		int num = Client.ReceiveFrom(m_Buffer, 65536, SocketFlags.None, ref remoteEP2);
		remoteEP = (IPEndPoint)remoteEP2;
		if (num < 65536)
		{
			byte[] array = new byte[num];
			Buffer.BlockCopy(m_Buffer, 0, array, 0, num);
			return array;
		}
		return m_Buffer;
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public IAsyncResult BeginReceive(AsyncCallback requestCallback, object state)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		EndPoint remoteEP = ((m_Family != AddressFamily.InterNetwork) ? IPEndPoint.IPv6Any : IPEndPoint.Any);
		return Client.BeginReceiveFrom(m_Buffer, 0, 65536, SocketFlags.None, ref remoteEP, requestCallback, state);
	}

	public byte[] EndReceive(IAsyncResult asyncResult, ref IPEndPoint remoteEP)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		EndPoint endPoint = ((m_Family != AddressFamily.InterNetwork) ? IPEndPoint.IPv6Any : IPEndPoint.Any);
		int num = Client.EndReceiveFrom(asyncResult, ref endPoint);
		remoteEP = (IPEndPoint)endPoint;
		if (num < 65536)
		{
			byte[] array = new byte[num];
			Buffer.BlockCopy(m_Buffer, 0, array, 0, num);
			return array;
		}
		return m_Buffer;
	}

	public void JoinMulticastGroup(IPAddress multicastAddr)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (multicastAddr == null)
		{
			throw new ArgumentNullException("multicastAddr");
		}
		if (multicastAddr.AddressFamily != m_Family)
		{
			throw new ArgumentException(global::SR.GetString("Multicast family is not the same as the family of the '{0}' Client.", "UDP"), "multicastAddr");
		}
		if (m_Family == AddressFamily.InterNetwork)
		{
			MulticastOption optionValue = new MulticastOption(multicastAddr);
			Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
		}
		else
		{
			IPv6MulticastOption optionValue2 = new IPv6MulticastOption(multicastAddr);
			Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, optionValue2);
		}
	}

	public void JoinMulticastGroup(IPAddress multicastAddr, IPAddress localAddress)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (m_Family != AddressFamily.InterNetwork)
		{
			throw new SocketException(SocketError.OperationNotSupported);
		}
		MulticastOption optionValue = new MulticastOption(multicastAddr, localAddress);
		Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, optionValue);
	}

	public void JoinMulticastGroup(int ifindex, IPAddress multicastAddr)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (multicastAddr == null)
		{
			throw new ArgumentNullException("multicastAddr");
		}
		if (ifindex < 0)
		{
			throw new ArgumentException(global::SR.GetString("The specified value cannot be negative."), "ifindex");
		}
		if (m_Family != AddressFamily.InterNetworkV6)
		{
			throw new SocketException(SocketError.OperationNotSupported);
		}
		IPv6MulticastOption optionValue = new IPv6MulticastOption(multicastAddr, ifindex);
		Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.AddMembership, optionValue);
	}

	public void JoinMulticastGroup(IPAddress multicastAddr, int timeToLive)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (multicastAddr == null)
		{
			throw new ArgumentNullException("multicastAddr");
		}
		if (!ValidationHelper.ValidateRange(timeToLive, 0, 255))
		{
			throw new ArgumentOutOfRangeException("timeToLive");
		}
		JoinMulticastGroup(multicastAddr);
		Client.SetSocketOption((m_Family != AddressFamily.InterNetwork) ? SocketOptionLevel.IPv6 : SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, timeToLive);
	}

	public void DropMulticastGroup(IPAddress multicastAddr)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (multicastAddr == null)
		{
			throw new ArgumentNullException("multicastAddr");
		}
		if (multicastAddr.AddressFamily != m_Family)
		{
			throw new ArgumentException(global::SR.GetString("Multicast family is not the same as the family of the '{0}' Client.", "UDP"), "multicastAddr");
		}
		if (m_Family == AddressFamily.InterNetwork)
		{
			MulticastOption optionValue = new MulticastOption(multicastAddr);
			Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, optionValue);
		}
		else
		{
			IPv6MulticastOption optionValue2 = new IPv6MulticastOption(multicastAddr);
			Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, optionValue2);
		}
	}

	public void DropMulticastGroup(IPAddress multicastAddr, int ifindex)
	{
		if (m_CleanedUp)
		{
			throw new ObjectDisposedException(GetType().FullName);
		}
		if (multicastAddr == null)
		{
			throw new ArgumentNullException("multicastAddr");
		}
		if (ifindex < 0)
		{
			throw new ArgumentException(global::SR.GetString("The specified value cannot be negative."), "ifindex");
		}
		if (m_Family != AddressFamily.InterNetworkV6)
		{
			throw new SocketException(SocketError.OperationNotSupported);
		}
		IPv6MulticastOption optionValue = new IPv6MulticastOption(multicastAddr, ifindex);
		Client.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.DropMembership, optionValue);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public Task<int> SendAsync(byte[] datagram, int bytes)
	{
		return Task<int>.Factory.FromAsync(BeginSend, EndSend, datagram, bytes, null);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public Task<int> SendAsync(byte[] datagram, int bytes, IPEndPoint endPoint)
	{
		return Task<int>.Factory.FromAsync(BeginSend, EndSend, datagram, bytes, endPoint, null);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public Task<int> SendAsync(byte[] datagram, int bytes, string hostname, int port)
	{
		return Task<int>.Factory.FromAsync((AsyncCallback callback, object state) => BeginSend(datagram, bytes, hostname, port, callback, state), EndSend, null);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public Task<UdpReceiveResult> ReceiveAsync()
	{
		return Task<UdpReceiveResult>.Factory.FromAsync((AsyncCallback callback, object state) => BeginReceive(callback, state), delegate(IAsyncResult ar)
		{
			IPEndPoint remoteEP = null;
			return new UdpReceiveResult(EndReceive(ar, ref remoteEP), remoteEP);
		}, null);
	}

	private void createClientSocket()
	{
		Client = new Socket(m_Family, SocketType.Dgram, ProtocolType.Udp);
	}
}
