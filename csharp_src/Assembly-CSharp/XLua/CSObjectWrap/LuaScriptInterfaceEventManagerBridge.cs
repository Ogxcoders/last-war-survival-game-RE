using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceEventManagerBridge : LuaBase, EventManager
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceEventManagerBridge(reference, luaenv);
	}

	public LuaScriptInterfaceEventManagerBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	void EventManager.DispatchCSEvent(int eventId, object userData)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "DispatchCSEvent");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function DispatchCSEvent");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.xlua_pushinteger(l, eventId);
		translator.PushAny(l, userData);
		if (Lua.lua_pcall(l, 3, 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}

	void EventManager.DispatchCSEventSFSObject(int eventId, byte[] sfsObjBinary)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "DispatchCSEventSFSObject");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function DispatchCSEventSFSObject");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.xlua_pushinteger(l, eventId);
		Lua.lua_pushstring(l, sfsObjBinary);
		if (Lua.lua_pcall(l, 3, 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}
}
