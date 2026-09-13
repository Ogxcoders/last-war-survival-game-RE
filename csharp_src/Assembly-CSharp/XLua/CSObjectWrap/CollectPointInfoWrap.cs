using System;
using Protobuf;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class CollectPointInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CollectPointInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 4, 4);
		Utils.RegisterFunc(L, -3, "Clone", _m_Clone);
		Utils.RegisterFunc(L, -3, "GetResourceType", _m_GetResourceType);
		Utils.RegisterFunc(L, -2, "resourceType", _g_get_resourceType);
		Utils.RegisterFunc(L, -2, "level", _g_get_level);
		Utils.RegisterFunc(L, -2, "type", _g_get_type);
		Utils.RegisterFunc(L, -2, "attachId", _g_get_attachId);
		Utils.RegisterFunc(L, -1, "resourceType", _s_set_resourceType);
		Utils.RegisterFunc(L, -1, "level", _s_set_level);
		Utils.RegisterFunc(L, -1, "type", _s_set_type);
		Utils.RegisterFunc(L, -1, "attachId", _s_set_attachId);
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
				CollectPointInfo o = new CollectPointInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<WorldPointInfo>(L, 2))
			{
				CollectPointInfo o2 = new CollectPointInfo((WorldPointInfo)objectTranslator.GetObject(L, 2, typeof(WorldPointInfo)));
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to CollectPointInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Clone(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			PointInfo o = ((CollectPointInfo)objectTranslator.FastGetCSObj(L, 1)).Clone();
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetResourceType(IntPtr L)
	{
		try
		{
			int resourceType = ((CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetResourceType();
			Lua.xlua_pushinteger(L, resourceType);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resourceType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CollectPointInfo collectPointInfo = (CollectPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushResourceType(L, collectPointInfo.resourceType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_level(IntPtr L)
	{
		try
		{
			CollectPointInfo collectPointInfo = (CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, collectPointInfo.level);
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
			CollectPointInfo collectPointInfo = (CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, collectPointInfo.type);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_attachId(IntPtr L)
	{
		try
		{
			CollectPointInfo collectPointInfo = (CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, collectPointInfo.attachId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resourceType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CollectPointInfo collectPointInfo = (CollectPointInfo)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ResourceType val);
			collectPointInfo.resourceType = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_level(IntPtr L)
	{
		try
		{
			((CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).level = Lua.xlua_tointeger(L, 2);
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
			((CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).type = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_attachId(IntPtr L)
	{
		try
		{
			((CollectPointInfo)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).attachId = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
