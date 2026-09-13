using System;
using FibMatrix.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FibMatrixRenderingCustomRenderObjectsFeatureFilterSettingsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CustomRenderObjectsFeature.FilterSettings);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 4, 4);
		Utils.RegisterFunc(L, -2, "RenderQueueType", _g_get_RenderQueueType);
		Utils.RegisterFunc(L, -2, "LayerMask", _g_get_LayerMask);
		Utils.RegisterFunc(L, -2, "PassNames", _g_get_PassNames);
		Utils.RegisterFunc(L, -2, "RenderingLayerMask", _g_get_RenderingLayerMask);
		Utils.RegisterFunc(L, -1, "RenderQueueType", _s_set_RenderQueueType);
		Utils.RegisterFunc(L, -1, "LayerMask", _s_set_LayerMask);
		Utils.RegisterFunc(L, -1, "PassNames", _s_set_PassNames);
		Utils.RegisterFunc(L, -1, "RenderingLayerMask", _s_set_RenderingLayerMask);
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
				CustomRenderObjectsFeature.FilterSettings o = new CustomRenderObjectsFeature.FilterSettings();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FibMatrix.Rendering.CustomRenderObjectsFeature.FilterSettings constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RenderQueueType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.FilterSettings filterSettings = (CustomRenderObjectsFeature.FilterSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, filterSettings.RenderQueueType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_LayerMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.FilterSettings filterSettings = (CustomRenderObjectsFeature.FilterSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, filterSettings.LayerMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_PassNames(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.FilterSettings filterSettings = (CustomRenderObjectsFeature.FilterSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, filterSettings.PassNames);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_RenderingLayerMask(IntPtr L)
	{
		try
		{
			CustomRenderObjectsFeature.FilterSettings filterSettings = (CustomRenderObjectsFeature.FilterSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, filterSettings.RenderingLayerMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RenderQueueType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.FilterSettings filterSettings = (CustomRenderObjectsFeature.FilterSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderQueueType v);
			filterSettings.RenderQueueType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_LayerMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.FilterSettings filterSettings = (CustomRenderObjectsFeature.FilterSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LayerMask v);
			filterSettings.LayerMask = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_PassNames(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CustomRenderObjectsFeature.FilterSettings)objectTranslator.FastGetCSObj(L, 1)).PassNames = (string[])objectTranslator.GetObject(L, 2, typeof(string[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_RenderingLayerMask(IntPtr L)
	{
		try
		{
			((CustomRenderObjectsFeature.FilterSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RenderingLayerMask = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
