using System;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FakeModelManagerParamWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(FakeModelManager.Param);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 7, 7);
		Utils.RegisterFunc(L, -2, "index", _g_get_index);
		Utils.RegisterFunc(L, -2, "type", _g_get_type);
		Utils.RegisterFunc(L, -2, "req", _g_get_req);
		Utils.RegisterFunc(L, -2, "prefabName", _g_get_prefabName);
		Utils.RegisterFunc(L, -2, "order", _g_get_order);
		Utils.RegisterFunc(L, -2, "renderers", _g_get_renderers);
		Utils.RegisterFunc(L, -2, "showPrefabName", _g_get_showPrefabName);
		Utils.RegisterFunc(L, -1, "index", _s_set_index);
		Utils.RegisterFunc(L, -1, "type", _s_set_type);
		Utils.RegisterFunc(L, -1, "req", _s_set_req);
		Utils.RegisterFunc(L, -1, "prefabName", _s_set_prefabName);
		Utils.RegisterFunc(L, -1, "order", _s_set_order);
		Utils.RegisterFunc(L, -1, "renderers", _s_set_renderers);
		Utils.RegisterFunc(L, -1, "showPrefabName", _s_set_showPrefabName);
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
				FakeModelManager.Param o = new FakeModelManager.Param();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FakeModelManager.Param constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_index(IntPtr L)
	{
		try
		{
			FakeModelManager.Param param = (FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, param.index);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FakeModelManager.Param param = (FakeModelManager.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushFakeModelManagerTempRoadType(L, param.type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_req(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FakeModelManager.Param param = (FakeModelManager.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, param.req);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_prefabName(IntPtr L)
	{
		try
		{
			FakeModelManager.Param param = (FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, param.prefabName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_order(IntPtr L)
	{
		try
		{
			FakeModelManager.Param param = (FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, param.order);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FakeModelManager.Param param = (FakeModelManager.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, param.renderers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_showPrefabName(IntPtr L)
	{
		try
		{
			FakeModelManager.Param param = (FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, param.showPrefabName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_index(IntPtr L)
	{
		try
		{
			((FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).index = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_type(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			FakeModelManager.Param param = (FakeModelManager.Param)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FakeModelManager.TempRoadType val);
			param.type = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_req(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((FakeModelManager.Param)objectTranslator.FastGetCSObj(L, 1)).req = (InstanceRequest)objectTranslator.GetObject(L, 2, typeof(InstanceRequest));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_prefabName(IntPtr L)
	{
		try
		{
			((FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).prefabName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_order(IntPtr L)
	{
		try
		{
			((FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).order = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((FakeModelManager.Param)objectTranslator.FastGetCSObj(L, 1)).renderers = (MeshRenderer[])objectTranslator.GetObject(L, 2, typeof(MeshRenderer[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_showPrefabName(IntPtr L)
	{
		try
		{
			((FakeModelManager.Param)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).showPrefabName = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
