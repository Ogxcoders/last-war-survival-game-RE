using System;
using System.IO;
using System.Threading;
using Joker;

namespace MiniGame.Core.Server;

public class GameServerProcessLock : IDisposable
{
	public const float DefaultTimeoutSeconds = 30f;

	private readonly string _lockName;

	private FileStream _fileStream;

	private bool _disposed;

	public GameServerProcessLock(string lockName)
	{
		_lockName = lockName;
	}

	public bool TryAcquire(float timeoutSeconds = 30f)
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("GameServerProcessLock");
		}
		DateTime dateTime = DateTime.UtcNow.AddSeconds(timeoutSeconds);
		string lockFilePath = GetLockFilePath();
		while (DateTime.UtcNow < dateTime)
		{
			try
			{
				_fileStream = new FileStream(lockFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 1, FileOptions.DeleteOnClose);
				return true;
			}
			catch (IOException)
			{
				Thread.Sleep(100);
			}
			catch (UnauthorizedAccessException inner)
			{
				throw new UnauthorizedAccessException("无权限访问锁文件: " + lockFilePath, inner);
			}
		}
		return false;
	}

	public void Dispose()
	{
		if (_disposed)
		{
			return;
		}
		_disposed = true;
		try
		{
			_fileStream?.Dispose();
		}
		finally
		{
			_fileStream = null;
		}
	}

	private string GetLockFilePath()
	{
		if (string.IsNullOrWhiteSpace(_lockName))
		{
			throw new ArgumentException("Lock name cannot be null or empty.", "_lockName");
		}
		string text = string.Concat(_lockName.Split(Path.GetInvalidFileNameChars(), StringSplitOptions.RemoveEmptyEntries)).Trim(new char[1] { '_' });
		if (string.IsNullOrEmpty(text))
		{
			text = "default";
		}
		string[] obj = new string[3]
		{
			Path.GetTempPath(),
			Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			string.Empty
		};
		string text2 = null;
		Exception ex = null;
		string[] array = obj;
		foreach (string text3 in array)
		{
			try
			{
				Directory.CreateDirectory(text3);
				string path = Path.Combine(text3, ".write_test_" + Guid.NewGuid().ToString("N"));
				File.WriteAllText(path, "ok");
				File.Delete(path);
				text2 = text3;
			}
			catch (Exception ex2)
			{
				ex = ex2;
				continue;
			}
			break;
		}
		if (text2 == null)
		{
			throw new InvalidOperationException("无法在任何候选目录中创建锁文件。最后错误: " + ex?.Message, ex);
		}
		return Path.Combine(text2, text + ".lock");
	}

	public static T Run<T>(string lockName, Func<T> action, float timeoutSeconds = 30f)
	{
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		using GameServerProcessLock gameServerProcessLock = new GameServerProcessLock(lockName);
		if (!gameServerProcessLock.TryAcquire(timeoutSeconds))
		{
			throw new TimeoutException($"无法在 {timeoutSeconds} 秒内获取进程锁 '{lockName}'");
		}
		return action();
	}

	public static void Run(string lockName, Action action, float timeoutSeconds = 30f)
	{
		Run(lockName, delegate
		{
			action();
			return true;
		}, timeoutSeconds);
	}

	public static T SafeRunWithDefault<T>(string lockName, Func<T> action, T def = default(T), float timeoutSeconds = 30f)
	{
		if (action == null)
		{
			return def;
		}
		GameServerProcessLock gameServerProcessLock = null;
		try
		{
			gameServerProcessLock = new GameServerProcessLock(lockName);
			if (!gameServerProcessLock.TryAcquire(timeoutSeconds))
			{
				return def;
			}
			return action();
		}
		catch (Exception e)
		{
			Log.Exception(e);
			return def;
		}
		finally
		{
			gameServerProcessLock?.Dispose();
		}
	}

	public static void SafeRunWithDefault(string lockName, Action action, float timeoutSeconds = 30f)
	{
		SafeRunWithDefault(lockName, delegate
		{
			action();
			return true;
		}, def: false, timeoutSeconds);
	}
}
