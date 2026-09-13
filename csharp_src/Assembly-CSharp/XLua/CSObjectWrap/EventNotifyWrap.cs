using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class EventNotifyWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(EventNotify);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 7, 0, 0);
		Utils.RegisterFunc(L, -4, "Fire", _m_Fire_xlua_st_);
		Utils.RegisterFunc(L, -4, "FireLong", _m_FireLong_xlua_st_);
		Utils.RegisterFunc(L, -4, "FireBool", _m_FireBool_xlua_st_);
		Utils.RegisterFunc(L, -4, "FireString", _m_FireString_xlua_st_);
		Utils.RegisterFunc(L, -4, "FireLuaTable", _m_FireLuaTable_xlua_st_);
		Utils.RegisterFunc(L, -4, "FireSFSObject", _m_FireSFSObject_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "EventNotify does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Fire_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out EventId v);
			EventNotify.Fire(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FireLong_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out EventId v);
			long userData = Lua.lua_toint64(L, 2);
			EventNotify.FireLong(v, userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FireBool_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out EventId v);
			bool userData = Lua.lua_toboolean(L, 2);
			EventNotify.FireBool(v, userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FireString_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out EventId v);
			string userData = Lua.lua_tostring(L, 2);
			EventNotify.FireString(v, userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FireLuaTable_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out EventId v);
			LuaTable userData = (LuaTable)objectTranslator.GetObject(L, 2, typeof(LuaTable));
			EventNotify.FireLuaTable(v, userData);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FireSFSObject_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out EventId v);
			byte[] sfsObjBinary = Lua.lua_tobytes(L, 2);
			EventNotify.FireSFSObject(v, sfsObjBinary);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
