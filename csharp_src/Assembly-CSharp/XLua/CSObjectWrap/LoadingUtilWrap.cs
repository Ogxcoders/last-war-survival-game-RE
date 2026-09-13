using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LoadingUtilWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LoadingUtil);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "GetLoadingStateText", _m_GetLoadingStateText_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsLoadingErrorState", _m_IsLoadingErrorState_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsLoadigMaintenanceState", _m_IsLoadigMaintenanceState_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "LoadingUtil does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLoadingStateText_xlua_st_(IntPtr L)
	{
		try
		{
			string loadingStateText = LoadingUtil.GetLoadingStateText(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushstring(L, loadingStateText);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLoadingErrorState_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = LoadingUtil.IsLoadingErrorState(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsLoadigMaintenanceState_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = LoadingUtil.IsLoadigMaintenanceState(Lua.xlua_tointeger(L, 1));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
