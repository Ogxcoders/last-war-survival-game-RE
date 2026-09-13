using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDialogDefineWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDialogDefine);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 82, 0, 0);
		Utils.RegisterObject(L, translator, -4, "SELECT_ITEM", "120140");
		Utils.RegisterObject(L, translator, -4, "SELECT", "360005");
		Utils.RegisterObject(L, translator, -4, "BUILD_LIMIT_OTHERBASE_RANGE", "130000");
		Utils.RegisterObject(L, translator, -4, "BUILD_BOARD_LIMIT_MYBASE_RANGE", "130001");
		Utils.RegisterObject(L, translator, -4, "BUILD_ROAD_LIMIT_MYBASE_RANGE", "130001");
		Utils.RegisterObject(L, translator, -4, "BUILD", "110015");
		Utils.RegisterObject(L, translator, -4, "REMOVE", "130242");
		Utils.RegisterObject(L, translator, -4, "BOARD", "100037");
		Utils.RegisterObject(L, translator, -4, "ROAD", "100038");
		Utils.RegisterObject(L, translator, -4, "OWN", "130128");
		Utils.RegisterObject(L, translator, -4, "OUT", "130217");
		Utils.RegisterObject(L, translator, -4, "STORAGE", "130216");
		Utils.RegisterObject(L, translator, -4, "REPORT", "310019");
		Utils.RegisterObject(L, translator, -4, "TASK", "100179");
		Utils.RegisterObject(L, translator, -4, "COMMANDERINFO", "280024");
		Utils.RegisterObject(L, translator, -4, "LEVEL_NUMBER", "300665");
		Utils.RegisterObject(L, translator, -4, "SPLIT", "150033");
		Utils.RegisterObject(L, translator, -4, "COMMANDER_REPORT", "100017");
		Utils.RegisterObject(L, translator, -4, "UIMAINBOTTOM_COMMANDER_TIP", "130003");
		Utils.RegisterObject(L, translator, -4, "TIME", "100018");
		Utils.RegisterObject(L, translator, -4, "NEED_MINE", "100019");
		Utils.RegisterObject(L, translator, -4, "NEED_BUILD_BUILDING", "130004");
		Utils.RegisterObject(L, translator, -4, "GOODS", "100080");
		Utils.RegisterObject(L, translator, -4, "OIL", "100014");
		Utils.RegisterObject(L, translator, -4, "METAL", "100013");
		Utils.RegisterObject(L, translator, -4, "NUCLEAR", "100012");
		Utils.RegisterObject(L, translator, -4, "FOOD", "100011");
		Utils.RegisterObject(L, translator, -4, "OXYGEN", "100010");
		Utils.RegisterObject(L, translator, -4, "TRADE_PERCENT", "100009");
		Utils.RegisterObject(L, translator, -4, "HOUSE_PERCENT", "100008");
		Utils.RegisterObject(L, translator, -4, "HOSPITAL_PERCENT", "100007");
		Utils.RegisterObject(L, translator, -4, "SCIENCE_PERCENT", "100006");
		Utils.RegisterObject(L, translator, -4, "BUILD_PERCENT", "100005");
		Utils.RegisterObject(L, translator, -4, "ENVIRONMENT_PERCENT", "100004");
		Utils.RegisterObject(L, translator, -4, "WATER", "100546");
		Utils.RegisterObject(L, translator, -4, "ELECTRICITY", "100002");
		Utils.RegisterObject(L, translator, -4, "PEOPLE", "390098");
		Utils.RegisterObject(L, translator, -4, "MONEY", "100000");
		Utils.RegisterObject(L, translator, -4, "LACK_RESOURCE", "130072");
		Utils.RegisterObject(L, translator, -4, "LACK_ELECTRCITY_TIP", "130005");
		Utils.RegisterObject(L, translator, -4, "CAN_PUT", "130006");
		Utils.RegisterObject(L, translator, -4, "NO_PUT_REASON", "130007");
		Utils.RegisterObject(L, translator, -4, "INCLUD_MINEPOINT", "130008");
		Utils.RegisterObject(L, translator, -4, "INCLUDE_MINERANGE_POINT", "130009");
		Utils.RegisterObject(L, translator, -4, "RESOURCE_BUILD_PUT_MINERANGE_POINT", "130010");
		Utils.RegisterObject(L, translator, -4, "OUT_MYBASE_RANGE", "130011");
		Utils.RegisterObject(L, translator, -4, "IN_OTHERBASE_RANGE", "130012");
		Utils.RegisterObject(L, translator, -4, "NO_PUT_RANGE", "130013");
		Utils.RegisterObject(L, translator, -4, "INCLUDE_BUILDING", "130014");
		Utils.RegisterObject(L, translator, -4, "INCLUDE_OTHER_BOARD", "130016");
		Utils.RegisterObject(L, translator, -4, "INCLUDE_OTHER_ROAD", "130016");
		Utils.RegisterObject(L, translator, -4, "INCLUDE_MY_ROAD", "130017");
		Utils.RegisterObject(L, translator, -4, "THIS_UPGRADING", "130018");
		Utils.RegisterObject(L, translator, -4, "UNDO", "110106");
		Utils.RegisterObject(L, translator, -4, "RANDOM", "130019");
		Utils.RegisterObject(L, translator, -4, "NEW_BASE_RESOURCE_PERCENT", "130020");
		Utils.RegisterObject(L, translator, -4, "INCLUDE_OTHER_MINERANGE_POINT", "130009");
		Utils.RegisterObject(L, translator, -4, "OUT_UNLOCK_RANGE_REASON", "130022");
		Utils.RegisterObject(L, translator, -4, "UNCONNECT_BOARD_REASON", "130023");
		Utils.RegisterObject(L, translator, -4, "UNCONNECT_ROAD_REASON", "130023");
		Utils.RegisterObject(L, translator, -4, "ONLY_IN_INSIDE", "120230");
		Utils.RegisterObject(L, translator, -4, "ONLY_OUT_INSIDE", "120231");
		Utils.RegisterObject(L, translator, -4, "ONLY_IN_MAIN_INSIDE", "120232");
		Utils.RegisterObject(L, translator, -4, "NOT_BUILD_ON_BASE_EXPANSION", "120233");
		Utils.RegisterObject(L, translator, -4, "NOT_BUILD_ON_WORLD_RESOURCE", "120234");
		Utils.RegisterObject(L, translator, -4, "ONLY_BUILD_ROAD", "120954");
		Utils.RegisterObject(L, translator, -4, "BUILD_UN_CONNECT", "120955");
		Utils.RegisterObject(L, translator, -4, "NEED_FIRST_BUILD", "170385");
		Utils.RegisterObject(L, translator, -4, "RESET", "150116");
		Utils.RegisterObject(L, translator, -4, "MONSTER", "100353");
		Utils.RegisterObject(L, translator, -4, "COLLECT_RESOURCE_DESTROY", "120006");
		Utils.RegisterObject(L, translator, -4, "NO_PUT_ROAD", "130212");
		Utils.RegisterObject(L, translator, -4, "GOTO", "110003");
		Utils.RegisterObject(L, translator, -4, "ROAD_UN_CONNECT", "130606");
		Utils.RegisterObject(L, translator, -4, "ROAD_REACH_BUILD_MAX", "100555");
		Utils.RegisterObject(L, translator, -4, "LEFT", "310134");
		Utils.RegisterObject(L, translator, -4, "GUIDE_BUILD_ROAD", "130212");
		Utils.RegisterObject(L, translator, -4, "NO_ARRIVE_FOG", "300716");
		Utils.RegisterObject(L, translator, -4, "CONFIRM", "110006");
		Utils.RegisterObject(L, translator, -4, "CANCEL", "110106");
		Utils.RegisterObject(L, translator, -4, "WEREWOLF", "season_s4_activity_1200011_name");
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
				GameDialogDefine o = new GameDialogDefine();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameDialogDefine constructor!");
	}
}
