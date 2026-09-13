using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceEarthOrderDataManagerBridge : LuaBase, EarthOrderDataManager
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceEarthOrderDataManagerBridge(reference, luaenv);
	}

	public LuaScriptInterfaceEarthOrderDataManagerBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	bool EarthOrderDataManager.IsShowEarthOrder()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "IsShowEarthOrder");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function IsShowEarthOrder");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		if (Lua.lua_pcall(l, 1, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		bool result = Lua.lua_toboolean(l, num + 1);
		Lua.lua_settop(l, num - 1);
		return result;
	}
}
