using System;
using System.Collections.Generic;
using System.Threading;

namespace Main.Scripts.Network;

public class FutureManager
{
	private Dictionary<int, msgSendInfo> _sendInfos = new Dictionary<int, msgSendInfo>();

	private int _futureId;

	private const long _timeOutTime = 500L;

	private long _totalCount;

	private float _totalTime;

	private long _minTime = long.MaxValue;

	private long _maxTime;

	public void reset()
	{
		_futureId = 0;
		_totalCount = 0L;
		_totalTime = 0f;
		_minTime = long.MaxValue;
		_maxTime = 0L;
		_sendInfos.Clear();
	}

	public int getFutureId()
	{
		Interlocked.Increment(ref _futureId);
		return _futureId;
	}

	public float getPing()
	{
		if (_totalCount == 0L)
		{
			return 0f;
		}
		return _totalTime / (float)_totalCount;
	}

	public void onSendRequest(int fuid, string msgId)
	{
		msgSendInfo value = new msgSendInfo(fuid, msgId);
		if (!_sendInfos.ContainsKey(fuid))
		{
			_sendInfos.Add(fuid, value);
		}
	}

	public void onServerMsgCome(int fuid, int serverTime)
	{
		if (_sendInfos.ContainsKey(fuid))
		{
			long num = DateTimeOffset.Now.ToUnixTimeMilliseconds() - _sendInfos[fuid].getSendTime() - serverTime;
			_totalCount++;
			_totalTime += num;
			_minTime = Math.Min(_minTime, num);
			_maxTime = Math.Max(_maxTime, num);
		}
		_sendInfos.Remove(fuid);
	}

	public void update()
	{
		long num = DateTimeOffset.Now.ToUnixTimeMilliseconds();
		foreach (msgSendInfo value in _sendInfos.Values)
		{
			_ = num - value.getSendTime();
			_ = 500;
		}
	}
}
