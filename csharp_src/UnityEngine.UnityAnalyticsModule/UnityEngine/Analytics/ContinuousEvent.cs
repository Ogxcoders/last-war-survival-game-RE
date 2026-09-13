using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics;

[NativeHeader("Modules/UnityAnalytics/Public/UnityAnalytics.h")]
[ExcludeFromDocs]
[NativeHeader("Modules/UnityAnalytics/ContinuousEvent/Manager.h")]
[RequiredByNativeCode]
public class ContinuousEvent
{
	public static AnalyticsResult RegisterCollector<T>(string metricName, Func<T> del) where T : struct, IComparable<T>, IEquatable<T>
	{
		if (string.IsNullOrEmpty(metricName))
		{
			throw new ArgumentException("Cannot set metric name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return InternalRegisterCollector(typeof(T).ToString(), metricName, del);
	}

	public static AnalyticsResult SetEventHistogramThresholds<T>(string eventName, int count, T[] data, int ver = 1, string prefix = "") where T : struct, IComparable<T>, IEquatable<T>
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return InternalSetEventHistogramThresholds(typeof(T).ToString(), eventName, count, data, ver, prefix);
	}

	public static AnalyticsResult SetCustomEventHistogramThresholds<T>(string eventName, int count, T[] data) where T : struct, IComparable<T>, IEquatable<T>
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return InternalSetCustomEventHistogramThresholds(typeof(T).ToString(), eventName, count, data);
	}

	public static AnalyticsResult ConfigureCustomEvent(string customEventName, string metricName, float interval, float period, bool enabled = true)
	{
		if (string.IsNullOrEmpty(customEventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return InternalConfigureCustomEvent(customEventName, metricName, interval, period, enabled);
	}

	public static AnalyticsResult ConfigureEvent(string eventName, string metricName, float interval, float period, bool enabled = true, int ver = 1, string prefix = "")
	{
		if (string.IsNullOrEmpty(eventName))
		{
			throw new ArgumentException("Cannot set event name to an empty or null string");
		}
		if (!IsInitialized())
		{
			return AnalyticsResult.NotInitialized;
		}
		return InternalConfigureEvent(eventName, metricName, interval, period, enabled, ver, prefix);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("::GetUnityAnalytics().GetContinuousEventManager()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult InternalRegisterCollector(string type, string metricName, object collector);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("::GetUnityAnalytics().GetContinuousEventManager()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult InternalSetEventHistogramThresholds(string type, string eventName, int count, object data, int ver, string prefix);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("::GetUnityAnalytics().GetContinuousEventManager()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult InternalSetCustomEventHistogramThresholds(string type, string eventName, int count, object data);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("::GetUnityAnalytics().GetContinuousEventManager()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult InternalConfigureCustomEvent(string customEventName, string metricName, float interval, float period, bool enabled);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("::GetUnityAnalytics().GetContinuousEventManager()", StaticAccessorType.Dot)]
	private static extern AnalyticsResult InternalConfigureEvent(string eventName, string metricName, float interval, float period, bool enabled, int ver, string prefix);

	internal static bool IsInitialized()
	{
		return Analytics.IsInitialized();
	}
}
