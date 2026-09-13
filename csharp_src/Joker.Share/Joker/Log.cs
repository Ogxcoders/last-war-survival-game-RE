using System;
using System.Runtime.CompilerServices;

namespace Joker;

public static class Log
{
	[ThreadStatic]
	private static ILogger _msLogger;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ILogger GetLogger(string name)
	{
		return App.Get<ILogService>().GetLogger(name);
	}

	public static ILogger GetLogger<T>()
	{
		return GetLogger(typeof(T).Name);
	}

	public static ILogger GetLogger(Type type)
	{
		return GetLogger(type.Name);
	}

	public static void Debug(string message)
	{
		_GetLog().Debug(message);
	}

	public static void Debug(string format, params object[] args)
	{
		_GetLog().Debug(_Format(format, args));
	}

	public static void Info(string message)
	{
		_GetLog().Info(message);
	}

	public static void Info(string format, params object[] args)
	{
		_GetLog().Info(_Format(format, args));
	}

	public static void Warning(string message)
	{
		_GetLog().Warning(message);
	}

	public static void Warning(string format, params object[] args)
	{
		_GetLog().Warning(_Format(format, args));
	}

	public static void Error(string message)
	{
		_GetLog().Error(message);
	}

	public static void Error(string format, params object[] args)
	{
		_GetLog().Error(_Format(format, args));
	}

	public static void Exception(Exception e)
	{
		_GetLog().Exception(e);
	}

	public static void Exception(string format, params object[] args)
	{
		_GetLog().Exception(new Exception(_Format(format, args)));
	}

	public static void Assert(bool condition, object message = null)
	{
		_GetLog().Assert(condition, message);
	}

	public static void Assert(bool condition, string format, params object[] args)
	{
		_GetLog().Assert(condition, _Format(format, args));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ILogger _GetLog()
	{
		return _msLogger ?? (_msLogger = App.Get<ILogService>()?.GetLogger());
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string _Format(string format, params object[] args)
	{
		return string.Format(format, args);
	}
}
