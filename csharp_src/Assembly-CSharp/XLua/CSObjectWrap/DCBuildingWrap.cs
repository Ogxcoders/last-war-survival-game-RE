using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DCBuildingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DCBuilding);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 10, 0, 0);
		Utils.RegisterFunc(L, -3, "GetBuildingDataByUuid", _m_GetBuildingDataByUuid);
		Utils.RegisterFunc(L, -3, "GetBuildingDataByBuildId", _m_GetBuildingDataByBuildId);
		Utils.RegisterFunc(L, -3, "GetMainLv", _m_GetMainLv);
		Utils.RegisterFunc(L, -3, "GetMainPos", _m_GetMainPos);
		Utils.RegisterFunc(L, -3, "SetMainPos", _m_SetMainPos);
		Utils.RegisterFunc(L, -3, "GetWorldMainPos", _m_GetWorldMainPos);
		Utils.RegisterFunc(L, -3, "GetDragonWorldPos", _m_GetDragonWorldPos);
		Utils.RegisterFunc(L, -3, "UpdateMyBuilding", _m_UpdateMyBuilding);
		Utils.RegisterFunc(L, -3, "UpdateDragonWorldPos", _m_UpdateDragonWorldPos);
		Utils.RegisterFunc(L, -3, "CheckIsMyBuilding", _m_CheckIsMyBuilding);
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
				DCBuilding o = new DCBuilding();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DCBuilding constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildingDataByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCBuilding obj = (DCBuilding)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			LuaBuildData buildingDataByUuid = obj.GetBuildingDataByUuid(uuid);
			objectTranslator.Push(L, buildingDataByUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildingDataByBuildId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCBuilding obj = (DCBuilding)objectTranslator.FastGetCSObj(L, 1);
			int buildId = Lua.xlua_tointeger(L, 2);
			LuaBuildData buildingDataByBuildId = obj.GetBuildingDataByBuildId(buildId);
			objectTranslator.Push(L, buildingDataByBuildId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMainLv(IntPtr L)
	{
		try
		{
			int mainLv = ((DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMainLv();
			Lua.xlua_pushinteger(L, mainLv);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMainPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2Int mainPos = ((DCBuilding)objectTranslator.FastGetCSObj(L, 1)).GetMainPos();
			objectTranslator.Push(L, mainPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMainPos(IntPtr L)
	{
		try
		{
			((DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetMainPos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldMainPos(IntPtr L)
	{
		try
		{
			int worldMainPos = ((DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetWorldMainPos();
			Lua.xlua_pushinteger(L, worldMainPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDragonWorldPos(IntPtr L)
	{
		try
		{
			int dragonWorldPos = ((DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDragonWorldPos();
			Lua.xlua_pushinteger(L, dragonWorldPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMyBuilding(IntPtr L)
	{
		try
		{
			DCBuilding obj = (DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool remove = Lua.lua_toboolean(L, 3);
			obj.UpdateMyBuilding(uuid, remove);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateDragonWorldPos(IntPtr L)
	{
		try
		{
			DCBuilding obj = (DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pos = Lua.xlua_tointeger(L, 2);
			obj.UpdateDragonWorldPos(pos);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckIsMyBuilding(IntPtr L)
	{
		try
		{
			DCBuilding obj = (DCBuilding)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.CheckIsMyBuilding(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
