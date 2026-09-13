using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BuildPowerFactoryWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BuildPowerFactory);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 0, 0);
		Utils.RegisterFunc(L, -3, "SetWorkModeOn", _m_SetWorkModeOn);
		Utils.RegisterFunc(L, -3, "SetWorkerStatus", _m_SetWorkerStatus);
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
				BuildPowerFactory o = new BuildPowerFactory();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to BuildPowerFactory constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorkModeOn(IntPtr L)
	{
		try
		{
			BuildPowerFactory obj = (BuildPowerFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool workModeOn = Lua.lua_toboolean(L, 2);
			obj.SetWorkModeOn(workModeOn);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetWorkerStatus(IntPtr L)
	{
		try
		{
			BuildPowerFactory obj = (BuildPowerFactory)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int workerStatus = Lua.xlua_tointeger(L, 2);
			obj.SetWorkerStatus(workerStatus);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
