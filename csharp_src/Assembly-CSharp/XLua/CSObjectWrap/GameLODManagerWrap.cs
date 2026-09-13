using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameLODManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GameLODManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 0, 0);
		Utils.RegisterFunc(L, -3, "Initialize", _m_Initialize);
		Utils.RegisterFunc(L, -3, "InitializeEffectLOD", _m_InitializeEffectLOD);
		Utils.RegisterFunc(L, -3, "InitializeSquadLOD", _m_InitializeSquadLOD);
		Utils.RegisterFunc(L, -3, "InitializeCPULOD", _m_InitializeCPULOD);
		Utils.RegisterFunc(L, -3, "ConfigureLODRange", _m_ConfigureLODRange);
		Utils.RegisterFunc(L, -3, "ConfigureEffectLOD", _m_ConfigureEffectLOD);
		Utils.RegisterFunc(L, -3, "ConfigureEffectWorldCommonLOD", _m_ConfigureEffectWorldCommonLOD);
		Utils.RegisterFunc(L, -3, "ConfigureEffectWorldTroopLOD", _m_ConfigureEffectWorldTroopLOD);
		Utils.RegisterFunc(L, -3, "ConfigureSkinnedMeshLOD", _m_ConfigureSkinnedMeshLOD);
		Utils.RegisterFunc(L, -3, "SetupEffectLODStaticCost", _m_SetupEffectLODStaticCost);
		Utils.RegisterFunc(L, -3, "SetupEffectLODDynamicCost", _m_SetupEffectLODDynamicCost);
		Utils.RegisterFunc(L, -3, "Shutdown", _m_Shutdown);
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
				GameLODManager o = new GameLODManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameLODManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Initialize(IntPtr L)
	{
		try
		{
			((GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Initialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitializeEffectLOD(IntPtr L)
	{
		try
		{
			((GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitializeEffectLOD();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitializeSquadLOD(IntPtr L)
	{
		try
		{
			((GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitializeSquadLOD();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_InitializeCPULOD(IntPtr L)
	{
		try
		{
			((GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).InitializeCPULOD();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConfigureLODRange(IntPtr L)
	{
		try
		{
			GameLODManager obj = (GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int min = Lua.xlua_tointeger(L, 2);
			int max = Lua.xlua_tointeger(L, 3);
			obj.ConfigureLODRange(min, max);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConfigureEffectLOD(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameLODManager gameLODManager = (GameLODManager)objectTranslator.FastGetCSObj(L, 1);
			int[] thresholds = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			gameLODManager.ConfigureEffectLOD(thresholds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConfigureEffectWorldCommonLOD(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameLODManager gameLODManager = (GameLODManager)objectTranslator.FastGetCSObj(L, 1);
			float[] costThresholds = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
			int[] countThresholds = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
			gameLODManager.ConfigureEffectWorldCommonLOD(costThresholds, countThresholds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConfigureEffectWorldTroopLOD(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameLODManager gameLODManager = (GameLODManager)objectTranslator.FastGetCSObj(L, 1);
			float[] costThresholds = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
			int[] countThresholds = (int[])objectTranslator.GetObject(L, 3, typeof(int[]));
			gameLODManager.ConfigureEffectWorldTroopLOD(costThresholds, countThresholds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConfigureSkinnedMeshLOD(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameLODManager gameLODManager = (GameLODManager)objectTranslator.FastGetCSObj(L, 1);
			int[] thresholds = (int[])objectTranslator.GetObject(L, 2, typeof(int[]));
			gameLODManager.ConfigureSkinnedMeshLOD(thresholds);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupEffectLODStaticCost(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameLODManager gameLODManager = (GameLODManager)objectTranslator.FastGetCSObj(L, 1);
			float[] staticCosts = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
			gameLODManager.SetupEffectLODStaticCost(staticCosts);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupEffectLODDynamicCost(IntPtr L)
	{
		try
		{
			GameLODManager gameLODManager = (GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float upThresholds = (float)Lua.lua_tonumber(L, 2);
				float downThresholds = (float)Lua.lua_tonumber(L, 3);
				float hysteresisTime = (float)Lua.lua_tonumber(L, 4);
				gameLODManager.SetupEffectLODDynamicCost(upThresholds, downThresholds, hysteresisTime);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float upThresholds2 = (float)Lua.lua_tonumber(L, 2);
				float downThresholds2 = (float)Lua.lua_tonumber(L, 3);
				gameLODManager.SetupEffectLODDynamicCost(upThresholds2, downThresholds2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameLODManager.SetupEffectLODDynamicCost!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Shutdown(IntPtr L)
	{
		try
		{
			((GameLODManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Shutdown();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
