using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Joker;

public sealed class TService : AService
{
	private readonly Dictionary<long, TChannel> idChannels = new Dictionary<long, TChannel>();

	private readonly SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();

	private Socket acceptor;

	public ConcurrentQueue<TArgs> Queue = new ConcurrentQueue<TArgs>();

	public TService()
	{
	}

	public TService(IPEndPoint ipEndPoint)
	{
		acceptor = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		innArgs.Completed += OnComplete;
		try
		{
			acceptor.Bind(ipEndPoint);
		}
		catch (Exception innerException)
		{
			throw new Exception($"bind error: {ipEndPoint}", innerException);
		}
		acceptor.Listen(1000);
		AcceptAsync();
	}

	private void OnComplete(object sender, SocketAsyncEventArgs e)
	{
		SocketAsyncOperation lastOperation = e.LastOperation;
		if (lastOperation == SocketAsyncOperation.Accept)
		{
			Queue.Enqueue(new TArgs
			{
				SocketAsyncEventArgs = e
			});
			return;
		}
		throw new Exception($"socket error: {e.LastOperation}");
	}

	private void OnAcceptComplete(SocketError socketError, Socket acceptSocket)
	{
		if (acceptor == null)
		{
			return;
		}
		if (socketError != SocketError.Success)
		{
			Log.Error($"accept error {socketError}");
			AcceptAsync();
			return;
		}
		try
		{
			TChannel tChannel = new TChannel(AService.CreateChannelId(), acceptSocket, this);
			idChannels.Add(tChannel.Id, tChannel);
			long id = tChannel.Id;
			AcceptCallback(id, tChannel.RemoteAddress);
		}
		catch (Exception e)
		{
			Log.Exception(e);
		}
		AcceptAsync();
	}

	private void AcceptAsync()
	{
		innArgs.AcceptSocket = null;
		if (!acceptor.AcceptAsync(innArgs))
		{
			OnAcceptComplete(innArgs.SocketError, innArgs.AcceptSocket);
		}
	}

	public override void Create(long id, IPEndPoint ipEndPoint)
	{
		if (!idChannels.TryGetValue(id, out var _))
		{
			TChannel tChannel = new TChannel(id, ipEndPoint, this);
			idChannels.Add(tChannel.Id, tChannel);
		}
	}

	public override long Create(IPEndPoint ipEndPoint)
	{
		long num = AService.CreateChannelId();
		Create(num, ipEndPoint);
		return num;
	}

	public TChannel Get(long id)
	{
		TChannel value = null;
		idChannels.TryGetValue(id, out value);
		return value;
	}

	public override long GetChannelId(IPEndPoint address)
	{
		foreach (KeyValuePair<long, TChannel> idChannel in idChannels)
		{
			if (idChannel.Value.RemoteAddress.Equals(address))
			{
				return idChannel.Key;
			}
		}
		return -1L;
	}

	public override IPEndPoint GetChannelAddress(long id)
	{
		return Get(id)?.RemoteAddress;
	}

	public override void Dispose()
	{
		base.Dispose();
		acceptor?.Close();
		acceptor = null;
		innArgs.Dispose();
		long[] array = idChannels.Keys.ToArray();
		foreach (long key in array)
		{
			idChannels[key].Dispose();
		}
		idChannels.Clear();
	}

	public override void Remove(long id, int error = 0)
	{
		if (idChannels.TryGetValue(id, out var value))
		{
			value.Error = error;
			value.Dispose();
		}
		idChannels.Remove(id);
	}

	public override void Send(long channelId, MemoryBuffer memoryBuffer)
	{
		try
		{
			TChannel tChannel = Get(channelId);
			if (tChannel == null)
			{
				ErrorCallback(channelId, 100213);
			}
			else
			{
				tChannel.Send(memoryBuffer);
			}
		}
		catch (Exception e)
		{
			Log.Exception(e);
		}
	}

	public override void Update()
	{
		TArgs result;
		while (Queue.TryDequeue(out result))
		{
			SocketAsyncEventArgs socketAsyncEventArgs = result.SocketAsyncEventArgs;
			if (socketAsyncEventArgs == null)
			{
				switch (result.Op)
				{
				case TcpOp.StartSend:
					Get(result.ChannelId)?.StartSend();
					break;
				case TcpOp.StartRecv:
					Get(result.ChannelId)?.StartRecv();
					break;
				case TcpOp.Connect:
					Get(result.ChannelId)?.ConnectAsync();
					break;
				}
				continue;
			}
			switch (socketAsyncEventArgs.LastOperation)
			{
			case SocketAsyncOperation.Accept:
			{
				SocketError socketError = socketAsyncEventArgs.SocketError;
				Socket acceptSocket = socketAsyncEventArgs.AcceptSocket;
				OnAcceptComplete(socketError, acceptSocket);
				break;
			}
			case SocketAsyncOperation.Connect:
				Get(result.ChannelId)?.OnConnectComplete(socketAsyncEventArgs);
				break;
			case SocketAsyncOperation.Disconnect:
				Get(result.ChannelId)?.OnDisconnectComplete(socketAsyncEventArgs);
				DisconnectCallback?.Invoke(result.ChannelId);
				break;
			case SocketAsyncOperation.Receive:
				Get(result.ChannelId)?.OnRecvComplete(socketAsyncEventArgs);
				break;
			case SocketAsyncOperation.Send:
				Get(result.ChannelId)?.OnSendComplete(socketAsyncEventArgs);
				break;
			default:
				throw new ArgumentOutOfRangeException($"{socketAsyncEventArgs.LastOperation}");
			}
		}
	}

	public override bool IsDisposed()
	{
		return acceptor == null;
	}
}
