using System;
using System.Diagnostics;
using UnityEngine;

public class StopwatchProfiler : IDisposable
{
	private Stopwatch _stopwatch;

	private string _label = string.Empty;

	public static StopwatchProfiler StartNew(string label)
	{
		return new StopwatchProfiler
		{
			_stopwatch = Stopwatch.StartNew(),
			_label = label
		};
	}

	private StopwatchProfiler()
	{
	}

	public void Dispose()
	{
		_stopwatch.Stop();
		DateTime now = DateTime.Now;
		_ = $"{now.Hour:00}:{now.Minute:00}:{now.Second:00}.{now.Millisecond:000}";
	}

	[Conditional("DEBUG")]
	public static void LogDebugTimestamp(string msg)
	{
		DateTime now = DateTime.Now;
		_ = $"{now.Hour:00}:{now.Minute:00}:{now.Second:00}.{now.Millisecond:000} frame - {Time.frameCount}";
	}
}
