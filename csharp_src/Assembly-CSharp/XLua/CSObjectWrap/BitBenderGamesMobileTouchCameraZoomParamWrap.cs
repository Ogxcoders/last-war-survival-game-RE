using System;
using BitBenderGames;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BitBenderGamesMobileTouchCameraZoomParamWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MobileTouchCamera.ZoomParam);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 3, 3);
		Utils.RegisterFunc(L, -2, "posY", _g_get_posY);
		Utils.RegisterFunc(L, -2, "offsetZ", _g_get_offsetZ);
		Utils.RegisterFunc(L, -2, "sensitivity", _g_get_sensitivity);
		Utils.RegisterFunc(L, -1, "posY", _s_set_posY);
		Utils.RegisterFunc(L, -1, "offsetZ", _s_set_offsetZ);
		Utils.RegisterFunc(L, -1, "sensitivity", _s_set_sensitivity);
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
				MobileTouchCamera.ZoomParam o = new MobileTouchCamera.ZoomParam();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BitBenderGames.MobileTouchCamera.ZoomParam constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_posY(IntPtr L)
	{
		try
		{
			MobileTouchCamera.ZoomParam zoomParam = (MobileTouchCamera.ZoomParam)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, zoomParam.posY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_offsetZ(IntPtr L)
	{
		try
		{
			MobileTouchCamera.ZoomParam zoomParam = (MobileTouchCamera.ZoomParam)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, zoomParam.offsetZ);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sensitivity(IntPtr L)
	{
		try
		{
			MobileTouchCamera.ZoomParam zoomParam = (MobileTouchCamera.ZoomParam)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, zoomParam.sensitivity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_posY(IntPtr L)
	{
		try
		{
			((MobileTouchCamera.ZoomParam)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).posY = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_offsetZ(IntPtr L)
	{
		try
		{
			((MobileTouchCamera.ZoomParam)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).offsetZ = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sensitivity(IntPtr L)
	{
		try
		{
			((MobileTouchCamera.ZoomParam)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sensitivity = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
