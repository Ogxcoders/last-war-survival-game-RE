using System;
using System.Collections.Generic;
using Protobuf;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldPointManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldPointManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 105, 6, 2);
		Utils.RegisterFunc(L, -3, "RegisterLodWatcher", _m_RegisterLodWatcher);
		Utils.RegisterFunc(L, -3, "UnregisterLodWatcher", _m_UnregisterLodWatcher);
		Utils.RegisterFunc(L, -3, "Description", _m_Description);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "RemoveAllObject", _m_RemoveAllObject);
		Utils.RegisterFunc(L, -3, "IsBuildFinish", _m_IsBuildFinish);
		Utils.RegisterFunc(L, -3, "GetPointInfo", _m_GetPointInfo);
		Utils.RegisterFunc(L, -3, "GetWorldDesertInfo", _m_GetWorldDesertInfo);
		Utils.RegisterFunc(L, -3, "GetWorldTileInfo", _m_GetWorldTileInfo);
		Utils.RegisterFunc(L, -3, "GetYellowLand", _m_GetYellowLand);
		Utils.RegisterFunc(L, -3, "GetBuildTileByItemId", _m_GetBuildTileByItemId);
		Utils.RegisterFunc(L, -3, "GetAllianceCitySizeByItemId", _m_GetAllianceCitySizeByItemId);
		Utils.RegisterFunc(L, -3, "GetTreasureSizeByItemId", _m_GetTreasureSizeByItemId);
		Utils.RegisterFunc(L, -3, "GetDragonBuildSizeByItemId", _m_GetDragonBuildSizeByItemId);
		Utils.RegisterFunc(L, -3, "GetAllianceCityTypeByItemId", _m_GetAllianceCityTypeByItemId);
		Utils.RegisterFunc(L, -3, "GetBuildOffsetRangeByBuildId", _m_GetBuildOffsetRangeByBuildId);
		Utils.RegisterFunc(L, -3, "GetPointInfoByUuid", _m_GetPointInfoByUuid);
		Utils.RegisterFunc(L, -3, "GetDesertInfoByUuid", _m_GetDesertInfoByUuid);
		Utils.RegisterFunc(L, -3, "GetMyPointInfo", _m_GetMyPointInfo);
		Utils.RegisterFunc(L, -3, "HasPointInfo", _m_HasPointInfo);
		Utils.RegisterFunc(L, -3, "IsSelfPoint", _m_IsSelfPoint);
		Utils.RegisterFunc(L, -3, "GetDesertPoint", _m_GetDesertPoint);
		Utils.RegisterFunc(L, -3, "GetDesertPointList", _m_GetDesertPointList);
		Utils.RegisterFunc(L, -3, "IsCollectPoint", _m_IsCollectPoint);
		Utils.RegisterFunc(L, -3, "IsCollectRangePoint", _m_IsCollectRangePoint);
		Utils.RegisterFunc(L, -3, "GetCollectRangePoint", _m_GetCollectRangePoint);
		Utils.RegisterFunc(L, -3, "GetCollectInfoByIndex", _m_GetCollectInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetResourcePointInfoByIndex", _m_GetResourcePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetExplorePointInfoByIndex", _m_GetExplorePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetSamplePointInfoByIndex", _m_GetSamplePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetDetectRetryTaskPointInfo", _m_GetDetectRetryTaskPointInfo);
		Utils.RegisterFunc(L, -3, "GetDetectAttackCityS0TaskPointInfo", _m_GetDetectAttackCityS0TaskPointInfo);
		Utils.RegisterFunc(L, -3, "GetHeroDispatchTaskPointInfoByIndex", _m_GetHeroDispatchTaskPointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetGhostreconPointInfoByIndex", _m_GetGhostreconPointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetGarbagePointInfoByIndex", _m_GetGarbagePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "IsRoadPoint", _m_IsRoadPoint);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "HandleViewPointsReply", _m_HandleViewPointsReply);
		Utils.RegisterFunc(L, -3, "ParseWorldPointFoldUp", _m_ParseWorldPointFoldUp);
		Utils.RegisterFunc(L, -3, "ParseWorldPointRemove", _m_ParseWorldPointRemove);
		Utils.RegisterFunc(L, -3, "ParseWorldPointUpdate", _m_ParseWorldPointUpdate);
		Utils.RegisterFunc(L, -3, "HandleViewUpdateNotify", _m_HandleViewUpdateNotify);
		Utils.RegisterFunc(L, -3, "HandleViewAssistanceInfoUpdateNotify", _m_HandleViewAssistanceInfoUpdateNotify);
		Utils.RegisterFunc(L, -3, "HandleViewTileUpdateNotify", _m_HandleViewTileUpdateNotify);
		Utils.RegisterFunc(L, -3, "HandleALPoints", _m_HandleALPoints);
		Utils.RegisterFunc(L, -3, "GetALMemberPoints", _m_GetALMemberPoints);
		Utils.RegisterFunc(L, -3, "ClearALMemberPoints", _m_ClearALMemberPoints);
		Utils.RegisterFunc(L, -3, "ShowObject", _m_ShowObject);
		Utils.RegisterFunc(L, -3, "HideObject", _m_HideObject);
		Utils.RegisterFunc(L, -3, "UpdateObject", _m_UpdateObject);
		Utils.RegisterFunc(L, -3, "UpdateObjectByUuid", _m_UpdateObjectByUuid);
		Utils.RegisterFunc(L, -3, "UpdateViewRequest", _m_UpdateViewRequest);
		Utils.RegisterFunc(L, -3, "GetServerLod", _m_GetServerLod);
		Utils.RegisterFunc(L, -3, "StartViewRequest", _m_StartViewRequest);
		Utils.RegisterFunc(L, -3, "AddToDeleteList", _m_AddToDeleteList);
		Utils.RegisterFunc(L, -3, "AddToDelayDestroyList", _m_AddToDelayDestroyList);
		Utils.RegisterFunc(L, -3, "MarkPointIsDirty", _m_MarkPointIsDirty);
		Utils.RegisterFunc(L, -3, "TryGetAssistanceCountByPointIndex", _m_TryGetAssistanceCountByPointIndex);
		Utils.RegisterFunc(L, -3, "GetObjectByPoint", _m_GetObjectByPoint);
		Utils.RegisterFunc(L, -3, "GetObjectByUuid", _m_GetObjectByUuid);
		Utils.RegisterFunc(L, -3, "GetBuildingByPoint", _m_GetBuildingByPoint);
		Utils.RegisterFunc(L, -3, "GetBuildingByUuid", _m_GetBuildingByUuid);
		Utils.RegisterFunc(L, -3, "GetWorldBuildingByPoint", _m_GetWorldBuildingByPoint);
		Utils.RegisterFunc(L, -3, "GetWorldBuildingByUuid", _m_GetWorldBuildingByUuid);
		Utils.RegisterFunc(L, -3, "IsRoad", _m_IsRoad);
		Utils.RegisterFunc(L, -3, "IsSelfRoad", _m_IsSelfRoad);
		Utils.RegisterFunc(L, -3, "IsSelfFreeBoard", _m_IsSelfFreeBoard);
		Utils.RegisterFunc(L, -3, "IsBoard", _m_IsBoard);
		Utils.RegisterFunc(L, -3, "GetCollectResourceTile", _m_GetCollectResourceTile);
		Utils.RegisterFunc(L, -3, "GetCollectResourceRange", _m_GetCollectResourceRange);
		Utils.RegisterFunc(L, -3, "GetCollectResourceBuildRange", _m_GetCollectResourceBuildRange);
		Utils.RegisterFunc(L, -3, "GetCollectPoint", _m_GetCollectPoint);
		Utils.RegisterFunc(L, -3, "GetAllCollectRangePoint", _m_GetAllCollectRangePoint);
		Utils.RegisterFunc(L, -3, "GetAllCollectRangePointType", _m_GetAllCollectRangePointType);
		Utils.RegisterFunc(L, -3, "GetAllCollectRange", _m_GetAllCollectRange);
		Utils.RegisterFunc(L, -3, "GetGarbagePoint", _m_GetGarbagePoint);
		Utils.RegisterFunc(L, -3, "GetBaseMainInfoByOwnerUid", _m_GetBaseMainInfoByOwnerUid);
		Utils.RegisterFunc(L, -3, "IsOutCityByPoint", _m_IsOutCityByPoint);
		Utils.RegisterFunc(L, -3, "GetBaseMainByScreen", _m_GetBaseMainByScreen);
		Utils.RegisterFunc(L, -3, "GetMainByScreen", _m_GetMainByScreen);
		Utils.RegisterFunc(L, -3, "GetAllMainBaseList", _m_GetAllMainBaseList);
		Utils.RegisterFunc(L, -3, "GetAllMainBaseListByType", _m_GetAllMainBaseListByType);
		Utils.RegisterFunc(L, -3, "GetAllDragonPointList", _m_GetAllDragonPointList);
		Utils.RegisterFunc(L, -3, "GetAllDragonResourceList", _m_GetAllDragonResourceList);
		Utils.RegisterFunc(L, -3, "IsNeedPlayPlacedAnim", _m_IsNeedPlayPlacedAnim);
		Utils.RegisterFunc(L, -3, "CheckNeedRefreshRoad", _m_CheckNeedRefreshRoad);
		Utils.RegisterFunc(L, -3, "RefreshRoads", _m_RefreshRoads);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -3, "ChangeOutEdgeScale", _m_ChangeOutEdgeScale);
		Utils.RegisterFunc(L, -3, "SetFirstViewRequestFlag", _m_SetFirstViewRequestFlag);
		Utils.RegisterFunc(L, -3, "SetBattleFieldFirst", _m_SetBattleFieldFirst);
		Utils.RegisterFunc(L, -3, "IsOutOfLWAoi", _m_IsOutOfLWAoi);
		Utils.RegisterFunc(L, -3, "GetLabelSkinColor", _m_GetLabelSkinColor);
		Utils.RegisterFunc(L, -3, "GetLabelSkinOffset", _m_GetLabelSkinOffset);
		Utils.RegisterFunc(L, -3, "GetLabelSkinSizeAdd", _m_GetLabelSkinSizeAdd);
		Utils.RegisterFunc(L, -3, "GetProfileSwitch", _m_GetProfileSwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggle", _m_ProfileToggle);
		Utils.RegisterFunc(L, -3, "GetSpecialPointDic", _m_GetSpecialPointDic);
		Utils.RegisterFunc(L, -3, "HandleSandWormUpdate", _m_HandleSandWormUpdate);
		Utils.RegisterFunc(L, -3, "StartRecordTileBlock", _m_StartRecordTileBlock);
		Utils.RegisterFunc(L, -3, "StopRecordTileBlock", _m_StopRecordTileBlock);
		Utils.RegisterFunc(L, -3, "SetFocusPoint", _m_SetFocusPoint);
		Utils.RegisterFunc(L, -3, "OnWorldColorDirty", _m_OnWorldColorDirty);
		Utils.RegisterFunc(L, -3, "HandlePushWorldObjStateChange", _m_HandlePushWorldObjStateChange);
		Utils.RegisterFunc(L, -2, "ObjsCount", _g_get_ObjsCount);
		Utils.RegisterFunc(L, -2, "worldGreen", _g_get_worldGreen);
		Utils.RegisterFunc(L, -2, "EnableWorldAssistanceOpt", _g_get_EnableWorldAssistanceOpt);
		Utils.RegisterFunc(L, -2, "TileBlockIndex", _g_get_TileBlockIndex);
		Utils.RegisterFunc(L, -2, "aoiAssistanceInfos", _g_get_aoiAssistanceInfos);
		Utils.RegisterFunc(L, -2, "alliancePointsInfos", _g_get_alliancePointsInfos);
		Utils.RegisterFunc(L, -1, "aoiAssistanceInfos", _s_set_aoiAssistanceInfos);
		Utils.RegisterFunc(L, -1, "alliancePointsInfos", _s_set_alliancePointsInfos);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 8, 5, 0);
		Utils.RegisterFunc(L, -4, "NewPointInfo", _m_NewPointInfo_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCircleRange", _m_GetCircleRange_xlua_st_);
		Utils.RegisterFunc(L, -4, "OnCityStatusChanged", _m_OnCityStatusChanged_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCityUuid", _m_GetCityUuid_xlua_st_);
		Utils.RegisterFunc(L, -4, "DoMissileFire", _m_DoMissileFire_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "ShowMapIconDist", WorldPointManager.ShowMapIconDist);
		Utils.RegisterObject(L, translator, -4, "LodRequestRange", WorldPointManager.LodRequestRange);
		Utils.RegisterFunc(L, -2, "time", _g_get_time);
		Utils.RegisterFunc(L, -2, "busy", _g_get_busy);
		Utils.RegisterFunc(L, -2, "defaultCountryFlag", _g_get_defaultCountryFlag);
		Utils.RegisterFunc(L, -2, "allCountryFlagSet", _g_get_allCountryFlagSet);
		Utils.RegisterFunc(L, -2, "lwAoiBlockSizeArray", _g_get_lwAoiBlockSizeArray);
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
				WorldPointManager o = new WorldPointManager((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterLodWatcher(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			IWorldLodWatcher watcher = (IWorldLodWatcher)objectTranslator.GetObject(L, 2, typeof(IWorldLodWatcher));
			worldPointManager.RegisterLodWatcher(watcher);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnregisterLodWatcher(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			IWorldLodWatcher watcher = (IWorldLodWatcher)objectTranslator.GetObject(L, 2, typeof(IWorldLodWatcher));
			worldPointManager.UnregisterLodWatcher(watcher);
			return 0;
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
			string str = ((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Description();
			Lua.lua_pushstring(L, str);
			return 1;
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
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
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
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAllObject(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveAllObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBuildFinish(IntPtr L)
	{
		try
		{
			bool value = ((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBuildFinish();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int pointIndex = Lua.xlua_tointeger(L, 2);
				PointInfo pointInfo = worldPointManager.GetPointInfo(pointIndex);
				objectTranslator.Push(L, pointInfo);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int pointIndex2 = Lua.xlua_tointeger(L, 2);
				int serverId = Lua.xlua_tointeger(L, 3);
				PointInfo pointInfo2 = worldPointManager.GetPointInfo(pointIndex2, serverId);
				objectTranslator.Push(L, pointInfo2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.GetPointInfo!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldDesertInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			WorldDesertInfo worldDesertInfo = obj.GetWorldDesertInfo(pointIndex);
			objectTranslator.Push(L, worldDesertInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldTileInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			WorldTileInfo worldTileInfo = obj.GetWorldTileInfo(pointIndex);
			objectTranslator.Push(L, worldTileInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetYellowLand(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			PointInfo yellowLand = obj.GetYellowLand(pointIndex);
			objectTranslator.Push(L, yellowLand);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildTileByItemId(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemId = Lua.xlua_tointeger(L, 2);
			int buildTileByItemId = obj.GetBuildTileByItemId(itemId);
			Lua.xlua_pushinteger(L, buildTileByItemId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceCitySizeByItemId(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemId = Lua.xlua_tointeger(L, 2);
			int allianceCitySizeByItemId = obj.GetAllianceCitySizeByItemId(itemId);
			Lua.xlua_pushinteger(L, allianceCitySizeByItemId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTreasureSizeByItemId(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemId = Lua.xlua_tointeger(L, 2);
			int treasureSizeByItemId = obj.GetTreasureSizeByItemId(itemId);
			Lua.xlua_pushinteger(L, treasureSizeByItemId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDragonBuildSizeByItemId(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemId = Lua.xlua_tointeger(L, 2);
			int dragonBuildSizeByItemId = obj.GetDragonBuildSizeByItemId(itemId);
			Lua.xlua_pushinteger(L, dragonBuildSizeByItemId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceCityTypeByItemId(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int itemId = Lua.xlua_tointeger(L, 2);
			int allianceCityTypeByItemId = obj.GetAllianceCityTypeByItemId(itemId);
			Lua.xlua_pushinteger(L, allianceCityTypeByItemId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildOffsetRangeByBuildId(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int buildId = Lua.xlua_tointeger(L, 2);
			int buildOffsetRangeByBuildId = obj.GetBuildOffsetRangeByBuildId(buildId);
			Lua.xlua_pushinteger(L, buildOffsetRangeByBuildId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointInfoByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			PointInfo pointInfoByUuid = obj.GetPointInfoByUuid(uuid);
			objectTranslator.Push(L, pointInfoByUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDesertInfoByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			WorldDesertInfo desertInfoByUuid = obj.GetDesertInfoByUuid(uuid);
			objectTranslator.Push(L, desertInfoByUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMyPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo myPointInfo = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetMyPointInfo();
			objectTranslator.Push(L, myPointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HasPointInfo(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			bool value = obj.HasPointInfo(pointIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSelfPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsSelfPoint(pointIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDesertPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lv = Lua.xlua_tointeger(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			int desertPoint = obj.GetDesertPoint(lv, type);
			Lua.xlua_pushinteger(L, desertPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDesertPointList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldTileInfo> desertPointList = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetDesertPointList();
			objectTranslator.Push(L, desertPointList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCollectPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsCollectPoint(pointIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsCollectRangePoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsCollectRangePoint(pointIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollectRangePoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			CollectPointInfo collectRangePoint = obj.GetCollectRangePoint(pointIndex);
			objectTranslator.Push(L, collectRangePoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollectInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			CollectPointInfo collectInfoByIndex = obj.GetCollectInfoByIndex(pointIndex);
			objectTranslator.Push(L, collectInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourcePointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			ResPointInfo resourcePointInfoByIndex = obj.GetResourcePointInfoByIndex(pointIndex);
			objectTranslator.Push(L, resourcePointInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetExplorePointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			ExplorePointInfo explorePointInfoByIndex = obj.GetExplorePointInfoByIndex(pointIndex);
			objectTranslator.Push(L, explorePointInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSamplePointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			SamplePointInfo samplePointInfoByIndex = obj.GetSamplePointInfoByIndex(pointIndex);
			objectTranslator.Push(L, samplePointInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDetectRetryTaskPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			DetectRetryTaskPointInfo detectRetryTaskPointInfo = obj.GetDetectRetryTaskPointInfo(pointIndex);
			objectTranslator.Push(L, detectRetryTaskPointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDetectAttackCityS0TaskPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			DetectAttackCityS0TaskPointInfo detectAttackCityS0TaskPointInfo = obj.GetDetectAttackCityS0TaskPointInfo(pointIndex);
			objectTranslator.Push(L, detectAttackCityS0TaskPointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHeroDispatchTaskPointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			HeroDispatchMissionPointInfo heroDispatchTaskPointInfoByIndex = obj.GetHeroDispatchTaskPointInfoByIndex(pointIndex);
			objectTranslator.Push(L, heroDispatchTaskPointInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGhostreconPointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			GhostreconPointInfo ghostreconPointInfoByIndex = obj.GetGhostreconPointInfoByIndex(pointIndex);
			objectTranslator.Push(L, ghostreconPointInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGarbagePointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			GarbagePointInfo garbagePointInfoByIndex = obj.GetGarbagePointInfoByIndex(pointIndex);
			objectTranslator.Push(L, garbagePointInfoByIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRoadPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			string uid = Lua.lua_tostring(L, 3);
			int dir = Lua.xlua_tointeger(L, 4);
			bool value = obj.IsRoadPoint(index, uid, dir);
			Lua.lua_pushboolean(L, value);
			return 1;
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
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_HandleViewPointsReply(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldPointManager.HandleViewPointsReply(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ParseWorldPointFoldUp(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				int serverId = Lua.xlua_tointeger(L, 3);
				int worldId = Lua.xlua_tointeger(L, 4);
				worldPointManager.ParseWorldPointFoldUp(message, serverId, worldId);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ISFSObject message2 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				int serverId2 = Lua.xlua_tointeger(L, 3);
				worldPointManager.ParseWorldPointFoldUp(message2, serverId2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<ISFSObject>(L, 2))
			{
				ISFSObject message3 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				worldPointManager.ParseWorldPointFoldUp(message3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.ParseWorldPointFoldUp!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ParseWorldPointRemove(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				int serverId = Lua.xlua_tointeger(L, 3);
				int worldId = Lua.xlua_tointeger(L, 4);
				worldPointManager.ParseWorldPointRemove(message, serverId, worldId);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ISFSObject message2 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				int serverId2 = Lua.xlua_tointeger(L, 3);
				worldPointManager.ParseWorldPointRemove(message2, serverId2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<ISFSObject>(L, 2))
			{
				ISFSObject message3 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				worldPointManager.ParseWorldPointRemove(message3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.ParseWorldPointRemove!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ParseWorldPointUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				int serverId = Lua.xlua_tointeger(L, 3);
				int worldId = Lua.xlua_tointeger(L, 4);
				worldPointManager.ParseWorldPointUpdate(message, serverId, worldId);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<ISFSObject>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				ISFSObject message2 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				int serverId2 = Lua.xlua_tointeger(L, 3);
				worldPointManager.ParseWorldPointUpdate(message2, serverId2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<ISFSObject>(L, 2))
			{
				ISFSObject message3 = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
				worldPointManager.ParseWorldPointUpdate(message3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.ParseWorldPointUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleViewUpdateNotify(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldPointManager.HandleViewUpdateNotify(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleViewAssistanceInfoUpdateNotify(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldPointManager.HandleViewAssistanceInfoUpdateNotify(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleViewTileUpdateNotify(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldPointManager.HandleViewTileUpdateNotify(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleALPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldPointManager.HandleALPoints(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetALMemberPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetALMemberPoints(out var leaderPosition, out var memberPositions);
			Lua.lua_pushint64(L, leaderPosition);
			objectTranslator.Push(L, memberPositions);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearALMemberPoints(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearALMemberPoints();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NewPointInfo_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointInfo p = (WorldPointInfo)objectTranslator.GetObject(L, 1, typeof(WorldPointInfo));
			bool isCreate = Lua.lua_toboolean(L, 2);
			PointInfo o = WorldPointManager.NewPointInfo(p, isCreate);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowObject(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			obj.ShowObject(point);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideObject(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			obj.HideObject(point);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateObject(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			obj.UpdateObject(point);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateObjectByUuid(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long pointUuid = Lua.lua_toint64(L, 2);
			obj.UpdateObjectByUuid(pointUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateViewRequest(IntPtr L)
	{
		try
		{
			WorldPointManager worldPointManager = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool isForce = Lua.lua_toboolean(L, 2);
				worldPointManager.UpdateViewRequest(isForce);
				return 0;
			}
			if (num == 1)
			{
				worldPointManager.UpdateViewRequest();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.UpdateViewRequest!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerLod(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			int serverLod = obj.GetServerLod(lod);
			Lua.xlua_pushinteger(L, serverLod);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartViewRequest(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StartViewRequest();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddToDeleteList(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.AddToDeleteList(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddToDelayDestroyList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			IWorldDelayDestroyObject obj = (IWorldDelayDestroyObject)objectTranslator.GetObject(L, 2, typeof(IWorldDelayDestroyObject));
			worldPointManager.AddToDelayDestroyList(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_MarkPointIsDirty(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long dirtyPointUuid = Lua.lua_toint64(L, 2);
			obj.MarkPointIsDirty(dirtyPointUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetAssistanceCountByPointIndex(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			int pointIndex = Lua.xlua_tointeger(L, 3);
			int count;
			int max;
			bool value = obj.TryGetAssistanceCountByPointIndex(serverId, pointIndex, out count, out max);
			Lua.lua_pushboolean(L, value);
			Lua.xlua_pushinteger(L, count);
			Lua.xlua_pushinteger(L, max);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectByPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int pointIndex = Lua.xlua_tointeger(L, 2);
				WorldPointObject objectByPoint = worldPointManager.GetObjectByPoint(pointIndex);
				objectTranslator.Push(L, objectByPoint);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int serverId = Lua.xlua_tointeger(L, 2);
				int pointIndex2 = Lua.xlua_tointeger(L, 3);
				WorldPointObject objectByPoint2 = worldPointManager.GetObjectByPoint(serverId, pointIndex2);
				objectTranslator.Push(L, objectByPoint2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.GetObjectByPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			WorldPointObject objectByUuid = obj.GetObjectByUuid(uuid);
			objectTranslator.Push(L, objectByUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildingByPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			CityBuilding buildingByPoint = obj.GetBuildingByPoint(pointIndex);
			objectTranslator.Push(L, buildingByPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildingByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			CityBuilding buildingByUuid = obj.GetBuildingByUuid(uuid);
			objectTranslator.Push(L, buildingByUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldBuildingByPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			WorldBuilding worldBuildingByPoint = obj.GetWorldBuildingByPoint(pointIndex);
			objectTranslator.Push(L, worldBuildingByPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldBuildingByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			WorldBuilding worldBuildingByUuid = obj.GetWorldBuildingByUuid(uuid);
			objectTranslator.Push(L, worldBuildingByUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRoad(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsRoad(index);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSelfRoad(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsSelfRoad(index);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsSelfFreeBoard(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsSelfFreeBoard(index);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsBoard(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsBoard(index);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollectResourceTile(IntPtr L)
	{
		try
		{
			int collectResourceTile = ((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCollectResourceTile();
			Lua.xlua_pushinteger(L, collectResourceTile);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollectResourceRange(IntPtr L)
	{
		try
		{
			int collectResourceRange = ((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCollectResourceRange();
			Lua.xlua_pushinteger(L, collectResourceRange);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollectResourceBuildRange(IntPtr L)
	{
		try
		{
			int collectResourceBuildRange = ((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCollectResourceBuildRange();
			Lua.xlua_pushinteger(L, collectResourceBuildRange);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCircleRange_xlua_st_(IntPtr L)
	{
		try
		{
			int circleRange = WorldPointManager.GetCircleRange(Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, circleRange);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCollectPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int resourceType = Lua.xlua_tointeger(L, 2);
			int collectPoint = obj.GetCollectPoint(resourceType);
			Lua.xlua_pushinteger(L, collectPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllCollectRangePoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int resourceType = Lua.xlua_tointeger(L, 2);
			List<int> allCollectRangePoint = obj.GetAllCollectRangePoint(resourceType);
			objectTranslator.Push(L, allCollectRangePoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllCollectRangePointType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int resourceType = Lua.xlua_tointeger(L, 2);
			int mainIndex = Lua.xlua_tointeger(L, 3);
			List<int> allCollectRangePointType = obj.GetAllCollectRangePointType(resourceType, mainIndex);
			objectTranslator.Push(L, allCollectRangePointType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllCollectRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int resourceType = Lua.xlua_tointeger(L, 2);
			int mainIndex = Lua.xlua_tointeger(L, 3);
			List<int> allCollectRange = obj.GetAllCollectRange(resourceType, mainIndex);
			objectTranslator.Push(L, allCollectRange);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGarbagePoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<int> garbagePoint = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetGarbagePoint();
			objectTranslator.Push(L, garbagePoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBaseMainInfoByOwnerUid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			string ownerUid = Lua.lua_tostring(L, 2);
			BuildPointInfo baseMainInfoByOwnerUid = obj.GetBaseMainInfoByOwnerUid(ownerUid);
			objectTranslator.Push(L, baseMainInfoByOwnerUid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOutCityByPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsOutCityByPoint(point);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBaseMainByScreen(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BuildPointInfo baseMainByScreen = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetBaseMainByScreen();
			objectTranslator.Push(L, baseMainByScreen);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMainByScreen(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<BuildPointInfo> mainByScreen = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetMainByScreen();
			objectTranslator.Push(L, mainByScreen);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllMainBaseList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<BuildPointInfo> allMainBaseList = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetAllMainBaseList();
			objectTranslator.Push(L, allMainBaseList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllMainBaseListByType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<PlayerType>(L, 2))
			{
				objectTranslator.Get(L, 2, out PlayerType val);
				List<BuildPointInfo> allMainBaseListByType = worldPointManager.GetAllMainBaseListByType(val);
				objectTranslator.Push(L, allMainBaseListByType);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<PlayerType>(L, 2) && objectTranslator.Assignable<PlayerType>(L, 3))
			{
				objectTranslator.Get(L, 2, out PlayerType val2);
				objectTranslator.Get(L, 3, out PlayerType val3);
				List<BuildPointInfo> allMainBaseListByType2 = worldPointManager.GetAllMainBaseListByType(val2, val3);
				objectTranslator.Push(L, allMainBaseListByType2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<PlayerType>(L, 2) && objectTranslator.Assignable<PlayerType>(L, 3) && objectTranslator.Assignable<PlayerType>(L, 4))
			{
				objectTranslator.Get(L, 2, out PlayerType val4);
				objectTranslator.Get(L, 3, out PlayerType val5);
				objectTranslator.Get(L, 4, out PlayerType val6);
				List<BuildPointInfo> allMainBaseListByType3 = worldPointManager.GetAllMainBaseListByType(val4, val5, val6);
				objectTranslator.Push(L, allMainBaseListByType3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<PlayerType>(L, 2) && objectTranslator.Assignable<PlayerType>(L, 3) && objectTranslator.Assignable<PlayerType>(L, 4) && objectTranslator.Assignable<PlayerType>(L, 5))
			{
				objectTranslator.Get(L, 2, out PlayerType val7);
				objectTranslator.Get(L, 3, out PlayerType val8);
				objectTranslator.Get(L, 4, out PlayerType val9);
				objectTranslator.Get(L, 5, out PlayerType val10);
				List<BuildPointInfo> allMainBaseListByType4 = worldPointManager.GetAllMainBaseListByType(val7, val8, val9, val10);
				objectTranslator.Push(L, allMainBaseListByType4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.GetAllMainBaseListByType!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllDragonPointList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<PointInfo> allDragonPointList = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetAllDragonPointList();
			objectTranslator.Push(L, allDragonPointList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllDragonResourceList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<int> allDragonResourceList = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetAllDragonResourceList();
			objectTranslator.Push(L, allDragonResourceList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsNeedPlayPlacedAnim(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsNeedPlayPlacedAnim(uuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckNeedRefreshRoad(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckNeedRefreshRoad();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshRoads(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshRoads();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrawGizmos(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeOutEdgeScale(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float scale = (float)Lua.lua_tonumber(L, 2);
			obj.ChangeOutEdgeScale(scale);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFirstViewRequestFlag(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool firstViewRequestFlag = Lua.lua_toboolean(L, 2);
			obj.SetFirstViewRequestFlag(firstViewRequestFlag);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetBattleFieldFirst(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool battleFieldFirst = Lua.lua_toboolean(L, 2);
			obj.SetBattleFieldFirst(battleFieldFirst);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOutOfLWAoi(IntPtr L)
	{
		try
		{
			WorldPointManager worldPointManager = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int pointIndex = Lua.xlua_tointeger(L, 2);
				int serverId = Lua.xlua_tointeger(L, 3);
				bool value = worldPointManager.IsOutOfLWAoi(pointIndex, serverId);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int pointIndex2 = Lua.xlua_tointeger(L, 2);
				bool value2 = worldPointManager.IsOutOfLWAoi(pointIndex2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointManager.IsOutOfLWAoi!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLabelSkinColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager obj = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			int skinId = Lua.xlua_tointeger(L, 2);
			int colorType = Lua.xlua_tointeger(L, 3);
			Color labelSkinColor = obj.GetLabelSkinColor(skinId, colorType);
			objectTranslator.PushUnityEngineColor(L, labelSkinColor);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLabelSkinOffset(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int skinId = Lua.xlua_tointeger(L, 2);
			float labelSkinOffset = obj.GetLabelSkinOffset(skinId);
			Lua.lua_pushnumber(L, labelSkinOffset);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLabelSkinSizeAdd(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int skinId = Lua.xlua_tointeger(L, 2);
			float labelSkinSizeAdd = obj.GetLabelSkinSizeAdd(skinId);
			Lua.lua_pushnumber(L, labelSkinSizeAdd);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetProfileSwitch(IntPtr L)
	{
		try
		{
			bool profileSwitch = ((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetProfileSwitch();
			Lua.lua_pushboolean(L, profileSwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggle(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggle();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpecialPointDic(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<int, int> specialPointDic = ((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).GetSpecialPointDic();
			objectTranslator.Push(L, specialPointDic);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleSandWormUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			BuildPointInfo bi = (BuildPointInfo)objectTranslator.GetObject(L, 2, typeof(BuildPointInfo));
			objectTranslator.Get(L, 3, out SandWormAnim v);
			worldPointManager.HandleSandWormUpdate(bi, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartRecordTileBlock(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int worldId = Lua.xlua_tointeger(L, 2);
			obj.StartRecordTileBlock(worldId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopRecordTileBlock(IntPtr L)
	{
		try
		{
			((WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopRecordTileBlock();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFocusPoint(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int focusPoint = Lua.xlua_tointeger(L, 2);
			obj.SetFocusPoint(focusPoint);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnWorldColorDirty(IntPtr L)
	{
		try
		{
			WorldPointManager obj = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceId = Lua.lua_tostring(L, 2);
			obj.OnWorldColorDirty(allianceId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldObjStateChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject msg = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			worldPointManager.HandlePushWorldObjStateChange(msg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnCityStatusChanged_xlua_st_(IntPtr L)
	{
		try
		{
			int serverId = Lua.xlua_tointeger(L, 1);
			int cityId = Lua.xlua_tointeger(L, 2);
			WorldPointManager.OnCityStatusChanged(serverId, cityId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCityUuid_xlua_st_(IntPtr L)
	{
		try
		{
			int serverId = Lua.xlua_tointeger(L, 1);
			int cityId = Lua.xlua_tointeger(L, 2);
			long cityUuid = WorldPointManager.GetCityUuid(serverId, cityId);
			Lua.lua_pushint64(L, cityUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DoMissileFire_xlua_st_(IntPtr L)
	{
		try
		{
			int serverId = Lua.xlua_tointeger(L, 1);
			int cityId = Lua.xlua_tointeger(L, 2);
			long uuid = Lua.lua_toint64(L, 3);
			WorldPointManager.DoMissileFire(serverId, cityId, uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_time(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldPointManager.time);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_busy(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, WorldPointManager.busy);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultCountryFlag(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, WorldPointManager.defaultCountryFlag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allCountryFlagSet(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldPointManager.allCountryFlagSet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ObjsCount(IntPtr L)
	{
		try
		{
			WorldPointManager worldPointManager = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldPointManager.ObjsCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldGreen(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldPointManager.worldGreen);
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
			WorldPointManager worldPointManager = (WorldPointManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldPointManager.EnableWorldAssistanceOpt);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lwAoiBlockSizeArray(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, WorldPointManager.lwAoiBlockSizeArray);
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
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldPointManager.TileBlockIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aoiAssistanceInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldPointManager.aoiAssistanceInfos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_alliancePointsInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointManager worldPointManager = (WorldPointManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldPointManager.alliancePointsInfos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aoiAssistanceInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).aoiAssistanceInfos = (WorldAoiAssistanceInfos)objectTranslator.GetObject(L, 2, typeof(WorldAoiAssistanceInfos));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_alliancePointsInfos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldPointManager)objectTranslator.FastGetCSObj(L, 1)).alliancePointsInfos = (WorldALPointsInfos)objectTranslator.GetObject(L, 2, typeof(WorldALPointsInfos));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
