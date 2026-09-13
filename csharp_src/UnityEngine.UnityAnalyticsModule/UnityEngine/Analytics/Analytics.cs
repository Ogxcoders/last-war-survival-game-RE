using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;

namespace UnityEngine.Analytics;

[StructLayout(LayoutKind.Sequential)]
[NativeHeader("Modules/UnityAnalytics/Public/Events/UserCustomEvent.h")]
[NativeHeader("Modules/UnityConnect/UnityConnectSettings.h")]
[NativeHeader("Modules/UnityAnalytics/Public/UnityAnalytics.h")]
public static class Analytics
{
	[Serializable]
	private struct UserInfo
	{
		public string custom_userid;

		public string sex;
	}

	[Serializable]
	private struct UserInfoBirthYear
	{
		public int birth_year;
	}

	public static bool initializeOnStartup
	{
		get
		{
			if (!IsInitialized())
			{
				return false;
			}
			return initializeOnStartupInternal;
		}
		set
		{
			if (IsInitialized())
			{
				initializeOnStartupInternal = value;
			}
		}
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern bool initializeOnStartupInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetInitializeOnStartup")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetInitializeOnStartup")]
		set;
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern bool enabledInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetEnabled")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetEnabled")]
		set;
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern bool playerOptedOutInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetPlayerOptedOut")]
		get;
	}

	[StaticAccessor("GetUnityConnectSettings()", StaticAccessorType.Dot)]
	private static extern string eventUrlInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetEventUrl")]
		get;
	}

	[StaticAccessor("GetUnityConnectSettings()", StaticAccessorType.Dot)]
	private static extern string configUrlInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetConfigUrl")]
		get;
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern bool limitUserTrackingInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetLimitUserTracking")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetLimitUserTracking")]
		set;
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern bool deviceStatsEnabledInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetDeviceStatsEnabled")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetDeviceStatsEnabled")]
		set;
	}

	public static bool playerOptedOut
	{
		get
		{
			if (!IsInitialized())
			{
				return false;
			}
			return playerOptedOutInternal;
		}
	}

	public static string eventUrl
	{
		get
		{
			if (!IsInitialized())
			{
				return string.Empty;
			}
			return eventUrlInternal;
		}
	}

	public static string configUrl
	{
		get
		{
			if (!IsInitialized())
			{
				return string.Empty;
			}
			return configUrlInternal;
		}
	}

	public static bool limitUserTracking
	{
		get
		{
			if (!IsInitialized())
			{
				return false;
			}
			return limitUserTrackingInternal;
		}
		set
		{
			if (IsInitialized())
			{
				limitUserTrackingInternal = value;
			}
		}
	}

	public static bool deviceStatsEnabled
	{
		get
		{
			if (!IsInitialized())
			{
				return false;
			}
			return deviceStatsEnabledInternal;
		}
		set
		{
			if (IsInitialized())
			{
				deviceStatsEnabledInternal = value;
			}
		}
	}

	public static bool enabled
	{
		get
		{
			if (!IsInitialized())
			{
				return false;
			}
			return enabledInternal;
		}
		set
		{
			if (IsInitialized())
			{
				enabledInternal = value;
			}
		}
	}

	public static AnalyticsResult ResumeInitialization()
	{
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return ResumeInitializationInternal();
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeMethod("ResumeInitialization")]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult ResumeInitializationInternal();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal static extern bool IsInitialized();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	[NativeMethod("FlushEvents")]
	private static extern bool FlushArchivedEvents();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult Transaction(string productId, double amount, string currency, string receiptPurchaseData, string signature, bool usingIAPService);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult SendCustomEventName(string customEventName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult SendCustomEvent(CustomEventData eventData);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult IsCustomEventWithLimitEnabled(string customEventName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult EnableCustomEventWithLimit(string customEventName, bool enable);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult IsEventWithLimitEnabled(string eventName, int ver, string prefix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult EnableEventWithLimit(string eventName, bool enable, int ver, string prefix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult RegisterEventWithLimit(string eventName, int maxEventPerHour, int maxItems, string vendorKey, int ver, string prefix, string assemblyInfo, bool notifyServer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult RegisterEventsWithLimit(string[] eventName, int maxEventPerHour, int maxItems, string vendorKey, int ver, string prefix, string assemblyInfo, bool notifyServer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	[ThreadSafe]
	internal static extern AnalyticsResult SendEventWithLimit(string eventName, object parameters, int ver, string prefix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult SetEventWithLimitEndPoint(string eventName, string endPoint, int ver, string prefix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern AnalyticsResult SetEventWithLimitPriority(string eventName, AnalyticsEventPriority eventPriority, int ver, string prefix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	internal static extern bool QueueEvent(string eventName, object parameters, int ver, string prefix);

	public static AnalyticsResult FlushEvents()
	{
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return (!FlushArchivedEvents()) ? AnalyticsResult.NotInitialized : AnalyticsResult.Ok;
	}

	public static AnalyticsResult SetUserId(string userId)
	{
		if (string.IsNullOrEmpty(userId))
		{
			throw new ArgumentException("Cannot set userId to an empty or null string");
		}
		return SendUserInfoEvent(new UserInfo
		{
			custom_userid = userId
		});
	}

	public static AnalyticsResult SetUserGender(Gender gender)
	{
		return SendUserInfoEvent(new UserInfo
		{
			sex = gender switch
			{
				Gender.Female => "F", 
				Gender.Male => "M", 
				_ => "U", 
			}
		});
	}

	public static AnalyticsResult SetUserBirthYear(int birthYear)
	{
		return SendUserInfoEvent(new UserInfoBirthYear
		{
			birth_year = birthYear
		});
	}

	private static AnalyticsResult SendUserInfoEvent(object param)
	{
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		QueueEvent("userInfo", param, 1, string.Empty);
		return AnalyticsResult.Ok;
	}

	public static AnalyticsResult Transaction(string productId, decimal amount, string currency)
	{
		return Transaction(productId, amount, currency, null, null, usingIAPService: false);
	}

	public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature)
	{
		return Transaction(productId, amount, currency, receiptPurchaseData, signature, usingIAPService: false);
	}

	public static AnalyticsResult Transaction(string productId, decimal amount, string currency, string receiptPurchaseData, string signature, bool usingIAPService)
	{
		if (string.IsNullOrEmpty(productId))
		{
			throw new ArgumentException("Cannot set productId to an empty or null string");
		}
		if (string.IsNullOrEmpty(currency))
		{
			throw new ArgumentException("Cannot set currency to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		if (receiptPurchaseData == null)
		{
			receiptPurchaseData = string.Empty;
		}
		if (signature == null)
		{
			signature = string.Empty;
		}
		return Transaction(productId, Convert.ToDouble(amount), currency, receiptPurchaseData, signature, usingIAPService);
	}

	public static AnalyticsResult CustomEvent(string customEventName)
	{
		if (string.IsNullOrEmpty(customEventName))
		{
			throw new ArgumentException("Cannot set custom event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return SendCustomEventName(customEventName);
	}

	public static AnalyticsResult CustomEvent(string customEventName, Vector3 position)
	{
		if (string.IsNullOrEmpty(customEventName))
		{
			throw new ArgumentException("Cannot set custom event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		CustomEventData customEventData = new CustomEventData(customEventName);
		customEventData.AddDouble("x", (double)Convert.ToDecimal(position.x));
		customEventData.AddDouble("y", (double)Convert.ToDecimal(position.y));
		customEventData.AddDouble("z", (double)Convert.ToDecimal(position.z));
		AnalyticsResult result = SendCustomEvent(customEventData);
		customEventData.Dispose();
		return result;
	}

	public static AnalyticsResult CustomEvent(string customEventName, IDictionary<string, object> eventData)
	{
		if (string.IsNullOrEmpty(customEventName))
		{
			throw new ArgumentException("Cannot set custom event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		if (eventData == null)
		{
			return SendCustomEventName(customEventName);
		}
		CustomEventData customEventData = new CustomEventData(customEventName);
		AnalyticsResult result = AnalyticsResult.InvalidData;
		try
		{
			customEventData.AddDictionary(eventData);
			result = SendCustomEvent(customEventData);
		}
		finally
		{
			customEventData.Dispose();
		}
		return result;
	}

	public static AnalyticsResult EnableCustomEvent(string customEventName, bool enabled)
	{
		if (string.IsNullOrEmpty(customEventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return EnableCustomEventWithLimit(customEventName, enabled);
	}

	public static AnalyticsResult IsCustomEventEnabled(string customEventName)
	{
		if (string.IsNullOrEmpty(customEventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return IsCustomEventWithLimitEnabled(customEventName);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AnalyticsResult RegisterEvent(string eventName, int maxEventPerHour, int maxItems, string vendorKey = "", string prefix = "")
	{
		string empty = string.Empty;
		empty = Assembly.GetCallingAssembly().FullName;
		return RegisterEvent(eventName, maxEventPerHour, maxItems, vendorKey, 1, prefix, empty);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AnalyticsResult RegisterEvent(string eventName, int maxEventPerHour, int maxItems, string vendorKey, int ver, string prefix = "")
	{
		string empty = string.Empty;
		empty = Assembly.GetCallingAssembly().FullName;
		return RegisterEvent(eventName, maxEventPerHour, maxItems, vendorKey, ver, prefix, empty);
	}

	private static AnalyticsResult RegisterEvent(string eventName, int maxEventPerHour, int maxItems, string vendorKey, int ver, string prefix, string assemblyInfo)
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return RegisterEventWithLimit(eventName, maxEventPerHour, maxItems, vendorKey, ver, prefix, assemblyInfo, notifyServer: true);
	}

	public static AnalyticsResult SendEvent(string eventName, object parameters, int ver = 1, string prefix = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (parameters == null)
		{
			throw new ArgumentException("Cannot set parameters to null");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return SendEventWithLimit(eventName, parameters, ver, prefix);
	}

	public static AnalyticsResult SetEventEndPoint(string eventName, string endPoint, int ver = 1, string prefix = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (endPoint == null)
		{
			throw new ArgumentException("Cannot set parameters to null");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return SetEventWithLimitEndPoint(eventName, endPoint, ver, prefix);
	}

	public static AnalyticsResult SetEventPriority(string eventName, AnalyticsEventPriority eventPriority, int ver = 1, string prefix = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return SetEventWithLimitPriority(eventName, eventPriority, ver, prefix);
	}

	public static AnalyticsResult EnableEvent(string eventName, bool enabled, int ver = 1, string prefix = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return EnableEventWithLimit(eventName, enabled, ver, prefix);
	}

	public static AnalyticsResult IsEventEnabled(string eventName, int ver = 1, string prefix = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return IsEventWithLimitEnabled(eventName, ver, prefix);
	}
}
