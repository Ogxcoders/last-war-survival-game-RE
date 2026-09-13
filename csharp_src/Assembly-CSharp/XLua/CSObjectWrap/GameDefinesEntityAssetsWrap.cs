using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesEntityAssetsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.EntityAssets);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 132, 0, 0);
		Utils.RegisterObject(L, translator, -4, "Building", "Assets/Main/Prefabs/Building/{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "AllianceBuilding", "Assets/Main/Prefabs/AllianceBuilding/{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldRoadRobot", "Assets/Main/Prefabs/World/WorldRoadRobot.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldBuildingRobot", "Assets/Main/Prefabs/World/WorldBuildingRobot.prefab");
		Utils.RegisterObject(L, translator, -4, "TouchTerrainEffect", "Assets/Main/Prefabs/World/TouchTerrainEffect.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityGrass", "Assets/Main/Prefabs/World/WorldCityGrass.prefab");
		Utils.RegisterObject(L, translator, -4, "BatteryAttackRange", "Assets/_Art/Effect/prefab/scene/Build/V_paota_fanwei.prefab");
		Utils.RegisterObject(L, translator, -4, "QuanEffectRange", "Assets/Main/Prefabs/BuildEffect/V_zdbx_quan.prefab");
		Utils.RegisterObject(L, translator, -4, "LandLock", "Assets/_Art_LastWar/Models/Environment/Interactive/Massif/{0}");
		Utils.RegisterObject(L, translator, -4, "BFLongPressEffect", "Assets/Main/Prefabs/Effect/BattleField/Eff_ui_longpress_signal.prefab");
		Utils.RegisterObject(L, translator, -4, "World", "Assets/Main/Prefabs/World/Scene_World.prefab");
		Utils.RegisterObject(L, translator, -4, "City", "Assets/Main/Prefabs/World/Scene_City.prefab");
		Utils.RegisterObject(L, translator, -4, "Wasteland_City", "Assets/Main/Prefabs/World/Scene_City2.prefab");
		Utils.RegisterObject(L, translator, -4, "Wasteland_City_Dig", "Assets/Main/Prefabs/World/Scene_City_Dig.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldSceneDesc", "Assets/Main/Scenes/WorldSceneDesc.bytes");
		Utils.RegisterObject(L, translator, -4, "NewWorldSceneDesc", "Assets/Main/Scenes/NewWorldSceneDesc.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldBlockDesc", "Assets/Main/Scenes/WorldBlockDesc.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldSceneDecoration", "Assets/Main/Scenes/WorldDecoration/O_build_cangqiong_shu_3_1023.asset");
		Utils.RegisterObject(L, translator, -4, "WorldDecorationBatchPath", "Assets/Main/Scenes/WorldDecoration/WorldDecoration.asset");
		Utils.RegisterObject(L, translator, -4, "WorldSceneAllianceCityDesc", "Assets/Main/Scenes/WorldSceneAllianceCityDesc.bytes");
		Utils.RegisterObject(L, translator, -4, "LandLockDesc", "Assets/Main/Scenes/LandLockDesc.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZone", "Assets/Main/Scenes/Zone/zone.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZoneS1", "Assets/Main/SeasonRes/S1/Scenes/zone_S1.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZoneS2", "Assets/Main/SeasonRes/S2/Scenes/zone_S2.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZoneS3", "Assets/Main/SeasonRes/S3/Scenes/Zone/zone_S3.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZoneS4", "Assets/Main/SeasonRes/S4/Scenes/Zone/zone_S4.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZoneS5", "Assets/Main/SeasonRes/S5/Scenes/Zone/zone_S5.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZonePosS0", "Assets/Main/Scenes/Zone/pos.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZonePosS1", "Assets/Main/SeasonRes/S1/Scenes/pos_S1.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZonePosS2", "Assets/Main/SeasonRes/S2/Scenes/pos_S2.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZonePosS3", "Assets/Main/SeasonRes/S3/Scenes/Zone/pos_S3.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZonePosS4", "Assets/Main/SeasonRes/S4/Scenes/Zone/pos_S4.bytes");
		Utils.RegisterObject(L, translator, -4, "WorldMapZonePosS5", "Assets/Main/SeasonRes/S5/Scenes/Zone/pos_S5.bytes");
		Utils.RegisterObject(L, translator, -4, "Terrain_World", "Assets/Main/Prefabs/World/Terrain_World.prefab");
		Utils.RegisterObject(L, translator, -4, "TerrainSetting_Low", "Assets/Main/Prefabs/World/TerrainSetting_Low.asset");
		Utils.RegisterObject(L, translator, -4, "TerrainSetting_High", "Assets/Main/Prefabs/World/TerrainSetting_High.asset");
		Utils.RegisterObject(L, translator, -4, "Terrain_City_Low", "Assets/Main/Prefabs/World/Terrain_City.prefab");
		Utils.RegisterObject(L, translator, -4, "Terrain_City_High", "Assets/Main/Prefabs/World/Terrain_City_High.prefab");
		Utils.RegisterObject(L, translator, -4, "TerrainSetting_City_Low", "Assets/Main/Prefabs/World/TerrainSetting_City_Low.asset");
		Utils.RegisterObject(L, translator, -4, "TerrainSetting_City_High", "Assets/Main/Prefabs/World/TerrainSetting_City_High.asset");
		Utils.RegisterObject(L, translator, -4, "TroopLine", "Assets/Main/Prefabs/March/TroopLine.prefab");
		Utils.RegisterObject(L, translator, -4, "TroopLineWithTimer", "Assets/Main/Prefabs/March/TroopLineWithTimer.prefab");
		Utils.RegisterObject(L, translator, -4, "TroopDestinationSignal", "Assets/Main/Prefabs/March/TroopDestinationSignal.prefab");
		Utils.RegisterObject(L, translator, -4, "TroopLineDrag", "Assets/Main/Prefabs/March/TroopLineDrag.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroop", "Assets/Main/Prefabs/March/WorldTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopHSR", "Assets/Main/SeasonRes/S5/Prefabs/World/WorldTroopHSR.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopTrain", "Assets/Main/Prefabs/March/WorldTroopTrain.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopTruck", "Assets/Main/Prefabs/March/WorldTroopTruck.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopFlowerTrain", "Assets/Main/Prefabs/World/FlowerTrain_World_Prefab/WorldTroopFlowerTrain.prefab");
		Utils.RegisterObject(L, translator, -4, "MonsterActBoss", "Assets/Main/Prefabs/Monsters/MonsterActBoss.prefab");
		Utils.RegisterObject(L, translator, -4, "ZombieBusTrain", "Assets/Main/Prefabs/March/WorldTroopZombieBusTrain.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopAlliance", "Assets/Main/Prefabs/March/WorldTroopAlliance.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopOther", "Assets/Main/Prefabs/March/WorldTroopOther.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopOtherYbc", "Assets/Main/Prefabs/March/WorldTroopOtherYbc.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldVirtualTroop", "Assets/Main/Prefabs/March/WorldVirtualTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "ScoutTroop", "Assets/Main/Prefabs/March/WorldTroopScout.prefab");
		Utils.RegisterObject(L, translator, -4, "donateTroop", "Assets/Main/Prefabs/March/WorldTroopMobilizationDonate.prefab");
		Utils.RegisterObject(L, translator, -4, "ResTransTroop", "Assets/Main/Prefabs/March/WorldTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "GolloesExploreTroop", "Assets/Main/Prefabs/March/GolloesExploreTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "GolloesTradeTroop", "Assets/Main/Prefabs/March/GolloesTradeTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldRallyTroop", "Assets/Main/Prefabs/March/WorldTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "FieldMonster", "Assets/Main/Prefabs/Monsters/FieldMonster.prefab");
		Utils.RegisterObject(L, translator, -4, "FieldBoss", "Assets/Main/Prefabs/Monsters/FieldBoss.prefab");
		Utils.RegisterObject(L, translator, -4, "ConstructMaterial", "Assets/Main/Material/building_construct.mat");
		Utils.RegisterObject(L, translator, -4, "Road_In_City", "Assets/Main/Prefabs/Road/Road_In_City_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "Road_Out_City", "Assets/Main/Prefabs/Road/Road_Out_City_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "Road_In_City_Updating", "Assets/Main/Prefabs/Road/Road_In_City_Updating_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "Road_Out_City_Updating", "Assets/Main/Prefabs/Road/Road_Out_City_Updating_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "Road_Self", "Assets/Main/Prefabs/Road/Road_Self_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "Road_Self_Updating", "Assets/Main/Prefabs/Road/Road_Self_Updating_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "Road_Fake", "Assets/Main/Prefabs/Road/Road_Fake_{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "TileUnlocked", "Assets/Main/Prefabs/World/TileUnlocked.prefab");
		Utils.RegisterObject(L, translator, -4, "TileLocked", "Assets/Main/Prefabs/World/TileLocked.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildGrid", "Assets/Main/Prefabs/Building/BuildGrid{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "RoadBlockGreen", "Assets/Main/Prefabs/Road/RoadBlockGreen2.prefab");
		Utils.RegisterObject(L, translator, -4, "RoadBlockRed", "Assets/Main/Prefabs/Road/RoadBlockRed2.prefab");
		Utils.RegisterObject(L, translator, -4, "RoadGrid", "Assets/Main/Prefabs/Road/RoadGrid.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildSelect", "Assets/Main/Prefabs/Building/BuildSelect{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "RoadLightInCity", "Assets/Main/Prefabs/Road/RoadLightInCity{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "RoadLightOutCity", "Assets/Main/Prefabs/Road/RoadLightOutCity{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "RoadLightSelf", "Assets/Main/Prefabs/Road/RoadLightSelf{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "MonsterPath", "Assets/Main/Prefabs/Monsters/{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildBlock", "Assets/Main/Prefabs/Building/BuildBlock.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopSoldier", "Assets/Main/Prefabs/March/WorldTroopSoldier.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopTank", "Assets/Main/Prefabs/March/WorldTroopTank.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopPlane", "Assets/Main/Prefabs/March/WorldTroopPlane.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldTroopJunkman", "Assets/Main/Prefabs/March/WorldTroopJunkman.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectAnimalModel", "Assets/Main/Prefabs/CollectResource/CollectAnimalModel.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectArmyAnimalModel", "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModel.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectArmyAnimalModelAlliance", "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModelAlliance.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectArmyAnimalModelEnemy", "Assets/Main/Prefabs/CollectResource/CollectArmyAnimalModelEnemy.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildMetalFew", "Assets/Main/Prefabs/Building/BuildMetalFew.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildMetalMiddle", "Assets/Main/Prefabs/Building/BuildMetalMiddle.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildMetalMax", "Assets/Main/Prefabs/Building/BuildMetalMax.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildWoodFew", "Assets/Main/Prefabs/Building/BuildWoodFew.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildWoodMiddle", "Assets/Main/Prefabs/Building/BuildWoodMiddle.prefab");
		Utils.RegisterObject(L, translator, -4, "BuildWoodMax", "Assets/Main/Prefabs/Building/BuildWoodMax.prefab");
		Utils.RegisterObject(L, translator, -4, "DetectEventUI", "Assets/Main/Prefabs/March/WorldDetectInfo.prefab");
		Utils.RegisterObject(L, translator, -4, "DetectEventFakePlayerUI", "Assets/Main/Prefabs/March/WorldFakePlayerDetectInfo.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldBaseHead", "Assets/Main/Prefabs/Building/WorldBaseHead.prefab");
		Utils.RegisterObject(L, translator, -4, "CityGarbagePath", "Assets/Main/Prefabs/Garbage/{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "CityTroop", "Assets/Main/Prefabs/March/CityTroop.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectGarbageUI", "Assets/Main/Prefabs/March/CollectGarbageUI.prefab");
		Utils.RegisterObject(L, translator, -4, "DispatchTaskCdUI", "Assets/Main/Prefabs/DispatchTask/dispatchTaskCdUI.prefab");
		Utils.RegisterObject(L, translator, -4, "DispatchTaskOpenUI", "Assets/Main/Prefabs/DispatchTask/dispatchTaskOpenUI.prefab");
		Utils.RegisterObject(L, translator, -4, "DispatchTaskRewardUI", "Assets/Main/Prefabs/DispatchTask/dispatchTaskRewardUI.prefab");
		Utils.RegisterObject(L, translator, -4, "GhostreconRewardUI", "Assets/Main/Prefabs/DispatchTask/ghostreconRewardUI.prefab");
		Utils.RegisterObject(L, translator, -4, "CityWorkMan", "Assets/Main/Prefabs/CityScene/CityWorkMan.prefab");
		Utils.RegisterObject(L, translator, -4, "FogPath", "Assets/Main/Prefabs/FogOfWar/{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "CityCameraSand", "Assets/Main/Prefabs/CityScene/CityCameraSand.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTreeHigh", "Assets/Main/Prefabs/World/WorldCityTreeHigh{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCityTree", "Assets/Main/Prefabs/World/WorldCityTree{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "FocusCurve", "Assets/Main/Prefabs/CityScene/FocusCurve{0}.prefab");
		Utils.RegisterObject(L, translator, -4, "CitySpaceMan", "Assets/Main/Prefabs/CityScene/CitySpaceMan.prefab");
		Utils.RegisterObject(L, translator, -4, "GarbageStone", "Assets/_Art/Models/Soldier/ShiHuangXiaoRen/prefab/A_soldie_shxr_rock_1.prefab");
		Utils.RegisterObject(L, translator, -4, "GarbageCrystal", "Assets/_Art/Models/Soldier/ShiHuangXiaoRen/prefab/A_soldie_shxr_tuohuang_crystal_1.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectBuildModelSelf", "Assets/Main/Prefabs/Building/building_collect.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectBuildModelAlliance", "Assets/Main/Prefabs/Building/building_collect_alliance.prefab");
		Utils.RegisterObject(L, translator, -4, "CollectBuildModelEnemy", "Assets/Main/Prefabs/Building/building_collect_enemy.prefab");
		Utils.RegisterObject(L, translator, -4, "LandLockFadeOut", "Assets/Main/Prefabs/World/LandLockFadeOut.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCloud1", "Assets/Main/Prefabs/World/Eff_daditu_yun_01.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCloud2", "Assets/Main/Prefabs/World/Eff_daditu_yun_02.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCloud3", "Assets/Main/Prefabs/World/Eff_daditu_yun_03.prefab");
		Utils.RegisterObject(L, translator, -4, "WorldCloud4", "Assets/Main/Prefabs/World/Eff_daditu_yun_04.prefab");
		Utils.RegisterObject(L, translator, -4, "DetectEventBarricade", "Assets/Main/Prefabs/World/DetectEvent_barricade.prefab");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattleActorFragmentAsset", "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentSmall.prefab");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattlePrepareFragmentAsset", "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldFragmentMiddle.prefab");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattleBigFragmentAsset", "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldDropDaddy.prefab");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattleFragmentDropWarningAsset", "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteSonDropWarning.prefab");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattleDropEffAsset", "Assets/Main/Prefabs/Effect/World/Meteorite/WorldMeteoriteAbandonEffect.prefab");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattleCameraEffAsset", "Assets/Main/Prefabs/World/Meteorite/EffPrefabs/MeteoriteWorldCameraEff.prefab");
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
				GameDefines.EntityAssets o = new GameDefines.EntityAssets();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.EntityAssets constructor!");
	}
}
