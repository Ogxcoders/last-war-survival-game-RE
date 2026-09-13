using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Joker;

public abstract class AService : IDisposable
{
	public Action<long, IPEndPoint> AcceptCallback;

	public Action<long, MemoryBuffer> ReadCallback;

	public Action<long, int> ErrorCallback;

	public Action<long> ConnectCallback;

	public Action<long> DisconnectCallback;

	private const int MaxMemoryBufferSize = 8192;

	private readonly Queue<MemoryBuffer> _pool = new Queue<MemoryBuffer>();

	private static long _msAcceptIdGenerator;

	public long Id { get; set; }

	internal static long CreateChannelId()
	{
		return (uint)Interlocked.Add(ref _msAcceptIdGenerator, 1L);
	}

	public MemoryBuffer Fetch(int size = 0)
	{
		if (size > 8192)
		{
			return new MemoryBuffer(size);
		}
		if (size < 8192)
		{
			size = 8192;
		}
		if (_pool.Count == 0)
		{
			return new MemoryBuffer(size);
		}
		return _pool.Dequeue();
	}

	public void Recycle(MemoryBuffer memoryBuffer)
	{
		if (memoryBuffer.Capacity <= 8192 && _pool.Count <= 10)
		{
			memoryBuffer.Seek(0L, SeekOrigin.Begin);
			memoryBuffer.SetLength(0L);
			_pool.Enqueue(memoryBuffer);
		}
	}

	public virtual void Dispose()
	{
	}

	public abstract void Update();

	public abstract void Remove(long id, int error = 0);

	public abstract bool IsDisposed();

	public abstract void Create(long id, IPEndPoint ipEndPoint);

	public abstract long Create(IPEndPoint ipEndPoint);

	public abstract void Send(long channelId, MemoryBuffer memoryBuffer);

	public virtual long GetChannelId(IPEndPoint address)
	{
		throw new NotImplementedException();
	}

	public virtual IPEndPoint GetChannelAddress(long channelId)
	{
		throw new NotImplementedException();
	}

	public virtual (uint, uint) GetChannelConn(long channelId)
	{
		throw new Exception($"default conn throw Exception! {channelId}");
	}

	public virtual void ChangeAddress(long channelId, IPEndPoint ipEndPoint)
	{
	}
}
