using System;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DCPlayerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DCPlayer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 47, 3, 2);
		Utils.RegisterFunc(L, -3, "GetUid", _m_GetUid);
		Utils.RegisterFunc(L, -3, "IsInSourceServer", _m_IsInSourceServer);
		Utils.RegisterFunc(L, -3, "GetSourceServerId", _m_GetSourceServerId);
		Utils.RegisterFunc(L, -3, "SetCrossFightSrcServerId", _m_SetCrossFightSrcServerId);
		Utils.RegisterFunc(L, -3, "GetCrossFightSrcServerId", _m_GetCrossFightSrcServerId);
		Utils.RegisterFunc(L, -3, "GetSelfServerId", _m_GetSelfServerId);
		Utils.RegisterFunc(L, -3, "GetCrossServerId", _m_GetCrossServerId);
		Utils.RegisterFunc(L, -3, "GetMainUuid", _m_GetMainUuid);
		Utils.RegisterFunc(L, -3, "GetCurServerId", _m_GetCurServerId);
		Utils.RegisterFunc(L, -3, "SetWorldId", _m_SetWorldId);
		Utils.RegisterFunc(L, -3, "GetWorldId", _m_GetWorldId);
		Utils.RegisterFunc(L, -3, "SetWorldType", _m_SetWorldType);
		Utils.RegisterFunc(L, -3, "GetWorldType", _m_GetWorldType);
		Utils.RegisterFunc(L, -3, "GetBattleFieldType", _m_GetBattleFieldType);
		Utils.RegisterFunc(L, -3, "UpdatePlayerWorldPointId", _m_UpdatePlayerWorldPointId);
		Utils.RegisterFunc(L, -3, "GetSrcWorldId", _m_GetSrcWorldId);
		Utils.RegisterFunc(L, -3, "IsInBattleField", _m_IsInBattleField);
		Utils.RegisterFunc(L, -3, "OnCrossServerId", _m_OnCrossServerId);
		Utils.RegisterFunc(L, -3, "GetName", _m_GetName);
		Utils.RegisterFunc(L, -3, "IsInSelfServer", _m_IsInSelfServer);
		Utils.RegisterFunc(L, -3, "GetAllianceId", _m_GetAllianceId);
		Utils.RegisterFunc(L, -3, "SetAllianceId", _m_SetAllianceId);
		Utils.RegisterFunc(L, -3, "GetAllianceLeaderID", _m_GetAllianceLeaderID);
		Utils.RegisterFunc(L, -3, "SetAllianceLeaderID", _m_SetAllianceLeaderID);
		Utils.RegisterFunc(L, -3, "CSInit", _m_CSInit);
		Utils.RegisterFunc(L, -3, "UpdateUser", _m_UpdateUser);
		Utils.RegisterFunc(L, -3, "SetFightAllianceId", _m_SetFightAllianceId);
		Utils.RegisterFunc(L, -3, "GetFightAllianceId", _m_GetFightAllianceId);
		Utils.RegisterFunc(L, -3, "SetFightServerList", _m_SetFightServerList);
		Utils.RegisterFunc(L, -3, "GetIsInFightServerList", _m_GetIsInFightServerList);
		Utils.RegisterFunc(L, -3, "SetAttackInfoList", _m_SetAttackInfoList);
		Utils.RegisterFunc(L, -3, "GetIsInAttackDic", _m_GetIsInAttackDic);
		Utils.RegisterFunc(L, -3, "SetAllianceServerCamp", _m_SetAllianceServerCamp);
		Utils.RegisterFunc(L, -3, "IsAllianceSelfCamp", _m_IsAllianceSelfCamp);
		Utils.RegisterFunc(L, -3, "GetAllianceCampByAllianceId", _m_GetAllianceCampByAllianceId);
		Utils.RegisterFunc(L, -3, "GetData", _m_GetData);
		Utils.RegisterFunc(L, -3, "SetData", _m_SetData);
		Utils.RegisterFunc(L, -3, "DeleteData", _m_DeleteData);
		Utils.RegisterFunc(L, -3, "GetAllData", _m_GetAllData);
		Utils.RegisterFunc(L, -3, "SetReceiveBerserkBossRewardDic", _m_SetReceiveBerserkBossRewardDic);
		Utils.RegisterFunc(L, -3, "GetIsAlreadyBerserkBossReward", _m_GetIsAlreadyBerserkBossReward);
		Utils.RegisterFunc(L, -3, "CheckSwitch", _m_CheckSwitch);
		Utils.RegisterFunc(L, -3, "CheckImmediateSwitch", _m_CheckImmediateSwitch);
		Utils.RegisterFunc(L, -3, "SyncImmediateSwitch", _m_SyncImmediateSwitch);
		Utils.RegisterFunc(L, -3, "IsPlayerInSeasonOrHalt", _m_IsPlayerInSeasonOrHalt);
		Utils.RegisterFunc(L, -3, "GetRegisterCountry", _m_GetRegisterCountry);
		Utils.RegisterFunc(L, -3, "ClearAllCacheSwitch", _m_ClearAllCacheSwitch);
		Utils.RegisterFunc(L, -2, "IsBattleFieldWatching", _g_get_IsBattleFieldWatching);
		Utils.RegisterFunc(L, -2, "PlayerWorldPointId", _g_get_PlayerWorldPointId);
		Utils.RegisterFunc(L, -2, "Uid", _g_get_Uid);
		Utils.RegisterFunc(L, -1, "IsBattleFieldWatching", _s_set_IsBattleFieldWatching);
		Utils.RegisterFunc(L, -1, "Uid", _s_set_Uid);
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
				DCPlayer o = new DCPlayer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DCPlayer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUid(IntPtr L)
	{
		try
		{
			string uid = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetUid();
			Lua.lua_pushstring(L, uid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInSourceServer(IntPtr L)
	{
		try
		{
			bool value = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInSourceServer();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSourceServerId(IntPtr L)
	{
		try
		{
			int sourceServerId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSourceServerId();
			Lua.xlua_pushinteger(L, sourceServerId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCrossFightSrcServerId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int crossFightSrcServerId = Lua.xlua_tointeger(L, 2);
			obj.SetCrossFightSrcServerId(crossFightSrcServerId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCrossFightSrcServerId(IntPtr L)
	{
		try
		{
			int crossFightSrcServerId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCrossFightSrcServerId();
			Lua.xlua_pushinteger(L, crossFightSrcServerId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSelfServerId(IntPtr L)
	{
		try
		{
			int selfServerId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSelfServerId();
			Lua.xlua_pushinteger(L, selfServerId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCrossServerId(IntPtr L)
	{
		try
		{
			int crossServerId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCrossServerId();
			Lua.xlua_pushinteger(L, crossServerId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMainUuid(IntPtr L)
	{
		try
		{
			long mainUuid = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMainUuid();
			Lua.lua_pushint64(L, mainUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurServerId(IntPtr L)
	{
		try
		{
			int curServerId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurServerId();
			Lua.xlua_pushinteger(L, curServerId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorldId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int worldId = Lua.xlua_tointeger(L, 2);
			obj.SetWorldId(worldId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldId(IntPtr L)
	{
		try
		{
			int worldId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetWorldId();
			Lua.xlua_pushinteger(L, worldId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorldType(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int worldType = Lua.xlua_tointeger(L, 2);
			obj.SetWorldType(worldType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldType(IntPtr L)
	{
		try
		{
			int worldType = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetWorldType();
			Lua.xlua_pushinteger(L, worldType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBattleFieldType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BattleFieldType battleFieldType = ((DCPlayer)objectTranslator.FastGetCSObj(L, 1)).GetBattleFieldType();
			objectTranslator.Push(L, battleFieldType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdatePlayerWorldPointId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int playerPointId = Lua.xlua_tointeger(L, 2);
			obj.UpdatePlayerWorldPointId(playerPointId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSrcWorldId(IntPtr L)
	{
		try
		{
			int srcWorldId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSrcWorldId();
			Lua.xlua_pushinteger(L, srcWorldId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInBattleField(IntPtr L)
	{
		try
		{
			DCPlayer dCPlayer = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int wType = Lua.xlua_tointeger(L, 2);
				bool value = dCPlayer.IsInBattleField(wType);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 1)
			{
				bool value2 = dCPlayer.IsInBattleField();
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DCPlayer.IsInBattleField!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCrossServerId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int targetServerId = Lua.xlua_tointeger(L, 2);
			obj.OnCrossServerId(targetServerId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetName(IntPtr L)
	{
		try
		{
			string name = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetName();
			Lua.lua_pushstring(L, name);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInSelfServer(IntPtr L)
	{
		try
		{
			bool value = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsInSelfServer();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceId(IntPtr L)
	{
		try
		{
			string allianceId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAllianceId();
			Lua.lua_pushstring(L, allianceId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAllianceId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceId = Lua.lua_tostring(L, 2);
			obj.SetAllianceId(allianceId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceLeaderID(IntPtr L)
	{
		try
		{
			string allianceLeaderID = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAllianceLeaderID();
			Lua.lua_pushstring(L, allianceLeaderID);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAllianceLeaderID(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceLeaderID = Lua.lua_tostring(L, 2);
			obj.SetAllianceLeaderID(allianceLeaderID);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CSInit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject obj = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			dCPlayer.CSInit(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateUser(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject user = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			dCPlayer.UpdateUser(user);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFightAllianceId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string fightAllianceId = Lua.lua_tostring(L, 2);
			obj.SetFightAllianceId(fightAllianceId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFightAllianceId(IntPtr L)
	{
		try
		{
			string fightAllianceId = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFightAllianceId();
			Lua.lua_pushstring(L, fightAllianceId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFightServerList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			LuaTable fightServerList = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			dCPlayer.SetFightServerList(fightServerList);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsInFightServerList(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int sId = Lua.xlua_tointeger(L, 2);
			bool isInFightServerList = obj.GetIsInFightServerList(sId);
			Lua.lua_pushboolean(L, isInFightServerList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAttackInfoList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			LuaTable attackInfoList = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			dCPlayer.SetAttackInfoList(attackInfoList);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsInAttackDic(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uid = Lua.lua_tostring(L, 2);
			bool isInAttackDic = obj.GetIsInAttackDic(uid);
			Lua.lua_pushboolean(L, isInAttackDic);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAllianceServerCamp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			LuaTable allianceServerCamp = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			dCPlayer.SetAllianceServerCamp(allianceServerCamp);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsAllianceSelfCamp(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceId = Lua.lua_tostring(L, 2);
			bool value = obj.IsAllianceSelfCamp(allianceId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceCampByAllianceId(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceId = Lua.lua_tostring(L, 2);
			int allianceCampByAllianceId = obj.GetAllianceCampByAllianceId(allianceId);
			Lua.xlua_pushinteger(L, allianceCampByAllianceId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetData(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string data = obj.GetData(key);
			Lua.lua_pushstring(L, data);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetData(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			string value = Lua.lua_tostring(L, 3);
			obj.SetData(key, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteData(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			obj.DeleteData(key);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<string, string> allData = ((DCPlayer)objectTranslator.FastGetCSObj(L, 1)).GetAllData();
			objectTranslator.Push(L, allData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetReceiveBerserkBossRewardDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			LuaTable receiveBerserkBossRewardDic = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			dCPlayer.SetReceiveBerserkBossRewardDic(receiveBerserkBossRewardDic);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIsAlreadyBerserkBossReward(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long bossUuid = Lua.lua_toint64(L, 2);
			bool isAlreadyBerserkBossReward = obj.GetIsAlreadyBerserkBossReward(bossUuid);
			Lua.lua_pushboolean(L, isAlreadyBerserkBossReward);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckSwitch(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string key = Lua.lua_tostring(L, 2);
			bool defaultVal = Lua.lua_toboolean(L, 3);
			bool value = obj.CheckSwitch(key, defaultVal);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckImmediateSwitch(IntPtr L)
	{
		try
		{
			DCPlayer obj = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int key = Lua.xlua_tointeger(L, 2);
			bool defaultVal = Lua.lua_toboolean(L, 3);
			bool value = obj.CheckImmediateSwitch(key, defaultVal);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncImmediateSwitch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCPlayer dCPlayer = (DCPlayer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out long? v);
			dCPlayer.SyncImmediateSwitch(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPlayerInSeasonOrHalt(IntPtr L)
	{
		try
		{
			bool value = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsPlayerInSeasonOrHalt();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRegisterCountry(IntPtr L)
	{
		try
		{
			string registerCountry = ((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetRegisterCountry();
			Lua.lua_pushstring(L, registerCountry);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllCacheSwitch(IntPtr L)
	{
		try
		{
			((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllCacheSwitch();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsBattleFieldWatching(IntPtr L)
	{
		try
		{
			DCPlayer dCPlayer = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, dCPlayer.IsBattleFieldWatching);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PlayerWorldPointId(IntPtr L)
	{
		try
		{
			DCPlayer dCPlayer = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, dCPlayer.PlayerWorldPointId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Uid(IntPtr L)
	{
		try
		{
			DCPlayer dCPlayer = (DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, dCPlayer.Uid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_IsBattleFieldWatching(IntPtr L)
	{
		try
		{
			((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBattleFieldWatching = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Uid(IntPtr L)
	{
		try
		{
			((DCPlayer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uid = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
