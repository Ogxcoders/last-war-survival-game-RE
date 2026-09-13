using System;
using TMPro;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class TMProTMP_DropdownOptionDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(TMP_Dropdown.OptionData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 2, 2);
		Utils.RegisterFunc(L, -2, "text", _g_get_text);
		Utils.RegisterFunc(L, -2, "image", _g_get_image);
		Utils.RegisterFunc(L, -1, "text", _s_set_text);
		Utils.RegisterFunc(L, -1, "image", _s_set_image);
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
				TMP_Dropdown.OptionData o = new TMP_Dropdown.OptionData();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				TMP_Dropdown.OptionData o2 = new TMP_Dropdown.OptionData(Lua.lua_tostring(L, 2));
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<Sprite>(L, 2))
			{
				TMP_Dropdown.OptionData o3 = new TMP_Dropdown.OptionData((Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite)));
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Sprite>(L, 3))
			{
				string text = Lua.lua_tostring(L, 2);
				Sprite image = (Sprite)objectTranslator.GetObject(L, 3, typeof(Sprite));
				TMP_Dropdown.OptionData o4 = new TMP_Dropdown.OptionData(text, image);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to TMPro.TMP_Dropdown.OptionData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_text(IntPtr L)
	{
		try
		{
			TMP_Dropdown.OptionData optionData = (TMP_Dropdown.OptionData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, optionData.text);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_image(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			TMP_Dropdown.OptionData optionData = (TMP_Dropdown.OptionData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, optionData.image);
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
			((TMP_Dropdown.OptionData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).text = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_image(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((TMP_Dropdown.OptionData)objectTranslator.FastGetCSObj(L, 1)).image = (Sprite)objectTranslator.GetObject(L, 2, typeof(Sprite));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
