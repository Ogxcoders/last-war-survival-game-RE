using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceMeteoriteHitPlaneEffectManagerBridge : LuaBase, MeteoriteHitPlaneEffectManager
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceMeteoriteHitPlaneEffectManagerBridge(reference, luaenv);
	}

	public LuaScriptInterfaceMeteoriteHitPlaneEffectManagerBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	bool MeteoriteHitPlaneEffectManager.IsCanShowEffectByPoint(int point)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "IsCanShowEffectByPoint");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function IsCanShowEffectByPoint");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.xlua_pushinteger(l, point);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		bool result = Lua.lua_toboolean(l, num + 1);
		Lua.lua_settop(l, num - 1);
		return result;
	}
}
