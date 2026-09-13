using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BestHTTP;
using GameFramework;
using Newtonsoft.Json;
using UnityEngine;

namespace FibMatrix;

public class RemoteLoggerTarget : IRemoteLoggerTarget, ILoggerTarget
{
	public enum ReportLevel
	{
		Info = 1,
		Warning,
		Error,
		Disabled
	}

	private static RemoteLoggerTarget _instance;

	public static ReportLevel reportLevel = ReportLevel.Info;

	private ClientInfo _clientInfo;

	private string _uid;

	private string _serverId;

	private ConcurrentQueue<RemoteLogInfo> _sampleQueue;

	private Thread _workThread;

	private volatile bool _closed;

	private AutoResetEvent _consumeEvt = new AutoResetEvent(initialState: false);

	private Uri _url;

	private Dictionary<string, Dictionary<string, long>> _stackMsgMap = new Dictionary<string, Dictionary<string, long>>();

	private long _lastGCTime;

	private const long COMBINE_TIME = 30000000L;

	private const long GC_TIME = 15000000L;

	private object _checkDuplicateLock = new object();

	private List<RemoteLogInfo> _finalLogs = new List<RemoteLogInfo>();

	private Dictionary<string, object> _finalLogContainer = new Dictionary<string, object> { { "__logs__", null } };

	private JsonSerializerSettings _settings = new JsonSerializerSettings();

	private static Dictionary<string, string> _cacheArgs = new Dictionary<string, string>
	{
		{ "resp", null },
		{ "stack", null }
	};

	public static RemoteLoggerTarget instance => _instance;

	public LogTargetType targetType => LogTargetType.Network;

	public static void Configure(string url, string userId, string serverId, ClientInfo clientInfo)
	{
		_instance = new RemoteLoggerTarget();
		_instance.Initialize(url, clientInfo);
		_instance.UpdateUserInfo(userId, serverId);
	}

	public void UpdateUserInfo(string userId, string serverId)
	{
		_uid = userId;
		_serverId = serverId;
		Log.Info("BIConfig::UpdateUserInfo " + userId + "," + serverId);
	}

	public void UpdateResVersion(string resVersion)
	{
		_clientInfo.packVer = resVersion;
	}

	public void UpdateCountry(string country)
	{
		_clientInfo.country = country;
	}

	private void Initialize(string url, ClientInfo clientInfo)
	{
		_url = new Uri(url);
		_clientInfo = clientInfo;
		_sampleQueue = new ConcurrentQueue<RemoteLogInfo>();
		_settings.NullValueHandling = NullValueHandling.Ignore;
		_workThread = new Thread(WorkThread);
		_workThread.IsBackground = true;
		_workThread.Start();
	}

	public void Dispose()
	{
		GC.SuppressFinalize(this);
		_closed = true;
		_consumeEvt.Set();
		if (_workThread.ThreadState == ThreadState.Background)
		{
			try
			{
				_workThread.Join();
			}
			catch (Exception exception)
			{
				Logger.Error(exception);
			}
		}
	}

	public void WorkThread()
	{
		while (WorkThreadQueueList())
		{
		}
	}

	private bool WorkThreadQueueList()
	{
		if (_finalLogs.Count > 0)
		{
			_finalLogs.Clear();
		}
		RemoteLogInfo result;
		while (!_closed && _sampleQueue.TryDequeue(out result))
		{
			_finalLogs.Add(result);
		}
		if (!_closed && _finalLogs.Count > 0)
		{
			int i = 0;
			for (int count = _finalLogs.Count; i < count; i++)
			{
				_finalLogs[i].SyncAllParams(_clientInfo);
			}
			_finalLogContainer["__logs__"] = _finalLogs;
			try
			{
				string s = JsonConvert.SerializeObject(_finalLogContainer, _settings);
				HTTPRequest hTTPRequest = new HTTPRequest(_url, HTTPMethods.Post);
				hTTPRequest.RawData = Encoding.UTF8.GetBytes(s);
				hTTPRequest.SetHeader("x-log-apiversion", "0.6.0");
				hTTPRequest.SetHeader("x-log-bodyrawsize", hTTPRequest.RawData.Length.ToString());
				hTTPRequest.SetHeader("Content-Type", "application/json; charset=UTF-8");
				hTTPRequest.Send();
			}
			catch (Exception)
			{
			}
			int j = 0;
			for (int count2 = _finalLogs.Count; j < count2; j++)
			{
				RemoteLogInfo.Recycle(_finalLogs[j]);
			}
			_finalLogs.Clear();
		}
		if (_closed)
		{
			return false;
		}
		_consumeEvt.WaitOne(5000);
		return true;
	}

	private void PushQueueHttps(LogLevel level, Dictionary<string, string> args, bool batch)
	{
		RemoteLogInfo remoteLogInfo = RemoteLogInfo.Allocate(level, _uid, _serverId);
		if (args != null)
		{
			foreach (KeyValuePair<string, string> arg in args)
			{
				if (arg.Value != null)
				{
					remoteLogInfo.Add(arg.Key, arg.Value);
				}
			}
		}
		_sampleQueue.Enqueue(remoteLogInfo);
		if (!batch)
		{
			_consumeEvt.Set();
		}
	}

	public void Debug(string message, object context, LogImportance importance)
	{
	}

	public void Info(string message, object context, LogImportance importance)
	{
		if (reportLevel <= ReportLevel.Info && !CheckDuplicateMsg(message, "EMPTY"))
		{
			Dictionary<string, string> dictionary = (context as Dictionary<string, string>) ?? _cacheArgs;
			dictionary["resp"] = message ?? "EMPTY";
			dictionary["stack"] = "EMPTY";
			PushQueueHttps(LogLevel.Info, dictionary, importance == LogImportance.Normal);
		}
	}

	public void Warning(string message, object context, LogImportance importance)
	{
		if (reportLevel <= ReportLevel.Warning && !CheckDuplicateMsg(message, "EMPTY"))
		{
			Dictionary<string, string> dictionary = (context as Dictionary<string, string>) ?? _cacheArgs;
			dictionary["resp"] = message ?? "EMPTY";
			dictionary["stack"] = "EMPTY";
			PushQueueHttps(LogLevel.Warning, dictionary, importance == LogImportance.Normal);
		}
	}

	public void Error(string message, string stackTrace, object context, LogImportance importance)
	{
		if (reportLevel <= ReportLevel.Error && !CheckDuplicateMsg(message, stackTrace))
		{
			Dictionary<string, string> dictionary = (context as Dictionary<string, string>) ?? _cacheArgs;
			dictionary["resp"] = message ?? "EMPTY";
			dictionary["stack"] = stackTrace ?? "EMPTY";
			PushQueueHttps(LogLevel.Error, dictionary, batch: false);
		}
	}

	private bool CheckDuplicateMsg(string message, string stackTrace)
	{
		lock (_checkDuplicateLock)
		{
			long ticks = DateTime.Now.Ticks;
			if (!string.IsNullOrEmpty(stackTrace) && !string.IsNullOrEmpty(message))
			{
				if (_stackMsgMap.TryGetValue(stackTrace, out var value))
				{
					if (value.TryGetValue(message, out var value2) && ticks - value2 <= 30000000)
					{
						return true;
					}
					value[message] = ticks;
				}
				else
				{
					value = new Dictionary<string, long> { { message, ticks } };
					_stackMsgMap.Add(stackTrace, value);
				}
			}
			if (ticks - _lastGCTime >= 15000000)
			{
				_lastGCTime = ticks;
				string text = null;
				string text2 = null;
				foreach (KeyValuePair<string, Dictionary<string, long>> item in _stackMsgMap)
				{
					string key = item.Key;
					Dictionary<string, long> value3 = item.Value;
					if (value3.Count == 0)
					{
						text = key;
						break;
					}
					foreach (KeyValuePair<string, long> item2 in value3)
					{
						if (ticks - item2.Value >= 30000000)
						{
							text2 = item2.Key;
							break;
						}
					}
					if (!string.IsNullOrEmpty(text2))
					{
						value3.Remove(text2);
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					_stackMsgMap.Remove(text);
				}
			}
			return false;
		}
	}

	private void _LogHandler(string logString, string stackTrace, LogType type)
	{
		switch (type)
		{
		case LogType.Log:
			Logger.Info(logString, stackTrace);
			break;
		case LogType.Warning:
			Logger.Warning(logString, stackTrace);
			break;
		case LogType.Error:
		case LogType.Assert:
		case LogType.Exception:
			Logger.Error(logString, stackTrace);
			break;
		}
	}

	private void _UncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
	{
		if (args == null || args.ExceptionObject == null)
		{
			return;
		}
		try
		{
			if (args.ExceptionObject.GetType() != typeof(Exception))
			{
				return;
			}
		}
		catch
		{
			return;
		}
		Exception ex = (Exception)args.ExceptionObject;
		Logger.Error(ex.Message, ex.StackTrace);
	}

	public void Update()
	{
	}
}
