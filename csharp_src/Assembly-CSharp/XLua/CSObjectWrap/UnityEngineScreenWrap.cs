using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineScreenWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Screen);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 16, 9);
		Utils.RegisterFunc(L, -4, "SetResolution", _m_SetResolution_xlua_st_);
		Utils.RegisterFunc(L, -2, "width", _g_get_width);
		Utils.RegisterFunc(L, -2, "height", _g_get_height);
		Utils.RegisterFunc(L, -2, "dpi", _g_get_dpi);
		Utils.RegisterFunc(L, -2, "currentResolution", _g_get_currentResolution);
		Utils.RegisterFunc(L, -2, "resolutions", _g_get_resolutions);
		Utils.RegisterFunc(L, -2, "fullScreen", _g_get_fullScreen);
		Utils.RegisterFunc(L, -2, "fullScreenMode", _g_get_fullScreenMode);
		Utils.RegisterFunc(L, -2, "safeArea", _g_get_safeArea);
		Utils.RegisterFunc(L, -2, "cutouts", _g_get_cutouts);
		Utils.RegisterFunc(L, -2, "autorotateToPortrait", _g_get_autorotateToPortrait);
		Utils.RegisterFunc(L, -2, "autorotateToPortraitUpsideDown", _g_get_autorotateToPortraitUpsideDown);
		Utils.RegisterFunc(L, -2, "autorotateToLandscapeLeft", _g_get_autorotateToLandscapeLeft);
		Utils.RegisterFunc(L, -2, "autorotateToLandscapeRight", _g_get_autorotateToLandscapeRight);
		Utils.RegisterFunc(L, -2, "orientation", _g_get_orientation);
		Utils.RegisterFunc(L, -2, "sleepTimeout", _g_get_sleepTimeout);
		Utils.RegisterFunc(L, -2, "brightness", _g_get_brightness);
		Utils.RegisterFunc(L, -1, "fullScreen", _s_set_fullScreen);
		Utils.RegisterFunc(L, -1, "fullScreenMode", _s_set_fullScreenMode);
		Utils.RegisterFunc(L, -1, "autorotateToPortrait", _s_set_autorotateToPortrait);
		Utils.RegisterFunc(L, -1, "autorotateToPortraitUpsideDown", _s_set_autorotateToPortraitUpsideDown);
		Utils.RegisterFunc(L, -1, "autorotateToLandscapeLeft", _s_set_autorotateToLandscapeLeft);
		Utils.RegisterFunc(L, -1, "autorotateToLandscapeRight", _s_set_autorotateToLandscapeRight);
		Utils.RegisterFunc(L, -1, "orientation", _s_set_orientation);
		Utils.RegisterFunc(L, -1, "sleepTimeout", _s_set_sleepTimeout);
		Utils.RegisterFunc(L, -1, "brightness", _s_set_brightness);
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
				Screen o = new Screen();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Screen constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetResolution_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int width = Lua.xlua_tointeger(L, 1);
				int height = Lua.xlua_tointeger(L, 2);
				bool fullscreen = Lua.lua_toboolean(L, 3);
				Screen.SetResolution(width, height, fullscreen);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int width2 = Lua.xlua_tointeger(L, 1);
				int height2 = Lua.xlua_tointeger(L, 2);
				bool fullscreen2 = Lua.lua_toboolean(L, 3);
				int preferredRefreshRate = Lua.xlua_tointeger(L, 4);
				Screen.SetResolution(width2, height2, fullscreen2, preferredRefreshRate);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<FullScreenMode>(L, 3))
			{
				int width3 = Lua.xlua_tointeger(L, 1);
				int height3 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out FullScreenMode v);
				Screen.SetResolution(width3, height3, v);
				return 0;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<FullScreenMode>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int width4 = Lua.xlua_tointeger(L, 1);
				int height4 = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out FullScreenMode v2);
				Screen.SetResolution(preferredRefreshRate: Lua.xlua_tointeger(L, 4), width: width4, height: height4, fullscreenMode: v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Screen.SetResolution!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_width(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Screen.width);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_height(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Screen.height);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dpi(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Screen.dpi);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_currentResolution(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Screen.currentResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resolutions(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Screen.resolutions);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fullScreen(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Screen.fullScreen);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fullScreenMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Screen.fullScreenMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_safeArea(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Screen.safeArea);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cutouts(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Screen.cutouts);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autorotateToPortrait(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Screen.autorotateToPortrait);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autorotateToPortraitUpsideDown(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Screen.autorotateToPortraitUpsideDown);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autorotateToLandscapeLeft(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Screen.autorotateToLandscapeLeft);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autorotateToLandscapeRight(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Screen.autorotateToLandscapeRight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orientation(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Screen.orientation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sleepTimeout(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Screen.sleepTimeout);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_brightness(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, Screen.brightness);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fullScreen(IntPtr L)
	{
		try
		{
			Screen.fullScreen = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fullScreenMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out FullScreenMode v);
			Screen.fullScreenMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autorotateToPortrait(IntPtr L)
	{
		try
		{
			Screen.autorotateToPortrait = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autorotateToPortraitUpsideDown(IntPtr L)
	{
		try
		{
			Screen.autorotateToPortraitUpsideDown = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autorotateToLandscapeLeft(IntPtr L)
	{
		try
		{
			Screen.autorotateToLandscapeLeft = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_autorotateToLandscapeRight(IntPtr L)
	{
		try
		{
			Screen.autorotateToLandscapeRight = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orientation(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ScreenOrientation v);
			Screen.orientation = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sleepTimeout(IntPtr L)
	{
		try
		{
			Screen.sleepTimeout = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_brightness(IntPtr L)
	{
		try
		{
			Screen.brightness = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
