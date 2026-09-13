using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DCFogWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DCFog);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 18, 0, 0);
		Utils.RegisterFunc(L, -3, "CSInit", _m_CSInit);
		Utils.RegisterFunc(L, -3, "InitFogDataByCacheInWasteLand", _m_InitFogDataByCacheInWasteLand);
		Utils.RegisterFunc(L, -3, "InitAllPoints", _m_InitAllPoints);
		Utils.RegisterFunc(L, -3, "GetFogIndexByPointId", _m_GetFogIndexByPointId);
		Utils.RegisterFunc(L, -3, "GetCanOpenNeighbourFogIds", _m_GetCanOpenNeighbourFogIds);
		Utils.RegisterFunc(L, -3, "GetPointIdsByFogIndex", _m_GetPointIdsByFogIndex);
		Utils.RegisterFunc(L, -3, "IsUnlock", _m_IsUnlock);
		Utils.RegisterFunc(L, -3, "IsUnlockByFogId", _m_IsUnlockByFogId);
		Utils.RegisterFunc(L, -3, "GetAllFogData", _m_GetAllFogData);
		Utils.RegisterFunc(L, -3, "UnlockFog", _m_UnlockFog);
		Utils.RegisterFunc(L, -3, "GetCanUnlockPointByLine", _m_GetCanUnlockPointByLine);
		Utils.RegisterFunc(L, -3, "GetPointIdCenterByFogIndex", _m_GetPointIdCenterByFogIndex);
		Utils.RegisterFunc(L, -3, "GetCanUnlockFog", _m_GetCanUnlockFog);
		Utils.RegisterFunc(L, -3, "GetUnlockFlagByFogId", _m_GetUnlockFlagByFogId);
		Utils.RegisterFunc(L, -3, "GetFogIdByOffset", _m_GetFogIdByOffset);
		Utils.RegisterFunc(L, -3, "GetFogPositionByFogId", _m_GetFogPositionByFogId);
		Utils.RegisterFunc(L, -3, "GetSmallDirectionByPointId", _m_GetSmallDirectionByPointId);
		Utils.RegisterFunc(L, -3, "GetSpecialFogIdByFodIdAndDirection", _m_GetSpecialFogIdByFodIdAndDirection);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 3, 3);
		Utils.RegisterFunc(L, -2, "FogSizeX", _g_get_FogSizeX);
		Utils.RegisterFunc(L, -2, "FogSizeY", _g_get_FogSizeY);
		Utils.RegisterFunc(L, -2, "FogTileCount", _g_get_FogTileCount);
		Utils.RegisterFunc(L, -1, "FogSizeX", _s_set_FogSizeX);
		Utils.RegisterFunc(L, -1, "FogSizeY", _s_set_FogSizeY);
		Utils.RegisterFunc(L, -1, "FogTileCount", _s_set_FogTileCount);
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
				DCFog o = new DCFog();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DCFog constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CSInit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog dCFog = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject obj = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			dCFog.CSInit(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitFogDataByCacheInWasteLand(IntPtr L)
	{
		try
		{
			((DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitFogDataByCacheInWasteLand();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitAllPoints(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog dCFog = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			ISFSObject obj = (ISFSObject)objectTranslator.GetObject(L, 2, typeof(ISFSObject));
			dCFog.InitAllPoints(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFogIndexByPointId(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			int fogIndexByPointId = obj.GetFogIndexByPointId(pointId);
			Lua.xlua_pushinteger(L, fogIndexByPointId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCanOpenNeighbourFogIds(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog obj = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			List<int> canOpenNeighbourFogIds = obj.GetCanOpenNeighbourFogIds(fogId);
			objectTranslator.Push(L, canOpenNeighbourFogIds);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointIdsByFogIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog obj = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			List<int> pointIdsByFogIndex = obj.GetPointIdsByFogIndex(fogId);
			objectTranslator.Push(L, pointIdsByFogIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnlock(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsUnlock(pointId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsUnlockByFogId(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			bool value = obj.IsUnlockByFogId(fogId);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllFogData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BitArray allFogData = ((DCFog)objectTranslator.FastGetCSObj(L, 1)).GetAllFogData();
			objectTranslator.Push(L, allFogData);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnlockFog(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			obj.UnlockFog(fogId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCanUnlockPointByLine(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog dCFog = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			objectTranslator.Get(L, 3, out Vector2Int v2);
			int canUnlockPointByLine = dCFog.GetCanUnlockPointByLine(v, v2);
			Lua.xlua_pushinteger(L, canUnlockPointByLine);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointIdCenterByFogIndex(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			int sizeIndex = Lua.xlua_tointeger(L, 3);
			int pointIdCenterByFogIndex = obj.GetPointIdCenterByFogIndex(fogId, sizeIndex);
			Lua.xlua_pushinteger(L, pointIdCenterByFogIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCanUnlockFog(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Dictionary<int, Dictionary<int, int>> canUnlockFog = ((DCFog)objectTranslator.FastGetCSObj(L, 1)).GetCanUnlockFog();
			objectTranslator.Push(L, canUnlockFog);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetUnlockFlagByFogId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog obj = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			Dictionary<int, int> unlockFlagByFogId = obj.GetUnlockFlagByFogId(fogId);
			objectTranslator.Push(L, unlockFlagByFogId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFogIdByOffset(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int x = Lua.xlua_tointeger(L, 3);
			int y = Lua.xlua_tointeger(L, 4);
			int fogIdByOffset = obj.GetFogIdByOffset(index, x, y);
			Lua.xlua_pushinteger(L, fogIdByOffset);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetFogPositionByFogId(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DCFog obj = (DCFog)objectTranslator.FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			Vector3 fogPositionByFogId = obj.GetFogPositionByFogId(fogId);
			objectTranslator.PushUnityEngineVector3(L, fogPositionByFogId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSmallDirectionByPointId(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int pointId = Lua.xlua_tointeger(L, 2);
			int smallDirectionByPointId = obj.GetSmallDirectionByPointId(pointId);
			Lua.xlua_pushinteger(L, smallDirectionByPointId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetSpecialFogIdByFodIdAndDirection(IntPtr L)
	{
		try
		{
			DCFog obj = (DCFog)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int fogId = Lua.xlua_tointeger(L, 2);
			int direction = Lua.xlua_tointeger(L, 3);
			int specialFogIdByFodIdAndDirection = obj.GetSpecialFogIdByFodIdAndDirection(fogId, direction);
			Lua.xlua_pushinteger(L, specialFogIdByFodIdAndDirection);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FogSizeX(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, DCFog.FogSizeX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FogSizeY(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, DCFog.FogSizeY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_FogTileCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, DCFog.FogTileCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FogSizeX(IntPtr L)
	{
		try
		{
			DCFog.FogSizeX = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FogSizeY(IntPtr L)
	{
		try
		{
			DCFog.FogSizeY = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_FogTileCount(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector2Int v);
			DCFog.FogTileCount = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
