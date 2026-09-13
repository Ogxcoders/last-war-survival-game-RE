using System;
using GameFramework;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GameFrameworkLogWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Log);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 10, 0, 0);
		Utils.RegisterFunc(L, -4, "Debug", _m_Debug_xlua_st_);
		Utils.RegisterFunc(L, -4, "LUA_Debug", _m_LUA_Debug_xlua_st_);
		Utils.RegisterFunc(L, -4, "PerfLog", _m_PerfLog_xlua_st_);
		Utils.RegisterFunc(L, -4, "Info", _m_Info_xlua_st_);
		Utils.RegisterFunc(L, -4, "Warning", _m_Warning_xlua_st_);
		Utils.RegisterFunc(L, -4, "Error", _m_Error_xlua_st_);
		Utils.RegisterFunc(L, -4, "LUA_Error", _m_LUA_Error_xlua_st_);
		Utils.RegisterFunc(L, -4, "Fatal", _m_Fatal_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogHelper", _m_LogHelper_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 1)
			{
				Log o = new Log();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameFramework.Log constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Debug_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				objectTranslator.GetObject(L, 1, typeof(object));
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Lua.lua_tostring(L, 1);
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2))
			{
				Lua.lua_tostring(L, 1);
				objectTranslator.GetObject(L, 2, typeof(object));
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				Lua.lua_tostring(L, 1);
				objectTranslator.GetParams<object>(L, 2);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3))
			{
				Lua.lua_tostring(L, 1);
				objectTranslator.GetObject(L, 2, typeof(object));
				objectTranslator.GetObject(L, 3, typeof(object));
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				Lua.lua_tostring(L, 1);
				objectTranslator.GetObject(L, 2, typeof(object));
				objectTranslator.GetObject(L, 3, typeof(object));
				objectTranslator.GetObject(L, 4, typeof(object));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameFramework.Log.Debug!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LUA_Debug_xlua_st_(IntPtr L)
	{
		try
		{
			Lua.lua_tostring(L, 1);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PerfLog_xlua_st_(IntPtr L)
	{
		try
		{
			Lua.lua_tostring(L, 1);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Info_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Log.Info(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Log.Info(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2))
			{
				string format = Lua.lua_tostring(L, 1);
				object arg = objectTranslator.GetObject(L, 2, typeof(object));
				Log.Info(format, arg);
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format2 = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Log.Info(format2, args);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3))
			{
				string format3 = Lua.lua_tostring(L, 1);
				object arg2 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg3 = objectTranslator.GetObject(L, 3, typeof(object));
				Log.Info(format3, arg2, arg3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				string format4 = Lua.lua_tostring(L, 1);
				object arg4 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg5 = objectTranslator.GetObject(L, 3, typeof(object));
				object arg6 = objectTranslator.GetObject(L, 4, typeof(object));
				Log.Info(format4, arg4, arg5, arg6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameFramework.Log.Info!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Warning_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Log.Warning(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Log.Warning(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2))
			{
				string format = Lua.lua_tostring(L, 1);
				object arg = objectTranslator.GetObject(L, 2, typeof(object));
				Log.Warning(format, arg);
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format2 = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Log.Warning(format2, args);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3))
			{
				string format3 = Lua.lua_tostring(L, 1);
				object arg2 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg3 = objectTranslator.GetObject(L, 3, typeof(object));
				Log.Warning(format3, arg2, arg3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				string format4 = Lua.lua_tostring(L, 1);
				object arg4 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg5 = objectTranslator.GetObject(L, 3, typeof(object));
				object arg6 = objectTranslator.GetObject(L, 4, typeof(object));
				Log.Warning(format4, arg4, arg5, arg6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameFramework.Log.Warning!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Error_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Log.Error(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Log.Error(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2))
			{
				string format = Lua.lua_tostring(L, 1);
				object arg = objectTranslator.GetObject(L, 2, typeof(object));
				Log.Error(format, arg);
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format2 = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Log.Error(format2, args);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3))
			{
				string format3 = Lua.lua_tostring(L, 1);
				object arg2 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg3 = objectTranslator.GetObject(L, 3, typeof(object));
				Log.Error(format3, arg2, arg3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				string format4 = Lua.lua_tostring(L, 1);
				object arg4 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg5 = objectTranslator.GetObject(L, 3, typeof(object));
				object arg6 = objectTranslator.GetObject(L, 4, typeof(object));
				Log.Error(format4, arg4, arg5, arg6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameFramework.Log.Error!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LUA_Error_xlua_st_(IntPtr L)
	{
		try
		{
			Log.LUA_Error(Lua.lua_tostring(L, 1));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Fatal_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Log.Fatal(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING))
			{
				Log.Fatal(Lua.lua_tostring(L, 1));
				return 0;
			}
			if (num == 2 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2))
			{
				string format = Lua.lua_tostring(L, 1);
				object arg = objectTranslator.GetObject(L, 2, typeof(object));
				Log.Fatal(format, arg);
				return 0;
			}
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format2 = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Log.Fatal(format2, args);
				return 0;
			}
			if (num == 3 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3))
			{
				string format3 = Lua.lua_tostring(L, 1);
				object arg2 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg3 = objectTranslator.GetObject(L, 3, typeof(object));
				Log.Fatal(format3, arg2, arg3);
				return 0;
			}
			if (num == 4 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<object>(L, 3) && objectTranslator.Assignable<object>(L, 4))
			{
				string format4 = Lua.lua_tostring(L, 1);
				object arg4 = objectTranslator.GetObject(L, 2, typeof(object));
				object arg5 = objectTranslator.GetObject(L, 3, typeof(object));
				object arg6 = objectTranslator.GetObject(L, 4, typeof(object));
				Log.Fatal(format4, arg4, arg5, arg6);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GameFramework.Log.Fatal!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogHelper_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out LogLevel v);
			string message = Lua.lua_tostring(L, 2);
			Log.LogHelper(v, message);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
