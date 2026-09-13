using System;
using System.Collections.Generic;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CostLODStrategyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CostLODStrategy);
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
				CostLODStrategy o = new CostLODStrategy();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<float[]>(L, 2))
			{
				CostLODStrategy o2 = new CostLODStrategy((float[])objectTranslator.GetObject(L, 2, typeof(float[])));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CostLODStrategy constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Configure(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CostLODStrategy costLODStrategy = (CostLODStrategy)objectTranslator.FastGetCSObj(L, 1);
			float[] thresholds = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
			costLODStrategy.Configure(thresholds);
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
			CostLODStrategy costLODStrategy = (CostLODStrategy)objectTranslator.FastGetCSObj(L, 1);
			int currentLevel = Lua.xlua_tointeger(L, 2);
			List<ISceneLODNode> nodes = (List<ISceneLODNode>)objectTranslator.GetObject(L, 3, typeof(List<ISceneLODNode>));
			int value = costLODStrategy.CalculateLODLevel(currentLevel, nodes);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
