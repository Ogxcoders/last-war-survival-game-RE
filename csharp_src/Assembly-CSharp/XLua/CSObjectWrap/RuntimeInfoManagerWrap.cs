using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class RuntimeInfoManagerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RuntimeInfoManager);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 0, 0);
		Utils.RegisterFunc(L, -3, "Init", _m_Init);
		Utils.RegisterFunc(L, -3, "GetModel", _m_GetModel);
		Utils.RegisterFunc(L, -3, "GetDeviceLevel", _m_GetDeviceLevel);
		Utils.RegisterFunc(L, -3, "GetDeviceScore", _m_GetDeviceScore);
		Utils.RegisterFunc(L, -3, "GetOperatingSystem", _m_GetOperatingSystem);
		Utils.RegisterFunc(L, -3, "LogRuntimeInfo", _m_LogRuntimeInfo);
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
				RuntimeInfoManager o = new RuntimeInfoManager();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to RuntimeInfoManager constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Init(IntPtr L)
	{
		try
		{
			((RuntimeInfoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Init();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetModel(IntPtr L)
	{
		try
		{
			string model = ((RuntimeInfoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetModel();
			Lua.lua_pushstring(L, model);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDeviceLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			DeviceLevel deviceLevel = ((RuntimeInfoManager)objectTranslator.FastGetCSObj(L, 1)).GetDeviceLevel();
			objectTranslator.PushDeviceLevel(L, deviceLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDeviceScore(IntPtr L)
	{
		try
		{
			float deviceScore = ((RuntimeInfoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetDeviceScore();
			Lua.lua_pushnumber(L, deviceScore);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetOperatingSystem(IntPtr L)
	{
		try
		{
			string operatingSystem = ((RuntimeInfoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetOperatingSystem();
			Lua.lua_pushstring(L, operatingSystem);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogRuntimeInfo(IntPtr L)
	{
		try
		{
			((RuntimeInfoManager)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).LogRuntimeInfo();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
