using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine;

internal static class RemoteConfigSettingsHelper
{
	[RequiredByNativeCode]
	internal enum Tag
	{
		kUnknown,
		kIntVal,
		kInt64Val,
		kUInt64Val,
		kDoubleVal,
		kBoolVal,
		kStringVal,
		kArrayVal,
		kMixedArrayVal,
		kMapVal,
		kMaxTags
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetSafeMap(IntPtr m, string key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string[] GetSafeMapKeys(IntPtr m);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Tag[] GetSafeMapTypes(IntPtr m);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern long GetSafeNumber(IntPtr m, string key, long defaultValue);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern float GetSafeFloat(IntPtr m, string key, float defaultValue);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetSafeBool(IntPtr m, string key, bool defaultValue);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string GetSafeStringValue(IntPtr m, string key, string defaultValue);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetSafeArray(IntPtr m, string key);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern long GetSafeArraySize(IntPtr a);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetSafeArrayArray(IntPtr a, long i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern IntPtr GetSafeArrayMap(IntPtr a, long i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern Tag GetSafeArrayType(IntPtr a, long i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern long GetSafeNumberArray(IntPtr a, long i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern float GetSafeArrayFloat(IntPtr a, long i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetSafeArrayBool(IntPtr a, long i);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string GetSafeArrayStringValue(IntPtr a, long i);

	public static IDictionary<string, object> GetDictionary(IntPtr m, string key)
	{
		if (m == IntPtr.Zero)
		{
			return null;
		}
		if (!string.IsNullOrEmpty(key))
		{
			m = GetSafeMap(m, key);
			if (m == IntPtr.Zero)
			{
				return null;
			}
		}
		return GetDictionary(m);
	}

	internal static IDictionary<string, object> GetDictionary(IntPtr m)
	{
		if (m == IntPtr.Zero)
		{
			return null;
		}
		IDictionary<string, object> dictionary = new Dictionary<string, object>();
		Tag[] safeMapTypes = GetSafeMapTypes(m);
		string[] safeMapKeys = GetSafeMapKeys(m);
		for (int i = 0; i < safeMapKeys.Length; i++)
		{
			SetDictKeyType(m, dictionary, safeMapKeys[i], safeMapTypes[i]);
		}
		return dictionary;
	}

	internal static object GetArrayArrayEntries(IntPtr a, long i)
	{
		return GetArrayEntries(GetSafeArrayArray(a, i));
	}

	internal static IDictionary<string, object> GetArrayMapEntries(IntPtr a, long i)
	{
		return GetDictionary(GetSafeArrayMap(a, i));
	}

	internal static T[] GetArrayEntriesType<T>(IntPtr a, long size, Func<IntPtr, long, T> f)
	{
		T[] array = new T[size];
		for (long num = 0L; num < size; num++)
		{
			array[num] = f(a, num);
		}
		return array;
	}

	internal static object GetArrayEntries(IntPtr a)
	{
		long safeArraySize = GetSafeArraySize(a);
		if (safeArraySize == 0)
		{
			return null;
		}
		switch (GetSafeArrayType(a, 0L))
		{
		case Tag.kIntVal:
		case Tag.kInt64Val:
			return GetArrayEntriesType(a, safeArraySize, GetSafeNumberArray);
		case Tag.kDoubleVal:
			return GetArrayEntriesType(a, safeArraySize, GetSafeArrayFloat);
		case Tag.kBoolVal:
			return GetArrayEntriesType(a, safeArraySize, GetSafeArrayBool);
		case Tag.kStringVal:
			return GetArrayEntriesType(a, safeArraySize, GetSafeArrayStringValue);
		case Tag.kArrayVal:
			return GetArrayEntriesType(a, safeArraySize, GetArrayArrayEntries);
		case Tag.kMapVal:
			return GetArrayEntriesType(a, safeArraySize, GetArrayMapEntries);
		default:
			return null;
		}
	}

	internal static object GetMixedArrayEntries(IntPtr a)
	{
		long safeArraySize = GetSafeArraySize(a);
		if (safeArraySize == 0)
		{
			return null;
		}
		object[] array = new object[safeArraySize];
		for (long num = 0L; num < safeArraySize; num++)
		{
			switch (GetSafeArrayType(a, num))
			{
			case Tag.kIntVal:
			case Tag.kInt64Val:
				array[num] = GetSafeNumberArray(a, num);
				break;
			case Tag.kDoubleVal:
				array[num] = GetSafeArrayFloat(a, num);
				break;
			case Tag.kBoolVal:
				array[num] = GetSafeArrayBool(a, num);
				break;
			case Tag.kStringVal:
				array[num] = GetSafeArrayStringValue(a, num);
				break;
			case Tag.kArrayVal:
				array[num] = GetArrayArrayEntries(a, num);
				break;
			case Tag.kMapVal:
				array[num] = GetArrayMapEntries(a, num);
				break;
			}
		}
		return array;
	}

	internal static void SetDictKeyType(IntPtr m, IDictionary<string, object> dict, string key, Tag tag)
	{
		switch (tag)
		{
		case Tag.kIntVal:
		case Tag.kInt64Val:
			dict[key] = GetSafeNumber(m, key, 0L);
			break;
		case Tag.kDoubleVal:
			dict[key] = GetSafeFloat(m, key, 0f);
			break;
		case Tag.kBoolVal:
			dict[key] = GetSafeBool(m, key, defaultValue: false);
			break;
		case Tag.kStringVal:
			dict[key] = GetSafeStringValue(m, key, "");
			break;
		case Tag.kArrayVal:
			dict[key] = GetArrayEntries(GetSafeArray(m, key));
			break;
		case Tag.kMixedArrayVal:
			dict[key] = GetMixedArrayEntries(GetSafeArray(m, key));
			break;
		case Tag.kMapVal:
			dict[key] = GetDictionary(GetSafeMap(m, key));
			break;
		case Tag.kUInt64Val:
			break;
		}
	}
}
