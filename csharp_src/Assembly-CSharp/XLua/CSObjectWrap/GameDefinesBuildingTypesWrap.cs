using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesBuildingTypesWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines.BuildingTypes);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 79, 0, 0);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_MAIN", 10100000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_BUSINESS_CENTER", 401000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_STABLE", 402000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_SCIENE", 403000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_SMITHY", 407000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_CONDOMINIUM", 409000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_HOSPITAL", 411000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_STONE", 412000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OIL", 413000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_ARROW_TOWER", 418000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_CAR_BARRACK", 423000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_INFANTRY_BARRACK", 424000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_AIRCRAFT_BARRACK", 425000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_TRAINFIELD_1", 427000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_TRAINFIELD_2", 793000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_TRAINFIELD_3", 794000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_TRAINFIELD_4", 795000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_WATER", 432000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_MARKET", 435000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_ROAD", 436000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_ELECTRICITY_STORAGE", 437000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_WATER_STORAGE", 438000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OIL_STORAGE", 439000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_IRON_STORAGE", 441000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_WIND_TURBINE", 444000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_SOLAR_POWER_STATION", 447000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_DRONE", 477000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_VILLA", 700000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_FARM", 701000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_FARM_FIELD", 702000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_PASTURE", 703000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_PASTURE_FIELD", 704000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OXYGEN", 705000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_METALLURGY", 706000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_FOOD", 707000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OIL_REFINERY", 708000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_INTEGRATED_FACTORY", 709000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_TRADING_CENTER", 710000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_FOODSHOP", 711000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_PRINT_FACTORY", 712000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_INFORMATION_CENTER", 713000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_COLD_STORAGE", 714000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_COMPREHENSIVE_STORAGE", 715000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_DEFENCE_CENTER", 716000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_DOME", 449000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_FORGE", 429000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_ELECTRICITY", 431000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_RECHARGE_GARAGE", 445000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_HONOR_HALL", 446000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_BUILDING_CENTER", 448000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OFFICER", 483000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_PASTURE_OSTRICH", 719000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_PASTURE_CATTLE", 720000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_PASTURE_SANDWORM", 721000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_WORMHOLE_MAIN", 791000);
		Utils.RegisterObject(L, translator, -4, "APS_BUILD_WORMHOLE_SUB", 792000);
		Utils.RegisterObject(L, translator, -4, "WORM_HOLE_CROSS", 735000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_RADAR_CENTER", 417000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_TEMP_WIND_POWER_PLANT", 796000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OUT_WOOD", 736000);
		Utils.RegisterObject(L, translator, -4, "FUN_BUILD_OUT_STONE", 737000);
		Utils.RegisterObject(L, translator, -4, "LW_BUILDING_SEASON2_PERSONAL_FURNACE", 770000);
		Utils.RegisterObject(L, translator, -4, "SEASON_STOVE_CENTER", 200000);
		Utils.RegisterObject(L, translator, -4, "SEASON_MUMMY_CENTER", 300000);
		Utils.RegisterObject(L, translator, -4, "SEASON_MUMMY_CENTER_CARRIER", 301000);
		Utils.RegisterObject(L, translator, -4, "SEASON_POWER_CENTER", 400000);
		Utils.RegisterObject(L, translator, -4, "SEASON_POWER_CENTER_CARRIER", 401000);
		Utils.RegisterObject(L, translator, -4, "SEASON_POWER_CENTER_PLUGIN1", 402000);
		Utils.RegisterObject(L, translator, -4, "SEASON_POWER_CENTER_PLUGIN2", 403000);
		Utils.RegisterObject(L, translator, -4, "SEASON_POWER_CENTER_PLUGIN3", 404000);
		Utils.RegisterObject(L, translator, -4, "LW_BUILD_ACTIVITY_ALARM_CLOCK", 10224000);
		Utils.RegisterObject(L, translator, -4, "LW_BUILD_SEASON4_POWER_STATION1", 808000);
		Utils.RegisterObject(L, translator, -4, "LW_BUILD_SEASON4_POWER_STATION2", 809000);
		Utils.RegisterObject(L, translator, -4, "LW_BUILD_SEASON4_POWER_STATION3", 810000);
		Utils.RegisterObject(L, translator, -4, "LW_BUILD_SEASON4_POWER_STATION4", 811000);
		Utils.RegisterObject(L, translator, -4, "LW_ALLIANCE_WAR_CAMP_2", 91003);
		Utils.RegisterObject(L, translator, -4, "LW_CITY_RUIN", 10301000);
		Utils.RegisterObject(L, translator, -4, "LW_CITY_RUIN_1", 10301001);
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
				GameDefines.BuildingTypes o = new GameDefines.BuildingTypes();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDefines.BuildingTypes constructor!");
	}
}
