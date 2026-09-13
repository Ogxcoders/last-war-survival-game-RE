using System.Collections.Concurrent;
using System.Diagnostics;

namespace ProtoBufNet;

public class NetSendProfiler
{
	private static readonly ConcurrentBag<NetSendProfiler> _pool = new ConcurrentBag<NetSendProfiler>();

	private Stopwatch _stopwatch = new Stopwatch();

	public long useTime { get; private set; }

	public static NetSendProfiler Get()
	{
		if (!_pool.TryTake(out var result))
		{
			return new NetSendProfiler();
		}
		return result;
	}

	public void Recycle()
	{
		useTime = 0L;
		_stopwatch.Reset();
		_pool.Add(this);
	}

	private NetSendProfiler()
	{
	}

	public void Start()
	{
		_stopwatch.Restart();
	}

	public void Stop()
	{
		_stopwatch.Stop();
		useTime = _stopwatch.ElapsedMilliseconds;
	}
}
