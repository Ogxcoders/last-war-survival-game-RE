using System;
using System.Diagnostics;
using UnityEngine;

namespace VEngine;

public static class Logger
{
	public static bool Loggable;

	public static void T(Action action, string name)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		action();
		stopwatch.Stop();
		UnityEngine.Debug.LogFormat("{0} with {1:f4}s.", name, stopwatch.ElapsedMilliseconds / 1024);
	}

	public static void E(string format, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(format, args);
	}

	public static void W(string format, params object[] args)
	{
		if (Loggable)
		{
			UnityEngine.Debug.LogWarningFormat(format, args);
		}
	}

	public static void I(string format, params object[] args)
	{
		if (Loggable)
		{
			UnityEngine.Debug.LogFormat(format, args);
		}
	}
}
