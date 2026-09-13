using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WindowFullScreenWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WindowFullScreen);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "SetFullScreen", _m_SetFullScreen_xlua_st_);
		Utils.RegisterFunc(L, -4, "ResetFullScreen", _m_ResetFullScreen_xlua_st_);
		Utils.RegisterFunc(L, -4, "SwitchFullScreen", _m_SwitchFullScreen_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "WindowFullScreen does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFullScreen_xlua_st_(IntPtr L)
	{
		try
		{
			WindowFullScreen.SetFullScreen();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetFullScreen_xlua_st_(IntPtr L)
	{
		try
		{
			WindowFullScreen.ResetFullScreen();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SwitchFullScreen_xlua_st_(IntPtr L)
	{
		try
		{
			WindowFullScreen.SwitchFullScreen();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
