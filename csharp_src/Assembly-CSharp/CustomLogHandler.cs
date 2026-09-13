using System;
using System.Threading;
using FibMatrix;
using UnityEngine;

public class CustomLogHandler : ILogHandler
{
	private readonly ILogHandler _defaultLogHandler = Debug.unityLogger.logHandler;

	private static int _mainThreadId;

	public CustomLogHandler()
	{
		_mainThreadId = Thread.CurrentThread.ManagedThreadId;
	}

	public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
	{
		if (logType == LogType.Log)
		{
			if (CommonUtils.IsDebug() || Application.isEditor)
			{
				_defaultLogHandler.LogFormat(logType, context, format, args);
			}
			else
			{
				FibMatrix.Logger.Info(string.Format(format, args), null, LogTargetType.Runtime | LogTargetType.Network);
			}
		}
		else
		{
			_defaultLogHandler.LogFormat(logType, context, format, args);
		}
	}

	public void LogException(Exception exception, UnityEngine.Object context)
	{
		_defaultLogHandler.LogException(exception, context);
	}

	private static bool IsLogInMainThread()
	{
		return Thread.CurrentThread.ManagedThreadId == _mainThreadId;
	}

	private static string FormatAppendLuaStackTrace(string format)
	{
		string luaStackTrace = GetLuaStackTrace();
		if (string.IsNullOrEmpty(luaStackTrace))
		{
			return format;
		}
		luaStackTrace = luaStackTrace.Replace("{", "{{").Replace("}", "}}");
		return format + "\n" + luaStackTrace;
	}

	private static string GetLuaStackTrace()
	{
		if (!IsLogInMainThread())
		{
			return string.Empty;
		}
		if (GameEntry.Lua == null)
		{
			return string.Empty;
		}
		return GameEntry.Lua.GetStackInfo();
	}
}
