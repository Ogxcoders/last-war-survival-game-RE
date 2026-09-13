using System;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ObjectPoolMgrWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ObjectPoolMgr);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 12, 2, 0);
		Utils.RegisterFunc(L, -3, "GetPool", _m_GetPool);
		Utils.RegisterFunc(L, -3, "PrefabHasCache", _m_PrefabHasCache);
		Utils.RegisterFunc(L, -3, "PrefabPoolIsReady", _m_PrefabPoolIsReady);
		Utils.RegisterFunc(L, -3, "ClearPool", _m_ClearPool);
		Utils.RegisterFunc(L, -3, "ClearPoolByTag", _m_ClearPoolByTag);
		Utils.RegisterFunc(L, -3, "ClearPoolByTagGroup", _m_ClearPoolByTagGroup);
		Utils.RegisterFunc(L, -3, "ClearAllPool", _m_ClearAllPool);
		Utils.RegisterFunc(L, -3, "ClearUnusedPool", _m_ClearUnusedPool);
		Utils.RegisterFunc(L, -3, "DebugOutput", _m_DebugOutput);
		Utils.RegisterFunc(L, -3, "TryCleanPool", _m_TryCleanPool);
		Utils.RegisterFunc(L, -3, "RegisterToCleanPoolList", _m_RegisterToCleanPoolList);
		Utils.RegisterFunc(L, -3, "UnRegisterToCleanPoolList", _m_UnRegisterToCleanPoolList);
		Utils.RegisterFunc(L, -2, "Root", _g_get_Root);
		Utils.RegisterFunc(L, -2, "interface_poolList", _g_get_interface_poolList);
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
			if (Lua.lua_gettop(L) == 1)
			{
				ObjectPoolMgr o = new ObjectPoolMgr();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ObjectPoolMgr constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetPool(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ObjectPoolMgr objectPoolMgr = (ObjectPoolMgr)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 4 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ResourceManager>(L, 3) && objectTranslator.Assignable<ObjectPoolTag>(L, 4))
			{
				string prefabPath = Lua.lua_tostring(L, 2);
				ResourceManager resourceManager = (ResourceManager)objectTranslator.GetObject(L, 3, typeof(ResourceManager));
				objectTranslator.Get(L, 4, out ObjectPoolTag val);
				global::ObjectPool pool = objectPoolMgr.GetPool(prefabPath, resourceManager, val);
				objectTranslator.Push(L, pool);
				return 1;
			}
			if (num == 3 && (Lua.lua_isnil(L, 2) || Lua.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && objectTranslator.Assignable<ResourceManager>(L, 3))
			{
				string prefabPath2 = Lua.lua_tostring(L, 2);
				ResourceManager resourceManager2 = (ResourceManager)objectTranslator.GetObject(L, 3, typeof(ResourceManager));
				global::ObjectPool pool2 = objectPoolMgr.GetPool(prefabPath2, resourceManager2);
				objectTranslator.Push(L, pool2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ObjectPoolMgr.GetPool!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrefabHasCache(IntPtr L)
	{
		try
		{
			ObjectPoolMgr obj = (ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			bool value = obj.PrefabHasCache(prefabPath);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_PrefabPoolIsReady(IntPtr L)
	{
		try
		{
			ObjectPoolMgr obj = (ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			bool value = obj.PrefabPoolIsReady(prefabPath);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPool(IntPtr L)
	{
		try
		{
			ObjectPoolMgr obj = (ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			obj.ClearPool(prefabPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPoolByTag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ObjectPoolMgr objectPoolMgr = (ObjectPoolMgr)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ObjectPoolTag val);
			objectPoolMgr.ClearPoolByTag(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearPoolByTagGroup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ObjectPoolMgr objectPoolMgr = (ObjectPoolMgr)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ObjectPoolTagGroup val);
			objectPoolMgr.ClearPoolByTagGroup(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllPool(IntPtr L)
	{
		try
		{
			((ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllPool();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearUnusedPool(IntPtr L)
	{
		try
		{
			((ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearUnusedPool();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DebugOutput(IntPtr L)
	{
		try
		{
			((ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).DebugOutput();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryCleanPool(IntPtr L)
	{
		try
		{
			((ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).TryCleanPool();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RegisterToCleanPoolList(IntPtr L)
	{
		try
		{
			ObjectPoolMgr obj = (ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			obj.RegisterToCleanPoolList(prefabPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UnRegisterToCleanPoolList(IntPtr L)
	{
		try
		{
			ObjectPoolMgr obj = (ObjectPoolMgr)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			string prefabPath = Lua.lua_tostring(L, 2);
			obj.UnRegisterToCleanPoolList(prefabPath);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Root(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ObjectPoolMgr objectPoolMgr = (ObjectPoolMgr)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, objectPoolMgr.Root);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_interface_poolList(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ObjectPoolMgr objectPoolMgr = (ObjectPoolMgr)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, objectPoolMgr.interface_poolList);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
