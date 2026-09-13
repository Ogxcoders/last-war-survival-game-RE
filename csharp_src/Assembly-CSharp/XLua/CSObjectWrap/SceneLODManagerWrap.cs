using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SceneLODManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SceneLODManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 0, 0);
		Utils.RegisterFunc(L, -3, "SetupLODType", _m_SetupLODType);
		Utils.RegisterFunc(L, -3, "GetStrategies", _m_GetStrategies);
		Utils.RegisterFunc(L, -3, "GetStrategy", _m_GetStrategy);
		Utils.RegisterFunc(L, -3, "ClearStrategies", _m_ClearStrategies);
		Utils.RegisterFunc(L, -3, "ClearAllStrategies", _m_ClearAllStrategies);
		Utils.RegisterFunc(L, -3, "SetupLODRange", _m_SetupLODRange);
		Utils.RegisterFunc(L, -3, "AddStrategy", _m_AddStrategy);
		Utils.RegisterFunc(L, -3, "RemoveStrategy", _m_RemoveStrategy);
		Utils.RegisterFunc(L, -3, "AddNode", _m_AddNode);
		Utils.RegisterFunc(L, -3, "RemoveNode", _m_RemoveNode);
		Utils.RegisterFunc(L, -3, "GetLODLevel", _m_GetLODLevel);
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
				SceneLODManager o = new SceneLODManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneLODManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupLODType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && objectTranslator.Assignable<LODType>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 2, out LODType val);
				float updateInterval = (float)Lua.lua_tonumber(L, 3);
				int updateMinNodesPerFrame = Lua.xlua_tointeger(L, 4);
				sceneLODManager.SetupLODType(val, updateInterval, updateMinNodesPerFrame);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<LODType>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				objectTranslator.Get(L, 2, out LODType val2);
				float updateInterval2 = (float)Lua.lua_tonumber(L, 3);
				sceneLODManager.SetupLODType(val2, updateInterval2);
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<LODType>(L, 2))
			{
				objectTranslator.Get(L, 2, out LODType val3);
				sceneLODManager.SetupLODType(val3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to SceneLODManager.SetupLODType!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStrategies(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LODType val);
			List<ILODStrategy> strategies = sceneLODManager.GetStrategies(val);
			objectTranslator.Push(L, strategies);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStrategy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LODType val);
			ILODStrategy o = sceneLODManager.GetStrategy(strategyType: (Type)objectTranslator.GetObject(L, 3, typeof(Type)), type: val);
			objectTranslator.PushAny(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearStrategies(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LODType val);
			sceneLODManager.ClearStrategies(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllStrategies(IntPtr L)
	{
		try
		{
			((SceneLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllStrategies();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupLODRange(IntPtr L)
	{
		try
		{
			SceneLODManager obj = (SceneLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int min = Lua.xlua_tointeger(L, 2);
			int max = Lua.xlua_tointeger(L, 3);
			obj.SetupLODRange(min, max);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddStrategy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LODType val);
			ILODStrategy strategy = (ILODStrategy)objectTranslator.GetObject(L, 3, typeof(ILODStrategy));
			sceneLODManager.AddStrategy(val, strategy);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveStrategy(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LODType val);
			ILODStrategy strategy = (ILODStrategy)objectTranslator.GetObject(L, 3, typeof(ILODStrategy));
			sceneLODManager.RemoveStrategy(val, strategy);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddNode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			ISceneLODNode node = (ISceneLODNode)objectTranslator.GetObject(L, 2, typeof(ISceneLODNode));
			sceneLODManager.AddNode(node);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveNode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			ISceneLODNode node = (ISceneLODNode)objectTranslator.GetObject(L, 2, typeof(ISceneLODNode));
			sceneLODManager.RemoveNode(node);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLODLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			SceneLODManager sceneLODManager = (SceneLODManager)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LODType val);
			int lODLevel = sceneLODManager.GetLODLevel(val);
			Lua.xlua_pushinteger(L, lODLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
