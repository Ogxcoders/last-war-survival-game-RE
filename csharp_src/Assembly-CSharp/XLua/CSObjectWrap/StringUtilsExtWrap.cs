using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class StringUtilsExtWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(StringUtilsExt);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "GetFormattedSeperatorNum", _m_GetFormattedSeperatorNum_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "StringUtilsExt does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFormattedSeperatorNum_xlua_st_(IntPtr L)
	{
		try
		{
			string formattedSeperatorNum = StringUtilsExt.GetFormattedSeperatorNum(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushstring(L, formattedSeperatorNum);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
