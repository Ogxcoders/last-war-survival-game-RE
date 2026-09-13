using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine;

[StructLayout(LayoutKind.Sequential)]
[ExcludeFromDocs]
[NativeHeader("UnityAnalyticsScriptingClasses.h")]
[NativeHeader("Modules/UnityAnalytics/RemoteSettings/RemoteSettings.h")]
public class RemoteConfigSettings : IDisposable
{
	[NonSerialized]
	internal IntPtr m_Ptr;

	public event Action<bool> Updated;

	private RemoteConfigSettings()
	{
	}

	public RemoteConfigSettings(string configKey)
	{
		m_Ptr = Internal_Create(this, configKey);
		this.Updated = null;
	}

	~RemoteConfigSettings()
	{
		Destroy();
	}

	private void Destroy()
	{
		if (m_Ptr != IntPtr.Zero)
		{
			Internal_Destroy(m_Ptr);
			m_Ptr = IntPtr.Zero;
		}
	}

	public void Dispose()
	{
		Destroy();
		GC.SuppressFinalize(this);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr Internal_Create(RemoteConfigSettings rcs, string configKey);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[ThreadSafe]
	internal static extern void Internal_Destroy(IntPtr ptr);

	[RequiredByNativeCode]
	internal static void RemoteConfigSettingsUpdated(RemoteConfigSettings rcs, bool wasLastUpdatedFromServer)
	{
		rcs.Updated?.Invoke(wasLastUpdatedFromServer);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool QueueConfig(string name, object param, int ver = 1, string prefix = "");

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern bool SendDeviceInfoInConfigRequest();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void AddSessionTag(string tag);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void ForceUpdate();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool WasLastUpdatedFromServer();

	[ExcludeFromDocs]
	public int GetInt(string key)
	{
		return GetInt(key, 0);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern int GetInt(string key, [DefaultValue("0")] int defaultValue);

	[ExcludeFromDocs]
	public long GetLong(string key)
	{
		return GetLong(key, 0L);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern long GetLong(string key, [DefaultValue("0")] long defaultValue);

	[ExcludeFromDocs]
	public float GetFloat(string key)
	{
		return GetFloat(key, 0f);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern float GetFloat(string key, [DefaultValue("0.0F")] float defaultValue);

	[ExcludeFromDocs]
	public string GetString(string key)
	{
		return GetString(key, "");
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern string GetString(string key, [DefaultValue("\"\"")] string defaultValue);

	[ExcludeFromDocs]
	public bool GetBool(string key)
	{
		return GetBool(key, defaultValue: false);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool GetBool(string key, [DefaultValue("false")] bool defaultValue);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern bool HasKey(string key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern int GetCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern string[] GetKeys();

	public T GetObject<T>(string key = "")
	{
		return (T)GetObject(typeof(T), key);
	}

	public object GetObject(Type type, string key = "")
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

	public object GetObject(string key, object defaultValue)
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
	internal extern object GetAsScriptingObject(Type t, object defaultValue, string key);

	public IDictionary<string, object> GetDictionary(string key = "")
	{
		UseSafeLock();
		IDictionary<string, object> dictionary = RemoteConfigSettingsHelper.GetDictionary(GetSafeTopMap(), key);
		ReleaseSafeLock();
		return dictionary;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void UseSafeLock();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern void ReleaseSafeLock();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal extern IntPtr GetSafeTopMap();
}
