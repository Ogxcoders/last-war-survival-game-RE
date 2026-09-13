using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class MirrorVersionConfigWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(MirrorVersionConfig);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 1, 0);
		Utils.RegisterFunc(L, -4, "RefreshOpenFlag", _m_RefreshOpenFlag_xlua_st_);
		Utils.RegisterFunc(L, -2, "IsMirrorVersionOpen", _g_get_IsMirrorVersionOpen);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "MirrorVersionConfig does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshOpenFlag_xlua_st_(IntPtr L)
	{
		try
		{
			MirrorVersionConfig.RefreshOpenFlag();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsMirrorVersionOpen(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, MirrorVersionConfig.IsMirrorVersionOpen);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
