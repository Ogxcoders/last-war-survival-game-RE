using System;
using VEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class ResourceManagerPreloadCacheWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ResourceManager.PreloadCache);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 2, 2);
		Utils.RegisterFunc(L, -2, "asset", _g_get_asset);
		Utils.RegisterFunc(L, -2, "expiredTime", _g_get_expiredTime);
		Utils.RegisterFunc(L, -1, "asset", _s_set_asset);
		Utils.RegisterFunc(L, -1, "expiredTime", _s_set_expiredTime);
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
				ResourceManager.PreloadCache o = new ResourceManager.PreloadCache();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to ResourceManager.PreloadCache constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ResourceManager.PreloadCache preloadCache = (ResourceManager.PreloadCache)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, preloadCache.asset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_expiredTime(IntPtr L)
	{
		try
		{
			ResourceManager.PreloadCache preloadCache = (ResourceManager.PreloadCache)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, preloadCache.expiredTime);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asset(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ResourceManager.PreloadCache)objectTranslator.FastGetCSObj(L, 1)).asset = (Asset)objectTranslator.GetObject(L, 2, typeof(Asset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_expiredTime(IntPtr L)
	{
		try
		{
			((ResourceManager.PreloadCache)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).expiredTime = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
