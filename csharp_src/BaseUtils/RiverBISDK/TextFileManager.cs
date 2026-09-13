using System;
using System.Threading;
using FibMatrix;

namespace RiverBISDK;

public class TextFileManager
{
	private readonly Timer _threadTimer;

	public TextFileManager()
	{
		_threadTimer = new Timer(TimeThread, null, BIConfig.fileHeartTime, BIConfig.fileHeartTime);
	}

	~TextFileManager()
	{
		_threadTimer.Dispose();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		if (_threadTimer != null)
		{
			_threadTimer.Dispose();
		}
	}

	public void TimeThread(object obj)
	{
		try
		{
			if (!string.IsNullOrEmpty(BIConfig.logFilePath))
			{
				TextFile.ReadTextFile();
			}
		}
		catch (Exception exception)
		{
			Logger.Error(exception);
		}
	}
}
