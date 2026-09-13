using System;
using System.Collections;
using System.Collections.Generic;
using ThinkingSDK.PC.Config;
using ThinkingSDK.PC.Constant;
using ThinkingSDK.PC.DataModel;
using ThinkingSDK.PC.Request;
using ThinkingSDK.PC.Storage;
using ThinkingSDK.PC.TaskManager;
using ThinkingSDK.PC.Time;
using ThinkingSDK.PC.Utils;
using UnityEngine;

namespace ThinkingSDK.PC.Main;

public class ThinkingSDKInstance
{
	private string mAppid;

	private string mServer;

	protected string mDistinctID;

	protected string mAccountID;

	private bool mOptTracking = true;

	private Dictionary<string, object> mTimeEvents = new Dictionary<string, object>();

	private Dictionary<string, object> mTimeEventsBefore = new Dictionary<string, object>();

	private bool mEnableTracking = true;

	private bool mEventSaveOnly;

	protected Dictionary<string, object> mSupperProperties = new Dictionary<string, object>();

	protected Dictionary<string, Dictionary<string, object>> mAutoTrackProperties = new Dictionary<string, Dictionary<string, object>>();

	private ThinkingSDKConfig mConfig;

	private ThinkingSDKBaseRequest mRequest;

	private ThinkingSDKTimeCalibration mTimeCalibration;

	private IDynamicSuperProperties_PC mDynamicProperties;

	private static ThinkingSDKInstance mCurrentInstance;

	private MonoBehaviour mMono;

	private static MonoBehaviour sMono;

	private ThinkingSDKAutoTrack mAutoTrack;

	private ResponseHandle mResponseHandle;

	private ThinkingSDKTask mTask
	{
		get
		{
			return ThinkingSDKTask.SingleTask();
		}
		set
		{
			mTask = value;
		}
	}

	public void SetTimeCalibratieton(ThinkingSDKTimeCalibration timeCalibration)
	{
		mTimeCalibration = timeCalibration;
	}

	private ThinkingSDKInstance()
	{
	}

	private void DefaultData()
	{
		DistinctId();
		AccountID();
		SuperProperties();
		DefaultTrackState();
	}

	public ThinkingSDKInstance(string appId, string server)
		: this(appId, server, null, null)
	{
	}

	public ThinkingSDKInstance(string appId, string server, string instanceName, ThinkingSDKConfig config, MonoBehaviour mono = null)
	{
		mMono = mono;
		sMono = mono;
		mResponseHandle = delegate
		{
			mTask.Release();
		};
		if (config == null)
		{
			mConfig = ThinkingSDKConfig.GetInstance(appId, server, instanceName);
		}
		else
		{
			mConfig = config;
		}
		mConfig.UpdateConfig(mono, delegate
		{
			if (mConfig.GetMode() == Mode.NORMAL)
			{
				sMono.StartCoroutine(WaitAndFlush());
			}
		});
		mAppid = appId;
		mServer = server;
		if (mConfig.GetMode() == Mode.NORMAL)
		{
			mRequest = new ThinkingSDKNormalRequest(appId, mConfig.NormalURL());
		}
		else
		{
			mRequest = new ThinkingSDKDebugRequest(appId, mConfig.DebugURL());
			if (mConfig.GetMode() == Mode.DEBUG_ONLY)
			{
				((ThinkingSDKDebugRequest)mRequest).SetDryRun(1);
			}
		}
		DefaultData();
		mCurrentInstance = this;
		UnityEngine.Object.DontDestroyOnLoad(new GameObject("ThinkingSDKTask", typeof(ThinkingSDKTask)));
		GameObject gameObject = new GameObject("ThinkingSDKAutoTrack", typeof(ThinkingSDKAutoTrack));
		mAutoTrack = (ThinkingSDKAutoTrack)gameObject.GetComponent(typeof(ThinkingSDKAutoTrack));
		if (!string.IsNullOrEmpty(instanceName))
		{
			mAutoTrack.SetAppId(instanceName);
		}
		else
		{
			mAutoTrack.SetAppId(mAppid);
		}
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
	}

	public static ThinkingSDKInstance CreateLightInstance()
	{
		return new LightThinkingSDKInstance(mCurrentInstance.mAppid, mCurrentInstance.mServer, mCurrentInstance.mConfig, sMono);
	}

	public ThinkingSDKTimeInter GetTime(DateTime dateTime)
	{
		ThinkingSDKTimeInter thinkingSDKTimeInter = null;
		if (dateTime == DateTime.MinValue)
		{
			if (mTimeCalibration == null)
			{
				return new ThinkingSDKTime(mConfig.TimeZone(), DateTime.Now);
			}
			return new ThinkingSDKCalibratedTime(mTimeCalibration, mConfig.TimeZone());
		}
		return new ThinkingSDKTime(mConfig.TimeZone(), dateTime);
	}

	public virtual void Identifiy(string distinctID)
	{
		if (!IsPaused() && !string.IsNullOrEmpty(distinctID))
		{
			mDistinctID = distinctID;
			ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.DISTINCT_ID, distinctID);
		}
	}

	public virtual string DistinctId()
	{
		mDistinctID = (string)ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.DISTINCT_ID, typeof(string));
		if (string.IsNullOrEmpty(mDistinctID))
		{
			mDistinctID = ThinkingSDKUtil.RandomID();
			ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.DISTINCT_ID, mDistinctID);
		}
		return mDistinctID;
	}

	public virtual void Login(string accountID)
	{
		if (!IsPaused() && !string.IsNullOrEmpty(accountID))
		{
			mAccountID = accountID;
			ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.ACCOUNT_ID, accountID);
		}
	}

	public virtual string AccountID()
	{
		mAccountID = (string)ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.ACCOUNT_ID, typeof(string));
		return mAccountID;
	}

	public virtual void Logout()
	{
		if (!IsPaused())
		{
			mAccountID = "";
			ThinkingSDKFile.DeleteData(mAppid, ThinkingSDKConstant.ACCOUNT_ID);
		}
	}

	public virtual void EnableAutoTrack(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties)
	{
		mAutoTrack.EnableAutoTrack(events, properties, mAppid);
	}

	public virtual void EnableAutoTrack(AUTO_TRACK_EVENTS events, IAutoTrackEventCallback_PC eventCallback)
	{
		mAutoTrack.EnableAutoTrack(events, eventCallback, mAppid);
	}

	public virtual void SetAutoTrackProperties(AUTO_TRACK_EVENTS events, Dictionary<string, object> properties)
	{
		mAutoTrack.SetAutoTrackProperties(events, properties);
	}

	public void Track(string eventName)
	{
		Track(eventName, null, DateTime.MinValue);
	}

	public void Track(string eventName, Dictionary<string, object> properties)
	{
		Track(eventName, properties, DateTime.MinValue);
	}

	public void Track(string eventName, Dictionary<string, object> properties, DateTime date)
	{
		Track(eventName, properties, date, null, immediately: false);
	}

	public void Track(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone)
	{
		Track(eventName, properties, date, timeZone, immediately: false);
	}

	public void Track(string eventName, Dictionary<string, object> properties, DateTime date, TimeZoneInfo timeZone, bool immediately)
	{
		ThinkingSDKEventData thinkingSDKEventData = new ThinkingSDKEventData(GetTime(date), eventName, properties);
		if (timeZone != null)
		{
			thinkingSDKEventData.SetTimeZone(timeZone);
		}
		SendData(thinkingSDKEventData, immediately);
	}

	private void SendData(ThinkingSDKEventData data)
	{
		SendData(data, immediately: false);
	}

	private void SendData(ThinkingSDKEventData data, bool immediately)
	{
		if (mDynamicProperties != null)
		{
			data.SetProperties(mDynamicProperties.GetDynamicSuperProperties_PC(), isOverwrite: false);
		}
		if (mSupperProperties != null && mSupperProperties.Count > 0)
		{
			data.SetProperties(mSupperProperties, isOverwrite: false);
		}
		Dictionary<string, object> dictionary = ThinkingSDKUtil.DeviceInfo();
		foreach (string disPresetProperty in ThinkingSDKUtil.DisPresetProperties)
		{
			if (dictionary.ContainsKey(disPresetProperty))
			{
				dictionary.Remove(disPresetProperty);
			}
		}
		data.SetProperties(dictionary, isOverwrite: false);
		float num = 0f;
		if (mTimeEvents.ContainsKey(data.EventName()))
		{
			int num2 = (int)mTimeEvents[data.EventName()];
			num = (float)((double)(Environment.TickCount - num2) / 1000.0);
			mTimeEvents.Remove(data.EventName());
			if (mTimeEventsBefore.ContainsKey(data.EventName()))
			{
				int num3 = (int)mTimeEventsBefore[data.EventName()];
				num += (float)((double)num3 / 1000.0);
				mTimeEventsBefore.Remove(data.EventName());
			}
		}
		if (num != 0f)
		{
			data.SetDuration(num);
		}
		SendData((ThinkingSDKBaseData)data, immediately);
	}

	private void SendData(ThinkingSDKBaseData data)
	{
		SendData(data, immediately: false);
	}

	private void SendData(ThinkingSDKBaseData data, bool immediately)
	{
		if (IsPaused())
		{
			return;
		}
		if (!string.IsNullOrEmpty(mAccountID))
		{
			data.SetAccountID(mAccountID);
		}
		if (string.IsNullOrEmpty(mDistinctID))
		{
			DistinctId();
		}
		data.SetDistinctID(mDistinctID);
		if (mConfig.IsDisabledEvent(data.EventName()))
		{
			ThinkingSDKLogger.Print("disabled Event: " + data.EventName());
			return;
		}
		IList<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
		list.Add(data.ToDictionary());
		if (mConfig.GetMode() == Mode.NORMAL && mRequest.GetType() != typeof(ThinkingSDKNormalRequest))
		{
			mRequest = new ThinkingSDKNormalRequest(mAppid, mConfig.NormalURL());
		}
		if (immediately)
		{
			mRequest.SendData_2(null, list);
			return;
		}
		Dictionary<string, object> dictionary = data.ToDictionary();
		int num = 0;
		if (!string.IsNullOrEmpty(mConfig.InstanceName()))
		{
			ThinkingSDKLogger.Print("Save event: " + ThinkingSDKJSON.Serialize(dictionary) + "\n  AppID: " + mAppid);
			num = ThinkingSDKFileJson.EnqueueTrackingData(dictionary, mConfig.InstanceName());
		}
		else
		{
			ThinkingSDKLogger.Print("Save event: " + ThinkingSDKJSON.Serialize(dictionary) + "\n  AppID: " + mAppid);
			num = ThinkingSDKFileJson.EnqueueTrackingData(dictionary, mAppid);
		}
		if (mConfig.GetMode() != Mode.NORMAL || num >= mConfig.mUploadSize)
		{
			Flush();
		}
	}

	private IEnumerator WaitAndFlush()
	{
		while (true)
		{
			yield return new WaitForSeconds(mConfig.mUploadInterval);
			Flush();
		}
	}

	public virtual void Flush()
	{
		if (mEventSaveOnly)
		{
			return;
		}
		mTask.SyncInvokeAllTask();
		int batchSize = ((mConfig.GetMode() != Mode.NORMAL) ? 1 : mConfig.mUploadSize);
		ResponseHandle responseHandle = delegate(Dictionary<string, object> result)
		{
			int num = 0;
			if (result != null)
			{
				int batchSize2 = 0;
				if (result.ContainsKey("flush_count"))
				{
					batchSize2 = (int)result["flush_count"];
				}
				num = (string.IsNullOrEmpty(mConfig.InstanceName()) ? ThinkingSDKFileJson.DeleteBatchTrackingData(batchSize2, mAppid) : ThinkingSDKFileJson.DeleteBatchTrackingData(batchSize2, mConfig.InstanceName()));
			}
			mTask.Release();
			if (num > 0)
			{
				Flush();
			}
		};
		if (!string.IsNullOrEmpty(mConfig.InstanceName()))
		{
			mTask.StartRequest(mRequest, responseHandle, batchSize, mConfig.InstanceName());
		}
		else
		{
			mTask.StartRequest(mRequest, responseHandle, batchSize, mAppid);
		}
	}

	public void FlushImmediately()
	{
		if (mEventSaveOnly)
		{
			return;
		}
		mTask.SyncInvokeAllTask();
		int batchSize = ((mConfig.GetMode() != Mode.NORMAL) ? 1 : mConfig.mUploadSize);
		ResponseHandle responseHandle = delegate(Dictionary<string, object> result)
		{
			int num = 0;
			if (result != null)
			{
				num = (string.IsNullOrEmpty(mConfig.InstanceName()) ? ThinkingSDKFileJson.DeleteBatchTrackingData(batchSize, mAppid) : ThinkingSDKFileJson.DeleteBatchTrackingData(batchSize, mConfig.InstanceName()));
			}
			mTask.Release();
			if (num > 0)
			{
				Flush();
			}
		};
		IList<Dictionary<string, object>> list = ThinkingSDKFileJson.DequeueBatchTrackingData(batchSize, mAppid);
		list = (string.IsNullOrEmpty(mConfig.InstanceName()) ? ThinkingSDKFileJson.DequeueBatchTrackingData(batchSize, mAppid) : ThinkingSDKFileJson.DequeueBatchTrackingData(batchSize, mConfig.InstanceName()));
		if (list.Count > 0)
		{
			mMono.StartCoroutine(mRequest.SendData_2(responseHandle, list));
		}
	}

	public void Track(ThinkingSDKEventData analyticsEvent)
	{
		ThinkingSDKTimeInter time = GetTime(analyticsEvent.Time());
		analyticsEvent.SetTime(time);
		SendData(analyticsEvent);
	}

	public virtual void SetSuperProperties(Dictionary<string, object> superProperties)
	{
		if (!IsPaused())
		{
			Dictionary<string, object> originalDic = new Dictionary<string, object>();
			string text = (string)ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY, typeof(string));
			if (!string.IsNullOrEmpty(text))
			{
				originalDic = ThinkingSDKJSON.Deserialize(text);
			}
			ThinkingSDKUtil.AddDictionary(originalDic, superProperties);
			mSupperProperties = originalDic;
			ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY, ThinkingSDKJSON.Serialize(mSupperProperties));
		}
	}

	public virtual void UnsetSuperProperty(string propertyKey)
	{
		if (!IsPaused())
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string text = (string)ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY, typeof(string));
			if (!string.IsNullOrEmpty(text))
			{
				dictionary = ThinkingSDKJSON.Deserialize(text);
			}
			if (dictionary.ContainsKey(propertyKey))
			{
				dictionary.Remove(propertyKey);
			}
			mSupperProperties = dictionary;
			ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY, ThinkingSDKJSON.Serialize(mSupperProperties));
		}
	}

	public virtual Dictionary<string, object> SuperProperties()
	{
		string text = (string)ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY, typeof(string));
		if (!string.IsNullOrEmpty(text))
		{
			mSupperProperties = ThinkingSDKJSON.Deserialize(text);
		}
		return mSupperProperties;
	}

	public Dictionary<string, object> PresetProperties()
	{
		return new Dictionary<string, object>
		{
			[ThinkingSDKConstant.DEVICE_ID] = ThinkingSDKDeviceInfo.DeviceID(),
			[ThinkingSDKConstant.CARRIER] = ThinkingSDKDeviceInfo.Carrier(),
			[ThinkingSDKConstant.OS] = ThinkingSDKDeviceInfo.OS(),
			[ThinkingSDKConstant.SCREEN_HEIGHT] = ThinkingSDKDeviceInfo.ScreenHeight(),
			[ThinkingSDKConstant.SCREEN_WIDTH] = ThinkingSDKDeviceInfo.ScreenWidth(),
			[ThinkingSDKConstant.MANUFACTURE] = ThinkingSDKDeviceInfo.Manufacture(),
			[ThinkingSDKConstant.DEVICE_MODEL] = ThinkingSDKDeviceInfo.DeviceModel(),
			[ThinkingSDKConstant.SYSTEM_LANGUAGE] = ThinkingSDKDeviceInfo.MachineLanguage(),
			[ThinkingSDKConstant.OS_VERSION] = ThinkingSDKDeviceInfo.OSVersion(),
			[ThinkingSDKConstant.NETWORK_TYPE] = ThinkingSDKDeviceInfo.NetworkType(),
			[ThinkingSDKConstant.APP_BUNDLEID] = ThinkingSDKAppInfo.AppIdentifier(),
			[ThinkingSDKConstant.APP_VERSION] = ThinkingSDKAppInfo.AppVersion(),
			[ThinkingSDKConstant.ZONE_OFFSET] = ThinkingSDKUtil.ZoneOffset(DateTime.Now, mConfig.TimeZone())
		};
	}

	public virtual void ClearSuperProperties()
	{
		if (!IsPaused())
		{
			mSupperProperties.Clear();
			ThinkingSDKFile.DeleteData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY);
		}
	}

	public void TimeEvent(string eventName)
	{
		if (!mTimeEvents.ContainsKey(eventName))
		{
			mTimeEvents.Add(eventName, Environment.TickCount);
		}
	}

	public void PauseTimeEvent(bool status, string eventName = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			string[] array = new string[mTimeEvents.Keys.Count];
			mTimeEvents.Keys.CopyTo(array, 0);
			foreach (string key in array)
			{
				if (status)
				{
					int num = int.Parse(mTimeEvents[key].ToString());
					int num2 = Environment.TickCount - num;
					if (mTimeEventsBefore.ContainsKey(key))
					{
						num2 += int.Parse(mTimeEventsBefore[key].ToString());
					}
					mTimeEventsBefore[key] = num2;
				}
				else
				{
					mTimeEvents[key] = Environment.TickCount;
				}
			}
		}
		else if (status)
		{
			int num3 = int.Parse(mTimeEvents[eventName].ToString());
			int num4 = Environment.TickCount - num3;
			mTimeEventsBefore[eventName] = num4;
		}
		else
		{
			mTimeEvents[eventName] = Environment.TickCount;
		}
	}

	public void UserSet(Dictionary<string, object> properties)
	{
		UserSet(properties, DateTime.MinValue);
	}

	public void UserSet(Dictionary<string, object> properties, DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), ThinkingSDKConstant.USER_SET, properties);
		SendData(data);
	}

	public void UserUnset(string propertyKey)
	{
		UserUnset(propertyKey, DateTime.MinValue);
	}

	public void UserUnset(string propertyKey, DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), properties: new Dictionary<string, object> { [propertyKey] = 0 }, eventType: ThinkingSDKConstant.USER_UNSET);
		SendData(data);
	}

	public void UserUnset(List<string> propertyKeys)
	{
		UserUnset(propertyKeys, DateTime.MinValue);
	}

	public void UserUnset(List<string> propertyKeys, DateTime dateTime)
	{
		ThinkingSDKTimeInter time = GetTime(dateTime);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		foreach (string propertyKey in propertyKeys)
		{
			dictionary[propertyKey] = 0;
		}
		ThinkingSDKUserData data = new ThinkingSDKUserData(time, ThinkingSDKConstant.USER_UNSET, dictionary);
		SendData(data);
	}

	public void UserSetOnce(Dictionary<string, object> properties)
	{
		UserSetOnce(properties, DateTime.MinValue);
	}

	public void UserSetOnce(Dictionary<string, object> properties, DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), ThinkingSDKConstant.USER_SETONCE, properties);
		SendData(data);
	}

	public void UserAdd(Dictionary<string, object> properties)
	{
		UserAdd(properties, DateTime.MinValue);
	}

	public void UserAdd(Dictionary<string, object> properties, DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), ThinkingSDKConstant.USER_ADD, properties);
		SendData(data);
	}

	public void UserAppend(Dictionary<string, object> properties)
	{
		UserAppend(properties, DateTime.MinValue);
	}

	public void UserAppend(Dictionary<string, object> properties, DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), ThinkingSDKConstant.USER_APPEND, properties);
		SendData(data);
	}

	public void UserUniqAppend(Dictionary<string, object> properties)
	{
		UserUniqAppend(properties, DateTime.MinValue);
	}

	public void UserUniqAppend(Dictionary<string, object> properties, DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), ThinkingSDKConstant.USER_UNIQ_APPEND, properties);
		SendData(data);
	}

	public void UserDelete()
	{
		UserDelete(DateTime.MinValue);
	}

	public void UserDelete(DateTime dateTime)
	{
		ThinkingSDKUserData data = new ThinkingSDKUserData(GetTime(dateTime), properties: new Dictionary<string, object>(), eventType: ThinkingSDKConstant.USER_DEL);
		SendData(data);
	}

	public void SetDynamicSuperProperties(IDynamicSuperProperties_PC dynamicSuperProperties)
	{
		if (!IsPaused())
		{
			mDynamicProperties = dynamicSuperProperties;
		}
	}

	protected bool IsPaused()
	{
		int num;
		if (mEnableTracking)
		{
			num = ((!mOptTracking) ? 1 : 0);
			if (num == 0)
			{
				goto IL_0021;
			}
		}
		else
		{
			num = 1;
		}
		ThinkingSDKLogger.Print("Track status is Pause or Stop");
		goto IL_0021;
		IL_0021:
		return (byte)num != 0;
	}

	public void SetTrackStatus(TA_TRACK_STATUS status)
	{
		ThinkingSDKLogger.Print("SetTrackStatus: " + status);
		switch (status)
		{
		case TA_TRACK_STATUS.PAUSE:
			mEventSaveOnly = false;
			OptTracking(optTracking: true);
			EnableTracking(isEnable: false);
			break;
		case TA_TRACK_STATUS.STOP:
			mEventSaveOnly = false;
			EnableTracking(isEnable: true);
			OptTracking(optTracking: false);
			break;
		case TA_TRACK_STATUS.SAVE_ONLY:
			mEventSaveOnly = true;
			EnableTracking(isEnable: true);
			OptTracking(optTracking: true);
			break;
		default:
			mEventSaveOnly = false;
			OptTracking(optTracking: true);
			EnableTracking(isEnable: true);
			Flush();
			break;
		}
	}

	public void OptTracking(bool optTracking)
	{
		mOptTracking = optTracking;
		int num = (optTracking ? 1 : 0);
		ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.OPT_TRACK, num);
		if (!optTracking)
		{
			ThinkingSDKFile.DeleteData(mAppid, ThinkingSDKConstant.ACCOUNT_ID);
			ThinkingSDKFile.DeleteData(mAppid, ThinkingSDKConstant.DISTINCT_ID);
			ThinkingSDKFile.DeleteData(mAppid, ThinkingSDKConstant.SUPER_PROPERTY);
			mAccountID = null;
			mDistinctID = null;
			mSupperProperties = new Dictionary<string, object>();
			ThinkingSDKFileJson.DeleteAllTrackingData(mAppid);
		}
	}

	public void EnableTracking(bool isEnable)
	{
		mEnableTracking = isEnable;
		int num = (isEnable ? 1 : 0);
		ThinkingSDKFile.SaveData(mAppid, ThinkingSDKConstant.ENABLE_TRACK, num);
	}

	private void DefaultTrackState()
	{
		object data = ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.ENABLE_TRACK, typeof(int));
		object data2 = ThinkingSDKFile.GetData(mAppid, ThinkingSDKConstant.OPT_TRACK, typeof(int));
		if (data != null)
		{
			mEnableTracking = (int)data == 1;
		}
		else
		{
			mEnableTracking = true;
		}
		if (data2 != null)
		{
			mOptTracking = (int)data2 == 1;
		}
		else
		{
			mOptTracking = true;
		}
	}

	public void OptTrackingAndDeleteUser()
	{
		UserDelete();
		OptTracking(optTracking: false);
	}

	public string TimeString(DateTime dateTime)
	{
		return ThinkingSDKUtil.FormatDate(dateTime, mConfig.TimeZone());
	}
}
