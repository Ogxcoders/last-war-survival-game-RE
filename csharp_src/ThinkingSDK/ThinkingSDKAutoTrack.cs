using System.Collections.Generic;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.Main;
using ThinkingSDK.PC.Storage;
using ThinkingSDK.PC.Utils;
using UnityEngine;

public class ThinkingSDKAutoTrack : MonoBehaviour
{
	private string mAppId;

	private AUTO_TRACK_EVENTS mAutoTrackEvents;

	private Dictionary<string, Dictionary<string, object>> mAutoTrackProperties = new Dictionary<string, Dictionary<string, object>>();

	private bool mStarted;

	private IAutoTrackEventCallback_PC mEventCallback_PC;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			if ((mAutoTrackEvents & AUTO_TRACK_EVENTS.APP_START) != AUTO_TRACK_EVENTS.NONE)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_START.ToString()))
				{
					ThinkingSDKUtil.AddDictionary(dictionary, mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_START.ToString()]);
				}
				if (mEventCallback_PC != null)
				{
					ThinkingSDKUtil.AddDictionary(dictionary, mEventCallback_PC.AutoTrackEventCallback_PC(1, dictionary));
				}
				ThinkingPCSDK.Track(ThinkingSDKConstant.START_EVENT, dictionary, mAppId);
			}
			if ((mAutoTrackEvents & AUTO_TRACK_EVENTS.APP_END) != AUTO_TRACK_EVENTS.NONE)
			{
				ThinkingPCSDK.TimeEvent(ThinkingSDKConstant.END_EVENT, mAppId);
			}
			ThinkingPCSDK.PauseTimeEvent(status: false, "", mAppId);
			return;
		}
		if ((mAutoTrackEvents & AUTO_TRACK_EVENTS.APP_END) != AUTO_TRACK_EVENTS.NONE)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_END.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(dictionary2, mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_END.ToString()]);
			}
			if (mEventCallback_PC != null)
			{
				ThinkingSDKUtil.AddDictionary(dictionary2, mEventCallback_PC.AutoTrackEventCallback_PC(2, dictionary2));
			}
			ThinkingPCSDK.Track(ThinkingSDKConstant.END_EVENT, dictionary2, mAppId);
		}
		ThinkingPCSDK.Flush(mAppId);
		ThinkingPCSDK.PauseTimeEvent(status: true, "", mAppId);
	}

	private void OnApplicationQuit()
	{
		if (Application.isFocused)
		{
			OnApplicationFocus(hasFocus: false);
		}
		ThinkingPCSDK.FlushImmediately(mAppId);
	}

	public void SetAppId(string appId)
	{
		mAppId = appId;
	}

	public void EnableAutoTrack(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties, string appId)
	{
		SetAutoTrackProperties(events, properties);
		if ((events & AUTO_TRACK_EVENTS.APP_INSTALL) != AUTO_TRACK_EVENTS.NONE && ThinkingSDKFile.GetData(appId, ThinkingSDKConstant.IS_INSTALL, typeof(int)) == null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(properties);
			ThinkingSDKFile.SaveData(appId, ThinkingSDKConstant.IS_INSTALL, 1);
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_INSTALL.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(dictionary, mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_INSTALL.ToString()]);
			}
			ThinkingPCSDK.Track(ThinkingSDKConstant.INSTALL_EVENT, dictionary, mAppId);
			ThinkingPCSDK.Flush(mAppId);
		}
		if ((events & AUTO_TRACK_EVENTS.APP_START) != AUTO_TRACK_EVENTS.NONE && !mStarted)
		{
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>(properties);
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_START.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(dictionary2, mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_START.ToString()]);
			}
			ThinkingPCSDK.Track(ThinkingSDKConstant.START_EVENT, dictionary2, mAppId);
			ThinkingPCSDK.Flush(mAppId);
		}
		if ((events & AUTO_TRACK_EVENTS.APP_END) != AUTO_TRACK_EVENTS.NONE && !mStarted)
		{
			ThinkingPCSDK.TimeEvent(ThinkingSDKConstant.END_EVENT, mAppId);
		}
		mStarted = true;
	}

	public void EnableAutoTrack(AUTO_TRACK_EVENTS events, IAutoTrackEventCallback_PC eventCallback, string appId)
	{
		mAutoTrackEvents = events;
		mEventCallback_PC = eventCallback;
		if ((events & AUTO_TRACK_EVENTS.APP_INSTALL) != AUTO_TRACK_EVENTS.NONE && ThinkingSDKFile.GetData(appId, ThinkingSDKConstant.IS_INSTALL, typeof(int)) == null)
		{
			ThinkingSDKFile.SaveData(appId, ThinkingSDKConstant.IS_INSTALL, 1);
			Dictionary<string, object> dictionary = null;
			dictionary = ((!mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_INSTALL.ToString())) ? new Dictionary<string, object>() : mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_INSTALL.ToString()]);
			if (mEventCallback_PC != null)
			{
				ThinkingSDKUtil.AddDictionary(dictionary, mEventCallback_PC.AutoTrackEventCallback_PC(32, dictionary));
			}
			ThinkingPCSDK.Track(ThinkingSDKConstant.INSTALL_EVENT, dictionary, mAppId);
			ThinkingPCSDK.Flush(mAppId);
		}
		if ((events & AUTO_TRACK_EVENTS.APP_START) != AUTO_TRACK_EVENTS.NONE && !mStarted)
		{
			Dictionary<string, object> dictionary2 = null;
			dictionary2 = ((!mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_START.ToString())) ? new Dictionary<string, object>() : mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_START.ToString()]);
			if (mEventCallback_PC != null)
			{
				ThinkingSDKUtil.AddDictionary(dictionary2, mEventCallback_PC.AutoTrackEventCallback_PC(1, dictionary2));
			}
			ThinkingPCSDK.Track(ThinkingSDKConstant.START_EVENT, dictionary2, mAppId);
			ThinkingPCSDK.Flush(mAppId);
		}
		if ((events & AUTO_TRACK_EVENTS.APP_END) != AUTO_TRACK_EVENTS.NONE && !mStarted)
		{
			ThinkingPCSDK.TimeEvent(ThinkingSDKConstant.END_EVENT, mAppId);
		}
		mStarted = true;
	}

	public void SetAutoTrackProperties(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties)
	{
		mAutoTrackEvents = events;
		if ((events & AUTO_TRACK_EVENTS.APP_INSTALL) != AUTO_TRACK_EVENTS.NONE)
		{
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_INSTALL.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_INSTALL.ToString()], properties);
			}
			mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_INSTALL.ToString()] = properties;
		}
		if ((events & AUTO_TRACK_EVENTS.APP_START) != AUTO_TRACK_EVENTS.NONE)
		{
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_START.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_START.ToString()], properties);
			}
			mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_START.ToString()] = properties;
		}
		if ((events & AUTO_TRACK_EVENTS.APP_END) != AUTO_TRACK_EVENTS.NONE)
		{
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_END.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_END.ToString()], properties);
			}
			mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_END.ToString()] = properties;
		}
		if ((events & AUTO_TRACK_EVENTS.APP_CRASH) != AUTO_TRACK_EVENTS.NONE)
		{
			if (mAutoTrackProperties.ContainsKey(AUTO_TRACK_EVENTS.APP_CRASH.ToString()))
			{
				ThinkingSDKUtil.AddDictionary(mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_CRASH.ToString()], properties);
			}
			mAutoTrackProperties[AUTO_TRACK_EVENTS.APP_CRASH.ToString()] = properties;
		}
	}
}
