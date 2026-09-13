using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CountLODStrategyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CountLODStrategy);
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
			if (Lua.lua_gettop(L) == 1)
			{
				CountLODStrategy o = new CountLODStrategy();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<int[]>(L, 2))
			{
				CountLODStrategy o2 = new CountLODStrategy((int[])objectTranslator.GetObject(L, 2, typeof(int[])));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CountLODStrategy constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Configure(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CountLODStrategy countLODStrategy = (CountLODStrategy)objectTranslator.FastGetCSObj(L, 1);
			int[] thresholds = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			countLODStrategy.Configure(thresholds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateLODLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CountLODStrategy countLODStrategy = (CountLODStrategy)objectTranslator.FastGetCSObj(L, 1);
			int currentLevel = Lua.xlua_tointeger(L, 2);
			List<ISceneLODNode> nodes = (List<ISceneLODNode>)objectTranslator.GetObject(L, 3, typeof(List<ISceneLODNode>));
			int value = countLODStrategy.CalculateLODLevel(currentLevel, nodes);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
