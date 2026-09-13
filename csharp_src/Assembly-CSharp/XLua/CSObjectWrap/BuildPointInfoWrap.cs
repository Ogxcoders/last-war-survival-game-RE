using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf.Collections;
using Protobuf;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BuildPointInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BuildPointInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 20, 65, 63);
		Utils.RegisterFunc(L, -3, "GetAOSType", _m_GetAOSType);
		Utils.RegisterFunc(L, -3, "UpdateAssistanceCount", _m_UpdateAssistanceCount);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -3, "GetShowState", _m_GetShowState);
		Utils.RegisterFunc(L, -3, "GetShowStateByIndex", _m_GetShowStateByIndex);
		Utils.RegisterFunc(L, -3, "IsThisState", _m_IsThisState);
		Utils.RegisterFunc(L, -3, "GetSkinId", _m_GetSkinId);
		Utils.RegisterFunc(L, -3, "GetModelPath", _m_GetModelPath);
		Utils.RegisterFunc(L, -3, "GetSkinModelName", _m_GetSkinModelName);
		Utils.RegisterFunc(L, -3, "GetDefaultModelName", _m_GetDefaultModelName);
		Utils.RegisterFunc(L, -3, "RefreshRoadDir", _m_RefreshRoadDir);
		Utils.RegisterFunc(L, -3, "GetPosIndexListByDir", _m_GetPosIndexListByDir);
		Utils.RegisterFunc(L, -3, "ChangeRoadDir", _m_ChangeRoadDir);
		Utils.RegisterFunc(L, -3, "IsNormalType", _m_IsNormalType);
		Utils.RegisterFunc(L, -3, "IsWormWrap", _m_IsWormWrap);
		Utils.RegisterFunc(L, -3, "GetPlayerType", _m_GetPlayerType);
		Utils.RegisterFunc(L, -3, "GetResourcePercent", _m_GetResourcePercent);
		Utils.RegisterFunc(L, -3, "IsWerewolfBirthing", _m_IsWerewolfBirthing);
		Utils.RegisterFunc(L, -3, "OnDescription", _m_OnDescription);
		Utils.RegisterFunc(L, -3, "UncoverWerewolf", _m_UncoverWerewolf);
		Utils.RegisterFunc(L, -2, "AOSType", _g_get_AOSType);
		Utils.RegisterFunc(L, -2, "IsWerewolf", _g_get_IsWerewolf);
		Utils.RegisterFunc(L, -2, "itemId", _g_get_itemId);
		Utils.RegisterFunc(L, -2, "level", _g_get_level);
		Utils.RegisterFunc(L, -2, "state", _g_get_state);
		Utils.RegisterFunc(L, -2, "buildState", _g_get_buildState);
		Utils.RegisterFunc(L, -2, "allianceId", _g_get_allianceId);
		Utils.RegisterFunc(L, -2, "startTime", _g_get_startTime);
		Utils.RegisterFunc(L, -2, "endTime", _g_get_endTime);
		Utils.RegisterFunc(L, -2, "inside", _g_get_inside);
		Utils.RegisterFunc(L, -2, "roadDir", _g_get_roadDir);
		Utils.RegisterFunc(L, -2, "curHp", _g_get_curHp);
		Utils.RegisterFunc(L, -2, "lastHpTime", _g_get_lastHpTime);
		Utils.RegisterFunc(L, -2, "protectEndTime", _g_get_protectEndTime);
		Utils.RegisterFunc(L, -2, "playerName", _g_get_playerName);
		Utils.RegisterFunc(L, -2, "alAbbr", _g_get_alAbbr);
		Utils.RegisterFunc(L, -2, "lastCollectTime", _g_get_lastCollectTime);
		Utils.RegisterFunc(L, -2, "unavailableTime", _g_get_unavailableTime);
		Utils.RegisterFunc(L, -2, "queueItemId", _g_get_queueItemId);
		Utils.RegisterFunc(L, -2, "queueStartTime", _g_get_queueStartTime);
		Utils.RegisterFunc(L, -2, "queueUpdateTime", _g_get_queueUpdateTime);
		Utils.RegisterFunc(L, -2, "destroyStartTime", _g_get_destroyStartTime);
		Utils.RegisterFunc(L, -2, "destroyEndTime", _g_get_destroyEndTime);
		Utils.RegisterFunc(L, -2, "appearanceId", _g_get_appearanceId);
		Utils.RegisterFunc(L, -2, "specialType", _g_get_specialType);
		Utils.RegisterFunc(L, -2, "positionId", _g_get_positionId);
		Utils.RegisterFunc(L, -2, "virusLayer", _g_get_virusLayer);
		Utils.RegisterFunc(L, -2, "virusEndTime", _g_get_virusEndTime);
		Utils.RegisterFunc(L, -2, "refuseTreadVirus", _g_get_refuseTreadVirus);
		Utils.RegisterFunc(L, -2, "refusePowerHelper", _g_get_refusePowerHelper);
		Utils.RegisterFunc(L, -2, "lightHouseInfo", _g_get_lightHouseInfo);
		Utils.RegisterFunc(L, -2, "recoverSpeed", _g_get_recoverSpeed);
		Utils.RegisterFunc(L, -2, "fireSpeed", _g_get_fireSpeed);
		Utils.RegisterFunc(L, -2, "assistanceCount", _g_get_assistanceCount);
		Utils.RegisterFunc(L, -2, "maxAssistanceCount", _g_get_maxAssistanceCount);
		Utils.RegisterFunc(L, -2, "wolfDecrHp", _g_get_wolfDecrHp);
		Utils.RegisterFunc(L, -2, "fireworksInfoList", _g_get_fireworksInfoList);
		Utils.RegisterFunc(L, -2, "fireworksGiftList", _g_get_fireworksGiftList);
		Utils.RegisterFunc(L, -2, "aosEndTime", _g_get_aosEndTime);
		Utils.RegisterFunc(L, -2, "skillId", _g_get_skillId);
		Utils.RegisterFunc(L, -2, "sandWorm", _g_get_sandWorm);
		Utils.RegisterFunc(L, -2, "mummyConvertId", _g_get_mummyConvertId);
		Utils.RegisterFunc(L, -2, "mummyConvertCount", _g_get_mummyConvertCount);
		Utils.RegisterFunc(L, -2, "skinId", _g_get_skinId);
		Utils.RegisterFunc(L, -2, "colourId", _g_get_colourId);
		Utils.RegisterFunc(L, -2, "colourTime", _g_get_colourTime);
		Utils.RegisterFunc(L, -2, "titleNameSkinId", _g_get_titleNameSkinId);
		Utils.RegisterFunc(L, -2, "effectId", _g_get_effectId);
		Utils.RegisterFunc(L, -2, "wolfCoveredInfo", _g_get_wolfCoveredInfo);
		Utils.RegisterFunc(L, -2, "crystal", _g_get_crystal);
		Utils.RegisterFunc(L, -2, "nucleus", _g_get_nucleus);
		Utils.RegisterFunc(L, -2, "countryFlag", _g_get_countryFlag);
		Utils.RegisterFunc(L, -2, "monsterInvasion", _g_get_monsterInvasion);
		Utils.RegisterFunc(L, -2, "zoneMobilization", _g_get_zoneMobilization);
		Utils.RegisterFunc(L, -2, "commonMonsterSkillInfo", _g_get_commonMonsterSkillInfo);
		Utils.RegisterFunc(L, -2, "challangeNewDonate", _g_get_challangeNewDonate);
		Utils.RegisterFunc(L, -2, "seasonRole", _g_get_seasonRole);
		Utils.RegisterFunc(L, -2, "quarantineSkillId", _g_get_quarantineSkillId);
		Utils.RegisterFunc(L, -2, "quarantineSkillSTime", _g_get_quarantineSkillSTime);
		Utils.RegisterFunc(L, -2, "quarantineSkillETime", _g_get_quarantineSkillETime);
		Utils.RegisterFunc(L, -2, "quarantineSkilTargetPId", _g_get_quarantineSkilTargetPId);
		Utils.RegisterFunc(L, -2, "quarantineRole", _g_get_quarantineRole);
		Utils.RegisterFunc(L, -2, "quarantineLeave", _g_get_quarantineLeave);
		Utils.RegisterFunc(L, -2, "curMaxHp", _g_get_curMaxHp);
		Utils.RegisterFunc(L, -2, "mummyCurseExpireTime", _g_get_mummyCurseExpireTime);
		Utils.RegisterFunc(L, -1, "itemId", _s_set_itemId);
		Utils.RegisterFunc(L, -1, "level", _s_set_level);
		Utils.RegisterFunc(L, -1, "state", _s_set_state);
		Utils.RegisterFunc(L, -1, "buildState", _s_set_buildState);
		Utils.RegisterFunc(L, -1, "allianceId", _s_set_allianceId);
		Utils.RegisterFunc(L, -1, "startTime", _s_set_startTime);
		Utils.RegisterFunc(L, -1, "endTime", _s_set_endTime);
		Utils.RegisterFunc(L, -1, "inside", _s_set_inside);
		Utils.RegisterFunc(L, -1, "roadDir", _s_set_roadDir);
		Utils.RegisterFunc(L, -1, "curHp", _s_set_curHp);
		Utils.RegisterFunc(L, -1, "lastHpTime", _s_set_lastHpTime);
		Utils.RegisterFunc(L, -1, "protectEndTime", _s_set_protectEndTime);
		Utils.RegisterFunc(L, -1, "playerName", _s_set_playerName);
		Utils.RegisterFunc(L, -1, "alAbbr", _s_set_alAbbr);
		Utils.RegisterFunc(L, -1, "lastCollectTime", _s_set_lastCollectTime);
		Utils.RegisterFunc(L, -1, "unavailableTime", _s_set_unavailableTime);
		Utils.RegisterFunc(L, -1, "queueItemId", _s_set_queueItemId);
		Utils.RegisterFunc(L, -1, "queueStartTime", _s_set_queueStartTime);
		Utils.RegisterFunc(L, -1, "queueUpdateTime", _s_set_queueUpdateTime);
		Utils.RegisterFunc(L, -1, "destroyStartTime", _s_set_destroyStartTime);
		Utils.RegisterFunc(L, -1, "destroyEndTime", _s_set_destroyEndTime);
		Utils.RegisterFunc(L, -1, "appearanceId", _s_set_appearanceId);
		Utils.RegisterFunc(L, -1, "specialType", _s_set_specialType);
		Utils.RegisterFunc(L, -1, "positionId", _s_set_positionId);
		Utils.RegisterFunc(L, -1, "virusLayer", _s_set_virusLayer);
		Utils.RegisterFunc(L, -1, "virusEndTime", _s_set_virusEndTime);
		Utils.RegisterFunc(L, -1, "refuseTreadVirus", _s_set_refuseTreadVirus);
		Utils.RegisterFunc(L, -1, "refusePowerHelper", _s_set_refusePowerHelper);
		Utils.RegisterFunc(L, -1, "lightHouseInfo", _s_set_lightHouseInfo);
		Utils.RegisterFunc(L, -1, "recoverSpeed", _s_set_recoverSpeed);
		Utils.RegisterFunc(L, -1, "fireSpeed", _s_set_fireSpeed);
		Utils.RegisterFunc(L, -1, "assistanceCount", _s_set_assistanceCount);
		Utils.RegisterFunc(L, -1, "maxAssistanceCount", _s_set_maxAssistanceCount);
		Utils.RegisterFunc(L, -1, "wolfDecrHp", _s_set_wolfDecrHp);
		Utils.RegisterFunc(L, -1, "fireworksInfoList", _s_set_fireworksInfoList);
		Utils.RegisterFunc(L, -1, "fireworksGiftList", _s_set_fireworksGiftList);
		Utils.RegisterFunc(L, -1, "aosEndTime", _s_set_aosEndTime);
		Utils.RegisterFunc(L, -1, "skillId", _s_set_skillId);
		Utils.RegisterFunc(L, -1, "sandWorm", _s_set_sandWorm);
		Utils.RegisterFunc(L, -1, "mummyConvertId", _s_set_mummyConvertId);
		Utils.RegisterFunc(L, -1, "mummyConvertCount", _s_set_mummyConvertCount);
		Utils.RegisterFunc(L, -1, "skinId", _s_set_skinId);
		Utils.RegisterFunc(L, -1, "colourId", _s_set_colourId);
		Utils.RegisterFunc(L, -1, "colourTime", _s_set_colourTime);
		Utils.RegisterFunc(L, -1, "titleNameSkinId", _s_set_titleNameSkinId);
		Utils.RegisterFunc(L, -1, "effectId", _s_set_effectId);
		Utils.RegisterFunc(L, -1, "wolfCoveredInfo", _s_set_wolfCoveredInfo);
		Utils.RegisterFunc(L, -1, "crystal", _s_set_crystal);
		Utils.RegisterFunc(L, -1, "nucleus", _s_set_nucleus);
		Utils.RegisterFunc(L, -1, "countryFlag", _s_set_countryFlag);
		Utils.RegisterFunc(L, -1, "monsterInvasion", _s_set_monsterInvasion);
		Utils.RegisterFunc(L, -1, "zoneMobilization", _s_set_zoneMobilization);
		Utils.RegisterFunc(L, -1, "commonMonsterSkillInfo", _s_set_commonMonsterSkillInfo);
		Utils.RegisterFunc(L, -1, "challangeNewDonate", _s_set_challangeNewDonate);
		Utils.RegisterFunc(L, -1, "seasonRole", _s_set_seasonRole);
		Utils.RegisterFunc(L, -1, "quarantineSkillId", _s_set_quarantineSkillId);
		Utils.RegisterFunc(L, -1, "quarantineSkillSTime", _s_set_quarantineSkillSTime);
		Utils.RegisterFunc(L, -1, "quarantineSkillETime", _s_set_quarantineSkillETime);
		Utils.RegisterFunc(L, -1, "quarantineSkilTargetPId", _s_set_quarantineSkilTargetPId);
		Utils.RegisterFunc(L, -1, "quarantineRole", _s_set_quarantineRole);
		Utils.RegisterFunc(L, -1, "quarantineLeave", _s_set_quarantineLeave);
		Utils.RegisterFunc(L, -1, "curMaxHp", _s_set_curMaxHp);
		Utils.RegisterFunc(L, -1, "mummyCurseExpireTime", _s_set_mummyCurseExpireTime);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 0, 0);
		Utils.RegisterFunc(L, -4, "GetSkinId", _m_GetSkinId_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetModelPathById", _m_GetModelPathById_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSkinModelNameById", _m_GetSkinModelNameById_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetDefaultModelNameByLevel", _m_GetDefaultModelNameByLevel_xlua_st_);
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
				BuildPointInfo o = new BuildPointInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				BuildPointInfo o2 = new BuildPointInfo((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildPointInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAOSType(IntPtr L)
	{
		try
		{
			int aOSType = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetAOSType();
			Lua.xlua_pushinteger(L, aOSType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAssistanceCount(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateAssistanceCount();
			return 0;
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
			PointInfo o = ((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShowState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			QueueState showState = ((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).GetShowState();
			objectTranslator.Push(L, showState);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetShowStateByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo obj = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			QueueState showStateByIndex = obj.GetShowStateByIndex(index);
			objectTranslator.Push(L, showStateByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsThisState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out QueueState v);
			bool value = buildPointInfo.IsThisState(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSkinId(IntPtr L)
	{
		try
		{
			int skinId = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSkinId();
			Lua.xlua_pushinteger(L, skinId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSkinId_xlua_st_(IntPtr L)
	{
		try
		{
			int skinId = Lua.xlua_tointeger(L, 1);
			int colourId = Lua.xlua_tointeger(L, 2);
			float colourTime = (float)Lua.lua_tonumber(L, 3);
			int skinId2 = BuildPointInfo.GetSkinId(skinId, colourId, colourTime);
			Lua.xlua_pushinteger(L, skinId2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModelPathById_xlua_st_(IntPtr L)
	{
		try
		{
			int skinId = Lua.xlua_tointeger(L, 1);
			int pointType = Lua.xlua_tointeger(L, 2);
			bool isNormal = Lua.lua_toboolean(L, 3);
			int pointIndex = Lua.xlua_tointeger(L, 4);
			int levelId = Lua.xlua_tointeger(L, 5);
			long uuid = Lua.lua_toint64(L, 6);
			string modelPathById = BuildPointInfo.GetModelPathById(skinId, pointType, isNormal, pointIndex, levelId, uuid);
			Lua.lua_pushstring(L, modelPathById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModelPath(IntPtr L)
	{
		try
		{
			string modelPath = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetModelPath();
			Lua.lua_pushstring(L, modelPath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSkinModelNameById_xlua_st_(IntPtr L)
	{
		try
		{
			int skinId = Lua.xlua_tointeger(L, 1);
			int pointType = Lua.xlua_tointeger(L, 2);
			string skinModelNameById = BuildPointInfo.GetSkinModelNameById(skinId, pointType);
			Lua.lua_pushstring(L, skinModelNameById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSkinModelName(IntPtr L)
	{
		try
		{
			string skinModelName = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetSkinModelName();
			Lua.lua_pushstring(L, skinModelName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDefaultModelNameByLevel_xlua_st_(IntPtr L)
	{
		try
		{
			int levelId = Lua.xlua_tointeger(L, 1);
			int pointType = Lua.xlua_tointeger(L, 2);
			string defaultModelNameByLevel = BuildPointInfo.GetDefaultModelNameByLevel(levelId, pointType);
			Lua.lua_pushstring(L, defaultModelNameByLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDefaultModelName(IntPtr L)
	{
		try
		{
			string defaultModelName = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDefaultModelName();
			Lua.lua_pushstring(L, defaultModelName);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshRoadDir(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshRoadDir();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPosIndexListByDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GameDefines.BuildConnectRoadDirection val);
			List<int> posIndexListByDir = buildPointInfo.GetPosIndexListByDir(val);
			objectTranslator.Push(L, posIndexListByDir);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeRoadDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GameDefines.BuildConnectRoadDirection val);
			buildPointInfo.ChangeRoadDir(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNormalType(IntPtr L)
	{
		try
		{
			bool value = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsNormalType();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWormWrap(IntPtr L)
	{
		try
		{
			bool value = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWormWrap();
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
			PlayerType playerType = ((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).GetPlayerType();
			objectTranslator.PushPlayerType(L, playerType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourcePercent(IntPtr L)
	{
		try
		{
			float resourcePercent = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetResourcePercent();
			Lua.lua_pushnumber(L, resourcePercent);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWerewolfBirthing(IntPtr L)
	{
		try
		{
			bool value = ((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsWerewolfBirthing();
			Lua.lua_pushboolean(L, value);
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
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			StringBuilder sb = (StringBuilder)objectTranslator.GetObject(L, 2, typeof(StringBuilder));
			buildPointInfo.OnDescription(sb);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UncoverWerewolf(IntPtr L)
	{
		try
		{
			BuildPointInfo obj = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long now = Lua.lua_toint64(L, 2);
			obj.UncoverWerewolf(now);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AOSType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.AOSType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsWerewolf(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, buildPointInfo.IsWerewolf);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_itemId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.itemId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_level(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.level);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_state(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.state);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_buildState(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.buildState);
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
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, buildPointInfo.allianceId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.startTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_endTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.endTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_inside(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.inside);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_roadDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushGameDefinesBuildConnectRoadDirection(L, buildPointInfo.roadDir);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curHp(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.curHp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastHpTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.lastHpTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_protectEndTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.protectEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_playerName(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, buildPointInfo.playerName);
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
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, buildPointInfo.alAbbr);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastCollectTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.lastCollectTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_unavailableTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.unavailableTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_queueItemId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.queueItemId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_queueStartTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.queueStartTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_queueUpdateTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.queueUpdateTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_destroyStartTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.destroyStartTime);
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
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.destroyEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_appearanceId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.appearanceId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_specialType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.specialType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_positionId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, buildPointInfo.positionId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_virusLayer(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.virusLayer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_virusEndTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.virusEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_refuseTreadVirus(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, buildPointInfo.refuseTreadVirus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_refusePowerHelper(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, buildPointInfo.refusePowerHelper);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lightHouseInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.lightHouseInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_recoverSpeed(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, buildPointInfo.recoverSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fireSpeed(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, buildPointInfo.fireSpeed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_assistanceCount(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.assistanceCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxAssistanceCount(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.maxAssistanceCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wolfDecrHp(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.wolfDecrHp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fireworksInfoList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.fireworksInfoList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fireworksGiftList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.fireworksGiftList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aosEndTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.aosEndTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skillId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.skillId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sandWorm(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.sandWorm);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mummyConvertId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.mummyConvertId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mummyConvertCount(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.mummyConvertCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skinId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.skinId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colourId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.colourId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colourTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, buildPointInfo.colourTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_titleNameSkinId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.titleNameSkinId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_effectId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.effectId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_wolfCoveredInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.wolfCoveredInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_crystal(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.crystal);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nucleus(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.nucleus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_countryFlag(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, buildPointInfo.countryFlag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_monsterInvasion(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.monsterInvasion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_zoneMobilization(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.zoneMobilization);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_commonMonsterSkillInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.commonMonsterSkillInfo);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_challangeNewDonate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, buildPointInfo.challangeNewDonate);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_seasonRole(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.seasonRole);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quarantineSkillId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.quarantineSkillId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quarantineSkillSTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.quarantineSkillSTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quarantineSkillETime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.quarantineSkillETime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quarantineSkilTargetPId(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.quarantineSkilTargetPId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quarantineRole(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.quarantineRole);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_quarantineLeave(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.quarantineLeave);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curMaxHp(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, buildPointInfo.curMaxHp);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mummyCurseExpireTime(IntPtr L)
	{
		try
		{
			BuildPointInfo buildPointInfo = (BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, buildPointInfo.mummyCurseExpireTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_itemId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).itemId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_level(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).level = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_state(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).state = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_buildState(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).buildState = Lua.xlua_tointeger(L, 2);
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
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allianceId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).startTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_endTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).endTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_inside(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).inside = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_roadDir(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out GameDefines.BuildConnectRoadDirection val);
			buildPointInfo.roadDir = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curHp(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).curHp = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lastHpTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lastHpTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_protectEndTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).protectEndTime = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_playerName(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).playerName = Lua.lua_tostring(L, 2);
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
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).alAbbr = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lastCollectTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).lastCollectTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_unavailableTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).unavailableTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_queueItemId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).queueItemId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_queueStartTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).queueStartTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_queueUpdateTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).queueUpdateTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_destroyStartTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).destroyStartTime = Lua.lua_toint64(L, 2);
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
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).destroyEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_appearanceId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).appearanceId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_specialType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo buildPointInfo = (BuildPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Protobuf.SpecialType v);
			buildPointInfo.specialType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_positionId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).positionId = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_virusLayer(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).virusLayer = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_virusEndTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).virusEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_refuseTreadVirus(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refuseTreadVirus = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_refusePowerHelper(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).refusePowerHelper = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lightHouseInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).lightHouseInfo = (LightHouseInfo)objectTranslator.GetObject(L, 2, typeof(LightHouseInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_recoverSpeed(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).recoverSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fireSpeed(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fireSpeed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_assistanceCount(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).assistanceCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxAssistanceCount(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxAssistanceCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wolfDecrHp(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).wolfDecrHp = (byte)Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fireworksInfoList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).fireworksInfoList = (RepeatedField<FireWorksInfo>)objectTranslator.GetObject(L, 2, typeof(RepeatedField<FireWorksInfo>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fireworksGiftList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).fireworksGiftList = (RepeatedField<FireWorksGift>)objectTranslator.GetObject(L, 2, typeof(RepeatedField<FireWorksGift>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aosEndTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).aosEndTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skillId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).skillId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sandWorm(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).sandWorm = (SandWormData)objectTranslator.GetObject(L, 2, typeof(SandWormData));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mummyConvertId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mummyConvertId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mummyConvertCount(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mummyConvertCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skinId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).skinId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colourId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).colourId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colourTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).colourTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_titleNameSkinId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).titleNameSkinId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_effectId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).effectId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_wolfCoveredInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).wolfCoveredInfo = (BuildPointInfo.WolfCoveredInfo)objectTranslator.GetObject(L, 2, typeof(BuildPointInfo.WolfCoveredInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_crystal(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).crystal = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nucleus(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).nucleus = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_countryFlag(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).countryFlag = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_monsterInvasion(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).monsterInvasion = (MonsterInvasion)objectTranslator.GetObject(L, 2, typeof(MonsterInvasion));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_zoneMobilization(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).zoneMobilization = (ZoneMobilization)objectTranslator.GetObject(L, 2, typeof(ZoneMobilization));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_commonMonsterSkillInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).commonMonsterSkillInfo = (CommonMonsterSkillInfo)objectTranslator.GetObject(L, 2, typeof(CommonMonsterSkillInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_challangeNewDonate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((BuildPointInfo)objectTranslator.FastGetCSObj(L, 1)).challangeNewDonate = (MonsterChallengeNewDonate)objectTranslator.GetObject(L, 2, typeof(MonsterChallengeNewDonate));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_seasonRole(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).seasonRole = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quarantineSkillId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quarantineSkillId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quarantineSkillSTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quarantineSkillSTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quarantineSkillETime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quarantineSkillETime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quarantineSkilTargetPId(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quarantineSkilTargetPId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quarantineRole(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quarantineRole = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_quarantineLeave(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).quarantineLeave = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curMaxHp(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).curMaxHp = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mummyCurseExpireTime(IntPtr L)
	{
		try
		{
			((BuildPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mummyCurseExpireTime = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
