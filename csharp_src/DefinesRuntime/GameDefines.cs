using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameDefines
{
	public enum BuildConnectRoadDirection
	{
		None,
		Top,
		Right,
		Left,
		Down
	}

	public enum DirectionType
	{
		Top = 1,
		Right,
		Left,
		Down
	}

	public class EntityAssets
	{
		public const string Building = "Assets/Main/Prefabs/Building/{0}.prefab";

		public const string AllianceBuilding = "Assets/Main/Prefabs/AllianceBuilding/{0}.prefab";

		public const string WorldRoadRobot = "Assets/Main/Prefabs/World/WorldRoadRobot.prefab";

		public const string WorldBuildingRobot = "Assets/Main/Prefabs/World/WorldBuildingRobot.prefab";

		public const string TouchTerrainEffect = "Assets/Main/Prefabs/World/TouchTerrainEffect.prefab";

		public const string WorldCityGrass = "Assets/Main/Prefabs/World/WorldCityGrass.prefab";

		public const string BatteryAttackRange = "Assets/_Art/Effect/prefab/scene/Build/V_paota_fanwei.prefab";

		public const string QuanEffectRange = "Assets/Main/Prefabs/BuildEffect/V_zdbx_quan.prefab";

		public const string LandLock = "Assets/_Art_LastWar/Models/Environment/Interactive/Massif/{0}";

		public const string BFLongPressEffect = "Assets/Main/Prefabs/Effect/BattleField/Eff_ui_longpress_signal.prefab";

		public const string World = "Assets/Main/Prefabs/World/Scene_World.prefab";

		public const string City = "Assets/Main/Prefabs/World/Scene_City.prefab";

		public const string Wasteland_City = "Assets/Main/Prefabs/World/Scene_City2.prefab";

		public const string Wasteland_City_Dig = "Assets/Main/Prefabs/World/Scene_City_Dig.prefab";

		public const string WorldSceneDesc = "Assets/Main/Scenes/WorldSceneDesc.bytes";

		public const string NewWorldSceneDesc = "Assets/Main/Scenes/NewWorldSceneDesc.bytes";

		public const string WorldBlockDesc = "Assets/Main/Scenes/WorldBlockDesc.bytes";

		public const string WorldSceneDecoration = "Assets/Main/Scenes/WorldDecoration/O_build_cangqiong_shu_3_1023.asset";

		public const string WorldDecorationBatchPath = "Assets/Main/Scenes/WorldDecoration/WorldDecoration.asset";

		public const string WorldSceneAllianceCityDesc = "Assets/Main/Scenes/WorldSceneAllianceCityDesc.bytes";

		public const string LandLockDesc = "Assets/Main/Scenes/LandLockDesc.bytes";

		public const string WorldMapZone = "Assets/Main/Scenes/Zone/zone.bytes";

		public const string WorldMapZoneS1 = "Assets/Main/SeasonRes/S1/Scenes/zone_S1.bytes";

		public const string WorldMapZoneS2 = "Assets/Main/SeasonRes/S2/Scenes/zone_S2.bytes";

		public const string WorldMapZoneS3 = "Assets/Main/SeasonRes/S3/Scenes/Zone/zone_S3.bytes";

		public const string WorldMapZoneS4 = "Assets/Main/SeasonRes/S4/Scenes/Zone/zone_S4.bytes";

		public const string WorldMapZoneS5 = "Assets/Main/SeasonRes/S5/Scenes/Zone/zone_S5.bytes";

		public const string WorldMapZonePosS0 = "Assets/Main/Scenes/Zone/pos.bytes";

		public const string WorldMapZonePosS1 = "Assets/Main/SeasonRes/S1/Scenes/pos_S1.bytes";

		public const string WorldMapZonePosS2 = "Assets/Main/SeasonRes/S2/Scenes/pos_S2.bytes";

		public const string WorldMapZonePosS3 = "Assets/Main/SeasonRes/S3/Scenes/Zone/pos_S3.bytes";

		public const string WorldMapZonePosS4 = "Assets/Main/SeasonRes/S4/Scenes/Zone/pos_S4.bytes";

		public const string WorldMapZonePosS5 = "Assets/Main/SeasonRes/S5/Scenes/Zone/pos_S5.bytes";

		public const string Terrain_World = "Assets/Main/Prefabs/World/Terrain_World.prefab";

		public const string TerrainSetting_Low = "Assets/Main/Prefabs/World/TerrainSetting_Low.asset";

		public const string TerrainSetting_High = "Assets/Main/Prefabs/World/TerrainSetting_High.asset";

		public const string Terrain_City_Low = "Assets/Main/Prefabs/World/Terrain_City.prefab";

		public const string Terrain_City_High = "Assets/Main/Prefabs/World/Terrain_City_High.prefab";

		public const string TerrainSetting_City_Low = "Assets/Main/Prefabs/World/TerrainSetting_City_Low.asset";

		public const string TerrainSetting_City_High = "Assets/Main/Prefabs/World/TerrainSetting_City_High.asset";

		public const string TroopLine = "Assets/Main/Prefabs/March/TroopLine.prefab";

		public const string TroopLineWithTimer = "Assets/Main/Prefabs/March/TroopLineWithTimer.prefab";

		public const string TroopDestinationSignal = "Assets/Main/Prefabs/March/TroopDestinationSignal.prefab";

		public const string TroopLineDrag = "Assets/Main/Prefabs/March/TroopLineDrag.prefab";

		public const string WorldTroop = "Assets/Main/Prefabs/March/WorldTroop.prefab";

		public const string WorldTroopHSR = "Assets/Main/SeasonRes/S5/Prefabs/World/WorldTroopHSR.prefab";

		public const string WorldTroopTrain = "Assets/Main/Prefabs/March/WorldTroopTrain.prefab";

		public const string WorldTroopTruck = "Assets/Main/Prefabs/March/WorldTroopTruck.prefab";

		public const string WorldTroopFlowerTrain = "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/WorldTroopFlowerTrain.prefab";

		public const string MonsterActBoss = "Assets/Main/Prefabs/Monsters/MonsterActBoss.prefab";

		public const string ZombieBusTrain = "Assets/Main/Prefabs/March/WorldTroopZombieBusTrain.prefab";

		public const string WorldTroopAlliance = "Assets/Main/Prefabs/March/WorldTroopAlliance.prefab";

		public const string WorldTroopOther = "Assets/Main/Prefabs/March/WorldTroopOther.prefab";

		public const string WorldTroopOtherYbc = "Assets/Main/Prefabs/March/WorldTroopOtherYbc.prefab";

		public const string WorldVirtualTroop = "Assets/Main/Prefabs/March/WorldVirtualTroop.prefab";

		public const string ScoutTroop = "Assets/Main/Prefabs/March/WorldTroopScout.prefab";

		public const string donateTroop = "Assets/Main/Prefabs/March/WorldTroopMobilizationDonate.prefab";

		public const string ResTransTroop = "Assets/Main/Prefabs/March/WorldTroop.prefab";

		public const string GolloesExploreTroop = "Assets/Main/Prefabs/March/GolloesExploreTroop.prefab";

		public const string GolloesTradeTroop = "Assets/Main/Prefabs/March/GolloesTradeTroop.prefab";

		public const string WorldRallyTroop = "Assets/Main/Prefabs/March/WorldTroop.prefab";

		public const string FieldMonster = "Assets/Main/Prefabs/Monsters/FieldMonster.prefab";

		public const string FieldBoss = "Assets/Main/Prefabs/Monsters/FieldBoss.prefab";

		public const string ConstructMaterial = "Assets/Main/Material/building_construct.mat";

		public const string Road_In_City = "Assets/Main/Prefabs/Road/Road_In_City_{0}.prefab";

		public const string Road_Out_City = "Assets/Main/Prefabs/Road/Road_Out_City_{0}.prefab";

		public const string Road_In_City_Updating = "Assets/Main/Prefabs/Road/Road_In_City_Updating_{0}.prefab";

		public const string Road_Out_City_Updating = "Assets/Main/Prefabs/Road/Road_Out_City_Updating_{0}.prefab";

		public const string Road_Self = "Assets/Main/Prefabs/Road/Road_Self_{0}.prefab";

		public const string Road_Self_Updating = "Assets/Main/Prefabs/Road/Road_Self_Updating_{0}.prefab";

		public const string Road_Fake = "Assets/Main/Prefabs/Road/Road_Fake_{0}.prefab";

		public const string TileUnlocked = "Assets/Main/Prefabs/World/TileUnlocked.prefab";

		public const string TileLocked = "Assets/Main/Prefabs/World/TileLocked.prefab";

		public const string BuildGrid = "Assets/Main/Prefabs/Building/BuildGrid{0}.prefab";

		public const string RoadBlockGreen = "Assets/Main/Prefabs/Road/RoadBlockGreen2.prefab";

		public const string RoadBlockRed = "Assets/Main/Prefabs/Road/RoadBlockRed2.prefab";

		public const string RoadGrid = "Assets/Main/Prefabs/Road/RoadGrid.prefab";

		public const string BuildSelect = "Assets/Main/Prefabs/Building/BuildSelect{0}.prefab";

		public const string RoadLightInCity = "Assets/Main/Prefabs/Road/RoadLightInCity{0}.prefab";

		public const string RoadLightOutCity = "Assets/Main/Prefabs/Road/RoadLightOutCity{0}.prefab";

		public const string RoadLightSelf = "Assets/Main/Prefabs/Road/RoadLightSelf{0}.prefab";

		public const string MonsterPath = "Assets/Main/Prefabs/Monsters/{0}.prefab";

		public const string BuildBlock = "Assets/Main/Prefabs/Building/BuildBlock.prefab";

		public const string WorldTroopSoldier = "Assets/Main/Prefabs/March/WorldTroopSoldier.prefab";

		public const string WorldTroopTank = "Assets/Main/Prefabs/March/WorldTroopTank.prefab";

		public const string WorldTroopPlane = "Assets/Main/Prefabs/March/WorldTroopPlane.prefab";

		public const string WorldTroopJunkman = "Assets/Main/Prefabs/March/WorldTroopJunkman.prefab";

		public const string CollectAnimalModel = "Assets/Main/Prefabs/CollectResource/CollectAnimalModel.prefab";

		public const string CollectArmyAnimalModel = "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModel.prefab";

		public const string CollectArmyAnimalModelAlliance = "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModelAlliance.prefab";

		public const string CollectArmyAnimalModelEnemy = "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModelEnemy.prefab";

		public const string BuildMetalFew = "Assets/Main/Prefabs/Building/BuildMetalFew.prefab";

		public const string BuildMetalMiddle = "Assets/Main/Prefabs/Building/BuildMetalMiddle.prefab";

		public const string BuildMetalMax = "Assets/Main/Prefabs/Building/BuildMetalMax.prefab";

		public const string BuildWoodFew = "Assets/Main/Prefabs/Building/BuildWoodFew.prefab";

		public const string BuildWoodMiddle = "Assets/Main/Prefabs/Building/BuildWoodMiddle.prefab";

		public const string BuildWoodMax = "Assets/Main/Prefabs/Building/BuildWoodMax.prefab";

		public const string DetectEventUI = "Assets/Main/Prefabs/March/WorldDetectInfo.prefab";

		public const string DetectEventFakePlayerUI = "Assets/Main/Prefabs/March/WorldFakePlayerDetectInfo.prefab";

		public const string WorldBaseHead = "Assets/Main/Prefabs/Building/WorldBaseHead.prefab";

		public const string CityGarbagePath = "Assets/Main/Prefabs/Garbage/{0}.prefab";

		public const string CityTroop = "Assets/Main/Prefabs/March/CityTroop.prefab";

		public const string CollectGarbageUI = "Assets/Main/Prefabs/March/CollectGarbageUI.prefab";

		public const string DispatchTaskCdUI = "Assets/Main/Prefabs/DispatchTask/dispatchTaskCdUI.prefab";

		public const string DispatchTaskOpenUI = "Assets/Main/Prefabs/DispatchTask/dispatchTaskOpenUI.prefab";

		public const string DispatchTaskRewardUI = "Assets/Main/Prefabs/DispatchTask/dispatchTaskRewardUI.prefab";

		public const string GhostreconRewardUI = "Assets/Main/Prefabs/DispatchTask/ghostreconRewardUI.prefab";

		public const string CityWorkMan = "Assets/Main/Prefabs/CityScene/CityWorkMan.prefab";

		public const string FogPath = "Assets/Main/Prefabs/FogOfWar/{0}.prefab";

		public const string CityCameraSand = "Assets/Main/Prefabs/CityScene/CityCameraSand.prefab";

		public const string WorldCityTreeHigh = "Assets/Main/Prefabs/World/WorldCityTreeHigh{0}.prefab";

		public const string WorldCityTree = "Assets/Main/Prefabs/World/WorldCityTree{0}.prefab";

		public const string FocusCurve = "Assets/Main/Prefabs/CityScene/FocusCurve{0}.prefab";

		public const string CitySpaceMan = "Assets/Main/Prefabs/CityScene/CitySpaceMan.prefab";

		public const string GarbageStone = "Assets/_Art/Models/Soldier/ShiHuangXiaoRen/prefab/A_soldie_shxr_rock_1.prefab";

		public const string GarbageCrystal = "Assets/_Art/Models/Soldier/ShiHuangXiaoRen/prefab/A_soldie_shxr_tuohuang_crystal_1.prefab";

		public const string CollectBuildModelSelf = "Assets/Main/Prefabs/Building/building_collect.prefab";

		public const string CollectBuildModelAlliance = "Assets/Main/Prefabs/Building/building_collect_alliance.prefab";

		public const string CollectBuildModelEnemy = "Assets/Main/Prefabs/Building/building_collect_enemy.prefab";

		public const string LandLockFadeOut = "Assets/Main/Prefabs/World/LandLockFadeOut.prefab";

		public const string WorldCloud1 = "Assets/Main/Prefabs/World/Eff_daditu_yun_01.prefab";

		public const string WorldCloud2 = "Assets/Main/Prefabs/World/Eff_daditu_yun_02.prefab";

		public const string WorldCloud3 = "Assets/Main/Prefabs/World/Eff_daditu_yun_03.prefab";

		public const string WorldCloud4 = "Assets/Main/Prefabs/World/Eff_daditu_yun_04.prefab";

		public const string DetectEventBarricade = "Assets/Main/Prefabs/World/DetectEvent_barricade.prefab";

		public const string MeteoriteBattleActorFragmentAsset = "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentSmall.prefab";

		public const string MeteoriteBattlePrepareFragmentAsset = "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentMiddle.prefab";

		public const string MeteoriteBattleBigFragmentAsset = "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldDropDaddy.prefab";

		public const string MeteoriteBattleFragmentDropWarningAsset = "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteSonDropWarning.prefab";

		public const string MeteoriteBattleDropEffAsset = "Assets/Main/Prefabs/Effect/World/Meteorite/WorldMeteoriteAbandonEffect.prefab";

		public const string MeteoriteBattleCameraEffAsset = "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldCameraEff.prefab";
	}

	public class UIAssets
	{
		public const string SubLoadingNormal = "Assets/Main/Loading/Prefabs/UILoading_Normal.prefab";

		public const string SubLoadingAmerica = "Assets/Main/Loading/Prefabs/UILoading_America.prefab";

		public const string SubLoadingArabic = "Assets/Main/Loading/Prefabs/UILoading_Arabic.prefab";

		public const string SubLoadingChineseSimplified = "Assets/Main/Loading/Prefabs/UILoading_ChineseSimplified.prefab";

		public const string SubLoadingKorean = "Assets/Main/Loading/Prefabs/UILoading_Korea.prefab";

		public const string SubLoadingJapanese = "Assets/Main/Loading/Prefabs/UILoading_Japan.prefab";

		public const string SubLoadingThai = "Assets/Main/Loading/Prefabs/UILoading_Thai.prefab";

		public const string SubLoadingIndo = "Assets/Main/Loading/Prefabs/UILoading_Indo.prefab";

		public const string SubLoadingFrench = "Assets/Main/Loading/Prefabs/UILoading_French.prefab";

		public const string SubLoadingGerman = "Assets/Main/Loading/Prefabs/UILoading_German.prefab";

		public const string SubLoadingBrazil = "Assets/Main/Loading/Prefabs/UILoading_Brazil.prefab";

		public const string SubLoadingSeasonLondon = "Assets/Main/Loading/Prefabs/UILoading_SeasonLondon.prefab";

		public const string SubLoadingSeasonSnow = "Assets/Main/Loading/Prefabs/UILoading_SeasonSnow.prefab";

		public const string SubLoadingSeasonMummy = "Assets/Main/Loading/Prefabs/UILoading_SeasonMummy.prefab";

		public const string SubLoadingSeasonDark = "Assets/Main/Loading/Prefabs/UILoading_SeasonDark.prefab";

		public const string SubLoadingSeasonNineNation = "Assets/Main/Loading/Prefabs/UILoading_SeasonNineNation.prefab";

		public const string UILoading = "Assets/Main/Loading/Prefabs/UILoading_Base.prefab";

		public const string UIPrivacy = "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacy.prefab";

		public const string UIPrivacyKR = "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacyKR.prefab";

		public const string UICoppaView = "Assets/Main/Prefabs/UI/LWUIPrivacy/UIPrivacyCoppa.prefab";

		public const string UIZendesk = "Assets/Main/Prefabs/UI/UIZendesk/UIZendesk.prefab";

		public const string ProfileGraphy = "Assets/Main/Prefabs/Debug/Graphy.prefab";

		public const string GFXConsole = "Assets/Main/Prefabs/Debug/GFXConsole.prefab";

		public const string UIChooseLocalUpdate = "Assets/Main/Prefabs/Debug/UIChooseLocalUpdate.prefab";

		public const string WorldCityTruck1 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck01.prefab";

		public const string WorldCityTruck2 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck02.prefab";

		public const string WorldCityTruck3 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck03.prefab";

		public const string WorldCityTruck4 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck04.prefab";

		public const string WorldCityTruck5 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck05.prefab";

		public const string WorldCityTruck6 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck06.prefab";

		public const string WorldCityTruck7 = "Assets/Main/Prefabs/Vehicle/WorldCityTruck07.prefab";

		public const string WorldCityPeople1 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople01.prefab";

		public const string WorldCityPeople2 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople02.prefab";

		public const string WorldCityPeople3 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople03.prefab";

		public const string WorldCityPeople4 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople04.prefab";

		public const string WorldCityPeople5 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople05.prefab";

		public const string WorldCityPeople6 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople06.prefab";

		public const string WorldCityPeople7 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople07.prefab";

		public const string WorldCityPeople8 = "Assets/Main/Prefabs/Vehicle/WorldCityPeople08.prefab";

		public const string UIPartsMaterialInfo = "Assets/Main/Prefabs/UI/Parts/UIPartsMaterialInfo.prefab";

		public const string UITokenShop = "Assets/Main/Prefabs/UI/Shop/UITimeLimitShop.prefab";

		public const string UIMultipleShop = "Assets/Main/Prefabs/UI/MilitaryInformation/UIMultipleShop.prefab";

		public const string SceneRocketFireEffect = "Assets/Main/Prefabs/RocketEffect/SceneRocketFireEffect.prefab";

		public const string SceneRocketSmokeEffect = "Assets/Main/Prefabs/RocketEffect/SceneRocketSmokeEffect.prefab";

		public const string UIMultiKill = "Assets/Main/Prefabs/UI/MultiKill/UIMultiKill.prefab";

		public const string WorldAssistanceLabelPlayer = "Assets/Main/Prefabs/MainCity/WorldAssistanceLabelPlayer.prefab";

		public const string WorldAssistanceLabelAllianceBuilding = "Assets/Main/Prefabs/MainCity/WorldAssistanceLabelAllianceBuilding.prefab";

		public const string WorldCityFireEffectAsync = "Assets/Main/Prefabs/MainCity/Eff_ui_zhushou_build_fire.prefab";

		public const string WorldCityMummyFireEffectAsync = "Assets/Main/SeasonRes/Shared/Prefabs/World/Eff_ui_zhushou_build_fire_blue.prefab";

		public const string WorldAssistanceHeroPlayer = "Assets/Main/Prefabs/MainCity/WorldAssistanceHeroPlayer.prefab";

		public const string WorldAssistanceHeroAllianceBuilding = "Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceBuilding.prefab";

		public const string WorldAssistanceHeroAllianceTrade = "Assets/Main/Prefabs/MainCity/WorldAssistanceHeroAllianceTrade.prefab";
	}

	public class SettingKeys
	{
		public const string ACCOUNT_LIST_DEBUG = "ACCOUNT_LIST_DEBUG";

		public const string LAST_SERVER_KEY = "DEBUG_LAST_SERVERID";

		public const string GAME_UID = "Setting.GAME_UID";

		public const string GM_FLAG = "Setting.GM_FLAG";

		public const string UUID = "Setting.UUID";

		public const string DEVICE_ID = "DEVICE_ID";

		public const string SERVER_IP = "SERVER_IP";

		public const string SERVER_PORT = "SERVER_PORT";

		public const string SERVER_ZONE = "SERVER_ZONE";

		public const string SERVER_CONNECTION_TYPE = "SERVER_CONNECTION_TYPE";

		public const string ACCESS_TOKEN = "Login.access_token";

		public const string ACCESS_TOKEN_TIME = "Login.access_token_time";

		public const string REFRESH_TOKEN = "Login.refresh_token";

		public const string REFRESH_TOKEN_TIME = "Login.refresh_token_time";

		public const string LOGIN_KEY = "Login.login_key";

		public const string COK_PURCHASE_SUCCESSED_KEY = "Setting.COK_PURCHASE_SUCCESSED_KEY";

		public const string COK_PURCHASE_KEY = "Setting.COK_PURCHASE_KEY";

		public const string CATCH_ITEM_ID = "Setting.CATCH_ITEM_ID";

		public const string EFFECT_MUSIC_ON = "isEffectMusicOn";

		public const string BG_MUSIC_ON = "isBGMusicOn";

		public const string ENV_SOUND_ON = "ENV_SOUND_ON";

		public const string ThreeDWORLD_SWITCH = "3dworld_switch";

		public const string SHOW_FAVORITE = "show_favorite";

		public const string TASK_TIPS_ON = "isTaskTipsOn";

		public const string WORLD_SCROLL_UI = "world_scroll_gameui";

		public const string HIDE_BASE_TEMPERATURE = "HIDE_BASE_TEMPERATURE";

		public const string TOUCH_SP_FUN = "touch_sp_fun";

		public const string COORDINATE_ON_SHOW = "COORDINATE_ON_SHOW";

		public const string Transporter_Hidden = "transporter_hidden";

		public const string ISETTING_CASTLE_CLICK_PRIORITY = "ISetting_CastleClickPriority";

		public const string USER_LANGUAGE = "Setting.USER_LANGUAGE";

		public const string RECHARGE_ACTV_TOMORROW_TIME = "recharge.actv.tomorrow.time.mark";

		public const string GUIDE_STEP = "guideStep";

		public const string GUIDE_MP4 = "guideMp4";

		public const string GUIDE_FOR_TERRITORY = "Setting.Armygroup_Territory6";

		public const string GUIDE_FOR_MARCH = "Setting.Armygroup_Over6";

		public const string POST_PROCESSING_BLOOM = "POST_PROCESSING_BLOOM";

		public const string POST_PROCESSING_VIGNETTE = "POST_PROCESSING_VIGNETTE";

		public const string SCENE_PARTICLES = "SCENE_PARTICLES";

		public const string RESOURCE_LOGGER = "Setting.Resource.Logger";

		public const string SCENE_GRAPHIC_LEVEL = "SCENE_GRAPHIC_LEVEL";

		public const string SCENE_FPS_LEVEL = "SCENE_FPS_LEVEL";

		public const string SHOW_DEBUG_CHOOSE_SERVER = "SHOW_DEBUG_CHOOSE_SERVER";

		public const string CITY_TROOP_POSITION = "CITY_TROOP_POSITION";

		public const string MAIL_LAST_OPEN_TIME_BY_GROUP = "MAIL_LAST_OPEN_TIME_BY_GROUP_";

		public const string ALLIANCE_WAR_OLD_DATA = "ALLIANCE_WAR_OLD_DATA";

		public const string ARABIC_AUTO_MIRROR_SWITCH = "ARABIC_AUTO_MIRROR_SWITCH";

		public const string SERVER_COUNTRY = "SERVER_COUNTRY";

		public const string SEASON_MAP_TYPE = "SEASON_MAP_TYPE";

		public const string SEASON_START_TIME = "SeasonStartTime";

		public const string SEASON_SETTLE_TIME = "SeasonSettleTime";

		public const string SEASON_END_TIME = "SeasonEndTime";

		public const string SEASON_LOADING_BGM = "SeasonBGM";

		public const string USE_SEASON_BGM = "USE_SEASON_BGM";

		public const string DEBUG_CHOOSE_URL_GROUP = "DEBUG_CHOOSE_URL_GROUP_NEW";

		public const string IS_CHANGE_DEBUG_CHOOSE_URL_GROUP = "IS_CHANGE_DEBUG_CHOOSE_URL_GROUP";

		public const string RELOAD_DEBUG_SHOW_SERVER_LIST = "RELOAD_DEBUG_SHOW_SERVER_LIST";

		public const string SHUMEI_SDK_IS_FUNCTION_OPEN = "SHUMEI_SDK_IS_FUNCTION_OPEN";

		public const string UNPACK_RESOURCE_DOWNLOAD_RECORD = "UNPACK_RESOURCE_DOWNLOAD_RECORD";

		public const string UNPACK_RESOURCE_DOWNLOAD_PAUSE_RECORD = "UNPACK_RESOURCE_DOWNLOAD_PAUSE_RECORD";

		public const string LOADING_NAME_FIRST = "LOADING_NAME_FIRST";

		public const string PC_DOWNLOAD_SETUP_NAME = "PC_DOWNLOAD_SETUP_NAME";

		public const string PC_DOWNLOAD_CLICK_KEY = "PC_DOWNLOAD_CLICK_KEY";

		public const string EFFECT_VOLUME = "EFFECT_VOLUME";

		public const string MUSIC_VOLUME = "MUSIC_VOLUME";

		public const string FULL_SCREEN_ON = "FULL_SCREEN_ON";

		public const string IS_FIRST_LOGIN = "IS_FIRST_LOGIN";

		public const string ENV_SOUND_VOLUME = "ENV_SOUND_VOLUME";

		public const string LOADING_DEFAULT_BGM = "LOADING_DEFAULT_BGM";
	}

	public class SoundAssets
	{
		public const string Music_M_city_1 = "m_city";

		public const string Music_M_city_3 = "m_field";

		public const string Music_Sfx_logo_loading = "sfx_logo_loading";

		public const string Music_M_battle_1 = "m_city";

		public const string Video_bg_1 = "vedio_bg_1";

		public const string Music_Bgm_city_night = "bgm_base_night";

		public const string Music_Bgm_city_night_01 = "bgm_base_night_01";

		public const string Music_Bgm_city_day = "bgm_base_day";

		public const string Music_Bgm_city_day_01 = "Assets/Main/Sound/Music/bgm_base_day_01.ogg";

		public const string Music_Bgm_city_day_02 = "bgm_base_day_02";

		public const string Music_Bgm_pve = "Bgm_Movie_Battle1";

		public const string Music_Bgm_parkour_battle = "bgm_pve_30034";

		public const string Music_Effect_Open = "effect_open";

		public const string Music_Effect_Close = "effect_close";

		public const string Music_Effect_Ground = "effect_ground";

		public const string Music_Effect_Mist = "effect_mist";

		public const string Music_Effect_Building = "effect_building";

		public const string Music_Effect_Farm = "effect_farm";

		public const string Music_Effect_Ranch = "effect_ranch";

		public const string Music_Effect_Creeps = "effect_creeps";

		public const string Music_Effect_Army = "effect_army";

		public const string Music_Effect_Electric = "effect_electric";

		public const string Music_Effect_Crystal = "effect_crystal";

		public const string Music_Effect_Water = "effect_water";

		public const string Music_Effect_Gas = "effect_gas";

		public const string Music_Effect_Coin = "effect_coin";

		public const string Music_Effect_Product1 = "effect_product1";

		public const string Music_Effect_Product2 = "effect_product2";

		public const string Music_Effect_Product3 = "effect_product3";

		public const string Music_Effect_Trained = "effect_trained";

		public const string Music_Effect_Plant = "effect_plant";

		public const string Music_Effect_Feed = "effect_feed";

		public const string Music_Effect_Produce_Put = "effect_produce_put";

		public const string Music_Effect_Produce_Box = "effect_produce_box";

		public const string Music_Effect_Bill = "effect_bill";

		public const string Music_Effect_Button = "effect_button";

		public const string Music_Effect_Road = "effect_road";

		public const string Music_Effect_Message = "effect_message";

		public const string Music_Effect_Finish = "effect_finished";

		public const string Music_Effect_Rocket = "effect_rocket";

		public const string Music_Effect_Rocket_Land = "effect_rocket_land";

		public const string Music_Effect_Radar = "effect_radar";

		public const string Music_Effect_Alliance = "effect_alliance";

		public const string Music_Effect_Attack = "effect_attack";

		public const string Music_Effect_Skill_Attack = "effect_skill";

		public const string Music_Invasion_Aisilla_Born = "aisila_born";

		public const string Music_Invasion_Aisilla_Dead = "aisila_dead";

		public const string Music_Invasion_Aisilla_Skill_Down = "aisila_skill_down_1";

		public const string Music_Invasion_Aisilla_Skill_Up = "aisila_skill_up";

		public const string MUSIC_NEW_CHALLENGE_BOX_BORN = "Gameplay/challenge_zombie/Baoxiang/SFX_Env_Baoxiang_World_Open";

		public const string DOMINATOR_COCKATRICE_TIMELINE = "Dominator/SFX_Env_Xunzhaohuoban_TL";
	}

	public class BuildingTypes
	{
		public const int FUN_BUILD_MAIN = 10100000;

		public const int FUN_BUILD_BUSINESS_CENTER = 401000;

		public const int FUN_BUILD_STABLE = 402000;

		public const int FUN_BUILD_SCIENE = 403000;

		public const int FUN_BUILD_SMITHY = 407000;

		public const int FUN_BUILD_CONDOMINIUM = 409000;

		public const int FUN_BUILD_HOSPITAL = 411000;

		public const int FUN_BUILD_STONE = 412000;

		public const int FUN_BUILD_OIL = 413000;

		public const int FUN_BUILD_ARROW_TOWER = 418000;

		public const int FUN_BUILD_CAR_BARRACK = 423000;

		public const int FUN_BUILD_INFANTRY_BARRACK = 424000;

		public const int FUN_BUILD_AIRCRAFT_BARRACK = 425000;

		public const int FUN_BUILD_TRAINFIELD_1 = 427000;

		public const int FUN_BUILD_TRAINFIELD_2 = 793000;

		public const int FUN_BUILD_TRAINFIELD_3 = 794000;

		public const int FUN_BUILD_TRAINFIELD_4 = 795000;

		public const int FUN_BUILD_WATER = 432000;

		public const int FUN_BUILD_MARKET = 435000;

		public const int FUN_BUILD_ROAD = 436000;

		public const int FUN_BUILD_ELECTRICITY_STORAGE = 437000;

		public const int FUN_BUILD_WATER_STORAGE = 438000;

		public const int FUN_BUILD_OIL_STORAGE = 439000;

		public const int FUN_BUILD_IRON_STORAGE = 441000;

		public const int FUN_BUILD_WIND_TURBINE = 444000;

		public const int FUN_BUILD_SOLAR_POWER_STATION = 447000;

		public const int FUN_BUILD_DRONE = 477000;

		public const int FUN_BUILD_VILLA = 700000;

		public const int APS_BUILD_FARM = 701000;

		public const int APS_BUILD_FARM_FIELD = 702000;

		public const int APS_BUILD_PASTURE = 703000;

		public const int APS_BUILD_PASTURE_FIELD = 704000;

		public const int FUN_BUILD_OXYGEN = 705000;

		public const int FUN_BUILD_METALLURGY = 706000;

		public const int FUN_BUILD_FOOD = 707000;

		public const int FUN_BUILD_OIL_REFINERY = 708000;

		public const int FUN_BUILD_INTEGRATED_FACTORY = 709000;

		public const int FUN_BUILD_TRADING_CENTER = 710000;

		public const int FUN_BUILD_FOODSHOP = 711000;

		public const int FUN_BUILD_PRINT_FACTORY = 712000;

		public const int FUN_BUILD_INFORMATION_CENTER = 713000;

		public const int FUN_BUILD_COLD_STORAGE = 714000;

		public const int FUN_BUILD_COMPREHENSIVE_STORAGE = 715000;

		public const int FUN_BUILD_DEFENCE_CENTER = 716000;

		public const int FUN_BUILD_DOME = 449000;

		public const int FUN_BUILD_FORGE = 429000;

		public const int FUN_BUILD_ELECTRICITY = 431000;

		public const int FUN_BUILD_RECHARGE_GARAGE = 445000;

		public const int FUN_BUILD_HONOR_HALL = 446000;

		public const int FUN_BUILD_BUILDING_CENTER = 448000;

		public const int FUN_BUILD_OFFICER = 483000;

		public const int APS_BUILD_PASTURE_OSTRICH = 719000;

		public const int APS_BUILD_PASTURE_CATTLE = 720000;

		public const int APS_BUILD_PASTURE_SANDWORM = 721000;

		public const int APS_BUILD_WORMHOLE_MAIN = 791000;

		public const int APS_BUILD_WORMHOLE_SUB = 792000;

		public const int WORM_HOLE_CROSS = 735000;

		public const int FUN_BUILD_RADAR_CENTER = 417000;

		public const int FUN_BUILD_TEMP_WIND_POWER_PLANT = 796000;

		public const int FUN_BUILD_OUT_WOOD = 736000;

		public const int FUN_BUILD_OUT_STONE = 737000;

		public const int LW_BUILDING_SEASON2_PERSONAL_FURNACE = 770000;

		public const int SEASON_STOVE_CENTER = 200000;

		public const int SEASON_MUMMY_CENTER = 300000;

		public const int SEASON_MUMMY_CENTER_CARRIER = 301000;

		public const int SEASON_POWER_CENTER = 400000;

		public const int SEASON_POWER_CENTER_CARRIER = 401000;

		public const int SEASON_POWER_CENTER_PLUGIN1 = 402000;

		public const int SEASON_POWER_CENTER_PLUGIN2 = 403000;

		public const int SEASON_POWER_CENTER_PLUGIN3 = 404000;

		public const int LW_BUILD_ACTIVITY_ALARM_CLOCK = 10224000;

		public const int LW_BUILD_SEASON4_POWER_STATION1 = 808000;

		public const int LW_BUILD_SEASON4_POWER_STATION2 = 809000;

		public const int LW_BUILD_SEASON4_POWER_STATION3 = 810000;

		public const int LW_BUILD_SEASON4_POWER_STATION4 = 811000;

		public const int LW_ALLIANCE_WAR_CAMP_2 = 91003;

		public const int LW_CITY_RUIN = 10301000;

		public const int LW_CITY_RUIN_1 = 10301001;
	}

	public class TableName
	{
		public const string LWHeros = "lw_hero";

		public const string LWSoldier = "lw_soldier";

		public const string APSMonster = "lw_world_monster";

		public const string APSHeros = "aps_new_heroes";

		public const string GuideTab = "guide";

		public const string PlotTab = "plot";

		public const string FieldMonster = "field_monster";

		public const string HeroTab = "new_heroes";

		public const string GoodsTab = "goods";

		public const string SkillTab = "skill";

		public const string BattleAnimation = "battle_animation";

		public const string StatusTab = "status";

		public const string EquipRandomEffect = "equip_random_effect";

		public const string AllianceGift = "alliance_gift";

		public const string AllianceGiftGroup = "alliance_gift_group";

		public const string AllianceItemWarehouse = "alliance_item_warehouse";

		public const string Territory = "territory";

		public const string TerritoryEffect = "territory_effect";

		public const string GoldrushBuilding = "goldrush_building";

		public const string ServerPos = "serverpos";

		public const string SiegeNPC = "siegeNPC";

		public const string Diary = "diary";

		public const string ActivityShow = "activity_show";

		public const string RightsEffectLevel = "rights_effect_level";

		public const string RightsEffect = "rights_effect";

		public const string VipStoreUnlock = "vip_store_unlock";

		public const string VipDetails = "vipdetails";

		public const string WorldSeason = "world_season";

		public const string WorldBuilding = "building_world";

		public const string DesertTalent = "DesertTalent_DesertTalent";

		public const string TalentShading = "DesertTalent_Shading";

		public const string TalentHome = "talentHome";

		public const string DesertGoldmineWar = "DesertGoldmineWar";

		public const string DesertTalentStats = "DesertTalentStats";

		public const string Decompose = "decompose";

		public const string Missile = "missile";

		public const string LoadingTips = "loadingTips";

		public const string Mail_ChannelID = "Mail_ChannelID";

		public const string PlayerCareerXml = "player_career";

		public const string QuestXml = "quest";

		public const string DesertSkillXml = "desertSkill";

		public const string GuideStep = "guide_step_GuideStep";

		public const string GuideStepContentInfo = "guide_step_ContentInfo";

		public const string Office = "office";

		public const string DoomsDayNote = "doomsdaynote_doomsdaynote";

		public const string DD_Season_Group = "DD_season_group";

		public const string Building = "building";

		public const string Train = "train_property";

		public const string TrainParam = "train_para";

		public const string Chapter = "chapter_1";

		public const string EffectName = "APS_effect_name";

		public const string Global = "APS_global";

		public const string Talent = "APS_talent";

		public const string ResourceItem = "aps_resource_item";

		public const string Farming = "aps_farming";

		public const string BaseExpansion = "aps_base_expansion";

		public const string GatherResource = "lw_gather_resource";

		public const string WorldCity = "lw_worldcity";

		public const string CityJunk = "aps_singlemap_junk";

		public const string LandLock = "aps_landlock";

		public const string Item = "item";

		public const string LwDispatchTask = "lw_dispatch_tasks";

		public const string LwDispatchSetting = "lw_dispatch_settings";

		public const string Decoration = "lw_decoration";

		public const string DecorationColorful = "decoration_colorful_skin";

		public const string DetectEvent = "detect_event";

		public const string WorldTreasure = "world_treasure";

		public const string Desert = "desert";

		public const string AllianceBuild = "alliance_res_build";

		public const string LwSound = "lw_Sound";

		public const string LWStatus = "lw_status";

		public const string LwGhostreconTask = "lw_ghostrecon_tasks";

		public const string LWIceSupplies = "ice_supplies";

		public const string LWSeasonBiuBiuServer = "season_bullet_server";

		public const string WorldTrigger = "season_landmine";

		public const string LW_Season = "lw_season";

		public const string World_Chess_Color = "world_chess_color";

		public const string Download_Packs = "download_packs";

		public const string LWSeasonBuildersAllianceGroup = "season_builders_alliance_group";

		public const string LWSeasonBuildersAllianceLevel = "season_builders_alliance_level";

		public const string LWSeasonBuildersAllianceList = "season_builders_alliance_list";

		public const string LWSeasonBuildersCityList = "season_builders_city_list";

		public const string ZoneMobilizationStage = "zone_mobilization_stage";

		public const string ZoneMobilizationBoss = "zone_mobilization_boss";

		public const string MeteoriteBattleEntity = "yuntie_battle_entity";

		public const string AllianceGovernmentSkill = "alliance_government_skill";

		public const string ActivityWorldTreasure = "activity_world_treasure";

		public const string MapSurprise = "map_surprise";

		public const string TREASURE_BOX_SHOW = "treasure_box_show";
	}

	public class SpecialItemID
	{
		public const int ITEM_MOVE_RANDOM = 200001;

		public const int ITEM_MOVE_CITY = 200002;

		public const int ITEM_FREE_MOVE_CITY = 200005;

		public const int LW_ITEM_ALLY_MOVE_CITY = 200008;

		public const int ITEM_MERGESERVER_MOVECITY = 200453;

		public const int ITEM_CROSS_MOVE_CITY = 200002;

		public const int ITEM_CROSS_FREE_CITY = 200005;

		public const int RECRUIT_TYPE_HERO_ACTIVITY = 200070;
	}

	public class SoundGround
	{
		public const string Music = "Music";

		public const string Sound = "Sound";

		public const string Effect = "Effect";

		public const string AMBSound = "AMBSound";

		public const string Dub = "Dub";

		public const string Hero = "Hero";

		public const string Timeline = "Timeline";
	}

	public class AtlasAssets
	{
		public const string PlayerHeadIcons = "PlayerHeadIcons";
	}

	public static class UILayer
	{
		public const string Scene = "Scene";

		public const string Background = "Background";

		public const string UIResource = "UIResource";

		public const string Normal = "Normal";

		public const string Info = "Info";

		public const string Dialog = "Dialog";

		public const string Guide = "Guide";

		public const string TopMost = "TopMost";

		public const string Battle3D = "3DUIContainer";
	}

	public static class FontPath
	{
		public static string Chinese = "Assets/fonts/方正粗圆简体.TTF";

		public static string Title = "Assets/fonts/domyouji-regular.otf";
	}

	public static class SpritePath
	{
		public const string HeroIconSmall = "Assets/Main/Sprites/HeroIconsSmall/";

		public const string UITitleTag = "Assets/Main/Sprites/UI/UITitleTag/";

		public const string ContryFlag = "Assets/Main/Sprites/CountryFlag/";
	}

	public static class SpriteName
	{
		public const string MeteoriteLodIcon_100_normal = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_bai.png";

		public const string MeteoriteLodIcon_100_enemy = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_hong.png";

		public const string MeteoriteLodIcon_100_alliance = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_lan.png";

		public const string MeteoriteLodIcon_100_self = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_lv.png";

		public const string MeteoriteLodIcon_100_sameServer = "Assets/Main/Sprites/LodIcon/zyf_yuntiesuipian_wujisuofang_huang.png";

		public const string MeteoriteScoreIcon_Red = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_hong.png";

		public const string MeteoriteScoreIcon_Yellow = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_huang.png";

		public const string MeteoriteScoreIcon_Blue = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_lan.png";

		public const string MeteoriteScoreIcon_Green = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_lv.png";

		public const string MeteoriteScoreIcon_Gray = "Assets/Main/Sprites/UI/LWActMeteorite/lrb_zhouliuhuodong_jifen_hui.png";

		public const string MyAssistanceIcon = "Assets/Main/Sprites/LodIcon/wxy_dashijie_zhufang_zhushouwo.png";

		public const string OtherAssistanceIcon = "Assets/Main/Sprites/LodIcon/wxy_dashijie_zhufang_zhushou.png";

		public const string MeteoriteLodIcon_101_normal = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_bai.png";

		public const string MeteoriteLodIcon_101_enemy = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_hong.png";

		public const string MeteoriteLodIcon_101_alliance = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_lan.png";

		public const string MeteoriteLodIcon_101_self = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_lv.png";

		public const string MeteoriteLodIcon_101_sameServer = "Assets/Main/Sprites/LodIcon/zyf_yuntiejiejing_wujisuofang_huang.png";

		public const string MeteoriteLodIcon_102_normal = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_bai.png";

		public const string MeteoriteLodIcon_102_enemy = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_hong.png";

		public const string MeteoriteLodIcon_102_alliance = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_lan.png";

		public const string MeteoriteLodIcon_102_self = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_lv.png";

		public const string MeteoriteLodIcon_102_sameServer = "Assets/Main/Sprites/LodIcon/zyf_yuntiejingti_wujisuofang_huang.png";
	}

	public static class QualitySetting
	{
		public const string PostProcess_Bloom = "QualitySetting.PostProcess.Bloom";

		public const string PostProcess_ColorAdjustments = "QualitySetting.PostProcess.ColorAdjustments";

		public const string PostProcess_Vignette = "QualitySetting.PostProcess.Vignette";

		public const string PostProcess_Tonemapping = "QualitySetting.PostProcess.Tonemapping";

		public const string PostProcess_LiftGammaGain = "QualitySetting.PostProcess.LiftGammaGain";

		public const string PostProcess_DepthOfField = "QualitySetting.PostProcess.DepthOfField";

		public const string Resolution = "QualitySetting.Resolution";

		public const string FPS = "QualitySetting.FPS";

		public const string Terrain = "QualitySetting.Terrain";

		public const string ShaderLOD = "QualitySetting.ShaderLOD";
	}

	public static class GuideType
	{
		public const int None = 0;

		public const int ClickButton = 1;

		public const int ShowTalk = 2;

		public const int ClickBuild = 3;

		public const int BuildPlace = 4;

		public const int BuildRoad = 5;

		public const int PlantFarm = 6;

		public const int GetFarm = 7;

		public const int QueueBuild = 8;

		public const int PlantAnimal = 9;

		public const int Factory = 10;

		public const int Bubble = 11;

		public const int CityGarbage = 12;

		public const int GotoMoveBubble = 13;

		public const int OpenFog = 14;

		public const int CityGarbageResultShow = 15;

		public const int DragCityTroop = 16;

		public const int PlayMovie = 17;

		public const int WaitMovieComplete = 18;

		public const int ClickQuest = 19;

		public const int WaitPlaceBuilding = 20;

		public const int WaitTroopArrive = 21;

		public const int WaitGarbageTroopMoveLeft = 22;

		public const int WaitCloseUI = 23;

		public const int ClickBuildFinishBox = 24;
	}

	public static class SeasonStatusId
	{
		public static int SEASON_MUMMY_CURSE1 = 703050;

		public static int SEASON_MUMMY_CURSE2 = 703060;

		public static int SEASON_MUMMY_CURSE3 = 703070;
	}

	public static class GuideTriggerType
	{
		public const int None = 0;

		public const int CityTroopFightMonsterTip = 23;
	}

	public static class CityLabelTextColor
	{
		public static Color32 Green = new Color32(181, 248, 49, byte.MaxValue);

		public static Color32 Blue = new Color32(84, 196, 242, byte.MaxValue);

		public static Color32 White = new Color32(228, 228, 228, byte.MaxValue);

		public static Color32 Yellow = new Color32(byte.MaxValue, 133, 39, byte.MaxValue);

		public static Color32 Red = new Color32(byte.MaxValue, 112, 109, byte.MaxValue);

		public static Color32 Purple = new Color32(167, 108, 240, byte.MaxValue);

		public static Color32 Black = new Color32(0, 0, 0, byte.MaxValue);

		public static Color32 AllianceEnemy = new Color32(229, 44, 32, byte.MaxValue);

		public static Color32 ZoneEnemy = new Color32(byte.MaxValue, 158, 132, byte.MaxValue);

		public static Color32 SeasonEnemy = new Color32(byte.MaxValue, 228, 0, byte.MaxValue);

		public static Color32 SeasonCamp = new Color32(95, byte.MaxValue, 250, byte.MaxValue);

		public static Color32 SeasonAssist = new Color32(45, 146, byte.MaxValue, byte.MaxValue);
	}

	public enum CityLabelColorType
	{
		Green = 1,
		Blue,
		White,
		Yellow,
		Red,
		Purple,
		Black,
		AllianceEnemy,
		ZoneEnemy,
		SeasonEnemy,
		SeasonCamp,
		SeasonAssist,
		BattlefieldDsbRole1,
		BattlefieldDsbRole2,
		BattlefieldDsbRole3,
		BattlefieldDsbRole4,
		BattlefieldDsbRoleMine
	}

	public static readonly Vector2 ScreenScaler = new Vector2(1920f, 1080f);

	public const string NoChannelParam = "FB_BTN_youjian";

	public const string UndisposedMail = "no_mail_channel";

	public const string GreenBlockFree = "Assets/Main/Sprites/Scene/road/green_block_free";

	public const string RedBlockFree = "Assets/Main/Sprites/Scene/road/red_block_free";

	public const string GreenFreeMoveKuang = "free_move_kuang_green";

	public const string RedFreeMoveKuang = "free_move_kuang_red";

	public const string DefaultDialog = "Assets/Main/Localization/English/Dictionaries/Dialog.txt";

	public const int EQUIP_SOLTCOUNT = 6;

	public const int AreaXSize = 8;

	public const int AreaYSize = 8;

	public const int TileXInArea = 8;

	public const int TileYInArea = 8;

	public const int MaxShowBuildBlockRange = 20;

	public static Vector2Int GuideBoardPos = new Vector2Int(-1, 2);

	public static Vector3 WorldCityNameBGScale = new Vector3(0.8f, 0.6f, 1f);

	public static int SkyBoxPosZ = 51;

	public static int SkyBoxPosZFreeGrass = SkyBoxPosZ + 14;

	public const int ITEM_TYPE_SPD = 2;

	public const string MAIN_CITY_ID = "10000";

	public const string MONTH_CARD_ID = "9007";

	public const string SUB_MONTH_CARD_ID = "9012";

	public const int MONTH_CARD_REWARD_COUNT = 10;

	public const string KINGDOM_KING_ID = "216000";

	public const string GREAT_KINGDOM_KING_ID = "222000";

	public const string INTRODE_KING_ID = "216017";

	public const string FIRST_GIVE_HERO_ID = "10009";

	public const string SECOND_GIVE_HERO_ID = "10010";

	public const int ITEM_GENE = 212005;

	public const int ITEM_RENAME = 200021;

	public const int THRONE_ID = 48;

	public const int THRONE_POINT_ID = 497500;

	public const float FLT_EPSILON = 1.1920929E-07f;

	public const string WB_SEASON_ACTIVITY = "57062";

	public const string WB_DECLARE_ACTIVITY = "57088";

	public const string ALLIANCE_NOTICE_KEY = "notice_0123456789";

	public const int MIN_NAME_CHAR = 3;

	public const int MAX_NAME_CHAR = 16;

	public const int MAX_MOOD_CHAR = 50;

	public const string FBLuckyDrawKey = "fbluckydrawkey";

	public const string DefaultMissile = "53301";

	public static Vector3 BlockPos = new Vector3(0f, 0.25f, 0f);

	public const int RandomTruckTargetMin = 3;

	public const float RandomTruckInterval = 3f;

	public const int RandomPeopleTargetMin = 2;

	public const float RandomPeopleInterval = 5f;

	public const float RoadRobotWorkHeight = 0f;

	public const float BuildRobotRotationSpeed = 270f;

	public const float BuildRobotFlyMaxSpeed = 15f;

	public const float BuildRobotTakeOffHeight = 4.81f;

	public const float BuildRobotWorkSpeed = 4f;

	public const float BuildRobotAcceleration = 12f;

	public const float BuildRobotApproachTime = 1f;

	public const int QualityLevel_Off = 0;

	public const int QualityLevel_Low = 1;

	public const int QualityLevel_Middle = 2;

	public const int QualityLevel_High = 3;

	public const string ShaderLODHigh = "_LOD_HIGH";

	public const string ShaderLODMiddle = "_LOD_MIDDLE";

	[Obsolete("shader中只定义high即可")]
	public const string ShaderLODLow = "_LOD_LOW";

	public const float RenderScaleLow = 0.8f;

	public const int MummySoldierMinId = 12001;

	public const int MummySoldierMaxId = 12011;

	public const string GrayMaterialName = "SpriteGray";

	public const float LookAtFocusTime = 0.4f;

	public const int FindCanBuildPointRange = 7;

	public const string SaveGuideDoneValue = "1";

	public const string FirstLaunchFlag = "FirstLaunchFlag";

	public const int FirstLaunch = 1;

	public const int NormalLaunch = 2;

	public const string FirstLaunchSkipUpdateFlag = "FirstLaunchSkipUpdateFlag";

	public const int FirstLaunchSkipUpdateDefault = 0;

	public const int FirstLaunchSkipUpdateRunning = 1;

	public const int FirstLaunchSkipUpdateDisable = 2;

	public static bool isArabicTMPFix = false;

	public static Vector3[] BuildTileCenterDelta = new Vector3[3]
	{
		new Vector3(0f, 0f, 0f),
		new Vector3(-1f, 0f, -1f),
		new Vector3(-2f, 0f, -2f)
	};

	public static List<BuildConnectRoadDirection> BuildConnectList = new List<BuildConnectRoadDirection>
	{
		BuildConnectRoadDirection.Down,
		BuildConnectRoadDirection.Left,
		BuildConnectRoadDirection.Right,
		BuildConnectRoadDirection.Top
	};

	public static List<DirectionType> ConnectDirList = new List<DirectionType>
	{
		DirectionType.Down,
		DirectionType.Left,
		DirectionType.Right,
		DirectionType.Top
	};

	public const int Int32Bit = 32;

	public const int ByteSize = 8;

	public static Dictionary<Vector2Int, Direction> OffsetToDirectionMap = new Dictionary<Vector2Int, Direction>
	{
		{
			new Vector2Int(1, 0),
			Direction.SE
		},
		{
			new Vector2Int(2, 0),
			Direction.SE
		},
		{
			new Vector2Int(-1, 0),
			Direction.NW
		},
		{
			new Vector2Int(-2, 0),
			Direction.NW
		},
		{
			new Vector2Int(0, 1),
			Direction.SW
		},
		{
			new Vector2Int(0, 2),
			Direction.SW
		},
		{
			new Vector2Int(0, -1),
			Direction.NE
		},
		{
			new Vector2Int(0, -2),
			Direction.NE
		},
		{
			new Vector2Int(1, 1),
			Direction.S
		},
		{
			new Vector2Int(2, 1),
			Direction.S
		},
		{
			new Vector2Int(1, 2),
			Direction.S
		},
		{
			new Vector2Int(2, 2),
			Direction.S
		},
		{
			new Vector2Int(-1, -1),
			Direction.N
		},
		{
			new Vector2Int(-2, -1),
			Direction.N
		},
		{
			new Vector2Int(-1, -2),
			Direction.N
		},
		{
			new Vector2Int(-2, -2),
			Direction.N
		},
		{
			new Vector2Int(1, -1),
			Direction.E
		},
		{
			new Vector2Int(2, -1),
			Direction.E
		},
		{
			new Vector2Int(1, -2),
			Direction.E
		},
		{
			new Vector2Int(2, -2),
			Direction.E
		},
		{
			new Vector2Int(-1, 1),
			Direction.W
		},
		{
			new Vector2Int(-2, 1),
			Direction.W
		},
		{
			new Vector2Int(-1, 2),
			Direction.W
		},
		{
			new Vector2Int(-2, 2),
			Direction.W
		}
	};

	public const string Player_CareerID = "222000";

	public static Vector3 LandLockPosOffset = new Vector3(-33f, 0f, -33f);
}
