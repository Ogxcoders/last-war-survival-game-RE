using System;
using System.Collections.Generic;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineUIToggleGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ToggleGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 1, 1);
		Utils.RegisterFunc(L, -3, "NotifyToggleOn", _m_NotifyToggleOn);
		Utils.RegisterFunc(L, -3, "UnregisterToggle", _m_UnregisterToggle);
		Utils.RegisterFunc(L, -3, "RegisterToggle", _m_RegisterToggle);
		Utils.RegisterFunc(L, -3, "EnsureValidState", _m_EnsureValidState);
		Utils.RegisterFunc(L, -3, "AnyTogglesOn", _m_AnyTogglesOn);
		Utils.RegisterFunc(L, -3, "ActiveToggles", _m_ActiveToggles);
		Utils.RegisterFunc(L, -3, "SetAllTogglesOff", _m_SetAllTogglesOff);
		Utils.RegisterFunc(L, -2, "allowSwitchOff", _g_get_allowSwitchOff);
		Utils.RegisterFunc(L, -1, "allowSwitchOff", _s_set_allowSwitchOff);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.UI.ToggleGroup does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NotifyToggleOn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ToggleGroup toggleGroup = (ToggleGroup)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Toggle>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Toggle toggle = (Toggle)objectTranslator.GetObject(L, 2, typeof(Toggle));
				bool sendCallback = Lua.lua_toboolean(L, 3);
				toggleGroup.NotifyToggleOn(toggle, sendCallback);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Toggle>(L, 2))
			{
				Toggle toggle2 = (Toggle)objectTranslator.GetObject(L, 2, typeof(Toggle));
				toggleGroup.NotifyToggleOn(toggle2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.ToggleGroup.NotifyToggleOn!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterToggle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ToggleGroup toggleGroup = (ToggleGroup)objectTranslator.FastGetCSObj(L, 1);
			Toggle toggle = (Toggle)objectTranslator.GetObject(L, 2, typeof(Toggle));
			toggleGroup.UnregisterToggle(toggle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterToggle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ToggleGroup toggleGroup = (ToggleGroup)objectTranslator.FastGetCSObj(L, 1);
			Toggle toggle = (Toggle)objectTranslator.GetObject(L, 2, typeof(Toggle));
			toggleGroup.RegisterToggle(toggle);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnsureValidState(IntPtr L)
	{
		try
		{
			((ToggleGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnsureValidState();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AnyTogglesOn(IntPtr L)
	{
		try
		{
			bool value = ((ToggleGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AnyTogglesOn();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ActiveToggles(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			IEnumerable<Toggle> o = ((ToggleGroup)objectTranslator.FastGetCSObj(L, 1)).ActiveToggles();
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAllTogglesOff(IntPtr L)
	{
		try
		{
			ToggleGroup toggleGroup = (ToggleGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool allTogglesOff = Lua.lua_toboolean(L, 2);
				toggleGroup.SetAllTogglesOff(allTogglesOff);
				return 0;
			}
			if (num == 1)
			{
				toggleGroup.SetAllTogglesOff();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.UI.ToggleGroup.SetAllTogglesOff!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowSwitchOff(IntPtr L)
	{
		try
		{
			ToggleGroup toggleGroup = (ToggleGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, toggleGroup.allowSwitchOff);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowSwitchOff(IntPtr L)
	{
		try
		{
			((ToggleGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowSwitchOff = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
