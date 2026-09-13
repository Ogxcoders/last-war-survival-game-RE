using System;
using LW.CountBattle;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LWCountBattleSteerGroupWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SteerGroup);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 26, 18);
		Utils.RegisterFunc(L, -3, "SetPosX", _m_SetPosX);
		Utils.RegisterFunc(L, -3, "SetPosY", _m_SetPosY);
		Utils.RegisterFunc(L, -3, "SetPosXZ", _m_SetPosXZ);
		Utils.RegisterFunc(L, -3, "SetPos", _m_SetPos);
		Utils.RegisterFunc(L, -3, "SetVelocity", _m_SetVelocity);
		Utils.RegisterFunc(L, -3, "AddTrapCollider", _m_AddTrapCollider);
		Utils.RegisterFunc(L, -3, "FindTrapCollider", _m_FindTrapCollider);
		Utils.RegisterFunc(L, -3, "UpdateTrapCollider", _m_UpdateTrapCollider);
		Utils.RegisterFunc(L, -3, "RemoveTrapCollider", _m_RemoveTrapCollider);
		Utils.RegisterFunc(L, -3, "Spawn", _m_Spawn);
		Utils.RegisterFunc(L, -3, "RemoveUnit", _m_RemoveUnit);
		Utils.RegisterFunc(L, -3, "Vibrate", _m_Vibrate);
		Utils.RegisterFunc(L, -2, "GroupPoint", _g_get_GroupPoint);
		Utils.RegisterFunc(L, -2, "GroupCircle", _g_get_GroupCircle);
		Utils.RegisterFunc(L, -2, "GroupRadius", _g_get_GroupRadius);
		Utils.RegisterFunc(L, -2, "GroupBoundLeft", _g_get_GroupBoundLeft);
		Utils.RegisterFunc(L, -2, "GroupBoundRight", _g_get_GroupBoundRight);
		Utils.RegisterFunc(L, -2, "GroupBoundTop", _g_get_GroupBoundTop);
		Utils.RegisterFunc(L, -2, "GroupBoundBottom", _g_get_GroupBoundBottom);
		Utils.RegisterFunc(L, -2, "GroupBoundBox", _g_get_GroupBoundBox);
		Utils.RegisterFunc(L, -2, "UnitCount", _g_get_UnitCount);
		Utils.RegisterFunc(L, -2, "DisableLogic", _g_get_DisableLogic);
		Utils.RegisterFunc(L, -2, "RepSwitch", _g_get_RepSwitch);
		Utils.RegisterFunc(L, -2, "AttSwitch", _g_get_AttSwitch);
		Utils.RegisterFunc(L, -2, "GroupPos", _g_get_GroupPos);
		Utils.RegisterFunc(L, -2, "GroupPosX", _g_get_GroupPosX);
		Utils.RegisterFunc(L, -2, "GroupPosY", _g_get_GroupPosY);
		Utils.RegisterFunc(L, -2, "GroupPosZ", _g_get_GroupPosZ);
		Utils.RegisterFunc(L, -2, "Units", _g_get_Units);
		Utils.RegisterFunc(L, -2, "EngageGroup", _g_get_EngageGroup);
		Utils.RegisterFunc(L, -2, "groupTag", _g_get_groupTag);
		Utils.RegisterFunc(L, -2, "spawnPerSecond", _g_get_spawnPerSecond);
		Utils.RegisterFunc(L, -2, "repForceFactor", _g_get_repForceFactor);
		Utils.RegisterFunc(L, -2, "attForceFactor", _g_get_attForceFactor);
		Utils.RegisterFunc(L, -2, "OnUnitSpawn", _g_get_OnUnitSpawn);
		Utils.RegisterFunc(L, -2, "OnUnitRemoved", _g_get_OnUnitRemoved);
		Utils.RegisterFunc(L, -2, "OnGroupPointChanged", _g_get_OnGroupPointChanged);
		Utils.RegisterFunc(L, -2, "OnCollideTrap", _g_get_OnCollideTrap);
		Utils.RegisterFunc(L, -1, "GroupPoint", _s_set_GroupPoint);
		Utils.RegisterFunc(L, -1, "GroupCircle", _s_set_GroupCircle);
		Utils.RegisterFunc(L, -1, "GroupBoundLeft", _s_set_GroupBoundLeft);
		Utils.RegisterFunc(L, -1, "GroupBoundRight", _s_set_GroupBoundRight);
		Utils.RegisterFunc(L, -1, "GroupBoundTop", _s_set_GroupBoundTop);
		Utils.RegisterFunc(L, -1, "GroupBoundBottom", _s_set_GroupBoundBottom);
		Utils.RegisterFunc(L, -1, "DisableLogic", _s_set_DisableLogic);
		Utils.RegisterFunc(L, -1, "RepSwitch", _s_set_RepSwitch);
		Utils.RegisterFunc(L, -1, "AttSwitch", _s_set_AttSwitch);
		Utils.RegisterFunc(L, -1, "EngageGroup", _s_set_EngageGroup);
		Utils.RegisterFunc(L, -1, "groupTag", _s_set_groupTag);
		Utils.RegisterFunc(L, -1, "spawnPerSecond", _s_set_spawnPerSecond);
		Utils.RegisterFunc(L, -1, "repForceFactor", _s_set_repForceFactor);
		Utils.RegisterFunc(L, -1, "attForceFactor", _s_set_attForceFactor);
		Utils.RegisterFunc(L, -1, "OnUnitSpawn", _s_set_OnUnitSpawn);
		Utils.RegisterFunc(L, -1, "OnUnitRemoved", _s_set_OnUnitRemoved);
		Utils.RegisterFunc(L, -1, "OnGroupPointChanged", _s_set_OnGroupPointChanged);
		Utils.RegisterFunc(L, -1, "OnCollideTrap", _s_set_OnCollideTrap);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 1, 1);
		Utils.RegisterFunc(L, -2, "TICK_INTERVAL", _g_get_TICK_INTERVAL);
		Utils.RegisterFunc(L, -1, "TICK_INTERVAL", _s_set_TICK_INTERVAL);
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
				SteerGroup o = new SteerGroup();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LW.CountBattle.SteerGroup constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosX(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float posX = (float)Lua.lua_tonumber(L, 2);
			obj.SetPosX(posX);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosY(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float posY = (float)Lua.lua_tonumber(L, 2);
			obj.SetPosY(posY);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPosXZ(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float z = (float)Lua.lua_tonumber(L, 3);
			obj.SetPosXZ(x, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetPos(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			float z = (float)Lua.lua_tonumber(L, 4);
			obj.SetPos(x, y, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetVelocity(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float vx = (float)Lua.lua_tonumber(L, 2);
			float vz = (float)Lua.lua_tonumber(L, 3);
			obj.SetVelocity(vx, vz);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddTrapCollider(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			Collider collider = (Collider)objectTranslator.GetObject(L, 3, typeof(Collider));
			bool value = steerGroup.AddTrapCollider(id, collider);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FindTrapCollider(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup obj = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			SteerCollider o = obj.FindTrapCollider(id);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateTrapCollider(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				int id = Lua.xlua_tointeger(L, 2);
				float x = (float)Lua.lua_tonumber(L, 3);
				float z = (float)Lua.lua_tonumber(L, 4);
				float angle = (float)Lua.lua_tonumber(L, 5);
				steerGroup.UpdateTrapCollider(id, x, z, angle);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Collider>(L, 3))
			{
				int id2 = Lua.xlua_tointeger(L, 2);
				Collider collider = (Collider)objectTranslator.GetObject(L, 3, typeof(Collider));
				steerGroup.UpdateTrapCollider(id2, collider);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LW.CountBattle.SteerGroup.UpdateTrapCollider!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveTrapCollider(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int id = Lua.xlua_tointeger(L, 2);
			obj.RemoveTrapCollider(id);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Spawn(IntPtr L)
	{
		try
		{
			SteerGroup obj = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int amount = Lua.xlua_tointeger(L, 2);
			int point = Lua.xlua_tointeger(L, 3);
			float radius = (float)Lua.lua_tonumber(L, 4);
			int value = obj.Spawn(amount, point, radius);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveUnit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			SteerUnit unit = (SteerUnit)objectTranslator.GetObject(L, 2, typeof(SteerUnit));
			steerGroup.RemoveUnit(unit);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Vibrate(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Vibrate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupPoint(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerGroup.GroupPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupCircle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.GroupCircle);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupRadius(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupRadius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupBoundLeft(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupBoundLeft);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupBoundRight(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupBoundRight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupBoundTop(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupBoundTop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupBoundBottom(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupBoundBottom);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupBoundBox(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.GroupBoundBox);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_UnitCount(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerGroup.UnitCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DisableLogic(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, steerGroup.DisableLogic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RepSwitch(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, steerGroup.RepSwitch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_AttSwitch(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, steerGroup.AttSwitch);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, steerGroup.GroupPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupPosX(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupPosX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupPosY(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupPosY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_GroupPosZ(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.GroupPosZ);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Units(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.Units);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_EngageGroup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.EngageGroup);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_TICK_INTERVAL(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, SteerGroup.TICK_INTERVAL);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_groupTag(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, steerGroup.groupTag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_spawnPerSecond(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerGroup.spawnPerSecond);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_repForceFactor(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.repForceFactor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_attForceFactor(IntPtr L)
	{
		try
		{
			SteerGroup steerGroup = (SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerGroup.attForceFactor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnUnitSpawn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.OnUnitSpawn);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnUnitRemoved(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.OnUnitRemoved);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnGroupPointChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.OnGroupPointChanged);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnCollideTrap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerGroup steerGroup = (SteerGroup)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerGroup.OnCollideTrap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GroupPoint(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GroupPoint = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GroupCircle(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerGroup)objectTranslator.FastGetCSObj(L, 1)).GroupCircle = (Circle)objectTranslator.GetObject(L, 2, typeof(Circle));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GroupBoundLeft(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GroupBoundLeft = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GroupBoundRight(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GroupBoundRight = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GroupBoundTop(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GroupBoundTop = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_GroupBoundBottom(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GroupBoundBottom = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_DisableLogic(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DisableLogic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RepSwitch(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RepSwitch = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_AttSwitch(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).AttSwitch = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_EngageGroup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerGroup)objectTranslator.FastGetCSObj(L, 1)).EngageGroup = (SteerGroup)objectTranslator.GetObject(L, 2, typeof(SteerGroup));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_TICK_INTERVAL(IntPtr L)
	{
		try
		{
			SteerGroup.TICK_INTERVAL = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_groupTag(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).groupTag = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_spawnPerSecond(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).spawnPerSecond = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_repForceFactor(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).repForceFactor = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_attForceFactor(IntPtr L)
	{
		try
		{
			((SteerGroup)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).attForceFactor = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnUnitSpawn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerGroup)objectTranslator.FastGetCSObj(L, 1)).OnUnitSpawn = objectTranslator.GetDelegate<Action<int, SteerUnit>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnUnitRemoved(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerGroup)objectTranslator.FastGetCSObj(L, 1)).OnUnitRemoved = objectTranslator.GetDelegate<Action<int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnGroupPointChanged(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerGroup)objectTranslator.FastGetCSObj(L, 1)).OnGroupPointChanged = objectTranslator.GetDelegate<Action<int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnCollideTrap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerGroup)objectTranslator.FastGetCSObj(L, 1)).OnCollideTrap = objectTranslator.GetDelegate<Action<int, int>>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
