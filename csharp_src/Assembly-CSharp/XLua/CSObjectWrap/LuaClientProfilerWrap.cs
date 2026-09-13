using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaClientProfilerWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LuaClientProfiler);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 12, 2, 0);
		Utils.RegisterFunc(L, -4, "BeginSampleById", _m_BeginSampleById_xlua_st_);
		Utils.RegisterFunc(L, -4, "BeginSampleByIdName", _m_BeginSampleByIdName_xlua_st_);
		Utils.RegisterFunc(L, -4, "EndSample", _m_EndSample_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaBeginSampleById", _m_LuaBeginSampleById_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaBeginSampleByIdName", _m_LuaBeginSampleByIdName_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaEndSample", _m_LuaEndSample_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaBeginRuntimeSampleById", _m_LuaBeginRuntimeSampleById_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaBeginRuntimeSampleByIdName", _m_LuaBeginRuntimeSampleByIdName_xlua_st_);
		Utils.RegisterFunc(L, -4, "LuaEndRuntimeSample", _m_LuaEndRuntimeSample_xlua_st_);
		Utils.RegisterFunc(L, -4, "Attach", _m_Attach_xlua_st_);
		Utils.RegisterFunc(L, -4, "Detach", _m_Detach_xlua_st_);
		Utils.RegisterFunc(L, -2, "LuaRuntimeSampleEnabled", _g_get_LuaRuntimeSampleEnabled);
		Utils.RegisterFunc(L, -2, "IsAttached", _g_get_IsAttached);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "LuaClientProfiler does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BeginSampleById_xlua_st_(IntPtr L)
	{
		try
		{
			int id = Lua.xlua_tointeger(L, 1);
			int luaAllocBytes = Lua.xlua_tointeger(L, 2);
			LuaClientProfiler.BeginSampleById(id, luaAllocBytes);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BeginSampleByIdName_xlua_st_(IntPtr L)
	{
		try
		{
			int id = Lua.xlua_tointeger(L, 1);
			string name = Lua.lua_tostring(L, 2);
			int luaAllocBytes = Lua.xlua_tointeger(L, 3);
			LuaClientProfiler.BeginSampleByIdName(id, name, luaAllocBytes);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EndSample_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.EndSample(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaBeginSampleById_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.LuaBeginSampleById(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaBeginSampleByIdName_xlua_st_(IntPtr L)
	{
		try
		{
			int id = Lua.xlua_tointeger(L, 1);
			string name = Lua.lua_tostring(L, 2);
			LuaClientProfiler.LuaBeginSampleByIdName(id, name);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaEndSample_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.LuaEndSample();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaBeginRuntimeSampleById_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.LuaBeginRuntimeSampleById(Lua.xlua_tointeger(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaBeginRuntimeSampleByIdName_xlua_st_(IntPtr L)
	{
		try
		{
			int id = Lua.xlua_tointeger(L, 1);
			string name = Lua.lua_tostring(L, 2);
			LuaClientProfiler.LuaBeginRuntimeSampleByIdName(id, name);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LuaEndRuntimeSample_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.LuaEndRuntimeSample();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Attach_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.Attach();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Detach_xlua_st_(IntPtr L)
	{
		try
		{
			LuaClientProfiler.Detach();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LuaRuntimeSampleEnabled(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, LuaClientProfiler.LuaRuntimeSampleEnabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_IsAttached(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, LuaClientProfiler.IsAttached);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
