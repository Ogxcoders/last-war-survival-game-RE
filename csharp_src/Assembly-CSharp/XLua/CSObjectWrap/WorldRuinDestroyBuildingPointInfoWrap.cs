using System;
using Protobuf;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldRuinDestroyBuildingPointInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldRuinDestroyBuildingPointInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 5, 5);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -3, "GetPlayerType", _m_GetPlayerType);
		Utils.RegisterFunc(L, -2, "serverId", _g_get_serverId);
		Utils.RegisterFunc(L, -2, "allianceId", _g_get_allianceId);
		Utils.RegisterFunc(L, -2, "alAbbr", _g_get_alAbbr);
		Utils.RegisterFunc(L, -2, "userName", _g_get_userName);
		Utils.RegisterFunc(L, -2, "destroyEndTime", _g_get_destroyEndTime);
		Utils.RegisterFunc(L, -1, "serverId", _s_set_serverId);
		Utils.RegisterFunc(L, -1, "allianceId", _s_set_allianceId);
		Utils.RegisterFunc(L, -1, "alAbbr", _s_set_alAbbr);
		Utils.RegisterFunc(L, -1, "userName", _s_set_userName);
		Utils.RegisterFunc(L, -1, "destroyEndTime", _s_set_destroyEndTime);
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
				WorldRuinDestroyBuildingPointInfo o = new WorldRuinDestroyBuildingPointInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				WorldRuinDestroyBuildingPointInfo o2 = new WorldRuinDestroyBuildingPointInfo((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldRuinDestroyBuildingPointInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo o = ((WorldRuinDestroyBuildingPointInfo)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPlayerType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PlayerType playerType = ((WorldRuinDestroyBuildingPointInfo)objectTranslator.FastGetCSObj(L, 1)).GetPlayerType();
			objectTranslator.PushPlayerType(L, playerType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverId(IntPtr L)
	{
		try
		{
			WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = (WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldRuinDestroyBuildingPointInfo.serverId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allianceId(IntPtr L)
	{
		try
		{
			WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = (WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldRuinDestroyBuildingPointInfo.allianceId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alAbbr(IntPtr L)
	{
		try
		{
			WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = (WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldRuinDestroyBuildingPointInfo.alAbbr);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_userName(IntPtr L)
	{
		try
		{
			WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = (WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldRuinDestroyBuildingPointInfo.userName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_destroyEndTime(IntPtr L)
	{
		try
		{
			WorldRuinDestroyBuildingPointInfo worldRuinDestroyBuildingPointInfo = (WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldRuinDestroyBuildingPointInfo.destroyEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverId(IntPtr L)
	{
		try
		{
			((WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allianceId(IntPtr L)
	{
		try
		{
			((WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alAbbr(IntPtr L)
	{
		try
		{
			((WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alAbbr = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_userName(IntPtr L)
	{
		try
		{
			((WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).userName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_destroyEndTime(IntPtr L)
	{
		try
		{
			((WorldRuinDestroyBuildingPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).destroyEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
