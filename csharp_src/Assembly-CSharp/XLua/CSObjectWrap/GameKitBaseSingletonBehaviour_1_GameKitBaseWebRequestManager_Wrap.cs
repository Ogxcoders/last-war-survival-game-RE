using System;
using GameKit.Base;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameKitBaseSingletonBehaviour_1_GameKitBaseWebRequestManager_Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SingletonBehaviour<WebRequestManager>);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 1, 1);
		Utils.RegisterFunc(L, -3, "Release", _m_Release);
		Utils.RegisterFunc(L, -2, "updateMode", _g_get_updateMode);
		Utils.RegisterFunc(L, -1, "updateMode", _s_set_updateMode);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 1, 0);
		Utils.RegisterFunc(L, -4, "IsInstanceValid", _m_IsInstanceValid_xlua_st_);
		Utils.RegisterFunc(L, -4, "DestroyInstance", _m_DestroyInstance_xlua_st_);
		Utils.RegisterFunc(L, -2, "Instance", _g_get_Instance);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				SingletonBehaviour<WebRequestManager> o = new SingletonBehaviour<WebRequestManager>();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameKit.Base.SingletonBehaviour<GameKit.Base.WebRequestManager> constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInstanceValid_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = SingletonBehaviour<WebRequestManager>.IsInstanceValid();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyInstance_xlua_st_(IntPtr L)
	{
		try
		{
			SingletonBehaviour<WebRequestManager>.DestroyInstance();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Release(IntPtr L)
	{
		try
		{
			((SingletonBehaviour<WebRequestManager>)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Release();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Instance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SingletonBehaviour<WebRequestManager>.Instance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_updateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SingletonBehaviour<WebRequestManager> singletonBehaviour = (SingletonBehaviour<WebRequestManager>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, singletonBehaviour.updateMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_updateMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SingletonBehaviour<WebRequestManager> singletonBehaviour = (SingletonBehaviour<WebRequestManager>)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out SingletonBehaviour<WebRequestManager>.UpdateMode v);
			singletonBehaviour.updateMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
