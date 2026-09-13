using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class PathUtilsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(PathUtils);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 22, 0, 0);
		Utils.RegisterFunc(L, -4, "GetBuildingNeighbors", _m_GetBuildingNeighbors_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetNeighborDir", _m_GetNeighborDir_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetSortedBuildingNeighbors", _m_GetSortedBuildingNeighbors_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsWalkAble", _m_IsWalkAble_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsTruckRoad", _m_IsTruckRoad_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsRoad", _m_IsRoad_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTruckPathString", _m_GetTruckPathString_xlua_st_);
		Utils.RegisterFunc(L, -4, "StringToPathInfos", _m_StringToPathInfos_xlua_st_);
		Utils.RegisterFunc(L, -4, "StringToPoint", _m_StringToPoint_xlua_st_);
		Utils.RegisterFunc(L, -4, "CalRobotMoveNeedTime", _m_CalRobotMoveNeedTime_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetNearestMainOutPos", _m_GetNearestMainOutPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTruckPathMainOutPosList", _m_GetTruckPathMainOutPosList_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindRoadMainBuildToOutPos", _m_FindRoadMainBuildToOutPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetTruckPathEndPos", _m_GetTruckPathEndPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindRoadOutPosToMainBuild", _m_FindRoadOutPosToMainBuild_xlua_st_);
		Utils.RegisterFunc(L, -4, "FindMainBuildRoadRoundPath", _m_FindMainBuildRoadRoundPath_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPeopleNearestMainOutPos", _m_GetPeopleNearestMainOutPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetPeopleMainAroundPath", _m_GetPeopleMainAroundPath_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetDroneCenterPos", _m_GetDroneCenterPos_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRoadRobotWorkStartPosForLua", _m_GetRoadRobotWorkStartPosForLua_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRoadRobotWorkStartPos", _m_GetRoadRobotWorkStartPos_xlua_st_);
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
				PathUtils o = new PathUtils();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PathUtils constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetBuildingNeighbors_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && (LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) || Lua.lua_isint64(L, 1)))
			{
				Vector2Int[] buildingNeighbors = PathUtils.GetBuildingNeighbors(Lua.lua_toint64(L, 1));
				objectTranslator.Push(L, buildingNeighbors);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				int itemId = Lua.xlua_tointeger(L, 1);
				int pointId = Lua.xlua_tointeger(L, 2);
				Vector2Int[] buildingNeighbors2 = PathUtils.GetBuildingNeighbors(itemId, pointId);
				objectTranslator.Push(L, buildingNeighbors2);
				return 1;
			}
			if (num == 1 && objectTranslator.Assignable<LuaBuildData>(L, 1))
			{
				List<Vector2Int> entries;
				List<Vector2Int> buildingNeighbors3 = PathUtils.GetBuildingNeighbors((LuaBuildData)objectTranslator.GetObject(L, 1, typeof(LuaBuildData)), out entries);
				objectTranslator.Push(L, buildingNeighbors3);
				objectTranslator.Push(L, entries);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PathUtils.GetBuildingNeighbors!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNeighborDir_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			Vector2Int vector2Int = PathUtils.GetNeighborDir(tiles: Lua.xlua_tointeger(L, 3), pos: v, buildPos: v2);
			objectTranslator.Push(L, vector2Int);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSortedBuildingNeighbors_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector2Int>(L, 3))
			{
				int itemId = Lua.xlua_tointeger(L, 1);
				int pointId = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector2Int v);
				Vector2Int[] sortedBuildingNeighbors = PathUtils.GetSortedBuildingNeighbors(itemId, pointId, v);
				objectTranslator.Push(L, sortedBuildingNeighbors);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<LuaBuildData>(L, 1) && objectTranslator.Assignable<Vector2Int>(L, 2) && objectTranslator.Assignable<Vector2Int[]>(L, 3))
			{
				LuaBuildData buildingDate = (LuaBuildData)objectTranslator.GetObject(L, 1, typeof(LuaBuildData));
				objectTranslator.Get(L, 2, out Vector2Int v2);
				Vector2Int[] o = PathUtils.GetSortedBuildingNeighbors(neighbors: (Vector2Int[])objectTranslator.GetObject(L, 3, typeof(Vector2Int[])), buildingDate: buildingDate, startPos: v2);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PathUtils.GetSortedBuildingNeighbors!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsWalkAble_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			objectTranslator.Get(L, 3, out FindPathType v3);
			bool value = PathUtils.IsWalkAble(v, v2, v3);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsTruckRoad_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			bool value = PathUtils.IsTruckRoad(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsRoad_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			bool value = PathUtils.IsRoad(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTruckPathString_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			long targetBuildingUuid = Lua.lua_toint64(L, 1);
			Dictionary<ResourceType, long> resDict = (Dictionary<ResourceType, long>)objectTranslator.GetObject(L, 2, typeof(Dictionary<ResourceType, long>));
			Action<string> onComplete = objectTranslator.GetDelegate<Action<string>>(L, 3);
			PathUtils.GetTruckPathString(targetBuildingUuid, resDict, onComplete);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringToPathInfos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<PathInfo> o = PathUtils.StringToPathInfos(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_StringToPoint_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			List<Vector2Int> o = PathUtils.StringToPoint(Lua.lua_tostring(L, 1));
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalRobotMoveNeedTime_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			objectTranslator.Get(L, 2, out Vector3 val2);
			float num = PathUtils.CalRobotMoveNeedTime(val, val2);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetNearestMainOutPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			Vector2Int nearestMainOutPos = PathUtils.GetNearestMainOutPos(v);
			objectTranslator.Push(L, nearestMainOutPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTruckPathMainOutPosList_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 0)
			{
				List<Vector2Int> truckPathMainOutPosList = PathUtils.GetTruckPathMainOutPosList();
				objectTranslator.Push(L, truckPathMainOutPosList);
				return 1;
			}
			if (num == 0)
			{
				List<Vector2Int> entries;
				List<Vector2Int> truckPathMainOutPosList2 = PathUtils.GetTruckPathMainOutPosList(out entries);
				objectTranslator.Push(L, truckPathMainOutPosList2);
				objectTranslator.Push(L, entries);
				return 2;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to PathUtils.GetTruckPathMainOutPosList!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindRoadMainBuildToOutPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			List<Vector2Int> o = PathUtils.FindRoadMainBuildToOutPos(v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetTruckPathEndPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2Int truckPathEndPos = PathUtils.GetTruckPathEndPos();
			objectTranslator.Push(L, truckPathEndPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindRoadOutPosToMainBuild_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			List<Vector2Int> o = PathUtils.FindRoadOutPosToMainBuild(v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindMainBuildRoadRoundPath_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			objectTranslator.Get(L, 2, out Vector2Int v2);
			List<Vector2Int> o = PathUtils.FindMainBuildRoadRoundPath(v, v2);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPeopleNearestMainOutPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2Int[] targetPosNeighbors = (Vector2Int[])objectTranslator.GetObject(L, 1, typeof(Vector2Int[]));
			objectTranslator.Get(L, 2, out Vector2Int v);
			Vector2Int[] peopleNearestMainOutPos = PathUtils.GetPeopleNearestMainOutPos(targetPosNeighbors, v);
			objectTranslator.Push(L, peopleNearestMainOutPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPeopleMainAroundPath_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector2Int v);
			List<Vector2Int> peopleMainAroundPath = PathUtils.GetPeopleMainAroundPath(v);
			objectTranslator.Push(L, peopleMainAroundPath);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDroneCenterPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out Vector3 val);
			Vector3 droneCenterPos = PathUtils.GetDroneCenterPos(val);
			objectTranslator.PushUnityEngineVector3(L, droneCenterPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRoadRobotWorkStartPosForLua_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 roadRobotWorkStartPosForLua = PathUtils.GetRoadRobotWorkStartPosForLua((LuaTable)objectTranslator.GetObject(L, 1, typeof(LuaTable)));
			objectTranslator.PushUnityEngineVector3(L, roadRobotWorkStartPosForLua);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRoadRobotWorkStartPos_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector3 roadRobotWorkStartPos = PathUtils.GetRoadRobotWorkStartPos((List<int>)objectTranslator.GetObject(L, 1, typeof(List<int>)));
			objectTranslator.PushUnityEngineVector3(L, roadRobotWorkStartPos);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
