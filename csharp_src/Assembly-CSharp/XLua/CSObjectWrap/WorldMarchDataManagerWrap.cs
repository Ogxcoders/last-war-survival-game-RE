using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldMarchDataManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldMarchDataManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 77, 5, 0);
		Utils.RegisterFunc(L, -3, "UpdateBattleMessage", _m_UpdateBattleMessage);
		Utils.RegisterFunc(L, -3, "BattleFinish", _m_BattleFinish);
		Utils.RegisterFunc(L, -3, "UpdateBattle", _m_UpdateBattle);
		Utils.RegisterFunc(L, -3, "SetWorldScene", _m_SetWorldScene);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "HandleWorldMarchGet", _m_HandleWorldMarchGet);
		Utils.RegisterFunc(L, -3, "HandleWorldMarchGetImpl", _m_HandleWorldMarchGetImpl);
		Utils.RegisterFunc(L, -3, "HandleWorldGetRectMarchInfos", _m_HandleWorldGetRectMarchInfos);
		Utils.RegisterFunc(L, -3, "CheckIsOtherTarget", _m_CheckIsOtherTarget);
		Utils.RegisterFunc(L, -3, "HandlePushWorldMarchAdd", _m_HandlePushWorldMarchAdd);
		Utils.RegisterFunc(L, -3, "HandlePushWorldMarchAddImpl", _m_HandlePushWorldMarchAddImpl);
		Utils.RegisterFunc(L, -3, "ParsePushWorldMarchAdd", _m_ParsePushWorldMarchAdd);
		Utils.RegisterFunc(L, -3, "HandlePushWorldMarchDel", _m_HandlePushWorldMarchDel);
		Utils.RegisterFunc(L, -3, "HandlePushWorldMarchDelImpl", _m_HandlePushWorldMarchDelImpl);
		Utils.RegisterFunc(L, -3, "HandleFormationMarch", _m_HandleFormationMarch);
		Utils.RegisterFunc(L, -3, "HandleFormationMarchImpl", _m_HandleFormationMarchImpl);
		Utils.RegisterFunc(L, -3, "HandleFormationMarchChange", _m_HandleFormationMarchChange);
		Utils.RegisterFunc(L, -3, "HandleFormationMarchChangeImpl", _m_HandleFormationMarchChangeImpl);
		Utils.RegisterFunc(L, -3, "DelFakeAttackMonsterMarchDataRandom", _m_DelFakeAttackMonsterMarchDataRandom);
		Utils.RegisterFunc(L, -3, "AddFakeAttackMonsterMarchData", _m_AddFakeAttackMonsterMarchData);
		Utils.RegisterFunc(L, -3, "UpdateFakeAttackMonsterMarch", _m_UpdateFakeAttackMonsterMarch);
		Utils.RegisterFunc(L, -3, "AddFakeRetreatMarchData", _m_AddFakeRetreatMarchData);
		Utils.RegisterFunc(L, -3, "RemoveFakeRetreatMarchData", _m_RemoveFakeRetreatMarchData);
		Utils.RegisterFunc(L, -3, "AddFakeSampleMarchData", _m_AddFakeSampleMarchData);
		Utils.RegisterFunc(L, -3, "GetAllSampleFakeData", _m_GetAllSampleFakeData);
		Utils.RegisterFunc(L, -3, "UpdateFakeSampleMarchDataWhenStartPick", _m_UpdateFakeSampleMarchDataWhenStartPick);
		Utils.RegisterFunc(L, -3, "UpdateFakeSampleMarchDataWhenBack", _m_UpdateFakeSampleMarchDataWhenBack);
		Utils.RegisterFunc(L, -3, "RemoveFakeSampleMarchData", _m_RemoveFakeSampleMarchData);
		Utils.RegisterFunc(L, -3, "ExistMarch", _m_ExistMarch);
		Utils.RegisterFunc(L, -3, "IsInRallyMarch", _m_IsInRallyMarch);
		Utils.RegisterFunc(L, -3, "IsInCollectMarch", _m_IsInCollectMarch);
		Utils.RegisterFunc(L, -3, "IsInAssistanceMarch", _m_IsInAssistanceMarch);
		Utils.RegisterFunc(L, -3, "IsSelfInCurrentMarchTeam", _m_IsSelfInCurrentMarchTeam);
		Utils.RegisterFunc(L, -3, "HasMarchUuid", _m_HasMarchUuid);
		Utils.RegisterFunc(L, -3, "GetMarch", _m_GetMarch);
		Utils.RegisterFunc(L, -3, "GetMonster", _m_GetMonster);
		Utils.RegisterFunc(L, -3, "GetOwnerMarches", _m_GetOwnerMarches);
		Utils.RegisterFunc(L, -3, "GetMyDisguiseMarches", _m_GetMyDisguiseMarches);
		Utils.RegisterFunc(L, -3, "GetAllianceMarchesInTeam", _m_GetAllianceMarchesInTeam);
		Utils.RegisterFunc(L, -3, "IsHaveMarchInWorld", _m_IsHaveMarchInWorld);
		Utils.RegisterFunc(L, -3, "GetBestMarch", _m_GetBestMarch);
		Utils.RegisterFunc(L, -3, "CleanDragonWar", _m_CleanDragonWar);
		Utils.RegisterFunc(L, -3, "GetOwnerFormationMarch", _m_GetOwnerFormationMarch);
		Utils.RegisterFunc(L, -3, "StartMarch", _m_StartMarch);
		Utils.RegisterFunc(L, -3, "WorldGetMarchInfos", _m_WorldGetMarchInfos);
		Utils.RegisterFunc(L, -3, "GetMyAssistanceCountByPointIndex", _m_GetMyAssistanceCountByPointIndex);
		Utils.RegisterFunc(L, -3, "GetMyAssistanceFirstHero", _m_GetMyAssistanceFirstHero);
		Utils.RegisterFunc(L, -3, "GetFirstMyAssistanceMarchUuid", _m_GetFirstMyAssistanceMarchUuid);
		Utils.RegisterFunc(L, -3, "UIMainWarningHide", _m_UIMainWarningHide);
		Utils.RegisterFunc(L, -3, "GetAllMarchesByCS", _m_GetAllMarchesByCS);
		Utils.RegisterFunc(L, -3, "GetMarchesByStartIndex", _m_GetMarchesByStartIndex);
		Utils.RegisterFunc(L, -3, "GetMarchByTargetPos", _m_GetMarchByTargetPos);
		Utils.RegisterFunc(L, -3, "GetMarchByType", _m_GetMarchByType);
		Utils.RegisterFunc(L, -3, "GetMarchCountToTargetGroupByOwnerUid", _m_GetMarchCountToTargetGroupByOwnerUid);
		Utils.RegisterFunc(L, -3, "GetMonsterListInArea", _m_GetMonsterListInArea);
		Utils.RegisterFunc(L, -3, "GetMarchesBossInfo", _m_GetMarchesBossInfo);
		Utils.RegisterFunc(L, -3, "GetMarchesTargetForMine", _m_GetMarchesTargetForMine);
		Utils.RegisterFunc(L, -3, "GetMarchesTargetForMineLite", _m_GetMarchesTargetForMineLite);
		Utils.RegisterFunc(L, -3, "GetMarchesTargetForMineCached", _m_GetMarchesTargetForMineCached);
		Utils.RegisterFunc(L, -3, "GetInimicalMarchesTargetForMine", _m_GetInimicalMarchesTargetForMine);
		Utils.RegisterFunc(L, -3, "IsTargetForMine", _m_IsTargetForMine);
		Utils.RegisterFunc(L, -3, "IsTargetForAlly", _m_IsTargetForAlly);
		Utils.RegisterFunc(L, -3, "GetTrainConfig", _m_GetTrainConfig);
		Utils.RegisterFunc(L, -3, "GetFlowerCarLength", _m_GetFlowerCarLength);
		Utils.RegisterFunc(L, -3, "CacheAllianceMembersHomePos", _m_CacheAllianceMembersHomePos);
		Utils.RegisterFunc(L, -3, "CleanAllianceMembersHomePos", _m_CleanAllianceMembersHomePos);
		Utils.RegisterFunc(L, -3, "IsMemberByPointId", _m_IsMemberByPointId);
		Utils.RegisterFunc(L, -3, "DestroyBerserkBossMarchData", _m_DestroyBerserkBossMarchData);
		Utils.RegisterFunc(L, -3, "SaveCreateMarchRecordTime", _m_SaveCreateMarchRecordTime);
		Utils.RegisterFunc(L, -3, "RemoveCreateMarchRecordTime", _m_RemoveCreateMarchRecordTime);
		Utils.RegisterFunc(L, -3, "UpdateBattleSoundData", _m_UpdateBattleSoundData);
		Utils.RegisterFunc(L, -3, "EditorDescription", _m_EditorDescription);
		Utils.RegisterFunc(L, -3, "AddMarch", _m_AddMarch);
		Utils.RegisterFunc(L, -3, "StartRecordMarchBlock", _m_StartRecordMarchBlock);
		Utils.RegisterFunc(L, -3, "StopRecordMarchBlock", _m_StopRecordMarchBlock);
		Utils.RegisterFunc(L, -2, "AllMarchesCount", _g_get_AllMarchesCount);
		Utils.RegisterFunc(L, -2, "EnableWorldAssistanceOpt", _g_get_EnableWorldAssistanceOpt);
		Utils.RegisterFunc(L, -2, "lastPerformanceCount", _g_get_lastPerformanceCount);
		Utils.RegisterFunc(L, -2, "lastPerformanceTroopLineCount", _g_get_lastPerformanceTroopLineCount);
		Utils.RegisterFunc(L, -2, "TileBlockIndex", _g_get_TileBlockIndex);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 5, 6, 4);
		Utils.RegisterFunc(L, -4, "AxisAlignRectIntersectAxisAlignSegment", _m_AxisAlignRectIntersectAxisAlignSegment_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsMyMarch", _m_IsMyMarch_xlua_st_);
		Utils.RegisterFunc(L, -4, "EditorLog", _m_EditorLog_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "normalAttackId", 100000);
		Utils.RegisterFunc(L, -2, "__INC_UPDATE__", _g_get___INC_UPDATE__);
		Utils.RegisterFunc(L, -2, "EditorInstance", _g_get_EditorInstance);
		Utils.RegisterFunc(L, -2, "OnMonsterAdd", _g_get_OnMonsterAdd);
		Utils.RegisterFunc(L, -2, "OnMonsterDelete", _g_get_OnMonsterDelete);
		Utils.RegisterFunc(L, -2, "SelectMarchUuid", _g_get_SelectMarchUuid);
		Utils.RegisterFunc(L, -2, "DebugMarchUuid", _g_get_DebugMarchUuid);
		Utils.RegisterFunc(L, -1, "OnMonsterAdd", _s_set_OnMonsterAdd);
		Utils.RegisterFunc(L, -1, "OnMonsterDelete", _s_set_OnMonsterDelete);
		Utils.RegisterFunc(L, -1, "SelectMarchUuid", _s_set_SelectMarchUuid);
		Utils.RegisterFunc(L, -1, "DebugMarchUuid", _s_set_DebugMarchUuid);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldScene>(L, 2))
			{
				WorldMarchDataManager o = new WorldMarchDataManager((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateBattleMessage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.UpdateBattleMessage(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BattleFinish(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.BattleFinish(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateBattle(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.UpdateBattle(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorldScene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldScene worldScene = (WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene));
			worldMarchDataManager.SetWorldScene(worldScene);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdate(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float deltaTime = (float)Lua.lua_tonumber(L, 2);
			obj.OnUpdate(deltaTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AxisAlignRectIntersectAxisAlignSegment_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Rect v);
			objectTranslator.Get(L, 2, out Vector2 val);
			objectTranslator.Get(L, 3, out Vector2 val2);
			bool value = WorldMarchDataManager.AxisAlignRectIntersectAxisAlignSegment(v, val, val2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleWorldMarchGet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleWorldMarchGet(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleWorldMarchGetImpl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleWorldMarchGetImpl(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleWorldGetRectMarchInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleWorldGetRectMarchInfos(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckIsOtherTarget(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			int targetPos = Lua.xlua_tointeger(L, 3);
			bool value = obj.CheckIsOtherTarget(uuid, targetPos);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldMarchAdd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandlePushWorldMarchAdd(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldMarchAddImpl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandlePushWorldMarchAddImpl(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ParsePushWorldMarchAdd(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			objectTranslator.Get(L, 3, out WorldMarchMessageType v);
			worldMarchDataManager.ParsePushWorldMarchAdd(message, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldMarchDel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandlePushWorldMarchDel(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldMarchDelImpl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandlePushWorldMarchDelImpl(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleFormationMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleFormationMarch(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleFormationMarchImpl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleFormationMarchImpl(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleFormationMarchChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleFormationMarchChange(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleFormationMarchChangeImpl(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldMarchDataManager.HandleFormationMarchChangeImpl(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DelFakeAttackMonsterMarchDataRandom(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int count = Lua.xlua_tointeger(L, 2);
			obj.DelFakeAttackMonsterMarchDataRandom(count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddFakeAttackMonsterMarchData(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long startIndex = Lua.lua_toint64(L, 2);
			long endIndex = Lua.lua_toint64(L, 3);
			float marchTimeSec = (float)Lua.lua_tonumber(L, 4);
			string ownerUid = Lua.lua_tostring(L, 5);
			obj.AddFakeAttackMonsterMarchData(startIndex, endIndex, marchTimeSec, ownerUid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFakeAttackMonsterMarch(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			obj.UpdateFakeAttackMonsterMarch(uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddFakeRetreatMarchData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch realMarch = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			PointInfo info = (PointInfo)objectTranslator.GetObject(L, 3, typeof(PointInfo));
			int timeDelta = Lua.xlua_tointeger(L, 4);
			worldMarchDataManager.AddFakeRetreatMarchData(realMarch, info, timeDelta);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveFakeRetreatMarchData(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long realUuid = Lua.lua_toint64(L, 2);
			obj.RemoveFakeRetreatMarchData(realUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddFakeSampleMarchData(IntPtr L)
	{
		try
		{
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 8 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) || Lua.lua_isint64(L, 5)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				long startIndex = Lua.lua_toint64(L, 2);
				long endIndex = Lua.lua_toint64(L, 3);
				long startTime = Lua.lua_toint64(L, 4);
				long endTime = Lua.lua_toint64(L, 5);
				int marchTargetType = Lua.xlua_tointeger(L, 6);
				int srcServer = Lua.xlua_tointeger(L, 7);
				int targetServer = Lua.xlua_tointeger(L, 8);
				worldMarchDataManager.AddFakeSampleMarchData(startIndex, endIndex, startTime, endTime, marchTargetType, srcServer, targetServer);
				return 0;
			}
			if (num == 7 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) || Lua.lua_isint64(L, 5)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				long startIndex2 = Lua.lua_toint64(L, 2);
				long endIndex2 = Lua.lua_toint64(L, 3);
				long startTime2 = Lua.lua_toint64(L, 4);
				long endTime2 = Lua.lua_toint64(L, 5);
				int marchTargetType2 = Lua.xlua_tointeger(L, 6);
				int srcServer2 = Lua.xlua_tointeger(L, 7);
				worldMarchDataManager.AddFakeSampleMarchData(startIndex2, endIndex2, startTime2, endTime2, marchTargetType2, srcServer2);
				return 0;
			}
			if (num == 6 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) || Lua.lua_isint64(L, 5)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				long startIndex3 = Lua.lua_toint64(L, 2);
				long endIndex3 = Lua.lua_toint64(L, 3);
				long startTime3 = Lua.lua_toint64(L, 4);
				long endTime3 = Lua.lua_toint64(L, 5);
				int marchTargetType3 = Lua.xlua_tointeger(L, 6);
				worldMarchDataManager.AddFakeSampleMarchData(startIndex3, endIndex3, startTime3, endTime3, marchTargetType3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager.AddFakeSampleMarchData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllSampleFakeData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldMarch> allSampleFakeData = ((WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1)).GetAllSampleFakeData();
			objectTranslator.Push(L, allSampleFakeData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFakeSampleMarchDataWhenStartPick(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long index = Lua.lua_toint64(L, 2);
			long endTime = Lua.lua_toint64(L, 3);
			obj.UpdateFakeSampleMarchDataWhenStartPick(index, endTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateFakeSampleMarchDataWhenBack(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long index = Lua.lua_toint64(L, 2);
			long startTime = Lua.lua_toint64(L, 3);
			long endTime = Lua.lua_toint64(L, 4);
			obj.UpdateFakeSampleMarchDataWhenBack(index, startTime, endTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveFakeSampleMarchData(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long index = Lua.lua_toint64(L, 2);
			obj.RemoveFakeSampleMarchData(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExistMarch(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.ExistMarch(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInRallyMarch(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsInRallyMarch(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInCollectMarch(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsInCollectMarch(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInAssistanceMarch(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsInAssistanceMarch(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSelfInCurrentMarchTeam(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long rallyMarchUuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsSelfInCurrentMarchTeam(rallyMarchUuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasMarchUuid(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.HasMarchUuid(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			WorldMarch march = obj.GetMarch(uuid);
			objectTranslator.Push(L, march);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMonster(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			long targetPoint = Lua.lua_toint64(L, 2);
			WorldMarch monster = obj.GetMonster(targetPoint);
			objectTranslator.Push(L, monster);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOwnerMarches(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid = Lua.lua_tostring(L, 2);
				string allianceUid = Lua.lua_tostring(L, 3);
				List<WorldMarch> ownerMarches = worldMarchDataManager.GetOwnerMarches(ownerUid, allianceUid);
				objectTranslator.Push(L, ownerMarches);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid2 = Lua.lua_tostring(L, 2);
				List<WorldMarch> ownerMarches2 = worldMarchDataManager.GetOwnerMarches(ownerUid2);
				objectTranslator.Push(L, ownerMarches2);
				return 1;
			}
			if (num == 1)
			{
				List<WorldMarch> ownerMarches3 = worldMarchDataManager.GetOwnerMarches();
				objectTranslator.Push(L, ownerMarches3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager.GetOwnerMarches!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyDisguiseMarches(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid = Lua.lua_tostring(L, 2);
				string allianceUid = Lua.lua_tostring(L, 3);
				List<WorldMarch> myDisguiseMarches = worldMarchDataManager.GetMyDisguiseMarches(ownerUid, allianceUid);
				objectTranslator.Push(L, myDisguiseMarches);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid2 = Lua.lua_tostring(L, 2);
				List<WorldMarch> myDisguiseMarches2 = worldMarchDataManager.GetMyDisguiseMarches(ownerUid2);
				objectTranslator.Push(L, myDisguiseMarches2);
				return 1;
			}
			if (num == 1)
			{
				List<WorldMarch> myDisguiseMarches3 = worldMarchDataManager.GetMyDisguiseMarches();
				objectTranslator.Push(L, myDisguiseMarches3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager.GetMyDisguiseMarches!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceMarchesInTeam(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			string allianceUid = Lua.lua_tostring(L, 2);
			long teamUuid = Lua.lua_toint64(L, 3);
			WorldMarch allianceMarchesInTeam = obj.GetAllianceMarchesInTeam(allianceUid, teamUuid);
			objectTranslator.Push(L, allianceMarchesInTeam);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsHaveMarchInWorld(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceUid = Lua.lua_tostring(L, 2);
			long teamUuid = Lua.lua_toint64(L, 3);
			bool value = obj.IsHaveMarchInWorld(allianceUid, teamUuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBestMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			string allianceId = Lua.lua_tostring(L, 3);
			int worldId = Lua.xlua_tointeger(L, 4);
			WorldMarch bestMarch = obj.GetBestMarch(pointId, allianceId, worldId);
			objectTranslator.Push(L, bestMarch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CleanDragonWar(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CleanDragonWar();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOwnerFormationMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid = Lua.lua_tostring(L, 2);
				long formationUuid = Lua.lua_toint64(L, 3);
				string allianceUid = Lua.lua_tostring(L, 4);
				WorldMarch ownerFormationMarch = worldMarchDataManager.GetOwnerFormationMarch(ownerUid, formationUuid, allianceUid);
				objectTranslator.Push(L, ownerFormationMarch);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				string ownerUid2 = Lua.lua_tostring(L, 2);
				long formationUuid2 = Lua.lua_toint64(L, 3);
				WorldMarch ownerFormationMarch2 = worldMarchDataManager.GetOwnerFormationMarch(ownerUid2, formationUuid2);
				objectTranslator.Push(L, ownerFormationMarch2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager.GetOwnerFormationMarch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartMarch(IntPtr L)
	{
		try
		{
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 11 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) || Lua.lua_isint64(L, 7)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && (Lua.lua_isnil(L, 9) || Lua.lua_type(L, 9) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 11))
			{
				int targetType = Lua.xlua_tointeger(L, 2);
				int targetPoint = Lua.xlua_tointeger(L, 3);
				long targetUuid = Lua.lua_toint64(L, 4);
				int timeIndex = Lua.xlua_tointeger(L, 5);
				long marchUuid = Lua.lua_toint64(L, 6);
				long formationUuid = Lua.lua_toint64(L, 7);
				int backHome = Lua.xlua_tointeger(L, 8);
				byte[] sfsObjBinary = Lua.lua_tobytes(L, 9);
				int startPos = Lua.xlua_tointeger(L, 10);
				int targetServerId = Lua.xlua_tointeger(L, 11);
				worldMarchDataManager.StartMarch(targetType, targetPoint, targetUuid, timeIndex, marchUuid, formationUuid, backHome, sfsObjBinary, startPos, targetServerId);
				return 0;
			}
			if (num == 10 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) || Lua.lua_isint64(L, 7)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && (Lua.lua_isnil(L, 9) || Lua.lua_type(L, 9) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 10))
			{
				int targetType2 = Lua.xlua_tointeger(L, 2);
				int targetPoint2 = Lua.xlua_tointeger(L, 3);
				long targetUuid2 = Lua.lua_toint64(L, 4);
				int timeIndex2 = Lua.xlua_tointeger(L, 5);
				long marchUuid2 = Lua.lua_toint64(L, 6);
				long formationUuid2 = Lua.lua_toint64(L, 7);
				int backHome2 = Lua.xlua_tointeger(L, 8);
				byte[] sfsObjBinary2 = Lua.lua_tobytes(L, 9);
				int startPos2 = Lua.xlua_tointeger(L, 10);
				worldMarchDataManager.StartMarch(targetType2, targetPoint2, targetUuid2, timeIndex2, marchUuid2, formationUuid2, backHome2, sfsObjBinary2, startPos2);
				return 0;
			}
			if (num == 9 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) || Lua.lua_isint64(L, 7)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && (Lua.lua_isnil(L, 9) || Lua.lua_type(L, 9) == LuaTypes.LUA_TSTRING))
			{
				int targetType3 = Lua.xlua_tointeger(L, 2);
				int targetPoint3 = Lua.xlua_tointeger(L, 3);
				long targetUuid3 = Lua.lua_toint64(L, 4);
				int timeIndex3 = Lua.xlua_tointeger(L, 5);
				long marchUuid3 = Lua.lua_toint64(L, 6);
				long formationUuid3 = Lua.lua_toint64(L, 7);
				int backHome3 = Lua.xlua_tointeger(L, 8);
				byte[] sfsObjBinary3 = Lua.lua_tobytes(L, 9);
				worldMarchDataManager.StartMarch(targetType3, targetPoint3, targetUuid3, timeIndex3, marchUuid3, formationUuid3, backHome3, sfsObjBinary3);
				return 0;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) || Lua.lua_isint64(L, 7)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				int targetType4 = Lua.xlua_tointeger(L, 2);
				int targetPoint4 = Lua.xlua_tointeger(L, 3);
				long targetUuid4 = Lua.lua_toint64(L, 4);
				int timeIndex4 = Lua.xlua_tointeger(L, 5);
				long marchUuid4 = Lua.lua_toint64(L, 6);
				long formationUuid4 = Lua.lua_toint64(L, 7);
				int backHome4 = Lua.xlua_tointeger(L, 8);
				worldMarchDataManager.StartMarch(targetType4, targetPoint4, targetUuid4, timeIndex4, marchUuid4, formationUuid4, backHome4);
				return 0;
			}
			if (num == 7 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) || Lua.lua_isint64(L, 7)))
			{
				int targetType5 = Lua.xlua_tointeger(L, 2);
				int targetPoint5 = Lua.xlua_tointeger(L, 3);
				long targetUuid5 = Lua.lua_toint64(L, 4);
				int timeIndex5 = Lua.xlua_tointeger(L, 5);
				long marchUuid5 = Lua.lua_toint64(L, 6);
				long formationUuid5 = Lua.lua_toint64(L, 7);
				worldMarchDataManager.StartMarch(targetType5, targetPoint5, targetUuid5, timeIndex5, marchUuid5, formationUuid5);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)))
			{
				int targetType6 = Lua.xlua_tointeger(L, 2);
				int targetPoint6 = Lua.xlua_tointeger(L, 3);
				long targetUuid6 = Lua.lua_toint64(L, 4);
				int timeIndex6 = Lua.xlua_tointeger(L, 5);
				long marchUuid6 = Lua.lua_toint64(L, 6);
				worldMarchDataManager.StartMarch(targetType6, targetPoint6, targetUuid6, timeIndex6, marchUuid6, 0L);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int targetType7 = Lua.xlua_tointeger(L, 2);
				int targetPoint7 = Lua.xlua_tointeger(L, 3);
				long targetUuid7 = Lua.lua_toint64(L, 4);
				int timeIndex7 = Lua.xlua_tointeger(L, 5);
				worldMarchDataManager.StartMarch(targetType7, targetPoint7, targetUuid7, timeIndex7, 0L, 0L);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager.StartMarch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldGetMarchInfos(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).WorldGetMarchInfos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMyMarch_xlua_st_(IntPtr L)
	{
		try
		{
			bool value = WorldMarchDataManager.IsMyMarch((WorldMarch)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(WorldMarch)));
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyAssistanceCountByPointIndex(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			int myAssistanceCountByPointIndex = obj.GetMyAssistanceCountByPointIndex(pointIndex);
			Lua.xlua_pushinteger(L, myAssistanceCountByPointIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyAssistanceFirstHero(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			int myAssistanceFirstHero = obj.GetMyAssistanceFirstHero(pointIndex);
			Lua.xlua_pushinteger(L, myAssistanceFirstHero);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFirstMyAssistanceMarchUuid(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			long firstMyAssistanceMarchUuid = obj.GetFirstMyAssistanceMarchUuid(pointIndex);
			Lua.lua_pushint64(L, firstMyAssistanceMarchUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIMainWarningHide(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WarningType v);
			IEnumerator o = worldMarchDataManager.UIMainWarningHide(v);
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllMarchesByCS(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldMarch> allMarchesByCS = ((WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1)).GetAllMarchesByCS();
			objectTranslator.Push(L, allMarchesByCS);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchesByStartIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int posIndex = Lua.xlua_tointeger(L, 2);
			WorldMarch marchesByStartIndex = obj.GetMarchesByStartIndex(posIndex);
			objectTranslator.Push(L, marchesByStartIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchByTargetPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int targetPos = Lua.xlua_tointeger(L, 2);
			WorldMarch marchByTargetPos = obj.GetMarchByTargetPos(targetPos);
			objectTranslator.Push(L, marchByTargetPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchByType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int marchType = Lua.xlua_tointeger(L, 2);
			WorldMarch marchByType = obj.GetMarchByType(marchType);
			objectTranslator.Push(L, marchByType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchCountToTargetGroupByOwnerUid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int targetPos = Lua.xlua_tointeger(L, 2);
			Dictionary<string, int> marchCountToTargetGroupByOwnerUid = obj.GetMarchCountToTargetGroupByOwnerUid(targetPos);
			objectTranslator.Push(L, marchCountToTargetGroupByOwnerUid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMonsterListInArea(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int size = Lua.xlua_tointeger(L, 3);
			Dictionary<int, int> monsterIds = (Dictionary<int, int>)objectTranslator.GetObject(L, 4, typeof(Dictionary<int, int>));
			Dictionary<long, Vector2Int> result = (Dictionary<long, Vector2Int>)objectTranslator.GetObject(L, 5, typeof(Dictionary<long, Vector2Int>));
			worldMarchDataManager.GetMonsterListInArea(v, size, monsterIds, result);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchesBossInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldMarch> marchesBossInfo = ((WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1)).GetMarchesBossInfo();
			objectTranslator.Push(L, marchesBossInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchesTargetForMine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			bool onlyDesertBattle = Lua.lua_toboolean(L, 2);
			Dictionary<long, WorldMarch> marchesTargetForMine = obj.GetMarchesTargetForMine(onlyDesertBattle);
			objectTranslator.Push(L, marchesTargetForMine);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchesTargetForMineLite(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldMarch> marchesTargetForMineLite = ((WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1)).GetMarchesTargetForMineLite();
			objectTranslator.Push(L, marchesTargetForMineLite);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMarchesTargetForMineCached(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			bool onlyDesertBattle = Lua.lua_toboolean(L, 2);
			Dictionary<long, WorldMarch> marchesTargetForMineCached = obj.GetMarchesTargetForMineCached(onlyDesertBattle);
			objectTranslator.Push(L, marchesTargetForMineCached);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInimicalMarchesTargetForMine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldMarch> inimicalMarchesTargetForMine = ((WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1)).GetInimicalMarchesTargetForMine();
			objectTranslator.Push(L, inimicalMarchesTargetForMine);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTargetForMine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<WorldMarch>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				bool onlyAttack = Lua.lua_toboolean(L, 3);
				bool isDesertBattle = Lua.lua_toboolean(L, 4);
				bool value = worldMarchDataManager.IsTargetForMine(march, onlyAttack, isDesertBattle);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<WorldMarch>(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				WorldMarch march2 = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				bool onlyAttack2 = Lua.lua_toboolean(L, 3);
				bool value2 = worldMarchDataManager.IsTargetForMine(march2, onlyAttack2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<WorldMarch>(L, 2))
			{
				WorldMarch march3 = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				bool value3 = worldMarchDataManager.IsTargetForMine(march3);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldMarchDataManager.IsTargetForMine!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTargetForAlly(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			bool value = worldMarchDataManager.IsTargetForAlly(march);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTrainConfig(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager obj = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			WorldTrainConfig trainConfig = obj.GetTrainConfig(id);
			objectTranslator.Push(L, trainConfig);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFlowerCarLength(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			int flowerCarLength = obj.GetFlowerCarLength(id);
			Lua.xlua_pushinteger(L, flowerCarLength);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CacheAllianceMembersHomePos(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CacheAllianceMembersHomePos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CleanAllianceMembersHomePos(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CleanAllianceMembersHomePos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMemberByPointId(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsMemberByPointId(pointId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyBerserkBossMarchData(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			obj.DestroyBerserkBossMarchData(marchUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SaveCreateMarchRecordTime(IntPtr L)
	{
		try
		{
			string str = ((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SaveCreateMarchRecordTime();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCreateMarchRecordTime(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldMarchDataManager.RemoveCreateMarchRecordTime(worldMarch);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateBattleSoundData(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateBattleSoundData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorLog_xlua_st_(IntPtr L)
	{
		try
		{
			Lua.lua_tostring(L, 1);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EditorDescription(IntPtr L)
	{
		try
		{
			string str = ((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EditorDescription();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			worldMarchDataManager.AddMarch(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartRecordMarchBlock(IntPtr L)
	{
		try
		{
			WorldMarchDataManager obj = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int worldId = Lua.xlua_tointeger(L, 2);
			obj.StartRecordMarchBlock(worldId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopRecordMarchBlock(IntPtr L)
	{
		try
		{
			((WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopRecordMarchBlock();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get___INC_UPDATE__(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, WorldMarchDataManager.__INC_UPDATE__);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AllMarchesCount(IntPtr L)
	{
		try
		{
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarchDataManager.AllMarchesCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EnableWorldAssistanceOpt(IntPtr L)
	{
		try
		{
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldMarchDataManager.EnableWorldAssistanceOpt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastPerformanceCount(IntPtr L)
	{
		try
		{
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarchDataManager.lastPerformanceCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lastPerformanceTroopLineCount(IntPtr L)
	{
		try
		{
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldMarchDataManager.lastPerformanceTroopLineCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EditorInstance(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldMarchDataManager.EditorInstance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TileBlockIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldMarchDataManager worldMarchDataManager = (WorldMarchDataManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldMarchDataManager.TileBlockIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnMonsterAdd(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldMarchDataManager.OnMonsterAdd);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnMonsterDelete(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldMarchDataManager.OnMonsterDelete);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SelectMarchUuid(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, WorldMarchDataManager.SelectMarchUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DebugMarchUuid(IntPtr L)
	{
		try
		{
			Lua.lua_pushint64(L, WorldMarchDataManager.DebugMarchUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnMonsterAdd(IntPtr L)
	{
		try
		{
			WorldMarchDataManager.OnMonsterAdd = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<long, int, int, int>>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnMonsterDelete(IntPtr L)
	{
		try
		{
			WorldMarchDataManager.OnMonsterDelete = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Action<long, int, int, int>>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SelectMarchUuid(IntPtr L)
	{
		try
		{
			WorldMarchDataManager.SelectMarchUuid = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DebugMarchUuid(IntPtr L)
	{
		try
		{
			WorldMarchDataManager.DebugMarchUuid = Lua.lua_toint64(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
