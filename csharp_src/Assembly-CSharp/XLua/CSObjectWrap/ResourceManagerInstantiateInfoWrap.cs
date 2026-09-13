using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourceManagerInstantiateInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ResourceManager.InstantiateInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 10, 10);
		Utils.RegisterFunc(L, -3, "RecordCreate", _m_RecordCreate);
		Utils.RegisterFunc(L, -3, "RecordSetup", _m_RecordSetup);
		Utils.RegisterFunc(L, -3, "SafeDivide", _m_SafeDivide);
		Utils.RegisterFunc(L, -3, "CsvInfo", _m_CsvInfo);
		Utils.RegisterFunc(L, -2, "loadCount", _g_get_loadCount);
		Utils.RegisterFunc(L, -2, "totalMsCost", _g_get_totalMsCost);
		Utils.RegisterFunc(L, -2, "maxMsCost", _g_get_maxMsCost);
		Utils.RegisterFunc(L, -2, "loadCountPool", _g_get_loadCountPool);
		Utils.RegisterFunc(L, -2, "loadCountNew", _g_get_loadCountNew);
		Utils.RegisterFunc(L, -2, "totalMsCostPool", _g_get_totalMsCostPool);
		Utils.RegisterFunc(L, -2, "totalMsCostNew", _g_get_totalMsCostNew);
		Utils.RegisterFunc(L, -2, "setupCount", _g_get_setupCount);
		Utils.RegisterFunc(L, -2, "totalSetupMsCost", _g_get_totalSetupMsCost);
		Utils.RegisterFunc(L, -2, "maxSetupMsCost", _g_get_maxSetupMsCost);
		Utils.RegisterFunc(L, -1, "loadCount", _s_set_loadCount);
		Utils.RegisterFunc(L, -1, "totalMsCost", _s_set_totalMsCost);
		Utils.RegisterFunc(L, -1, "maxMsCost", _s_set_maxMsCost);
		Utils.RegisterFunc(L, -1, "loadCountPool", _s_set_loadCountPool);
		Utils.RegisterFunc(L, -1, "loadCountNew", _s_set_loadCountNew);
		Utils.RegisterFunc(L, -1, "totalMsCostPool", _s_set_totalMsCostPool);
		Utils.RegisterFunc(L, -1, "totalMsCostNew", _s_set_totalMsCostNew);
		Utils.RegisterFunc(L, -1, "setupCount", _s_set_setupCount);
		Utils.RegisterFunc(L, -1, "totalSetupMsCost", _s_set_totalSetupMsCost);
		Utils.RegisterFunc(L, -1, "maxSetupMsCost", _s_set_maxSetupMsCost);
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
				ResourceManager.InstantiateInfo o = new ResourceManager.InstantiateInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.InstantiateInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordCreate(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo obj = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long msCost = Lua.lua_toint64(L, 2);
			bool fromPool = Lua.lua_toboolean(L, 3);
			obj.RecordCreate(msCost, fromPool);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RecordSetup(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo obj = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long msCost = Lua.lua_toint64(L, 2);
			obj.RecordSetup(msCost);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SafeDivide(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo obj = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			long numerator = Lua.lua_toint64(L, 2);
			long denominator = Lua.lua_toint64(L, 3);
			float num = obj.SafeDivide(numerator, denominator);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CsvInfo(IntPtr L)
	{
		try
		{
			string str = ((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).CsvInfo();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loadCount(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, instantiateInfo.loadCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalMsCost(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, instantiateInfo.totalMsCost);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxMsCost(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, instantiateInfo.maxMsCost);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loadCountPool(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, instantiateInfo.loadCountPool);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_loadCountNew(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, instantiateInfo.loadCountNew);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalMsCostPool(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, instantiateInfo.totalMsCostPool);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalMsCostNew(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, instantiateInfo.totalMsCostNew);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_setupCount(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, instantiateInfo.setupCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalSetupMsCost(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, instantiateInfo.totalSetupMsCost);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxSetupMsCost(IntPtr L)
	{
		try
		{
			ResourceManager.InstantiateInfo instantiateInfo = (ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushint64(L, instantiateInfo.maxSetupMsCost);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loadCount(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loadCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalMsCost(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalMsCost = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxMsCost(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxMsCost = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loadCountPool(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loadCountPool = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_loadCountNew(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).loadCountNew = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalMsCostPool(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalMsCostPool = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalMsCostNew(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalMsCostNew = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_setupCount(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).setupCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_totalSetupMsCost(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).totalSetupMsCost = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxSetupMsCost(IntPtr L)
	{
		try
		{
			((ResourceManager.InstantiateInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxSetupMsCost = Lua.lua_toint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
