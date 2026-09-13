using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DevUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DevUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 3, 1);
		Utils.RegisterFunc(L, -2, "AutoDebugLua", _g_get_AutoDebugLua);
		Utils.RegisterFunc(L, -2, "EditorEmmyLibPath", _g_get_EditorEmmyLibPath);
		Utils.RegisterFunc(L, -2, "luaDevUtils", _g_get_luaDevUtils);
		Utils.RegisterFunc(L, -1, "luaDevUtils", _s_set_luaDevUtils);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "DevUtils does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoDebugLua(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, DevUtils.AutoDebugLua);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EditorEmmyLibPath(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, DevUtils.EditorEmmyLibPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_luaDevUtils(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DevUtils.luaDevUtils);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_luaDevUtils(IntPtr L)
	{
		try
		{
			DevUtils.luaDevUtils = (LuaTable)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaTable));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
