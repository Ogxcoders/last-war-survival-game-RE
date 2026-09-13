using System;
using XLua.LuaDLL;

public struct LuaStackTable
{
	private int oldTop;

	private IntPtr L;

	public LuaStackTable(IntPtr L1)
	{
		L = L1;
		oldTop = Lua.lua_gettop(L);
		Lua.lua_newtable(L);
	}

	public void SetInt(string key, int value)
	{
		Lua.lua_pushstring(L, key);
		Lua.xlua_pushinteger(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetInt(int key, int value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.xlua_pushinteger(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetLong(string key, long value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushint64(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetLong(int key, long value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushint64(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetDouble(string key, double value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushnumber(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetDouble(int key, double value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushnumber(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetString(string key, string value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushstring(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetString(int key, string value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushstring(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetBool(string key, bool value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushboolean(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetBool(int key, bool value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushboolean(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetULong(string key, ulong value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushuint64(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetULong(int key, ulong value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushuint64(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetByte(string key, int value)
	{
		Lua.lua_pushstring(L, key);
		Lua.xlua_pushinteger(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetByte(int key, int value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.xlua_pushinteger(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetShort(string key, short value)
	{
		Lua.lua_pushstring(L, key);
		Lua.xlua_pushinteger(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetShort(int key, short value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.xlua_pushinteger(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetFloat(string key, float value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushnumber(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetFloat(int key, float value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushnumber(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetBytes(string key, byte[] value)
	{
		Lua.lua_pushstring(L, key);
		Lua.lua_pushstring(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetBytes(int key, byte[] value)
	{
		Lua.xlua_pushinteger(L, key);
		Lua.lua_pushstring(L, value);
		Lua.lua_rawset(L, -3);
	}

	public void SetStackTable(string key, LuaStackTable t)
	{
		Lua.lua_rawset(L, -3);
	}

	public void SetStackTable(int key, LuaStackTable t)
	{
		Lua.lua_rawset(L, -3);
	}

	public void push()
	{
		Lua.lua_pushvalue(L, oldTop + 1);
	}
}
