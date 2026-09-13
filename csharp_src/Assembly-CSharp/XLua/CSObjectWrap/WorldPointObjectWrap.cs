using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldPointObjectWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldPointObject);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 22, 8, 0);
		Utils.RegisterFunc(L, -3, "InitByPointInfo", _m_InitByPointInfo);
		Utils.RegisterFunc(L, -3, "GetServerId", _m_GetServerId);
		Utils.RegisterFunc(L, -3, "SetVisible", _m_SetVisible);
		Utils.RegisterFunc(L, -3, "GetPointIndex", _m_GetPointIndex);
		Utils.RegisterFunc(L, -3, "GetPointType", _m_GetPointType);
		Utils.RegisterFunc(L, -3, "GetGameObject", _m_GetGameObject);
		Utils.RegisterFunc(L, -3, "CreateGameObject", _m_CreateGameObject);
		Utils.RegisterFunc(L, -3, "SetClickEvent", _m_SetClickEvent);
		Utils.RegisterFunc(L, -3, "UpdateSelfMarch", _m_UpdateSelfMarch);
		Utils.RegisterFunc(L, -3, "UpdateGameObject", _m_UpdateGameObject);
		Utils.RegisterFunc(L, -3, "OnUpdate", _m_OnUpdate);
		Utils.RegisterFunc(L, -3, "OnWorldColorDirty", _m_OnWorldColorDirty);
		Utils.RegisterFunc(L, -3, "CheckShowTroopDestination", _m_CheckShowTroopDestination);
		Utils.RegisterFunc(L, -3, "ShowTroopDestinationSignal", _m_ShowTroopDestinationSignal);
		Utils.RegisterFunc(L, -3, "HideTroopDestinationSignal", _m_HideTroopDestinationSignal);
		Utils.RegisterFunc(L, -3, "OnUpdateIconScale", _m_OnUpdateIconScale);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "SetAutoAdjustLod", _m_SetAutoAdjustLod);
		Utils.RegisterFunc(L, -3, "CheckShowDesertTile", _m_CheckShowDesertTile);
		Utils.RegisterFunc(L, -3, "UpdateTileSize", _m_UpdateTileSize);
		Utils.RegisterFunc(L, -3, "RecordBlockIndex", _m_RecordBlockIndex);
		Utils.RegisterFunc(L, -3, "RemoveBlockIndex", _m_RemoveBlockIndex);
		Utils.RegisterFunc(L, -2, "WorldId", _g_get_WorldId);
		Utils.RegisterFunc(L, -2, "PointBattlefieldType", _g_get_PointBattlefieldType);
		Utils.RegisterFunc(L, -2, "InstanceRequested", _g_get_InstanceRequested);
		Utils.RegisterFunc(L, -2, "TitleRequested", _g_get_TitleRequested);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "AutoLookAtThreshold", _g_get_AutoLookAtThreshold);
		Utils.RegisterFunc(L, -2, "AutoAdjustLod", _g_get_AutoAdjustLod);
		Utils.RegisterFunc(L, -2, "WorldPosition", _g_get_WorldPosition);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "AsyncLoad", _m_AsyncLoad_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "WorldPointObject does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitByPointInfo(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			PointInfo pointInfo = (PointInfo)objectTranslator.GetObject(L, 2, typeof(PointInfo));
			worldPointObject.InitByPointInfo(pointInfo);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetServerId(IntPtr L)
	{
		try
		{
			int serverId = ((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetServerId();
			Lua.xlua_pushinteger(L, serverId);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVisible(IntPtr L)
	{
		try
		{
			WorldPointObject obj = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool visible = Lua.lua_toboolean(L, 2);
			obj.SetVisible(visible);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointIndex(IntPtr L)
	{
		try
		{
			int pointIndex = ((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPointIndex();
			Lua.xlua_pushinteger(L, pointIndex);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPointType(IntPtr L)
	{
		try
		{
			int pointType = ((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPointType();
			Lua.xlua_pushinteger(L, pointType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject gameObject = ((WorldPointObject)objectTranslator.FastGetCSObj(L, 1)).GetGameObject();
			objectTranslator.Push(L, gameObject);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CreateGameObject(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CreateGameObject();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetClickEvent(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetClickEvent();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateSelfMarch(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			object o = objectTranslator.GetObject(L, 2, typeof(object));
			worldPointObject.UpdateSelfMarch(o);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateGameObject(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).UpdateGameObject();
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
			WorldPointObject obj = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
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
	private static int _m_OnWorldColorDirty(IntPtr L)
	{
		try
		{
			WorldPointObject obj = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string allianceId = Lua.lua_tostring(L, 2);
			obj.OnWorldColorDirty(allianceId);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckShowTroopDestination(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckShowTroopDestination();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ShowTroopDestinationSignal(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			objectTranslator.Get(L, 3, out EnumDestinationSignalType v);
			int tileSize = Lua.xlua_tointeger(L, 4);
			worldPointObject.ShowTroopDestinationSignal(val, v, tileSize);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HideTroopDestinationSignal(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).HideTroopDestinationSignal();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnUpdateIconScale(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Quaternion val);
			float scale = (float)Lua.lua_tonumber(L, 3);
			worldPointObject.OnUpdateIconScale(val, scale);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetAutoAdjustLod(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).SetAutoAdjustLod();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CheckShowDesertTile(IntPtr L)
	{
		try
		{
			((WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CheckShowDesertTile();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AsyncLoad_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Transform>(L, 2) && objectTranslator.Assignable<Action<InstanceRequest>>(L, 3))
			{
				string prefabPath = Lua.lua_tostring(L, 1);
				Transform parent = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				Action<InstanceRequest> completed = objectTranslator.GetDelegate<Action<InstanceRequest>>(L, 3);
				InstanceRequest o = WorldPointObject.AsyncLoad(prefabPath, parent, completed);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Transform>(L, 2))
			{
				string prefabPath2 = Lua.lua_tostring(L, 1);
				Transform parent2 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				InstanceRequest o2 = WorldPointObject.AsyncLoad(prefabPath2, parent2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 6 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Transform>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5) && objectTranslator.Assignable<Action<InstanceRequest>>(L, 6))
			{
				string prefabPath3 = Lua.lua_tostring(L, 1);
				Transform parent3 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				objectTranslator.Get(L, 3, out Vector3 val);
				objectTranslator.Get(L, 4, out Vector3 val2);
				objectTranslator.Get(L, 5, out Quaternion val3);
				InstanceRequest o3 = WorldPointObject.AsyncLoad(completed: objectTranslator.GetDelegate<Action<InstanceRequest>>(L, 6), prefabPath: prefabPath3, parent: parent3, pos: val, scale: val2, rotation: val3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 5 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Transform>(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && objectTranslator.Assignable<Vector3>(L, 4) && objectTranslator.Assignable<Quaternion>(L, 5))
			{
				string prefabPath4 = Lua.lua_tostring(L, 1);
				Transform parent4 = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
				objectTranslator.Get(L, 3, out Vector3 val4);
				objectTranslator.Get(L, 4, out Vector3 val5);
				objectTranslator.Get(L, 5, out Quaternion val6);
				InstanceRequest o4 = WorldPointObject.AsyncLoad(prefabPath4, parent4, val4, val5, val6);
				objectTranslator.Push(L, o4);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldPointObject.AsyncLoad!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTileSize(IntPtr L)
	{
		try
		{
			WorldPointObject obj = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int tileSize = Lua.xlua_tointeger(L, 2);
			obj.UpdateTileSize(tileSize);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordBlockIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			Dictionary<int, int> set = (Dictionary<int, int>)objectTranslator.GetObject(L, 2, typeof(Dictionary<int, int>));
			worldPointObject.RecordBlockIndex(set);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveBlockIndex(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			Dictionary<int, int> set = (Dictionary<int, int>)objectTranslator.GetObject(L, 2, typeof(Dictionary<int, int>));
			worldPointObject.RemoveBlockIndex(set);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldId(IntPtr L)
	{
		try
		{
			WorldPointObject worldPointObject = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldPointObject.WorldId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PointBattlefieldType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldPointObject.PointBattlefieldType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_InstanceRequested(IntPtr L)
	{
		try
		{
			WorldPointObject worldPointObject = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldPointObject.InstanceRequested);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TitleRequested(IntPtr L)
	{
		try
		{
			WorldPointObject worldPointObject = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, worldPointObject.TitleRequested);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_id(IntPtr L)
	{
		try
		{
			WorldPointObject worldPointObject = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, worldPointObject.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoLookAtThreshold(IntPtr L)
	{
		try
		{
			WorldPointObject worldPointObject = (WorldPointObject)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, worldPointObject.AutoLookAtThreshold);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AutoAdjustLod(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, worldPointObject.AutoAdjustLod);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_WorldPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldPointObject worldPointObject = (WorldPointObject)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, worldPointObject.WorldPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
