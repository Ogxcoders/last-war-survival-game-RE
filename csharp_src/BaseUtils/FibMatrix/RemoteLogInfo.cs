using System;
using System.Collections.Generic;

namespace FibMatrix;

public class RemoteLogInfo : Dictionary<string, object>, IDisposable
{
	private static readonly ThreadSafeObjectPool<RemoteLogInfo> _pool = new ThreadSafeObjectPool<RemoteLogInfo>(() => new RemoteLogInfo());

	private static int _seqCounter = 0;

	private int _seq;

	private DateTime _dateTime;

	private string _uid;

	private string _sid;

	private LogLevel _level;

	public void SyncAllParams(ClientInfo info)
	{
		base["uid"] = _uid;
		base["sid"] = _sid;
		base["_seq_"] = _seq.ToString();
		base["_datetime_"] = _dateTime.ToString("O");
		base["_level_"] = _level.ToString();
		info.CopyToDictionary(this);
	}

	public void Dispose()
	{
		Clear();
	}

	public static RemoteLogInfo Allocate(LogLevel level, string userId, string serverId)
	{
		RemoteLogInfo remoteLogInfo = _pool.Allocate();
		remoteLogInfo._seq = ++_seqCounter;
		remoteLogInfo._dateTime = DateTime.Now;
		remoteLogInfo._level = level;
		remoteLogInfo._uid = userId;
		remoteLogInfo._sid = serverId;
		return remoteLogInfo;
	}

	public static void Recycle(RemoteLogInfo info)
	{
		_pool.Recycle(info);
	}
}
