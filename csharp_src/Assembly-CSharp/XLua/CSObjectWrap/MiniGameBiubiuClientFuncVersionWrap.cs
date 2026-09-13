using System;
using MiniGame.Biubiu.Client;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MiniGameBiubiuClientFuncVersionWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FuncVersion);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 1);
		Utils.RegisterFunc(L, -2, "CSharpCodeMD5", _g_get_CSharpCodeMD5);
		Utils.RegisterFunc(L, -1, "CSharpCodeMD5", _s_set_CSharpCodeMD5);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MiniGame.Biubiu.Client.FuncVersion does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CSharpCodeMD5(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, FuncVersion.CSharpCodeMD5);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CSharpCodeMD5(IntPtr L)
	{
		try
		{
			FuncVersion.CSharpCodeMD5 = Lua.lua_tostring(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
