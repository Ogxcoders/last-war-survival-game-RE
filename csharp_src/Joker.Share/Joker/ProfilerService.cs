using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Joker;

public class ProfilerService : IService, IUpdateService
{
	public class SampleData
	{
		public static readonly float kMsFreqInv = 1000f / (float)Stopwatch.Frequency;

		public long TotalTicks;

		public int CallCount;

		public long MaxTicks;

		public long MinTicks = long.MaxValue;

		public string Name;

		public ConcurrentDictionary<string, SampleData> Children = new ConcurrentDictionary<string, SampleData>();

		public SampleData Parent;

		public float TotalTime => (float)TotalTicks * kMsFreqInv;

		public float AverageTime
		{
			get
			{
				if (CallCount <= 0)
				{
					return 0f;
				}
				return (float)TotalTicks * kMsFreqInv / (float)CallCount;
			}
		}

		public float MaxTime => (float)MaxTicks * kMsFreqInv;

		public float MinTime => (float)MinTicks * kMsFreqInv;

		public void Reset()
		{
			TotalTicks = 0L;
			MaxTicks = long.MinValue;
			MinTicks = long.MaxValue;
			CallCount = 0;
			foreach (SampleData value in Children.Values)
			{
				value.Reset();
			}
		}
	}

	public class Sample : IDisposable
	{
		private bool _disposed;

		public Sample(string name)
		{
			try
			{
				Instance.BeginSample(name);
			}
			catch (Exception arg)
			{
				Instance.Log.Error($"Sample constructor error: {arg}");
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				try
				{
					Instance.EndSample();
				}
				catch (Exception arg)
				{
					Instance.Log.Error($"Sample dispose error: {arg}");
				}
				_disposed = true;
			}
		}
	}

	protected ILogger Log;

	private readonly object _lock = new object();

	private float _runningTime;

	private readonly ConcurrentDictionary<string, SampleData> _samples = new ConcurrentDictionary<string, SampleData>();

	private readonly ConcurrentStack<Stopwatch> _watchStack = new ConcurrentStack<Stopwatch>();

	private readonly ConcurrentStack<string> _nameStack = new ConcurrentStack<string>();

	private readonly List<SampleData> _sortedSamples = new List<SampleData>(128);

	private readonly ObjectPool<Stopwatch> _watchPool = new ObjectPool<Stopwatch>(null, delegate(Stopwatch sw)
	{
		sw.Reset();
	});

	private List<SampleData> _totalTimeTop = new List<SampleData>(128);

	private List<SampleData> _avgTimeTop = new List<SampleData>(128);

	private float _reportLastTime;

	private Action _reportCallback;

	private int _reportTopCount = 10;

	private long _reportMinTicks = 10000L;

	private float _reportInterval = 300f;

	private float _reportFirstDelay = 300f;

	private bool _reportFirstCommited;

	private SampleData _currentSample;

	public static ProfilerService Instance { get; private set; }

	public ProfilerService()
	{
		Instance = this;
		_reportCallback = ReportProfilerInfo;
	}

	public ProfilerService(float interval, float minTime)
		: this()
	{
		SetReportSettings(10, interval, interval, minTime);
	}

	public void SetReportCallback(Action callback)
	{
		lock (_lock)
		{
			_reportCallback = callback;
		}
	}

	public void SetReportSettings(int topCount = 10, float reportInterval = 300f, float firstReportDelay = 300f, float minTime = 0.03f)
	{
		lock (_lock)
		{
			_reportTopCount = Math.Max(1, topCount);
			_reportMinTicks = Math.Max(0L, (long)(minTime * (float)Stopwatch.Frequency));
			if (_reportTopCount > _totalTimeTop.Count)
			{
				_totalTimeTop.Capacity = _reportTopCount;
				_avgTimeTop.Capacity = _reportTopCount;
			}
			_reportInterval = Math.Max(1f, reportInterval);
			_reportFirstDelay = Math.Max(1f, firstReportDelay);
		}
	}

	public List<SampleData> GetTopTotalTimeSamples(int count)
	{
		lock (_lock)
		{
			_sortedSamples.Clear();
			_sortedSamples.AddRange(_samples.Values);
			_totalTimeTop.Clear();
			count = Math.Min(count, _sortedSamples.Count);
			if (count > 0)
			{
				QuickSelectTopK(_sortedSamples, count, (SampleData a, SampleData b) => b.TotalTicks.CompareTo(a.TotalTicks));
				for (int num = 0; num < count; num++)
				{
					_totalTimeTop.Add(_sortedSamples[num]);
				}
			}
			_totalTimeTop.Sort((SampleData a, SampleData b) => b.TotalTicks.CompareTo(a.TotalTicks));
			return new List<SampleData>(_totalTimeTop);
		}
	}

	public List<SampleData> GetTopAverageTimeSamples(int count)
	{
		lock (_lock)
		{
			_sortedSamples.Clear();
			_sortedSamples.AddRange(_samples.Values);
			_avgTimeTop.Clear();
			count = Math.Min(count, _sortedSamples.Count);
			if (count > 0)
			{
				QuickSelectTopK(_sortedSamples, count, (SampleData a, SampleData b) => b.AverageTime.CompareTo(a.AverageTime));
				for (int num = 0; num < count; num++)
				{
					_avgTimeTop.Add(_sortedSamples[num]);
				}
			}
			_avgTimeTop.Sort((SampleData a, SampleData b) => b.AverageTime.CompareTo(a.AverageTime));
			return new List<SampleData>(_avgTimeTop);
		}
	}

	public Sample CreateSample(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return null;
		}
		return new Sample(name);
	}

	public void BeginSample(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return;
		}
		lock (_lock)
		{
			Stopwatch stopwatch = _watchPool.Get();
			stopwatch.Start();
			_watchStack.Push(stopwatch);
			_nameStack.Push(name);
			SampleData value = null;
			if (_currentSample != null)
			{
				if (!_currentSample.Children.TryGetValue(name, out value))
				{
					value = new SampleData
					{
						Name = name,
						Parent = _currentSample
					};
					_currentSample.Children.TryAdd(name, value);
				}
			}
			else if (!_samples.TryGetValue(name, out value))
			{
				value = new SampleData
				{
					Name = name
				};
				_samples.TryAdd(name, value);
			}
			_currentSample = value;
		}
	}

	public void EndSample()
	{
		if (_watchStack.IsEmpty || _nameStack.IsEmpty)
		{
			return;
		}
		lock (_lock)
		{
			if (_watchStack.TryPop(out var result) && _nameStack.TryPop(out var result2))
			{
				result.Stop();
				CommitSample(result2, result.ElapsedTicks);
				_watchPool.Release(result);
				if (_currentSample != null)
				{
					_currentSample = _currentSample.Parent;
				}
			}
			else
			{
				SafeClearSampleStack();
			}
		}
	}

	private void CommitSample(string name, long elapsedTicks)
	{
		if (elapsedTicks < 0)
		{
			return;
		}
		lock (_lock)
		{
			if (_currentSample != null)
			{
				_currentSample.TotalTicks += elapsedTicks;
				_currentSample.MaxTicks = Math.Max(_currentSample.MaxTicks, elapsedTicks);
				_currentSample.MinTicks = Math.Min(_currentSample.MinTicks, elapsedTicks);
				_ = name == "App.Update";
				_currentSample.CallCount++;
				if (_currentSample.Parent == null)
				{
					CheckReport();
				}
			}
		}
	}

	public void ResetSamples()
	{
		lock (_lock)
		{
			foreach (SampleData value in _samples.Values)
			{
				value.Reset();
			}
		}
	}

	private void CheckReport()
	{
		if (_reportCallback == null)
		{
			return;
		}
		lock (_lock)
		{
			if (!_reportFirstCommited)
			{
				if (_runningTime < _reportFirstDelay)
				{
					return;
				}
				_reportFirstCommited = true;
				_reportLastTime = _runningTime;
			}
			else if (_runningTime - _reportLastTime < _reportInterval)
			{
				return;
			}
			try
			{
				_reportCallback();
				ResetSamples();
				_reportLastTime = _runningTime;
			}
			catch (Exception arg)
			{
				Log.Error($"CheckReport error: {arg}");
			}
		}
	}

	private void QuickSelectTopK(List<SampleData> list, int k, Comparison<SampleData> comparison)
	{
		int num = 0;
		int num2 = list.Count - 1;
		while (num < num2)
		{
			int num3 = Partition(list, num, num2, comparison);
			if (num3 != k - 1)
			{
				if (num3 < k - 1)
				{
					num = num3 + 1;
				}
				else
				{
					num2 = num3 - 1;
				}
				continue;
			}
			break;
		}
	}

	private int Partition(List<SampleData> list, int left, int right, Comparison<SampleData> comparison)
	{
		int num = left + (right - left) / 2;
		SampleData y = list[num];
		Swap(list, num, right);
		int num2 = left;
		for (int i = left; i < right; i++)
		{
			if (comparison(list[i], y) <= 0)
			{
				Swap(list, i, num2);
				num2++;
			}
		}
		Swap(list, num2, right);
		return num2;
	}

	private void Swap(List<SampleData> list, int i, int j)
	{
		SampleData sampleData = list[j];
		SampleData sampleData2 = list[i];
		SampleData sampleData3 = (list[i] = sampleData);
		sampleData3 = (list[j] = sampleData2);
	}

	public string DumpTopTotalTimeProfilerTree(int topCount = 10)
	{
		StringBuilder stringBuilder = new StringBuilder(2048);
		foreach (SampleData topTotalTimeSample in GetTopTotalTimeSamples(topCount))
		{
			if (topTotalTimeSample.CallCount > 0 && topTotalTimeSample.TotalTicks > _reportMinTicks)
			{
				DumpSampleRecursive(topTotalTimeSample, stringBuilder, 0);
			}
		}
		return stringBuilder.ToString();
	}

	public string DumpTopAverageTimeProfilerTree(int topCount = 10)
	{
		StringBuilder stringBuilder = new StringBuilder(2048);
		foreach (SampleData topAverageTimeSample in GetTopAverageTimeSamples(topCount))
		{
			if (topAverageTimeSample.CallCount > 0 && topAverageTimeSample.TotalTicks >= _reportMinTicks)
			{
				DumpSampleRecursive(topAverageTimeSample, stringBuilder, 0);
			}
		}
		return stringBuilder.ToString();
	}

	private void DumpSampleRecursive(SampleData sample, StringBuilder sb, int depth)
	{
		for (int i = 0; i < depth * 2; i++)
		{
			sb.Append("  ");
		}
		sb.Append(sample.Name).Append(": ").Append(sample.TotalTime)
			.Append("ms (Avg: ")
			.Append(sample.AverageTime.ToString("F2"))
			.Append("ms Max: ")
			.Append(sample.MaxTime.ToString("F2"))
			.Append("ms Min: ")
			.Append(sample.MinTime.ToString("F2"))
			.Append("ms, Count: ")
			.Append(sample.CallCount)
			.AppendLine(")");
		foreach (SampleData value in sample.Children.Values)
		{
			if (value.CallCount > 0 && value.TotalTicks >= _reportMinTicks)
			{
				DumpSampleRecursive(value, sb, depth + 1);
			}
		}
	}

	public void DumpToLog(int topCount = 10, bool separateLog = false)
	{
		if (separateLog)
		{
			foreach (SampleData topTotalTimeSample in GetTopTotalTimeSamples(topCount))
			{
				if (topTotalTimeSample.CallCount > 0 && topTotalTimeSample.TotalTicks > _reportMinTicks)
				{
					StringBuilder stringBuilder = new StringBuilder(2048);
					DumpSampleRecursive(topTotalTimeSample, stringBuilder, 0);
					Log.Info($"Total Time Sample:\n{stringBuilder}");
				}
			}
			{
				foreach (SampleData topAverageTimeSample in GetTopAverageTimeSamples(topCount))
				{
					if (topAverageTimeSample.CallCount > 0 && topAverageTimeSample.TotalTicks > _reportMinTicks)
					{
						StringBuilder stringBuilder2 = new StringBuilder(2048);
						DumpSampleRecursive(topAverageTimeSample, stringBuilder2, 0);
						Log.Info($"Average Time Sample:\n{stringBuilder2}");
					}
				}
				return;
			}
		}
		lock (_lock)
		{
			string text = DumpTopTotalTimeProfilerTree(topCount);
			string text2 = DumpTopAverageTimeProfilerTree(topCount);
			Log.Info("Total Time Top:\n" + text);
			Log.Info("Average Time Top:\n" + text2);
		}
	}

	public virtual void ReportProfilerInfo()
	{
		string text = DumpTopAverageTimeProfilerTree(_reportTopCount);
		Log.Info("Total Time Top:\n" + text);
	}

	public void Awake()
	{
	}

	public void Startup()
	{
		Log = Joker.Log.GetLogger<ProfilerService>();
	}

	public void Shutdown()
	{
		SafeClearSampleStack();
	}

	public void Destroy()
	{
	}

	public void Update(float dt)
	{
		_runningTime += dt;
	}

	protected void SafeClearSampleStack()
	{
		lock (_lock)
		{
			Stopwatch result;
			while (_watchStack.TryPop(out result))
			{
				result.Stop();
				_watchPool.Release(result);
			}
			string result2;
			while (_nameStack.TryPop(out result2))
			{
				Log.Error("Stack sample invalid: " + result2);
			}
			_currentSample = null;
		}
	}
}
