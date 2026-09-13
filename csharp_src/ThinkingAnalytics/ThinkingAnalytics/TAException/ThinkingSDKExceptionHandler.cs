using System;
using System.Collections.Generic;
using UnityEngine;

namespace ThinkingAnalytics.TAException;

public class ThinkingSDKExceptionHandler
{
	public static bool IsQuitWhenException = false;

	public static bool IsRegistered = false;

	private static IAutoTrackEventCallback mEventCallback;

	private static Dictionary<string, object> mProperties;

	private static Dictionary<string, float> _filterTimeMap = new Dictionary<string, float>();

	private const float REPORT_FREQUENCY = 10f;

	public static void SetAutoTrackProperties(Dictionary<string, object> properties)
	{
		if (mProperties == null)
		{
			mProperties = new Dictionary<string, object>();
		}
		foreach (KeyValuePair<string, object> property in properties)
		{
			if (!mProperties.ContainsKey(property.Key))
			{
				mProperties.Add(property.Key, property.Value);
			}
		}
	}

	public static void RegisterTAExceptionHandler(IAutoTrackEventCallback eventCallback)
	{
		mEventCallback = eventCallback;
		try
		{
			if (!IsRegistered)
			{
				Application.logMessageReceived += _LogHandler;
				AppDomain.CurrentDomain.UnhandledException += _UncaughtExceptionHandler;
				IsRegistered = true;
			}
		}
		catch
		{
		}
	}

	public static void RegisterTAExceptionHandler(Dictionary<string, object> properties)
	{
		SetAutoTrackProperties(properties);
		try
		{
			if (!IsRegistered)
			{
				Application.logMessageReceived += _LogHandler;
				AppDomain.CurrentDomain.UnhandledException += _UncaughtExceptionHandler;
				IsRegistered = true;
			}
		}
		catch
		{
		}
	}

	public static void UnregisterTAExceptionHandler()
	{
		try
		{
			Application.logMessageReceived -= _LogHandler;
			AppDomain.CurrentDomain.UnhandledException -= _UncaughtExceptionHandler;
		}
		catch
		{
		}
	}

	private static void _LogHandler(string logString, string stackTrace, LogType type)
	{
		if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)
		{
			return;
		}
		string text = "exception_type: " + type.ToString() + " <br> exception_message: " + logString + " <br> stack_trace: " + stackTrace + " <br> ";
		if (CheckCanReport("ta_app_crash", text))
		{
			Dictionary<string, object> properties = new Dictionary<string, object> { { "#app_crashed_reason", text } };
			properties = MergeProperties(properties);
			ThinkingAnalyticsAPI.Track("ta_app_crash", properties);
			if (IsQuitWhenException)
			{
				Application.Quit();
			}
		}
	}

	private static bool CheckCanReport(string eventName, string reason)
	{
		if (eventName != "ta_app_crash")
		{
			return true;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (!_filterTimeMap.TryGetValue(reason, out var value))
		{
			_filterTimeMap[reason] = realtimeSinceStartup;
			return true;
		}
		if (realtimeSinceStartup - value > 10f)
		{
			_filterTimeMap[reason] = realtimeSinceStartup;
			return true;
		}
		return false;
	}

	private static void _UncaughtExceptionHandler(object sender, UnhandledExceptionEventArgs args)
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
		string text = "UnhandledException:" + ex.GetType().Name + " <br> exception_message: " + ex.Message + " <br> stack_trace: " + ex.StackTrace + " <br> ";
		if (CheckCanReport("ta_app_crash", text))
		{
			Dictionary<string, object> properties = new Dictionary<string, object> { { "#app_crashed_reason", text } };
			properties = MergeProperties(properties);
			ThinkingAnalyticsAPI.Track("ta_app_crash", properties);
			if (IsQuitWhenException)
			{
				Application.Quit();
			}
		}
	}

	private static Dictionary<string, object> MergeProperties(Dictionary<string, object> properties)
	{
		if (mEventCallback != null)
		{
			foreach (KeyValuePair<string, object> item in mEventCallback.AutoTrackEventCallback(16, properties))
			{
				if (!properties.ContainsKey(item.Key))
				{
					properties.Add(item.Key, item.Value);
				}
			}
		}
		if (mProperties != null)
		{
			foreach (KeyValuePair<string, object> mProperty in mProperties)
			{
				if (!properties.ContainsKey(mProperty.Key))
				{
					properties.Add(mProperty.Key, mProperty.Value);
				}
			}
		}
		return properties;
	}
}
