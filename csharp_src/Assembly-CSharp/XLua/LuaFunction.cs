using System;
using GameFramework;
using XLua.LuaDLL;

namespace XLua;

public class LuaFunction : LuaBase
{
	public LuaFunction(int reference, LuaEnv luaenv)
		: base(reference, luaenv)
	{
	}

	public void Action<T>(T a)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, a);
		if (Lua.lua_pcall(l, 1, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public TResult Func<T, TResult>(T a)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, a);
		if (Lua.lua_pcall(l, 1, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public void Action<T1, T2>(T1 a1, T2 a2)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, a1);
		translator.PushByType(l, a2);
		if (Lua.lua_pcall(l, 2, 0, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		Lua.lua_settop(l, num);
	}

	public TResult Func<T1, T2, TResult>(T1 a1, T2 a2)
	{
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		translator.PushByType(l, a1);
		translator.PushByType(l, a2);
		if (Lua.lua_pcall(l, 2, 1, errfunc) != 0)
		{
			luaEnv.ThrowExceptionFromError(num);
		}
		try
		{
			translator.Get(l, -1, out TResult v);
			return v;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	public object[] Call(object[] args, Type[] returnTypes)
	{
		int nArgs = 0;
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		int oldTop = Lua.lua_gettop(l);
		int num = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		if (args != null)
		{
			nArgs = args.Length;
			for (int i = 0; i < args.Length; i++)
			{
				translator.PushAny(l, args[i]);
			}
		}
		if (Lua.lua_pcall(l, nArgs, -1, num) != 0)
		{
			luaEnv.ThrowExceptionFromError(oldTop);
		}
		Lua.lua_remove(l, num);
		if (returnTypes != null)
		{
			return translator.popValues(l, oldTop, returnTypes);
		}
		return translator.popValues(l, oldTop);
	}

	public object[] Call(params object[] args)
	{
		return Call(args, null);
	}

	public T Cast<T>()
	{
		if (!typeof(T).IsSubclassOf(typeof(Delegate)))
		{
			throw new InvalidOperationException(typeof(T).Name + " is not a delegate type");
		}
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		push(l);
		T result = (T)translator.GetObject(l, -1, typeof(T));
		Lua.lua_pop(luaEnv.L, 1);
		return result;
	}

	public void SetEnv(LuaTable env)
	{
		IntPtr l = luaEnv.L;
		int newTop = Lua.lua_gettop(l);
		push(l);
		env.push(l);
		Lua.lua_setfenv(l, -2);
		Lua.lua_settop(l, newTop);
	}

	internal override void push(IntPtr L)
	{
		Lua.lua_getref(L, luaReference);
	}

	public override string ToString()
	{
		return "function :" + luaReference;
	}

	public Delegate Cast(Type delType)
	{
		if (!delType.IsSubclassOf(typeof(Delegate)))
		{
			throw new InvalidOperationException(delType.Name + " is not a delegate type");
		}
		IntPtr l = luaEnv.L;
		ObjectTranslator translator = luaEnv.translator;
		push(l);
		Delegate result = (Delegate)translator.GetObject(l, -1, delType);
		Lua.lua_pop(luaEnv.L, 1);
		return result;
	}

	public LuaBuildData CallReturnBuildingData(long uuid, int itemId)
	{
		int num = 0;
		IntPtr l = luaEnv.L;
		int num2 = Lua.lua_gettop(l);
		num = num2;
		int num3 = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		if (uuid != 0L)
		{
			Lua.lua_pushint64(l, uuid);
		}
		else
		{
			Lua.xlua_pushinteger(l, itemId);
		}
		if (Lua.lua_pcall(l, 1, -1, num3) != 0)
		{
			Log.Error("CallReturnBuildingData call error!!! --- {0}", uuid);
			Lua.lua_settop(l, num2);
			return null;
		}
		Lua.lua_remove(l, num3);
		int num4 = Lua.lua_gettop(l);
		if (num4 - num != 6)
		{
			_ = 0;
			Lua.lua_settop(l, num2);
			return null;
		}
		long uid = Lua.lua_toint64(l, -6);
		long updateTime = Lua.lua_toint64(l, -5);
		int point = Lua.xlua_tointeger(l, -4);
		int tempState = Lua.xlua_tointeger(l, -3);
		int itemId2 = Lua.xlua_tointeger(l, -2);
		int lv = Lua.xlua_tointeger(l, -1);
		Lua.lua_settop(l, num2);
		return new LuaBuildData(uid, updateTime, point, tempState, itemId2, lv);
	}

	public void CallForPushTable(string cmd, object table)
	{
		IntPtr l = luaEnv.L;
		int num = Lua.lua_gettop(l);
		int errfunc = Lua.load_error_func(l, luaEnv.errorFuncRef);
		Lua.lua_getref(l, luaReference);
		Lua.lua_pushstring(l, cmd);
		if (table is LuaTable)
		{
			((LuaTable)table).push(l);
		}
		else
		{
			((LuaStackTable)table).push();
			num--;
		}
		int num2 = Lua.lua_pcall(l, 2, 0, errfunc);
		if (num2 != 0)
		{
			string arg = Lua.lua_tostring(l, -1);
			Log.Error("CallForPushTable error {0}, cmd{1}, message: {2}", num2, cmd, arg);
			Lua.lua_settop(l, num);
		}
		else
		{
			Lua.lua_settop(l, num);
		}
	}
}
