using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceUIManagerBridge : LuaBase, UIManager
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceUIManagerBridge(reference, luaenv);
	}

	public LuaScriptInterfaceUIManagerBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	void UIManager.OpenWindow(string uiName, object[] args)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "OpenWindow");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function OpenWindow");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.lua_pushstring(l, uiName);
		if (args != null)
		{
			for (int i = 0; i < args.Length; i++)
			{
				translator.PushAny(l, args[i]);
			}
		}
		if (Lua.lua_pcall(l, 2 + ((args != null) ? args.Length : 0), 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}

	void UIManager.DestroyWindow(string uiName, object[] args)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "DestroyWindow");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function DestroyWindow");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.lua_pushstring(l, uiName);
		if (args != null)
		{
			for (int i = 0; i < args.Length; i++)
			{
				translator.PushAny(l, args[i]);
			}
		}
		if (Lua.lua_pcall(l, 2 + ((args != null) ? args.Length : 0), 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}

	bool UIManager.IsWindowOpen(string uiName)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "IsWindowOpen");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function IsWindowOpen");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.lua_pushstring(l, uiName);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		bool result = Lua.lua_toboolean(l, num + 1);
		Lua.lua_settop(l, num - 1);
		return result;
	}
}
