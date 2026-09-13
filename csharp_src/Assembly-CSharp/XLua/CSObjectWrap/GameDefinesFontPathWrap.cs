using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesFontPathWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.FontPath);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 2, 2);
		Utils.RegisterFunc(L, -2, "Chinese", _g_get_Chinese);
		Utils.RegisterFunc(L, -2, "Title", _g_get_Title);
		Utils.RegisterFunc(L, -1, "Chinese", _s_set_Chinese);
		Utils.RegisterFunc(L, -1, "Title", _s_set_Title);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines.FontPath does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Chinese(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, GameDefines.FontPath.Chinese);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Title(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, GameDefines.FontPath.Title);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Chinese(IntPtr L)
	{
		try
		{
			GameDefines.FontPath.Chinese = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Title(IntPtr L)
	{
		try
		{
			GameDefines.FontPath.Title = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
