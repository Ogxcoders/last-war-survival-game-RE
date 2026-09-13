using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceQueueDataManagerBridge : LuaBase, QueueDataManager
{
	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceQueueDataManagerBridge(reference, luaenv);
	}

	public LuaScriptInterfaceQueueDataManagerBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	void QueueDataManager.UpdateQueueData(LuaTable message)
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

	QueueData QueueDataManager.GetQueueByType(int qType)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetQueueByType");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetQueueByType");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.xlua_pushinteger(l, qType);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		QueueData result = (QueueData)translator.GetObject(l, num + 1, typeof(QueueData));
		Lua.lua_settop(l, num - 1);
		return result;
	}

	void QueueDataManager.ResetAllQueue()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "ResetAllQueue");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function ResetAllQueue");
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

	QueueData QueueDataManager.GetQueueByBuildUuidForFarm(long bUuid)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		ObjectTranslator translator = luaEnv.translator;
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetQueueByBuildUuidForFarm");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetQueueByBuildUuidForFarm");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.lua_pushint64(l, bUuid);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		QueueData result = (QueueData)translator.GetObject(l, num + 1, typeof(QueueData));
		Lua.lua_settop(l, num - 1);
		return result;
	}

	bool QueueDataManager.GetCanPlantForPastureByBuildUuid(long bUuid)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetCanPlantForPastureByBuildUuid");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetCanPlantForPastureByBuildUuid");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		Lua.lua_pushint64(l, bUuid);
		if (Lua.lua_pcall(l, 2, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		bool result = Lua.lua_toboolean(l, num + 1);
		Lua.lua_settop(l, num - 1);
		return result;
	}
}
