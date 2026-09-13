using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class BasePoolWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(BasePool);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 2, 0);
		Utils.RegisterFunc(L, -3, "Spawn", _m_Spawn);
		Utils.RegisterFunc(L, -3, "DeSpawn", _m_DeSpawn);
		Utils.RegisterFunc(L, -3, "Clear", _m_Clear);
		Utils.RegisterFunc(L, -3, "DeSpawnImmediate", _m_DeSpawnImmediate);
		Utils.RegisterFunc(L, -3, "BeforeInstantiate", _m_BeforeInstantiate);
		Utils.RegisterFunc(L, -3, "GetPoolCount", _m_GetPoolCount);
		Utils.RegisterFunc(L, -3, "GetObjCount", _m_GetObjCount);
		Utils.RegisterFunc(L, -3, "OnClear", _e_OnClear);
		Utils.RegisterFunc(L, -2, "Request", _g_get_Request);
		Utils.RegisterFunc(L, -2, "prefabPath", _g_get_prefabPath);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "BasePool does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Spawn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GameObject o = ((BasePool)objectTranslator.FastGetCSObj(L, 1)).Spawn();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeSpawn(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BasePool basePool = (BasePool)objectTranslator.FastGetCSObj(L, 1);
			GameObject obj = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			basePool.DeSpawn(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clear(IntPtr L)
	{
		try
		{
			bool value = ((BasePool)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Clear();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DeSpawnImmediate(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BasePool basePool = (BasePool)objectTranslator.FastGetCSObj(L, 1);
			GameObject obj = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
			basePool.DeSpawnImmediate(obj);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_BeforeInstantiate(IntPtr L)
	{
		try
		{
			((BasePool)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).BeforeInstantiate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPoolCount(IntPtr L)
	{
		try
		{
			int poolCount = ((BasePool)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetPoolCount();
			Lua.xlua_pushinteger(L, poolCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetObjCount(IntPtr L)
	{
		try
		{
			int objCount = ((BasePool)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetObjCount();
			Lua.xlua_pushinteger(L, objCount);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Request(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			BasePool basePool = (BasePool)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, basePool.Request);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_prefabPath(IntPtr L)
	{
		try
		{
			BasePool basePool = (BasePool)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, basePool.prefabPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_OnClear(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			BasePool basePool = (BasePool)objectTranslator.FastGetCSObj(L, 1);
			Action action = objectTranslator.GetDelegate<Action>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					basePool.OnClear += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					basePool.OnClear -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to BasePool.OnClear!");
		return 0;
	}
}
