using System;
using System.Text;
using Google.Protobuf.Collections;
using Protobuf;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PointInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PointInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 14, 15, 12);
		Utils.RegisterFunc(L, -3, "CheckClickIsValid", _m_CheckClickIsValid);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -3, "IsMine", _m_IsMine);
		Utils.RegisterFunc(L, -3, "GetPlayerType", _m_GetPlayerType);
		Utils.RegisterFunc(L, -3, "IsFrozen", _m_IsFrozen);
		Utils.RegisterFunc(L, -3, "IsOverHeat", _m_IsOverHeat);
		Utils.RegisterFunc(L, -3, "Description", _m_Description);
		Utils.RegisterFunc(L, -3, "OnDescription", _m_OnDescription);
		Utils.RegisterFunc(L, -3, "IsHasStatusOn", _m_IsHasStatusOn);
		Utils.RegisterFunc(L, -3, "GetStatusExpireTime", _m_GetStatusExpireTime);
		Utils.RegisterFunc(L, -3, "GetStatusExpireTimeById", _m_GetStatusExpireTimeById);
		Utils.RegisterFunc(L, -3, "SetStatus", _m_SetStatus);
		Utils.RegisterFunc(L, -3, "GetStatusByType", _m_GetStatusByType);
		Utils.RegisterFunc(L, -3, "GetCityVirusLayer", _m_GetCityVirusLayer);
		Utils.RegisterFunc(L, -2, "LodLayerMask", _g_get_LodLayerMask);
		Utils.RegisterFunc(L, -2, "isMainPoint", _g_get_isMainPoint);
		Utils.RegisterFunc(L, -2, "PointType", _g_get_PointType);
		Utils.RegisterFunc(L, -2, "pointIndex", _g_get_pointIndex);
		Utils.RegisterFunc(L, -2, "mainIndex", _g_get_mainIndex);
		Utils.RegisterFunc(L, -2, "pointType", _g_get_pointType);
		Utils.RegisterFunc(L, -2, "ownerUid", _g_get_ownerUid);
		Utils.RegisterFunc(L, -2, "tileSize", _g_get_tileSize);
		Utils.RegisterFunc(L, -2, "uuid", _g_get_uuid);
		Utils.RegisterFunc(L, -2, "extraInfo", _g_get_extraInfo);
		Utils.RegisterFunc(L, -2, "serverId", _g_get_serverId);
		Utils.RegisterFunc(L, -2, "srcServerId", _g_get_srcServerId);
		Utils.RegisterFunc(L, -2, "worldId", _g_get_worldId);
		Utils.RegisterFunc(L, -2, "thermalConductor", _g_get_thermalConductor);
		Utils.RegisterFunc(L, -2, "status", _g_get_status);
		Utils.RegisterFunc(L, -1, "pointIndex", _s_set_pointIndex);
		Utils.RegisterFunc(L, -1, "mainIndex", _s_set_mainIndex);
		Utils.RegisterFunc(L, -1, "pointType", _s_set_pointType);
		Utils.RegisterFunc(L, -1, "ownerUid", _s_set_ownerUid);
		Utils.RegisterFunc(L, -1, "tileSize", _s_set_tileSize);
		Utils.RegisterFunc(L, -1, "uuid", _s_set_uuid);
		Utils.RegisterFunc(L, -1, "extraInfo", _s_set_extraInfo);
		Utils.RegisterFunc(L, -1, "serverId", _s_set_serverId);
		Utils.RegisterFunc(L, -1, "srcServerId", _s_set_srcServerId);
		Utils.RegisterFunc(L, -1, "worldId", _s_set_worldId);
		Utils.RegisterFunc(L, -1, "thermalConductor", _s_set_thermalConductor);
		Utils.RegisterFunc(L, -1, "status", _s_set_status);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 7, 0, 0);
		Utils.RegisterObject(L, translator, -4, "WEREWOLF_STATUS_ID", 704102);
		Utils.RegisterObject(L, translator, -4, "WEREWOLF_BASE_SKIN_ID", -89757);
		Utils.RegisterObject(L, translator, -4, "WEREWOLF_SKIN_PREFAB", "Assets/Main/SeasonRes/S4/Prefabs/Building/A_build_werewolf_world.prefab");
		Utils.RegisterObject(L, translator, -4, "DEFAULT_BUILD_MODEL_PATH", "Assets/Main/Prefabs/Building/building_10100035_world.prefab");
		Utils.RegisterObject(L, translator, -4, "WEREWOLF_TITLE_SKIN_ID", 0);
		Utils.RegisterObject(L, translator, -4, "WEREWOLF_EFFECT_SKIN_ID", 0);
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
				PointInfo o = new PointInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				PointInfo o2 = new PointInfo((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PointInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckClickIsValid(IntPtr L)
	{
		try
		{
			PointInfo obj = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			bool value = obj.CheckClickIsValid(lod);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo o = ((PointInfo)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMine(IntPtr L)
	{
		try
		{
			bool value = ((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsMine();
			Lua.lua_pushboolean(L, value);
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
			PlayerType playerType = ((PointInfo)objectTranslator.FastGetCSObj(L, 1)).GetPlayerType();
			objectTranslator.PushPlayerType(L, playerType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFrozen(IntPtr L)
	{
		try
		{
			bool value = ((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsFrozen();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOverHeat(IntPtr L)
	{
		try
		{
			bool value = ((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsOverHeat();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Description(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<StringBuilder>(L, 2))
			{
				StringBuilder sb = (StringBuilder)objectTranslator.GetObject(L, 2, typeof(StringBuilder));
				string str = pointInfo.Description(sb);
				Lua.lua_pushstring(L, str);
				return 1;
			}
			if (num == 1)
			{
				string str2 = pointInfo.Description();
				Lua.lua_pushstring(L, str2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PointInfo.Description!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDescription(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			StringBuilder sb = (StringBuilder)objectTranslator.GetObject(L, 2, typeof(StringBuilder));
			pointInfo.OnDescription(sb);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsHasStatusOn(IntPtr L)
	{
		try
		{
			PointInfo obj = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int type = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsHasStatusOn(type);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStatusExpireTime(IntPtr L)
	{
		try
		{
			PointInfo obj = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int type = Lua.xlua_tointeger(L, 2);
			long statusExpireTime = obj.GetStatusExpireTime(type);
			Lua.lua_pushint64(L, statusExpireTime);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStatusExpireTimeById(IntPtr L)
	{
		try
		{
			PointInfo obj = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			long statusExpireTimeById = obj.GetStatusExpireTimeById(id);
			Lua.lua_pushint64(L, statusExpireTimeById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStatus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject status = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			pointInfo.SetStatus(status);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStatusByType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo obj = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			int type = Lua.xlua_tointeger(L, 2);
			Status statusByType = obj.GetStatusByType(type);
			objectTranslator.Push(L, statusByType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCityVirusLayer(IntPtr L)
	{
		try
		{
			int cityVirusLayer = ((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCityVirusLayer();
			Lua.xlua_pushinteger(L, cityVirusLayer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LodLayerMask(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.LodLayerMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isMainPoint(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, pointInfo.isMainPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PointType(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.PointType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointIndex(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.pointIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainIndex(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.mainIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pointType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushWorldPointType(L, pointInfo.pointType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ownerUid(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, pointInfo.ownerUid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tileSize(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.tileSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_uuid(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, pointInfo.uuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_extraInfo(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, pointInfo.extraInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_serverId(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.serverId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_srcServerId(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.srcServerId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldId(IntPtr L)
	{
		try
		{
			PointInfo pointInfo = (PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, pointInfo.worldId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_thermalConductor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointInfo.thermalConductor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, pointInfo.status);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointIndex(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).pointIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mainIndex(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mainIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pointType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo pointInfo = (PointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldPointType val);
			pointInfo.pointType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ownerUid(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ownerUid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tileSize(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).tileSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_uuid(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).uuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_extraInfo(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).extraInfo = Lua.lua_tobytes(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_serverId(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).serverId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_srcServerId(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).srcServerId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldId(IntPtr L)
	{
		try
		{
			((PointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).worldId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_thermalConductor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointInfo)objectTranslator.FastGetCSObj(L, 1)).thermalConductor = (ThermalConductor)objectTranslator.GetObject(L, 2, typeof(ThermalConductor));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_status(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((PointInfo)objectTranslator.FastGetCSObj(L, 1)).status = (RepeatedField<Status>)objectTranslator.GetObject(L, 2, typeof(RepeatedField<Status>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
