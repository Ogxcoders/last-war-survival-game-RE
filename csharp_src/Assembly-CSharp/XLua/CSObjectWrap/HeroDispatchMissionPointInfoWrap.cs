using System;
using System.Collections.Generic;
using Protobuf;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class HeroDispatchMissionPointInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(HeroDispatchMissionPointInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 9, 9);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -2, "cfgId", _g_get_cfgId);
		Utils.RegisterFunc(L, -2, "completionTime", _g_get_completionTime);
		Utils.RegisterFunc(L, -2, "stealList", _g_get_stealList);
		Utils.RegisterFunc(L, -2, "heroList", _g_get_heroList);
		Utils.RegisterFunc(L, -2, "rewarded", _g_get_rewarded);
		Utils.RegisterFunc(L, -2, "actEndTime", _g_get_actEndTime);
		Utils.RegisterFunc(L, -2, "accList", _g_get_accList);
		Utils.RegisterFunc(L, -2, "allianceId", _g_get_allianceId);
		Utils.RegisterFunc(L, -2, "expiredTime", _g_get_expiredTime);
		Utils.RegisterFunc(L, -1, "cfgId", _s_set_cfgId);
		Utils.RegisterFunc(L, -1, "completionTime", _s_set_completionTime);
		Utils.RegisterFunc(L, -1, "stealList", _s_set_stealList);
		Utils.RegisterFunc(L, -1, "heroList", _s_set_heroList);
		Utils.RegisterFunc(L, -1, "rewarded", _s_set_rewarded);
		Utils.RegisterFunc(L, -1, "actEndTime", _s_set_actEndTime);
		Utils.RegisterFunc(L, -1, "accList", _s_set_accList);
		Utils.RegisterFunc(L, -1, "allianceId", _s_set_allianceId);
		Utils.RegisterFunc(L, -1, "expiredTime", _s_set_expiredTime);
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
				HeroDispatchMissionPointInfo o = new HeroDispatchMissionPointInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				HeroDispatchMissionPointInfo o2 = new HeroDispatchMissionPointInfo((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to HeroDispatchMissionPointInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo o = ((HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cfgId(IntPtr L)
	{
		try
		{
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, heroDispatchMissionPointInfo.cfgId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_completionTime(IntPtr L)
	{
		try
		{
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, heroDispatchMissionPointInfo.completionTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stealList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, heroDispatchMissionPointInfo.stealList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_heroList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, heroDispatchMissionPointInfo.heroList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rewarded(IntPtr L)
	{
		try
		{
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, heroDispatchMissionPointInfo.rewarded);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_actEndTime(IntPtr L)
	{
		try
		{
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, heroDispatchMissionPointInfo.actEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_accList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, heroDispatchMissionPointInfo.accList);
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
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, heroDispatchMissionPointInfo.allianceId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_expiredTime(IntPtr L)
	{
		try
		{
			HeroDispatchMissionPointInfo heroDispatchMissionPointInfo = (HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, heroDispatchMissionPointInfo.expiredTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cfgId(IntPtr L)
	{
		try
		{
			((HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cfgId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_completionTime(IntPtr L)
	{
		try
		{
			((HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).completionTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stealList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1)).stealList = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_heroList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1)).heroList = (List<long>)objectTranslator.GetObject(L, 2, typeof(List<long>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rewarded(IntPtr L)
	{
		try
		{
			((HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).rewarded = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_actEndTime(IntPtr L)
	{
		try
		{
			((HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).actEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_accList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((HeroDispatchMissionPointInfo)objectTranslator.FastGetCSObj(L, 1)).accList = (List<string>)objectTranslator.GetObject(L, 2, typeof(List<string>));
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
			((HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_expiredTime(IntPtr L)
	{
		try
		{
			((HeroDispatchMissionPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).expiredTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
