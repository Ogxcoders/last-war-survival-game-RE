using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Joker.Client;

public class UnityLogService : Singleton<UnityLogService>, ILogService, IService
{
	[ThreadStatic]
	private ILogger _logger;

	private readonly ConcurrentDictionary<int, ILogger> _loggers = new ConcurrentDictionary<int, ILogger>();

	public string UploadTag { get; set; }

	public void Awake()
	{
	}

	public void Startup()
	{
	}

	public void Shutdown()
	{
		_loggers.Clear();
	}

	public void Destroy()
	{
	}

	public ILogger GetLogger()
	{
		if (_logger != null)
		{
			return _logger;
		}
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		if (_loggers.TryGetValue(managedThreadId, out var value))
		{
			return value;
		}
		value = ((!Singleton<ThreadService>.Instance.IsMainThread()) ? ((ILogger)new UnityThreadLogger()) : ((ILogger)new UnityLogger()));
		_loggers.TryAdd(managedThreadId, value);
		return value;
	}

	public ILogger GetLogger(string name)
	{
		return GetLogger();
	}
}
