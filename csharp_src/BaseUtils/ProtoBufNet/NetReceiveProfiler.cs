using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using GameFramework;
using Sfs2X.Controllers;

namespace ProtoBufNet;

public class NetReceiveProfiler
{
	public static bool recordCMDId = false;

	private static readonly ConcurrentBag<NetReceiveProfiler> _pool = new ConcurrentBag<NetReceiveProfiler>();

	private Stopwatch _stopwatch = new Stopwatch();

	public long receiveTime { get; private set; }

	public long decryptTime { get; private set; }

	public long uncompressTime { get; private set; }

	public long sfsObjTime { get; private set; }

	public long totalTime { get; private set; }

	public int packetSize { get; private set; }

	public long dispatchTime { get; private set; }

	public string cmd { get; private set; }

	public static NetReceiveProfiler Get()
	{
		if (!_pool.TryTake(out var result))
		{
			return new NetReceiveProfiler();
		}
		return result;
	}

	public void Recycle()
	{
		receiveTime = 0L;
		decryptTime = 0L;
		uncompressTime = 0L;
		sfsObjTime = 0L;
		totalTime = 0L;
		packetSize = 0;
		dispatchTime = 0L;
		cmd = "";
		_stopwatch.Reset();
		_pool.Add(this);
	}

	private NetReceiveProfiler()
	{
	}

	public void OnReceivePacketBegin()
	{
		_stopwatch.Restart();
	}

	public void OnReceivePacketFinish(int size)
	{
		_stopwatch.Stop();
		receiveTime = _stopwatch.ElapsedMilliseconds;
		packetSize = size;
	}

	public void OnReceiveDecryptBegin()
	{
		_stopwatch.Restart();
	}

	public void OnReceiveDecryptFinish()
	{
		_stopwatch.Stop();
		decryptTime = _stopwatch.ElapsedMilliseconds;
	}

	public void OnReceiveUncompressBegin()
	{
		_stopwatch.Restart();
	}

	public void OnReceiveUncompressFinish()
	{
		_stopwatch.Stop();
		uncompressTime = _stopwatch.ElapsedMilliseconds;
	}

	public void OnReceiveDecodeSFSObjectBegin()
	{
		_stopwatch.Restart();
	}

	public void OnReceiveDecodeSFSObjectFinish()
	{
		_stopwatch.Stop();
		sfsObjTime = _stopwatch.ElapsedMilliseconds;
		totalTime = receiveTime + decryptTime + uncompressTime + sfsObjTime;
	}

	public void OnReceiveDispatchBegin(INetPacket p)
	{
		_stopwatch.Restart();
		if (recordCMDId)
		{
			try
			{
				cmd = p.info.GetSFSObject(MessageDispather.PARAM_ID)?.GetUtfString(ExtensionController.KEY_CMD) ?? "unknown";
				return;
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				return;
			}
		}
		cmd = "c";
	}

	public void OnReceiveDispatchFinish()
	{
		_stopwatch.Stop();
		dispatchTime = _stopwatch.ElapsedMilliseconds;
	}
}
