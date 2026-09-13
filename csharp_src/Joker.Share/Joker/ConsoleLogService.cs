using System.Collections.Concurrent;
using System.Threading;

namespace Joker;

public class ConsoleLogService : Singleton<ConsoleLogService>, ILogService, IService
{
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
	}

	public void Destroy()
	{
	}

	public ILogger GetLogger()
	{
		int managedThreadId = Thread.CurrentThread.ManagedThreadId;
		if (_loggers.TryGetValue(managedThreadId, out var value))
		{
			return value;
		}
		value = new ConsoleLogger();
		_loggers.TryAdd(managedThreadId, value);
		return value;
	}

	public ILogger GetLogger(string name)
	{
		return GetLogger();
	}
}
