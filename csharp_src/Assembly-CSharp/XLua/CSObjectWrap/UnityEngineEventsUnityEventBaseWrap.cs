using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineEventsUnityEventBaseWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UnityEventBase);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 0, 0);
		Utils.RegisterFunc(L, -3, "GetPersistentEventCount", _m_GetPersistentEventCount);
		Utils.RegisterFunc(L, -3, "GetPersistentTarget", _m_GetPersistentTarget);
		Utils.RegisterFunc(L, -3, "GetPersistentMethodName", _m_GetPersistentMethodName);
		Utils.RegisterFunc(L, -3, "SetPersistentListenerState", _m_SetPersistentListenerState);
		Utils.RegisterFunc(L, -3, "RemoveAllListeners", _m_RemoveAllListeners);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "GetValidMethodInfo", _m_GetValidMethodInfo_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Events.UnityEventBase does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPersistentEventCount(IntPtr L)
	{
		try
		{
			int persistentEventCount = ((UnityEventBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPersistentEventCount();
			Lua.xlua_pushinteger(L, persistentEventCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPersistentTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEventBase obj = (UnityEventBase)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			UnityEngine.Object persistentTarget = obj.GetPersistentTarget(index);
			objectTranslator.Push(L, persistentTarget);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPersistentMethodName(IntPtr L)
	{
		try
		{
			UnityEventBase obj = (UnityEventBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			string persistentMethodName = obj.GetPersistentMethodName(index);
			Lua.lua_pushstring(L, persistentMethodName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPersistentListenerState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UnityEventBase unityEventBase = (UnityEventBase)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out UnityEventCallState v);
			unityEventBase.SetPersistentListenerState(index, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAllListeners(IntPtr L)
	{
		try
		{
			((UnityEventBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveAllListeners();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			string str = ((UnityEventBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToString();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetValidMethodInfo_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			object obj = objectTranslator.GetObject(L, 1, typeof(object));
			string functionName = Lua.lua_tostring(L, 2);
			Type[] argumentTypes = (Type[])objectTranslator.GetObject(L, 3, typeof(Type[]));
			MethodInfo validMethodInfo = UnityEventBase.GetValidMethodInfo(obj, functionName, argumentTypes);
			objectTranslator.Push(L, validMethodInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			((UnityEventBase)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
