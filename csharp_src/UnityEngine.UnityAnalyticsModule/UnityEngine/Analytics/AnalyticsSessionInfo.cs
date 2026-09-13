using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Analytics;

[NativeHeader("Modules/UnityAnalytics/Public/UnityAnalytics.h")]
[RequiredByNativeCode]
[NativeHeader("UnityAnalyticsScriptingClasses.h")]
public static class AnalyticsSessionInfo
{
	public delegate void SessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged);

	public delegate void IdentityTokenChanged(string token);

	public static extern AnalyticsSessionState sessionState
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetPlayerSessionState")]
		get;
	}

	public static extern long sessionId
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetPlayerSessionId")]
		get;
	}

	public static extern long sessionCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetPlayerSessionCount")]
		get;
	}

	public static extern long sessionElapsedTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetPlayerSessionElapsedTime")]
		get;
	}

	public static extern bool sessionFirstRun
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetPlayerSessionFirstRun", false, true)]
		get;
	}

	public static extern string userId
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetUserId")]
		get;
	}

	public static string customUserId
	{
		get
		{
			if (!Analytics.IsInitialized())
			{
				return null;
			}
			return customUserIdInternal;
		}
		set
		{
			if (Analytics.IsInitialized())
			{
				customUserIdInternal = value;
			}
		}
	}

	public static string customDeviceId
	{
		get
		{
			if (!Analytics.IsInitialized())
			{
				return null;
			}
			return customDeviceIdInternal;
		}
		set
		{
			if (Analytics.IsInitialized())
			{
				customDeviceIdInternal = value;
			}
		}
	}

	public static string identityToken
	{
		get
		{
			if (!Analytics.IsInitialized())
			{
				return null;
			}
			return identityTokenInternal;
		}
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern string identityTokenInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetIdentityToken")]
		get;
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern string customUserIdInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetCustomUserId")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetCustomUserId")]
		set;
	}

	[StaticAccessor("GetUnityAnalytics()", StaticAccessorType.Dot)]
	private static extern string customDeviceIdInternal
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("GetCustomDeviceId")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		[NativeMethod("SetCustomDeviceId")]
		set;
	}

	public static event SessionStateChanged sessionStateChanged;

	public static event IdentityTokenChanged identityTokenChanged;

	[RequiredByNativeCode]
	internal static void CallSessionStateChanged(AnalyticsSessionState sessionState, long sessionId, long sessionElapsedTime, bool sessionChanged)
	{
		AnalyticsSessionInfo.sessionStateChanged?.Invoke(sessionState, sessionId, sessionElapsedTime, sessionChanged);
	}

	[RequiredByNativeCode]
	internal static void CallIdentityTokenChanged(string token)
	{
		AnalyticsSessionInfo.identityTokenChanged?.Invoke(token);
	}
}
