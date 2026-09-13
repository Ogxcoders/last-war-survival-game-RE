using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneLuaArrayFacadeWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneLuaArrayFacade);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 4, 0, 0);
		Utils.RegisterFunc(L, -4, "InitLongArrayAccess", _m_InitLongArrayAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "UnInitLongArrayAccess", _m_UnInitLongArrayAccess_xlua_st_);
		Utils.RegisterFunc(L, -4, "SyncCityBuildNameId", _m_SyncCityBuildNameId_xlua_st_);
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
				SceneLuaArrayFacade o = new SceneLuaArrayFacade();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneLuaArrayFacade constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitLongArrayAccess_xlua_st_(IntPtr L)
	{
		try
		{
			SceneLuaArrayFacade.InitLongArrayAccess((LuaArrAccess)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(LuaArrAccess)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInitLongArrayAccess_xlua_st_(IntPtr L)
	{
		try
		{
			SceneLuaArrayFacade.UnInitLongArrayAccess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncCityBuildNameId_xlua_st_(IntPtr L)
	{
		try
		{
			string cityBuildName = Lua.lua_tostring(L, 1);
			long id = Lua.lua_toint64(L, 2);
			SceneLuaArrayFacade.SyncCityBuildNameId(cityBuildName, id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
