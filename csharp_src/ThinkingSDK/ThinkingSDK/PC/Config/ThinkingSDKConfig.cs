using System;
using System.Collections;
using System.Collections.Generic;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Request;
using ThinkingSDK.PC.Utils;
using UnityEngine;

namespace ThinkingSDK.PC.Config;

public class ThinkingSDKConfig
{
	private string mToken;

	private string mServerUrl;

	private string mNormalUrl;

	private string mDebugUrl;

	private string mConfigUrl;

	private string mInstanceName;

	private Mode mMode;

	private TimeZoneInfo mTimeZone;

	public int mUploadInterval = 30;

	public int mUploadSize = 30;

	private List<string> mDisableEvents = new List<string>();

	private static Dictionary<string, ThinkingSDKConfig> sInstances = new Dictionary<string, ThinkingSDKConfig>();

	private ThinkingSDKConfig(string token, string serverUrl, string instanceName)
	{
		serverUrl = VerifyUrl(serverUrl);
		mServerUrl = serverUrl;
		mNormalUrl = serverUrl + "/sync";
		mDebugUrl = serverUrl + "/data_debug";
		mConfigUrl = serverUrl + "/config";
		mToken = token;
		mInstanceName = instanceName;
		try
		{
			mTimeZone = TimeZoneInfo.Local;
		}
		catch (Exception)
		{
		}
	}

	private string VerifyUrl(string serverUrl)
	{
		Uri uri = new Uri(serverUrl);
		serverUrl = uri.Scheme + "://" + uri.Host + ":" + uri.Port;
		return serverUrl;
	}

	public void SetMode(Mode mode)
	{
		mMode = mode;
	}

	public Mode GetMode()
	{
		return mMode;
	}

	public string DebugURL()
	{
		return mDebugUrl;
	}

	public string NormalURL()
	{
		return mNormalUrl;
	}

	public string ConfigURL()
	{
		return mConfigUrl;
	}

	public string Server()
	{
		return mServerUrl;
	}

	public string InstanceName()
	{
		return mInstanceName;
	}

	public static ThinkingSDKConfig GetInstance(string token, string server, string instanceName)
	{
		ThinkingSDKConfig thinkingSDKConfig = null;
		if (!string.IsNullOrEmpty(instanceName))
		{
			if (sInstances.ContainsKey(instanceName))
			{
				thinkingSDKConfig = sInstances[instanceName];
			}
			else
			{
				thinkingSDKConfig = new ThinkingSDKConfig(token, server, instanceName);
				sInstances.Add(instanceName, thinkingSDKConfig);
			}
		}
		else if (sInstances.ContainsKey(token))
		{
			thinkingSDKConfig = sInstances[token];
		}
		else
		{
			thinkingSDKConfig = new ThinkingSDKConfig(token, server, null);
			sInstances.Add(token, thinkingSDKConfig);
		}
		return thinkingSDKConfig;
	}

	public void SetTimeZone(TimeZoneInfo timeZoneInfo)
	{
		mTimeZone = timeZoneInfo;
	}

	public TimeZoneInfo TimeZone()
	{
		return mTimeZone;
	}

	public List<string> DisableEvents()
	{
		return mDisableEvents;
	}

	public bool IsDisabledEvent(string eventName)
	{
		if (mDisableEvents == null)
		{
			return false;
		}
		return mDisableEvents.Contains(eventName);
	}

	public void UpdateConfig(MonoBehaviour mono, ResponseHandle callback = null)
	{
		new Dictionary<string, object>();
		ResponseHandle responseHandle = delegate(Dictionary<string, object> result)
		{
			try
			{
				int num = int.Parse(result["code"].ToString());
				if (result != null && num == 0)
				{
					foreach (KeyValuePair<string, object> item in (Dictionary<string, object>)result["data"])
					{
						if (item.Key == "sync_interval")
						{
							mUploadInterval = int.Parse(item.Value.ToString());
						}
						else if (item.Key == "sync_batch_size")
						{
							mUploadSize = int.Parse(item.Value.ToString());
						}
						else if (item.Key == "disable_event_list")
						{
							foreach (object item2 in (List<object>)item.Value)
							{
								mDisableEvents.Add((string)item2);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ThinkingSDKLogger.Print("Get config failed: " + ex.Message);
			}
			if (callback != null)
			{
				callback();
			}
		};
		mono.StartCoroutine(GetWithFORM(mConfigUrl, mToken, null, responseHandle));
	}

	private IEnumerator GetWithFORM(string url, string appId, Dictionary<string, object> param, ResponseHandle responseHandle)
	{
		yield return ThinkingSDKBaseRequest.GetWithFORM_2(mConfigUrl, mToken, param, responseHandle);
	}
}
