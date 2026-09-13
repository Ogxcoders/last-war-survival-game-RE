using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class AutoSyncActiveWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(AutoSyncActive);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 2, 2);
		Utils.RegisterFunc(L, -3, "SetCtrlAndTarget", _m_SetCtrlAndTarget);
		Utils.RegisterFunc(L, -2, "ctrl", _g_get_ctrl);
		Utils.RegisterFunc(L, -2, "target", _g_get_target);
		Utils.RegisterFunc(L, -1, "ctrl", _s_set_ctrl);
		Utils.RegisterFunc(L, -1, "target", _s_set_target);
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
				AutoSyncActive o = new AutoSyncActive();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to AutoSyncActive constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCtrlAndTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoSyncActive autoSyncActive = (AutoSyncActive)objectTranslator.FastGetCSObj(L, 1);
			GameObject ctrl = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			GameObject target = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			autoSyncActive.SetCtrlAndTarget(ctrl, target);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ctrl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoSyncActive autoSyncActive = (AutoSyncActive)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, autoSyncActive.ctrl);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			AutoSyncActive autoSyncActive = (AutoSyncActive)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, autoSyncActive.target);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ctrl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((AutoSyncActive)objectTranslator.FastGetCSObj(L, 1)).ctrl = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_target(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((AutoSyncActive)objectTranslator.FastGetCSObj(L, 1)).target = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
