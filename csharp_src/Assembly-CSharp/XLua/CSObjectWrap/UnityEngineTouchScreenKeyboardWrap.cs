using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineTouchScreenKeyboardWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TouchScreenKeyboard);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 9, 5);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "active", _g_get_active);
		Utils.RegisterFunc(L, -2, "status", _g_get_status);
		Utils.RegisterFunc(L, -2, "characterLimit", _g_get_characterLimit);
		Utils.RegisterFunc(L, -2, "canGetSelection", _g_get_canGetSelection);
		Utils.RegisterFunc(L, -2, "canSetSelection", _g_get_canSetSelection);
		Utils.RegisterFunc(L, -2, "selection", _g_get_selection);
		Utils.RegisterFunc(L, -2, "type", _g_get_type);
		Utils.RegisterFunc(L, -2, "targetDisplay", _g_get_targetDisplay);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "active", _s_set_active);
		Utils.RegisterFunc(L, -1, "characterLimit", _s_set_characterLimit);
		Utils.RegisterFunc(L, -1, "selection", _s_set_selection);
		Utils.RegisterFunc(L, -1, "targetDisplay", _s_set_targetDisplay);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 5, 1);
		Utils.RegisterFunc(L, -4, "Open", _m_Open_xlua_st_);
		Utils.RegisterFunc(L, -2, "isSupported", _g_get_isSupported);
		Utils.RegisterFunc(L, -2, "isInPlaceEditingAllowed", _g_get_isInPlaceEditingAllowed);
		Utils.RegisterFunc(L, -2, "hideInput", _g_get_hideInput);
		Utils.RegisterFunc(L, -2, "area", _g_get_area);
		Utils.RegisterFunc(L, -2, "visible", _g_get_visible);
		Utils.RegisterFunc(L, -1, "hideInput", _s_set_hideInput);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 9 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && (Lua.lua_isnil(L, 8) || Lua.lua_type(L, 8) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				string text = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out TouchScreenKeyboardType v);
				TouchScreenKeyboard o = new TouchScreenKeyboard(autocorrection: Lua.lua_toboolean(L, 4), multiline: Lua.lua_toboolean(L, 5), secure: Lua.lua_toboolean(L, 6), alert: Lua.lua_toboolean(L, 7), textPlaceholder: Lua.lua_tostring(L, 8), characterLimit: Lua.xlua_tointeger(L, 9), text: text, keyboardType: v);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.TouchScreenKeyboard constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Open_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				TouchScreenKeyboard o = TouchScreenKeyboard.Open(Lua.lua_tostring(L, 1));
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2))
			{
				string text = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v);
				TouchScreenKeyboard o2 = TouchScreenKeyboard.Open(text, v);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string text2 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v2);
				TouchScreenKeyboard o3 = TouchScreenKeyboard.Open(autocorrection: Lua.lua_toboolean(L, 3), text: text2, keyboardType: v2);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				string text3 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v3);
				TouchScreenKeyboard o4 = TouchScreenKeyboard.Open(autocorrection: Lua.lua_toboolean(L, 3), multiline: Lua.lua_toboolean(L, 4), text: text3, keyboardType: v3);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				string text4 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v4);
				TouchScreenKeyboard o5 = TouchScreenKeyboard.Open(autocorrection: Lua.lua_toboolean(L, 3), multiline: Lua.lua_toboolean(L, 4), secure: Lua.lua_toboolean(L, 5), text: text4, keyboardType: v4);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				string text5 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v5);
				TouchScreenKeyboard o6 = TouchScreenKeyboard.Open(autocorrection: Lua.lua_toboolean(L, 3), multiline: Lua.lua_toboolean(L, 4), secure: Lua.lua_toboolean(L, 5), alert: Lua.lua_toboolean(L, 6), text: text5, keyboardType: v5);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 7 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && (Lua.lua_isnil(L, 7) || Lua.lua_type(L, 7) == LuaTypes.LUA_TSTRING))
			{
				string text6 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v6);
				TouchScreenKeyboard o7 = TouchScreenKeyboard.Open(autocorrection: Lua.lua_toboolean(L, 3), multiline: Lua.lua_toboolean(L, 4), secure: Lua.lua_toboolean(L, 5), alert: Lua.lua_toboolean(L, 6), textPlaceholder: Lua.lua_tostring(L, 7), text: text6, keyboardType: v6);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 8 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<TouchScreenKeyboardType>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && (Lua.lua_isnil(L, 7) || Lua.lua_type(L, 7) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				string text7 = Lua.lua_tostring(L, 1);
				objectTranslator.Get(L, 2, out TouchScreenKeyboardType v7);
				TouchScreenKeyboard o8 = TouchScreenKeyboard.Open(autocorrection: Lua.lua_toboolean(L, 3), multiline: Lua.lua_toboolean(L, 4), secure: Lua.lua_toboolean(L, 5), alert: Lua.lua_toboolean(L, 6), textPlaceholder: Lua.lua_tostring(L, 7), characterLimit: Lua.xlua_tointeger(L, 8), text: text7, keyboardType: v7);
				objectTranslator.Push(L, o8);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.TouchScreenKeyboard.Open!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isSupported(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, TouchScreenKeyboard.isSupported);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isInPlaceEditingAllowed(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, TouchScreenKeyboard.isInPlaceEditingAllowed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_text(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, touchScreenKeyboard.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hideInput(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, TouchScreenKeyboard.hideInput);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_active(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchScreenKeyboard.active);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchScreenKeyboard.status);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_characterLimit(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, touchScreenKeyboard.characterLimit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canGetSelection(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchScreenKeyboard.canGetSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_canSetSelection(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, touchScreenKeyboard.canSetSelection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_selection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchScreenKeyboard.selection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, touchScreenKeyboard.type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetDisplay(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, touchScreenKeyboard.targetDisplay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_area(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, TouchScreenKeyboard.area);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_visible(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, TouchScreenKeyboard.visible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_text(IntPtr L)
	{
		try
		{
			((TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_hideInput(IntPtr L)
	{
		try
		{
			TouchScreenKeyboard.hideInput = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_active(IntPtr L)
	{
		try
		{
			((TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).active = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_characterLimit(IntPtr L)
	{
		try
		{
			((TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).characterLimit = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_selection(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TouchScreenKeyboard touchScreenKeyboard = (TouchScreenKeyboard)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RangeInt v);
			touchScreenKeyboard.selection = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetDisplay(IntPtr L)
	{
		try
		{
			((TouchScreenKeyboard)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetDisplay = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
