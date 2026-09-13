using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class DynamicCostLODStrategyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(DynamicCostLODStrategy);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 0, 0);
		Utils.RegisterFunc(L, -3, "Configure", _m_Configure);
		Utils.RegisterFunc(L, -3, "CalculateLODLevel", _m_CalculateLODLevel);
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
			if (Lua.lua_gettop(L) == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float upThresholds = (float)Lua.lua_tonumber(L, 2);
				float downThresholds = (float)Lua.lua_tonumber(L, 3);
				float hysteresisTime = (float)Lua.lua_tonumber(L, 4);
				DynamicCostLODStrategy o = new DynamicCostLODStrategy(upThresholds, downThresholds, hysteresisTime);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float upThresholds2 = (float)Lua.lua_tonumber(L, 2);
				float downThresholds2 = (float)Lua.lua_tonumber(L, 3);
				DynamicCostLODStrategy o2 = new DynamicCostLODStrategy(upThresholds2, downThresholds2);
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicCostLODStrategy constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Configure(IntPtr L)
	{
		try
		{
			DynamicCostLODStrategy dynamicCostLODStrategy = (DynamicCostLODStrategy)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float upThresholds = (float)Lua.lua_tonumber(L, 2);
				float downThresholds = (float)Lua.lua_tonumber(L, 3);
				float hysteresisTime = (float)Lua.lua_tonumber(L, 4);
				dynamicCostLODStrategy.Configure(upThresholds, downThresholds, hysteresisTime);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float upThresholds2 = (float)Lua.lua_tonumber(L, 2);
				float downThresholds2 = (float)Lua.lua_tonumber(L, 3);
				dynamicCostLODStrategy.Configure(upThresholds2, downThresholds2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to DynamicCostLODStrategy.Configure!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLODLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DynamicCostLODStrategy dynamicCostLODStrategy = (DynamicCostLODStrategy)objectTranslator.FastGetCSObj(L, 1);
			int currentLevel = Lua.xlua_tointeger(L, 2);
			List<ISceneLODNode> nodes = (List<ISceneLODNode>)objectTranslator.GetObject(L, 3, typeof(List<ISceneLODNode>));
			int value = dynamicCostLODStrategy.CalculateLODLevel(currentLevel, nodes);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
