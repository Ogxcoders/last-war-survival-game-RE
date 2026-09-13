using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp(GenFlag.No)]
public class GMSwitch
{
	public class Logger
	{
		[LuaCallCSharp(GenFlag.No)]
		public class Log
		{
			public string condition;

			public string truncatedCondition;

			public string stacktrace;

			public long time;

			public int logType;
		}

		private bool registered;

		private ConcurrentQueue<Log> logQueueThread;

		private Queue<Log> logQueueMain;

		private const int CAPACITY = 99;

		private volatile int currentLevel;

		private volatile bool isDirty;

		private int warningCount;

		private int errorCount;

		private float timer;

		public Queue<Log> LogQueueMain => logQueueMain;

		public Logger()
		{
			logQueueThread = new ConcurrentQueue<Log>();
			logQueueMain = new Queue<Log>();
		}

		public void Dispose()
		{
			Application.logMessageReceivedThreaded -= OnLogMessageReceivedThread;
			registered = false;
		}

		public void Clear()
		{
			warningCount = 0;
			errorCount = 0;
			logQueueMain.Clear();
			logQueueThread = new ConcurrentQueue<Log>();
		}

		private Log Get()
		{
			return new Log();
		}

		private string TruncateWithEllipsis(string input, int maxLength = 60)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			if (input.Length > maxLength)
			{
				return input.Substring(0, maxLength) + "...";
			}
			return input;
		}

		private void OnLogMessageReceivedThread(string condition, string stacktrace, LogType type)
		{
			switch (type)
			{
			case LogType.Log:
				return;
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				if ((currentLevel & 2) == 0)
				{
					return;
				}
				break;
			case LogType.Warning:
				if ((currentLevel & 1) == 0)
				{
					return;
				}
				break;
			}
			Log log = Get();
			log.condition = condition;
			log.truncatedCondition = TruncateWithEllipsis(condition);
			log.stacktrace = stacktrace;
			log.logType = (int)type;
			log.time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
			logQueueThread.Enqueue(log);
			isDirty = true;
		}

		public void SetLogLevel(int level)
		{
			if (level != currentLevel)
			{
				currentLevel = level;
				if (currentLevel == 0 && registered)
				{
					Application.logMessageReceivedThreaded -= OnLogMessageReceivedThread;
					registered = false;
				}
				else if (currentLevel > 0 && !registered)
				{
					Application.logMessageReceivedThreaded += OnLogMessageReceivedThread;
					registered = true;
				}
			}
		}

		public void MainThreadUpdate(float deltaTime)
		{
			if (currentLevel <= 0)
			{
				return;
			}
			timer += deltaTime;
			if (timer < 0.1f)
			{
				return;
			}
			timer = 0f;
			if (!isDirty)
			{
				return;
			}
			isDirty = false;
			while (logQueueThread.Count > 0)
			{
				if (logQueueThread.TryDequeue(out var result))
				{
					if (result.logType == 2)
					{
						warningCount++;
					}
					else
					{
						errorCount++;
					}
					if (logQueueMain.Count >= 99)
					{
						logQueueMain.Dequeue();
					}
					logQueueMain.Enqueue(result);
				}
			}
			GameEntry.Lua?.Call("GMUtils.UpdateLogCount", warningCount, errorCount);
		}
	}

	private static Logger debugLogger;

	private static Dictionary<string, bool> boolDic = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, int> intDic = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

	private static Dictionary<string, object> objectDic = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

	private static bool isGM = false;

	public static bool FocusMyClick => false;

	public static Queue<Logger.Log> DebugLogQueue => debugLogger?.LogQueueMain;

	public static bool IsGM
	{
		get
		{
			return isGM;
		}
		set
		{
			if (!CommonUtils.IsDebug())
			{
				isGM = false;
				return;
			}
			Init();
			isGM = value;
			DebugUtils.isGM = isGM;
		}
	}

	public static bool DebugClickLogWarning => GetBool("DebugClickLogWarning");

	public static void Init()
	{
	}

	public static void Dispose()
	{
		boolDic.Clear();
		intDic.Clear();
		objectDic.Clear();
		debugLogger?.Dispose();
		debugLogger = null;
	}

	public static void Reload()
	{
		debugLogger?.Dispose();
		debugLogger = null;
	}

	public static void Update(float deltaTime)
	{
		debugLogger?.MainThreadUpdate(deltaTime);
	}

	public static void ClearDebugLog()
	{
		debugLogger?.Clear();
	}

	public static bool GetBool(string key, bool defaultVal = false)
	{
		if (!IsGM)
		{
			return defaultVal;
		}
		if (boolDic.TryGetValue(key, out var value))
		{
			return value;
		}
		XLuaManager lua = GameEntry.Lua;
		if (lua == null)
		{
			return defaultVal;
		}
		bool flag = lua.CallWithReturn<bool, string, bool>("GMUtils.GetBool", key, defaultVal);
		boolDic[key] = flag;
		return flag;
	}

	public static void LuaSetBool(string key, bool value)
	{
		if (!IsGM)
		{
			return;
		}
		boolDic[key] = value;
		if (!(key == "DebugLocalLogEnable"))
		{
			if (key == "DebugSendMsg")
			{
				DebugUtils.logClinetSendMsgDetail = value;
			}
		}
		else
		{
			RefreshLogger();
		}
	}

	public static int GetInt(string key, int defaultVal = 0)
	{
		if (!IsGM)
		{
			return defaultVal;
		}
		if (intDic.TryGetValue(key, out var value))
		{
			return value;
		}
		XLuaManager lua = GameEntry.Lua;
		if (lua == null)
		{
			return defaultVal;
		}
		int num = lua.CallWithReturn<int, string, int>("GMUtils.GetInt", key, defaultVal);
		intDic[key] = num;
		return num;
	}

	public static void LuaSetInt(string key, int value)
	{
		if (IsGM)
		{
			intDic[key] = value;
			if (key == "DebugLocalLogLevel")
			{
				debugLogger?.SetLogLevel(value);
			}
		}
	}

	public static void LuaSetObject(string key, object value)
	{
		if (IsGM)
		{
			objectDic[key] = value;
		}
	}

	public static object GetObject(string key)
	{
		if (!IsGM)
		{
			return null;
		}
		if (objectDic.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}

	private static void RefreshLogger()
	{
		if (GetBool("DebugLocalLogEnable"))
		{
			debugLogger = debugLogger ?? new Logger();
		}
		else if (debugLogger != null)
		{
			debugLogger.Dispose();
			debugLogger = null;
		}
	}
}
