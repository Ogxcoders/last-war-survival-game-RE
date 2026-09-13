using System;
using System.Collections;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class SystemCollectionsIEnumeratorBridge : LuaBase, IEnumerator
{
	object IEnumerator.Current
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			ObjectTranslator translator = luaEnv.translator;
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "Current");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			object result = translator.GetObject(l, -1, typeof(object));
			Lua.lua_pop(l, 2);
			return result;
		}
	}

	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new SystemCollectionsIEnumeratorBridge(reference, luaenv);
	}

	public SystemCollectionsIEnumeratorBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	bool IEnumerator.MoveNext()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "MoveNext");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function MoveNext");
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

	void IEnumerator.Reset()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "Reset");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function Reset");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		if (Lua.lua_pcall(l, 1, 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}
}
