using System;
using System.Collections.Generic;
using LW_JPS;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LW_JPSJPSPathfindingWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(JPSPathfinding);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 3, 3);
		Utils.RegisterFunc(L, -3, "InitMap", _m_InitMap);
		Utils.RegisterFunc(L, -3, "UpdateMap", _m_UpdateMap);
		Utils.RegisterFunc(L, -3, "GetPath", _m_GetPath);
		Utils.RegisterFunc(L, -2, "Destination", _g_get_Destination);
		Utils.RegisterFunc(L, -2, "map", _g_get_map);
		Utils.RegisterFunc(L, -2, "GizmosListForline", _g_get_GizmosListForline);
		Utils.RegisterFunc(L, -1, "Destination", _s_set_Destination);
		Utils.RegisterFunc(L, -1, "map", _s_set_map);
		Utils.RegisterFunc(L, -1, "GizmosListForline", _s_set_GizmosListForline);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
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
				JPSPathfinding o = new JPSPathfinding();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LW_JPS.JPSPathfinding constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitMap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			JPSPathfinding jPSPathfinding = (JPSPathfinding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			Vector2Int[] obstacles = (Vector2Int[])objectTranslator.GetObject(L, 3, typeof(Vector2Int[]));
			objectTranslator.Get(L, 4, out Vector3 val);
			bool obstacleEndIsOn = Lua.lua_toboolean(L, 5);
			jPSPathfinding.InitMap(v, obstacles, val, obstacleEndIsOn);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateMap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			JPSPathfinding jPSPathfinding = (JPSPathfinding)objectTranslator.FastGetCSObj(L, 1);
			Vector2Int[] obstacles = (Vector2Int[])objectTranslator.GetObject(L, 2, typeof(Vector2Int[]));
			int isOn = Lua.xlua_tointeger(L, 3);
			jPSPathfinding.UpdateMap(obstacles, isOn);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			JPSPathfinding jPSPathfinding = (JPSPathfinding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2Int v);
			objectTranslator.Get(L, 3, out Vector2Int v2);
			List<Point> path = jPSPathfinding.GetPath(v, v2);
			objectTranslator.Push(L, path);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Destination(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			JPSPathfinding jPSPathfinding = (JPSPathfinding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, jPSPathfinding.Destination);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_map(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			JPSPathfinding jPSPathfinding = (JPSPathfinding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, jPSPathfinding.map);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GizmosListForline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			JPSPathfinding jPSPathfinding = (JPSPathfinding)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, jPSPathfinding.GizmosListForline);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Destination(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((JPSPathfinding)objectTranslator.FastGetCSObj(L, 1)).Destination = (Point)objectTranslator.GetObject(L, 2, typeof(Point));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_map(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((JPSPathfinding)objectTranslator.FastGetCSObj(L, 1)).map = (int[,])objectTranslator.GetObject(L, 2, typeof(int[,]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GizmosListForline(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((JPSPathfinding)objectTranslator.FastGetCSObj(L, 1)).GizmosListForline = (List<Point>)objectTranslator.GetObject(L, 2, typeof(List<Point>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
