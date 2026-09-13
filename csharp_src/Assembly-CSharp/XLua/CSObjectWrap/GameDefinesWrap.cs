using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameDefinesWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameDefines);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 72, 11, 11);
		Utils.RegisterObject(L, translator, -4, "ScreenScaler", GameDefines.ScreenScaler);
		Utils.RegisterObject(L, translator, -4, "NoChannelParam", "FB_BTN_youjian");
		Utils.RegisterObject(L, translator, -4, "UndisposedMail", "no_mail_channel");
		Utils.RegisterObject(L, translator, -4, "GreenBlockFree", "Assets/Main/Sprites/Scene/road/green_block_free");
		Utils.RegisterObject(L, translator, -4, "RedBlockFree", "Assets/Main/Sprites/Scene/road/red_block_free");
		Utils.RegisterObject(L, translator, -4, "GreenFreeMoveKuang", "free_move_kuang_green");
		Utils.RegisterObject(L, translator, -4, "RedFreeMoveKuang", "free_move_kuang_red");
		Utils.RegisterObject(L, translator, -4, "DefaultDialog", "Assets/Main/Localization/English/Dictionaries/Dialog.txt");
		Utils.RegisterObject(L, translator, -4, "EQUIP_SOLTCOUNT", 6);
		Utils.RegisterObject(L, translator, -4, "AreaXSize", 8);
		Utils.RegisterObject(L, translator, -4, "AreaYSize", 8);
		Utils.RegisterObject(L, translator, -4, "TileXInArea", 8);
		Utils.RegisterObject(L, translator, -4, "TileYInArea", 8);
		Utils.RegisterObject(L, translator, -4, "MaxShowBuildBlockRange", 20);
		Utils.RegisterObject(L, translator, -4, "ITEM_TYPE_SPD", 2);
		Utils.RegisterObject(L, translator, -4, "MAIN_CITY_ID", "10000");
		Utils.RegisterObject(L, translator, -4, "MONTH_CARD_ID", "9007");
		Utils.RegisterObject(L, translator, -4, "SUB_MONTH_CARD_ID", "9012");
		Utils.RegisterObject(L, translator, -4, "MONTH_CARD_REWARD_COUNT", 10);
		Utils.RegisterObject(L, translator, -4, "KINGDOM_KING_ID", "216000");
		Utils.RegisterObject(L, translator, -4, "GREAT_KINGDOM_KING_ID", "222000");
		Utils.RegisterObject(L, translator, -4, "INTRODE_KING_ID", "216017");
		Utils.RegisterObject(L, translator, -4, "FIRST_GIVE_HERO_ID", "10009");
		Utils.RegisterObject(L, translator, -4, "SECOND_GIVE_HERO_ID", "10010");
		Utils.RegisterObject(L, translator, -4, "ITEM_GENE", 212005);
		Utils.RegisterObject(L, translator, -4, "ITEM_RENAME", 200021);
		Utils.RegisterObject(L, translator, -4, "THRONE_ID", 48);
		Utils.RegisterObject(L, translator, -4, "THRONE_POINT_ID", 497500);
		Utils.RegisterObject(L, translator, -4, "FLT_EPSILON", 1.1920929E-07f);
		Utils.RegisterObject(L, translator, -4, "WB_SEASON_ACTIVITY", "57062");
		Utils.RegisterObject(L, translator, -4, "WB_DECLARE_ACTIVITY", "57088");
		Utils.RegisterObject(L, translator, -4, "ALLIANCE_NOTICE_KEY", "notice_0123456789");
		Utils.RegisterObject(L, translator, -4, "MIN_NAME_CHAR", 3);
		Utils.RegisterObject(L, translator, -4, "MAX_NAME_CHAR", 16);
		Utils.RegisterObject(L, translator, -4, "MAX_MOOD_CHAR", 50);
		Utils.RegisterObject(L, translator, -4, "FBLuckyDrawKey", "fbluckydrawkey");
		Utils.RegisterObject(L, translator, -4, "DefaultMissile", "53301");
		Utils.RegisterObject(L, translator, -4, "RandomTruckTargetMin", 3);
		Utils.RegisterObject(L, translator, -4, "RandomTruckInterval", 3f);
		Utils.RegisterObject(L, translator, -4, "RandomPeopleTargetMin", 2);
		Utils.RegisterObject(L, translator, -4, "RandomPeopleInterval", 5f);
		Utils.RegisterObject(L, translator, -4, "RoadRobotWorkHeight", 0f);
		Utils.RegisterObject(L, translator, -4, "BuildRobotRotationSpeed", 270f);
		Utils.RegisterObject(L, translator, -4, "BuildRobotFlyMaxSpeed", 15f);
		Utils.RegisterObject(L, translator, -4, "BuildRobotTakeOffHeight", 4.81f);
		Utils.RegisterObject(L, translator, -4, "BuildRobotWorkSpeed", 4f);
		Utils.RegisterObject(L, translator, -4, "BuildRobotAcceleration", 12f);
		Utils.RegisterObject(L, translator, -4, "BuildRobotApproachTime", 1f);
		Utils.RegisterObject(L, translator, -4, "QualityLevel_Off", 0);
		Utils.RegisterObject(L, translator, -4, "QualityLevel_Low", 1);
		Utils.RegisterObject(L, translator, -4, "QualityLevel_Middle", 2);
		Utils.RegisterObject(L, translator, -4, "QualityLevel_High", 3);
		Utils.RegisterObject(L, translator, -4, "ShaderLODHigh", "_LOD_HIGH");
		Utils.RegisterObject(L, translator, -4, "ShaderLODMiddle", "_LOD_MIDDLE");
		Utils.RegisterObject(L, translator, -4, "RenderScaleLow", 0.8f);
		Utils.RegisterObject(L, translator, -4, "MummySoldierMinId", 12001);
		Utils.RegisterObject(L, translator, -4, "MummySoldierMaxId", 12011);
		Utils.RegisterObject(L, translator, -4, "GrayMaterialName", "SpriteGray");
		Utils.RegisterObject(L, translator, -4, "LookAtFocusTime", 0.4f);
		Utils.RegisterObject(L, translator, -4, "FindCanBuildPointRange", 7);
		Utils.RegisterObject(L, translator, -4, "SaveGuideDoneValue", "1");
		Utils.RegisterObject(L, translator, -4, "FirstLaunchFlag", "FirstLaunchFlag");
		Utils.RegisterObject(L, translator, -4, "FirstLaunch", 1);
		Utils.RegisterObject(L, translator, -4, "NormalLaunch", 2);
		Utils.RegisterObject(L, translator, -4, "FirstLaunchSkipUpdateFlag", "FirstLaunchSkipUpdateFlag");
		Utils.RegisterObject(L, translator, -4, "FirstLaunchSkipUpdateDefault", 0);
		Utils.RegisterObject(L, translator, -4, "FirstLaunchSkipUpdateRunning", 1);
		Utils.RegisterObject(L, translator, -4, "FirstLaunchSkipUpdateDisable", 2);
		Utils.RegisterObject(L, translator, -4, "Int32Bit", 32);
		Utils.RegisterObject(L, translator, -4, "ByteSize", 8);
		Utils.RegisterObject(L, translator, -4, "Player_CareerID", "222000");
		Utils.RegisterFunc(L, -2, "GuideBoardPos", _g_get_GuideBoardPos);
		Utils.RegisterFunc(L, -2, "WorldCityNameBGScale", _g_get_WorldCityNameBGScale);
		Utils.RegisterFunc(L, -2, "SkyBoxPosZ", _g_get_SkyBoxPosZ);
		Utils.RegisterFunc(L, -2, "SkyBoxPosZFreeGrass", _g_get_SkyBoxPosZFreeGrass);
		Utils.RegisterFunc(L, -2, "BlockPos", _g_get_BlockPos);
		Utils.RegisterFunc(L, -2, "isArabicTMPFix", _g_get_isArabicTMPFix);
		Utils.RegisterFunc(L, -2, "BuildTileCenterDelta", _g_get_BuildTileCenterDelta);
		Utils.RegisterFunc(L, -2, "BuildConnectList", _g_get_BuildConnectList);
		Utils.RegisterFunc(L, -2, "ConnectDirList", _g_get_ConnectDirList);
		Utils.RegisterFunc(L, -2, "OffsetToDirectionMap", _g_get_OffsetToDirectionMap);
		Utils.RegisterFunc(L, -2, "LandLockPosOffset", _g_get_LandLockPosOffset);
		Utils.RegisterFunc(L, -1, "GuideBoardPos", _s_set_GuideBoardPos);
		Utils.RegisterFunc(L, -1, "WorldCityNameBGScale", _s_set_WorldCityNameBGScale);
		Utils.RegisterFunc(L, -1, "SkyBoxPosZ", _s_set_SkyBoxPosZ);
		Utils.RegisterFunc(L, -1, "SkyBoxPosZFreeGrass", _s_set_SkyBoxPosZFreeGrass);
		Utils.RegisterFunc(L, -1, "BlockPos", _s_set_BlockPos);
		Utils.RegisterFunc(L, -1, "isArabicTMPFix", _s_set_isArabicTMPFix);
		Utils.RegisterFunc(L, -1, "BuildTileCenterDelta", _s_set_BuildTileCenterDelta);
		Utils.RegisterFunc(L, -1, "BuildConnectList", _s_set_BuildConnectList);
		Utils.RegisterFunc(L, -1, "ConnectDirList", _s_set_ConnectDirList);
		Utils.RegisterFunc(L, -1, "OffsetToDirectionMap", _s_set_OffsetToDirectionMap);
		Utils.RegisterFunc(L, -1, "LandLockPosOffset", _s_set_LandLockPosOffset);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "GameDefines does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GuideBoardPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.GuideBoardPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldCityNameBGScale(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, GameDefines.WorldCityNameBGScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkyBoxPosZ(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GameDefines.SkyBoxPosZ);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SkyBoxPosZFreeGrass(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GameDefines.SkyBoxPosZFreeGrass);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BlockPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, GameDefines.BlockPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isArabicTMPFix(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GameDefines.isArabicTMPFix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BuildTileCenterDelta(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.BuildTileCenterDelta);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_BuildConnectList(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.BuildConnectList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ConnectDirList(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.ConnectDirList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OffsetToDirectionMap(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GameDefines.OffsetToDirectionMap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LandLockPosOffset(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, GameDefines.LandLockPosOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GuideBoardPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			GameDefines.GuideBoardPos = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_WorldCityNameBGScale(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			GameDefines.WorldCityNameBGScale = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SkyBoxPosZ(IntPtr L)
	{
		try
		{
			GameDefines.SkyBoxPosZ = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_SkyBoxPosZFreeGrass(IntPtr L)
	{
		try
		{
			GameDefines.SkyBoxPosZFreeGrass = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BlockPos(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			GameDefines.BlockPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isArabicTMPFix(IntPtr L)
	{
		try
		{
			GameDefines.isArabicTMPFix = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BuildTileCenterDelta(IntPtr L)
	{
		try
		{
			GameDefines.BuildTileCenterDelta = (Vector3[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Vector3[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_BuildConnectList(IntPtr L)
	{
		try
		{
			GameDefines.BuildConnectList = (List<GameDefines.BuildConnectRoadDirection>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(List<GameDefines.BuildConnectRoadDirection>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_ConnectDirList(IntPtr L)
	{
		try
		{
			GameDefines.ConnectDirList = (List<GameDefines.DirectionType>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(List<GameDefines.DirectionType>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OffsetToDirectionMap(IntPtr L)
	{
		try
		{
			GameDefines.OffsetToDirectionMap = (Dictionary<Vector2Int, Direction>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<Vector2Int, Direction>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LandLockPosOffset(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			GameDefines.LandLockPosOffset = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
