using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Joker;

public class NetworkSystem : ISystem, IUpdate, IShutdownSync
{
	public Action<long, IMessage> OnMessage;

	public Action<long, MemoryBuffer> OnMessageRaw;

	public Action<long, IPEndPoint> OnAccept;

	public Action<long, int> OnError;

	public Action<long> OnConnect;

	public Action<long> OnDisconnect;

	private AService _network;

	private DoubleMap<IPEndPoint, long> _addressMap = new DoubleMap<IPEndPoint, long>();

	public World World { get; set; }

	public NetworkSystem(IPEndPoint address)
		: this(address, NetworkProtocol.TCP)
	{
	}

	public NetworkSystem(IPEndPoint address, NetworkProtocol type)
	{
		switch (type)
		{
		case NetworkProtocol.TCP:
			_network = new TService(address);
			_InitCallback();
			break;
		default:
			throw new ArgumentException("Invalid Network Protocol");
		}
	}

	public NetworkSystem()
		: this(NetworkProtocol.TCP)
	{
	}

	public NetworkSystem(NetworkProtocol type)
	{
		switch (type)
		{
		case NetworkProtocol.TCP:
			_network = new TService();
			_InitCallback();
			break;
		default:
			throw new ArgumentException("Invalid Network Protocol");
		}
	}

	public void Send(long channel, IMessage message)
	{
		MemoryBuffer memoryBuffer = _network.Fetch();
		MessageService.Instance.Serialize(message, memoryBuffer);
		memoryBuffer.Seek(0L, SeekOrigin.Begin);
		_network.Send(channel, memoryBuffer);
		_network.Recycle(memoryBuffer);
	}

	public void Send(IPEndPoint address, IMessage message)
	{
		long valueByKey = _addressMap.GetValueByKey(address);
		Send(valueByKey, message);
	}

	public void SendRaw(long channel, byte[] data)
	{
		MemoryBuffer memoryBuffer = _network.Fetch();
		memoryBuffer.Write(data);
		memoryBuffer.Seek(0L, SeekOrigin.Begin);
		_network.Send(channel, memoryBuffer);
		_network.Recycle(memoryBuffer);
	}

	public void SendRaw(long channel, MemoryBuffer data)
	{
		_network.Send(channel, data);
	}

	public Task<TResponse> Request<TRequest, TResponse>(long channel, TRequest message) where TRequest : IMessage where TResponse : IMessage
	{
		TaskCompletionSource<TResponse> taskCompletionSource = new TaskCompletionSource<TResponse>();
		taskCompletionSource.SetResult(default(TResponse));
		return taskCompletionSource.Task;
	}

	public long Connect(IPEndPoint address)
	{
		return _network.Create(address);
	}

	public void Disconnect(long channelId)
	{
		_network.Remove(channelId);
	}

	public long GetChannelId(IPEndPoint address)
	{
		return _network.GetChannelId(address);
	}

	public IPEndPoint GetChannelAddress(long channelId)
	{
		return _network.GetChannelAddress(channelId);
	}

	public bool IsChannelValid(long channelId)
	{
		return GetChannelAddress(channelId) != null;
	}

	private void _InitCallback()
	{
		AService network = _network;
		network.AcceptCallback = (Action<long, IPEndPoint>)Delegate.Combine(network.AcceptCallback, new Action<long, IPEndPoint>(_OnAccept));
		AService network2 = _network;
		network2.ReadCallback = (Action<long, MemoryBuffer>)Delegate.Combine(network2.ReadCallback, new Action<long, MemoryBuffer>(_OnRead));
		AService network3 = _network;
		network3.ErrorCallback = (Action<long, int>)Delegate.Combine(network3.ErrorCallback, new Action<long, int>(_OnError));
		AService network4 = _network;
		network4.ConnectCallback = (Action<long>)Delegate.Combine(network4.ConnectCallback, new Action<long>(_OnConnect));
		AService network5 = _network;
		network5.DisconnectCallback = (Action<long>)Delegate.Combine(network5.DisconnectCallback, new Action<long>(_OnDisconnect));
	}

	private void _OnAccept(long channelId, IPEndPoint address)
	{
		_addressMap.Add(address, channelId);
		OnAccept?.Invoke(channelId, address);
	}

	private void _OnRead(long channelId, MemoryBuffer buffer)
	{
		OnMessageRaw?.Invoke(channelId, buffer);
		IMessage arg = MessageService.Instance?.Deserialize(buffer);
		OnMessage?.Invoke(channelId, arg);
	}

	private void _OnError(long channelId, int error)
	{
		OnError?.Invoke(channelId, error);
	}

	private void _OnConnect(long channelId)
	{
		OnConnect?.Invoke(channelId);
	}

	private void _OnDisconnect(long channelId)
	{
		OnDisconnect?.Invoke(channelId);
		_addressMap.RemoveByValue(channelId);
	}

	public virtual void Shutdown()
	{
		_network?.Dispose();
		_network = null;
	}

	public virtual void Update()
	{
		_network?.Update();
	}

	public static string GetErrorDesc(int error)
	{
		if (Enum.IsDefined(typeof(NetworkError), error))
		{
			NetworkError networkError = (NetworkError)error;
			return networkError.ToString();
		}
		if (Enum.IsDefined(typeof(SocketError), error))
		{
			SocketError socketError = (SocketError)error;
			return socketError.ToString();
		}
		return "Unknown Error";
	}

	public static bool IsPortUsed(int port)
	{
		return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners().Any((IPEndPoint endpoint) => endpoint.Port == port);
	}

	public static IPEndPoint[] GetPortsUsed()
	{
		return IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();
	}
}
public class NetworkSystem<T> : NetworkSystem
{
	public NetworkSystem(IPEndPoint address)
		: base(address)
	{
	}

	public NetworkSystem(IPEndPoint address, NetworkProtocol type)
		: base(address, type)
	{
	}

	public NetworkSystem(NetworkProtocol type)
		: base(type)
	{
	}
}
