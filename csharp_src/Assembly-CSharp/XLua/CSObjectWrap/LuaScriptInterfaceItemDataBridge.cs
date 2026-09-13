using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceItemDataBridge : LuaBase, ItemData
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceItemDataBridge(reference, luaenv);
	}

	public LuaScriptInterfaceItemDataBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	void ItemData.UpdateItems(LuaTable data)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "UpdateItems");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function UpdateItems");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		translator.Push(l, data);
		if (Lua.lua_pcall(l, 2, 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}

	void ItemData.UpdateOneItem(LuaTable data)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "UpdateOneItem");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function UpdateOneItem");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		translator.Push(l, data);
		if (Lua.lua_pcall(l, 2, 0, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_settop(l, num - 1);
	}

	ItemInfo ItemData.GetItemById(string itemId)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetItemById");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetItemById");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.lua_pushstring(l, itemId);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		ItemInfo result = (ItemInfo)translator.GetObject(l, num + 1, typeof(ItemInfo));
		Lua.lua_settop(l, num - 1);
		return result;
	}

	StatusItemData ItemData.GetStatusItem(int type)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetStatusItem");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetStatusItem");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.xlua_pushinteger(l, type);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		StatusItemData result = (StatusItemData)translator.GetObject(l, num + 1, typeof(StatusItemData));
		Lua.lua_settop(l, num - 1);
		return result;
	}
}
