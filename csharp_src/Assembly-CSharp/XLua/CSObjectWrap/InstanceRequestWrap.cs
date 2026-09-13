using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class InstanceRequestWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(InstanceRequest);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 8, 4);
		Utils.RegisterFunc(L, -3, "Instantiate", _m_Instantiate);
		Utils.RegisterFunc(L, -3, "RealDestroy", _m_RealDestroy);
		Utils.RegisterFunc(L, -3, "Destroy", _m_Destroy);
		Utils.RegisterFunc(L, -3, "Update", _m_Update);
		Utils.RegisterFunc(L, -3, "completed", _e_completed);
		Utils.RegisterFunc(L, -2, "PrefabPath", _g_get_PrefabPath);
		Utils.RegisterFunc(L, -2, "isDone", _g_get_isDone);
		Utils.RegisterFunc(L, -2, "isError", _g_get_isError);
		Utils.RegisterFunc(L, -2, "poolIsReady", _g_get_poolIsReady);
		Utils.RegisterFunc(L, -2, "state", _g_get_state);
		Utils.RegisterFunc(L, -2, "gameObject", _g_get_gameObject);
		Utils.RegisterFunc(L, -2, "isUseCache", _g_get_isUseCache);
		Utils.RegisterFunc(L, -2, "_poolTag", _g_get__poolTag);
		Utils.RegisterFunc(L, -1, "state", _s_set_state);
		Utils.RegisterFunc(L, -1, "gameObject", _s_set_gameObject);
		Utils.RegisterFunc(L, -1, "isUseCache", _s_set_isUseCache);
		Utils.RegisterFunc(L, -1, "_poolTag", _s_set__poolTag);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			if (Lua.lua_gettop(L) == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Func<string, BasePool>>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string prefabPath = Lua.lua_tostring(L, 2);
				Func<string, BasePool> createPoolFunc = objectTranslator.GetDelegate<Func<string, BasePool>>(L, 3);
				int priority = Lua.xlua_tointeger(L, 4);
				InstanceRequest o = new InstanceRequest(prefabPath, createPoolFunc, priority);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<Func<string, BasePool>>(L, 3))
			{
				string prefabPath2 = Lua.lua_tostring(L, 2);
				Func<string, BasePool> createPoolFunc2 = objectTranslator.GetDelegate<Func<string, BasePool>>(L, 3);
				InstanceRequest o2 = new InstanceRequest(prefabPath2, createPoolFunc2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (Lua.lua_gettop(L) == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ObjectPoolTag>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				string prefabPath3 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out ObjectPoolTag val);
				InstanceRequest o3 = new InstanceRequest(priority: Lua.xlua_tointeger(L, 4), prefabPath: prefabPath3, poolTag: val);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (Lua.lua_gettop(L) == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ObjectPoolTag>(L, 3))
			{
				string prefabPath4 = Lua.lua_tostring(L, 2);
				objectTranslator.Get(L, 3, out ObjectPoolTag val2);
				InstanceRequest o4 = new InstanceRequest(prefabPath4, val2);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING))
			{
				InstanceRequest o5 = new InstanceRequest(Lua.lua_tostring(L, 2));
				objectTranslator.Push(L, o5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to InstanceRequest constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Instantiate(IntPtr L)
	{
		try
		{
			((InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Instantiate();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RealDestroy(IntPtr L)
	{
		try
		{
			((InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RealDestroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Destroy(IntPtr L)
	{
		try
		{
			((InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Destroy();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Update(IntPtr L)
	{
		try
		{
			bool value = ((InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Update();
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PrefabPath(IntPtr L)
	{
		try
		{
			InstanceRequest instanceRequest = (InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, instanceRequest.PrefabPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isDone(IntPtr L)
	{
		try
		{
			InstanceRequest instanceRequest = (InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, instanceRequest.isDone);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isError(IntPtr L)
	{
		try
		{
			InstanceRequest instanceRequest = (InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, instanceRequest.isError);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_poolIsReady(IntPtr L)
	{
		try
		{
			InstanceRequest instanceRequest = (InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, instanceRequest.poolIsReady);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_state(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InstanceRequest instanceRequest = (InstanceRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushInstanceRequestState(L, instanceRequest.state);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InstanceRequest instanceRequest = (InstanceRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, instanceRequest.gameObject);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isUseCache(IntPtr L)
	{
		try
		{
			InstanceRequest instanceRequest = (InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, instanceRequest.isUseCache);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get__poolTag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InstanceRequest instanceRequest = (InstanceRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushObjectPoolTag(L, instanceRequest._poolTag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_state(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InstanceRequest instanceRequest = (InstanceRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out InstanceRequest.State val);
			instanceRequest.state = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gameObject(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((InstanceRequest)objectTranslator.FastGetCSObj(L, 1)).gameObject = (GameObject)objectTranslator.GetObject(L, 2, typeof(GameObject));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_isUseCache(IntPtr L)
	{
		try
		{
			((InstanceRequest)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).isUseCache = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set__poolTag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			InstanceRequest instanceRequest = (InstanceRequest)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ObjectPoolTag val);
			instanceRequest._poolTag = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _e_completed(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			InstanceRequest instanceRequest = (InstanceRequest)objectTranslator.FastGetCSObj(L, 1);
			Action<InstanceRequest> action = objectTranslator.GetDelegate<Action<InstanceRequest>>(L, 3);
			if (action == null)
			{
				return Lua.luaL_error(L, "#3 need System.Action<InstanceRequest>!");
			}
			if (num == 3)
			{
				if (Lua.xlua_is_eq_str(L, 2, "+"))
				{
					instanceRequest.completed += action;
					return 0;
				}
				if (Lua.xlua_is_eq_str(L, 2, "-"))
				{
					instanceRequest.completed -= action;
					return 0;
				}
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		Lua.luaL_error(L, "invalid arguments to InstanceRequest.completed!");
		return 0;
	}
}
