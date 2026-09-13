using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine;

[NativeHeader("UnityAnalyticsScriptingClasses.h")]
[NativeHeader("Modules/UnityAnalytics/RemoteSettings/RemoteSettings.h")]
public static class RemoteSettings
{
	public delegate void UpdatedEventHandler();

	public static event UpdatedEventHandler Updated;

	public static event Action BeforeFetchFromServer;

	public static event Action<bool, bool, int> Completed;

	[RequiredByNativeCode]
	internal static void RemoteSettingsUpdated(bool wasLastUpdatedFromServer)
	{
		RemoteSettings.Updated?.Invoke();
	}

	[RequiredByNativeCode]
	internal static void RemoteSettingsBeforeFetchFromServer()
	{
		RemoteSettings.BeforeFetchFromServer?.Invoke();
	}

	[RequiredByNativeCode]
	internal static void RemoteSettingsUpdateCompleted(bool wasLastUpdatedFromServer, bool settingsChanged, int response)
	{
		RemoteSettings.Completed?.Invoke(wasLastUpdatedFromServer, settingsChanged, response);
	}

	[Obsolete("Calling CallOnUpdate() is not necessary any more and should be removed. Use RemoteSettingsUpdated instead", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static void CallOnUpdate()
	{
		throw new NotSupportedException("Calling CallOnUpdate() is not necessary any more and should be removed.");
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void ForceUpdate();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool WasLastUpdatedFromServer();

	[ExcludeFromDocs]
	public static int GetInt(string key)
	{
		return GetInt(key, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int GetInt(string key, [UnityEngine.Internal.DefaultValue("0")] int defaultValue);

	[ExcludeFromDocs]
	public static long GetLong(string key)
	{
		return GetLong(key, 0L);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern long GetLong(string key, [UnityEngine.Internal.DefaultValue("0")] long defaultValue);

	[ExcludeFromDocs]
	public static float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern float GetFloat(string key, [UnityEngine.Internal.DefaultValue("0.0F")] float defaultValue);

	[ExcludeFromDocs]
	public static string GetString(string key)
	{
		return GetString(key, "");
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string GetString(string key, [UnityEngine.Internal.DefaultValue("\"\"")] string defaultValue);

	[ExcludeFromDocs]
	public static bool GetBool(string key)
	{
		return GetBool(key, defaultValue: false);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool GetBool(string key, [UnityEngine.Internal.DefaultValue("false")] bool defaultValue);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool HasKey(string key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern int GetCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern string[] GetKeys();

	public static T GetObject<T>(string key = "")
	{
		return (T)GetObject(typeof(T), key);
	}

	public static object GetObject(Type type, string key = "")
	{
		if ((object)type == null)
		{
			throw new ArgumentNullException("type");
		}
		if (type.IsAbstract || type.IsSubclassOf(typeof(Object)))
		{
			throw new ArgumentException("Cannot deserialize to new instances of type '" + type.Name + ".'");
		}
		return GetAsScriptingObject(type, null, key);
	}

	public static object GetObject(string key, object defaultValue)
	{
		if (defaultValue == null)
		{
			throw new ArgumentNullException("defaultValue");
		}
		Type type = defaultValue.GetType();
		if (type.IsAbstract || type.IsSubclassOf(typeof(Object)))
		{
			throw new ArgumentException("Cannot deserialize to new instances of type '" + type.Name + ".'");
		}
		return GetAsScriptingObject(type, defaultValue, key);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern object GetAsScriptingObject(Type t, object defaultValue, string key);

	public static IDictionary<string, object> GetDictionary(string key = "")
	{
		UseSafeLock();
		IDictionary<string, object> dictionary = RemoteConfigSettingsHelper.GetDictionary(GetSafeTopMap(), key);
		ReleaseSafeLock();
		return dictionary;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void UseSafeLock();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void ReleaseSafeLock();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetSafeTopMap();
}
