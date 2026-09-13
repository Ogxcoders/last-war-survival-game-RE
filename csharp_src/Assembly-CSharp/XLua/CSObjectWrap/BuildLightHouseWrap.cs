using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BuildLightHouseWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BuildLightHouse);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 0, 0);
		Utils.RegisterFunc(L, -3, "UpdateData", _m_UpdateData);
		Utils.RegisterFunc(L, -3, "UpdateLineStatus", _m_UpdateLineStatus);
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
				BuildLightHouse o = new BuildLightHouse();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildLightHouse constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateData(IntPtr L)
	{
		try
		{
			BuildLightHouse obj = (BuildLightHouse)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int brightnessLevel = Lua.xlua_tointeger(L, 2);
			bool activePower = Lua.lua_toboolean(L, 3);
			bool activePower2 = Lua.lua_toboolean(L, 4);
			bool activePower3 = Lua.lua_toboolean(L, 5);
			bool activePower4 = Lua.lua_toboolean(L, 6);
			obj.UpdateData(brightnessLevel, activePower, activePower2, activePower3, activePower4);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateLineStatus(IntPtr L)
	{
		try
		{
			BuildLightHouse obj = (BuildLightHouse)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool activePower = Lua.lua_toboolean(L, 2);
			bool activePower2 = Lua.lua_toboolean(L, 3);
			bool activePower3 = Lua.lua_toboolean(L, 4);
			bool activePower4 = Lua.lua_toboolean(L, 5);
			obj.UpdateLineStatus(activePower, activePower2, activePower3, activePower4);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
