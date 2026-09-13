using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FibMatrix;

public class Logger
{
	private static List<ILoggerTarget> _targets = new List<ILoggerTarget>();

	private static int _targetCount = 0;

	private static LogLevel _level = LogLevel.Debug;

	private static IRemoteLoggerTarget _remoteLoggerTarget = null;

	public static LogLevel level
	{
		get
		{
			return _level;
		}
		set
		{
			_level = value;
		}
	}

	public static bool UseUnityWebRequestRemoteLogger => true;

	public static IRemoteLoggerTarget CurrentRemoteLoggerTarget
	{
		get
		{
			if (_remoteLoggerTarget == null)
			{
				throw new NullReferenceException("Get remote logger target when it's null");
			}
			return _remoteLoggerTarget;
		}
	}

	public static void AppendTarget(ILoggerTarget target)
	{
		_targets.Add(target);
		_targetCount = _targets.Count;
	}

	public static void RemoveTarget(ILoggerTarget target)
	{
		_targets.Remove(target);
		_targetCount = _targets.Count;
	}

	public static void DisposeTargets()
	{
		foreach (ILoggerTarget target in _targets)
		{
			if (target is IRemoteLoggerTarget remoteLoggerTarget)
			{
				remoteLoggerTarget.Dispose();
			}
		}
	}

	public static void Update()
	{
		if (_remoteLoggerTarget != null)
		{
			_remoteLoggerTarget.Update();
		}
	}

	[Conditional("DEBUG")]
	public static void Debug(string message)
	{
		DoDebug(message, null);
	}

	[Conditional("DEBUG")]
	public static void DebugFormat(string messageFormat, params object[] args)
	{
		DoDebug(string.Format(messageFormat, args), null);
	}

	[Conditional("DEBUG")]
	public static void Debug(string message, object context)
	{
		DoDebug(message, context);
	}

	[Conditional("DEBUG")]
	public static void DebugFormat(object context, string messageFormat, params object[] args)
	{
		DoDebug(string.Format(messageFormat, args), context);
	}

	private static void DoDebug(string message, object context)
	{
		if (_level > LogLevel.Debug)
		{
			return;
		}
		for (int i = 0; i < _targetCount; i++)
		{
			ILoggerTarget loggerTarget = _targets[i];
			if ((loggerTarget.targetType & LogTargetType.Runtime) != LogTargetType.None)
			{
				loggerTarget.Debug(message, context, LogImportance.Normal);
			}
		}
	}

	public static void Info(string message)
	{
		Info(message, (object)null, LogTargetType.Runtime, LogImportance.Normal);
	}

	public static void Info(string message, object context)
	{
		Info(message, context, LogTargetType.Runtime, LogImportance.Normal);
	}

	public static void Info(string message, Dictionary<string, string> context, LogTargetType targetType, LogImportance importance = LogImportance.Normal)
	{
		Info(message, (object)context, targetType, importance);
	}

	private static void Info(string message, object context, LogTargetType targetType, LogImportance importance)
	{
		if (_level > LogLevel.Info)
		{
			return;
		}
		for (int i = 0; i < _targetCount; i++)
		{
			ILoggerTarget loggerTarget = _targets[i];
			if ((loggerTarget.targetType & targetType) != LogTargetType.None)
			{
				loggerTarget.Info(message, context, importance);
			}
		}
	}

	public static void Warning(string message)
	{
		Warning(message, (object)null, LogTargetType.Runtime | LogTargetType.Network);
	}

	public static void Warning(string message, object context)
	{
		Warning(message, context, LogTargetType.Runtime | LogTargetType.Network);
	}

	public static void Warning(string message, Dictionary<string, string> context, LogTargetType targetType = LogTargetType.Runtime | LogTargetType.Network)
	{
		Warning(message, (object)context, targetType);
	}

	private static void Warning(string message, object context, LogTargetType targetType)
	{
		if (_level > LogLevel.Warning)
		{
			return;
		}
		for (int i = 0; i < _targetCount; i++)
		{
			ILoggerTarget loggerTarget = _targets[i];
			if ((loggerTarget.targetType & targetType) != LogTargetType.None)
			{
				loggerTarget.Warning(message, context, LogImportance.High);
			}
		}
	}

	public static void Error(string message)
	{
		Error(message, (object)null, LogTargetType.Runtime | LogTargetType.Network);
	}

	public static void Error(string message, object context)
	{
		Error(message, context, LogTargetType.Runtime | LogTargetType.Network);
	}

	public static void Error(string message, Dictionary<string, string> context, LogTargetType targetType = LogTargetType.Runtime | LogTargetType.Network)
	{
		Error(message, (object)context, targetType);
	}

	private static void Error(string message, object context, LogTargetType targetType)
	{
		if (_level > LogLevel.Error)
		{
			return;
		}
		for (int i = 0; i < _targetCount; i++)
		{
			ILoggerTarget loggerTarget = _targets[i];
			if ((loggerTarget.targetType & targetType) != LogTargetType.None)
			{
				loggerTarget.Error(message, Environment.StackTrace, context, LogImportance.High);
			}
		}
	}

	public static void Error(Exception exception)
	{
		Error(exception, (object)null, LogTargetType.Runtime | LogTargetType.Network);
	}

	public static void Error(Exception exception, object context)
	{
		Error(exception, context, LogTargetType.Runtime | LogTargetType.Network);
	}

	public static void Error(Exception exception, Dictionary<string, string> context, LogTargetType targetType = LogTargetType.Runtime | LogTargetType.Network)
	{
		Error(exception, (object)context, targetType);
	}

	private static void Error(Exception exception, object context, LogTargetType targetType)
	{
		if (_level > LogLevel.Error)
		{
			return;
		}
		for (int i = 0; i < _targetCount; i++)
		{
			ILoggerTarget loggerTarget = _targets[i];
			if ((loggerTarget.targetType & targetType) != LogTargetType.None)
			{
				loggerTarget.Error(exception.Message, exception.StackTrace, context, LogImportance.High);
			}
		}
	}

	public static void ConfigureRemoteLoggerTarget(string url, string userId, string serverId, ClientInfo clientInfo)
	{
		if (UseUnityWebRequestRemoteLogger)
		{
			RemoteLoggerTarget_UnityWebRequest.Configure(url, userId, serverId, clientInfo);
			_remoteLoggerTarget = RemoteLoggerTarget_UnityWebRequest.instance;
		}
		else
		{
			RemoteLoggerTarget.Configure(url, userId, serverId, clientInfo);
			_remoteLoggerTarget = RemoteLoggerTarget.instance;
		}
	}

	public static void AppendRemoteTarget()
	{
		if (_remoteLoggerTarget != null)
		{
			_targets.Add(_remoteLoggerTarget);
			_targetCount = _targets.Count;
			return;
		}
		throw new NullReferenceException("Append remote logger target when it's null");
	}

	public static void SetRemoteLoggerReportLevel(RemoteLoggerTarget.ReportLevel reportLevel)
	{
		RemoteLoggerTarget.reportLevel = reportLevel;
		RemoteLoggerTarget_UnityWebRequest.reportLevel = reportLevel;
	}
}
