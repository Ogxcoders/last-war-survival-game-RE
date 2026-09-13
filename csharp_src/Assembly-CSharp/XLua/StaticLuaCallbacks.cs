using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using BitBenderGames;
using DG.Tweening;
using Framework.Utils.UnityEx;
using GameFramework.Localization;
using SuperScrollView;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Profiling;
using UnityEngine.Rendering.Universal;
using UnityEngine.Timeline;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua;

public class StaticLuaCallbacks
{
	internal lua_CSFunction GcMeta;

	internal lua_CSFunction ToStringMeta;

	internal lua_CSFunction EnumAndMeta;

	internal lua_CSFunction EnumOrMeta;

	internal lua_CSFunction StaticCSFunctionWraper;

	internal lua_CSFunction FixCSFunctionWraper;

	internal lua_CSFunction DelegateCtor;

	public static bool k_SampleLuaCallCS = Debug.isDebugBuild;

	private static CustomSampler _cs_FixCSFunction = CustomSampler.Create("FixCSFunction");

	public StaticLuaCallbacks()
	{
		GcMeta = LuaGC;
		ToStringMeta = ToString;
		EnumAndMeta = EnumAnd;
		EnumOrMeta = EnumOr;
		StaticCSFunctionWraper = StaticCSFunction;
		FixCSFunctionWraper = FixCSFunction;
		DelegateCtor = DelegateConstructor;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int EnumAnd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object obj = objectTranslator.FastGetCSObj(L, 1);
			object obj2 = objectTranslator.FastGetCSObj(L, 2);
			Type type = obj.GetType();
			if (!type.IsEnum() || type != obj2.GetType())
			{
				return Lua.luaL_error(L, "invalid argument for Enum BitwiseAnd");
			}
			objectTranslator.PushAny(L, Enum.ToObject(type, Convert.ToInt64(obj) & Convert.ToInt64(obj2)));
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in Enum BitwiseAnd:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int EnumOr(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object obj = objectTranslator.FastGetCSObj(L, 1);
			object obj2 = objectTranslator.FastGetCSObj(L, 2);
			Type type = obj.GetType();
			if (!type.IsEnum() || type != obj2.GetType())
			{
				return Lua.luaL_error(L, "invalid argument for Enum BitwiseOr");
			}
			objectTranslator.PushAny(L, Enum.ToObject(type, Convert.ToInt64(obj) | Convert.ToInt64(obj2)));
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in Enum BitwiseOr:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int StaticCSFunction(IntPtr L)
	{
		_ = k_SampleLuaCallCS;
		try
		{
			int result = ((lua_CSFunction)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, Lua.xlua_upvalueindex(1)))(L);
			_ = k_SampleLuaCallCS;
			return result;
		}
		catch (Exception ex)
		{
			_ = k_SampleLuaCallCS;
			return Lua.luaL_error(L, "c# exception in StaticCSFunction:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int FixCSFunction(IntPtr L)
	{
		_ = k_SampleLuaCallCS;
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int index = Lua.xlua_tointeger(L, Lua.xlua_upvalueindex(1));
			int result = objectTranslator.GetFixCSFunction(index)(L);
			_ = k_SampleLuaCallCS;
			return result;
		}
		catch (Exception ex)
		{
			_ = k_SampleLuaCallCS;
			return Lua.luaL_error(L, "c# exception in FixCSFunction:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int DelegateCall(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object obj = objectTranslator.FastGetCSObj(L, 1);
			if (obj == null || !(obj is Delegate))
			{
				return Lua.luaL_error(L, "trying to invoke a value that is not delegate nor callable");
			}
			return objectTranslator.methodWrapsCache.GetDelegateWrap(obj.GetType())(L);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in DelegateCall:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int LuaGC(IntPtr L)
	{
		try
		{
			int num = Lua.xlua_tocsobj_safe(L, 1);
			if (num != -1)
			{
				ObjectTranslatorPool.Instance.Find(L)?.collectObject(num);
			}
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in LuaGC:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ToString(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object obj = objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, (obj != null) ? (obj.ToString() + ": " + obj.GetHashCode()) : "<invalid c# object>");
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in ToString:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int DelegateCombine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = objectTranslator.FastGetCSObj(L, (Lua.lua_type(L, 1) == LuaTypes.LUA_TUSERDATA) ? 1 : 2).GetType();
			Delegate obj = objectTranslator.GetObject(L, 1, type) as Delegate;
			Delegate obj2 = objectTranslator.GetObject(L, 2, type) as Delegate;
			if ((object)obj == null || (object)obj2 == null)
			{
				return Lua.luaL_error(L, "one parameter must be a delegate, other one must be delegate or function");
			}
			objectTranslator.PushAny(L, Delegate.Combine(obj, obj2));
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in DelegateCombine:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int DelegateRemove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (!(objectTranslator.FastGetCSObj(L, 1) is Delegate obj))
			{
				return Lua.luaL_error(L, "#1 parameter must be a delegate");
			}
			if (!(objectTranslator.GetObject(L, 2, obj.GetType()) is Delegate value))
			{
				return Lua.luaL_error(L, "#2 parameter must be a delegate or a function ");
			}
			objectTranslator.PushAny(L, Delegate.Remove(obj, value));
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in DelegateRemove:" + ex);
		}
	}

	private static bool tryPrimitiveArrayGet(Type type, IntPtr L, object obj, int index)
	{
		bool result = true;
		if (type == typeof(int[]))
		{
			int[] array = obj as int[];
			Lua.xlua_pushinteger(L, array[index]);
		}
		else if (type == typeof(float[]))
		{
			float[] array2 = obj as float[];
			Lua.lua_pushnumber(L, array2[index]);
		}
		else if (type == typeof(double[]))
		{
			double[] array3 = obj as double[];
			Lua.lua_pushnumber(L, array3[index]);
		}
		else if (type == typeof(bool[]))
		{
			bool[] array4 = obj as bool[];
			Lua.lua_pushboolean(L, array4[index]);
		}
		else if (type == typeof(long[]))
		{
			long[] array5 = obj as long[];
			Lua.lua_pushint64(L, array5[index]);
		}
		else if (type == typeof(ulong[]))
		{
			ulong[] array6 = obj as ulong[];
			Lua.lua_pushuint64(L, array6[index]);
		}
		else if (type == typeof(sbyte[]))
		{
			sbyte[] array7 = obj as sbyte[];
			Lua.xlua_pushinteger(L, array7[index]);
		}
		else if (type == typeof(short[]))
		{
			short[] array8 = obj as short[];
			Lua.xlua_pushinteger(L, array8[index]);
		}
		else if (type == typeof(ushort[]))
		{
			ushort[] array9 = obj as ushort[];
			Lua.xlua_pushinteger(L, array9[index]);
		}
		else if (type == typeof(char[]))
		{
			char[] array10 = obj as char[];
			Lua.xlua_pushinteger(L, array10[index]);
		}
		else if (type == typeof(uint[]))
		{
			uint[] array11 = obj as uint[];
			Lua.xlua_pushuint(L, array11[index]);
		}
		else if (type == typeof(IntPtr[]))
		{
			IntPtr[] array12 = obj as IntPtr[];
			Lua.lua_pushlightuserdata(L, array12[index]);
		}
		else if (type == typeof(decimal[]))
		{
			decimal[] array13 = obj as decimal[];
			ObjectTranslatorPool.Instance.Find(L).PushDecimal(L, array13[index]);
		}
		else if (type == typeof(string[]))
		{
			string[] array14 = obj as string[];
			Lua.lua_pushstring(L, array14[index]);
		}
		else
		{
			result = false;
		}
		return result;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ArrayIndexer(IntPtr L)
	{
		_ = k_SampleLuaCallCS;
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array array = (Array)objectTranslator.FastGetCSObj(L, 1);
			if (array == null)
			{
				_ = k_SampleLuaCallCS;
				return Lua.luaL_error(L, "#1 parameter is not a array!");
			}
			int num = Lua.xlua_tointeger(L, 2);
			if (num >= array.Length)
			{
				_ = k_SampleLuaCallCS;
				return Lua.luaL_error(L, "index out of range! i =" + num + ", array.Length=" + array.Length);
			}
			Type type = array.GetType();
			if (tryPrimitiveArrayGet(type, L, array, num))
			{
				_ = k_SampleLuaCallCS;
				return 1;
			}
			if (InternalGlobals.genTryArrayGetPtr != null)
			{
				try
				{
					if (InternalGlobals.genTryArrayGetPtr(type, L, objectTranslator, array, num))
					{
						_ = k_SampleLuaCallCS;
						return 1;
					}
				}
				catch (Exception ex)
				{
					_ = k_SampleLuaCallCS;
					return Lua.luaL_error(L, "c# exception:" + ex.Message + ",stack:" + ex.StackTrace);
				}
			}
			object value = array.GetValue(num);
			objectTranslator.PushAny(L, value);
			_ = k_SampleLuaCallCS;
			return 1;
		}
		catch (Exception ex2)
		{
			_ = k_SampleLuaCallCS;
			return Lua.luaL_error(L, "c# exception in ArrayIndexer:" + ex2);
		}
	}

	public static bool TryPrimitiveArraySet(Type type, IntPtr L, object obj, int array_idx, int obj_idx)
	{
		bool result = true;
		LuaTypes luaTypes = Lua.lua_type(L, obj_idx);
		if (type == typeof(int[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as int[])[array_idx] = Lua.xlua_tointeger(L, obj_idx);
		}
		else if (type == typeof(float[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as float[])[array_idx] = (float)Lua.lua_tonumber(L, obj_idx);
		}
		else if (type == typeof(double[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as double[])[array_idx] = Lua.lua_tonumber(L, obj_idx);
		}
		else if (type == typeof(bool[]) && luaTypes == LuaTypes.LUA_TBOOLEAN)
		{
			(obj as bool[])[array_idx] = Lua.lua_toboolean(L, obj_idx);
		}
		else if (type == typeof(long[]) && Lua.lua_isint64(L, obj_idx))
		{
			(obj as long[])[array_idx] = Lua.lua_toint64(L, obj_idx);
		}
		else if (type == typeof(ulong[]) && Lua.lua_isuint64(L, obj_idx))
		{
			(obj as ulong[])[array_idx] = Lua.lua_touint64(L, obj_idx);
		}
		else if (type == typeof(sbyte[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as sbyte[])[array_idx] = (sbyte)Lua.xlua_tointeger(L, obj_idx);
		}
		else if (type == typeof(short[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as short[])[array_idx] = (short)Lua.xlua_tointeger(L, obj_idx);
		}
		else if (type == typeof(ushort[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as ushort[])[array_idx] = (ushort)Lua.xlua_tointeger(L, obj_idx);
		}
		else if (type == typeof(char[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as char[])[array_idx] = (char)Lua.xlua_tointeger(L, obj_idx);
		}
		else if (type == typeof(uint[]) && luaTypes == LuaTypes.LUA_TNUMBER)
		{
			(obj as uint[])[array_idx] = Lua.xlua_touint(L, obj_idx);
		}
		else if (type == typeof(IntPtr[]) && luaTypes == LuaTypes.LUA_TLIGHTUSERDATA)
		{
			(obj as IntPtr[])[array_idx] = Lua.lua_touserdata(L, obj_idx);
		}
		else if (type == typeof(decimal[]))
		{
			decimal[] array = obj as decimal[];
			if (luaTypes == LuaTypes.LUA_TNUMBER)
			{
				array[array_idx] = (decimal)Lua.lua_tonumber(L, obj_idx);
			}
			if (luaTypes == LuaTypes.LUA_TUSERDATA)
			{
				ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
				if (objectTranslator.IsDecimal(L, obj_idx))
				{
					objectTranslator.Get(L, obj_idx, out array[array_idx]);
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = false;
			}
		}
		else if (type == typeof(string[]) && luaTypes == LuaTypes.LUA_TSTRING)
		{
			(obj as string[])[array_idx] = Lua.lua_tostring(L, obj_idx);
		}
		else
		{
			result = false;
		}
		return result;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ArrayNewIndexer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Array array = (Array)objectTranslator.FastGetCSObj(L, 1);
			if (array == null)
			{
				return Lua.luaL_error(L, "#1 parameter is not a array!");
			}
			int num = Lua.xlua_tointeger(L, 2);
			if (num >= array.Length)
			{
				return Lua.luaL_error(L, "index out of range! i =" + num + ", array.Length=" + array.Length);
			}
			Type type = array.GetType();
			if (TryPrimitiveArraySet(type, L, array, num, 3))
			{
				return 0;
			}
			if (InternalGlobals.genTryArraySetPtr != null)
			{
				try
				{
					if (InternalGlobals.genTryArraySetPtr(type, L, objectTranslator, array, num, 3))
					{
						return 0;
					}
				}
				catch (Exception ex)
				{
					return Lua.luaL_error(L, "c# exception:" + ex.Message + ",stack:" + ex.StackTrace);
				}
			}
			object value = objectTranslator.GetObject(L, 3, type.GetElementType());
			array.SetValue(value, num);
			return 0;
		}
		catch (Exception ex2)
		{
			return Lua.luaL_error(L, "c# exception in ArrayNewIndexer:" + ex2);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ArrayLength(IntPtr L)
	{
		try
		{
			Array array = (Array)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, array.Length);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in ArrayLength:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int MetaFuncIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = objectTranslator.FastGetCSObj(L, 2) as Type;
			if (type == null)
			{
				return Lua.luaL_error(L, "#2 param need a System.Type!");
			}
			objectTranslator.GetTypeId(L, type);
			Lua.lua_pushvalue(L, 2);
			Lua.lua_rawget(L, 1);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in MetaFuncIndex:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int Panic(IntPtr L)
	{
		throw new LuaException($"unprotected error in call to Lua API ({Lua.lua_tostring(L, -1)})");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int Print(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			string text = string.Empty;
			if (Lua.xlua_getglobal(L, "tostring") != 0)
			{
				return Lua.luaL_error(L, "can not get tostring in print:");
			}
			for (int i = 1; i <= num; i++)
			{
				Lua.lua_pushvalue(L, -1);
				Lua.lua_pushvalue(L, i);
				if (Lua.lua_pcall(L, 1, 1, 0) != 0)
				{
					return Lua.lua_error(L);
				}
				text += Lua.lua_tostring(L, -1);
				if (i != num)
				{
					text += "\t";
				}
				Lua.lua_pop(L, 1);
			}
			Debug.Log("LUA: " + text);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in print:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int LoadSocketCore(IntPtr L)
	{
		return Lua.luaopen_socket_core(L);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int LoadCS(IntPtr L)
	{
		Lua.xlua_pushasciistring(L, "xlua_csharp_namespace");
		Lua.lua_rawget(L, LuaIndexes.LUA_REGISTRYINDEX);
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int LoadBuiltinLib(IntPtr L)
	{
		try
		{
			string text = Lua.lua_tostring(L, 1);
			if (ObjectTranslatorPool.Instance.Find(L).luaEnv.buildin_initer.TryGetValue(text, out var value))
			{
				Lua.lua_pushstdcallcfunction(L, value);
			}
			else
			{
				Lua.lua_pushstring(L, $"\n\tno such builtin lib '{text}'");
			}
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in LoadBuiltinLib:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int LoadFromResource(IntPtr L)
	{
		try
		{
			string text = Lua.lua_tostring(L, 1).Replace('.', '/') + ".lua";
			TextAsset textAsset = (TextAsset)Resources.Load(text);
			if (textAsset == null)
			{
				Lua.lua_pushstring(L, $"\n\tno such resource '{text}'");
			}
			else if (Lua.xluaL_loadbuffer(L, textAsset.bytes, textAsset.bytes.Length, "@" + text) != 0)
			{
				return Lua.luaL_error(L, $"error loading module {Lua.lua_tostring(L, 1)} from resource, {Lua.lua_tostring(L, -1)}");
			}
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in LoadFromResource:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int LoadFromStreamingAssetsPath(IntPtr L)
	{
		try
		{
			string text = Lua.lua_tostring(L, 1).Replace('.', '/') + ".lua";
			WWW wWW = new WWW(Application.streamingAssetsPath + "/" + text);
			while (!wWW.isDone && string.IsNullOrEmpty(wWW.error))
			{
			}
			Thread.Sleep(50);
			if (!string.IsNullOrEmpty(wWW.error))
			{
				Lua.lua_pushstring(L, $"\n\tno such file '{text}' in streamingAssetsPath!");
			}
			else
			{
				Debug.LogWarning("load lua file from StreamingAssets is obsolete, filename:" + text);
				if (Lua.xluaL_loadbuffer(L, wWW.bytes, wWW.bytes.Length, "@" + text) != 0)
				{
					return Lua.luaL_error(L, $"error loading module {Lua.lua_tostring(L, 1)} from streamingAssetsPath, {Lua.lua_tostring(L, -1)}");
				}
			}
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in LoadFromStreamingAssetsPath:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	internal static int LoadFromCustomLoaders(IntPtr L)
	{
		try
		{
			string text = Lua.lua_tostring(L, 1);
			foreach (LuaEnv.CustomLoader customLoader in ObjectTranslatorPool.Instance.Find(L).luaEnv.customLoaders)
			{
				string filepath = text;
				byte[] array = customLoader(ref filepath);
				if (array != null)
				{
					if (Lua.xluaL_loadbuffer(L, array, array.Length, "@" + filepath) != 0)
					{
						return Lua.luaL_error(L, $"error loading module {Lua.lua_tostring(L, 1)} from CustomLoader, {Lua.lua_tostring(L, -1)}");
					}
					return 1;
				}
			}
			Lua.lua_pushstring(L, $"\n\tno such file '{text}' in CustomLoaders!");
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in LoadFromCustomLoaders:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int LoadAssembly(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string text = Lua.lua_tostring(L, 1);
			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load(text);
			}
			catch (BadImageFormatException)
			{
			}
			if (assembly == null)
			{
				assembly = Assembly.Load(AssemblyName.GetAssemblyName(text));
			}
			if (assembly != null && !objectTranslator.assemblies.Contains(assembly))
			{
				objectTranslator.assemblies.Add(assembly);
			}
			return 0;
		}
		catch (Exception ex2)
		{
			return Lua.luaL_error(L, "c# exception in xlua.load_assembly:" + ex2);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ImportType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string className = Lua.lua_tostring(L, 1);
			Type type = objectTranslator.FindType(className);
			if (type != null)
			{
				if (objectTranslator.GetTypeId(L, type) < 0)
				{
					return Lua.luaL_error(L, "can not load type " + type);
				}
				Lua.lua_pushboolean(L, value: true);
			}
			else
			{
				Lua.lua_pushnil(L);
			}
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.import_type:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ImportGenericType(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num < 2)
			{
				return Lua.luaL_error(L, "import generic type need at lease 2 arguments");
			}
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			string text = Lua.lua_tostring(L, 1);
			if (text.EndsWith("<>"))
			{
				text = text.Substring(0, text.Length - 2);
			}
			Type type = objectTranslator.FindType(text + "`" + (num - 1));
			if (type == null || !type.IsGenericTypeDefinition())
			{
				Lua.lua_pushnil(L);
			}
			else
			{
				Type[] array = new Type[num - 1];
				for (int i = 2; i <= num; i++)
				{
					array[i - 2] = getType(L, objectTranslator, i);
					if (array[i - 2] == null)
					{
						return Lua.luaL_error(L, "param need a type");
					}
				}
				Type type2 = type.MakeGenericType(array);
				objectTranslator.GetTypeId(L, type2);
				objectTranslator.PushAny(L, type2);
			}
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.import_type:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int Cast(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 2, out Type v);
			if (v == null)
			{
				return Lua.luaL_error(L, "#2 param[" + Lua.lua_tostring(L, 2) + "]is not valid type indicator");
			}
			Lua.luaL_getmetatable(L, v.FullName);
			if (Lua.lua_isnil(L, -1))
			{
				return Lua.luaL_error(L, "no gen code for " + Lua.lua_tostring(L, 2));
			}
			Lua.lua_setmetatable(L, 1);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.cast:" + ex);
		}
	}

	private static Type getType(IntPtr L, ObjectTranslator translator, int idx)
	{
		if (Lua.lua_type(L, idx) == LuaTypes.LUA_TTABLE)
		{
			translator.Get(L, idx, out LuaTable v);
			return v.Get<Type>("UnderlyingSystemType");
		}
		if (Lua.lua_type(L, idx) == LuaTypes.LUA_TSTRING)
		{
			string className = Lua.lua_tostring(L, idx);
			return translator.FindType(className);
		}
		if (translator.GetObject(L, idx) is Type)
		{
			return translator.GetObject(L, idx) as Type;
		}
		return null;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int XLuaAccess(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = getType(L, objectTranslator, 1);
			object obj = null;
			if (type == null && Lua.lua_type(L, 1) == LuaTypes.LUA_TUSERDATA)
			{
				obj = objectTranslator.SafeGetCSObj(L, 1);
				if (obj == null)
				{
					return Lua.luaL_error(L, "xlua.access, #1 parameter must a type/c# object/string");
				}
				type = obj.GetType();
			}
			if (type == null)
			{
				return Lua.luaL_error(L, "xlua.access, can not find c# type");
			}
			string text = Lua.lua_tostring(L, 2);
			BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			if (Lua.lua_gettop(L) > 2)
			{
				FieldInfo field = type.GetField(text, bindingAttr);
				if (field != null)
				{
					field.SetValue(obj, objectTranslator.GetObject(L, 3, field.FieldType));
					return 0;
				}
				PropertyInfo property = type.GetProperty(text, bindingAttr);
				if (property != null)
				{
					property.SetValue(obj, objectTranslator.GetObject(L, 3, property.PropertyType), null);
					return 0;
				}
			}
			else
			{
				FieldInfo field2 = type.GetField(text, bindingAttr);
				if (field2 != null)
				{
					objectTranslator.PushAny(L, field2.GetValue(obj));
					return 1;
				}
				PropertyInfo property2 = type.GetProperty(text, bindingAttr);
				if (property2 != null)
				{
					objectTranslator.PushAny(L, property2.GetValue(obj, null));
					return 1;
				}
			}
			return Lua.luaL_error(L, "xlua.access, no field " + text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.access: " + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int XLuaPrivateAccessible(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = getType(L, objectTranslator, 1);
			if (type == null)
			{
				return Lua.luaL_error(L, "xlua.private_accessible, can not find c# type");
			}
			while (type != null)
			{
				objectTranslator.PrivateAccessible(L, type);
				type = type.BaseType();
			}
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.private_accessible: " + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int XLuaMetatableOperation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = getType(L, objectTranslator, 1);
			if (type == null)
			{
				return Lua.luaL_error(L, "xlua.metatable_operation, can not find c# type");
			}
			bool is_first = false;
			int typeId = objectTranslator.getTypeId(L, type, out is_first);
			int num = Lua.lua_gettop(L);
			switch (num)
			{
			case 1:
				Lua.xlua_rawgeti(L, LuaIndexes.LUA_REGISTRYINDEX, typeId);
				return 1;
			case 2:
				if (Lua.lua_type(L, 2) != LuaTypes.LUA_TTABLE)
				{
					return Lua.luaL_error(L, "argument #2 must be a table");
				}
				Lua.lua_pushnumber(L, typeId);
				Lua.xlua_rawseti(L, 2, 1L);
				Lua.xlua_rawseti(L, LuaIndexes.LUA_REGISTRYINDEX, typeId);
				return 0;
			default:
				return Lua.luaL_error(L, "invalid argument num for xlua.metatable_operation: " + num);
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.metatable_operation: " + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int DelegateConstructor(IntPtr L)
	{
		_ = k_SampleLuaCallCS;
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = getType(L, objectTranslator, 1);
			if (type == null || !typeof(Delegate).IsAssignableFrom(type))
			{
				_ = k_SampleLuaCallCS;
				return Lua.luaL_error(L, "delegate constructor: #1 argument must be a Delegate's type");
			}
			objectTranslator.PushAny(L, objectTranslator.GetObject(L, 2, type));
			_ = k_SampleLuaCallCS;
			return 1;
		}
		catch (Exception ex)
		{
			_ = k_SampleLuaCallCS;
			return Lua.luaL_error(L, "c# exception in delegate constructor: " + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ToFunction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out MethodBase v);
			if (v == null)
			{
				return Lua.luaL_error(L, "ToFunction: #1 argument must be a MethodBase");
			}
			objectTranslator.PushFixCSFunction(L, objectTranslator.methodWrapsCache._GenMethodWrap(v.DeclaringType, v.Name, new MethodBase[1] { v }).Call);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in ToFunction: " + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int GenericMethodWraper(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, Lua.xlua_upvalueindex(1), out MethodInfo v);
			int num = Lua.lua_gettop(L);
			Type[] array = new Type[num];
			for (int i = 0; i < num; i++)
			{
				Type type = getType(L, objectTranslator, i + 1);
				if (type == null)
				{
					return Lua.luaL_error(L, "param #" + (i + 1) + " is not a type");
				}
				array[i] = type;
			}
			MethodInfo methodInfo = v.MakeGenericMethod(array);
			objectTranslator.PushFixCSFunction(L, objectTranslator.methodWrapsCache._GenMethodWrap(methodInfo.DeclaringType, methodInfo.Name, new MethodBase[1] { methodInfo }).Call);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in GenericMethodWraper: " + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int GetGenericMethod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Type type = getType(L, objectTranslator, 1);
			if (type == null)
			{
				return Lua.luaL_error(L, "xlua.get_generic_method, can not find c# type");
			}
			string text = Lua.lua_tostring(L, 2);
			if (string.IsNullOrEmpty(text))
			{
				return Lua.luaL_error(L, "xlua.get_generic_method, #2 param need a string");
			}
			List<MethodInfo> list = new List<MethodInfo>();
			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (MethodInfo methodInfo in methods)
			{
				if (methodInfo.Name == text && methodInfo.IsGenericMethodDefinition)
				{
					list.Add(methodInfo);
				}
			}
			int index = 0;
			if (list.Count == 0)
			{
				Lua.lua_pushnil(L);
			}
			else
			{
				if (Lua.lua_isinteger(L, 3))
				{
					index = Lua.xlua_tointeger(L, 3);
				}
				objectTranslator.PushAny(L, list[index]);
				Lua.lua_pushstdcallcfunction(L, GenericMethodWraper, 1);
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in xlua.get_generic_method: " + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	public static int ReleaseCsObject(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).ReleaseCSObj(L, 1);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception in ReleaseCsObject: " + ex);
		}
	}

	internal static bool __tryArrayGet(Type type, IntPtr L, ObjectTranslator translator, object obj, int index)
	{
		if (type == typeof(TouchInfo[]))
		{
			TouchInfo[] array = obj as TouchInfo[];
			translator.PushFrameworkUtilsUnityExTouchInfo(L, array[index]);
			return true;
		}
		if (type == typeof(Vector2[]))
		{
			Vector2[] array2 = obj as Vector2[];
			translator.PushUnityEngineVector2(L, array2[index]);
			return true;
		}
		if (type == typeof(Vector3[]))
		{
			Vector3[] array3 = obj as Vector3[];
			translator.PushUnityEngineVector3(L, array3[index]);
			return true;
		}
		if (type == typeof(Vector4[]))
		{
			Vector4[] array4 = obj as Vector4[];
			translator.PushUnityEngineVector4(L, array4[index]);
			return true;
		}
		if (type == typeof(Color[]))
		{
			Color[] array5 = obj as Color[];
			translator.PushUnityEngineColor(L, array5[index]);
			return true;
		}
		if (type == typeof(Quaternion[]))
		{
			Quaternion[] array6 = obj as Quaternion[];
			translator.PushUnityEngineQuaternion(L, array6[index]);
			return true;
		}
		if (type == typeof(Ray[]))
		{
			Ray[] array7 = obj as Ray[];
			translator.PushUnityEngineRay(L, array7[index]);
			return true;
		}
		if (type == typeof(Bounds[]))
		{
			Bounds[] array8 = obj as Bounds[];
			translator.PushUnityEngineBounds(L, array8[index]);
			return true;
		}
		if (type == typeof(Ray2D[]))
		{
			Ray2D[] array9 = obj as Ray2D[];
			translator.PushUnityEngineRay2D(L, array9[index]);
			return true;
		}
		if (type == typeof(BindingFlags[]))
		{
			BindingFlags[] array10 = obj as BindingFlags[];
			translator.PushSystemReflectionBindingFlags(L, array10[index]);
			return true;
		}
		if (type == typeof(KeyCode[]))
		{
			KeyCode[] array11 = obj as KeyCode[];
			translator.PushUnityEngineKeyCode(L, array11[index]);
			return true;
		}
		if (type == typeof(Camera.GateFitMode[]))
		{
			Camera.GateFitMode[] array12 = obj as Camera.GateFitMode[];
			translator.PushUnityEngineCameraGateFitMode(L, array12[index]);
			return true;
		}
		if (type == typeof(Camera.FieldOfViewAxis[]))
		{
			Camera.FieldOfViewAxis[] array13 = obj as Camera.FieldOfViewAxis[];
			translator.PushUnityEngineCameraFieldOfViewAxis(L, array13[index]);
			return true;
		}
		if (type == typeof(Camera.StereoscopicEye[]))
		{
			Camera.StereoscopicEye[] array14 = obj as Camera.StereoscopicEye[];
			translator.PushUnityEngineCameraStereoscopicEye(L, array14[index]);
			return true;
		}
		if (type == typeof(Camera.MonoOrStereoscopicEye[]))
		{
			Camera.MonoOrStereoscopicEye[] array15 = obj as Camera.MonoOrStereoscopicEye[];
			translator.PushUnityEngineCameraMonoOrStereoscopicEye(L, array15[index]);
			return true;
		}
		if (type == typeof(Ease[]))
		{
			Ease[] array16 = obj as Ease[];
			translator.PushDGTweeningEase(L, array16[index]);
			return true;
		}
		if (type == typeof(DOTweenAnimation.AnimationType[]))
		{
			DOTweenAnimation.AnimationType[] array17 = obj as DOTweenAnimation.AnimationType[];
			translator.PushDGTweeningDOTweenAnimationAnimationType(L, array17[index]);
			return true;
		}
		if (type == typeof(DOTweenAnimation.TargetType[]))
		{
			DOTweenAnimation.TargetType[] array18 = obj as DOTweenAnimation.TargetType[];
			translator.PushDGTweeningDOTweenAnimationTargetType(L, array18[index]);
			return true;
		}
		if (type == typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions[]))
		{
			TextMeshProUGUIEx.HorizontalAlignmentOptions[] array19 = obj as TextMeshProUGUIEx.HorizontalAlignmentOptions[];
			translator.PushTextMeshProUGUIExHorizontalAlignmentOptions(L, array19[index]);
			return true;
		}
		if (type == typeof(TextMeshProUGUIEx.VerticalAlignmentOptions[]))
		{
			TextMeshProUGUIEx.VerticalAlignmentOptions[] array20 = obj as TextMeshProUGUIEx.VerticalAlignmentOptions[];
			translator.PushTextMeshProUGUIExVerticalAlignmentOptions(L, array20[index]);
			return true;
		}
		if (type == typeof(RectTransform.Edge[]))
		{
			RectTransform.Edge[] array21 = obj as RectTransform.Edge[];
			translator.PushUnityEngineRectTransformEdge(L, array21[index]);
			return true;
		}
		if (type == typeof(RectTransform.Axis[]))
		{
			RectTransform.Axis[] array22 = obj as RectTransform.Axis[];
			translator.PushUnityEngineRectTransformAxis(L, array22[index]);
			return true;
		}
		if (type == typeof(PointerEventData.InputButton[]))
		{
			PointerEventData.InputButton[] array23 = obj as PointerEventData.InputButton[];
			translator.PushUnityEngineEventSystemsPointerEventDataInputButton(L, array23[index]);
			return true;
		}
		if (type == typeof(PointerEventData.FramePressState[]))
		{
			PointerEventData.FramePressState[] array24 = obj as PointerEventData.FramePressState[];
			translator.PushUnityEngineEventSystemsPointerEventDataFramePressState(L, array24[index]);
			return true;
		}
		if (type == typeof(RenderTextureFormat[]))
		{
			RenderTextureFormat[] array25 = obj as RenderTextureFormat[];
			translator.PushUnityEngineRenderTextureFormat(L, array25[index]);
			return true;
		}
		if (type == typeof(Space[]))
		{
			Space[] array26 = obj as Space[];
			translator.PushUnityEngineSpace(L, array26[index]);
			return true;
		}
		if (type == typeof(QueryTriggerInteraction[]))
		{
			QueryTriggerInteraction[] array27 = obj as QueryTriggerInteraction[];
			translator.PushUnityEngineQueryTriggerInteraction(L, array27[index]);
			return true;
		}
		if (type == typeof(AntialiasingMode[]))
		{
			AntialiasingMode[] array28 = obj as AntialiasingMode[];
			translator.PushUnityEngineRenderingUniversalAntialiasingMode(L, array28[index]);
			return true;
		}
		if (type == typeof(Selectable.Transition[]))
		{
			Selectable.Transition[] array29 = obj as Selectable.Transition[];
			translator.PushUnityEngineUISelectableTransition(L, array29[index]);
			return true;
		}
		if (type == typeof(InputField.ContentType[]))
		{
			InputField.ContentType[] array30 = obj as InputField.ContentType[];
			translator.PushUnityEngineUIInputFieldContentType(L, array30[index]);
			return true;
		}
		if (type == typeof(InputField.InputType[]))
		{
			InputField.InputType[] array31 = obj as InputField.InputType[];
			translator.PushUnityEngineUIInputFieldInputType(L, array31[index]);
			return true;
		}
		if (type == typeof(InputField.CharacterValidation[]))
		{
			InputField.CharacterValidation[] array32 = obj as InputField.CharacterValidation[];
			translator.PushUnityEngineUIInputFieldCharacterValidation(L, array32[index]);
			return true;
		}
		if (type == typeof(InputField.LineType[]))
		{
			InputField.LineType[] array33 = obj as InputField.LineType[];
			translator.PushUnityEngineUIInputFieldLineType(L, array33[index]);
			return true;
		}
		if (type == typeof(Image.Type[]))
		{
			Image.Type[] array34 = obj as Image.Type[];
			translator.PushUnityEngineUIImageType(L, array34[index]);
			return true;
		}
		if (type == typeof(Image.FillMethod[]))
		{
			Image.FillMethod[] array35 = obj as Image.FillMethod[];
			translator.PushUnityEngineUIImageFillMethod(L, array35[index]);
			return true;
		}
		if (type == typeof(Image.OriginHorizontal[]))
		{
			Image.OriginHorizontal[] array36 = obj as Image.OriginHorizontal[];
			translator.PushUnityEngineUIImageOriginHorizontal(L, array36[index]);
			return true;
		}
		if (type == typeof(Image.OriginVertical[]))
		{
			Image.OriginVertical[] array37 = obj as Image.OriginVertical[];
			translator.PushUnityEngineUIImageOriginVertical(L, array37[index]);
			return true;
		}
		if (type == typeof(Image.Origin90[]))
		{
			Image.Origin90[] array38 = obj as Image.Origin90[];
			translator.PushUnityEngineUIImageOrigin90(L, array38[index]);
			return true;
		}
		if (type == typeof(Image.Origin180[]))
		{
			Image.Origin180[] array39 = obj as Image.Origin180[];
			translator.PushUnityEngineUIImageOrigin180(L, array39[index]);
			return true;
		}
		if (type == typeof(Image.Origin360[]))
		{
			Image.Origin360[] array40 = obj as Image.Origin360[];
			translator.PushUnityEngineUIImageOrigin360(L, array40[index]);
			return true;
		}
		if (type == typeof(ScrollRect.MovementType[]))
		{
			ScrollRect.MovementType[] array41 = obj as ScrollRect.MovementType[];
			translator.PushUnityEngineUIScrollRectMovementType(L, array41[index]);
			return true;
		}
		if (type == typeof(ScrollRect.ScrollbarVisibility[]))
		{
			ScrollRect.ScrollbarVisibility[] array42 = obj as ScrollRect.ScrollbarVisibility[];
			translator.PushUnityEngineUIScrollRectScrollbarVisibility(L, array42[index]);
			return true;
		}
		if (type == typeof(Slider.Direction[]))
		{
			Slider.Direction[] array43 = obj as Slider.Direction[];
			translator.PushUnityEngineUISliderDirection(L, array43[index]);
			return true;
		}
		if (type == typeof(Toggle.ToggleTransition[]))
		{
			Toggle.ToggleTransition[] array44 = obj as Toggle.ToggleTransition[];
			translator.PushUnityEngineUIToggleToggleTransition(L, array44[index]);
			return true;
		}
		if (type == typeof(GridLayoutGroup.Corner[]))
		{
			GridLayoutGroup.Corner[] array45 = obj as GridLayoutGroup.Corner[];
			translator.PushUnityEngineUIGridLayoutGroupCorner(L, array45[index]);
			return true;
		}
		if (type == typeof(GridLayoutGroup.Axis[]))
		{
			GridLayoutGroup.Axis[] array46 = obj as GridLayoutGroup.Axis[];
			translator.PushUnityEngineUIGridLayoutGroupAxis(L, array46[index]);
			return true;
		}
		if (type == typeof(GridLayoutGroup.Constraint[]))
		{
			GridLayoutGroup.Constraint[] array47 = obj as GridLayoutGroup.Constraint[];
			translator.PushUnityEngineUIGridLayoutGroupConstraint(L, array47[index]);
			return true;
		}
		if (type == typeof(ContentSizeFitter.FitMode[]))
		{
			ContentSizeFitter.FitMode[] array48 = obj as ContentSizeFitter.FitMode[];
			translator.PushUnityEngineUIContentSizeFitterFitMode(L, array48[index]);
			return true;
		}
		if (type == typeof(SuperTextMesh.Alignment[]))
		{
			SuperTextMesh.Alignment[] array49 = obj as SuperTextMesh.Alignment[];
			translator.PushSuperTextMeshAlignment(L, array49[index]);
			return true;
		}
		if (type == typeof(AnimatorCullingMode[]))
		{
			AnimatorCullingMode[] array50 = obj as AnimatorCullingMode[];
			translator.PushUnityEngineAnimatorCullingMode(L, array50[index]);
			return true;
		}
		if (type == typeof(TextAnchor[]))
		{
			TextAnchor[] array51 = obj as TextAnchor[];
			translator.PushUnityEngineTextAnchor(L, array51[index]);
			return true;
		}
		if (type == typeof(ScrollView.MovementType[]))
		{
			ScrollView.MovementType[] array52 = obj as ScrollView.MovementType[];
			translator.PushScrollViewMovementType(L, array52[index]);
			return true;
		}
		if (type == typeof(ScrollView.ScrollbarVisibility[]))
		{
			ScrollView.ScrollbarVisibility[] array53 = obj as ScrollView.ScrollbarVisibility[];
			translator.PushScrollViewScrollbarVisibility(L, array53[index]);
			return true;
		}
		if (type == typeof(ScrollView.ScrollViewLayoutType[]))
		{
			ScrollView.ScrollViewLayoutType[] array54 = obj as ScrollView.ScrollViewLayoutType[];
			translator.PushScrollViewScrollViewLayoutType(L, array54[index]);
			return true;
		}
		if (type == typeof(TouchPhase[]))
		{
			TouchPhase[] array55 = obj as TouchPhase[];
			translator.PushUnityEngineTouchPhase(L, array55[index]);
			return true;
		}
		if (type == typeof(MobileTouchCamera.State[]))
		{
			MobileTouchCamera.State[] array56 = obj as MobileTouchCamera.State[];
			translator.PushBitBenderGamesMobileTouchCameraState(L, array56[index]);
			return true;
		}
		if (type == typeof(Language[]))
		{
			Language[] array57 = obj as Language[];
			translator.PushGameFrameworkLocalizationLanguage(L, array57[index]);
			return true;
		}
		if (type == typeof(GameDefines.CityLabelColorType[]))
		{
			GameDefines.CityLabelColorType[] array58 = obj as GameDefines.CityLabelColorType[];
			translator.PushGameDefinesCityLabelColorType(L, array58[index]);
			return true;
		}
		if (type == typeof(GameDefines.BuildConnectRoadDirection[]))
		{
			GameDefines.BuildConnectRoadDirection[] array59 = obj as GameDefines.BuildConnectRoadDirection[];
			translator.PushGameDefinesBuildConnectRoadDirection(L, array59[index]);
			return true;
		}
		if (type == typeof(GameDefines.DirectionType[]))
		{
			GameDefines.DirectionType[] array60 = obj as GameDefines.DirectionType[];
			translator.PushGameDefinesDirectionType(L, array60[index]);
			return true;
		}
		if (type == typeof(ResourceManager.PreloadType[]))
		{
			ResourceManager.PreloadType[] array61 = obj as ResourceManager.PreloadType[];
			translator.PushResourceManagerPreloadType(L, array61[index]);
			return true;
		}
		if (type == typeof(LODType[]))
		{
			LODType[] array62 = obj as LODType[];
			translator.PushLODType(L, array62[index]);
			return true;
		}
		if (type == typeof(SceneManager.SceneID[]))
		{
			SceneManager.SceneID[] array63 = obj as SceneManager.SceneID[];
			translator.PushSceneManagerSceneID(L, array63[index]);
			return true;
		}
		if (type == typeof(CityBuilding.BuildSceneType[]))
		{
			CityBuilding.BuildSceneType[] array64 = obj as CityBuilding.BuildSceneType[];
			translator.PushCityBuildingBuildSceneType(L, array64[index]);
			return true;
		}
		if (type == typeof(ModelManager.ModelObjectType[]))
		{
			ModelManager.ModelObjectType[] array65 = obj as ModelManager.ModelObjectType[];
			translator.PushModelManagerModelObjectType(L, array65[index]);
			return true;
		}
		if (type == typeof(FakeModelManager.TempRoadType[]))
		{
			FakeModelManager.TempRoadType[] array66 = obj as FakeModelManager.TempRoadType[];
			translator.PushFakeModelManagerTempRoadType(L, array66[index]);
			return true;
		}
		if (type == typeof(WorldMarchDataManager.BattleWordType[]))
		{
			WorldMarchDataManager.BattleWordType[] array67 = obj as WorldMarchDataManager.BattleWordType[];
			translator.PushWorldMarchDataManagerBattleWordType(L, array67[index]);
			return true;
		}
		if (type == typeof(NewQueueState[]))
		{
			NewQueueState[] array68 = obj as NewQueueState[];
			translator.PushNewQueueState(L, array68[index]);
			return true;
		}
		if (type == typeof(ResourceType[]))
		{
			ResourceType[] array69 = obj as ResourceType[];
			translator.PushResourceType(L, array69[index]);
			return true;
		}
		if (type == typeof(BuildingState[]))
		{
			BuildingState[] array70 = obj as BuildingState[];
			translator.PushBuildingState(L, array70[index]);
			return true;
		}
		if (type == typeof(PlaceBuildType[]))
		{
			PlaceBuildType[] array71 = obj as PlaceBuildType[];
			translator.PushPlaceBuildType(L, array71[index]);
			return true;
		}
		if (type == typeof(MarchStatus[]))
		{
			MarchStatus[] array72 = obj as MarchStatus[];
			translator.PushMarchStatus(L, array72[index]);
			return true;
		}
		if (type == typeof(ClipCaps[]))
		{
			ClipCaps[] array73 = obj as ClipCaps[];
			translator.PushUnityEngineTimelineClipCaps(L, array73[index]);
			return true;
		}
		if (type == typeof(TextAlignmentOptions[]))
		{
			TextAlignmentOptions[] array74 = obj as TextAlignmentOptions[];
			translator.PushTMProTextAlignmentOptions(L, array74[index]);
			return true;
		}
		if (type == typeof(TMP_InputField.ContentType[]))
		{
			TMP_InputField.ContentType[] array75 = obj as TMP_InputField.ContentType[];
			translator.PushTMProTMP_InputFieldContentType(L, array75[index]);
			return true;
		}
		if (type == typeof(TMP_InputField.InputType[]))
		{
			TMP_InputField.InputType[] array76 = obj as TMP_InputField.InputType[];
			translator.PushTMProTMP_InputFieldInputType(L, array76[index]);
			return true;
		}
		if (type == typeof(TMP_InputField.CharacterValidation[]))
		{
			TMP_InputField.CharacterValidation[] array77 = obj as TMP_InputField.CharacterValidation[];
			translator.PushTMProTMP_InputFieldCharacterValidation(L, array77[index]);
			return true;
		}
		if (type == typeof(TMP_InputField.LineType[]))
		{
			TMP_InputField.LineType[] array78 = obj as TMP_InputField.LineType[];
			translator.PushTMProTMP_InputFieldLineType(L, array78[index]);
			return true;
		}
		if (type == typeof(TMP_InputFieldEx.HorizontalAlignmentOptions[]))
		{
			TMP_InputFieldEx.HorizontalAlignmentOptions[] array79 = obj as TMP_InputFieldEx.HorizontalAlignmentOptions[];
			translator.PushTMProTMP_InputFieldExHorizontalAlignmentOptions(L, array79[index]);
			return true;
		}
		if (type == typeof(InstanceRequest.State[]))
		{
			InstanceRequest.State[] array80 = obj as InstanceRequest.State[];
			translator.PushInstanceRequestState(L, array80[index]);
			return true;
		}
		if (type == typeof(NewMarchType[]))
		{
			NewMarchType[] array81 = obj as NewMarchType[];
			translator.PushNewMarchType(L, array81[index]);
			return true;
		}
		if (type == typeof(WorldPointType[]))
		{
			WorldPointType[] array82 = obj as WorldPointType[];
			translator.PushWorldPointType(L, array82[index]);
			return true;
		}
		if (type == typeof(ListItemArrangeType[]))
		{
			ListItemArrangeType[] array83 = obj as ListItemArrangeType[];
			translator.PushSuperScrollViewListItemArrangeType(L, array83[index]);
			return true;
		}
		if (type == typeof(URLGroupType[]))
		{
			URLGroupType[] array84 = obj as URLGroupType[];
			translator.PushURLGroupType(L, array84[index]);
			return true;
		}
		if (type == typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType[]))
		{
			MeteoriteWorldEffectPlayer.FragmentData.FragmentType[] array85 = obj as MeteoriteWorldEffectPlayer.FragmentData.FragmentType[];
			translator.PushMeteoriteWorldEffectPlayerFragmentDataFragmentType(L, array85[index]);
			return true;
		}
		if (type == typeof(WorldMeteoritePoint.MeteoritePointState[]))
		{
			WorldMeteoritePoint.MeteoritePointState[] array86 = obj as WorldMeteoritePoint.MeteoritePointState[];
			translator.PushWorldMeteoritePointMeteoritePointState(L, array86[index]);
			return true;
		}
		if (type == typeof(BattleColliderUtils.ColliderType[]))
		{
			BattleColliderUtils.ColliderType[] array87 = obj as BattleColliderUtils.ColliderType[];
			translator.PushBattleColliderUtilsColliderType(L, array87[index]);
			return true;
		}
		if (type == typeof(ViewSkinProPropertyRecorder.CodeType[]))
		{
			ViewSkinProPropertyRecorder.CodeType[] array88 = obj as ViewSkinProPropertyRecorder.CodeType[];
			translator.PushViewSkinProPropertyRecorderCodeType(L, array88[index]);
			return true;
		}
		if (type == typeof(FOWSystem.LOSChecks[]))
		{
			FOWSystem.LOSChecks[] array89 = obj as FOWSystem.LOSChecks[];
			translator.PushFOWSystemLOSChecks(L, array89[index]);
			return true;
		}
		if (type == typeof(FOWSystem.State[]))
		{
			FOWSystem.State[] array90 = obj as FOWSystem.State[];
			translator.PushFOWSystemState(L, array90[index]);
			return true;
		}
		if (type == typeof(ObjectPoolTag[]))
		{
			ObjectPoolTag[] array91 = obj as ObjectPoolTag[];
			translator.PushObjectPoolTag(L, array91[index]);
			return true;
		}
		if (type == typeof(ObjectPoolTagGroup[]))
		{
			ObjectPoolTagGroup[] array92 = obj as ObjectPoolTagGroup[];
			translator.PushObjectPoolTagGroup(L, array92[index]);
			return true;
		}
		if (type == typeof(DeviceLevel[]))
		{
			DeviceLevel[] array93 = obj as DeviceLevel[];
			translator.PushDeviceLevel(L, array93[index]);
			return true;
		}
		if (type == typeof(PlayerType[]))
		{
			PlayerType[] array94 = obj as PlayerType[];
			translator.PushPlayerType(L, array94[index]);
			return true;
		}
		return false;
	}

	internal static bool __tryArraySet(Type type, IntPtr L, ObjectTranslator translator, object obj, int array_idx, int obj_idx)
	{
		if (type == typeof(TouchInfo[]))
		{
			TouchInfo[] array = obj as TouchInfo[];
			translator.Get(L, obj_idx, out array[array_idx]);
			return true;
		}
		if (type == typeof(Vector2[]))
		{
			Vector2[] array2 = obj as Vector2[];
			translator.Get(L, obj_idx, out array2[array_idx]);
			return true;
		}
		if (type == typeof(Vector3[]))
		{
			Vector3[] array3 = obj as Vector3[];
			translator.Get(L, obj_idx, out array3[array_idx]);
			return true;
		}
		if (type == typeof(Vector4[]))
		{
			Vector4[] array4 = obj as Vector4[];
			translator.Get(L, obj_idx, out array4[array_idx]);
			return true;
		}
		if (type == typeof(Color[]))
		{
			Color[] array5 = obj as Color[];
			translator.Get(L, obj_idx, out array5[array_idx]);
			return true;
		}
		if (type == typeof(Quaternion[]))
		{
			Quaternion[] array6 = obj as Quaternion[];
			translator.Get(L, obj_idx, out array6[array_idx]);
			return true;
		}
		if (type == typeof(Ray[]))
		{
			Ray[] array7 = obj as Ray[];
			translator.Get(L, obj_idx, out array7[array_idx]);
			return true;
		}
		if (type == typeof(Bounds[]))
		{
			Bounds[] array8 = obj as Bounds[];
			translator.Get(L, obj_idx, out array8[array_idx]);
			return true;
		}
		if (type == typeof(Ray2D[]))
		{
			Ray2D[] array9 = obj as Ray2D[];
			translator.Get(L, obj_idx, out array9[array_idx]);
			return true;
		}
		if (type == typeof(BindingFlags[]))
		{
			BindingFlags[] array10 = obj as BindingFlags[];
			translator.Get(L, obj_idx, out array10[array_idx]);
			return true;
		}
		if (type == typeof(KeyCode[]))
		{
			KeyCode[] array11 = obj as KeyCode[];
			translator.Get(L, obj_idx, out array11[array_idx]);
			return true;
		}
		if (type == typeof(Camera.GateFitMode[]))
		{
			Camera.GateFitMode[] array12 = obj as Camera.GateFitMode[];
			translator.Get(L, obj_idx, out array12[array_idx]);
			return true;
		}
		if (type == typeof(Camera.FieldOfViewAxis[]))
		{
			Camera.FieldOfViewAxis[] array13 = obj as Camera.FieldOfViewAxis[];
			translator.Get(L, obj_idx, out array13[array_idx]);
			return true;
		}
		if (type == typeof(Camera.StereoscopicEye[]))
		{
			Camera.StereoscopicEye[] array14 = obj as Camera.StereoscopicEye[];
			translator.Get(L, obj_idx, out array14[array_idx]);
			return true;
		}
		if (type == typeof(Camera.MonoOrStereoscopicEye[]))
		{
			Camera.MonoOrStereoscopicEye[] array15 = obj as Camera.MonoOrStereoscopicEye[];
			translator.Get(L, obj_idx, out array15[array_idx]);
			return true;
		}
		if (type == typeof(Ease[]))
		{
			Ease[] array16 = obj as Ease[];
			translator.Get(L, obj_idx, out array16[array_idx]);
			return true;
		}
		if (type == typeof(DOTweenAnimation.AnimationType[]))
		{
			DOTweenAnimation.AnimationType[] array17 = obj as DOTweenAnimation.AnimationType[];
			translator.Get(L, obj_idx, out array17[array_idx]);
			return true;
		}
		if (type == typeof(DOTweenAnimation.TargetType[]))
		{
			DOTweenAnimation.TargetType[] array18 = obj as DOTweenAnimation.TargetType[];
			translator.Get(L, obj_idx, out array18[array_idx]);
			return true;
		}
		if (type == typeof(TextMeshProUGUIEx.HorizontalAlignmentOptions[]))
		{
			TextMeshProUGUIEx.HorizontalAlignmentOptions[] array19 = obj as TextMeshProUGUIEx.HorizontalAlignmentOptions[];
			translator.Get(L, obj_idx, out array19[array_idx]);
			return true;
		}
		if (type == typeof(TextMeshProUGUIEx.VerticalAlignmentOptions[]))
		{
			TextMeshProUGUIEx.VerticalAlignmentOptions[] array20 = obj as TextMeshProUGUIEx.VerticalAlignmentOptions[];
			translator.Get(L, obj_idx, out array20[array_idx]);
			return true;
		}
		if (type == typeof(RectTransform.Edge[]))
		{
			RectTransform.Edge[] array21 = obj as RectTransform.Edge[];
			translator.Get(L, obj_idx, out array21[array_idx]);
			return true;
		}
		if (type == typeof(RectTransform.Axis[]))
		{
			RectTransform.Axis[] array22 = obj as RectTransform.Axis[];
			translator.Get(L, obj_idx, out array22[array_idx]);
			return true;
		}
		if (type == typeof(PointerEventData.InputButton[]))
		{
			PointerEventData.InputButton[] array23 = obj as PointerEventData.InputButton[];
			translator.Get(L, obj_idx, out array23[array_idx]);
			return true;
		}
		if (type == typeof(PointerEventData.FramePressState[]))
		{
			PointerEventData.FramePressState[] array24 = obj as PointerEventData.FramePressState[];
			translator.Get(L, obj_idx, out array24[array_idx]);
			return true;
		}
		if (type == typeof(RenderTextureFormat[]))
		{
			RenderTextureFormat[] array25 = obj as RenderTextureFormat[];
			translator.Get(L, obj_idx, out array25[array_idx]);
			return true;
		}
		if (type == typeof(Space[]))
		{
			Space[] array26 = obj as Space[];
			translator.Get(L, obj_idx, out array26[array_idx]);
			return true;
		}
		if (type == typeof(QueryTriggerInteraction[]))
		{
			QueryTriggerInteraction[] array27 = obj as QueryTriggerInteraction[];
			translator.Get(L, obj_idx, out array27[array_idx]);
			return true;
		}
		if (type == typeof(AntialiasingMode[]))
		{
			AntialiasingMode[] array28 = obj as AntialiasingMode[];
			translator.Get(L, obj_idx, out array28[array_idx]);
			return true;
		}
		if (type == typeof(Selectable.Transition[]))
		{
			Selectable.Transition[] array29 = obj as Selectable.Transition[];
			translator.Get(L, obj_idx, out array29[array_idx]);
			return true;
		}
		if (type == typeof(InputField.ContentType[]))
		{
			InputField.ContentType[] array30 = obj as InputField.ContentType[];
			translator.Get(L, obj_idx, out array30[array_idx]);
			return true;
		}
		if (type == typeof(InputField.InputType[]))
		{
			InputField.InputType[] array31 = obj as InputField.InputType[];
			translator.Get(L, obj_idx, out array31[array_idx]);
			return true;
		}
		if (type == typeof(InputField.CharacterValidation[]))
		{
			InputField.CharacterValidation[] array32 = obj as InputField.CharacterValidation[];
			translator.Get(L, obj_idx, out array32[array_idx]);
			return true;
		}
		if (type == typeof(InputField.LineType[]))
		{
			InputField.LineType[] array33 = obj as InputField.LineType[];
			translator.Get(L, obj_idx, out array33[array_idx]);
			return true;
		}
		if (type == typeof(Image.Type[]))
		{
			Image.Type[] array34 = obj as Image.Type[];
			translator.Get(L, obj_idx, out array34[array_idx]);
			return true;
		}
		if (type == typeof(Image.FillMethod[]))
		{
			Image.FillMethod[] array35 = obj as Image.FillMethod[];
			translator.Get(L, obj_idx, out array35[array_idx]);
			return true;
		}
		if (type == typeof(Image.OriginHorizontal[]))
		{
			Image.OriginHorizontal[] array36 = obj as Image.OriginHorizontal[];
			translator.Get(L, obj_idx, out array36[array_idx]);
			return true;
		}
		if (type == typeof(Image.OriginVertical[]))
		{
			Image.OriginVertical[] array37 = obj as Image.OriginVertical[];
			translator.Get(L, obj_idx, out array37[array_idx]);
			return true;
		}
		if (type == typeof(Image.Origin90[]))
		{
			Image.Origin90[] array38 = obj as Image.Origin90[];
			translator.Get(L, obj_idx, out array38[array_idx]);
			return true;
		}
		if (type == typeof(Image.Origin180[]))
		{
			Image.Origin180[] array39 = obj as Image.Origin180[];
			translator.Get(L, obj_idx, out array39[array_idx]);
			return true;
		}
		if (type == typeof(Image.Origin360[]))
		{
			Image.Origin360[] array40 = obj as Image.Origin360[];
			translator.Get(L, obj_idx, out array40[array_idx]);
			return true;
		}
		if (type == typeof(ScrollRect.MovementType[]))
		{
			ScrollRect.MovementType[] array41 = obj as ScrollRect.MovementType[];
			translator.Get(L, obj_idx, out array41[array_idx]);
			return true;
		}
		if (type == typeof(ScrollRect.ScrollbarVisibility[]))
		{
			ScrollRect.ScrollbarVisibility[] array42 = obj as ScrollRect.ScrollbarVisibility[];
			translator.Get(L, obj_idx, out array42[array_idx]);
			return true;
		}
		if (type == typeof(Slider.Direction[]))
		{
			Slider.Direction[] array43 = obj as Slider.Direction[];
			translator.Get(L, obj_idx, out array43[array_idx]);
			return true;
		}
		if (type == typeof(Toggle.ToggleTransition[]))
		{
			Toggle.ToggleTransition[] array44 = obj as Toggle.ToggleTransition[];
			translator.Get(L, obj_idx, out array44[array_idx]);
			return true;
		}
		if (type == typeof(GridLayoutGroup.Corner[]))
		{
			GridLayoutGroup.Corner[] array45 = obj as GridLayoutGroup.Corner[];
			translator.Get(L, obj_idx, out array45[array_idx]);
			return true;
		}
		if (type == typeof(GridLayoutGroup.Axis[]))
		{
			GridLayoutGroup.Axis[] array46 = obj as GridLayoutGroup.Axis[];
			translator.Get(L, obj_idx, out array46[array_idx]);
			return true;
		}
		if (type == typeof(GridLayoutGroup.Constraint[]))
		{
			GridLayoutGroup.Constraint[] array47 = obj as GridLayoutGroup.Constraint[];
			translator.Get(L, obj_idx, out array47[array_idx]);
			return true;
		}
		if (type == typeof(ContentSizeFitter.FitMode[]))
		{
			ContentSizeFitter.FitMode[] array48 = obj as ContentSizeFitter.FitMode[];
			translator.Get(L, obj_idx, out array48[array_idx]);
			return true;
		}
		if (type == typeof(SuperTextMesh.Alignment[]))
		{
			SuperTextMesh.Alignment[] array49 = obj as SuperTextMesh.Alignment[];
			translator.Get(L, obj_idx, out array49[array_idx]);
			return true;
		}
		if (type == typeof(AnimatorCullingMode[]))
		{
			AnimatorCullingMode[] array50 = obj as AnimatorCullingMode[];
			translator.Get(L, obj_idx, out array50[array_idx]);
			return true;
		}
		if (type == typeof(TextAnchor[]))
		{
			TextAnchor[] array51 = obj as TextAnchor[];
			translator.Get(L, obj_idx, out array51[array_idx]);
			return true;
		}
		if (type == typeof(ScrollView.MovementType[]))
		{
			ScrollView.MovementType[] array52 = obj as ScrollView.MovementType[];
			translator.Get(L, obj_idx, out array52[array_idx]);
			return true;
		}
		if (type == typeof(ScrollView.ScrollbarVisibility[]))
		{
			ScrollView.ScrollbarVisibility[] array53 = obj as ScrollView.ScrollbarVisibility[];
			translator.Get(L, obj_idx, out array53[array_idx]);
			return true;
		}
		if (type == typeof(ScrollView.ScrollViewLayoutType[]))
		{
			ScrollView.ScrollViewLayoutType[] array54 = obj as ScrollView.ScrollViewLayoutType[];
			translator.Get(L, obj_idx, out array54[array_idx]);
			return true;
		}
		if (type == typeof(TouchPhase[]))
		{
			TouchPhase[] array55 = obj as TouchPhase[];
			translator.Get(L, obj_idx, out array55[array_idx]);
			return true;
		}
		if (type == typeof(MobileTouchCamera.State[]))
		{
			MobileTouchCamera.State[] array56 = obj as MobileTouchCamera.State[];
			translator.Get(L, obj_idx, out array56[array_idx]);
			return true;
		}
		if (type == typeof(Language[]))
		{
			Language[] array57 = obj as Language[];
			translator.Get(L, obj_idx, out array57[array_idx]);
			return true;
		}
		if (type == typeof(GameDefines.CityLabelColorType[]))
		{
			GameDefines.CityLabelColorType[] array58 = obj as GameDefines.CityLabelColorType[];
			translator.Get(L, obj_idx, out array58[array_idx]);
			return true;
		}
		if (type == typeof(GameDefines.BuildConnectRoadDirection[]))
		{
			GameDefines.BuildConnectRoadDirection[] array59 = obj as GameDefines.BuildConnectRoadDirection[];
			translator.Get(L, obj_idx, out array59[array_idx]);
			return true;
		}
		if (type == typeof(GameDefines.DirectionType[]))
		{
			GameDefines.DirectionType[] array60 = obj as GameDefines.DirectionType[];
			translator.Get(L, obj_idx, out array60[array_idx]);
			return true;
		}
		if (type == typeof(ResourceManager.PreloadType[]))
		{
			ResourceManager.PreloadType[] array61 = obj as ResourceManager.PreloadType[];
			translator.Get(L, obj_idx, out array61[array_idx]);
			return true;
		}
		if (type == typeof(LODType[]))
		{
			LODType[] array62 = obj as LODType[];
			translator.Get(L, obj_idx, out array62[array_idx]);
			return true;
		}
		if (type == typeof(SceneManager.SceneID[]))
		{
			SceneManager.SceneID[] array63 = obj as SceneManager.SceneID[];
			translator.Get(L, obj_idx, out array63[array_idx]);
			return true;
		}
		if (type == typeof(CityBuilding.BuildSceneType[]))
		{
			CityBuilding.BuildSceneType[] array64 = obj as CityBuilding.BuildSceneType[];
			translator.Get(L, obj_idx, out array64[array_idx]);
			return true;
		}
		if (type == typeof(ModelManager.ModelObjectType[]))
		{
			ModelManager.ModelObjectType[] array65 = obj as ModelManager.ModelObjectType[];
			translator.Get(L, obj_idx, out array65[array_idx]);
			return true;
		}
		if (type == typeof(FakeModelManager.TempRoadType[]))
		{
			FakeModelManager.TempRoadType[] array66 = obj as FakeModelManager.TempRoadType[];
			translator.Get(L, obj_idx, out array66[array_idx]);
			return true;
		}
		if (type == typeof(WorldMarchDataManager.BattleWordType[]))
		{
			WorldMarchDataManager.BattleWordType[] array67 = obj as WorldMarchDataManager.BattleWordType[];
			translator.Get(L, obj_idx, out array67[array_idx]);
			return true;
		}
		if (type == typeof(NewQueueState[]))
		{
			NewQueueState[] array68 = obj as NewQueueState[];
			translator.Get(L, obj_idx, out array68[array_idx]);
			return true;
		}
		if (type == typeof(ResourceType[]))
		{
			ResourceType[] array69 = obj as ResourceType[];
			translator.Get(L, obj_idx, out array69[array_idx]);
			return true;
		}
		if (type == typeof(BuildingState[]))
		{
			BuildingState[] array70 = obj as BuildingState[];
			translator.Get(L, obj_idx, out array70[array_idx]);
			return true;
		}
		if (type == typeof(PlaceBuildType[]))
		{
			PlaceBuildType[] array71 = obj as PlaceBuildType[];
			translator.Get(L, obj_idx, out array71[array_idx]);
			return true;
		}
		if (type == typeof(MarchStatus[]))
		{
			MarchStatus[] array72 = obj as MarchStatus[];
			translator.Get(L, obj_idx, out array72[array_idx]);
			return true;
		}
		if (type == typeof(ClipCaps[]))
		{
			ClipCaps[] array73 = obj as ClipCaps[];
			translator.Get(L, obj_idx, out array73[array_idx]);
			return true;
		}
		if (type == typeof(TextAlignmentOptions[]))
		{
			TextAlignmentOptions[] array74 = obj as TextAlignmentOptions[];
			translator.Get(L, obj_idx, out array74[array_idx]);
			return true;
		}
		if (type == typeof(TMP_InputField.ContentType[]))
		{
			TMP_InputField.ContentType[] array75 = obj as TMP_InputField.ContentType[];
			translator.Get(L, obj_idx, out array75[array_idx]);
			return true;
		}
		if (type == typeof(TMP_InputField.InputType[]))
		{
			TMP_InputField.InputType[] array76 = obj as TMP_InputField.InputType[];
			translator.Get(L, obj_idx, out array76[array_idx]);
			return true;
		}
		if (type == typeof(TMP_InputField.CharacterValidation[]))
		{
			TMP_InputField.CharacterValidation[] array77 = obj as TMP_InputField.CharacterValidation[];
			translator.Get(L, obj_idx, out array77[array_idx]);
			return true;
		}
		if (type == typeof(TMP_InputField.LineType[]))
		{
			TMP_InputField.LineType[] array78 = obj as TMP_InputField.LineType[];
			translator.Get(L, obj_idx, out array78[array_idx]);
			return true;
		}
		if (type == typeof(TMP_InputFieldEx.HorizontalAlignmentOptions[]))
		{
			TMP_InputFieldEx.HorizontalAlignmentOptions[] array79 = obj as TMP_InputFieldEx.HorizontalAlignmentOptions[];
			translator.Get(L, obj_idx, out array79[array_idx]);
			return true;
		}
		if (type == typeof(InstanceRequest.State[]))
		{
			InstanceRequest.State[] array80 = obj as InstanceRequest.State[];
			translator.Get(L, obj_idx, out array80[array_idx]);
			return true;
		}
		if (type == typeof(NewMarchType[]))
		{
			NewMarchType[] array81 = obj as NewMarchType[];
			translator.Get(L, obj_idx, out array81[array_idx]);
			return true;
		}
		if (type == typeof(WorldPointType[]))
		{
			WorldPointType[] array82 = obj as WorldPointType[];
			translator.Get(L, obj_idx, out array82[array_idx]);
			return true;
		}
		if (type == typeof(ListItemArrangeType[]))
		{
			ListItemArrangeType[] array83 = obj as ListItemArrangeType[];
			translator.Get(L, obj_idx, out array83[array_idx]);
			return true;
		}
		if (type == typeof(URLGroupType[]))
		{
			URLGroupType[] array84 = obj as URLGroupType[];
			translator.Get(L, obj_idx, out array84[array_idx]);
			return true;
		}
		if (type == typeof(MeteoriteWorldEffectPlayer.FragmentData.FragmentType[]))
		{
			MeteoriteWorldEffectPlayer.FragmentData.FragmentType[] array85 = obj as MeteoriteWorldEffectPlayer.FragmentData.FragmentType[];
			translator.Get(L, obj_idx, out array85[array_idx]);
			return true;
		}
		if (type == typeof(WorldMeteoritePoint.MeteoritePointState[]))
		{
			WorldMeteoritePoint.MeteoritePointState[] array86 = obj as WorldMeteoritePoint.MeteoritePointState[];
			translator.Get(L, obj_idx, out array86[array_idx]);
			return true;
		}
		if (type == typeof(BattleColliderUtils.ColliderType[]))
		{
			BattleColliderUtils.ColliderType[] array87 = obj as BattleColliderUtils.ColliderType[];
			translator.Get(L, obj_idx, out array87[array_idx]);
			return true;
		}
		if (type == typeof(ViewSkinProPropertyRecorder.CodeType[]))
		{
			ViewSkinProPropertyRecorder.CodeType[] array88 = obj as ViewSkinProPropertyRecorder.CodeType[];
			translator.Get(L, obj_idx, out array88[array_idx]);
			return true;
		}
		if (type == typeof(FOWSystem.LOSChecks[]))
		{
			FOWSystem.LOSChecks[] array89 = obj as FOWSystem.LOSChecks[];
			translator.Get(L, obj_idx, out array89[array_idx]);
			return true;
		}
		if (type == typeof(FOWSystem.State[]))
		{
			FOWSystem.State[] array90 = obj as FOWSystem.State[];
			translator.Get(L, obj_idx, out array90[array_idx]);
			return true;
		}
		if (type == typeof(ObjectPoolTag[]))
		{
			ObjectPoolTag[] array91 = obj as ObjectPoolTag[];
			translator.Get(L, obj_idx, out array91[array_idx]);
			return true;
		}
		if (type == typeof(ObjectPoolTagGroup[]))
		{
			ObjectPoolTagGroup[] array92 = obj as ObjectPoolTagGroup[];
			translator.Get(L, obj_idx, out array92[array_idx]);
			return true;
		}
		if (type == typeof(DeviceLevel[]))
		{
			DeviceLevel[] array93 = obj as DeviceLevel[];
			translator.Get(L, obj_idx, out array93[array_idx]);
			return true;
		}
		if (type == typeof(PlayerType[]))
		{
			PlayerType[] array94 = obj as PlayerType[];
			translator.Get(L, obj_idx, out array94[array_idx]);
			return true;
		}
		return false;
	}
}
