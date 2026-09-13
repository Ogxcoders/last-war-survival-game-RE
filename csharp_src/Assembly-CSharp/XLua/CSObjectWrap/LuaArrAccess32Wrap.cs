using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaArrAccess32Wrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(LuaArrAccess32);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 11, 0, 0);
		Utils.RegisterFunc(L, -3, "ToString", _m_ToString);
		Utils.RegisterFunc(L, -3, "OnPin", _m_OnPin);
		Utils.RegisterFunc(L, -3, "OnGC", _m_OnGC);
		Utils.RegisterFunc(L, -3, "IsValid", _m_IsValid);
		Utils.RegisterFunc(L, -3, "GetArrayCapacity", _m_GetArrayCapacity);
		Utils.RegisterFunc(L, -3, "GetInt", _m_GetInt);
		Utils.RegisterFunc(L, -3, "SetInt", _m_SetInt);
		Utils.RegisterFunc(L, -3, "GetDouble", _m_GetDouble);
		Utils.RegisterFunc(L, -3, "SetDouble", _m_SetDouble);
		Utils.RegisterFunc(L, -3, "GetLong", _m_GetLong);
		Utils.RegisterFunc(L, -3, "SetLong", _m_SetLong);
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
				LuaArrAccess32 o = new LuaArrAccess32();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to LuaArrAccess32 constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToString(IntPtr L)
	{
		try
		{
			string str = ((LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ToString();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnPin(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			IntPtr tablePtr = Lua.lua_touserdata(L, 2);
			obj.OnPin(tablePtr);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnGC(IntPtr L)
	{
		try
		{
			((LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnGC();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsValid(IntPtr L)
	{
		try
		{
			bool value = ((LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).IsValid();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetArrayCapacity(IntPtr L)
	{
		try
		{
			uint arrayCapacity = ((LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetArrayCapacity();
			Lua.xlua_pushuint(L, arrayCapacity);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetInt(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int value = obj.GetInt(index);
			Lua.xlua_pushinteger(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetInt(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			int value = Lua.xlua_tointeger(L, 3);
			obj.SetInt(index, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetDouble(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			double number = obj.GetDouble(index);
			Lua.lua_pushnumber(L, number);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetDouble(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			double value = Lua.lua_tonumber(L, 3);
			obj.SetDouble(index, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetLong(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			long n = obj.GetLong(index);
			Lua.lua_pushint64(L, n);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetLong(IntPtr L)
	{
		try
		{
			LuaArrAccess32 obj = (LuaArrAccess32)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			long value = Lua.lua_toint64(L, 3);
			obj.SetLong(index, value);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
