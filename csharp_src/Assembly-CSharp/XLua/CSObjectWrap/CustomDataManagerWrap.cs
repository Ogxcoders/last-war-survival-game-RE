using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CustomDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CustomDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 4, 0);
		Utils.RegisterFunc(L, -3, "Release", _m_Release);
		Utils.RegisterFunc(L, -3, "Reset", _m_Reset);
		Utils.RegisterFunc(L, -2, "Player", _g_get_Player);
		Utils.RegisterFunc(L, -2, "Building", _g_get_Building);
		Utils.RegisterFunc(L, -2, "Fog", _g_get_Fog);
		Utils.RegisterFunc(L, -2, "Road", _g_get_Road);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				CustomDataManager o = new CustomDataManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CustomDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Release(IntPtr L)
	{
		try
		{
			((CustomDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Release();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset(IntPtr L)
	{
		try
		{
			((CustomDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Player(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomDataManager customDataManager = (CustomDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, customDataManager.Player);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Building(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomDataManager customDataManager = (CustomDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, customDataManager.Building);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Fog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomDataManager customDataManager = (CustomDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, customDataManager.Fog);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Road(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomDataManager customDataManager = (CustomDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, customDataManager.Road);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
