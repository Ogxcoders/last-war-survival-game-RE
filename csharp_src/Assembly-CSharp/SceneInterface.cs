using System;
using System.Collections;
using System.Collections.Generic;
using BitBenderGames;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua;

public interface SceneInterface
{
	Transform DynamicObjNode { get; }

	Transform BuildBubbleNode { get; }

	GameObject SceneInstanceGameObject { get; }

	Vector2Int BlockCount { get; }

	int BlockSize { get; }

	float TileSize { get; }

	Vector2Int TileCount { get; }

	int CurTileCountXMin { get; }

	int CurTileCountXMax { get; }

	int CurTileCountYMin { get; }

	int CurTileCountYMax { get; }

	int WorldSize { get; }

	Vector3 CurTarget { get; }

	float InitZoom { get; }

	float Zoom { get; set; }

	bool CanMoving { get; set; }

	bool DisableClampToEdge { get; set; }

	float LodCameraDistanceScale { get; }

	TouchInputController TouchInputController { get; }

	bool IsFocus { get; }

	Vector2Int CurTilePos { get; }

	Vector2Int CurTilePosClamped { get; }

	bool Enabled { get; set; }

	int curIndex { get; set; }

	Vector2Int curTouchTile { get; set; }

	Vector3 curTouchPoint { get; set; }

	long marchUuid { get; set; }

	List<int> touchPickablePos { get; set; }

	ITouchPickable SelectBuild { get; set; }

	event Action AfterUpdate;

	void Init(GameObject go);

	void CreateScene(Action callback = null);

	void Uninit();

	void ChangeQualitySetting();

	void Update();

	void FixedUpdate();

	void SetWorldSize(int size);

	void SetWorldSize(int sizeX, int sizeY);

	void SetMapZoneActive(bool active);

	int GetBuildTileByItemId(int itemId);

	int GetAllianceCitySizeByItemId(int itemId);

	int GetTreasureSizeByItemId(int itemId);

	int GetDragonBuildSizeByItemId(int itemId);

	int GetBuildOffsetRangeByBuildId(int buildId);

	Vector3 TileToWorld(Vector2Int tilePos);

	Vector3 TileToWorld(int tilePosX, int tilePosY);

	Vector2Int WorldToTile(Vector3 worldPos);

	Vector3 SnapToTileCenter(Vector3 worldPos);

	Vector3 TileFloatToWorld(Vector2 tilePos);

	Vector3 TileFloatToWorld(float x, float y);

	Vector2 WorldToTileFloat(Vector3 worldPos);

	Vector2Int IndexToTilePos(int index);

	int TilePosToIndex(Vector2Int tilePos);

	Vector3 TileIndexToWorld(int index);

	Vector3 TileIndexToWorld(int index, int serverId);

	int WorldToTileIndex(Vector3 pos);

	float TileDistance(Vector2Int a, Vector2Int b);

	int GetIndexByOffset(int index, int x = 0, int y = 0);

	int GetIndexByOffsetByDirection(int index, int dir);

	void OnPlayerBankruptcyFinish(string uid);

	void ShowBattleBlood(object param, string path = "Assets/Main/Prefabs/UI/BattleWord/BattleNormalBloodTip.prefab");

	void RegisterPhysics(MonoBehaviour obj);

	void UnregisterPhysics(MonoBehaviour obj);

	void SetZoomParams(int level, float y, float offsetZ, float sensitivity);

	void SetCameraFOV(float fov);

	Vector2Int GetTouchTilePos();

	Vector3 GetTouchPoint();

	Vector3 GetTouchPoint(Vector3 screenPos);

	Vector3 GetRaycastGroundPoint(Vector3 screenPos);

	void AutoFocus(Vector3 lookat, LookAtFocusState state, float time, bool focusToCenter = true, bool lockView = false, Action onComplete = null);

	void QuitFocus(float time);

	float GetLodDistance();

	float GetLodDistanceByLod(int lod);

	Quaternion GetRotation();

	void Get_rotation(out float x, out float y, out float z, out float w);

	float GetPreviousLodDistance();

	float GetMapIconScale();

	Vector3 GetCameraPos();

	Ray ScreenPointToRay(Vector3 pos);

	float GetMinLodDistance();

	Vector3 WorldToScreenPoint(Vector3 worldPos);

	void AutoLookat(Vector3 lookat, float zoom = -1f, float time = 0.2f, Action onComplete = null);

	void AutoZoom(float zoom, float time = 0.2f, Action onComplete = null);

	void Lookat(Vector3 lookWorldPosition);

	Vector3 ScreenPointToWorld(Vector3 worldPos, float disPlane = 0f);

	int GetLodLevel();

	void UpdateViewRequest(bool isForce = false);

	void SetFirstViewRequestFlag(bool isFirstTime);

	void TrackMarch(long marchId);

	void TrackHSR(long marchId, GameObject go);

	void DisablePostProcess();

	void EnablePostProcess();

	void SetTouchInputControllerEnable(bool able);

	bool GetTouchInputControllerEnable();

	void StopCameraMove();

	int EnterTimeline(Camera camInTimeline, float transitionTime);

	bool ExitTimeline(int handle);

	void LockCamera(Vector3 pos, float duration);

	void FreeCamera();

	bool IsInMap(Vector2Int pt);

	bool IsInMapByIndex(int index);

	Vector2Int ClampTilePos(Vector2Int tilePos);

	void ChangeServer(int serverId);

	void OnChangeServerRemove();

	void RemoveBlackDesert();

	void InitBlackBlock();

	GameObject GetDragonLandRangeObj();

	void CreateDragonLandRange();

	void RemoveDragonLandRange();

	void RemoveDragonLandPoint(int pointIndex);

	int GetCollectResourceBuildRange();

	int GetCollectResourceTile();

	void CheckNeedRefreshRoad();

	List<Vector2Int> GetNearestPathForBuildingConnect(long uuid);

	ExplorePointInfo GetExplorePointInfoByIndex(int pointIndex);

	SamplePointInfo GetSamplePointInfoByIndex(int pointIndex);

	HeroDispatchMissionPointInfo GetHeroDispatchTaskPointInfoByIndex(int pointIndex);

	DetectRetryTaskPointInfo GetDetectRetryTaskPointInfo(int pointIndex);

	DetectAttackCityS0TaskPointInfo GetDetectAttackCityS0TaskPointInfo(int pointIndex);

	GhostreconPointInfo GetGhostreconPointInfoByIndex(int pointIndex);

	ResPointInfo GetResourcePointInfoByIndex(int pointIndex);

	bool IsCollectRangePoint(int pointIndex);

	PointInfo GetPointInfo(int pointIndex);

	PointInfo GetPointInfoWithServer(int pointIndex, int serverId);

	WorldTriggerData GetWorldTriggerData(long uuid);

	WorldTriggerData GetTriggerDataByPointId(int pointId, int serverId);

	WorldDesertInfo GetWorldDesertInfo(int pointIndex);

	WorldMarch GetMarchesByStartIndex(int pointIndex);

	WorldTileInfo GetWorldTileInfo(int pointIndex);

	PointInfo GetYellowLand(int pointIndex);

	CollectPointInfo GetCollectRangePoint(int pointIndex);

	int GetCollectPoint(int resourceType);

	List<int> GetAllCollectRangePoint(int resourceType);

	List<int> GetAllCollectRangePointType(int resourceType, int mainIndex);

	bool IsCollectPoint(int pointIndex);

	CollectPointInfo GetCollectInfoByIndex(int pointIndex);

	GarbagePointInfo GetGarbagePointInfoByIndex(int pointIndex);

	PointInfo GetPointInfoByUuid(long uuid);

	WorldDesertInfo GetDesertInfoByUuid(long uuid);

	PointInfo GetMyPointInfo();

	WorldPointObject GetObjectByPoint(int pointIndex);

	bool IsOutCityByPoint(int point);

	BuildPointInfo GetBaseMainByScreen();

	void ShowObject(int point);

	CityBuilding GetBuildingByPoint(int pointIndex);

	void SetLevelUpActive(int pointIndex, bool active);

	float GetBuildingHeight(int pointIndex);

	void HandleViewPointsReply(ISFSObject message);

	void HandleViewUpdateNotify(ISFSObject message);

	void HandleViewTileUpdateNotify(ISFSObject message);

	void HandleViewAssistanceInfoUpdateNotify(ISFSObject message);

	void HandleLandUpdate(ISFSObject message);

	void SendViewRequest(Vector2Int tilePos, int viewLevel, int serverId);

	bool IsNeedPlayPlacedAnim(long uuid);

	bool IsSelfRoad(int index);

	bool IsBuildFinish();

	WorldPointObject GetObjectByUuid(long uuid);

	CityBuilding GetBuildingByUuid(long uuid);

	WorldBuilding GetWorldBuildingByPoint(int pointIndex);

	WorldBuilding GetWorldBuildingByUuid(long uuid);

	bool HasPointInfo(int pointIndex);

	void AddToDeleteList(int index);

	BuildPointInfo GetBaseMainInfoByOwnerUid(string ownerUid);

	void HideObject(int point);

	bool IsSelfPoint(int pointIndex);

	int GetPointType(int index);

	bool IsSelfFreeBoard(int index);

	void RemoveObjectByPoint(int point);

	void RemoveOneObjectByPointType(int index, int pointType);

	void RefreshView();

	List<BuildPointInfo> GetMainByScreen();

	List<BuildPointInfo> GetAllMainBaseList();

	List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0);

	List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1);

	List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2);

	List<BuildPointInfo> GetAllMainBaseListByType(PlayerType t0, PlayerType t1, PlayerType t2, PlayerType t3);

	List<PointInfo> GetAllDragonPointList();

	List<int> GetAllDragonResourceList();

	Dictionary<int, int> GetSpecialPointDic();

	void OnMainBuildMove();

	List<int> GetGarbagePoint();

	WorldMarch GetMarch(long uuid);

	WorldMarch GetMonster(long targetPoint);

	bool IsTargetForMine(WorldMarch marchData);

	bool IsTargetForAlly(WorldMarch marchData);

	void StartMarch(int targetType, int targetPoint, long targetUuid, int timeIndex, long marchUuid = 0L, long formationUuid = 0L, int backHome = 1, byte[] sfsObjBinary = null, int startPos = 0, int targetServerId = -1);

	void RemoveFakeSampleMarchData(long index);

	void UpdateFakeSampleMarchDataWhenBack(long index, long startTime, long endTime);

	void UpdateFakeSampleMarchDataWhenStartPick(long index, long endTime);

	void AddFakeSampleMarchData(long startIndex, long endIndex, long startTime, long endTime, int marchTargetType);

	WorldMarch GetOwnerFormationMarch(string ownerUid, long formationUuid, string allianceUid = "");

	WorldMarch GetAllianceMarchesInTeam(string allianceUid, long teamUuid);

	List<WorldMarch> GetOwnerMarches(string ownerUid, string allianceUid = "");

	void HandlePushWorldTriggerUpdate(ISFSObject message, int serverId, int worldId);

	void HandlePushWorldTriggerDel(ISFSObject message, int serverId, int worldId);

	void HandlePushWorldMarchAdd(ISFSObject message);

	void HandlePushWorldMarchDel(ISFSObject message);

	void HandleWorldMarchGet(ISFSObject message);

	void HandleFormationMarch(ISFSObject message);

	void HandleFormationMarchChange(ISFSObject message);

	void HandlePushMultiKillUpdate(ISFSObject message);

	void HandlePushWolfStatusChange(ISFSObject message);

	bool ExistMarch(long uuid);

	bool IsInRallyMarch(long uuid);

	bool IsInCollectMarch(long uuid);

	bool IsInAssistanceMarch(long uuid);

	bool IsSelfInCurrentMarchTeam(long rallyMarchUuid);

	int GetMyAssistanceCount(int pointIndex);

	int GetMyAssistanceFirstHero(int pointIndex);

	void MarkPointIsDirty(long dirtyPointUuid);

	Dictionary<long, WorldMarch> GetMarchesBossInfo();

	void GetMonsterListInArea(Vector2Int center, int size, Dictionary<int, int> monsterIds, Dictionary<long, Vector2Int> result);

	void HandleUpdateLightData(ISFSObject message);

	void DestroyBerserkBossMarchData(long uuid);

	string SaveCreateMarchRecordTime();

	void RemoveCreateMarchRecordTime(WorldMarch marchInfo);

	bool CanUseInput();

	void SetUseInput(bool canUse);

	void SetSelectedPickable(ITouchPickable pickable);

	void DragSelectedPickable(Vector3 position);

	void ShowLoad(Vector3 pos);

	int GetClickWorldBulidingPos();

	void HideTouchEffect();

	long GetRaycastHitMarch(Vector3 screenPos);

	void SetDragFormationData(long uuid, int pointId);

	bool IsTileWalkable(Vector3 worldPos);

	void AddOccupyPoints(Vector2Int p, Vector2Int size, int serverId = 0);

	void RemoveOccupyPoints(Vector2Int p, Vector2Int size, int serverId = 0);

	void SetStaticVisibleChunk(int range);

	PlayerType IsMyEnemy(int serverId, string allianceId);

	void CleanAllianceCacheData();

	WorldTroop CreateGroupTroop(WorldMarch march);

	float GetModelHeight(long marchUuid);

	WorldTroop GetTroop(long marchUuid);

	EnumDestinationSignalType GetDestinationType(long marchUuid, long targetMarchUuid, int endPos, MarchTargetType targetType, bool isFormation, ref Vector3 realPos, ref int tileSize);

	MarchTargetType GetTargetType(long targetMarchUuid, int pointId);

	void CreateBattleVFX(string prefabPath, float life, Action<GameObject> onComplete);

	int CreateVFX(string prefabPath, Vector3 pos, float duration, float delay = 0f, Action<GameObject> onComplete = null);

	void RemoveVFX(int id);

	void OnTroopDragUpdate(long marchUuid, Vector3 dragPosCurrent, long targetMarchUuid, int startPointId = 0, bool isFormation = false);

	void OnTroopDragStop(long marchUuid, long targetMarchUuid, bool isFormation = false);

	int GetPointSize(int index);

	bool IsTroopCreate(long marchUuid);

	void CreateTroop(WorldMarch march);

	void UpdateTroop(WorldMarch march);

	float DestroyTroop(long marchUuid, bool isBattleFailed = false);

	void CreateTroopLine(WorldMarch march);

	void DestroyTroopLine(long marchUuid);

	void UpdateTroopLine(WorldMarch march, WorldTroopPathSegment[] path, int currPath, Vector3 currPos, int realTargetPos = 0, bool needRefresh = false, bool clear = false);

	bool IsTruckRoad(Vector2Int point, FindPathType findPathType);

	void RemoveCullingBounds(WorldCulling.ICullingObject cullingObject);

	void AddCullingBounds(WorldCulling.ICullingObject cullingObject);

	void BattleFinish(ISFSObject message);

	void UpdateBattleMessage(ISFSObject message);

	void StartPrintRoad(List<int> roads, bool isOther);

	void StartPrintRoadByPathStr(string paths, bool isOther, float buildPerRoadTime = 0f);

	void FinishPrintRoad(int pointId);

	List<int> UIDestroyRoad();

	void UICreateBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildListStr = null);

	void UICreateAllianceBuilding(int buildId, long buildUuid, int point, int buildTopType, LuaTable noBuildListStr = null);

	void UIChangeBuilding(int index);

	void UIDestroyBuilding();

	void UIChangeAllianceBuilding(int index);

	void UIDestroyAllianceBuilding();

	void UICreateWorldMoveMarch(string modelPath, long uuid, Vector3 pos);

	void UIDestroyRreCreateMarch();

	void UICreateWorldTrigger(string modelPath, int cfgId, int pointId, int skillId, int serverId);

	void UIDestroyPreCreateTrigger();

	void UICreateWorldFlowerTrain(string modelPath, int pointId, int goodsId);

	void UIDestroyPreCreateFlowerTrain();

	void UIChangeRoad();

	void UICreateBoard(int index, bool isAfter = true);

	void UIHideBoard(int deleteCount, bool isAfter = true);

	void UIDestroyRreCreateBuild();

	void UIDestroyRreCreateAllianceBuild();

	ModelManager.ModelObject AddObjectByPointId(int index, int type);

	ModelManager.ModelObject GetObjectByPointId(int index);

	bool IsRoadPoint(int index, string uid, int dir);

	void CreateRoadRobot(List<int> roads, bool isOther, long printId);

	void AddToNeedRemoveList(long bUuid);

	WorldRoadRobot GetRoadRobot(int roodPoint);

	void ChangeBuildRobotState(long bUuid, WorldBuildRobot.State state);

	void ChangeBuildRobotFinishTime(long bUuid, float finishTime);

	void CreateBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY, bool isTransit = false);

	void CreateOtherBuildRobot(long bUuid, Vector3 targetPos, float height, float duration, int tileSizeX, int tileSizeY);

	WorldBuildRobot GetBuildRobot(long bUuid);

	void CreateAnimalObject(long uuid);

	void DestroyAnimalObject(long uuid);

	void InitFogOfWar(BitArray fogData);

	void ReInitFogOfWar();

	void UnlockFogOfWar(int fogIndex);

	void UnlockFogOfWar2x2(int unlockIndex);

	void SetFogVisible(bool visible);

	void RegisterFogCompleteAction(Action callback);

	void ReInitObject();

	void ClearReInitObject();

	CityTroop GetCityTroop();

	long GetFormationUuid();

	void LoadCityTroop(int createPos, int targetPos = 0);

	void DestroyCityTroop();

	Dictionary<string, LodConfig> GetLodConfigs(int lodType);

	void AddLodAdjuster(AutoAdjustLod adjuster);

	void RemoveLodAdjuster(AutoAdjustLod adjuster);

	CitySpaceMan CreateCitySpaceMan();

	void SetVisibleByPointType(int pointType, bool isVisible);

	void SetCameraMinHeight(int height);

	void SetCameraRotRange(bool overrideValue, Vector2 rotRange);

	void UpdateTroopLineColor(string color);

	void SetCameraMaxHeight(int height);

	void ResetCameraMaxHeight();

	void ResetCameraMinHeight();

	void DrawBuildGrid(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count);

	GameObject GetPeopleById(int index);

	void OnSkinChange(bool ignoreCache);

	void TestNetworkDisconnect();

	int GetZoneIdByPosId(int pointId);

	int GetZoneIdByWorldPos(Vector3 worldPos);

	float PausePeopleAndPlayAnim(int index, string anim);

	Color GetLabelSkinColor(int skinId, int colorType);

	float GetLabelSkinOffset(int skinId);

	float GetLabelSkinSizeAdd(int skinId);

	void ResumePeople(int index);

	void UpdateBattleSoundData();

	void SetFocusPoint(int focusPoint);

	string Description(string name, object[] args);

	SeasonType GetCurSeasonType();
}
