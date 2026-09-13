using System;
using System.Text;
using GameFramework;
using UnityEngine;
using XLua;
using XLua.LuaDLL;

public class LuaMonoBehaviour : MonoBehaviour
{
	[Serializable]
	public class Objects
	{
		public string name;

		public GameObject value;
	}

	[Serializable]
	public class Values
	{
		public string name;

		public string value;
	}

	public string _luaPath;

	public Values[] _values;

	public Objects[] _objects;

	private static readonly int LUA_REFNIL = -1;

	private int _luaTableReference = LUA_REFNIL;

	private int _Awake = LUA_REFNIL;

	private int _Start = LUA_REFNIL;

	private int _OnDestroy = LUA_REFNIL;

	private int _OnEnable = LUA_REFNIL;

	private int _OnDisable = LUA_REFNIL;

	private int _OnCollisionEnter = LUA_REFNIL;

	private int _OnCollisionExit = LUA_REFNIL;

	private int _OnTriggerEnterAction = LUA_REFNIL;

	private int _OnTriggerExitAction = LUA_REFNIL;

	private int _OnAnimationAction = LUA_REFNIL;

	private int _OnApplicationFocus = LUA_REFNIL;

	private int _OnApplicationPause = LUA_REFNIL;

	private static char[] globalBuf = new char[256];

	private static StringBuilder globalSB = new StringBuilder(256);

	private LuaMonoBehaviour()
	{
	}

	private void MakeLuaBytes(out byte[] bytes, out int bytes_len)
	{
		if (string.IsNullOrEmpty(_luaPath))
		{
			bytes = null;
			bytes_len = 0;
			return;
		}
		globalSB.Clear();
		globalSB.Append("return require '");
		globalSB.Append(_luaPath);
		globalSB.Append("'");
		globalSB.CopyTo(0, globalBuf, 0, globalSB.Length);
		int length = globalSB.Length;
		if (length > InternalGlobals.strBuff.Length)
		{
			bytes = Encoding.UTF8.GetBytes(globalBuf, 0, length);
			bytes_len = bytes.Length;
		}
		else
		{
			bytes_len = Encoding.UTF8.GetBytes(globalBuf, 0, length, InternalGlobals.strBuff, 0);
			bytes = InternalGlobals.strBuff;
		}
	}

	private void Awake()
	{
		MakeLuaBytes(out var bytes, out var bytes_len);
		if (bytes_len > 0)
		{
			_luaTableReference = GetLuaTable(bytes, bytes_len);
			if (_luaTableReference == LUA_REFNIL)
			{
				Log.Error("LuaMonoBehaviour Awake error : {0}", _luaPath ?? "");
				return;
			}
			Set(_luaTableReference, "Mono", this);
			GetFunctions();
			CallLuaFunc(_Awake);
			CallLuaFunc(_Start);
		}
	}

	private void OnDestroy()
	{
		CallLuaFunc(_OnDestroy);
		UnRefAll();
	}

	private void OnEnable()
	{
		CallLuaFunc(_OnEnable);
	}

	private void OnDisable()
	{
		CallLuaFunc(_OnDisable);
	}

	private void OnCollisionEnter(Collision other)
	{
		CallLuaFunc(_OnCollisionEnter, other);
	}

	private void OnCollisionExit(Collision other)
	{
		CallLuaFunc(_OnCollisionExit, other);
	}

	private void OnTriggerEnter(Collider other)
	{
		CallLuaFunc(_OnTriggerEnterAction, other);
	}

	private void OnTriggerExit(Collider other)
	{
		CallLuaFunc(_OnTriggerExitAction, other);
	}

	private void OnAnimationAction(string name)
	{
		CallLuaFunc(_OnAnimationAction, name);
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		CallLuaFunc(_OnApplicationFocus, hasFocus);
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		CallLuaFunc(_OnApplicationPause, pauseStatus);
	}

	private void CallLuaFunc(int funcref)
	{
		CallLuaFunc<object>(funcref, 0, null);
	}

	private void CallLuaFunc<T>(int funcref, T para)
	{
		CallLuaFunc(funcref, 1, para);
	}

	private void CallLuaFunc<T>(int funcref, int para_count, T para)
	{
		if (funcref <= 0)
		{
			return;
		}
		if (GameEntry.Lua == null || GameEntry.Lua.Env == null)
		{
			Log.Info("CallLuaFunc lua not ready!");
			return;
		}
		LuaEnv env = GameEntry.Lua.Env;
		IntPtr l = env.L;
		ObjectTranslator translator = env.translator;
		int num = Lua.lua_gettop(l);
		try
		{
			int num2 = Lua.load_error_func(l, env.errorFuncRef);
			Lua.lua_getref(l, funcref);
			Lua.lua_gettop(l);
			Lua.lua_getref(l, _luaTableReference);
			Lua.lua_gettop(l);
			if (para_count > 0)
			{
				translator.PushAny(l, para);
				para_count = 2;
			}
			else
			{
				para_count = 1;
			}
			Lua.lua_gettop(l);
			if (Lua.lua_pcall(l, para_count, -1, num2) != 0)
			{
				env.ThrowExceptionFromError(num);
			}
			Lua.lua_remove(l, num2);
		}
		catch (Exception ex)
		{
			Log.Error("CallLuaFunc error!\n{0}", ex.Message);
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	private int GetFunction(string funcName)
	{
		IntPtr l = GameEntry.Lua.Env.L;
		ObjectTranslator translator = GameEntry.Lua.Env.translator;
		int newTop = Lua.lua_gettop(l);
		int result = LUA_REFNIL;
		translator.PushByType(l, funcName);
		Lua.lua_type(l, -1);
		Lua.xlua_pgettable(l, -2);
		if (Lua.lua_type(l, -1) == LuaTypes.LUA_TFUNCTION)
		{
			result = Lua.luaL_ref(l, LuaIndexes.LUA_REGISTRYINDEX);
		}
		Lua.lua_settop(l, newTop);
		return result;
	}

	private void GetFunctions()
	{
		IntPtr l = GameEntry.Lua.Env.L;
		_ = GameEntry.Lua.Env.translator;
		int newTop = Lua.lua_gettop(l);
		try
		{
			Lua.lua_getref(l, _luaTableReference);
			_Awake = GetFunction("Awake");
			_Start = GetFunction("Start");
			_OnDestroy = GetFunction("OnDestroy");
			_OnEnable = GetFunction("OnEnable");
			_OnDisable = GetFunction("OnDisable");
			_OnCollisionEnter = GetFunction("OnCollisionEnter");
			_OnCollisionExit = GetFunction("OnCollisionExit");
			_OnTriggerEnterAction = GetFunction("OnTriggerEnterAction");
			_OnTriggerExitAction = GetFunction("OnTriggerExitAction");
			_OnAnimationAction = GetFunction("OnAnimationAction");
			_OnApplicationFocus = GetFunction("_OnApplicationFocus");
			_OnApplicationPause = GetFunction("_OnApplicationPause");
		}
		catch (Exception)
		{
			Log.Error("GetFunctions error!");
		}
		finally
		{
			Lua.lua_settop(l, newTop);
		}
	}

	private int GetLuaTable(byte[] bytes, int bytes_len)
	{
		if (GameEntry.Lua == null || GameEntry.Lua.Env == null)
		{
			Log.Info("GetLuaTable Lua not ready!");
			return LUA_REFNIL;
		}
		LuaEnv env = GameEntry.Lua.Env;
		IntPtr l = env.L;
		int num = Lua.lua_gettop(l);
		int result = LUA_REFNIL;
		try
		{
			int num2 = Lua.load_error_func(l, env.errorFuncRef);
			if (Lua.xluaL_loadbuffer(l, bytes, bytes_len, "") == 0)
			{
				if (Lua.lua_pcall(l, 0, -1, num2) == 0)
				{
					Lua.lua_remove(l, num2);
				}
				else
				{
					env.ThrowExceptionFromError(num);
				}
			}
			else
			{
				env.ThrowExceptionFromError(num);
			}
			if (Lua.lua_type(l, num + 1) == LuaTypes.LUA_TTABLE)
			{
				result = Lua.luaL_ref(l, LuaIndexes.LUA_REGISTRYINDEX);
			}
		}
		catch (Exception)
		{
			Log.Error("GetLuaTable error!");
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
		return result;
	}

	private void Set<TKey, TValue>(int luaref, TKey key, TValue value)
	{
		LuaEnv env = GameEntry.Lua.Env;
		IntPtr l = env.L;
		int num = Lua.lua_gettop(l);
		try
		{
			ObjectTranslator translator = env.translator;
			Lua.lua_getref(l, luaref);
			translator.PushByType(l, key);
			translator.PushByType(l, value);
			if (Lua.xlua_psettable(l, -3) != 0)
			{
				env.ThrowExceptionFromError(num);
			}
		}
		catch (Exception)
		{
			Log.Error("Set error!");
		}
		finally
		{
			Lua.lua_settop(l, num);
		}
	}

	private void UnRef(ref int funcref)
	{
		IntPtr l = GameEntry.Lua.Env.L;
		if (funcref > 0)
		{
			Lua.lua_unref(l, funcref);
			funcref = LUA_REFNIL;
		}
	}

	private void UnRefAll()
	{
		if (GameEntry.Lua == null || GameEntry.Lua.Env == null)
		{
			Log.Info("UnRefAll lua not ready!");
			return;
		}
		UnRef(ref _Awake);
		UnRef(ref _Start);
		UnRef(ref _OnDestroy);
		UnRef(ref _OnEnable);
		UnRef(ref _OnDisable);
		UnRef(ref _OnCollisionEnter);
		UnRef(ref _OnCollisionExit);
		UnRef(ref _OnTriggerEnterAction);
		UnRef(ref _OnTriggerExitAction);
		UnRef(ref _OnAnimationAction);
		UnRef(ref _OnApplicationFocus);
		UnRef(ref _OnApplicationPause);
	}
}
