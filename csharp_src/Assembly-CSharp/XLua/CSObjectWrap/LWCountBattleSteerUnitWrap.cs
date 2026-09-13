using System;
using LW.CountBattle;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LWCountBattleSteerUnitWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SteerUnit);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 11, 7);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "SetPos", _m_SetPos);
		Utils.RegisterFunc(L, -3, "Move", _m_Move);
		Utils.RegisterFunc(L, -3, "Kill", _m_Kill);
		Utils.RegisterFunc(L, -3, "Tick", _m_Tick);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "Sync", _m_Sync);
		Utils.RegisterFunc(L, -2, "pos", _g_get_pos);
		Utils.RegisterFunc(L, -2, "radius", _g_get_radius);
		Utils.RegisterFunc(L, -2, "DisplayPos", _g_get_DisplayPos);
		Utils.RegisterFunc(L, -2, "IsDead", _g_get_IsDead);
		Utils.RegisterFunc(L, -2, "id", _g_get_id);
		Utils.RegisterFunc(L, -2, "initPoint", _g_get_initPoint);
		Utils.RegisterFunc(L, -2, "point", _g_get_point);
		Utils.RegisterFunc(L, -2, "shape", _g_get_shape);
		Utils.RegisterFunc(L, -2, "velocity", _g_get_velocity);
		Utils.RegisterFunc(L, -2, "sqrMag2Center", _g_get_sqrMag2Center);
		Utils.RegisterFunc(L, -2, "OnDead", _g_get_OnDead);
		Utils.RegisterFunc(L, -1, "id", _s_set_id);
		Utils.RegisterFunc(L, -1, "initPoint", _s_set_initPoint);
		Utils.RegisterFunc(L, -1, "point", _s_set_point);
		Utils.RegisterFunc(L, -1, "shape", _s_set_shape);
		Utils.RegisterFunc(L, -1, "velocity", _s_set_velocity);
		Utils.RegisterFunc(L, -1, "sqrMag2Center", _s_set_sqrMag2Center);
		Utils.RegisterFunc(L, -1, "OnDead", _s_set_OnDead);
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
			if (Lua.lua_gettop(L) == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6))
			{
				int id = Lua.xlua_tointeger(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				SteerUnit o = new SteerUnit(point: Lua.xlua_tointeger(L, 4), radius: (float)Lua.lua_tonumber(L, 5), firstTickTimeFix: (float)Lua.lua_tonumber(L, 6), id: id, pos0: val);
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LW.CountBattle.SteerUnit constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
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
			SteerUnit obj = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float z = (float)Lua.lua_tonumber(L, 3);
			obj.SetPos(x, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Move(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			steerUnit.Move(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Kill(IntPtr L)
	{
		try
		{
			((SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Kill();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Tick(IntPtr L)
	{
		try
		{
			SteerUnit obj = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float dt = (float)Lua.lua_tonumber(L, 2);
			float y = (float)Lua.lua_tonumber(L, 3);
			obj.Tick(dt, y);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			SteerUnit obj = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float dt = (float)Lua.lua_tonumber(L, 2);
			obj.Update(dt);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Sync(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			Transform transform = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
			steerUnit.Sync(transform);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, steerUnit.pos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_radius(IntPtr L)
	{
		try
		{
			SteerUnit steerUnit = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerUnit.radius);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_DisplayPos(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, steerUnit.DisplayPos);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsDead(IntPtr L)
	{
		try
		{
			SteerUnit steerUnit = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, steerUnit.IsDead);
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
			SteerUnit steerUnit = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerUnit.id);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_initPoint(IntPtr L)
	{
		try
		{
			SteerUnit steerUnit = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerUnit.initPoint);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_point(IntPtr L)
	{
		try
		{
			SteerUnit steerUnit = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, steerUnit.point);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerUnit.shape);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, steerUnit.velocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sqrMag2Center(IntPtr L)
	{
		try
		{
			SteerUnit steerUnit = (SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, steerUnit.sqrMag2Center);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_OnDead(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, steerUnit.OnDead);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_id(IntPtr L)
	{
		try
		{
			((SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).id = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_initPoint(IntPtr L)
	{
		try
		{
			((SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).initPoint = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_point(IntPtr L)
	{
		try
		{
			((SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).point = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shape(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerUnit)objectTranslator.FastGetCSObj(L, 1)).shape = (Circle)objectTranslator.GetObject(L, 2, typeof(Circle));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SteerUnit steerUnit = (SteerUnit)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			steerUnit.velocity = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sqrMag2Center(IntPtr L)
	{
		try
		{
			((SteerUnit)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).sqrMag2Center = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_OnDead(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((SteerUnit)objectTranslator.FastGetCSObj(L, 1)).OnDead = objectTranslator.GetDelegate<Action>(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
