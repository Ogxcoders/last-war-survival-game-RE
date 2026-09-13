using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CircleImageWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CircleImage);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 5, 5);
		Utils.RegisterFunc(L, -3, "IsRaycastLocationValid", _m_IsRaycastLocationValid);
		Utils.RegisterFunc(L, -3, "LoadSprite", _m_LoadSprite);
		Utils.RegisterFunc(L, -2, "fillPercent", _g_get_fillPercent);
		Utils.RegisterFunc(L, -2, "fill", _g_get_fill);
		Utils.RegisterFunc(L, -2, "thickness", _g_get_thickness);
		Utils.RegisterFunc(L, -2, "segements", _g_get_segements);
		Utils.RegisterFunc(L, -2, "isCircle", _g_get_isCircle);
		Utils.RegisterFunc(L, -1, "fillPercent", _s_set_fillPercent);
		Utils.RegisterFunc(L, -1, "fill", _s_set_fill);
		Utils.RegisterFunc(L, -1, "thickness", _s_set_thickness);
		Utils.RegisterFunc(L, -1, "segements", _s_set_segements);
		Utils.RegisterFunc(L, -1, "isCircle", _s_set_isCircle);
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
				CircleImage o = new CircleImage();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CircleImage constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRaycastLocationValid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CircleImage circleImage = (CircleImage)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			Camera eventCamera = (Camera)objectTranslator.GetObject(L, 3, typeof(Camera));
			bool value = circleImage.IsRaycastLocationValid(val, eventCamera);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadSprite(IntPtr L)
	{
		try
		{
			CircleImage image = (CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string spritePath = Lua.lua_tostring(L, 2);
				string defaultSprite = Lua.lua_tostring(L, 3);
				image.LoadSprite(spritePath, defaultSprite);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string spritePath2 = Lua.lua_tostring(L, 2);
				image.LoadSprite(spritePath2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CircleImage.LoadSprite!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fillPercent(IntPtr L)
	{
		try
		{
			CircleImage circleImage = (CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, circleImage.fillPercent);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fill(IntPtr L)
	{
		try
		{
			CircleImage circleImage = (CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, circleImage.fill);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_thickness(IntPtr L)
	{
		try
		{
			CircleImage circleImage = (CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, circleImage.thickness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_segements(IntPtr L)
	{
		try
		{
			CircleImage circleImage = (CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, circleImage.segements);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isCircle(IntPtr L)
	{
		try
		{
			CircleImage circleImage = (CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, circleImage.isCircle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fillPercent(IntPtr L)
	{
		try
		{
			((CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fillPercent = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fill(IntPtr L)
	{
		try
		{
			((CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fill = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_thickness(IntPtr L)
	{
		try
		{
			((CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).thickness = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_segements(IntPtr L)
	{
		try
		{
			((CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).segements = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isCircle(IntPtr L)
	{
		try
		{
			((CircleImage)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isCircle = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
