using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LWBattleRVOManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LWBattleRVOManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 7, 1, 0);
		Utils.RegisterFunc(L, -3, "InitLW", _m_InitLW);
		Utils.RegisterFunc(L, -3, "Append", _m_Append);
		Utils.RegisterFunc(L, -3, "Destory", _m_Destory);
		Utils.RegisterFunc(L, -3, "SyncTargetPosition", _m_SyncTargetPosition);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "AddAgent", _m_AddAgent);
		Utils.RegisterFunc(L, -3, "DeleteAgent", _m_DeleteAgent);
		Utils.RegisterFunc(L, -2, "targetPosition", _g_get_targetPosition);
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
				LWBattleRVOManager o = new LWBattleRVOManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LWBattleRVOManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitLW(IntPtr L)
	{
		try
		{
			LWBattleRVOManager lWBattleRVOManager = (LWBattleRVOManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 10 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 10))
			{
				float timeStep = (float)Lua.lua_tonumber(L, 2);
				float neighborDist = (float)Lua.lua_tonumber(L, 3);
				int maxNeighbors = Lua.xlua_tointeger(L, 4);
				float timeHorizon = (float)Lua.lua_tonumber(L, 5);
				float timeHorizonObst = (float)Lua.lua_tonumber(L, 6);
				float radius = (float)Lua.lua_tonumber(L, 7);
				float maxSpeed = (float)Lua.lua_tonumber(L, 8);
				int step = Lua.xlua_tointeger(L, 9);
				bool agentOpt = Lua.lua_toboolean(L, 10);
				lWBattleRVOManager.InitLW(timeStep, neighborDist, maxNeighbors, timeHorizon, timeHorizonObst, radius, maxSpeed, step, agentOpt);
				return 0;
			}
			if (num == 9 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 9))
			{
				float timeStep2 = (float)Lua.lua_tonumber(L, 2);
				float neighborDist2 = (float)Lua.lua_tonumber(L, 3);
				int maxNeighbors2 = Lua.xlua_tointeger(L, 4);
				float timeHorizon2 = (float)Lua.lua_tonumber(L, 5);
				float timeHorizonObst2 = (float)Lua.lua_tonumber(L, 6);
				float radius2 = (float)Lua.lua_tonumber(L, 7);
				float maxSpeed2 = (float)Lua.lua_tonumber(L, 8);
				int step2 = Lua.xlua_tointeger(L, 9);
				lWBattleRVOManager.InitLW(timeStep2, neighborDist2, maxNeighbors2, timeHorizon2, timeHorizonObst2, radius2, maxSpeed2, step2);
				return 0;
			}
			if (num == 8 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 6) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 7) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 8))
			{
				float timeStep3 = (float)Lua.lua_tonumber(L, 2);
				float neighborDist3 = (float)Lua.lua_tonumber(L, 3);
				int maxNeighbors3 = Lua.xlua_tointeger(L, 4);
				float timeHorizon3 = (float)Lua.lua_tonumber(L, 5);
				float timeHorizonObst3 = (float)Lua.lua_tonumber(L, 6);
				float radius3 = (float)Lua.lua_tonumber(L, 7);
				float maxSpeed3 = (float)Lua.lua_tonumber(L, 8);
				lWBattleRVOManager.InitLW(timeStep3, neighborDist3, maxNeighbors3, timeHorizon3, timeHorizonObst3, radius3, maxSpeed3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LWBattleRVOManager.InitLW!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Append(IntPtr L)
	{
		try
		{
			LWBattleRVOManager obj = (LWBattleRVOManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string configPath = Lua.lua_tostring(L, 2);
			float offset = (float)Lua.lua_tonumber(L, 3);
			obj.Append(configPath, offset);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destory(IntPtr L)
	{
		try
		{
			((LWBattleRVOManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destory();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SyncTargetPosition(IntPtr L)
	{
		try
		{
			LWBattleRVOManager obj = (LWBattleRVOManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float z = (float)Lua.lua_tonumber(L, 3);
			obj.SyncTargetPosition(x, z);
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
			LWBattleRVOManager obj = (LWBattleRVOManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			float x = (float)Lua.lua_tonumber(L, 2);
			float z = (float)Lua.lua_tonumber(L, 3);
			obj.Update(x, z);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddAgent(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LWBattleRVOManager lWBattleRVOManager = (LWBattleRVOManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			GameObject gameObject = (GameObject)objectTranslator.GetObject(L, 3, typeof(GameObject));
			float speed = (float)Lua.lua_tonumber(L, 4);
			float radius = (float)Lua.lua_tonumber(L, 5);
			int value = lWBattleRVOManager.AddAgent(val, gameObject, speed, radius);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeleteAgent(IntPtr L)
	{
		try
		{
			LWBattleRVOManager obj = (LWBattleRVOManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int sid = Lua.xlua_tointeger(L, 2);
			obj.DeleteAgent(sid);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetPosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			LWBattleRVOManager lWBattleRVOManager = (LWBattleRVOManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, lWBattleRVOManager.targetPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
