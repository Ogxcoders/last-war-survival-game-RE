using System;
using System.Net;
using System.Net.Sockets;

namespace Joker;

public sealed class TChannel : NetworkChannel
{
	private readonly TService Service;

	private Socket socket;

	private SocketAsyncEventArgs innArgs = new SocketAsyncEventArgs();

	private SocketAsyncEventArgs outArgs = new SocketAsyncEventArgs();

	private readonly CircularBuffer recvBuffer = new CircularBuffer();

	private readonly CircularBuffer sendBuffer = new CircularBuffer();

	private bool isSending;

	private bool isConnected;

	private readonly PacketParser parser;

	private readonly byte[] sendCache = new byte[18];

	private void OnComplete(object sender, SocketAsyncEventArgs e)
	{
		Service.Queue.Enqueue(new TArgs
		{
			ChannelId = Id,
			SocketAsyncEventArgs = e
		});
	}

	public TChannel(long id, IPEndPoint ipEndPoint, TService service)
	{
		Service = service;
		base.ChannelType = ChannelType.Connect;
		Id = id;
		socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		socket.NoDelay = true;
		parser = new PacketParser(recvBuffer, Service);
		innArgs.Completed += OnComplete;
		outArgs.Completed += OnComplete;
		base.RemoteAddress = ipEndPoint;
		isConnected = false;
		isSending = false;
		Service.Queue.Enqueue(new TArgs
		{
			Op = TcpOp.Connect,
			ChannelId = Id
		});
	}

	public TChannel(long id, Socket socket, TService service)
	{
		Service = service;
		base.ChannelType = ChannelType.Accept;
		Id = id;
		this.socket = socket;
		this.socket.NoDelay = true;
		parser = new PacketParser(recvBuffer, Service);
		innArgs.Completed += OnComplete;
		outArgs.Completed += OnComplete;
		base.RemoteAddress = (IPEndPoint)socket.RemoteEndPoint;
		isConnected = true;
		isSending = false;
		Service.Queue.Enqueue(new TArgs
		{
			Op = TcpOp.StartSend,
			ChannelId = Id
		});
		Service.Queue.Enqueue(new TArgs
		{
			Op = TcpOp.StartRecv,
			ChannelId = Id
		});
	}

	public override void Dispose()
	{
		if (!base.IsDisposed)
		{
			Log.Info($"channel dispose: {Id} {base.RemoteAddress} {base.Error}");
			long id = Id;
			Id = 0L;
			Service.Remove(id);
			socket.Close();
			innArgs.Dispose();
			outArgs.Dispose();
			innArgs = null;
			outArgs = null;
			socket = null;
		}
	}

	public void Send(MemoryBuffer stream)
	{
		if (base.IsDisposed)
		{
			throw new Exception("TChannel已经被Dispose, 不能发送消息");
		}
		int num = (int)(stream.Length - stream.Position);
		if (num > 1048560)
		{
			throw new Exception($"send packet too large: {stream.Length} {stream.Position}");
		}
		sendCache.WriteTo(0, num);
		sendBuffer.Write(sendCache, 0, 4);
		sendBuffer.Write(stream.GetBuffer(), (int)stream.Position, (int)(stream.Length - stream.Position));
		if (!isSending)
		{
			Service.Queue.Enqueue(new TArgs
			{
				Op = TcpOp.StartSend,
				ChannelId = Id
			});
		}
	}

	public void ConnectAsync()
	{
		outArgs.RemoteEndPoint = base.RemoteAddress;
		if (!socket.ConnectAsync(outArgs))
		{
			OnConnectComplete(outArgs);
		}
	}

	public void OnConnectComplete(SocketAsyncEventArgs e)
	{
		if (socket != null)
		{
			if (e.SocketError != SocketError.Success)
			{
				OnError((int)e.SocketError);
				return;
			}
			e.RemoteEndPoint = null;
			isConnected = true;
			Service.Queue.Enqueue(new TArgs
			{
				Op = TcpOp.StartSend,
				ChannelId = Id
			});
			Service.Queue.Enqueue(new TArgs
			{
				Op = TcpOp.StartRecv,
				ChannelId = Id
			});
			Service.ConnectCallback?.Invoke(Id);
		}
	}

	public void OnDisconnectComplete(SocketAsyncEventArgs e)
	{
		OnError((int)e.SocketError);
	}

	public void StartRecv()
	{
		while (true)
		{
			try
			{
				if (socket == null)
				{
					break;
				}
				int count = recvBuffer.ChunkSize - recvBuffer.LastIndex;
				innArgs.SetBuffer(recvBuffer.Last, recvBuffer.LastIndex, count);
			}
			catch (Exception arg)
			{
				Log.Error($"tchannel error: {Id}\n{arg}");
				OnError(100214);
				break;
			}
			if (socket.ReceiveAsync(innArgs))
			{
				break;
			}
			HandleRecv(innArgs);
		}
	}

	public void OnRecvComplete(SocketAsyncEventArgs o)
	{
		HandleRecv(o);
		if (socket != null)
		{
			Service.Queue.Enqueue(new TArgs
			{
				Op = TcpOp.StartRecv,
				ChannelId = Id
			});
		}
	}

	private void HandleRecv(SocketAsyncEventArgs e)
	{
		if (socket == null)
		{
			return;
		}
		if (e.SocketError != SocketError.Success)
		{
			OnError((int)e.SocketError);
			return;
		}
		if (e.BytesTransferred == 0)
		{
			OnError(100208);
			return;
		}
		recvBuffer.LastIndex += e.BytesTransferred;
		if (recvBuffer.LastIndex == recvBuffer.ChunkSize)
		{
			recvBuffer.AddLast();
			recvBuffer.LastIndex = 0;
		}
		while (socket != null)
		{
			try
			{
				if (recvBuffer.Length == 0L || !parser.Parse(out var memoryBuffer))
				{
					break;
				}
				OnRead(memoryBuffer);
				Service.Recycle(memoryBuffer);
			}
			catch (Exception arg)
			{
				Log.Error($"ip: {base.RemoteAddress} {arg}");
				OnError(100210);
				break;
			}
		}
	}

	public void StartSend()
	{
		if (!isConnected || isSending)
		{
			return;
		}
		while (true)
		{
			try
			{
				if (socket == null)
				{
					isSending = false;
					break;
				}
				if (sendBuffer.Length == 0L)
				{
					isSending = false;
					break;
				}
				isSending = true;
				int num = sendBuffer.ChunkSize - sendBuffer.FirstIndex;
				if (num > sendBuffer.Length)
				{
					num = (int)sendBuffer.Length;
				}
				outArgs.SetBuffer(sendBuffer.First, sendBuffer.FirstIndex, num);
				if (socket.SendAsync(outArgs))
				{
					break;
				}
				HandleSend(outArgs);
			}
			catch (Exception innerException)
			{
				throw new Exception($"socket set buffer error: {sendBuffer.First.Length}, {sendBuffer.FirstIndex}", innerException);
			}
		}
	}

	public void OnSendComplete(SocketAsyncEventArgs o)
	{
		HandleSend(o);
		isSending = false;
		Service.Queue.Enqueue(new TArgs
		{
			Op = TcpOp.StartSend,
			ChannelId = Id
		});
	}

	private void HandleSend(SocketAsyncEventArgs e)
	{
		if (socket == null)
		{
			return;
		}
		if (e.SocketError != SocketError.Success)
		{
			OnError((int)e.SocketError);
			return;
		}
		if (e.BytesTransferred == 0)
		{
			OnError(100208);
			return;
		}
		sendBuffer.FirstIndex += e.BytesTransferred;
		if (sendBuffer.FirstIndex == sendBuffer.ChunkSize)
		{
			sendBuffer.FirstIndex = 0;
			sendBuffer.RemoveFirst();
		}
	}

	private void OnRead(MemoryBuffer memoryStream)
	{
		try
		{
			Service.ReadCallback(Id, memoryStream);
		}
		catch (Exception e)
		{
			Log.Exception(e);
			OnError(110005);
		}
	}

	private void OnError(int error)
	{
		switch (error)
		{
		case 100208:
			Log.Info($"TChannel PeerDisconnect: {base.RemoteAddress}");
			break;
		case 10054:
			Log.Info($"TChannel ConnectionReset: {base.RemoteAddress}");
			break;
		default:
			Log.Error($"TChannel OnError: {error} {base.RemoteAddress}");
			break;
		}
		long id = Id;
		Service.Remove(id);
		Service.ErrorCallback(id, error);
		Service.DisconnectCallback(id);
	}
}
