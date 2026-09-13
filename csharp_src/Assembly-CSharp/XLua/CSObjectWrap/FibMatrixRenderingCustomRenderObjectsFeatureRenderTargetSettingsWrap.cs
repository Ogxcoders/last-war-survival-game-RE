using System;
using FibMatrix.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FibMatrixRenderingCustomRenderObjectsFeatureRenderTargetSettingsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CustomRenderObjectsFeature.RenderTargetSettings);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 9, 9);
		Utils.RegisterFunc(L, -2, "overrideTarget", _g_get_overrideTarget);
		Utils.RegisterFunc(L, -2, "name", _g_get_name);
		Utils.RegisterFunc(L, -2, "scale", _g_get_scale);
		Utils.RegisterFunc(L, -2, "size", _g_get_size);
		Utils.RegisterFunc(L, -2, "depth", _g_get_depth);
		Utils.RegisterFunc(L, -2, "format", _g_get_format);
		Utils.RegisterFunc(L, -2, "clearFlag", _g_get_clearFlag);
		Utils.RegisterFunc(L, -2, "clearColor", _g_get_clearColor);
		Utils.RegisterFunc(L, -2, "filterMode", _g_get_filterMode);
		Utils.RegisterFunc(L, -1, "overrideTarget", _s_set_overrideTarget);
		Utils.RegisterFunc(L, -1, "name", _s_set_name);
		Utils.RegisterFunc(L, -1, "scale", _s_set_scale);
		Utils.RegisterFunc(L, -1, "size", _s_set_size);
		Utils.RegisterFunc(L, -1, "depth", _s_set_depth);
		Utils.RegisterFunc(L, -1, "format", _s_set_format);
		Utils.RegisterFunc(L, -1, "clearFlag", _s_set_clearFlag);
		Utils.RegisterFunc(L, -1, "clearColor", _s_set_clearColor);
		Utils.RegisterFunc(L, -1, "filterMode", _s_set_filterMode);
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
				CustomRenderObjectsFeature.RenderTargetSettings o = new CustomRenderObjectsFeature.RenderTargetSettings();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FibMatrix.Rendering.CustomRenderObjectsFeature.RenderTargetSettings constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideTarget(IntPtr L)
	{
		try
		{
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderTargetSettings.overrideTarget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_name(IntPtr L)
	{
		try
		{
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, renderTargetSettings.name);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scale(IntPtr L)
	{
		try
		{
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, renderTargetSettings.scale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, renderTargetSettings.size);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_depth(IntPtr L)
	{
		try
		{
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderTargetSettings.depth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_format(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineRenderTextureFormat(L, renderTargetSettings.format);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearFlag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTargetSettings.clearFlag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, renderTargetSettings.clearColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_filterMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderTargetSettings.filterMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideTarget(IntPtr L)
	{
		try
		{
			((CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideTarget = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_name(IntPtr L)
	{
		try
		{
			((CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).name = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scale(IntPtr L)
	{
		try
		{
			((CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).scale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_size(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			renderTargetSettings.size = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_depth(IntPtr L)
	{
		try
		{
			((CustomRenderObjectsFeature.RenderTargetSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).depth = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_format(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderTextureFormat val);
			renderTargetSettings.format = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearFlag(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ClearFlag v);
			renderTargetSettings.clearFlag = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			renderTargetSettings.clearColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_filterMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature.RenderTargetSettings renderTargetSettings = (CustomRenderObjectsFeature.RenderTargetSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out FilterMode v);
			renderTargetSettings.filterMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
