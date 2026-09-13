using System;
using System.Collections.Generic;
using GameFramework;

namespace RiverBISDK;

internal class BIManagerCore
{
	private static BIManagerCore _instance;

	private TextFileManager _textFileMgr;

	private NetManager _netManager;

	public static BIManagerCore instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new BIManagerCore();
			}
			return _instance;
		}
	}

	public NetManager GetNetManager()
	{
		if (_netManager == null)
		{
			return null;
		}
		return _netManager;
	}

	public void Dispose()
	{
		if (_textFileMgr != null)
		{
			_textFileMgr.Dispose();
			_textFileMgr = null;
		}
		if (_netManager != null)
		{
			_netManager.Dispose();
			_netManager = null;
		}
	}

	public void InitBI(IDictionary<string, object> initInfoDict, bool firstLaunch)
	{
		BIConfig.InitConfig(initInfoDict, firstLaunch);
		TextFile.InitTextFile();
		_netManager = new NetManager();
		_textFileMgr = new TextFileManager();
	}

	public void UpdateResVersion(string resVersion)
	{
		BIConfig.pfVersion = resVersion;
	}

	public void Reset()
	{
		BIConfig.beginGameTime = GameEntryProxy.Timer.GetServerTime();
		BIConfig.totalHideGameTime = 0L;
		BIConfig.lastHideGameTime = 0L;
		BIConfig.firstLaunch = 0;
	}

	public void OnApplicationPaused(bool paused)
	{
		if (paused)
		{
			BIConfig.lastHideGameTime = BIConfig.GetTimeStamp();
			return;
		}
		BIConfig.lastShowGameTime = BIConfig.GetTimeStamp();
		BIConfig.UpdateTotalHideTime();
		BIConfig.lastHideGameTime = 0L;
	}

	public void UpdateGameInfo(IDictionary<string, object> gameInfoDict)
	{
		BIConfig.UpdateGameInfo(gameInfoDict);
	}

	public void SendToBI(string eventName, IDictionary<string, object> eventInfoMap)
	{
		PackParams packParams = new PackParams(eventName, BIConfig.GetCommonData(), eventInfoMap);
		if (!BIConfig.isDebug)
		{
			GetNetManager().PushQueueHttps(packParams);
		}
		else
		{
			LogSendParams(packParams);
		}
	}

	public void SendToBI(string eventName)
	{
		PackParams packParams = new PackParams(eventName, BIConfig.GetCommonData());
		if (!BIConfig.isDebug)
		{
			LogSendParams(packParams);
			GetNetManager().PushQueueHttps(packParams);
		}
		else
		{
			LogSendParams(packParams);
		}
	}

	public void LogSendParams(PackParams param)
	{
		Log.Info($"Send to BI debug?{BIConfig.isDebug}:" + param.DataDictionaryToString());
	}

	public void Dlog(string info)
	{
		string message = "River Bi DLog:" + info;
		try
		{
			Log.Info(message);
		}
		catch (Exception ex)
		{
			Log.Error("River Bi log error:" + ex.Message);
		}
	}
}
