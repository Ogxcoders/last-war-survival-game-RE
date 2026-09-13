using System.Diagnostics;
using UnityEngine;

namespace GameFramework;

public class Log
{
	[Conditional("DEBUG")]
	public static void Debug(object message)
	{
		LogHelper(LogLevel.Debug, message);
	}

	[Conditional("DEBUG")]
	public static void LUA_Debug(string message)
	{
		LogHelper(LogLevel.Debug, message);
	}

	[Conditional("DEBUG")]
	public static void Debug(string message)
	{
		LogHelper(LogLevel.Debug, message);
	}

	[Conditional("DEBUG")]
	public static void Debug(string format, object arg0)
	{
		LogHelper(LogLevel.Debug, string.Format(format, arg0));
	}

	[Conditional("DEBUG")]
	public static void Debug(string format, object arg0, object arg1)
	{
		LogHelper(LogLevel.Debug, string.Format(format, arg0, arg1));
	}

	[Conditional("DEBUG")]
	public static void Debug(string format, object arg0, object arg1, object arg2)
	{
		LogHelper(LogLevel.Debug, string.Format(format, arg0, arg1, arg2));
	}

	[Conditional("DEBUG")]
	public static void Debug(string format, params object[] args)
	{
		LogHelper(LogLevel.Debug, string.Format(format, args));
	}

	[Conditional("DEBUG")]
	public static void PerfLog(string msg)
	{
		UnityEngine.Debug.Log($"-----PerfLog f:{Time.frameCount}, t:{Time.realtimeSinceStartup}, {msg}");
	}

	public static void Info(object message)
	{
		LogHelper(LogLevel.Info, message);
	}

	public static void Info(string message)
	{
		LogHelper(LogLevel.Info, message);
	}

	public static void Info(string format, object arg0)
	{
		LogHelper(LogLevel.Info, string.Format(format, arg0));
	}

	public static void Info(string format, object arg0, object arg1)
	{
		LogHelper(LogLevel.Info, string.Format(format, arg0, arg1));
	}

	public static void Info(string format, object arg0, object arg1, object arg2)
	{
		LogHelper(LogLevel.Info, string.Format(format, arg0, arg1, arg2));
	}

	public static void Info(string format, params object[] args)
	{
		LogHelper(LogLevel.Info, string.Format(format, args));
	}

	public static void Warning(object message)
	{
		LogHelper(LogLevel.Warning, message);
	}

	public static void Warning(string message)
	{
		LogHelper(LogLevel.Warning, message);
	}

	public static void Warning(string format, object arg0)
	{
		LogHelper(LogLevel.Warning, string.Format(format, arg0));
	}

	public static void Warning(string format, object arg0, object arg1)
	{
		LogHelper(LogLevel.Warning, string.Format(format, arg0, arg1));
	}

	public static void Warning(string format, object arg0, object arg1, object arg2)
	{
		LogHelper(LogLevel.Warning, string.Format(format, arg0, arg1, arg2));
	}

	public static void Warning(string format, params object[] args)
	{
		LogHelper(LogLevel.Warning, string.Format(format, args));
	}

	public static void Error(object message)
	{
		LogHelper(LogLevel.Error, message);
	}

	public static void Error(string message)
	{
		LogHelper(LogLevel.Error, message);
	}

	public static void LUA_Error(string message)
	{
		LogHelper(LogLevel.Error, message);
	}

	public static void Error(string format, object arg0)
	{
		LogHelper(LogLevel.Error, string.Format(format, arg0));
	}

	public static void Error(string format, object arg0, object arg1)
	{
		LogHelper(LogLevel.Error, string.Format(format, arg0, arg1));
	}

	public static void Error(string format, object arg0, object arg1, object arg2)
	{
		LogHelper(LogLevel.Error, string.Format(format, arg0, arg1, arg2));
	}

	public static void Error(string format, params object[] args)
	{
		LogHelper(LogLevel.Error, string.Format(format, args));
	}

	public static void Fatal(object message)
	{
		LogHelper(LogLevel.Fatal, message);
	}

	public static void Fatal(string message)
	{
		LogHelper(LogLevel.Fatal, message);
	}

	public static void Fatal(string format, object arg0)
	{
		LogHelper(LogLevel.Fatal, string.Format(format, arg0));
	}

	public static void Fatal(string format, object arg0, object arg1)
	{
		LogHelper(LogLevel.Fatal, string.Format(format, arg0, arg1));
	}

	public static void Fatal(string format, object arg0, object arg1, object arg2)
	{
		LogHelper(LogLevel.Fatal, string.Format(format, arg0, arg1, arg2));
	}

	public static void Fatal(string format, params object[] args)
	{
		LogHelper(LogLevel.Fatal, string.Format(format, args));
	}

	private static void LogHelper(LogLevel level, object message)
	{
		switch (level)
		{
		case LogLevel.Debug:
			UnityEngine.Debug.Log(message.ToString());
			break;
		case LogLevel.Info:
			UnityEngine.Debug.Log(message.ToString());
			break;
		case LogLevel.Warning:
			UnityEngine.Debug.LogWarning(message.ToString());
			break;
		case LogLevel.Error:
			UnityEngine.Debug.LogError(message.ToString());
			break;
		}
	}

	public static void LogHelper(LogLevel level, string message)
	{
		if (message != null)
		{
			switch (level)
			{
			case LogLevel.Debug:
				UnityEngine.Debug.Log(message);
				break;
			case LogLevel.Info:
				UnityEngine.Debug.Log(message);
				break;
			case LogLevel.Warning:
				UnityEngine.Debug.LogWarning(message);
				break;
			case LogLevel.Error:
				UnityEngine.Debug.LogError(message);
				break;
			}
		}
	}
}
