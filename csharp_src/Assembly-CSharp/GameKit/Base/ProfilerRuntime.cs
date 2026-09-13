using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using GameFramework;
using UnityEngine;

namespace GameKit.Base;

public class ProfilerRuntime
{
	private class SafeClearSampleStackUpdater : IFastLateUpdate
	{
		public void DoLateUpdate()
		{
			if (_enabled)
			{
				SafeClearSampleStack();
			}
		}
	}

	public class SampleData
	{
		public long TotalTime;

		public int CallCount;

		public string Name;

		public Dictionary<string, SampleData> Children = new Dictionary<string, SampleData>();

		public SampleData Parent;

		public float AverageTime
		{
			get
			{
				if (CallCount <= 0)
				{
					return 0f;
				}
				return (float)TotalTime / (float)CallCount;
			}
		}

		public void Reset()
		{
			TotalTime = 0L;
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
				BeginSample(name);
			}
			catch (Exception arg)
			{
				Log.Error($"ProfilerRuntime.Sample constructor error: {arg}");
			}
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				try
				{
					EndSample();
				}
				catch (Exception arg)
				{
					Log.Error($"ProfilerRuntime.Sample dispose error: {arg}");
				}
				_disposed = true;
			}
		}
	}

	private static bool _enabled = false;

	private static IFastLateUpdate _safeChecker = null;

	private static readonly Dictionary<string, SampleData> _samples = new Dictionary<string, SampleData>(128);

	private static readonly Stack<Stopwatch> _watchStack = new Stack<Stopwatch>(16);

	private static readonly Stack<string> _nameStack = new Stack<string>(16);

	private static readonly List<SampleData> _sortedSamples = new List<SampleData>(128);

	private static readonly ObjectPool<Stopwatch> _watchPool = new ObjectPool<Stopwatch>(null, delegate(Stopwatch sw)
	{
		sw.Reset();
	});

	private static List<SampleData> _totalTimeTop = new List<SampleData>(128);

	private static List<SampleData> _avgTimeTop = new List<SampleData>(128);

	private static readonly StringBuilder _dumpBuilder = new StringBuilder(2048);

	private static Action _reportCallback;

	private static int _reportTopCount = 10;

	private static float _reportMinTime = 5f;

	private static float _reportInterval = 300f;

	private static float _reportLastTime;

	private static float _reportFirstDelay = 300f;

	private static bool _reportFirstCommited = false;

	private static SampleData _currentSample = null;

	public static bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			if (_enabled == value)
			{
				return;
			}
			_enabled = value;
			if (_enabled)
			{
				if (_safeChecker == null)
				{
					_safeChecker = new SafeClearSampleStackUpdater();
					SingletonBehaviour<FastUpdater>.Instance.Add(_safeChecker);
				}
			}
			else if (_safeChecker != null)
			{
				SingletonBehaviour<FastUpdater>.Instance.Remove(_safeChecker);
				_safeChecker = null;
			}
		}
	}

	public static void SetReportCallback(Action callback)
	{
		_reportCallback = callback;
	}

	public static void SetReportSettings(int topCount = 10, float reportInterval = 300f, float firstReportDelay = 300f, float minTime = 3f)
	{
		_reportTopCount = Mathf.Max(1, topCount);
		_reportMinTime = Mathf.Max(0.1f, minTime);
		if (_reportTopCount > _totalTimeTop.Count)
		{
			_totalTimeTop.Capacity = _reportTopCount;
			_avgTimeTop.Capacity = _reportTopCount;
		}
		_reportInterval = Mathf.Max(0.1f, reportInterval);
		_reportFirstDelay = Mathf.Max(0.1f, firstReportDelay);
	}

	public static List<SampleData> GetTopTotalTimeSamples(int count)
	{
		_sortedSamples.Clear();
		_sortedSamples.AddRange(_samples.Values);
		_totalTimeTop.Clear();
		count = Mathf.Min(count, _sortedSamples.Count);
		if (count > 0)
		{
			QuickSelectTopK(_sortedSamples, count, (SampleData a, SampleData b) => b.TotalTime.CompareTo(a.TotalTime));
			for (int num = 0; num < count; num++)
			{
				_totalTimeTop.Add(_sortedSamples[num]);
			}
		}
		_totalTimeTop.Sort((SampleData a, SampleData b) => b.TotalTime.CompareTo(a.TotalTime));
		return _totalTimeTop;
	}

	public static List<SampleData> GetTopAverageTimeSamples(int count)
	{
		_sortedSamples.Clear();
		_sortedSamples.AddRange(_samples.Values);
		_avgTimeTop.Clear();
		count = Mathf.Min(count, _sortedSamples.Count);
		if (count > 0)
		{
			QuickSelectTopK(_sortedSamples, count, (SampleData a, SampleData b) => b.AverageTime.CompareTo(a.AverageTime));
			for (int num = 0; num < count; num++)
			{
				_avgTimeTop.Add(_sortedSamples[num]);
			}
		}
		_avgTimeTop.Sort((SampleData a, SampleData b) => b.AverageTime.CompareTo(a.AverageTime));
		return _avgTimeTop;
	}

	public static Sample CreateSample(string name)
	{
		if (!Enabled || string.IsNullOrEmpty(name))
		{
			return null;
		}
		return new Sample(name);
	}

	public static void BeginSample(string name)
	{
		if (!Enabled || string.IsNullOrEmpty(name))
		{
			return;
		}
		Stopwatch stopwatch = _watchPool.Get();
		stopwatch.Start();
		_watchStack.Push(stopwatch);
		_nameStack.Push(name);
		SampleData value;
		if (_currentSample != null)
		{
			if (!_currentSample.Children.TryGetValue(name, out value))
			{
				value = new SampleData
				{
					Name = name,
					Parent = _currentSample
				};
				_currentSample.Children[name] = value;
			}
		}
		else if (!_samples.TryGetValue(name, out value))
		{
			value = new SampleData
			{
				Name = name
			};
			_samples[name] = value;
		}
		_currentSample = value;
	}

	public static void EndSample()
	{
		if (Enabled && _watchStack.Count != 0 && _nameStack.Count != 0)
		{
			Stopwatch stopwatch = _watchStack.Pop();
			stopwatch.Stop();
			CommitSample(_nameStack.Pop(), stopwatch.ElapsedMilliseconds);
			_watchPool.Release(stopwatch);
			if (_currentSample != null)
			{
				_currentSample = _currentSample.Parent;
			}
		}
	}

	private static void CommitSample(string name, long elapsedMs)
	{
		if (Enabled && elapsedMs >= 0 && _currentSample != null)
		{
			_currentSample.TotalTime += elapsedMs;
			_currentSample.CallCount++;
			if (_currentSample.Parent == null)
			{
				CheckReport();
			}
		}
	}

	private static void SafeClearSampleStack()
	{
		while (_watchStack.Count > 0)
		{
			Stopwatch stopwatch = _watchStack.Pop();
			stopwatch.Stop();
			_watchPool.Release(stopwatch);
		}
		if (_nameStack.Count > 0)
		{
			Log.Error(string.Format("ProfilerRuntime.SafeClearSampleStack nameStack count: {0} {1}", _nameStack.Count, string.Join(",", _nameStack)));
			_nameStack.Clear();
		}
		_currentSample = null;
	}

	public static void ResetSamples()
	{
		foreach (SampleData value in _samples.Values)
		{
			value.Reset();
		}
	}

	private static void CheckReport()
	{
		if (!Enabled || _reportCallback == null)
		{
			return;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (!_reportFirstCommited)
		{
			if (realtimeSinceStartup < _reportFirstDelay)
			{
				return;
			}
			_reportFirstCommited = true;
			_reportLastTime = realtimeSinceStartup;
		}
		else if (realtimeSinceStartup - _reportLastTime < _reportInterval)
		{
			return;
		}
		try
		{
			_reportCallback();
			_reportLastTime = realtimeSinceStartup;
		}
		catch (Exception arg)
		{
			Log.Error($"ProfilerRuntime.CheckReport error: {arg}");
		}
	}

	private static void QuickSelectTopK(List<SampleData> list, int k, Comparison<SampleData> comparison)
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

	private static int Partition(List<SampleData> list, int left, int right, Comparison<SampleData> comparison)
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

	private static void Swap(List<SampleData> list, int i, int j)
	{
		SampleData sampleData = list[j];
		SampleData sampleData2 = list[i];
		SampleData sampleData3 = (list[i] = sampleData);
		sampleData3 = (list[j] = sampleData2);
	}

	public static string DumpTopTotalTimeProfilerTree(int topCount = 10)
	{
		if (!Enabled)
		{
			return string.Empty;
		}
		_dumpBuilder.Clear();
		foreach (SampleData topTotalTimeSample in GetTopTotalTimeSamples(topCount))
		{
			if (topTotalTimeSample.CallCount > 0 && (float)topTotalTimeSample.TotalTime > _reportMinTime)
			{
				DumpSampleRecursive(topTotalTimeSample, _dumpBuilder, 0);
			}
		}
		return _dumpBuilder.ToString();
	}

	public static string DumpTopAverageTimeProfilerTree(int topCount = 10)
	{
		if (!Enabled)
		{
			return string.Empty;
		}
		_dumpBuilder.Clear();
		foreach (SampleData topAverageTimeSample in GetTopAverageTimeSamples(topCount))
		{
			if (topAverageTimeSample.CallCount > 0 && (float)topAverageTimeSample.TotalTime > _reportMinTime)
			{
				DumpSampleRecursive(topAverageTimeSample, _dumpBuilder, 0);
			}
		}
		return _dumpBuilder.ToString();
	}

	private static void DumpSampleRecursive(SampleData sample, StringBuilder sb, int depth)
	{
		for (int i = 0; i < depth * 2; i++)
		{
			sb.Append("  ");
		}
		sb.Append(sample.Name).Append(": ").Append(sample.TotalTime)
			.Append("ms (Avg: ")
			.Append(sample.AverageTime.ToString("F2"))
			.Append("ms, Count: ")
			.Append(sample.CallCount)
			.AppendLine(")");
		foreach (SampleData value in sample.Children.Values)
		{
			if (value.CallCount > 0 && (float)value.TotalTime > _reportMinTime)
			{
				DumpSampleRecursive(value, sb, depth + 1);
			}
		}
	}

	public static void DumpToConsole(int topCount = 10, bool separateLog = false)
	{
		if (!Enabled)
		{
			return;
		}
		if (separateLog)
		{
			foreach (SampleData topTotalTimeSample in GetTopTotalTimeSamples(topCount))
			{
				if (topTotalTimeSample.CallCount > 0 && (float)topTotalTimeSample.TotalTime > _reportMinTime)
				{
					_dumpBuilder.Clear();
					DumpSampleRecursive(topTotalTimeSample, _dumpBuilder, 0);
					Log.Info($"ProfilerRuntime Total Time Sample:\n{_dumpBuilder}");
				}
			}
			{
				foreach (SampleData topAverageTimeSample in GetTopAverageTimeSamples(topCount))
				{
					if (topAverageTimeSample.CallCount > 0 && (float)topAverageTimeSample.TotalTime > _reportMinTime)
					{
						_dumpBuilder.Clear();
						DumpSampleRecursive(topAverageTimeSample, _dumpBuilder, 0);
						Log.Info($"ProfilerRuntime Average Time Sample:\n{_dumpBuilder}");
					}
				}
				return;
			}
		}
		string text = DumpTopTotalTimeProfilerTree(topCount);
		string text2 = DumpTopAverageTimeProfilerTree(topCount);
		Log.Info("ProfilerRuntime Total Time Top:\n" + text);
		Log.Info("ProfilerRuntime Average Time Top:\n" + text2);
	}
}
