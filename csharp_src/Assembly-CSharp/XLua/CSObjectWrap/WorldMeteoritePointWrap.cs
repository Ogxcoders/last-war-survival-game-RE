using System;
using System.Text;
using Protobuf;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldMeteoritePointWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldMeteoritePoint);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 27, 16);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -3, "OnDescription", _m_OnDescription);
		Utils.RegisterFunc(L, -2, "VirtualRemainTimeSec", _g_get_VirtualRemainTimeSec);
		Utils.RegisterFunc(L, -2, "VirtualRemainTimeMs", _g_get_VirtualRemainTimeMs);
		Utils.RegisterFunc(L, -2, "VirtualRemainCount", _g_get_VirtualRemainCount);
		Utils.RegisterFunc(L, -2, "VirtualCollectCount", _g_get_VirtualCollectCount);
		Utils.RegisterFunc(L, -2, "OpenTimeCountdownSec", _g_get_OpenTimeCountdownSec);
		Utils.RegisterFunc(L, -2, "OpenTimeCountdownMs", _g_get_OpenTimeCountdownMs);
		Utils.RegisterFunc(L, -2, "State", _g_get_State);
		Utils.RegisterFunc(L, -2, "IsFull", _g_get_IsFull);
		Utils.RegisterFunc(L, -2, "IsCollecting", _g_get_IsCollecting);
		Utils.RegisterFunc(L, -2, "ExpiredTime", _g_get_ExpiredTime);
		Utils.RegisterFunc(L, -2, "CanCollect", _g_get_CanCollect);
		Utils.RegisterFunc(L, -2, "fromPoint", _g_get_fromPoint);
		Utils.RegisterFunc(L, -2, "buildId", _g_get_buildId);
		Utils.RegisterFunc(L, -2, "openTime", _g_get_openTime);
		Utils.RegisterFunc(L, -2, "collectEndTime", _g_get_collectEndTime);
		Utils.RegisterFunc(L, -2, "expireTime", _g_get_expireTime);
		Utils.RegisterFunc(L, -2, "remainTime", _g_get_remainTime);
		Utils.RegisterFunc(L, -2, "gatherUUID", _g_get_gatherUUID);
		Utils.RegisterFunc(L, -2, "gatherUid", _g_get_gatherUid);
		Utils.RegisterFunc(L, -2, "gatherAllianceId", _g_get_gatherAllianceId);
		Utils.RegisterFunc(L, -2, "targetPic", _g_get_targetPic);
		Utils.RegisterFunc(L, -2, "targetPicVer", _g_get_targetPicVer);
		Utils.RegisterFunc(L, -2, "targetHeadId", _g_get_targetHeadId);
		Utils.RegisterFunc(L, -2, "targetHeadET", _g_get_targetHeadET);
		Utils.RegisterFunc(L, -2, "maxGatherTime", _g_get_maxGatherTime);
		Utils.RegisterFunc(L, -2, "lastPoint", _g_get_lastPoint);
		Utils.RegisterFunc(L, -2, "pointPerSec", _g_get_pointPerSec);
		Utils.RegisterFunc(L, -1, "fromPoint", _s_set_fromPoint);
		Utils.RegisterFunc(L, -1, "buildId", _s_set_buildId);
		Utils.RegisterFunc(L, -1, "openTime", _s_set_openTime);
		Utils.RegisterFunc(L, -1, "collectEndTime", _s_set_collectEndTime);
		Utils.RegisterFunc(L, -1, "expireTime", _s_set_expireTime);
		Utils.RegisterFunc(L, -1, "remainTime", _s_set_remainTime);
		Utils.RegisterFunc(L, -1, "gatherUUID", _s_set_gatherUUID);
		Utils.RegisterFunc(L, -1, "gatherUid", _s_set_gatherUid);
		Utils.RegisterFunc(L, -1, "gatherAllianceId", _s_set_gatherAllianceId);
		Utils.RegisterFunc(L, -1, "targetPic", _s_set_targetPic);
		Utils.RegisterFunc(L, -1, "targetPicVer", _s_set_targetPicVer);
		Utils.RegisterFunc(L, -1, "targetHeadId", _s_set_targetHeadId);
		Utils.RegisterFunc(L, -1, "targetHeadET", _s_set_targetHeadET);
		Utils.RegisterFunc(L, -1, "maxGatherTime", _s_set_maxGatherTime);
		Utils.RegisterFunc(L, -1, "lastPoint", _s_set_lastPoint);
		Utils.RegisterFunc(L, -1, "pointPerSec", _s_set_pointPerSec);
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
				WorldMeteoritePoint o = new WorldMeteoritePoint();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				WorldMeteoritePoint o2 = new WorldMeteoritePoint((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMeteoritePoint constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo o = ((WorldMeteoritePoint)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDescription(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)objectTranslator.FastGetCSObj(L, 1);
			StringBuilder sb = (StringBuilder)objectTranslator.GetObject(L, 2, typeof(StringBuilder));
			worldMeteoritePoint.OnDescription(sb);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VirtualRemainTimeSec(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.VirtualRemainTimeSec);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VirtualRemainTimeMs(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.VirtualRemainTimeMs);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VirtualRemainCount(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.VirtualRemainCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_VirtualCollectCount(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.VirtualCollectCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OpenTimeCountdownSec(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.OpenTimeCountdownSec);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OpenTimeCountdownMs(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.OpenTimeCountdownMs);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_State(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushWorldMeteoritePointMeteoritePointState(L, worldMeteoritePoint.State);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsFull(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMeteoritePoint.IsFull);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsCollecting(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMeteoritePoint.IsCollecting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ExpiredTime(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMeteoritePoint.ExpiredTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CanCollect(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMeteoritePoint.CanCollect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fromPoint(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.fromPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildId(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.buildId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_openTime(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.openTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_collectEndTime(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.collectEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_expireTime(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.expireTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_remainTime(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.remainTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gatherUUID(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMeteoritePoint.gatherUUID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gatherUid(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMeteoritePoint.gatherUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gatherAllianceId(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMeteoritePoint.gatherAllianceId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPic(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldMeteoritePoint.targetPic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPicVer(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.targetPicVer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetHeadId(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.targetHeadId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetHeadET(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, worldMeteoritePoint.targetHeadET);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxGatherTime(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.maxGatherTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastPoint(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.lastPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointPerSec(IntPtr L)
	{
		try
		{
			WorldMeteoritePoint worldMeteoritePoint = (WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMeteoritePoint.pointPerSec);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fromPoint(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fromPoint = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildId(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buildId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_openTime(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).openTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_collectEndTime(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).collectEndTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_expireTime(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).expireTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_remainTime(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).remainTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gatherUUID(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gatherUUID = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gatherUid(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gatherUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gatherAllianceId(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).gatherAllianceId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetPic(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetPic = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetPicVer(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetPicVer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetHeadId(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetHeadId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetHeadET(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetHeadET = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxGatherTime(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxGatherTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lastPoint(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lastPoint = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointPerSec(IntPtr L)
	{
		try
		{
			((WorldMeteoritePoint)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointPerSec = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
