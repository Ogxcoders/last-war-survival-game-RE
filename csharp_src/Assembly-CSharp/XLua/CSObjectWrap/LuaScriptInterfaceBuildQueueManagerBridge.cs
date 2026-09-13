using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceBuildQueueManagerBridge : LuaBase, BuildQueueManager
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceBuildQueueManagerBridge(reference, luaenv);
	}

	public LuaScriptInterfaceBuildQueueManagerBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	void BuildQueueManager.UpdateQueueData(LuaTable message)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "UpdateQueueData");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function UpdateQueueData");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		translator.Push(l, message);
		if (Lua.lua_pcall(l, 2, 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}
}
