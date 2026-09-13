using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldTroopLineLittleSmartLineWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldTroopLineLittleSmart.Line);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 7, 7);
		Utils.RegisterFunc(L, -3, "Refresh", _m_Refresh);
		Utils.RegisterFunc(L, -2, "DataIndex", _g_get_DataIndex);
		Utils.RegisterFunc(L, -2, "dataIndex", _g_get_dataIndex);
		Utils.RegisterFunc(L, -2, "startPos", _g_get_startPos);
		Utils.RegisterFunc(L, -2, "targetPos", _g_get_targetPos);
		Utils.RegisterFunc(L, -2, "troopLineColor", _g_get_troopLineColor);
		Utils.RegisterFunc(L, -2, "tranVec", _g_get_tranVec);
		Utils.RegisterFunc(L, -2, "speed", _g_get_speed);
		Utils.RegisterFunc(L, -1, "DataIndex", _s_set_DataIndex);
		Utils.RegisterFunc(L, -1, "dataIndex", _s_set_dataIndex);
		Utils.RegisterFunc(L, -1, "startPos", _s_set_startPos);
		Utils.RegisterFunc(L, -1, "targetPos", _s_set_targetPos);
		Utils.RegisterFunc(L, -1, "troopLineColor", _s_set_troopLineColor);
		Utils.RegisterFunc(L, -1, "tranVec", _s_set_tranVec);
		Utils.RegisterFunc(L, -1, "speed", _s_set_speed);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 3, 3);
		Utils.RegisterFunc(L, -2, "GlobalTroopLineScale", _g_get_GlobalTroopLineScale);
		Utils.RegisterFunc(L, -2, "GlobalTroopCircleScale", _g_get_GlobalTroopCircleScale);
		Utils.RegisterFunc(L, -2, "GlobalTroopLineAlpha", _g_get_GlobalTroopLineAlpha);
		Utils.RegisterFunc(L, -1, "GlobalTroopLineScale", _s_set_GlobalTroopLineScale);
		Utils.RegisterFunc(L, -1, "GlobalTroopCircleScale", _s_set_GlobalTroopCircleScale);
		Utils.RegisterFunc(L, -1, "GlobalTroopLineAlpha", _s_set_GlobalTroopLineAlpha);
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
				WorldTroopLineLittleSmart.Line o = new WorldTroopLineLittleSmart.Line();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldTroopLineLittleSmart.Line constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Refresh(IntPtr L)
	{
		try
		{
			((WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Refresh();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GlobalTroopLineScale(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldTroopLineLittleSmart.Line.GlobalTroopLineScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GlobalTroopCircleScale(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldTroopLineLittleSmart.Line.GlobalTroopCircleScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GlobalTroopLineAlpha(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, WorldTroopLineLittleSmart.Line.GlobalTroopLineAlpha);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DataIndex(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, line.DataIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dataIndex(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, line.dataIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_startPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, line.startPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, line.targetPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_troopLineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, line.troopLineColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_tranVec(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector4(L, line.tranVec);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_speed(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, line.speed);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GlobalTroopLineScale(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Line.GlobalTroopLineScale = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GlobalTroopCircleScale(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Line.GlobalTroopCircleScale = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GlobalTroopLineAlpha(IntPtr L)
	{
		try
		{
			WorldTroopLineLittleSmart.Line.GlobalTroopLineAlpha = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DataIndex(IntPtr L)
	{
		try
		{
			((WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DataIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dataIndex(IntPtr L)
	{
		try
		{
			((WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).dataIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_startPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			line.startPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			line.targetPos = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_troopLineColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			line.troopLineColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_tranVec(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldTroopLineLittleSmart.Line line = (WorldTroopLineLittleSmart.Line)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			line.tranVec = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_speed(IntPtr L)
	{
		try
		{
			((WorldTroopLineLittleSmart.Line)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).speed = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
