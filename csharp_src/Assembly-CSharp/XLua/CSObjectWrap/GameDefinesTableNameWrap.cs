using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesTableNameWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.TableName);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 90, 0, 0);
		Utils.RegisterObject(L, translator, -4, "LWHeros", "lw_hero");
		Utils.RegisterObject(L, translator, -4, "LWSoldier", "lw_soldier");
		Utils.RegisterObject(L, translator, -4, "APSMonster", "lw_world_monster");
		Utils.RegisterObject(L, translator, -4, "APSHeros", "aps_new_heroes");
		Utils.RegisterObject(L, translator, -4, "GuideTab", "guide");
		Utils.RegisterObject(L, translator, -4, "PlotTab", "plot");
		Utils.RegisterObject(L, translator, -4, "FieldMonster", "field_monster");
		Utils.RegisterObject(L, translator, -4, "HeroTab", "new_heroes");
		Utils.RegisterObject(L, translator, -4, "GoodsTab", "goods");
		Utils.RegisterObject(L, translator, -4, "SkillTab", "skill");
		Utils.RegisterObject(L, translator, -4, "BattleAnimation", "battle_animation");
		Utils.RegisterObject(L, translator, -4, "StatusTab", "status");
		Utils.RegisterObject(L, translator, -4, "EquipRandomEffect", "equip_random_effect");
		Utils.RegisterObject(L, translator, -4, "AllianceGift", "alliance_gift");
		Utils.RegisterObject(L, translator, -4, "AllianceGiftGroup", "alliance_gift_group");
		Utils.RegisterObject(L, translator, -4, "AllianceItemWarehouse", "alliance_item_warehouse");
		Utils.RegisterObject(L, translator, -4, "Territory", "territory");
		Utils.RegisterObject(L, translator, -4, "TerritoryEffect", "territory_effect");
		Utils.RegisterObject(L, translator, -4, "GoldrushBuilding", "goldrush_building");
		Utils.RegisterObject(L, translator, -4, "ServerPos", "serverpos");
		Utils.RegisterObject(L, translator, -4, "SiegeNPC", "siegeNPC");
		Utils.RegisterObject(L, translator, -4, "Diary", "diary");
		Utils.RegisterObject(L, translator, -4, "ActivityShow", "activity_show");
		Utils.RegisterObject(L, translator, -4, "RightsEffectLevel", "rights_effect_level");
		Utils.RegisterObject(L, translator, -4, "RightsEffect", "rights_effect");
		Utils.RegisterObject(L, translator, -4, "VipStoreUnlock", "vip_store_unlock");
		Utils.RegisterObject(L, translator, -4, "VipDetails", "vipdetails");
		Utils.RegisterObject(L, translator, -4, "WorldSeason", "world_season");
		Utils.RegisterObject(L, translator, -4, "WorldBuilding", "building_world");
		Utils.RegisterObject(L, translator, -4, "DesertTalent", "DesertTalent_DesertTalent");
		Utils.RegisterObject(L, translator, -4, "TalentShading", "DesertTalent_Shading");
		Utils.RegisterObject(L, translator, -4, "TalentHome", "talentHome");
		Utils.RegisterObject(L, translator, -4, "DesertGoldmineWar", "DesertGoldmineWar");
		Utils.RegisterObject(L, translator, -4, "DesertTalentStats", "DesertTalentStats");
		Utils.RegisterObject(L, translator, -4, "Decompose", "decompose");
		Utils.RegisterObject(L, translator, -4, "Missile", "missile");
		Utils.RegisterObject(L, translator, -4, "LoadingTips", "loadingTips");
		Utils.RegisterObject(L, translator, -4, "Mail_ChannelID", "Mail_ChannelID");
		Utils.RegisterObject(L, translator, -4, "PlayerCareerXml", "player_career");
		Utils.RegisterObject(L, translator, -4, "QuestXml", "quest");
		Utils.RegisterObject(L, translator, -4, "DesertSkillXml", "desertSkill");
		Utils.RegisterObject(L, translator, -4, "GuideStep", "guide_step_GuideStep");
		Utils.RegisterObject(L, translator, -4, "GuideStepContentInfo", "guide_step_ContentInfo");
		Utils.RegisterObject(L, translator, -4, "Office", "office");
		Utils.RegisterObject(L, translator, -4, "DoomsDayNote", "doomsdaynote_doomsdaynote");
		Utils.RegisterObject(L, translator, -4, "DD_Season_Group", "DD_season_group");
		Utils.RegisterObject(L, translator, -4, "Building", "building");
		Utils.RegisterObject(L, translator, -4, "Train", "train_property");
		Utils.RegisterObject(L, translator, -4, "TrainParam", "train_para");
		Utils.RegisterObject(L, translator, -4, "Chapter", "chapter_1");
		Utils.RegisterObject(L, translator, -4, "EffectName", "APS_effect_name");
		Utils.RegisterObject(L, translator, -4, "Global", "APS_global");
		Utils.RegisterObject(L, translator, -4, "Talent", "APS_talent");
		Utils.RegisterObject(L, translator, -4, "ResourceItem", "aps_resource_item");
		Utils.RegisterObject(L, translator, -4, "Farming", "aps_farming");
		Utils.RegisterObject(L, translator, -4, "BaseExpansion", "aps_base_expansion");
		Utils.RegisterObject(L, translator, -4, "GatherResource", "lw_gather_resource");
		Utils.RegisterObject(L, translator, -4, "WorldCity", "lw_worldcity");
		Utils.RegisterObject(L, translator, -4, "CityJunk", "aps_singlemap_junk");
		Utils.RegisterObject(L, translator, -4, "LandLock", "aps_landlock");
		Utils.RegisterObject(L, translator, -4, "Item", "item");
		Utils.RegisterObject(L, translator, -4, "LwDispatchTask", "lw_dispatch_tasks");
		Utils.RegisterObject(L, translator, -4, "LwDispatchSetting", "lw_dispatch_settings");
		Utils.RegisterObject(L, translator, -4, "Decoration", "lw_decoration");
		Utils.RegisterObject(L, translator, -4, "DecorationColorful", "decoration_colorful_skin");
		Utils.RegisterObject(L, translator, -4, "DetectEvent", "detect_event");
		Utils.RegisterObject(L, translator, -4, "WorldTreasure", "world_treasure");
		Utils.RegisterObject(L, translator, -4, "Desert", "desert");
		Utils.RegisterObject(L, translator, -4, "AllianceBuild", "alliance_res_build");
		Utils.RegisterObject(L, translator, -4, "LwSound", "lw_Sound");
		Utils.RegisterObject(L, translator, -4, "LWStatus", "lw_status");
		Utils.RegisterObject(L, translator, -4, "LwGhostreconTask", "lw_ghostrecon_tasks");
		Utils.RegisterObject(L, translator, -4, "LWIceSupplies", "ice_supplies");
		Utils.RegisterObject(L, translator, -4, "LWSeasonBiuBiuServer", "season_bullet_server");
		Utils.RegisterObject(L, translator, -4, "WorldTrigger", "season_landmine");
		Utils.RegisterObject(L, translator, -4, "LW_Season", "lw_season");
		Utils.RegisterObject(L, translator, -4, "World_Chess_Color", "world_chess_color");
		Utils.RegisterObject(L, translator, -4, "Download_Packs", "download_packs");
		Utils.RegisterObject(L, translator, -4, "LWSeasonBuildersAllianceGroup", "season_builders_alliance_group");
		Utils.RegisterObject(L, translator, -4, "LWSeasonBuildersAllianceLevel", "season_builders_alliance_level");
		Utils.RegisterObject(L, translator, -4, "LWSeasonBuildersAllianceList", "season_builders_alliance_list");
		Utils.RegisterObject(L, translator, -4, "LWSeasonBuildersCityList", "season_builders_city_list");
		Utils.RegisterObject(L, translator, -4, "ZoneMobilizationStage", "zone_mobilization_stage");
		Utils.RegisterObject(L, translator, -4, "ZoneMobilizationBoss", "zone_mobilization_boss");
		Utils.RegisterObject(L, translator, -4, "MeteoriteBattleEntity", "yuntie_battle_entity");
		Utils.RegisterObject(L, translator, -4, "AllianceGovernmentSkill", "alliance_government_skill");
		Utils.RegisterObject(L, translator, -4, "ActivityWorldTreasure", "activity_world_treasure");
		Utils.RegisterObject(L, translator, -4, "MapSurprise", "map_surprise");
		Utils.RegisterObject(L, translator, -4, "TREASURE_BOX_SHOW", "treasure_box_show");
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
				GameDefines.TableName o = new GameDefines.TableName();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.TableName constructor!");
	}
}
