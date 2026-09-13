using System;
using LuaScriptInterface;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class LuaScriptInterfaceQueueDataBridge : LuaBase, QueueData
{
	long QueueData.uuid
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "uuid");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			long result = Lua.lua_toint64(l, -1);
			Lua.lua_pop(l, 2);
			return result;
		}
		set
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "uuid");
			Lua.lua_pushint64(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			Lua.lua_pop(l, 1);
		}
	}

	string QueueData.itemId
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "itemId");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			string result = Lua.lua_tostring(l, -1);
			Lua.lua_pop(l, 2);
			return result;
		}
		set
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "itemId");
			Lua.lua_pushstring(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			Lua.lua_pop(l, 1);
		}
	}

	long QueueData.startTime
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "startTime");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			long result = Lua.lua_toint64(l, -1);
			Lua.lua_pop(l, 2);
			return result;
		}
		set
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "startTime");
			Lua.lua_pushint64(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			Lua.lua_pop(l, 1);
		}
	}

	long QueueData.endTime
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "endTime");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			long result = Lua.lua_toint64(l, -1);
			Lua.lua_pop(l, 2);
			return result;
		}
		set
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "endTime");
			Lua.lua_pushint64(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			Lua.lua_pop(l, 1);
		}
	}

	string QueueData.newItemId
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "newItemId");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			string result = Lua.lua_tostring(l, -1);
			Lua.lua_pop(l, 2);
			return result;
		}
		set
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "newItemId");
			Lua.lua_pushstring(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			Lua.lua_pop(l, 1);
		}
	}

	int QueueData.type
	{
		get
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "type");
			if (Lua.xlua_pgettable(l, -2) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			int result = Lua.xlua_tointeger(l, -1);
			Lua.lua_pop(l, 2);
			return result;
		}
		set
		{
			IntPtr l = luaEnv.L;
			int oldTop = Lua.lua_gettop(l);
			Lua.lua_getref(l, luaReference);
			Lua.xlua_pushasciistring(l, "type");
			Lua.xlua_pushinteger(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				luaEnv.ThrowExceptionFromError(oldTop);
			}
			Lua.lua_pop(l, 1);
		}
	}

	public static LuaBase __Create(int reference, LuaEnv luaenv)
	{
		return new LuaScriptInterfaceQueueDataBridge(reference, luaenv);
	}

	public LuaScriptInterfaceQueueDataBridge(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	int QueueData.GetQueueState()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetQueueState");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetQueueState");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		if (Lua.lua_pcall(l, 1, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		int result = Lua.xlua_tointeger(l, num + 1);
		Lua.lua_settop(l, num - 1);
		return result;
	}

	int QueueData.GetParaState()
	{
		IntPtr l = luaEnv.L;
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.xlua_pushasciistring(l, "GetParaState");
		if (Lua.xlua_pgettable(l, -2) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		if (!Lua.lua_isfunction(l, -1))
		{
			Lua.xlua_pushasciistring(l, "no such function GetParaState");
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		Lua.lua_pushvalue(l, -2);
		Lua.lua_remove(l, -3);
		if (Lua.lua_pcall(l, 1, 1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(num - 1);
		}
		int result = Lua.xlua_tointeger(l, num + 1);
		Lua.lua_settop(l, num - 1);
		return result;
	}
}
