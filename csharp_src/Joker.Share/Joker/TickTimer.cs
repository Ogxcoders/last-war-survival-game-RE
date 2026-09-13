using System;
using System.Diagnostics;

namespace Joker;

public class TickTimer
{
	public const int KMillisecondToSecond = 1000;

	private Stopwatch _stopwatch;

	private long _nextTime;

	public bool IsRunning => _stopwatch?.IsRunning ?? false;

	public long StartTime { get; private set; }

	public long Interval { get; private set; }

	public TickTimer()
	{
	}

	public TickTimer(long interval)
	{
		Start(interval);
	}

	public TickTimer(long timeStart, long interval)
	{
		Start(timeStart, interval);
	}

	public TickTimer(long interval, bool immediate)
	{
		Start(interval, immediate);
	}

	public TickTimer(long timeStart, long interval, bool immediate)
	{
		Start(timeStart, interval, immediate);
	}

	public void Start(long interval)
	{
		Start(interval, immediate: false);
	}

	public void Start(long timeStart, long interval)
	{
		Start(timeStart, interval, immediate: false);
	}

	public void Start(long interval, bool immediate)
	{
		Start(0L, interval, immediate);
	}

	public void Start(long timeStart, long interval, bool immediate)
	{
		if (_stopwatch != null)
		{
			if (_stopwatch.IsRunning)
			{
				_stopwatch.Restart();
			}
			else
			{
				_stopwatch.Start();
			}
		}
		else
		{
			_stopwatch = new Stopwatch();
			_stopwatch.Start();
		}
		if (interval <= 0)
		{
			throw new ArgumentException("interval must be greater than 0");
		}
		Interval = interval;
		if (immediate)
		{
			_nextTime = timeStart;
		}
		else
		{
			_nextTime = timeStart + interval;
		}
		StartTime = timeStart;
	}

	public void Stop()
	{
		_stopwatch.Stop();
	}

	public bool IsOnce()
	{
		if (!IsRunning)
		{
			return false;
		}
		if (_nextTime > _stopwatch.ElapsedMilliseconds)
		{
			return false;
		}
		Stop();
		return true;
	}

	public bool IsPeriod()
	{
		if (!IsRunning)
		{
			return false;
		}
		if (_nextTime > _stopwatch.ElapsedMilliseconds)
		{
			return false;
		}
		_nextTime += Interval;
		return true;
	}

	public long GetTimeLeft()
	{
		if (!IsRunning)
		{
			return 0L;
		}
		long elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;
		if (_nextTime <= elapsedMilliseconds)
		{
			return 0L;
		}
		return _nextTime - elapsedMilliseconds;
	}

	public void Restart()
	{
		if (!IsRunning)
		{
			throw new InvalidOperationException("timer is not running");
		}
		_nextTime = _stopwatch.ElapsedMilliseconds + Interval;
	}

	public void SetTimeNext(long next)
	{
		_nextTime = next;
	}

	public void ExtendTimeNext(long extend)
	{
		_nextTime += extend;
	}
}
