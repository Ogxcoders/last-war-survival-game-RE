using System;
using UnityEngine;
using UnityEngine.UI;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UIGrayWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UIGray);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 7, 0, 0);
		Utils.RegisterFunc(L, -4, "DoCheckAfterSetSprite", _m_DoCheckAfterSetSprite_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGray", _m_SetGray_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGrayWithIgnore", _m_SetGrayWithIgnore_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGrayNotRecursively", _m_SetGrayNotRecursively_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGraphicGrayRecursively", _m_SetGraphicGrayRecursively_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetGraphicGrayRecursivelyWithIgnore", _m_SetGraphicGrayRecursivelyWithIgnore_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UIGray does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoCheckAfterSetSprite_xlua_st_(IntPtr L)
	{
		try
		{
			UIGray.DoCheckAfterSetSprite((Image)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Image)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGray_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Transform>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				Transform parent = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				bool bGray = Lua.lua_toboolean(L, 2);
				bool canClick = Lua.lua_toboolean(L, 3);
				bool withGraphic = Lua.lua_toboolean(L, 4);
				UIGray.SetGray(parent, bGray, canClick, withGraphic);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Transform>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				Transform parent2 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				bool bGray2 = Lua.lua_toboolean(L, 2);
				bool canClick2 = Lua.lua_toboolean(L, 3);
				UIGray.SetGray(parent2, bGray2, canClick2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Transform>(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				Transform parent3 = (Transform)objectTranslator.GetObject(L, 1, typeof(Transform));
				bool bGray3 = Lua.lua_toboolean(L, 2);
				UIGray.SetGray(parent3, bGray3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UIGray.SetGray!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGrayWithIgnore_xlua_st_(IntPtr L)
	{
		try
		{
			Transform parent = (Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform));
			bool bGray = Lua.lua_toboolean(L, 2);
			string ignoreName = Lua.lua_tostring(L, 3);
			UIGray.SetGrayWithIgnore(parent, bGray, ignoreName);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGrayNotRecursively_xlua_st_(IntPtr L)
	{
		try
		{
			Image image = (Image)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Image));
			bool bGray = Lua.lua_toboolean(L, 2);
			UIGray.SetGrayNotRecursively(image, bGray);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGraphicGrayRecursively_xlua_st_(IntPtr L)
	{
		try
		{
			UIGray.SetGraphicGrayRecursively((Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGraphicGrayRecursivelyWithIgnore_xlua_st_(IntPtr L)
	{
		try
		{
			Transform parent = (Transform)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Transform));
			string ignore = Lua.lua_tostring(L, 2);
			UIGray.SetGraphicGrayRecursivelyWithIgnore(parent, ignore);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
