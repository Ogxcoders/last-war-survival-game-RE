using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineDebugWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Debug);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 17, 3, 1);
		Utils.RegisterFunc(L, -4, "DrawLine", _m_DrawLine_xlua_st_);
		Utils.RegisterFunc(L, -4, "DrawRay", _m_DrawRay_xlua_st_);
		Utils.RegisterFunc(L, -4, "Break", _m_Break_xlua_st_);
		Utils.RegisterFunc(L, -4, "DebugBreak", _m_DebugBreak_xlua_st_);
		Utils.RegisterFunc(L, -4, "Log", _m_Log_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogFormat", _m_LogFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogError", _m_LogError_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogErrorFormat", _m_LogErrorFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "ClearDeveloperConsole", _m_ClearDeveloperConsole_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogException", _m_LogException_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogWarning", _m_LogWarning_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogWarningFormat", _m_LogWarningFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "Assert", _m_Assert_xlua_st_);
		Utils.RegisterFunc(L, -4, "AssertFormat", _m_AssertFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogAssertion", _m_LogAssertion_xlua_st_);
		Utils.RegisterFunc(L, -4, "LogAssertionFormat", _m_LogAssertionFormat_xlua_st_);
		Utils.RegisterFunc(L, -2, "unityLogger", _g_get_unityLogger);
		Utils.RegisterFunc(L, -2, "developerConsoleVisible", _g_get_developerConsoleVisible);
		Utils.RegisterFunc(L, -2, "isDebugBuild", _g_get_isDebugBuild);
		Utils.RegisterFunc(L, -1, "developerConsoleVisible", _s_set_developerConsoleVisible);
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
				Debug o = new Debug();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawLine_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				Debug.DrawLine(val, val2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Color>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Color val5);
				Debug.DrawLine(val3, val4, val5);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Color>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				objectTranslator.Get(L, 3, out Color val8);
				float duration = (float)Lua.lua_tonumber(L, 4);
				Debug.DrawLine(val6, val7, val8, duration);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Color>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				objectTranslator.Get(L, 3, out Color val11);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				bool depthTest = Lua.lua_toboolean(L, 5);
				Debug.DrawLine(val9, val10, val11, duration2, depthTest);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.DrawLine!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DrawRay_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 1, out Vector3 val);
				objectTranslator.Get(L, 2, out Vector3 val2);
				Debug.DrawRay(val, val2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Color>(L, 3))
			{
				objectTranslator.Get(L, 1, out Vector3 val3);
				objectTranslator.Get(L, 2, out Vector3 val4);
				objectTranslator.Get(L, 3, out Color val5);
				Debug.DrawRay(val3, val4, val5);
				return 0;
			}
			if (num == 4 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Color>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				objectTranslator.Get(L, 1, out Vector3 val6);
				objectTranslator.Get(L, 2, out Vector3 val7);
				objectTranslator.Get(L, 3, out Color val8);
				float duration = (float)Lua.lua_tonumber(L, 4);
				Debug.DrawRay(val6, val7, val8, duration);
				return 0;
			}
			if (num == 5 && objectTranslator.Assignable<Vector3>(L, 1) && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Color>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 5))
			{
				objectTranslator.Get(L, 1, out Vector3 val9);
				objectTranslator.Get(L, 2, out Vector3 val10);
				objectTranslator.Get(L, 3, out Color val11);
				float duration2 = (float)Lua.lua_tonumber(L, 4);
				bool depthTest = Lua.lua_toboolean(L, 5);
				Debug.DrawRay(val9, val10, val11, duration2, depthTest);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.DrawRay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Break_xlua_st_(IntPtr L)
	{
		try
		{
			Debug.Break();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugBreak_xlua_st_(IntPtr L)
	{
		try
		{
			Debug.DebugBreak();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Log_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Debug.Log(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				object message = objectTranslator.GetObject(L, 1, typeof(object));
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				Debug.Log(message, context);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.Log!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Debug.LogFormat(format, args);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<object>(L, 3)))
			{
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				string format2 = Lua.lua_tostring(L, 2);
				object[] args2 = objectTranslator.GetParams<object>(L, 3);
				Debug.LogFormat(context, format2, args2);
				return 0;
			}
			if (num >= 4 && objectTranslator.Assignable<LogType>(L, 1) && objectTranslator.Assignable<LogOption>(L, 2) && objectTranslator.Assignable<UnityEngine.Object>(L, 3) && (Lua.lua_isnil(L, 4) || Lua.lua_type(L, 4) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 5) || objectTranslator.Assignable<object>(L, 5)))
			{
				objectTranslator.Get(L, 1, out LogType v);
				objectTranslator.Get(L, 2, out LogOption v2);
				UnityEngine.Object context2 = (UnityEngine.Object)objectTranslator.GetObject(L, 3, typeof(UnityEngine.Object));
				string format3 = Lua.lua_tostring(L, 4);
				object[] args3 = objectTranslator.GetParams<object>(L, 5);
				Debug.LogFormat(v, v2, context2, format3, args3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogFormat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogError_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Debug.LogError(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				object message = objectTranslator.GetObject(L, 1, typeof(object));
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				Debug.LogError(message, context);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogError!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogErrorFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Debug.LogErrorFormat(format, args);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<object>(L, 3)))
			{
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				string format2 = Lua.lua_tostring(L, 2);
				object[] args2 = objectTranslator.GetParams<object>(L, 3);
				Debug.LogErrorFormat(context, format2, args2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogErrorFormat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearDeveloperConsole_xlua_st_(IntPtr L)
	{
		try
		{
			Debug.ClearDeveloperConsole();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogException_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<Exception>(L, 1))
			{
				Debug.LogException((Exception)objectTranslator.GetObject(L, 1, typeof(Exception)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<Exception>(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				Exception exception = (Exception)objectTranslator.GetObject(L, 1, typeof(Exception));
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				Debug.LogException(exception, context);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogException!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogWarning_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && objectTranslator.Assignable<object>(L, 1))
			{
				Debug.LogWarning(objectTranslator.GetObject(L, 1, typeof(object)));
				return 0;
			}
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				object message = objectTranslator.GetObject(L, 1, typeof(object));
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				Debug.LogWarning(message, context);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogWarning!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogWarningFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				string format = Lua.lua_tostring(L, 1);
				object[] args = objectTranslator.GetParams<object>(L, 2);
				Debug.LogWarningFormat(format, args);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<object>(L, 3)))
			{
				UnityEngine.Object context = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				string format2 = Lua.lua_tostring(L, 2);
				object[] args2 = objectTranslator.GetParams<object>(L, 3);
				Debug.LogWarningFormat(context, format2, args2);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogWarningFormat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Assert_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
			{
				Lua.lua_toboolean(L, 1);
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				Lua.lua_toboolean(L, 1);
				_ = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && objectTranslator.Assignable<object>(L, 2))
			{
				Lua.lua_toboolean(L, 1);
				objectTranslator.GetObject(L, 2, typeof(object));
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				Lua.lua_toboolean(L, 1);
				Lua.lua_tostring(L, 2);
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && objectTranslator.Assignable<object>(L, 2) && objectTranslator.Assignable<UnityEngine.Object>(L, 3))
			{
				Lua.lua_toboolean(L, 1);
				objectTranslator.GetObject(L, 2, typeof(object));
				_ = (UnityEngine.Object)objectTranslator.GetObject(L, 3, typeof(UnityEngine.Object));
				return 0;
			}
			if (num == 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<UnityEngine.Object>(L, 3))
			{
				Lua.lua_toboolean(L, 1);
				Lua.lua_tostring(L, 2);
				_ = (UnityEngine.Object)objectTranslator.GetObject(L, 3, typeof(UnityEngine.Object));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.Assert!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AssertFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 2 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<object>(L, 3)))
			{
				Lua.lua_toboolean(L, 1);
				Lua.lua_tostring(L, 2);
				objectTranslator.GetParams<object>(L, 3);
				return 0;
			}
			if (num >= 3 && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2) && (Lua.lua_isnil(L, 3) || Lua.lua_type(L, 3) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 4) || objectTranslator.Assignable<object>(L, 4)))
			{
				Lua.lua_toboolean(L, 1);
				_ = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				Lua.lua_tostring(L, 3);
				objectTranslator.GetParams<object>(L, 4);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.AssertFormat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogAssertion_xlua_st_(IntPtr L)
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
			if (num == 2 && objectTranslator.Assignable<object>(L, 1) && objectTranslator.Assignable<UnityEngine.Object>(L, 2))
			{
				objectTranslator.GetObject(L, 1, typeof(object));
				_ = (UnityEngine.Object)objectTranslator.GetObject(L, 2, typeof(UnityEngine.Object));
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogAssertion!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LogAssertionFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num >= 1 && (Lua.lua_isnil(L, 1) || Lua.lua_type(L, 1) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 2) || objectTranslator.Assignable<object>(L, 2)))
			{
				Lua.lua_tostring(L, 1);
				objectTranslator.GetParams<object>(L, 2);
				return 0;
			}
			if (num >= 2 && objectTranslator.Assignable<UnityEngine.Object>(L, 1) && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaTypes.LUA_TNONE == Lua.lua_type(L, 3) || objectTranslator.Assignable<object>(L, 3)))
			{
				_ = (UnityEngine.Object)objectTranslator.GetObject(L, 1, typeof(UnityEngine.Object));
				Lua.lua_tostring(L, 2);
				objectTranslator.GetParams<object>(L, 3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Debug.LogAssertionFormat!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_unityLogger(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushAny(L, Debug.unityLogger);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_developerConsoleVisible(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Debug.developerConsoleVisible);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDebugBuild(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, Debug.isDebugBuild);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_developerConsoleVisible(IntPtr L)
	{
		try
		{
			Debug.developerConsoleVisible = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
