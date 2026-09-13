using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using UnityEngine;

public class TimerComponent : IGameController
{
	private class Timer : ITimer
	{
		private bool _isPause;

		private readonly Action _onComplete;

		private float repeatSec;

		private float finishTime;

		private float leftTime;

		public bool isCompleted { get; private set; }

		public bool isCancelled { get; private set; }

		public bool isPause
		{
			get
			{
				return _isPause;
			}
			set
			{
				_isPause = value;
				if (!isDone)
				{
					if (_isPause)
					{
						leftTime = finishTime - GetNowTime();
					}
					else
					{
						finishTime = GetNowTime() + leftTime;
					}
				}
			}
		}

		public bool isDone
		{
			get
			{
				if (!isCompleted)
				{
					return isCancelled;
				}
				return true;
			}
		}

		public void Cancel()
		{
			if (!isDone)
			{
				isCancelled = true;
			}
		}

		public Timer(float delaySec, float repeatSec, Action onComplete)
		{
			_onComplete = onComplete;
			this.repeatSec = repeatSec;
			finishTime = GetNowTime() + delaySec;
		}

		private float GetNowTime()
		{
			return Time.time;
		}

		public void Update()
		{
			if (!isDone && !_isPause && GetNowTime() >= finishTime)
			{
				try
				{
					_onComplete?.Invoke();
				}
				catch (Exception ex)
				{
					Log.Error("定时器回调执行异常: " + ex.Message + "\n堆栈跟踪: " + ex.StackTrace);
				}
				if (repeatSec >= 0f)
				{
					finishTime = GetNowTime() + repeatSec;
				}
				else
				{
					isCompleted = true;
				}
			}
		}
	}

	private long m_ServerDeltaTime;

	private int m_TimeZone;

	private int m_FrameCount;

	private bool m_IsNight;

	private bool _2Hours = true;

	private int _serverOffset;

	private DateTime _orignTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private bool _light;

	public static bool useRealTime;

	private List<Timer> timers = new List<Timer>();

	private List<Timer> timersToAdd = new List<Timer>();

	private long _minDeltaTime = long.MaxValue;

	private long _serverBaseTime;

	private long _serverTimeSyncTime;

	private volatile int _serverTimeGetFrameCount;

	private long _serverTime;

	private readonly object _lock = new object();

	public bool Light
	{
		get
		{
			return _light;
		}
		set
		{
			if (_light != value)
			{
				_light = value;
				GameEntry.Event.Fire(EventId.LightChange);
			}
		}
	}

	public long Tomorrow { get; set; }

	public TimerComponent()
	{
		_light = false;
		ResetServerTimeSync();
	}

	public void Shutdown()
	{
		CancelAllTimers();
		ResetServerTimeSync();
	}

	public void OnUpdate(float elapseSeconds)
	{
		UpdateAllTimers();
	}

	public void SetServerOffset(int offset)
	{
		_serverOffset = offset;
	}

	public void SetWorldTime(int t, int tz)
	{
	}

	public long ChangeTime(long t)
	{
		return t - _serverOffset;
	}

	public void UpdateServerMilliseconds(long ms)
	{
		m_ServerDeltaTime = ms - GetLocalMilliseconds();
	}

	public long GetLocalMilliseconds()
	{
		return (long)DateTime.UtcNow.Subtract(_orignTime).TotalMilliseconds;
	}

	public long GetLocalSeconds()
	{
		return (long)DateTime.UtcNow.Subtract(_orignTime).TotalSeconds;
	}

	public long GetServerTime()
	{
		if (useRealTime)
		{
			return __GetServerTime();
		}
		return m_ServerDeltaTime + GetLocalMilliseconds();
	}

	public int GetServerTimeSeconds()
	{
		return (int)(GetServerTime() / 1000);
	}

	public string MillisecondToSecondString(long mstime, string separator)
	{
		mstime /= 1000;
		return SecondsToSecondString(mstime, separator);
	}

	public string MillisecondsToStringWithoutHour(long mstime, string separator)
	{
		mstime /= 1000;
		return SecondsToStringWithoutHour(mstime, separator);
	}

	public string SecondsToStringWithoutHour(long second, string separator)
	{
		long num = ((second < 0) ? 0 : second);
		StringBuilder stringBuilder = new StringBuilder();
		long num2 = num / 60;
		if (num2 < 10)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num2);
		stringBuilder.Append(separator);
		long num3 = num % 60;
		if (num3 < 10)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num3);
		return stringBuilder.ToString();
	}

	public string SecondsToSecondString(long second, string separator)
	{
		long num = ((second < 0) ? 0 : second);
		StringBuilder stringBuilder = new StringBuilder();
		long num2 = num / 3600;
		if (num2 < 10)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num2);
		stringBuilder.Append(separator);
		long num3 = num % 3600;
		long num4 = num3 / 60;
		if (num4 < 10)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num4);
		stringBuilder.Append(separator);
		long num5 = num3 % 60;
		if (num5 < 10)
		{
			stringBuilder.Append("0");
		}
		stringBuilder.Append(num5);
		return stringBuilder.ToString();
	}

	public string MilliSecondToFmtString(long milliSecond)
	{
		int num = (int)(milliSecond / 1000);
		return SecondToFmtString(num);
	}

	public string SecondToFmtString(long secs)
	{
		if (secs > 86400)
		{
			return $"{secs / 86400}d {secs / 3600 % 24:00}:{secs / 60 % 60:00}:{secs % 60:00}";
		}
		return $"{secs / 3600:00}:{secs / 60 % 60:00}:{secs % 60:00}";
	}

	public string MilliSecondToFmtStringHighestTime(long milliSecond)
	{
		int num = (int)(milliSecond / 1000);
		return SecondToFmtStringHighestTime(num);
	}

	public string SecondToFmtStringHighestTime(long secs)
	{
		if (secs > 86400)
		{
			return $"{secs / 86400}d {secs / 3600 % 24:00}:{secs / 60 % 60:00}:{secs % 60:00}";
		}
		if (secs > 3600)
		{
			return $"{secs / 3600:00}:{secs / 60 % 60:00}:{secs % 60:00}";
		}
		if (secs > 60)
		{
			return $"{secs / 60:00}:{secs % 60:00}";
		}
		return $"{secs:00}";
	}

	public long RefixTimebyZone(long time)
	{
		if (time <= 0)
		{
			return 0L;
		}
		return time + m_TimeZone * 3600;
	}

	public string PassTimeSecondString(long passTime, bool isMstime = true)
	{
		if (isMstime)
		{
			passTime /= 1000;
		}
		if (passTime > 86400)
		{
			int num = (int)(passTime / 86400);
			return GameEntry.Localization.GetString("390506", num);
		}
		if (passTime > 3600)
		{
			int num2 = (int)(passTime / 3600);
			return GameEntry.Localization.GetString("390505", num2);
		}
		if (passTime > 60)
		{
			int num3 = (int)(passTime / 60);
			return GameEntry.Localization.GetString("390504", num3);
		}
		return GameEntry.Localization.GetString("100355");
	}

	public DateTime GetDateTime(long timeStamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		long ticks = timeStamp * 10000;
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value);
	}

	public string TimeStampToTimeSimple(long timestamp, string format = "T")
	{
		return GetDateTime(timestamp).ToString(format);
	}

	public string TimeStampToTimeDate(long timestamp, string format = "yyyy-MM-dd")
	{
		return GetDateTime(timestamp).ToString(format);
	}

	public string TimeStampToTimeDateMd(long timestamp, string format = "MM-dd")
	{
		return GetDateTime(timestamp).ToString(format);
	}

	public string TimeStampToTime(long timestamp, string format = "yyyy-MM-dd HH:mm:ss")
	{
		return GetDateTime(timestamp).ToString(format);
	}

	public string TimeStampToHMS(long timestamp, string format = "HH:mm:ss")
	{
		return GetDateTime(timestamp).ToString(format);
	}

	public string GetUniversalTime(long timeStamp, string format = "yyyy-MM-dd HH:mm:ss")
	{
		timeStamp = ChangeTime(timeStamp);
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long ticks = timeStamp * 10000;
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value).ToString(format);
	}

	public string GetUniversalChatTime(long timeStamp, string format = "yyyy-MM-dd HH:mm:ss")
	{
		DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
		long ticks = timeStamp * 10000;
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value).ToString(format);
	}

	public long GetCurZoneTimestamp(int year, int month, int day, int hour, int min, int sec)
	{
		return new DateTimeOffset(new DateTime(year, month, day, hour, min, sec)).ToUnixTimeSeconds();
	}

	public long GetUTCTimestamp(int year, int month, int day, int hour, int min, int sec)
	{
		return new DateTimeOffset(new DateTime(year, month, day, hour, min, sec, DateTimeKind.Utc)).ToUnixTimeSeconds();
	}

	public int GetServerWeekDay()
	{
		long serverTime = GetServerTime();
		DateTime dateTime = GetDateTime(serverTime);
		return WeekDay(dateTime);
	}

	public int WeekDay(DateTime time)
	{
		int num = (int)time.DayOfWeek;
		if (num == 0)
		{
			num = 7;
		}
		return num;
	}

	public int GetWeekDay(long milliSecond)
	{
		DateTime time = new DateTime(milliSecond);
		return WeekDay(time);
	}

	public int GetResSecondsTo24()
	{
		return 86400 - GetServerTimeSeconds() % 86400;
	}

	public DateTime TimeStampToServerTime(long timeStamp)
	{
		timeStamp += _serverOffset;
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		long ticks = timeStamp * 10000;
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value);
	}

	public DateTime TimeStampToLocalTime(long timeStamp)
	{
		DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		long ticks = timeStamp * 10000;
		TimeSpan value = new TimeSpan(ticks);
		return dateTime.Add(value).ToLocalTime();
	}

	public DateTime GetLocalTimeFromUtcOffset(long timeStamp, float offset)
	{
		TimeSpan offset2 = TimeSpan.FromHours(offset);
		return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).ToOffset(offset2).DateTime;
	}

	public ITimer RegisterTimer(float delaySec, Action onComplete)
	{
		Timer timer = new Timer(delaySec, -1f, onComplete);
		timersToAdd.Add(timer);
		return timer;
	}

	public ITimer RegisterTimerRepeat(float delaySec, float repeatSec, Action onComplete)
	{
		Timer timer = new Timer(delaySec, repeatSec, onComplete);
		timersToAdd.Add(timer);
		return timer;
	}

	public void CancelTimer(ITimer timer)
	{
		if (timer != null)
		{
			((Timer)timer).Cancel();
		}
	}

	public void CancelAllTimers()
	{
		foreach (Timer timer in timers)
		{
			timer.Cancel();
		}
		timers.Clear();
		timersToAdd.Clear();
	}

	private void UpdateAllTimers()
	{
		if (timersToAdd.Count > 0)
		{
			timers.AddRange(timersToAdd);
			timersToAdd.Clear();
		}
		foreach (Timer timer in timers)
		{
			timer.Update();
		}
		timers.RemoveAll((Timer t) => t.isDone);
	}

	private void ResetServerTimeSync()
	{
		_minDeltaTime = long.MaxValue;
		_serverBaseTime = 0L;
		_serverTimeSyncTime = 0L;
		_serverTimeGetFrameCount = 0;
		_serverTime = 0L;
	}

	public void SyncServerTime(long recvTime, long serverTime, long clientTime, bool reset)
	{
		lock (_lock)
		{
			long num = recvTime - clientTime;
			bool flag = _minDeltaTime == long.MaxValue;
			if (num < _minDeltaTime || num < 200 || _minDeltaTime == 0 || reset)
			{
				_minDeltaTime = num;
				_serverBaseTime = serverTime + (long)((float)_minDeltaTime * 0.5f);
				_serverTimeSyncTime = recvTime;
			}
			if (recvTime == clientTime && flag)
			{
				_minDeltaTime = long.MaxValue;
			}
		}
	}

	public long __GetServerTime()
	{
		lock (_lock)
		{
			return _serverBaseTime + (RealTimer.elapsedMilliseconds - _serverTimeSyncTime);
		}
	}

	public long GetServerTimeWithoutOffset()
	{
		lock (_lock)
		{
			return _minDeltaTime;
		}
	}

	public long GetServerTimeCS()
	{
		return __GetServerTime();
	}
}
