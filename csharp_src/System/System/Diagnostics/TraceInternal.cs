using System.IO;

namespace System.Diagnostics;

internal static class TraceInternal
{
	private static volatile string appName = null;

	private static volatile TraceListenerCollection listeners;

	private static volatile bool autoFlush;

	private static volatile bool useGlobalLock;

	[ThreadStatic]
	private static int indentLevel;

	private static volatile int indentSize;

	internal static readonly object critSec = new object();

	public static TraceListenerCollection Listeners
	{
		get
		{
			InitializeSettings();
			if (listeners == null)
			{
				lock (critSec)
				{
					if (listeners == null)
					{
						listeners = new TraceListenerCollection();
						TraceListener traceListener = new DefaultTraceListener();
						traceListener.IndentLevel = indentLevel;
						traceListener.IndentSize = indentSize;
						listeners.Add(traceListener);
					}
				}
			}
			return listeners;
		}
	}

	internal static string AppName
	{
		get
		{
			if (appName == null)
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				if (commandLineArgs.Length != 0)
				{
					appName = Path.GetFileName(commandLineArgs[0]);
				}
			}
			return appName;
		}
	}

	public static bool AutoFlush
	{
		get
		{
			InitializeSettings();
			return autoFlush;
		}
		set
		{
			InitializeSettings();
			autoFlush = value;
		}
	}

	public static bool UseGlobalLock
	{
		get
		{
			InitializeSettings();
			return useGlobalLock;
		}
		set
		{
			InitializeSettings();
			useGlobalLock = value;
		}
	}

	public static int IndentLevel
	{
		get
		{
			return indentLevel;
		}
		set
		{
			lock (critSec)
			{
				if (value < 0)
				{
					value = 0;
				}
				indentLevel = value;
				if (listeners == null)
				{
					return;
				}
				foreach (TraceListener listener in Listeners)
				{
					listener.IndentLevel = indentLevel;
				}
			}
		}
	}

	public static int IndentSize
	{
		get
		{
			InitializeSettings();
			return indentSize;
		}
		set
		{
			InitializeSettings();
			SetIndentSize(value);
		}
	}

	private static void SetIndentSize(int value)
	{
		lock (critSec)
		{
			if (value < 0)
			{
				value = 0;
			}
			indentSize = value;
			if (listeners == null)
			{
				return;
			}
			foreach (TraceListener listener in Listeners)
			{
				listener.IndentSize = indentSize;
			}
		}
	}

	public static void Indent()
	{
		lock (critSec)
		{
			InitializeSettings();
			if (indentLevel < int.MaxValue)
			{
				indentLevel++;
			}
			foreach (TraceListener listener in Listeners)
			{
				listener.IndentLevel = indentLevel;
			}
		}
	}

	public static void Unindent()
	{
		lock (critSec)
		{
			InitializeSettings();
			if (indentLevel > 0)
			{
				indentLevel--;
			}
			foreach (TraceListener listener in Listeners)
			{
				listener.IndentLevel = indentLevel;
			}
		}
	}

	public static void Flush()
	{
		if (listeners == null)
		{
			return;
		}
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Flush();
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Flush();
				}
			}
			else
			{
				listener2.Flush();
			}
		}
	}

	public static void Close()
	{
		if (listeners == null)
		{
			return;
		}
		lock (critSec)
		{
			foreach (TraceListener listener in Listeners)
			{
				listener.Close();
			}
		}
	}

	public static void Assert(bool condition)
	{
		if (!condition)
		{
			Fail(string.Empty);
		}
	}

	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			Fail(message);
		}
	}

	public static void Assert(bool condition, string message, string detailMessage)
	{
		if (!condition)
		{
			Fail(message, detailMessage);
		}
	}

	public static void Fail(string message)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Fail(message);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Fail(message);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.Fail(message);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void Fail(string message, string detailMessage)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Fail(message, detailMessage);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Fail(message, detailMessage);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.Fail(message, detailMessage);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	private static void InitializeSettings()
	{
	}

	internal static void Refresh()
	{
		lock (critSec)
		{
			listeners = null;
		}
		InitializeSettings();
	}

	public static void TraceEvent(TraceEventType eventType, int id, string format, params object[] args)
	{
		TraceEventCache eventCache = new TraceEventCache();
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				if (args == null)
				{
					foreach (TraceListener listener in Listeners)
					{
						listener.TraceEvent(eventCache, AppName, eventType, id, format);
						if (AutoFlush)
						{
							listener.Flush();
						}
					}
					return;
				}
				foreach (TraceListener listener2 in Listeners)
				{
					listener2.TraceEvent(eventCache, AppName, eventType, id, format, args);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
				return;
			}
		}
		if (args == null)
		{
			foreach (TraceListener listener3 in Listeners)
			{
				if (!listener3.IsThreadSafe)
				{
					lock (listener3)
					{
						listener3.TraceEvent(eventCache, AppName, eventType, id, format);
						if (AutoFlush)
						{
							listener3.Flush();
						}
					}
				}
				else
				{
					listener3.TraceEvent(eventCache, AppName, eventType, id, format);
					if (AutoFlush)
					{
						listener3.Flush();
					}
				}
			}
			return;
		}
		foreach (TraceListener listener4 in Listeners)
		{
			if (!listener4.IsThreadSafe)
			{
				lock (listener4)
				{
					listener4.TraceEvent(eventCache, AppName, eventType, id, format, args);
					if (AutoFlush)
					{
						listener4.Flush();
					}
				}
			}
			else
			{
				listener4.TraceEvent(eventCache, AppName, eventType, id, format, args);
				if (AutoFlush)
				{
					listener4.Flush();
				}
			}
		}
	}

	public static void Write(string message)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Write(message);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Write(message);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.Write(message);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void Write(object value)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Write(value);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Write(value);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.Write(value);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void Write(string message, string category)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Write(message, category);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Write(message, category);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.Write(message, category);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void Write(object value, string category)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.Write(value, category);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.Write(value, category);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.Write(value, category);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void WriteLine(string message)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.WriteLine(message);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.WriteLine(message);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.WriteLine(message);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void WriteLine(object value)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.WriteLine(value);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.WriteLine(value);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.WriteLine(value);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void WriteLine(string message, string category)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.WriteLine(message, category);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.WriteLine(message, category);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.WriteLine(message, category);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void WriteLine(object value, string category)
	{
		if (UseGlobalLock)
		{
			lock (critSec)
			{
				foreach (TraceListener listener in Listeners)
				{
					listener.WriteLine(value, category);
					if (AutoFlush)
					{
						listener.Flush();
					}
				}
				return;
			}
		}
		foreach (TraceListener listener2 in Listeners)
		{
			if (!listener2.IsThreadSafe)
			{
				lock (listener2)
				{
					listener2.WriteLine(value, category);
					if (AutoFlush)
					{
						listener2.Flush();
					}
				}
			}
			else
			{
				listener2.WriteLine(value, category);
				if (AutoFlush)
				{
					listener2.Flush();
				}
			}
		}
	}

	public static void WriteIf(bool condition, string message)
	{
		if (condition)
		{
			Write(message);
		}
	}

	public static void WriteIf(bool condition, object value)
	{
		if (condition)
		{
			Write(value);
		}
	}

	public static void WriteIf(bool condition, string message, string category)
	{
		if (condition)
		{
			Write(message, category);
		}
	}

	public static void WriteIf(bool condition, object value, string category)
	{
		if (condition)
		{
			Write(value, category);
		}
	}

	public static void WriteLineIf(bool condition, string message)
	{
		if (condition)
		{
			WriteLine(message);
		}
	}

	public static void WriteLineIf(bool condition, object value)
	{
		if (condition)
		{
			WriteLine(value);
		}
	}

	public static void WriteLineIf(bool condition, string message, string category)
	{
		if (condition)
		{
			WriteLine(message, category);
		}
	}

	public static void WriteLineIf(bool condition, object value, string category)
	{
		if (condition)
		{
			WriteLine(value, category);
		}
	}
}
