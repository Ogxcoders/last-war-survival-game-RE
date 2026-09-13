using System;
using System.Collections;
using System.Collections.Generic;
using BitBenderGames;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CitySceneWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CityScene);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 334, 35, 14);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "OnSkinChange", _m_OnSkinChange);
		Utils.RegisterFunc(L, -3, "GetLabelSkinColor", _m_GetLabelSkinColor);
		Utils.RegisterFunc(L, -3, "GetLabelSkinOffset", _m_GetLabelSkinOffset);
		Utils.RegisterFunc(L, -3, "GetLabelSkinSizeAdd", _m_GetLabelSkinSizeAdd);
		Utils.RegisterFunc(L, -3, "GetPeopleById", _m_GetPeopleById);
		Utils.RegisterFunc(L, -3, "PausePeopleAndPlayAnim", _m_PausePeopleAndPlayAnim);
		Utils.RegisterFunc(L, -3, "ResumePeople", _m_ResumePeople);
		Utils.RegisterFunc(L, -3, "CreateScene", _m_CreateScene);
		Utils.RegisterFunc(L, -3, "LoadCityPostProcessVolume", _m_LoadCityPostProcessVolume);
		Utils.RegisterFunc(L, -3, "SetNewPostProcessVolume", _m_SetNewPostProcessVolume);
		Utils.RegisterFunc(L, -3, "CrateS4NightColorRenderer", _m_CrateS4NightColorRenderer);
		Utils.RegisterFunc(L, -3, "CloseS4NightColorRenderer", _m_CloseS4NightColorRenderer);
		Utils.RegisterFunc(L, -3, "RemoveFakeSampleMarchData", _m_RemoveFakeSampleMarchData);
		Utils.RegisterFunc(L, -3, "UpdateFakeSampleMarchDataWhenBack", _m_UpdateFakeSampleMarchDataWhenBack);
		Utils.RegisterFunc(L, -3, "UpdateFakeSampleMarchDataWhenStartPick", _m_UpdateFakeSampleMarchDataWhenStartPick);
		Utils.RegisterFunc(L, -3, "AddFakeSampleMarchData", _m_AddFakeSampleMarchData);
		Utils.RegisterFunc(L, -3, "EndDig", _m_EndDig);
		Utils.RegisterFunc(L, -3, "UninitSubModulesAndCameraUpdate", _m_UninitSubModulesAndCameraUpdate);
		Utils.RegisterFunc(L, -3, "UninitSubModulesAndCameraUpdate_withoutCameraMove", _m_UninitSubModulesAndCameraUpdate_withoutCameraMove);
		Utils.RegisterFunc(L, -3, "UninitCameraMovement", _m_UninitCameraMovement);
		Utils.RegisterFunc(L, -3, "Uninit", _m_Uninit);
		Utils.RegisterFunc(L, -3, "ChangeQualitySetting", _m_ChangeQualitySetting);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "FixedUpdate", _m_FixedUpdate);
		Utils.RegisterFunc(L, -3, "TileToWorld", _m_TileToWorld);
		Utils.RegisterFunc(L, -3, "WorldToTile", _m_WorldToTile);
		Utils.RegisterFunc(L, -3, "SnapToTileCenter", _m_SnapToTileCenter);
		Utils.RegisterFunc(L, -3, "TileFloatToWorld", _m_TileFloatToWorld);
		Utils.RegisterFunc(L, -3, "WorldToTileFloat", _m_WorldToTileFloat);
		Utils.RegisterFunc(L, -3, "IndexToTilePos", _m_IndexToTilePos);
		Utils.RegisterFunc(L, -3, "TilePosToIndex", _m_TilePosToIndex);
		Utils.RegisterFunc(L, -3, "TileIndexToWorld", _m_TileIndexToWorld);
		Utils.RegisterFunc(L, -3, "WorldToTileIndex", _m_WorldToTileIndex);
		Utils.RegisterFunc(L, -3, "TileDistance", _m_TileDistance);
		Utils.RegisterFunc(L, -3, "SetWorldSize", _m_SetWorldSize);
		Utils.RegisterFunc(L, -3, "SetMapZoneActive", _m_SetMapZoneActive);
		Utils.RegisterFunc(L, -3, "GetBuildOffsetRangeByBuildId", _m_GetBuildOffsetRangeByBuildId);
		Utils.RegisterFunc(L, -3, "ChangeServer", _m_ChangeServer);
		Utils.RegisterFunc(L, -3, "OnChangeServerRemove", _m_OnChangeServerRemove);
		Utils.RegisterFunc(L, -3, "RemoveBlackDesert", _m_RemoveBlackDesert);
		Utils.RegisterFunc(L, -3, "InitBlackBlock", _m_InitBlackBlock);
		Utils.RegisterFunc(L, -3, "GetDragonLandRangeObj", _m_GetDragonLandRangeObj);
		Utils.RegisterFunc(L, -3, "CreateDragonLandRange", _m_CreateDragonLandRange);
		Utils.RegisterFunc(L, -3, "RemoveDragonLandRange", _m_RemoveDragonLandRange);
		Utils.RegisterFunc(L, -3, "RemoveDragonLandPoint", _m_RemoveDragonLandPoint);
		Utils.RegisterFunc(L, -3, "HandlePushWolfStatusChange", _m_HandlePushWolfStatusChange);
		Utils.RegisterFunc(L, -3, "GetMainByScreen", _m_GetMainByScreen);
		Utils.RegisterFunc(L, -3, "GetAllMainBaseList", _m_GetAllMainBaseList);
		Utils.RegisterFunc(L, -3, "GetAllMainBaseListByType", _m_GetAllMainBaseListByType);
		Utils.RegisterFunc(L, -3, "GetAllDragonPointList", _m_GetAllDragonPointList);
		Utils.RegisterFunc(L, -3, "GetAllDragonResourceList", _m_GetAllDragonResourceList);
		Utils.RegisterFunc(L, -3, "ShowTruck", _m_ShowTruck);
		Utils.RegisterFunc(L, -3, "ShowFreeTruck", _m_ShowFreeTruck);
		Utils.RegisterFunc(L, -3, "GetTruckPathString", _m_GetTruckPathString);
		Utils.RegisterFunc(L, -3, "GetIndexByOffset", _m_GetIndexByOffset);
		Utils.RegisterFunc(L, -3, "GetIndexByOffsetByDirection", _m_GetIndexByOffsetByDirection);
		Utils.RegisterFunc(L, -3, "OnPlayerBankruptcyFinish", _m_OnPlayerBankruptcyFinish);
		Utils.RegisterFunc(L, -3, "OnExtendDome", _m_OnExtendDome);
		Utils.RegisterFunc(L, -3, "ShowBattleBlood", _m_ShowBattleBlood);
		Utils.RegisterFunc(L, -3, "RegisterPhysics", _m_RegisterPhysics);
		Utils.RegisterFunc(L, -3, "GetBuildTileByItemId", _m_GetBuildTileByItemId);
		Utils.RegisterFunc(L, -3, "GetAllianceCitySizeByItemId", _m_GetAllianceCitySizeByItemId);
		Utils.RegisterFunc(L, -3, "GetTreasureSizeByItemId", _m_GetTreasureSizeByItemId);
		Utils.RegisterFunc(L, -3, "GetDragonBuildSizeByItemId", _m_GetDragonBuildSizeByItemId);
		Utils.RegisterFunc(L, -3, "UnregisterPhysics", _m_UnregisterPhysics);
		Utils.RegisterFunc(L, -3, "GetTouchTilePos", _m_GetTouchTilePos);
		Utils.RegisterFunc(L, -3, "GetTouchPoint", _m_GetTouchPoint);
		Utils.RegisterFunc(L, -3, "GetRaycastGroundPoint", _m_GetRaycastGroundPoint);
		Utils.RegisterFunc(L, -3, "AutoFocus", _m_AutoFocus);
		Utils.RegisterFunc(L, -3, "QuitFocus", _m_QuitFocus);
		Utils.RegisterFunc(L, -3, "GetLodDistance", _m_GetLodDistance);
		Utils.RegisterFunc(L, -3, "GetLodDistanceByLod", _m_GetLodDistanceByLod);
		Utils.RegisterFunc(L, -3, "GetRotation", _m_GetRotation);
		Utils.RegisterFunc(L, -3, "Get_rotation", _m_Get_rotation);
		Utils.RegisterFunc(L, -3, "GetMapIconScale", _m_GetMapIconScale);
		Utils.RegisterFunc(L, -3, "GetCameraPos", _m_GetCameraPos);
		Utils.RegisterFunc(L, -3, "ScreenPointToRay", _m_ScreenPointToRay);
		Utils.RegisterFunc(L, -3, "GetMinLodDistance", _m_GetMinLodDistance);
		Utils.RegisterFunc(L, -3, "WorldToScreenPoint", _m_WorldToScreenPoint);
		Utils.RegisterFunc(L, -3, "Lookat", _m_Lookat);
		Utils.RegisterFunc(L, -3, "AutoLookat", _m_AutoLookat);
		Utils.RegisterFunc(L, -3, "AutoZoom", _m_AutoZoom);
		Utils.RegisterFunc(L, -3, "ScreenPointToWorld", _m_ScreenPointToWorld);
		Utils.RegisterFunc(L, -3, "GetLodLevel", _m_GetLodLevel);
		Utils.RegisterFunc(L, -3, "TrackMarch", _m_TrackMarch);
		Utils.RegisterFunc(L, -3, "TrackHSR", _m_TrackHSR);
		Utils.RegisterFunc(L, -3, "DisablePostProcess", _m_DisablePostProcess);
		Utils.RegisterFunc(L, -3, "EnablePostProcess", _m_EnablePostProcess);
		Utils.RegisterFunc(L, -3, "SetTouchInputControllerEnable", _m_SetTouchInputControllerEnable);
		Utils.RegisterFunc(L, -3, "SetCameraTouchEnable", _m_SetCameraTouchEnable);
		Utils.RegisterFunc(L, -3, "UpdateTroopLineColor", _m_UpdateTroopLineColor);
		Utils.RegisterFunc(L, -3, "GetTouchInputControllerEnable", _m_GetTouchInputControllerEnable);
		Utils.RegisterFunc(L, -3, "StopCameraMove", _m_StopCameraMove);
		Utils.RegisterFunc(L, -3, "GetIndexByOffset_New", _m_GetIndexByOffset_New);
		Utils.RegisterFunc(L, -3, "GetIndexByOffsetByDirection_New", _m_GetIndexByOffsetByDirection_New);
		Utils.RegisterFunc(L, -3, "SetZoomParams", _m_SetZoomParams);
		Utils.RegisterFunc(L, -3, "SetCameraFOV", _m_SetCameraFOV);
		Utils.RegisterFunc(L, -3, "GetZoomParams", _m_GetZoomParams);
		Utils.RegisterFunc(L, -3, "GetFOV", _m_GetFOV);
		Utils.RegisterFunc(L, -3, "IsInMap", _m_IsInMap);
		Utils.RegisterFunc(L, -3, "IsInMapByIndex", _m_IsInMapByIndex);
		Utils.RegisterFunc(L, -3, "ClampTilePos", _m_ClampTilePos);
		Utils.RegisterFunc(L, -3, "GetCollectResourceBuildRange", _m_GetCollectResourceBuildRange);
		Utils.RegisterFunc(L, -3, "GetCollectResourceTile", _m_GetCollectResourceTile);
		Utils.RegisterFunc(L, -3, "CheckNeedRefreshRoad", _m_CheckNeedRefreshRoad);
		Utils.RegisterFunc(L, -3, "GetGlobalShaderLOD", _m_GetGlobalShaderLOD);
		Utils.RegisterFunc(L, -3, "SetGlobalShaderLOD", _m_SetGlobalShaderLOD);
		Utils.RegisterFunc(L, -3, "GetProfileTerrainSwitch", _m_GetProfileTerrainSwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggleTerrain", _m_ProfileToggleTerrain);
		Utils.RegisterFunc(L, -3, "GetProfileBuildingSwitch", _m_GetProfileBuildingSwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggleBuilding", _m_ProfileToggleBuilding);
		Utils.RegisterFunc(L, -3, "GetProfileStaticSwitch", _m_GetProfileStaticSwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggleStatic", _m_ProfileToggleStatic);
		Utils.RegisterFunc(L, -3, "GetHeightFogSwitch", _m_GetHeightFogSwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggleHeightFog", _m_ProfileToggleHeightFog);
		Utils.RegisterFunc(L, -3, "GetGraphySwitch", _m_GetGraphySwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggleMarch", _m_ProfileToggleMarch);
		Utils.RegisterFunc(L, -3, "GetExplorePointInfoByIndex", _m_GetExplorePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetSamplePointInfoByIndex", _m_GetSamplePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetDetectRetryTaskPointInfo", _m_GetDetectRetryTaskPointInfo);
		Utils.RegisterFunc(L, -3, "GetDetectAttackCityS0TaskPointInfo", _m_GetDetectAttackCityS0TaskPointInfo);
		Utils.RegisterFunc(L, -3, "GetHeroDispatchTaskPointInfoByIndex", _m_GetHeroDispatchTaskPointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetGhostreconPointInfoByIndex", _m_GetGhostreconPointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetResourcePointInfoByIndex", _m_GetResourcePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "IsCollectRangePoint", _m_IsCollectRangePoint);
		Utils.RegisterFunc(L, -3, "GetWorldTriggerData", _m_GetWorldTriggerData);
		Utils.RegisterFunc(L, -3, "GetTriggerDataByPointId", _m_GetTriggerDataByPointId);
		Utils.RegisterFunc(L, -3, "GetPointInfo", _m_GetPointInfo);
		Utils.RegisterFunc(L, -3, "GetPointInfoWithServer", _m_GetPointInfoWithServer);
		Utils.RegisterFunc(L, -3, "GetWorldDesertInfo", _m_GetWorldDesertInfo);
		Utils.RegisterFunc(L, -3, "GetMarchesByStartIndex", _m_GetMarchesByStartIndex);
		Utils.RegisterFunc(L, -3, "GetWorldTileInfo", _m_GetWorldTileInfo);
		Utils.RegisterFunc(L, -3, "GetYellowLand", _m_GetYellowLand);
		Utils.RegisterFunc(L, -3, "GetCollectRangePoint", _m_GetCollectRangePoint);
		Utils.RegisterFunc(L, -3, "GetCollectPoint", _m_GetCollectPoint);
		Utils.RegisterFunc(L, -3, "GetAllCollectRangePoint", _m_GetAllCollectRangePoint);
		Utils.RegisterFunc(L, -3, "GetAllCollectRangePointType", _m_GetAllCollectRangePointType);
		Utils.RegisterFunc(L, -3, "IsCollectPoint", _m_IsCollectPoint);
		Utils.RegisterFunc(L, -3, "GetCollectInfoByIndex", _m_GetCollectInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetGarbagePointInfoByIndex", _m_GetGarbagePointInfoByIndex);
		Utils.RegisterFunc(L, -3, "GetPointInfoByUuid", _m_GetPointInfoByUuid);
		Utils.RegisterFunc(L, -3, "GetDesertInfoByUuid", _m_GetDesertInfoByUuid);
		Utils.RegisterFunc(L, -3, "GetMyPointInfo", _m_GetMyPointInfo);
		Utils.RegisterFunc(L, -3, "GetObjectByPoint", _m_GetObjectByPoint);
		Utils.RegisterFunc(L, -3, "IsOutCityByPoint", _m_IsOutCityByPoint);
		Utils.RegisterFunc(L, -3, "GetBaseMainByScreen", _m_GetBaseMainByScreen);
		Utils.RegisterFunc(L, -3, "ShowObject", _m_ShowObject);
		Utils.RegisterFunc(L, -3, "GetBuildingByPoint", _m_GetBuildingByPoint);
		Utils.RegisterFunc(L, -3, "SetLevelUpActive", _m_SetLevelUpActive);
		Utils.RegisterFunc(L, -3, "GetBuildingHeight", _m_GetBuildingHeight);
		Utils.RegisterFunc(L, -3, "HandleViewPointsReply", _m_HandleViewPointsReply);
		Utils.RegisterFunc(L, -3, "HandleViewAssistanceInfoUpdateNotify", _m_HandleViewAssistanceInfoUpdateNotify);
		Utils.RegisterFunc(L, -3, "HandleViewUpdateNotify", _m_HandleViewUpdateNotify);
		Utils.RegisterFunc(L, -3, "HandleViewTileUpdateNotify", _m_HandleViewTileUpdateNotify);
		Utils.RegisterFunc(L, -3, "HandleLandUpdate", _m_HandleLandUpdate);
		Utils.RegisterFunc(L, -3, "SendViewRequest", _m_SendViewRequest);
		Utils.RegisterFunc(L, -3, "IsNeedPlayPlacedAnim", _m_IsNeedPlayPlacedAnim);
		Utils.RegisterFunc(L, -3, "IsSelfRoad", _m_IsSelfRoad);
		Utils.RegisterFunc(L, -3, "IsBuildFinish", _m_IsBuildFinish);
		Utils.RegisterFunc(L, -3, "GetObjectByUuid", _m_GetObjectByUuid);
		Utils.RegisterFunc(L, -3, "GetBuildingByUuid", _m_GetBuildingByUuid);
		Utils.RegisterFunc(L, -3, "GetWorldBuildingByPoint", _m_GetWorldBuildingByPoint);
		Utils.RegisterFunc(L, -3, "GetWorldBuildingByUuid", _m_GetWorldBuildingByUuid);
		Utils.RegisterFunc(L, -3, "HasPointInfo", _m_HasPointInfo);
		Utils.RegisterFunc(L, -3, "AddToDeleteList", _m_AddToDeleteList);
		Utils.RegisterFunc(L, -3, "GetBaseMainInfoByOwnerUid", _m_GetBaseMainInfoByOwnerUid);
		Utils.RegisterFunc(L, -3, "HideObject", _m_HideObject);
		Utils.RegisterFunc(L, -3, "IsSelfPoint", _m_IsSelfPoint);
		Utils.RegisterFunc(L, -3, "GetPointType", _m_GetPointType);
		Utils.RegisterFunc(L, -3, "IsSelfFreeBoard", _m_IsSelfFreeBoard);
		Utils.RegisterFunc(L, -3, "AddObjectByPoint", _m_AddObjectByPoint);
		Utils.RegisterFunc(L, -3, "RemoveObjectByPoint", _m_RemoveObjectByPoint);
		Utils.RegisterFunc(L, -3, "RemoveOneObjectByPointType", _m_RemoveOneObjectByPointType);
		Utils.RegisterFunc(L, -3, "RefreshView", _m_RefreshView);
		Utils.RegisterFunc(L, -3, "OnMainBuildMove", _m_OnMainBuildMove);
		Utils.RegisterFunc(L, -3, "UpdateViewRequest", _m_UpdateViewRequest);
		Utils.RegisterFunc(L, -3, "SetFirstViewRequestFlag", _m_SetFirstViewRequestFlag);
		Utils.RegisterFunc(L, -3, "IsRoadPoint", _m_IsRoadPoint);
		Utils.RegisterFunc(L, -3, "GetNearestPathForBuildingConnect", _m_GetNearestPathForBuildingConnect);
		Utils.RegisterFunc(L, -3, "GetGarbagePoint", _m_GetGarbagePoint);
		Utils.RegisterFunc(L, -3, "IsMyEnemy", _m_IsMyEnemy);
		Utils.RegisterFunc(L, -3, "CleanAllianceCacheData", _m_CleanAllianceCacheData);
		Utils.RegisterFunc(L, -3, "CreateGroupTroop", _m_CreateGroupTroop);
		Utils.RegisterFunc(L, -3, "GetMarch", _m_GetMarch);
		Utils.RegisterFunc(L, -3, "GetMonster", _m_GetMonster);
		Utils.RegisterFunc(L, -3, "IsTargetForMine", _m_IsTargetForMine);
		Utils.RegisterFunc(L, -3, "IsTargetForAlly", _m_IsTargetForAlly);
		Utils.RegisterFunc(L, -3, "StartMarch", _m_StartMarch);
		Utils.RegisterFunc(L, -3, "GetOwnerFormationMarch", _m_GetOwnerFormationMarch);
		Utils.RegisterFunc(L, -3, "GetAllianceMarchesInTeam", _m_GetAllianceMarchesInTeam);
		Utils.RegisterFunc(L, -3, "GetOwnerMarches", _m_GetOwnerMarches);
		Utils.RegisterFunc(L, -3, "HandlePushWorldTriggerUpdate", _m_HandlePushWorldTriggerUpdate);
		Utils.RegisterFunc(L, -3, "HandlePushWorldTriggerDel", _m_HandlePushWorldTriggerDel);
		Utils.RegisterFunc(L, -3, "HandlePushMultiKillUpdate", _m_HandlePushMultiKillUpdate);
		Utils.RegisterFunc(L, -3, "HandlePushWorldMarchAdd", _m_HandlePushWorldMarchAdd);
		Utils.RegisterFunc(L, -3, "HandlePushWorldMarchDel", _m_HandlePushWorldMarchDel);
		Utils.RegisterFunc(L, -3, "HandleWorldMarchGet", _m_HandleWorldMarchGet);
		Utils.RegisterFunc(L, -3, "HandleFormationMarch", _m_HandleFormationMarch);
		Utils.RegisterFunc(L, -3, "HandleFormationMarchChange", _m_HandleFormationMarchChange);
		Utils.RegisterFunc(L, -3, "HandleUpdateLightData", _m_HandleUpdateLightData);
		Utils.RegisterFunc(L, -3, "ExistMarch", _m_ExistMarch);
		Utils.RegisterFunc(L, -3, "IsInRallyMarch", _m_IsInRallyMarch);
		Utils.RegisterFunc(L, -3, "IsInCollectMarch", _m_IsInCollectMarch);
		Utils.RegisterFunc(L, -3, "IsInAssistanceMarch", _m_IsInAssistanceMarch);
		Utils.RegisterFunc(L, -3, "IsSelfInCurrentMarchTeam", _m_IsSelfInCurrentMarchTeam);
		Utils.RegisterFunc(L, -3, "GetMyAssistanceCount", _m_GetMyAssistanceCount);
		Utils.RegisterFunc(L, -3, "GetMyAssistanceFirstHero", _m_GetMyAssistanceFirstHero);
		Utils.RegisterFunc(L, -3, "MarkPointIsDirty", _m_MarkPointIsDirty);
		Utils.RegisterFunc(L, -3, "GetMarchesBossInfo", _m_GetMarchesBossInfo);
		Utils.RegisterFunc(L, -3, "GetMonsterListInArea", _m_GetMonsterListInArea);
		Utils.RegisterFunc(L, -3, "DestroyBerserkBossMarchData", _m_DestroyBerserkBossMarchData);
		Utils.RegisterFunc(L, -3, "SaveCreateMarchRecordTime", _m_SaveCreateMarchRecordTime);
		Utils.RegisterFunc(L, -3, "RemoveCreateMarchRecordTime", _m_RemoveCreateMarchRecordTime);
		Utils.RegisterFunc(L, -3, "CanUseInput", _m_CanUseInput);
		Utils.RegisterFunc(L, -3, "SetUseInput", _m_SetUseInput);
		Utils.RegisterFunc(L, -3, "SetSelectedPickable", _m_SetSelectedPickable);
		Utils.RegisterFunc(L, -3, "DragSelectedPickable", _m_DragSelectedPickable);
		Utils.RegisterFunc(L, -3, "ShowLoad", _m_ShowLoad);
		Utils.RegisterFunc(L, -3, "SetDragFormationData", _m_SetDragFormationData);
		Utils.RegisterFunc(L, -3, "GetClickWorldBulidingPos", _m_GetClickWorldBulidingPos);
		Utils.RegisterFunc(L, -3, "HideTouchEffect", _m_HideTouchEffect);
		Utils.RegisterFunc(L, -3, "GetRaycastHitMarch", _m_GetRaycastHitMarch);
		Utils.RegisterFunc(L, -3, "IsTileWalkable", _m_IsTileWalkable);
		Utils.RegisterFunc(L, -3, "AddOccupyPoints", _m_AddOccupyPoints);
		Utils.RegisterFunc(L, -3, "RemoveOccupyPoints", _m_RemoveOccupyPoints);
		Utils.RegisterFunc(L, -3, "GreenAreaChange", _m_GreenAreaChange);
		Utils.RegisterFunc(L, -3, "UpdateGreenArea", _m_UpdateGreenArea);
		Utils.RegisterFunc(L, -3, "IsGreen", _m_IsGreen);
		Utils.RegisterFunc(L, -3, "CanGreen", _m_CanGreen);
		Utils.RegisterFunc(L, -3, "SetStaticVisibleChunk", _m_SetStaticVisibleChunk);
		Utils.RegisterFunc(L, -3, "GetModelHeight", _m_GetModelHeight);
		Utils.RegisterFunc(L, -3, "GetTroop", _m_GetTroop);
		Utils.RegisterFunc(L, -3, "GetDestinationType", _m_GetDestinationType);
		Utils.RegisterFunc(L, -3, "GetTargetType", _m_GetTargetType);
		Utils.RegisterFunc(L, -3, "CreateBattleVFX", _m_CreateBattleVFX);
		Utils.RegisterFunc(L, -3, "CreateVFX", _m_CreateVFX);
		Utils.RegisterFunc(L, -3, "RemoveVFX", _m_RemoveVFX);
		Utils.RegisterFunc(L, -3, "OnTroopDragUpdate", _m_OnTroopDragUpdate);
		Utils.RegisterFunc(L, -3, "OnTroopDragStop", _m_OnTroopDragStop);
		Utils.RegisterFunc(L, -3, "GetPointSize", _m_GetPointSize);
		Utils.RegisterFunc(L, -3, "IsTroopCreate", _m_IsTroopCreate);
		Utils.RegisterFunc(L, -3, "CreateTroop", _m_CreateTroop);
		Utils.RegisterFunc(L, -3, "UpdateTroop", _m_UpdateTroop);
		Utils.RegisterFunc(L, -3, "DestroyTroop", _m_DestroyTroop);
		Utils.RegisterFunc(L, -3, "CreateTroopLine", _m_CreateTroopLine);
		Utils.RegisterFunc(L, -3, "DestroyTroopLine", _m_DestroyTroopLine);
		Utils.RegisterFunc(L, -3, "UpdateTroopLine", _m_UpdateTroopLine);
		Utils.RegisterFunc(L, -3, "IsTruckRoad", _m_IsTruckRoad);
		Utils.RegisterFunc(L, -3, "RemoveCullingBounds", _m_RemoveCullingBounds);
		Utils.RegisterFunc(L, -3, "AddCullingBounds", _m_AddCullingBounds);
		Utils.RegisterFunc(L, -3, "BattleFinish", _m_BattleFinish);
		Utils.RegisterFunc(L, -3, "UpdateBattleMessage", _m_UpdateBattleMessage);
		Utils.RegisterFunc(L, -3, "StartPrintRoad", _m_StartPrintRoad);
		Utils.RegisterFunc(L, -3, "StartPrintRoadByPathStr", _m_StartPrintRoadByPathStr);
		Utils.RegisterFunc(L, -3, "FinishPrintRoad", _m_FinishPrintRoad);
		Utils.RegisterFunc(L, -3, "UICreateBoard", _m_UICreateBoard);
		Utils.RegisterFunc(L, -3, "UIHideBoard", _m_UIHideBoard);
		Utils.RegisterFunc(L, -3, "UIDestroyRoad", _m_UIDestroyRoad);
		Utils.RegisterFunc(L, -3, "UICreateBuilding", _m_UICreateBuilding);
		Utils.RegisterFunc(L, -3, "UICreateAllianceBuilding", _m_UICreateAllianceBuilding);
		Utils.RegisterFunc(L, -3, "UIChangeBuilding", _m_UIChangeBuilding);
		Utils.RegisterFunc(L, -3, "UIDestroyBuilding", _m_UIDestroyBuilding);
		Utils.RegisterFunc(L, -3, "UIChangeRoad", _m_UIChangeRoad);
		Utils.RegisterFunc(L, -3, "UIChangeAllianceBuilding", _m_UIChangeAllianceBuilding);
		Utils.RegisterFunc(L, -3, "UIDestroyAllianceBuilding", _m_UIDestroyAllianceBuilding);
		Utils.RegisterFunc(L, -3, "UICreateWorldMoveMarch", _m_UICreateWorldMoveMarch);
		Utils.RegisterFunc(L, -3, "UICreateWorldTrigger", _m_UICreateWorldTrigger);
		Utils.RegisterFunc(L, -3, "UIDestroyRreCreateMarch", _m_UIDestroyRreCreateMarch);
		Utils.RegisterFunc(L, -3, "UIDestroyPreCreateTrigger", _m_UIDestroyPreCreateTrigger);
		Utils.RegisterFunc(L, -3, "UICreateWorldFlowerTrain", _m_UICreateWorldFlowerTrain);
		Utils.RegisterFunc(L, -3, "UIDestroyPreCreateFlowerTrain", _m_UIDestroyPreCreateFlowerTrain);
		Utils.RegisterFunc(L, -3, "UIDestroyRreCreateBuild", _m_UIDestroyRreCreateBuild);
		Utils.RegisterFunc(L, -3, "UIDestroyRreCreateAllianceBuild", _m_UIDestroyRreCreateAllianceBuild);
		Utils.RegisterFunc(L, -3, "AddObjectByPointId", _m_AddObjectByPointId);
		Utils.RegisterFunc(L, -3, "GetObjectByPointId", _m_GetObjectByPointId);
		Utils.RegisterFunc(L, -3, "CreateRoadRobot", _m_CreateRoadRobot);
		Utils.RegisterFunc(L, -3, "AddToNeedRemoveList", _m_AddToNeedRemoveList);
		Utils.RegisterFunc(L, -3, "GetRoadRobot", _m_GetRoadRobot);
		Utils.RegisterFunc(L, -3, "ChangeBuildRobotState", _m_ChangeBuildRobotState);
		Utils.RegisterFunc(L, -3, "ChangeBuildRobotFinishTime", _m_ChangeBuildRobotFinishTime);
		Utils.RegisterFunc(L, -3, "CreateBuildRobot", _m_CreateBuildRobot);
		Utils.RegisterFunc(L, -3, "CreateOtherBuildRobot", _m_CreateOtherBuildRobot);
		Utils.RegisterFunc(L, -3, "GetBuildRobot", _m_GetBuildRobot);
		Utils.RegisterFunc(L, -3, "CreateAnimalObject", _m_CreateAnimalObject);
		Utils.RegisterFunc(L, -3, "DestroyAnimalObject", _m_DestroyAnimalObject);
		Utils.RegisterFunc(L, -3, "InitFogOfWar", _m_InitFogOfWar);
		Utils.RegisterFunc(L, -3, "ReInitFogOfWar", _m_ReInitFogOfWar);
		Utils.RegisterFunc(L, -3, "UnlockFogOfWar", _m_UnlockFogOfWar);
		Utils.RegisterFunc(L, -3, "UnlockFogOfWar2x2", _m_UnlockFogOfWar2x2);
		Utils.RegisterFunc(L, -3, "SetFogVisible", _m_SetFogVisible);
		Utils.RegisterFunc(L, -3, "RegisterFogCompleteAction", _m_RegisterFogCompleteAction);
		Utils.RegisterFunc(L, -3, "ReInitObject", _m_ReInitObject);
		Utils.RegisterFunc(L, -3, "ClearReInitObject", _m_ClearReInitObject);
		Utils.RegisterFunc(L, -3, "GetCityTroop", _m_GetCityTroop);
		Utils.RegisterFunc(L, -3, "GetFormationUuid", _m_GetFormationUuid);
		Utils.RegisterFunc(L, -3, "LoadCityTroop", _m_LoadCityTroop);
		Utils.RegisterFunc(L, -3, "DestroyCityTroop", _m_DestroyCityTroop);
		Utils.RegisterFunc(L, -3, "GetLodConfigs", _m_GetLodConfigs);
		Utils.RegisterFunc(L, -3, "CreateCitySpaceMan", _m_CreateCitySpaceMan);
		Utils.RegisterFunc(L, -3, "DestroyCitySpaceMan", _m_DestroyCitySpaceMan);
		Utils.RegisterFunc(L, -3, "AddLodAdjuster", _m_AddLodAdjuster);
		Utils.RegisterFunc(L, -3, "RemoveLodAdjuster", _m_RemoveLodAdjuster);
		Utils.RegisterFunc(L, -3, "SetVisibleByPointType", _m_SetVisibleByPointType);
		Utils.RegisterFunc(L, -3, "GetSpecialPointDic", _m_GetSpecialPointDic);
		Utils.RegisterFunc(L, -3, "SetFocusPoint", _m_SetFocusPoint);
		Utils.RegisterFunc(L, -3, "SetCameraMaxHeight", _m_SetCameraMaxHeight);
		Utils.RegisterFunc(L, -3, "SetCameraMinHeight", _m_SetCameraMinHeight);
		Utils.RegisterFunc(L, -3, "SetCameraRotRange", _m_SetCameraRotRange);
		Utils.RegisterFunc(L, -3, "ResetCameraMaxHeight", _m_ResetCameraMaxHeight);
		Utils.RegisterFunc(L, -3, "ResetCameraMinHeight", _m_ResetCameraMinHeight);
		Utils.RegisterFunc(L, -3, "GetPreviousLodDistance", _m_GetPreviousLodDistance);
		Utils.RegisterFunc(L, -3, "DrawBuildGrid", _m_DrawBuildGrid);
		Utils.RegisterFunc(L, -3, "EnterTimeline", _m_EnterTimeline);
		Utils.RegisterFunc(L, -3, "ExitTimeline", _m_ExitTimeline);
		Utils.RegisterFunc(L, -3, "LockCamera", _m_LockCamera);
		Utils.RegisterFunc(L, -3, "FreeCamera", _m_FreeCamera);
		Utils.RegisterFunc(L, -3, "TestNetworkDisconnect", _m_TestNetworkDisconnect);
		Utils.RegisterFunc(L, -3, "GetZoneIdByPosId", _m_GetZoneIdByPosId);
		Utils.RegisterFunc(L, -3, "GetZoneIdByWorldPos", _m_GetZoneIdByWorldPos);
		Utils.RegisterFunc(L, -3, "OnBloodyNightActivityRefresh", _m_OnBloodyNightActivityRefresh);
		Utils.RegisterFunc(L, -3, "IsDawn", _m_IsDawn);
		Utils.RegisterFunc(L, -3, "OnBatteryPowerResourceUpdated", _m_OnBatteryPowerResourceUpdated);
		Utils.RegisterFunc(L, -3, "OnPveBattleExit", _m_OnPveBattleExit);
		Utils.RegisterFunc(L, -3, "OnPveBattleEnter", _m_OnPveBattleEnter);
		Utils.RegisterFunc(L, -3, "IsPointInAllianceCity", _m_IsPointInAllianceCity);
		Utils.RegisterFunc(L, -3, "IsPointInBlackArea", _m_IsPointInBlackArea);
		Utils.RegisterFunc(L, -3, "ShowBlackArea", _m_ShowBlackArea);
		Utils.RegisterFunc(L, -3, "HideBlackArea", _m_HideBlackArea);
		Utils.RegisterFunc(L, -3, "GetZoneData", _m_GetZoneData);
		Utils.RegisterFunc(L, -3, "UpdateBattleSoundData", _m_UpdateBattleSoundData);
		Utils.RegisterFunc(L, -3, "Description", _m_Description);
		Utils.RegisterFunc(L, -3, "GetCurSeasonType", _m_GetCurSeasonType);
		Utils.RegisterFunc(L, -3, "AfterUpdate", _e_AfterUpdate);
		Utils.RegisterFunc(L, -2, "BlockCount", _g_get_BlockCount);
		Utils.RegisterFunc(L, -2, "BlockSize", _g_get_BlockSize);
		Utils.RegisterFunc(L, -2, "TileSize", _g_get_TileSize);
		Utils.RegisterFunc(L, -2, "TileCount", _g_get_TileCount);
		Utils.RegisterFunc(L, -2, "DynamicObjNode", _g_get_DynamicObjNode);
		Utils.RegisterFunc(L, -2, "BuildBubbleNode", _g_get_BuildBubbleNode);
		Utils.RegisterFunc(L, -2, "SceneInstanceGameObject", _g_get_SceneInstanceGameObject);
		Utils.RegisterFunc(L, -2, "Transform", _g_get_Transform);
		Utils.RegisterFunc(L, -2, "CurTileCountXMin", _g_get_CurTileCountXMin);
		Utils.RegisterFunc(L, -2, "CurTileCountXMax", _g_get_CurTileCountXMax);
		Utils.RegisterFunc(L, -2, "CurTileCountYMin", _g_get_CurTileCountYMin);
		Utils.RegisterFunc(L, -2, "CurTileCountYMax", _g_get_CurTileCountYMax);
		Utils.RegisterFunc(L, -2, "WorldSize", _g_get_WorldSize);
		Utils.RegisterFunc(L, -2, "CurTarget", _g_get_CurTarget);
		Utils.RegisterFunc(L, -2, "InitZoom", _g_get_InitZoom);
		Utils.RegisterFunc(L, -2, "Zoom", _g_get_Zoom);
		Utils.RegisterFunc(L, -2, "CanMoving", _g_get_CanMoving);
		Utils.RegisterFunc(L, -2, "DisableClampToEdge", _g_get_DisableClampToEdge);
		Utils.RegisterFunc(L, -2, "TouchInputController", _g_get_TouchInputController);
		Utils.RegisterFunc(L, -2, "IsFocus", _g_get_IsFocus);
		Utils.RegisterFunc(L, -2, "CurTilePos", _g_get_CurTilePos);
		Utils.RegisterFunc(L, -2, "CurTilePosClamped", _g_get_CurTilePosClamped);
		Utils.RegisterFunc(L, -2, "Enabled", _g_get_Enabled);
		Utils.RegisterFunc(L, -2, "frameBufferWidth", _g_get_frameBufferWidth);
		Utils.RegisterFunc(L, -2, "frameBufferHeight", _g_get_frameBufferHeight);
		Utils.RegisterFunc(L, -2, "curIndex", _g_get_curIndex);
		Utils.RegisterFunc(L, -2, "curTouchTile", _g_get_curTouchTile);
		Utils.RegisterFunc(L, -2, "curTouchPoint", _g_get_curTouchPoint);
		Utils.RegisterFunc(L, -2, "marchUuid", _g_get_marchUuid);
		Utils.RegisterFunc(L, -2, "touchPickablePos", _g_get_touchPickablePos);
		Utils.RegisterFunc(L, -2, "SelectBuild", _g_get_SelectBuild);
		Utils.RegisterFunc(L, -2, "preCreateBuild", _g_get_preCreateBuild);
		Utils.RegisterFunc(L, -2, "placeFalseBuild", _g_get_placeFalseBuild);
		Utils.RegisterFunc(L, -2, "LodCameraDistanceScale", _g_get_LodCameraDistanceScale);
		Utils.RegisterFunc(L, -2, "mCityNightRenderer", _g_get_mCityNightRenderer);
		Utils.RegisterFunc(L, -1, "InitZoom", _s_set_InitZoom);
		Utils.RegisterFunc(L, -1, "Zoom", _s_set_Zoom);
		Utils.RegisterFunc(L, -1, "CanMoving", _s_set_CanMoving);
		Utils.RegisterFunc(L, -1, "DisableClampToEdge", _s_set_DisableClampToEdge);
		Utils.RegisterFunc(L, -1, "Enabled", _s_set_Enabled);
		Utils.RegisterFunc(L, -1, "curIndex", _s_set_curIndex);
		Utils.RegisterFunc(L, -1, "curTouchTile", _s_set_curTouchTile);
		Utils.RegisterFunc(L, -1, "curTouchPoint", _s_set_curTouchPoint);
		Utils.RegisterFunc(L, -1, "marchUuid", _s_set_marchUuid);
		Utils.RegisterFunc(L, -1, "touchPickablePos", _s_set_touchPickablePos);
		Utils.RegisterFunc(L, -1, "SelectBuild", _s_set_SelectBuild);
		Utils.RegisterFunc(L, -1, "preCreateBuild", _s_set_preCreateBuild);
		Utils.RegisterFunc(L, -1, "placeFalseBuild", _s_set_placeFalseBuild);
		Utils.RegisterFunc(L, -1, "mCityNightRenderer", _s_set_mCityNightRenderer);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 0);
		Utils.RegisterFunc(L, -2, "LodArray", _g_get_LodArray);
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
				CityScene o = new CityScene();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			GameObject go = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			cityScene.Init(go);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnSkinChange(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool ignoreCache = Lua.lua_toboolean(L, 2);
			obj.OnSkinChange(ignoreCache);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLabelSkinColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetPeopleById(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			GameObject peopleById = obj.GetPeopleById(index);
			objectTranslator.Push(L, peopleById);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PausePeopleAndPlayAnim(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			string anim = Lua.lua_tostring(L, 3);
			float num = obj.PausePeopleAndPlayAnim(index, anim);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResumePeople(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.ResumePeople(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateScene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Action>(L, 2))
			{
				Action callback = objectTranslator.GetDelegate<Action>(L, 2);
				cityScene.CreateScene(callback);
				return 0;
			}
			if (num == 1)
			{
				cityScene.CreateScene();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.CreateScene!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadCityPostProcessVolume(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoadCityPostProcessVolume();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetNewPostProcessVolume(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetNewPostProcessVolume();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CrateS4NightColorRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			objectTranslator.Get(L, 3, out Color val2);
			CityNightColorRenderer o = cityScene.CrateS4NightColorRenderer(val, val2);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CloseS4NightColorRenderer(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CloseS4NightColorRenderer();
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_UpdateFakeSampleMarchDataWhenBack(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_UpdateFakeSampleMarchDataWhenStartPick(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_AddFakeSampleMarchData(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long startIndex = Lua.lua_toint64(L, 2);
			long endIndex = Lua.lua_toint64(L, 3);
			long startTime = Lua.lua_toint64(L, 4);
			long endTime = Lua.lua_toint64(L, 5);
			int marchTargetType = Lua.xlua_tointeger(L, 6);
			obj.AddFakeSampleMarchData(startIndex, endIndex, startTime, endTime, marchTargetType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndDig(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EndDig();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UninitSubModulesAndCameraUpdate(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UninitSubModulesAndCameraUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UninitSubModulesAndCameraUpdate_withoutCameraMove(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UninitSubModulesAndCameraUpdate_withoutCameraMove();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UninitCameraMovement(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UninitCameraMovement();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Uninit(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Uninit();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeQualitySetting(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeQualitySetting();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FixedUpdate(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FixedUpdate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TileToWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int tilePosX = Lua.xlua_tointeger(L, 2);
				int tilePosY = Lua.xlua_tointeger(L, 3);
				Vector3 val = cityScene.TileToWorld(tilePosX, tilePosY);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				Vector3 val2 = cityScene.TileToWorld(v);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.TileToWorld!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToTile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector2Int vector2Int = cityScene.WorldToTile(val);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SnapToTileCenter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = cityScene.SnapToTileCenter(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TileFloatToWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float x = (float)Lua.lua_tonumber(L, 2);
				float y = (float)Lua.lua_tonumber(L, 3);
				Vector3 val = cityScene.TileFloatToWorld(x, y);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector2>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2 val2);
				Vector3 val3 = cityScene.TileFloatToWorld(val2);
				objectTranslator.PushUnityEngineVector3(L, val3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.TileFloatToWorld!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToTileFloat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector2 val2 = cityScene.WorldToTileFloat(val);
			objectTranslator.PushUnityEngineVector2(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IndexToTilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			Vector2Int vector2Int = obj.IndexToTilePos(index);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TilePosToIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int value = cityScene.TilePosToIndex(v);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TileIndexToWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index = Lua.xlua_tointeger(L, 2);
				Vector3 val = cityScene.TileIndexToWorld(index);
				objectTranslator.PushUnityEngineVector3(L, val);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				int serverId = Lua.xlua_tointeger(L, 3);
				Vector3 val2 = cityScene.TileIndexToWorld(index2, serverId);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.TileIndexToWorld!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToTileIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			int value = cityScene.WorldToTileIndex(val);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TileDistance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			objectTranslator.Get(L, 3, out Vector2Int v2);
			float num = cityScene.TileDistance(v, v2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorldSize(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int worldSize = Lua.xlua_tointeger(L, 2);
				cityScene.SetWorldSize(worldSize);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int width = Lua.xlua_tointeger(L, 2);
				int height = Lua.xlua_tointeger(L, 3);
				cityScene.SetWorldSize(width, height);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.SetWorldSize!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetMapZoneActive(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool mapZoneActive = Lua.lua_toboolean(L, 2);
			obj.SetMapZoneActive(mapZoneActive);
			return 0;
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_ChangeServer(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			obj.ChangeServer(serverId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnChangeServerRemove(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnChangeServerRemove();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveBlackDesert(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveBlackDesert();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBlackBlock(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitBlackBlock();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDragonLandRangeObj(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject dragonLandRangeObj = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetDragonLandRangeObj();
			objectTranslator.Push(L, dragonLandRangeObj);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateDragonLandRange(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateDragonLandRange();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveDragonLandRange(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveDragonLandRange();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveDragonLandPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			obj.RemoveDragonLandPoint(pointIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWolfStatusChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandlePushWolfStatusChange(message);
			return 0;
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
			List<BuildPointInfo> mainByScreen = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetMainByScreen();
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
			List<BuildPointInfo> allMainBaseList = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetAllMainBaseList();
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<PlayerType>(L, 2))
			{
				objectTranslator.Get(L, 2, out PlayerType val);
				List<BuildPointInfo> allMainBaseListByType = cityScene.GetAllMainBaseListByType(val);
				objectTranslator.Push(L, allMainBaseListByType);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<PlayerType>(L, 2) && objectTranslator.Assignable<PlayerType>(L, 3))
			{
				objectTranslator.Get(L, 2, out PlayerType val2);
				objectTranslator.Get(L, 3, out PlayerType val3);
				List<BuildPointInfo> allMainBaseListByType2 = cityScene.GetAllMainBaseListByType(val2, val3);
				objectTranslator.Push(L, allMainBaseListByType2);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<PlayerType>(L, 2) && objectTranslator.Assignable<PlayerType>(L, 3) && objectTranslator.Assignable<PlayerType>(L, 4))
			{
				objectTranslator.Get(L, 2, out PlayerType val4);
				objectTranslator.Get(L, 3, out PlayerType val5);
				objectTranslator.Get(L, 4, out PlayerType val6);
				List<BuildPointInfo> allMainBaseListByType3 = cityScene.GetAllMainBaseListByType(val4, val5, val6);
				objectTranslator.Push(L, allMainBaseListByType3);
				return 1;
			}
			if (num == 5 && objectTranslator.Assignable<PlayerType>(L, 2) && objectTranslator.Assignable<PlayerType>(L, 3) && objectTranslator.Assignable<PlayerType>(L, 4) && objectTranslator.Assignable<PlayerType>(L, 5))
			{
				objectTranslator.Get(L, 2, out PlayerType val7);
				objectTranslator.Get(L, 3, out PlayerType val8);
				objectTranslator.Get(L, 4, out PlayerType val9);
				objectTranslator.Get(L, 5, out PlayerType val10);
				List<BuildPointInfo> allMainBaseListByType4 = cityScene.GetAllMainBaseListByType(val7, val8, val9, val10);
				objectTranslator.Push(L, allMainBaseListByType4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GetAllMainBaseListByType!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllDragonPointList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<PointInfo> allDragonPointList = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetAllDragonPointList();
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
			List<int> allDragonResourceList = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetAllDragonResourceList();
			objectTranslator.Push(L, allDragonResourceList);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowTruck(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			List<List<Vector2Int>> path = (List<List<Vector2Int>>)objectTranslator.GetObject(L, 2, typeof(List<List<Vector2Int>>));
			cityScene.ShowTruck(path);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowFreeTruck(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			List<List<Vector2Int>> path = (List<List<Vector2Int>>)objectTranslator.GetObject(L, 2, typeof(List<List<Vector2Int>>));
			WorldCityTruck freeTruck = (WorldCityTruck)objectTranslator.GetObject(L, 3, typeof(WorldCityTruck));
			cityScene.ShowFreeTruck(path, freeTruck);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTruckPathString(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			List<Vector2Int> tempList = (List<Vector2Int>)objectTranslator.GetObject(L, 2, typeof(List<Vector2Int>));
			List<Vector2Int> truckPathString = cityScene.GetTruckPathString(tempList);
			objectTranslator.Push(L, truckPathString);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexByOffset(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int index = Lua.xlua_tointeger(L, 2);
				int x = Lua.xlua_tointeger(L, 3);
				int y = Lua.xlua_tointeger(L, 4);
				int indexByOffset = cityScene.GetIndexByOffset(index, x, y);
				Lua.xlua_pushinteger(L, indexByOffset);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				int x2 = Lua.xlua_tointeger(L, 3);
				int indexByOffset2 = cityScene.GetIndexByOffset(index2, x2);
				Lua.xlua_pushinteger(L, indexByOffset2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index3 = Lua.xlua_tointeger(L, 2);
				int indexByOffset3 = cityScene.GetIndexByOffset(index3);
				Lua.xlua_pushinteger(L, indexByOffset3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GetIndexByOffset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexByOffsetByDirection(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int dir = Lua.xlua_tointeger(L, 3);
			int indexByOffsetByDirection = obj.GetIndexByOffsetByDirection(index, dir);
			Lua.xlua_pushinteger(L, indexByOffsetByDirection);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPlayerBankruptcyFinish(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string uid = Lua.lua_tostring(L, 2);
			obj.OnPlayerBankruptcyFinish(uid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnExtendDome(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnExtendDome();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBattleBlood(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<object>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				object param = objectTranslator.GetObject(L, 2, typeof(object));
				string path = Lua.lua_tostring(L, 3);
				cityScene.ShowBattleBlood(param, path);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 2))
			{
				object param2 = objectTranslator.GetObject(L, 2, typeof(object));
				cityScene.ShowBattleBlood(param2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.ShowBattleBlood!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterPhysics(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			MonoBehaviour obj = (MonoBehaviour)objectTranslator.GetObject(L, 2, typeof(MonoBehaviour));
			cityScene.RegisterPhysics(obj);
			return 0;
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_UnregisterPhysics(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			MonoBehaviour obj = (MonoBehaviour)objectTranslator.GetObject(L, 2, typeof(MonoBehaviour));
			cityScene.UnregisterPhysics(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTouchTilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2Int touchTilePos = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetTouchTilePos();
			objectTranslator.Push(L, touchTilePos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTouchPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				Vector3 touchPoint2 = cityScene.GetTouchPoint();
				objectTranslator.PushUnityEngineVector3(L, touchPoint2);
				return 1;
			}
			case 2:
				if (objectTranslator.Assignable<Vector3>(L, 2))
				{
					objectTranslator.Get(L, 2, out Vector3 val);
					Vector3 touchPoint = cityScene.GetTouchPoint(val);
					objectTranslator.PushUnityEngineVector3(L, touchPoint);
					return 1;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GetTouchPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRaycastGroundPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 raycastGroundPoint = cityScene.GetRaycastGroundPoint(val);
			objectTranslator.PushUnityEngineVector3(L, raycastGroundPoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoFocus(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 7 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<LookAtFocusState>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6) && objectTranslator.Assignable<Action>(L, 7))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				objectTranslator.Get(L, 3, out LookAtFocusState v);
				float time = (float)Lua.lua_tonumber(L, 4);
				bool focusToCenter = Lua.lua_toboolean(L, 5);
				bool lockView = Lua.lua_toboolean(L, 6);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 7);
				cityScene.AutoFocus(val, v, time, focusToCenter, lockView, onComplete);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<LookAtFocusState>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				objectTranslator.Get(L, 3, out LookAtFocusState v2);
				float time2 = (float)Lua.lua_tonumber(L, 4);
				bool focusToCenter2 = Lua.lua_toboolean(L, 5);
				bool lockView2 = Lua.lua_toboolean(L, 6);
				cityScene.AutoFocus(val2, v2, time2, focusToCenter2, lockView2);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<LookAtFocusState>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out LookAtFocusState v3);
				float time3 = (float)Lua.lua_tonumber(L, 4);
				bool focusToCenter3 = Lua.lua_toboolean(L, 5);
				cityScene.AutoFocus(val3, v3, time3, focusToCenter3);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<LookAtFocusState>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out LookAtFocusState v4);
				float time4 = (float)Lua.lua_tonumber(L, 4);
				cityScene.AutoFocus(val4, v4, time4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.AutoFocus!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_QuitFocus(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float time = (float)Lua.lua_tonumber(L, 2);
			obj.QuitFocus(time);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLodDistance(IntPtr L)
	{
		try
		{
			float lodDistance = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLodDistance();
			Lua.lua_pushnumber(L, lodDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLodDistanceByLod(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int lod = Lua.xlua_tointeger(L, 2);
			float lodDistanceByLod = obj.GetLodDistanceByLod(lod);
			Lua.lua_pushnumber(L, lodDistanceByLod);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Quaternion rotation = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetRotation();
			objectTranslator.PushUnityEngineQuaternion(L, rotation);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Get_rotation(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Get_rotation(out var x, out var y, out var z, out var w);
			Lua.lua_pushnumber(L, x);
			Lua.lua_pushnumber(L, y);
			Lua.lua_pushnumber(L, z);
			Lua.lua_pushnumber(L, w);
			return 4;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMapIconScale(IntPtr L)
	{
		try
		{
			float mapIconScale = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMapIconScale();
			Lua.lua_pushnumber(L, mapIconScale);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCameraPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 cameraPos = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetCameraPos();
			objectTranslator.PushUnityEngineVector3(L, cameraPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToRay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Ray val2 = cityScene.ScreenPointToRay(val);
			objectTranslator.PushUnityEngineRay(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetMinLodDistance(IntPtr L)
	{
		try
		{
			float minLodDistance = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetMinLodDistance();
			Lua.lua_pushnumber(L, minLodDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToScreenPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = cityScene.WorldToScreenPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Lookat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityScene.Lookat(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoLookat(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Action>(L, 5))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float zoom = (float)Lua.lua_tonumber(L, 3);
				float time = (float)Lua.lua_tonumber(L, 4);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 5);
				cityScene.AutoLookat(val, zoom, time, onComplete);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector3 val2);
				float zoom2 = (float)Lua.lua_tonumber(L, 3);
				float time2 = (float)Lua.lua_tonumber(L, 4);
				cityScene.AutoLookat(val2, zoom2, time2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				float zoom3 = (float)Lua.lua_tonumber(L, 3);
				cityScene.AutoLookat(val3, zoom3);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val4);
				cityScene.AutoLookat(val4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.AutoLookat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AutoZoom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Action>(L, 4))
			{
				float zoom = (float)Lua.lua_tonumber(L, 2);
				float time = (float)Lua.lua_tonumber(L, 3);
				Action onComplete = objectTranslator.GetDelegate<Action>(L, 4);
				cityScene.AutoZoom(zoom, time, onComplete);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float zoom2 = (float)Lua.lua_tonumber(L, 2);
				float time2 = (float)Lua.lua_tonumber(L, 3);
				cityScene.AutoZoom(zoom2, time2);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float zoom3 = (float)Lua.lua_tonumber(L, 2);
				cityScene.AutoZoom(zoom3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.AutoZoom!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToWorld(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				float disPlane = (float)Lua.lua_tonumber(L, 3);
				Vector3 val2 = cityScene.ScreenPointToWorld(val, disPlane);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				Vector3 val4 = cityScene.ScreenPointToWorld(val3);
				objectTranslator.PushUnityEngineVector3(L, val4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.ScreenPointToWorld!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLodLevel(IntPtr L)
	{
		try
		{
			int lodLevel = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetLodLevel();
			Lua.xlua_pushinteger(L, lodLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrackMarch(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchId = Lua.lua_toint64(L, 2);
			obj.TrackMarch(marchId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TrackHSR(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long marchId = Lua.lua_toint64(L, 2);
			GameObject go = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			cityScene.TrackHSR(marchId, go);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DisablePostProcess(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisablePostProcess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnablePostProcess(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).EnablePostProcess();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTouchInputControllerEnable(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool touchInputControllerEnable = Lua.lua_toboolean(L, 2);
			obj.SetTouchInputControllerEnable(touchInputControllerEnable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCameraTouchEnable(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool cameraTouchEnable = Lua.lua_toboolean(L, 2);
			obj.SetCameraTouchEnable(cameraTouchEnable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroopLineColor(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string colorArgs = Lua.lua_tostring(L, 2);
			obj.UpdateTroopLineColor(colorArgs);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTouchInputControllerEnable(IntPtr L)
	{
		try
		{
			bool touchInputControllerEnable = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetTouchInputControllerEnable();
			Lua.lua_pushboolean(L, touchInputControllerEnable);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StopCameraMove(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).StopCameraMove();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexByOffset_New(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				int index = Lua.xlua_tointeger(L, 2);
				int x = Lua.xlua_tointeger(L, 3);
				int y = Lua.xlua_tointeger(L, 4);
				int indexByOffset_New = cityScene.GetIndexByOffset_New(index, x, y);
				Lua.xlua_pushinteger(L, indexByOffset_New);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				int x2 = Lua.xlua_tointeger(L, 3);
				int indexByOffset_New2 = cityScene.GetIndexByOffset_New(index2, x2);
				Lua.xlua_pushinteger(L, indexByOffset_New2);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index3 = Lua.xlua_tointeger(L, 2);
				int indexByOffset_New3 = cityScene.GetIndexByOffset_New(index3);
				Lua.xlua_pushinteger(L, indexByOffset_New3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GetIndexByOffset_New!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetIndexByOffsetByDirection_New(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int dir = Lua.xlua_tointeger(L, 3);
			int indexByOffsetByDirection_New = obj.GetIndexByOffsetByDirection_New(index, dir);
			Lua.xlua_pushinteger(L, indexByOffsetByDirection_New);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetZoomParams(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int level = Lua.xlua_tointeger(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float offsetZ = (float)Lua.lua_tonumber(L, 4);
			float sensitivity = (float)Lua.lua_tonumber(L, 5);
			obj.SetZoomParams(level, y, offsetZ, sensitivity);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCameraFOV(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float cameraFOV = (float)Lua.lua_tonumber(L, 2);
			obj.SetCameraFOV(cameraFOV);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZoomParams(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<MobileTouchCamera.ZoomParam> zoomParams = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetZoomParams();
			objectTranslator.Push(L, zoomParams);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFOV(IntPtr L)
	{
		try
		{
			float fOV = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFOV();
			Lua.lua_pushnumber(L, fOV);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInMap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			bool value = cityScene.IsInMap(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInMapByIndex(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsInMapByIndex(index);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClampTilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			Vector2Int vector2Int = cityScene.ClampTilePos(v);
			objectTranslator.Push(L, vector2Int);
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
			int collectResourceBuildRange = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCollectResourceBuildRange();
			Lua.xlua_pushinteger(L, collectResourceBuildRange);
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
			int collectResourceTile = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCollectResourceTile();
			Lua.xlua_pushinteger(L, collectResourceTile);
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
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckNeedRefreshRoad();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGlobalShaderLOD(IntPtr L)
	{
		try
		{
			int globalShaderLOD = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetGlobalShaderLOD();
			Lua.xlua_pushinteger(L, globalShaderLOD);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetGlobalShaderLOD(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int globalShaderLOD = Lua.xlua_tointeger(L, 2);
			bool value = obj.SetGlobalShaderLOD(globalShaderLOD);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetProfileTerrainSwitch(IntPtr L)
	{
		try
		{
			bool profileTerrainSwitch = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetProfileTerrainSwitch();
			Lua.lua_pushboolean(L, profileTerrainSwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggleTerrain(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleTerrain();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetProfileBuildingSwitch(IntPtr L)
	{
		try
		{
			bool profileBuildingSwitch = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetProfileBuildingSwitch();
			Lua.lua_pushboolean(L, profileBuildingSwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggleBuilding(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleBuilding();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetProfileStaticSwitch(IntPtr L)
	{
		try
		{
			bool profileStaticSwitch = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetProfileStaticSwitch();
			Lua.lua_pushboolean(L, profileStaticSwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggleStatic(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleStatic();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetHeightFogSwitch(IntPtr L)
	{
		try
		{
			bool heightFogSwitch = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetHeightFogSwitch();
			Lua.lua_pushboolean(L, heightFogSwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggleHeightFog(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleHeightFog();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGraphySwitch(IntPtr L)
	{
		try
		{
			bool graphySwitch = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetGraphySwitch();
			Lua.lua_pushboolean(L, graphySwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ProfileToggleMarch(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleMarch();
			return 0;
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetResourcePointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_IsCollectRangePoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetWorldTriggerData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			WorldTriggerData worldTriggerData = obj.GetWorldTriggerData(uuid);
			objectTranslator.Push(L, worldTriggerData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTriggerDataByPointId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			int serverId = Lua.xlua_tointeger(L, 3);
			WorldTriggerData triggerDataByPointId = obj.GetTriggerDataByPointId(pointIndex, serverId);
			objectTranslator.Push(L, triggerDataByPointId);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			PointInfo pointInfo = obj.GetPointInfo(pointIndex);
			objectTranslator.Push(L, pointInfo);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointInfoWithServer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			int serverId = Lua.xlua_tointeger(L, 3);
			PointInfo pointInfoWithServer = obj.GetPointInfoWithServer(pointIndex, serverId);
			objectTranslator.Push(L, pointInfoWithServer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetWorldDesertInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetMarchesByStartIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			WorldMarch marchesByStartIndex = obj.GetMarchesByStartIndex(pointIndex);
			objectTranslator.Push(L, marchesByStartIndex);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetCollectRangePoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetCollectPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_IsCollectPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetCollectInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetGarbagePointInfoByIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetPointInfoByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			PointInfo myPointInfo = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetMyPointInfo();
			objectTranslator.Push(L, myPointInfo);
			return 1;
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			WorldPointObject objectByPoint = obj.GetObjectByPoint(pointIndex);
			objectTranslator.Push(L, objectByPoint);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			BuildPointInfo baseMainByScreen = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetBaseMainByScreen();
			objectTranslator.Push(L, baseMainByScreen);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetBuildingByPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_SetLevelUpActive(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			bool active = Lua.lua_toboolean(L, 3);
			obj.SetLevelUpActive(pointIndex, active);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildingHeight(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			float buildingHeight = obj.GetBuildingHeight(pointIndex);
			Lua.lua_pushnumber(L, buildingHeight);
			return 1;
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleViewPointsReply(message);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleViewAssistanceInfoUpdateNotify(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleViewUpdateNotify(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleViewUpdateNotify(message);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleViewTileUpdateNotify(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleLandUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleLandUpdate(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SendViewRequest(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int viewLevel = Lua.xlua_tointeger(L, 3);
			int serverId = Lua.xlua_tointeger(L, 4);
			cityScene.SendViewRequest(v, viewLevel, serverId);
			return 0;
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsSelfRoad(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsBuildFinish(IntPtr L)
	{
		try
		{
			bool value = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsBuildFinish();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetBuildingByUuid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_HasPointInfo(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_AddToDeleteList(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetBaseMainInfoByOwnerUid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_HideObject(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsSelfPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetPointType(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int pointType = obj.GetPointType(index);
			Lua.xlua_pushinteger(L, pointType);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_AddObjectByPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			obj.AddObjectByPoint(point, type);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveObjectByPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			obj.RemoveObjectByPoint(point);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveOneObjectByPointType(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int pointType = Lua.xlua_tointeger(L, 3);
			obj.RemoveOneObjectByPointType(index, pointType);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RefreshView(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RefreshView();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnMainBuildMove(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnMainBuildMove();
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
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				bool isForce = Lua.lua_toboolean(L, 2);
				cityScene.UpdateViewRequest(isForce);
				return 0;
			}
			if (num == 1)
			{
				cityScene.UpdateViewRequest();
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.UpdateViewRequest!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFirstViewRequestFlag(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_IsRoadPoint(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetNearestPathForBuildingConnect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			List<Vector2Int> nearestPathForBuildingConnect = obj.GetNearestPathForBuildingConnect(uuid);
			objectTranslator.Push(L, nearestPathForBuildingConnect);
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
			List<int> garbagePoint = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetGarbagePoint();
			objectTranslator.Push(L, garbagePoint);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsMyEnemy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int serverId = Lua.xlua_tointeger(L, 2);
			string allianceId = Lua.lua_tostring(L, 3);
			PlayerType val = obj.IsMyEnemy(serverId, allianceId);
			objectTranslator.PushPlayerType(L, val);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CleanAllianceCacheData(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CleanAllianceCacheData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGroupTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			WorldTroop o = cityScene.CreateGroupTroop(march);
			objectTranslator.Push(L, o);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_IsTargetForMine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch marchData = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			bool value = cityScene.IsTargetForMine(marchData);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTargetForAlly(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch marchData = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			bool value = cityScene.IsTargetForAlly(marchData);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartMarch(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
				cityScene.StartMarch(targetType, targetPoint, targetUuid, timeIndex, marchUuid, formationUuid, backHome, sfsObjBinary, startPos, targetServerId);
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
				cityScene.StartMarch(targetType2, targetPoint2, targetUuid2, timeIndex2, marchUuid2, formationUuid2, backHome2, sfsObjBinary2, startPos2);
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
				cityScene.StartMarch(targetType3, targetPoint3, targetUuid3, timeIndex3, marchUuid3, formationUuid3, backHome3, sfsObjBinary3);
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
				cityScene.StartMarch(targetType4, targetPoint4, targetUuid4, timeIndex4, marchUuid4, formationUuid4, backHome4);
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
				cityScene.StartMarch(targetType5, targetPoint5, targetUuid5, timeIndex5, marchUuid5, formationUuid5);
				return 0;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) || Lua.lua_isint64(L, 6)))
			{
				int targetType6 = Lua.xlua_tointeger(L, 2);
				int targetPoint6 = Lua.xlua_tointeger(L, 3);
				long targetUuid6 = Lua.lua_toint64(L, 4);
				int timeIndex6 = Lua.xlua_tointeger(L, 5);
				long marchUuid6 = Lua.lua_toint64(L, 6);
				cityScene.StartMarch(targetType6, targetPoint6, targetUuid6, timeIndex6, marchUuid6, 0L);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int targetType7 = Lua.xlua_tointeger(L, 2);
				int targetPoint7 = Lua.xlua_tointeger(L, 3);
				long targetUuid7 = Lua.lua_toint64(L, 4);
				int timeIndex7 = Lua.xlua_tointeger(L, 5);
				cityScene.StartMarch(targetType7, targetPoint7, targetUuid7, timeIndex7, 0L, 0L);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.StartMarch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOwnerFormationMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid = Lua.lua_tostring(L, 2);
				long formationUuid = Lua.lua_toint64(L, 3);
				string allianceUid = Lua.lua_tostring(L, 4);
				WorldMarch ownerFormationMarch = cityScene.GetOwnerFormationMarch(ownerUid, formationUuid, allianceUid);
				objectTranslator.Push(L, ownerFormationMarch);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				string ownerUid2 = Lua.lua_tostring(L, 2);
				long formationUuid2 = Lua.lua_toint64(L, 3);
				WorldMarch ownerFormationMarch2 = cityScene.GetOwnerFormationMarch(ownerUid2, formationUuid2);
				objectTranslator.Push(L, ownerFormationMarch2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GetOwnerFormationMarch!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllianceMarchesInTeam(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
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
	private static int _m_GetOwnerMarches(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid = Lua.lua_tostring(L, 2);
				string allianceUid = Lua.lua_tostring(L, 3);
				List<WorldMarch> ownerMarches = cityScene.GetOwnerMarches(ownerUid, allianceUid);
				objectTranslator.Push(L, ownerMarches);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				string ownerUid2 = Lua.lua_tostring(L, 2);
				List<WorldMarch> ownerMarches2 = cityScene.GetOwnerMarches(ownerUid2);
				objectTranslator.Push(L, ownerMarches2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GetOwnerMarches!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldTriggerUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			int serverId = Lua.xlua_tointeger(L, 3);
			int worldId = Lua.xlua_tointeger(L, 4);
			cityScene.HandlePushWorldTriggerUpdate(message, serverId, worldId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushWorldTriggerDel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			int serverId = Lua.xlua_tointeger(L, 3);
			int worldId = Lua.xlua_tointeger(L, 4);
			cityScene.HandlePushWorldTriggerDel(message, serverId, worldId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandlePushMultiKillUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandlePushMultiKillUpdate(message);
			return 0;
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandlePushWorldMarchAdd(message);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandlePushWorldMarchDel(message);
			return 0;
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleWorldMarchGet(message);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleFormationMarch(message);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleFormationMarchChange(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HandleUpdateLightData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.HandleUpdateLightData(message);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetMyAssistanceCount(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			int myAssistanceCount = obj.GetMyAssistanceCount(pointIndex);
			Lua.xlua_pushinteger(L, myAssistanceCount);
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_MarkPointIsDirty(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_GetMarchesBossInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<long, WorldMarch> marchesBossInfo = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetMarchesBossInfo();
			objectTranslator.Push(L, marchesBossInfo);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int size = Lua.xlua_tointeger(L, 3);
			Dictionary<int, int> monsterIds = (Dictionary<int, int>)objectTranslator.GetObject(L, 4, typeof(Dictionary<int, int>));
			Dictionary<long, Vector2Int> result = (Dictionary<long, Vector2Int>)objectTranslator.GetObject(L, 5, typeof(Dictionary<long, Vector2Int>));
			cityScene.GetMonsterListInArea(v, size, monsterIds, result);
			return 0;
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
			string str = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SaveCreateMarchRecordTime();
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch worldMarch = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			cityScene.RemoveCreateMarchRecordTime(worldMarch);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanUseInput(IntPtr L)
	{
		try
		{
			bool value = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanUseInput();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetUseInput(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool useInput = Lua.lua_toboolean(L, 2);
			obj.SetUseInput(useInput);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetSelectedPickable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ITouchPickable selectedPickable = (ITouchPickable)objectTranslator.GetObject(L, 2, typeof(ITouchPickable));
			cityScene.SetSelectedPickable(selectedPickable);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DragSelectedPickable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityScene.DragSelectedPickable(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowLoad(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityScene.ShowLoad(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDragFormationData(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			int pointId = Lua.xlua_tointeger(L, 3);
			obj.SetDragFormationData(uuid, pointId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetClickWorldBulidingPos(IntPtr L)
	{
		try
		{
			int clickWorldBulidingPos = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetClickWorldBulidingPos();
			Lua.xlua_pushinteger(L, clickWorldBulidingPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideTouchEffect(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideTouchEffect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRaycastHitMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			long raycastHitMarch = cityScene.GetRaycastHitMarch(val);
			Lua.lua_pushint64(L, raycastHitMarch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTileWalkable(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			bool value = cityScene.IsTileWalkable(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddOccupyPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2Int>(L, 2) && objectTranslator.Assignable<Vector2Int>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				objectTranslator.Get(L, 3, out Vector2Int v2);
				int serverIn = Lua.xlua_tointeger(L, 4);
				cityScene.AddOccupyPoints(v, v2, serverIn);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2Int>(L, 2) && objectTranslator.Assignable<Vector2Int>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2Int v3);
				objectTranslator.Get(L, 3, out Vector2Int v4);
				cityScene.AddOccupyPoints(v3, v4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.AddOccupyPoints!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveOccupyPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<Vector2Int>(L, 2) && objectTranslator.Assignable<Vector2Int>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				objectTranslator.Get(L, 3, out Vector2Int v2);
				int serverIn = Lua.xlua_tointeger(L, 4);
				cityScene.RemoveOccupyPoints(v, v2, serverIn);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2Int>(L, 2) && objectTranslator.Assignable<Vector2Int>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2Int v3);
				objectTranslator.Get(L, 3, out Vector2Int v4);
				cityScene.RemoveOccupyPoints(v3, v4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.RemoveOccupyPoints!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GreenAreaChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<WorldAreaGreenInfo.GreenType>(L, 2) && objectTranslator.Assignable<HashSet<int>>(L, 3))
			{
				objectTranslator.Get(L, 2, out WorldAreaGreenInfo.GreenType v);
				HashSet<int> changePoints = (HashSet<int>)objectTranslator.GetObject(L, 3, typeof(HashSet<int>));
				cityScene.GreenAreaChange(v, changePoints);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<WorldAreaGreenInfo.GreenType>(L, 2))
			{
				objectTranslator.Get(L, 2, out WorldAreaGreenInfo.GreenType v2);
				cityScene.GreenAreaChange(v2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.GreenAreaChange!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGreenArea(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int xMin = Lua.xlua_tointeger(L, 2);
			int yMin = Lua.xlua_tointeger(L, 3);
			int xMax = Lua.xlua_tointeger(L, 4);
			int yMax = Lua.xlua_tointeger(L, 5);
			obj.UpdateGreenArea(xMin, yMin, xMax, yMax);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsGreen(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointIndex = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsGreen(pointIndex);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanGreen(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int pointIndex = Lua.xlua_tointeger(L, 2);
				bool showTip = Lua.lua_toboolean(L, 3);
				bool value = cityScene.CanGreen(pointIndex, showTip);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int pointIndex2 = Lua.xlua_tointeger(L, 2);
				bool value2 = cityScene.CanGreen(pointIndex2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.CanGreen!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStaticVisibleChunk(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int staticVisibleChunk = Lua.xlua_tointeger(L, 2);
			obj.SetStaticVisibleChunk(staticVisibleChunk);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModelHeight(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			float modelHeight = obj.GetModelHeight(marchUuid);
			Lua.lua_pushnumber(L, modelHeight);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			WorldTroop troop = obj.GetTroop(marchUuid);
			objectTranslator.Push(L, troop);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDestinationType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			long targetMarchUuid = Lua.lua_toint64(L, 3);
			int endPos = Lua.xlua_tointeger(L, 4);
			objectTranslator.Get(L, 5, out MarchTargetType v);
			bool isFormation = Lua.lua_toboolean(L, 6);
			objectTranslator.Get(L, 7, out Vector3 val);
			int tileSize = Lua.xlua_tointeger(L, 8);
			EnumDestinationSignalType destinationType = cityScene.GetDestinationType(marchUuid, targetMarchUuid, endPos, v, isFormation, ref val, ref tileSize);
			objectTranslator.Push(L, destinationType);
			objectTranslator.PushUnityEngineVector3(L, val);
			objectTranslator.UpdateUnityEngineVector3(L, 7, val);
			Lua.xlua_pushinteger(L, tileSize);
			return 3;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTargetType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long targetMarchUuid = Lua.lua_toint64(L, 2);
			int pointId = Lua.xlua_tointeger(L, 3);
			MarchTargetType targetType = obj.GetTargetType(targetMarchUuid, pointId);
			objectTranslator.Push(L, targetType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBattleVFX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			float life = (float)Lua.lua_tonumber(L, 3);
			Action<GameObject> onComplete = objectTranslator.GetDelegate<Action<GameObject>>(L, 4);
			cityScene.CreateBattleVFX(prefabPath, life, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateVFX(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Action<GameObject>>(L, 6))
			{
				string prefabPath = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				float duration = (float)Lua.lua_tonumber(L, 4);
				float delay = (float)Lua.lua_tonumber(L, 5);
				Action<GameObject> onComplete = objectTranslator.GetDelegate<Action<GameObject>>(L, 6);
				int value = cityScene.CreateVFX(prefabPath, val, duration, delay, onComplete);
				Lua.xlua_pushinteger(L, value);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				string prefabPath2 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				float delay2 = (float)Lua.lua_tonumber(L, 5);
				int value2 = cityScene.CreateVFX(prefabPath2, val2, duration2, delay2);
				Lua.xlua_pushinteger(L, value2);
				return 1;
			}
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string prefabPath3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				float duration3 = (float)Lua.lua_tonumber(L, 4);
				int value3 = cityScene.CreateVFX(prefabPath3, val3, duration3);
				Lua.xlua_pushinteger(L, value3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.CreateVFX!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveVFX(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			obj.RemoveVFX(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnTroopDragUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				long marchUuid = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				long targetMarchUuid = Lua.lua_toint64(L, 4);
				int startPointId = Lua.xlua_tointeger(L, 5);
				bool isFormation = Lua.lua_toboolean(L, 6);
				cityScene.OnTroopDragUpdate(marchUuid, val, targetMarchUuid, startPointId, isFormation);
				return 0;
			}
			if (num == 5 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				long marchUuid2 = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				long targetMarchUuid2 = Lua.lua_toint64(L, 4);
				int startPointId2 = Lua.xlua_tointeger(L, 5);
				cityScene.OnTroopDragUpdate(marchUuid2, val2, targetMarchUuid2, startPointId2);
				return 0;
			}
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) || Lua.lua_isint64(L, 4)))
			{
				long marchUuid3 = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				long targetMarchUuid3 = Lua.lua_toint64(L, 4);
				cityScene.OnTroopDragUpdate(marchUuid3, val3, targetMarchUuid3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.OnTroopDragUpdate!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnTroopDragStop(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 4))
			{
				long marchUuid = Lua.lua_toint64(L, 2);
				long targetMarchUuid = Lua.lua_toint64(L, 3);
				bool isFormation = Lua.lua_toboolean(L, 4);
				cityScene.OnTroopDragStop(marchUuid, targetMarchUuid, isFormation);
				return 0;
			}
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)))
			{
				long marchUuid2 = Lua.lua_toint64(L, 2);
				long targetMarchUuid2 = Lua.lua_toint64(L, 3);
				cityScene.OnTroopDragStop(marchUuid2, targetMarchUuid2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.OnTroopDragStop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointSize(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int pointSize = obj.GetPointSize(index);
			Lua.xlua_pushinteger(L, pointSize);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTroopCreate(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			bool value = obj.IsTroopCreate(marchUuid);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			cityScene.CreateTroop(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			cityScene.UpdateTroop(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyTroop(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				long marchUuid = Lua.lua_toint64(L, 2);
				bool isBattleFailed = Lua.lua_toboolean(L, 3);
				float num2 = cityScene.DestroyTroop(marchUuid, isBattleFailed);
				Lua.lua_pushnumber(L, num2);
				return 1;
			}
			if (num == 2 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)))
			{
				long marchUuid2 = Lua.lua_toint64(L, 2);
				float num3 = cityScene.DestroyTroop(marchUuid2);
				Lua.lua_pushnumber(L, num3);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.DestroyTroop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateTroopLine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
			cityScene.CreateTroopLine(march);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyTroopLine(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long marchUuid = Lua.lua_toint64(L, 2);
			obj.DestroyTroopLine(marchUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTroopLine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 8 && objectTranslator.Assignable<WorldMarch>(L, 2) && objectTranslator.Assignable<WorldTroopPathSegment[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Vector3>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				WorldMarch march = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				WorldTroopPathSegment[] path = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 3, typeof(WorldTroopPathSegment[]));
				int currPath = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out Vector3 val);
				int realTargetPos = Lua.xlua_tointeger(L, 6);
				bool needRefresh = Lua.lua_toboolean(L, 7);
				bool clear = Lua.lua_toboolean(L, 8);
				cityScene.UpdateTroopLine(march, path, currPath, val, realTargetPos, needRefresh, clear);
				return 0;
			}
			if (num == 7 && objectTranslator.Assignable<WorldMarch>(L, 2) && objectTranslator.Assignable<WorldTroopPathSegment[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Vector3>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 7))
			{
				WorldMarch march2 = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				WorldTroopPathSegment[] path2 = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 3, typeof(WorldTroopPathSegment[]));
				int currPath2 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out Vector3 val2);
				int realTargetPos2 = Lua.xlua_tointeger(L, 6);
				bool needRefresh2 = Lua.lua_toboolean(L, 7);
				cityScene.UpdateTroopLine(march2, path2, currPath2, val2, realTargetPos2, needRefresh2);
				return 0;
			}
			if (num == 6 && objectTranslator.Assignable<WorldMarch>(L, 2) && objectTranslator.Assignable<WorldTroopPathSegment[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Vector3>(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				WorldMarch march3 = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				WorldTroopPathSegment[] path3 = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 3, typeof(WorldTroopPathSegment[]));
				int currPath3 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out Vector3 val3);
				int realTargetPos3 = Lua.xlua_tointeger(L, 6);
				cityScene.UpdateTroopLine(march3, path3, currPath3, val3, realTargetPos3);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<WorldMarch>(L, 2) && objectTranslator.Assignable<WorldTroopPathSegment[]>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && objectTranslator.Assignable<Vector3>(L, 5))
			{
				WorldMarch march4 = (WorldMarch)objectTranslator.GetObject(L, 2, typeof(WorldMarch));
				WorldTroopPathSegment[] path4 = (WorldTroopPathSegment[])objectTranslator.GetObject(L, 3, typeof(WorldTroopPathSegment[]));
				int currPath4 = Lua.xlua_tointeger(L, 4);
				objectTranslator.Get(L, 5, out Vector3 val4);
				cityScene.UpdateTroopLine(march4, path4, currPath4, val4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.UpdateTroopLine!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTruckRoad(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			objectTranslator.Get(L, 3, out FindPathType v2);
			bool value = cityScene.IsTruckRoad(v, v2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCullingBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldCulling.ICullingObject cullingObject = (WorldCulling.ICullingObject)objectTranslator.GetObject(L, 2, typeof(WorldCulling.ICullingObject));
			cityScene.RemoveCullingBounds(cullingObject);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCullingBounds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			WorldCulling.ICullingObject cullingObject = (WorldCulling.ICullingObject)objectTranslator.GetObject(L, 2, typeof(WorldCulling.ICullingObject));
			cityScene.AddCullingBounds(cullingObject);
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
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.BattleFinish(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateBattleMessage(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject message = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			cityScene.UpdateBattleMessage(message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartPrintRoad(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			List<int> roads = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
			bool isOther = Lua.lua_toboolean(L, 3);
			cityScene.StartPrintRoad(roads, isOther);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StartPrintRoadByPathStr(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string paths = Lua.lua_tostring(L, 2);
				bool isOther = Lua.lua_toboolean(L, 3);
				float buildPerRoadTime = (float)Lua.lua_tonumber(L, 4);
				cityScene.StartPrintRoadByPathStr(paths, isOther, buildPerRoadTime);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				string paths2 = Lua.lua_tostring(L, 2);
				bool isOther2 = Lua.lua_toboolean(L, 3);
				cityScene.StartPrintRoadByPathStr(paths2, isOther2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.StartPrintRoadByPathStr!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FinishPrintRoad(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			obj.FinishPrintRoad(pointId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UICreateBoard(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int index = Lua.xlua_tointeger(L, 2);
				bool isAfter = Lua.lua_toboolean(L, 3);
				cityScene.UICreateBoard(index, isAfter);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int index2 = Lua.xlua_tointeger(L, 2);
				cityScene.UICreateBoard(index2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.UICreateBoard!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIHideBoard(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 3))
			{
				int deleteCount = Lua.xlua_tointeger(L, 2);
				bool isAfter = Lua.lua_toboolean(L, 3);
				cityScene.UIHideBoard(deleteCount, isAfter);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int deleteCount2 = Lua.xlua_tointeger(L, 2);
				cityScene.UIHideBoard(deleteCount2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.UIHideBoard!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyRoad(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<int> o = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).UIDestroyRoad();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UICreateBuilding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TTABLE))
			{
				int buildId = Lua.xlua_tointeger(L, 2);
				long buildUuid = Lua.lua_toint64(L, 3);
				int point = Lua.xlua_tointeger(L, 4);
				int buildTopType = Lua.xlua_tointeger(L, 5);
				LuaTable noBuildListStr = (LuaTable)objectTranslator.GetObject(L, 6, typeof(LuaTable));
				cityScene.UICreateBuilding(buildId, buildUuid, point, buildTopType, noBuildListStr);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int buildId2 = Lua.xlua_tointeger(L, 2);
				long buildUuid2 = Lua.lua_toint64(L, 3);
				int point2 = Lua.xlua_tointeger(L, 4);
				int buildTopType2 = Lua.xlua_tointeger(L, 5);
				cityScene.UICreateBuilding(buildId2, buildUuid2, point2, buildTopType2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.UICreateBuilding!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UICreateAllianceBuilding(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && (Lua.lua_isnil(L, 6) || Lua.lua_type(L, 6) == LuaTypes.LUA_TTABLE))
			{
				int buildId = Lua.xlua_tointeger(L, 2);
				long buildUuid = Lua.lua_toint64(L, 3);
				int point = Lua.xlua_tointeger(L, 4);
				int buildTopType = Lua.xlua_tointeger(L, 5);
				LuaTable noBuildListStr = (LuaTable)objectTranslator.GetObject(L, 6, typeof(LuaTable));
				cityScene.UICreateAllianceBuilding(buildId, buildUuid, point, buildTopType, noBuildListStr);
				return 0;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) || Lua.lua_isint64(L, 3)) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int buildId2 = Lua.xlua_tointeger(L, 2);
				long buildUuid2 = Lua.lua_toint64(L, 3);
				int point2 = Lua.xlua_tointeger(L, 4);
				int buildTopType2 = Lua.xlua_tointeger(L, 5);
				cityScene.UICreateAllianceBuilding(buildId2, buildUuid2, point2, buildTopType2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.UICreateAllianceBuilding!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIChangeBuilding(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.UIChangeBuilding(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyBuilding(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyBuilding();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIChangeRoad(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIChangeRoad();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIChangeAllianceBuilding(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			obj.UIChangeAllianceBuilding(index);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyAllianceBuilding(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyAllianceBuilding();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UICreateWorldMoveMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			string modelPath = Lua.lua_tostring(L, 2);
			long uuid = Lua.lua_toint64(L, 3);
			objectTranslator.Get(L, 4, out Vector3 val);
			cityScene.UICreateWorldMoveMarch(modelPath, uuid, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UICreateWorldTrigger(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string modelPath = Lua.lua_tostring(L, 2);
			int uuid = Lua.xlua_tointeger(L, 3);
			int pos = Lua.xlua_tointeger(L, 4);
			int skillId = Lua.xlua_tointeger(L, 5);
			int serverId = Lua.xlua_tointeger(L, 6);
			obj.UICreateWorldTrigger(modelPath, uuid, pos, skillId, serverId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyRreCreateMarch(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyRreCreateMarch();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyPreCreateTrigger(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyPreCreateTrigger();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UICreateWorldFlowerTrain(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string modelPath = Lua.lua_tostring(L, 2);
			int pointId = Lua.xlua_tointeger(L, 3);
			int goodsId = Lua.xlua_tointeger(L, 4);
			obj.UICreateWorldFlowerTrain(modelPath, pointId, goodsId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyPreCreateFlowerTrain(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyPreCreateFlowerTrain();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyRreCreateBuild(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyRreCreateBuild();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UIDestroyRreCreateAllianceBuild(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UIDestroyRreCreateAllianceBuild();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddObjectByPointId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int type = Lua.xlua_tointeger(L, 3);
			ModelManager.ModelObject o = obj.AddObjectByPointId(index, type);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjectByPointId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			ModelManager.ModelObject objectByPointId = obj.GetObjectByPointId(index);
			objectTranslator.Push(L, objectByPointId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateRoadRobot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			List<int> roads = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
			bool isOther = Lua.lua_toboolean(L, 3);
			long printId = Lua.lua_toint64(L, 4);
			cityScene.CreateRoadRobot(roads, isOther, printId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddToNeedRemoveList(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long bUuid = Lua.lua_toint64(L, 2);
			obj.AddToNeedRemoveList(bUuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRoadRobot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int roodPoint = Lua.xlua_tointeger(L, 2);
			WorldRoadRobot roadRobot = obj.GetRoadRobot(roodPoint);
			objectTranslator.Push(L, roadRobot);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeBuildRobotState(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long bUuid = Lua.lua_toint64(L, 2);
			objectTranslator.Get(L, 3, out WorldBuildRobot.State v);
			cityScene.ChangeBuildRobotState(bUuid, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeBuildRobotFinishTime(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long bUuid = Lua.lua_toint64(L, 2);
			float finishTime = (float)Lua.lua_tonumber(L, 3);
			obj.ChangeBuildRobotFinishTime(bUuid, finishTime);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateBuildRobot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 8 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 8))
			{
				long bUuid = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				float height = (float)Lua.lua_tonumber(L, 4);
				float duration = (float)Lua.lua_tonumber(L, 5);
				int tileSizeX = Lua.xlua_tointeger(L, 6);
				int tileSizeY = Lua.xlua_tointeger(L, 7);
				bool isTransit = Lua.lua_toboolean(L, 8);
				cityScene.CreateBuildRobot(bUuid, val, height, duration, tileSizeX, tileSizeY, isTransit);
				return 0;
			}
			if (num == 7 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) || Lua.lua_isint64(L, 2)) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7))
			{
				long bUuid2 = Lua.lua_toint64(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				float height2 = (float)Lua.lua_tonumber(L, 4);
				float duration2 = (float)Lua.lua_tonumber(L, 5);
				int tileSizeX2 = Lua.xlua_tointeger(L, 6);
				int tileSizeY2 = Lua.xlua_tointeger(L, 7);
				cityScene.CreateBuildRobot(bUuid2, val2, height2, duration2, tileSizeX2, tileSizeY2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.CreateBuildRobot!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateOtherBuildRobot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long bUuid = Lua.lua_toint64(L, 2);
			objectTranslator.Get(L, 3, out Vector3 val);
			float height = (float)Lua.lua_tonumber(L, 4);
			float duration = (float)Lua.lua_tonumber(L, 5);
			int tileSizeX = Lua.xlua_tointeger(L, 6);
			int tileSizeY = Lua.xlua_tointeger(L, 7);
			cityScene.CreateOtherBuildRobot(bUuid, val, height, duration, tileSizeX, tileSizeY);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildRobot(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			long bUuid = Lua.lua_toint64(L, 2);
			WorldBuildRobot buildRobot = obj.GetBuildRobot(bUuid);
			objectTranslator.Push(L, buildRobot);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateAnimalObject(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			obj.CreateAnimalObject(uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyAnimalObject(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long uuid = Lua.lua_toint64(L, 2);
			obj.DestroyAnimalObject(uuid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitFogOfWar(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			BitArray fogData = (BitArray)objectTranslator.GetObject(L, 2, typeof(BitArray));
			cityScene.InitFogOfWar(fogData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReInitFogOfWar(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReInitFogOfWar();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnlockFogOfWar(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogIndex = Lua.xlua_tointeger(L, 2);
			obj.UnlockFogOfWar(fogIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnlockFogOfWar2x2(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int unlockIndex = Lua.xlua_tointeger(L, 2);
			obj.UnlockFogOfWar2x2(unlockIndex);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetFogVisible(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool fogVisible = Lua.lua_toboolean(L, 2);
			obj.SetFogVisible(fogVisible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterFogCompleteAction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			Action callback = objectTranslator.GetDelegate<Action>(L, 2);
			cityScene.RegisterFogCompleteAction(callback);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ReInitObject(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ReInitObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearReInitObject(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearReInitObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCityTroop(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityTroop cityTroop = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetCityTroop();
			objectTranslator.Push(L, cityTroop);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFormationUuid(IntPtr L)
	{
		try
		{
			long formationUuid = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetFormationUuid();
			Lua.lua_pushint64(L, formationUuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadCityTroop(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				int createPos = Lua.xlua_tointeger(L, 2);
				int targetPos = Lua.xlua_tointeger(L, 3);
				cityScene.LoadCityTroop(createPos, targetPos);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int createPos2 = Lua.xlua_tointeger(L, 2);
				cityScene.LoadCityTroop(createPos2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CityScene.LoadCityTroop!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyCityTroop(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyCityTroop();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLodConfigs(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int lodType = Lua.xlua_tointeger(L, 2);
			Dictionary<string, LodConfig> lodConfigs = obj.GetLodConfigs(lodType);
			objectTranslator.Push(L, lodConfigs);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateCitySpaceMan(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CitySpaceMan o = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).CreateCitySpaceMan();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DestroyCitySpaceMan(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DestroyCitySpaceMan();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddLodAdjuster(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			AutoAdjustLod adjuster = (AutoAdjustLod)objectTranslator.GetObject(L, 2, typeof(AutoAdjustLod));
			cityScene.AddLodAdjuster(adjuster);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveLodAdjuster(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			AutoAdjustLod adjuster = (AutoAdjustLod)objectTranslator.GetObject(L, 2, typeof(AutoAdjustLod));
			cityScene.RemoveLodAdjuster(adjuster);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisibleByPointType(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointType = Lua.xlua_tointeger(L, 2);
			bool isVisible = Lua.lua_toboolean(L, 3);
			obj.SetVisibleByPointType(pointType, isVisible);
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
			Dictionary<int, int> specialPointDic = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetSpecialPointDic();
			objectTranslator.Push(L, specialPointDic);
			return 1;
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
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_SetCameraMaxHeight(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cameraMaxHeight = Lua.xlua_tointeger(L, 2);
			obj.SetCameraMaxHeight(cameraMaxHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCameraMinHeight(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int cameraMinHeight = Lua.xlua_tointeger(L, 2);
			obj.SetCameraMinHeight(cameraMinHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCameraRotRange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			bool overrideVal = Lua.lua_toboolean(L, 2);
			objectTranslator.Get(L, 3, out Vector2 val);
			cityScene.SetCameraRotRange(overrideVal, val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCameraMaxHeight(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetCameraMaxHeight();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCameraMinHeight(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetCameraMinHeight();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPreviousLodDistance(IntPtr L)
	{
		try
		{
			float previousLodDistance = ((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPreviousLodDistance();
			Lua.lua_pushnumber(L, previousLodDistance);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawBuildGrid(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			Mesh mesh = (Mesh)objectTranslator.GetObject(L, 2, typeof(Mesh));
			int submeshIndex = Lua.xlua_tointeger(L, 3);
			Material material = (Material)objectTranslator.GetObject(L, 4, typeof(Material));
			Matrix4x4[] matrices = (Matrix4x4[])objectTranslator.GetObject(L, 5, typeof(Matrix4x4[]));
			int count = Lua.xlua_tointeger(L, 6);
			cityScene.DrawBuildGrid(mesh, submeshIndex, material, matrices, count);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnterTimeline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			Camera camInTimeline = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
			float transitionTime = (float)Lua.lua_tonumber(L, 3);
			int value = cityScene.EnterTimeline(camInTimeline, transitionTime);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ExitTimeline(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int handle = Lua.xlua_tointeger(L, 2);
			bool value = obj.ExitTimeline(handle);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LockCamera(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			float duration = (float)Lua.lua_tonumber(L, 3);
			cityScene.LockCamera(val, duration);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FreeCamera(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).FreeCamera();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TestNetworkDisconnect(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TestNetworkDisconnect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZoneIdByPosId(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			int zoneIdByPosId = obj.GetZoneIdByPosId(pointId);
			Lua.xlua_pushinteger(L, zoneIdByPosId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZoneIdByWorldPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			int zoneIdByWorldPos = cityScene.GetZoneIdByWorldPos(val);
			Lua.xlua_pushinteger(L, zoneIdByWorldPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBloodyNightActivityRefresh(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			object serverId = objectTranslator.GetObject(L, 2, typeof(object));
			cityScene.OnBloodyNightActivityRefresh(serverId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsDawn(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int targetServerId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsDawn(targetServerId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBatteryPowerResourceUpdated(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			object obj = objectTranslator.GetObject(L, 2, typeof(object));
			cityScene.OnBatteryPowerResourceUpdated(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPveBattleExit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			object val = objectTranslator.GetObject(L, 2, typeof(object));
			cityScene.OnPveBattleExit(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPveBattleEnter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			object val = objectTranslator.GetObject(L, 2, typeof(object));
			cityScene.OnPveBattleEnter(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPointInAllianceCity(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			int serverId = Lua.xlua_tointeger(L, 3);
			bool value = obj.IsPointInAllianceCity(pointId, serverId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsPointInBlackArea(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsPointInBlackArea(pointId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowBlackArea(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			int tileWidth = Lua.xlua_tointeger(L, 3);
			int tileHeight = Lua.xlua_tointeger(L, 4);
			obj.ShowBlackArea(point, tileWidth, tileHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideBlackArea(IntPtr L)
	{
		try
		{
			CityScene obj = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int point = Lua.xlua_tointeger(L, 2);
			int tileWidth = Lua.xlua_tointeger(L, 3);
			int tileHeight = Lua.xlua_tointeger(L, 4);
			obj.HideBlackArea(point, tileWidth, tileHeight);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetZoneData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene obj = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			int zoneId = Lua.xlua_tointeger(L, 2);
			WorldZoneData zoneData = obj.GetZoneData(zoneId);
			objectTranslator.Push(L, zoneData);
			return 1;
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
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateBattleSoundData();
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
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			string name = Lua.lua_tostring(L, 2);
			object[] args = (object[])objectTranslator.GetObject(L, 3, typeof(object[]));
			string str = cityScene.Description(name, args);
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurSeasonType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SeasonType curSeasonType = ((CityScene)objectTranslator.FastGetCSObj(L, 1)).GetCurSeasonType();
			objectTranslator.Push(L, curSeasonType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BlockCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.BlockCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BlockSize(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.BlockSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TileSize(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityScene.TileSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TileCount(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.TileCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DynamicObjNode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.DynamicObjNode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BuildBubbleNode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.BuildBubbleNode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LodArray(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, CityScene.LodArray);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SceneInstanceGameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.SceneInstanceGameObject);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Transform(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.Transform);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTileCountXMin(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.CurTileCountXMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTileCountXMax(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.CurTileCountXMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTileCountYMin(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.CurTileCountYMin);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTileCountYMax(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.CurTileCountYMax);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldSize(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.WorldSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, cityScene.CurTarget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_InitZoom(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityScene.InitZoom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Zoom(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityScene.Zoom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CanMoving(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, cityScene.CanMoving);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DisableClampToEdge(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, cityScene.DisableClampToEdge);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TouchInputController(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.TouchInputController);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsFocus(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, cityScene.IsFocus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.CurTilePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurTilePosClamped(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.CurTilePosClamped);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Enabled(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, cityScene.Enabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameBufferWidth(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.frameBufferWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_frameBufferHeight(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.frameBufferHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curIndex(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, cityScene.curIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curTouchTile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.curTouchTile);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_curTouchPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, cityScene.curTouchPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_marchUuid(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, cityScene.marchUuid);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_touchPickablePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.touchPickablePos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SelectBuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushAny(L, cityScene.SelectBuild);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_preCreateBuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.preCreateBuild);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_placeFalseBuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.placeFalseBuild);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LodCameraDistanceScale(IntPtr L)
	{
		try
		{
			CityScene cityScene = (CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, cityScene.LodCameraDistanceScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mCityNightRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, cityScene.mCityNightRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_InitZoom(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitZoom = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Zoom(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Zoom = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_CanMoving(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CanMoving = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DisableClampToEdge(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisableClampToEdge = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Enabled(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Enabled = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curIndex(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).curIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curTouchTile(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			cityScene.curTouchTile = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_curTouchPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			cityScene.curTouchPoint = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_marchUuid(IntPtr L)
	{
		try
		{
			((CityScene)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).marchUuid = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_touchPickablePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CityScene)objectTranslator.FastGetCSObj(L, 1)).touchPickablePos = (List<int>)objectTranslator.GetObject(L, 2, typeof(List<int>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SelectBuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CityScene)objectTranslator.FastGetCSObj(L, 1)).SelectBuild = (ITouchPickable)objectTranslator.GetObject(L, 2, typeof(ITouchPickable));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_preCreateBuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CityScene)objectTranslator.FastGetCSObj(L, 1)).preCreateBuild = (FakeBuilding)objectTranslator.GetObject(L, 2, typeof(FakeBuilding));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_placeFalseBuild(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CityScene)objectTranslator.FastGetCSObj(L, 1)).placeFalseBuild = (Queue<FakeBuilding>)objectTranslator.GetObject(L, 2, typeof(Queue<FakeBuilding>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mCityNightRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CityScene)objectTranslator.FastGetCSObj(L, 1)).mCityNightRenderer = (CityNightColorRenderer)objectTranslator.GetObject(L, 2, typeof(CityNightColorRenderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_AfterUpdate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			CityScene cityScene = (CityScene)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					cityScene.AfterUpdate += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					cityScene.AfterUpdate -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to CityScene.AfterUpdate!");
		return 0;
	}
}
