using System;
using System.Collections.Generic;
using UnityEngine;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldStaticManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldStaticManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 54, 7, 4);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "UnInit", _m_UnInit);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "RemoveBlackDesert", _m_RemoveBlackDesert);
		Utils.RegisterFunc(L, -3, "IsTileWalkable", _m_IsTileWalkable);
		Utils.RegisterFunc(L, -3, "AddOccupyPoints", _m_AddOccupyPoints);
		Utils.RegisterFunc(L, -3, "RemoveOccupyPoints", _m_RemoveOccupyPoints);
		Utils.RegisterFunc(L, -3, "IsOccupied", _m_IsOccupied);
		Utils.RegisterFunc(L, -3, "IsObstacle", _m_IsObstacle);
		Utils.RegisterFunc(L, -3, "GreenAreaChange", _m_GreenAreaChange);
		Utils.RegisterFunc(L, -3, "UpdateGreenArea", _m_UpdateGreenArea);
		Utils.RegisterFunc(L, -3, "SetViewDirty", _m_SetViewDirty);
		Utils.RegisterFunc(L, -3, "NeedPreLoadS2Terrain", _m_NeedPreLoadS2Terrain);
		Utils.RegisterFunc(L, -3, "SetS2TerrainPreLoadAsset", _m_SetS2TerrainPreLoadAsset);
		Utils.RegisterFunc(L, -3, "SetS2TerrainPreLoadAssetLoadFinish", _m_SetS2TerrainPreLoadAssetLoadFinish);
		Utils.RegisterFunc(L, -3, "LoadTerrainAssets", _m_LoadTerrainAssets);
		Utils.RegisterFunc(L, -3, "OnSkinChange", _m_OnSkinChange);
		Utils.RegisterFunc(L, -3, "InitBlackBlock", _m_InitBlackBlock);
		Utils.RegisterFunc(L, -3, "GetDragonLandRangeObj", _m_GetDragonLandRangeObj);
		Utils.RegisterFunc(L, -3, "CreateDragonLandRange", _m_CreateDragonLandRange);
		Utils.RegisterFunc(L, -3, "RemoveDragonLandRange", _m_RemoveDragonLandRange);
		Utils.RegisterFunc(L, -3, "RemoveDragonLandPoint", _m_RemoveDragonLandPoint);
		Utils.RegisterFunc(L, -3, "ChangeTerrainMat", _m_ChangeTerrainMat);
		Utils.RegisterFunc(L, -3, "ChangeTerrain", _m_ChangeTerrain);
		Utils.RegisterFunc(L, -3, "ChunkCoordToTilePos", _m_ChunkCoordToTilePos);
		Utils.RegisterFunc(L, -3, "OnDrawGizmosTerrainS2", _m_OnDrawGizmosTerrainS2);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -3, "GetProfileTerrainSwitch", _m_GetProfileTerrainSwitch);
		Utils.RegisterFunc(L, -3, "ProfileToggleTerrain", _m_ProfileToggleTerrain);
		Utils.RegisterFunc(L, -3, "GetProfileSwitch", _m_GetProfileSwitch);
		Utils.RegisterFunc(L, -3, "CreateS4FogSystem", _m_CreateS4FogSystem);
		Utils.RegisterFunc(L, -3, "UnloadS4FogSystem", _m_UnloadS4FogSystem);
		Utils.RegisterFunc(L, -3, "IsDawn", _m_IsDawn);
		Utils.RegisterFunc(L, -3, "LoadDecoration", _m_LoadDecoration);
		Utils.RegisterFunc(L, -3, "SetDecorationInvisibleRect", _m_SetDecorationInvisibleRect);
		Utils.RegisterFunc(L, -3, "LoadAllRegionsAsset", _m_LoadAllRegionsAsset);
		Utils.RegisterFunc(L, -3, "UnloadAllRegionsAsset", _m_UnloadAllRegionsAsset);
		Utils.RegisterFunc(L, -3, "UnloadSomeRegionsData", _m_UnloadSomeRegionsData);
		Utils.RegisterFunc(L, -3, "UnloadRegionAsset", _m_UnloadRegionAsset);
		Utils.RegisterFunc(L, -3, "GetCurrentCameraRegionCoord", _m_GetCurrentCameraRegionCoord);
		Utils.RegisterFunc(L, -3, "LoadRegionAsset", _m_LoadRegionAsset);
		Utils.RegisterFunc(L, -3, "GetCurrentRegionCount", _m_GetCurrentRegionCount);
		Utils.RegisterFunc(L, -3, "LoadRenderAsset", _m_LoadRenderAsset);
		Utils.RegisterFunc(L, -3, "UnInit_S5", _m_UnInit_S5);
		Utils.RegisterFunc(L, -3, "LoadWorldFog_S5", _m_LoadWorldFog_S5);
		Utils.RegisterFunc(L, -3, "UpdateTerrainObject_S5", _m_UpdateTerrainObject_S5);
		Utils.RegisterFunc(L, -3, "InitS5TerrainMatAssetInfo", _m_InitS5TerrainMatAssetInfo);
		Utils.RegisterFunc(L, -3, "GetTerrainMatAsset", _m_GetTerrainMatAsset);
		Utils.RegisterFunc(L, -3, "TryHideTerrain_S5", _m_TryHideTerrain_S5);
		Utils.RegisterFunc(L, -3, "CanJumpUpdateDecoration", _m_CanJumpUpdateDecoration);
		Utils.RegisterFunc(L, -3, "IsObstacle_S5", _m_IsObstacle_S5);
		Utils.RegisterFunc(L, -3, "SetKingCityObstacle_S5", _m_SetKingCityObstacle_S5);
		Utils.RegisterFunc(L, -3, "IsInKingCityRange", _m_IsInKingCityRange);
		Utils.RegisterFunc(L, -3, "InitBlackBlock_S5", _m_InitBlackBlock_S5);
		Utils.RegisterFunc(L, -2, "ObstaclesSet", _g_get_ObstaclesSet);
		Utils.RegisterFunc(L, -2, "TerrainPrefabName", _g_get_TerrainPrefabName);
		Utils.RegisterFunc(L, -2, "CurSeasonType", _g_get_CurSeasonType);
		Utils.RegisterFunc(L, -2, "mIsNineNationMode", _g_get_mIsNineNationMode);
		Utils.RegisterFunc(L, -2, "mCurrentTerrain_HightQuality", _g_get_mCurrentTerrain_HightQuality);
		Utils.RegisterFunc(L, -2, "regionsMap_S5", _g_get_regionsMap_S5);
		Utils.RegisterFunc(L, -2, "mRegionChunksCache", _g_get_mRegionChunksCache);
		Utils.RegisterFunc(L, -1, "mIsNineNationMode", _s_set_mIsNineNationMode);
		Utils.RegisterFunc(L, -1, "mCurrentTerrain_HightQuality", _s_set_mCurrentTerrain_HightQuality);
		Utils.RegisterFunc(L, -1, "regionsMap_S5", _s_set_regionsMap_S5);
		Utils.RegisterFunc(L, -1, "mRegionChunksCache", _s_set_mRegionChunksCache);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 1, 1);
		Utils.RegisterFunc(L, -4, "GetGuidRange", _m_GetGuidRange_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetServerIdFromGuid", _m_GetServerIdFromGuid_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTileCoordServerIndex", _m_GetTileCoordServerIndex_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRegionCoordFromTileCoord", _m_GetRegionCoordFromTileCoord_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetServerIndexFromRegionCoord", _m_GetServerIndexFromRegionCoord_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetServerIndexFromChunkCoord", _m_GetServerIndexFromChunkCoord_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "TileCountPerRegion", 500);
		Utils.RegisterObject(L, translator, -4, "BlockBetweenServerWidth", 4);
		Utils.RegisterObject(L, translator, -4, "BlockBetweenCenterWidth", 6);
		Utils.RegisterObject(L, translator, -4, "BlockBetweenCenterWidth_Big", 5);
		Utils.RegisterObject(L, translator, -4, "BlockBetweenCenterWidth_Small", 2);
		Utils.RegisterFunc(L, -2, "RegionWidth", _g_get_RegionWidth);
		Utils.RegisterFunc(L, -1, "RegionWidth", _s_set_RegionWidth);
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
				WorldStaticManager o = new WorldStaticManager((WorldScene)objectTranslator.GetObject(L, 2, typeof(WorldScene)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldStaticManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit();
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
			WorldStaticManager obj = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_RemoveBlackDesert(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveBlackDesert();
			return 0;
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
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				bool value = worldStaticManager.IsTileWalkable(v);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				bool value2 = worldStaticManager.IsTileWalkable(val);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldStaticManager.IsTileWalkable!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddOccupyPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int serverId = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out Vector2Int v2);
			worldStaticManager.AddOccupyPoints(v, serverId, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveOccupyPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			int serverId = Lua.xlua_tointeger(L, 3);
			objectTranslator.Get(L, 4, out Vector2Int v2);
			worldStaticManager.RemoveOccupyPoints(v, serverId, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsOccupied(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			bool value = worldStaticManager.IsOccupied(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsObstacle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				bool value = worldStaticManager.IsObstacle(v);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector2Int>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector2Int v2);
				int serverIndex = Lua.xlua_tointeger(L, 3);
				bool value2 = worldStaticManager.IsObstacle(v2, serverIndex);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldStaticManager.IsObstacle!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GreenAreaChange(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldAreaGreenInfo.GreenType v);
			HashSet<int> changePoints = (HashSet<int>)objectTranslator.GetObject(L, 3, typeof(HashSet<int>));
			worldStaticManager.GreenAreaChange(v, changePoints);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGreenArea(IntPtr L)
	{
		try
		{
			WorldStaticManager obj = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_SetViewDirty(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetViewDirty();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_NeedPreLoadS2Terrain(IntPtr L)
	{
		try
		{
			bool value = ((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).NeedPreLoadS2Terrain();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetS2TerrainPreLoadAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			Asset matAsset = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
			worldStaticManager.SetS2TerrainPreLoadAsset(path, matAsset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetS2TerrainPreLoadAssetLoadFinish(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			string path = Lua.lua_tostring(L, 2);
			Asset matAsset = (Asset)objectTranslator.GetObject(L, 3, typeof(Asset));
			worldStaticManager.SetS2TerrainPreLoadAssetLoadFinish(path, matAsset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadTerrainAssets(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			Action callback = objectTranslator.GetDelegate<Action>(L, 2);
			worldStaticManager.LoadTerrainAssets(callback);
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnSkinChange();
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitBlackBlock();
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
			GameObject dragonLandRangeObj = ((WorldStaticManager)objectTranslator.FastGetCSObj(L, 1)).GetDragonLandRangeObj();
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateDragonLandRange();
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveDragonLandRange();
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
			WorldStaticManager obj = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_ChangeTerrainMat(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeTerrainMat();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChangeTerrain(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ChangeTerrain();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ChunkCoordToTilePos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			Vector2Int vector2Int = worldStaticManager.ChunkCoordToTilePos(v);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrawGizmosTerrainS2(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmosTerrainS2();
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
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
			bool profileTerrainSwitch = ((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetProfileTerrainSwitch();
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
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ProfileToggleTerrain();
			return 0;
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
			bool profileSwitch = ((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetProfileSwitch();
			Lua.lua_pushboolean(L, profileSwitch);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateS4FogSystem(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateS4FogSystem();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadS4FogSystem(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadS4FogSystem();
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
			WorldStaticManager obj = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_LoadDecoration(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoadDecoration();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDecorationInvisibleRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldStaticManager.DecorationInvisibleRectType v);
			objectTranslator.Get(L, 3, out Vector3 val);
			float radius = (float)Lua.lua_tonumber(L, 4);
			worldStaticManager.SetDecorationInvisibleRect(v, val, radius);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGuidRange_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2Int guidRange = WorldStaticManager.GetGuidRange(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, guidRange);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerIdFromGuid_xlua_st_(IntPtr L)
	{
		try
		{
			int serverIdFromGuid = WorldStaticManager.GetServerIdFromGuid(Lua.xlua_tointeger(L, 1));
			Lua.xlua_pushinteger(L, serverIdFromGuid);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadAllRegionsAsset(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoadAllRegionsAsset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadAllRegionsAsset(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadAllRegionsAsset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadSomeRegionsData(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnloadSomeRegionsData();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnloadRegionAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			worldStaticManager.UnloadRegionAsset(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentCameraRegionCoord(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<Vector2Int> currentCameraRegionCoord = ((WorldStaticManager)objectTranslator.FastGetCSObj(L, 1)).GetCurrentCameraRegionCoord();
			objectTranslator.Push(L, currentCameraRegionCoord);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadRegionAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			worldStaticManager.LoadRegionAsset(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCurrentRegionCount(IntPtr L)
	{
		try
		{
			int currentRegionCount = ((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetCurrentRegionCount();
			Lua.xlua_pushinteger(L, currentRegionCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadRenderAsset(IntPtr L)
	{
		try
		{
			WorldStaticManager obj = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int guid = Lua.xlua_tointeger(L, 2);
			obj.LoadRenderAsset(guid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnInit_S5(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UnInit_S5();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTileCoordServerIndex_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int width = Lua.xlua_tointeger(L, 1);
			int height = Lua.xlua_tointeger(L, 2);
			objectTranslator.Get(L, 3, out Vector2Int v);
			int tileCoordServerIndex = WorldStaticManager.GetTileCoordServerIndex(width, height, v);
			Lua.xlua_pushinteger(L, tileCoordServerIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRegionCoordFromTileCoord_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			Vector2Int regionCoordFromTileCoord = WorldStaticManager.GetRegionCoordFromTileCoord(v);
			objectTranslator.Push(L, regionCoordFromTileCoord);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerIndexFromRegionCoord_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			int serverIndexFromRegionCoord = WorldStaticManager.GetServerIndexFromRegionCoord(v);
			Lua.xlua_pushinteger(L, serverIndexFromRegionCoord);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerIndexFromChunkCoord_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			int serverIndexFromChunkCoord = WorldStaticManager.GetServerIndexFromChunkCoord(v);
			Lua.xlua_pushinteger(L, serverIndexFromChunkCoord);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadWorldFog_S5(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LoadWorldFog_S5();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTerrainObject_S5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<WorldCamera>(L, 2))
			{
				WorldCamera worldCamera = (WorldCamera)objectTranslator.GetObject(L, 2, typeof(WorldCamera));
				worldStaticManager.UpdateTerrainObject_S5(worldCamera);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				worldStaticManager.UpdateTerrainObject_S5(v);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldStaticManager.UpdateTerrainObject_S5!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitS5TerrainMatAssetInfo(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitS5TerrainMatAssetInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTerrainMatAsset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int serverIndex = Lua.xlua_tointeger(L, 2);
				Asset terrainMatAsset = worldStaticManager.GetTerrainMatAsset(serverIndex);
				objectTranslator.Push(L, terrainMatAsset);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<Vector2Int>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector2Int v);
				Material terrainMatAsset2 = worldStaticManager.GetTerrainMatAsset(v);
				objectTranslator.Push(L, terrainMatAsset2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldStaticManager.GetTerrainMatAsset!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryHideTerrain_S5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			object currentShowBg = objectTranslator.GetObject(L, 2, typeof(object));
			worldStaticManager.TryHideTerrain_S5(currentShowBg);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CanJumpUpdateDecoration(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			object jumpedState = objectTranslator.GetObject(L, 2, typeof(object));
			worldStaticManager.CanJumpUpdateDecoration(jumpedState);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsObstacle_S5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			bool value = worldStaticManager.IsObstacle_S5(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetKingCityObstacle_S5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out WorldMapGridRenderer.KingCity v);
			worldStaticManager.SetKingCityObstacle_S5(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsInKingCityRange(IntPtr L)
	{
		try
		{
			WorldStaticManager obj = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int x = Lua.xlua_tointeger(L, 2);
			int y = Lua.xlua_tointeger(L, 3);
			bool value = obj.IsInKingCityRange(x, y);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitBlackBlock_S5(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitBlackBlock_S5();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_ObstaclesSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldStaticManager.ObstaclesSet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TerrainPrefabName(IntPtr L)
	{
		try
		{
			WorldStaticManager worldStaticManager = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, worldStaticManager.TerrainPrefabName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_CurSeasonType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldStaticManager.CurSeasonType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mIsNineNationMode(IntPtr L)
	{
		try
		{
			WorldStaticManager worldStaticManager = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldStaticManager.mIsNineNationMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mCurrentTerrain_HightQuality(IntPtr L)
	{
		try
		{
			WorldStaticManager worldStaticManager = (WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldStaticManager.mCurrentTerrain_HightQuality);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RegionWidth(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, WorldStaticManager.RegionWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_regionsMap_S5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldStaticManager.regionsMap_S5);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mRegionChunksCache(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldStaticManager worldStaticManager = (WorldStaticManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldStaticManager.mRegionChunksCache);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mIsNineNationMode(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mIsNineNationMode = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mCurrentTerrain_HightQuality(IntPtr L)
	{
		try
		{
			((WorldStaticManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mCurrentTerrain_HightQuality = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RegionWidth(IntPtr L)
	{
		try
		{
			WorldStaticManager.RegionWidth = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_regionsMap_S5(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldStaticManager)objectTranslator.FastGetCSObj(L, 1)).regionsMap_S5 = (Dictionary<Vector2Int, WorldStaticManager.Region_S5>)objectTranslator.GetObject(L, 2, typeof(Dictionary<Vector2Int, WorldStaticManager.Region_S5>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mRegionChunksCache(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((WorldStaticManager)objectTranslator.FastGetCSObj(L, 1)).mRegionChunksCache = (Dictionary<Vector2Int, List<WorldDecorationChunkDesc>>)objectTranslator.GetObject(L, 2, typeof(Dictionary<Vector2Int, List<WorldDecorationChunkDesc>>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
