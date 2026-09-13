using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineExperimentalRenderingUniversalRenderObjectsRenderObjectsSettingsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RenderObjects.RenderObjectsSettings);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 10, 10);
		Utils.RegisterFunc(L, -2, "passTag", _g_get_passTag);
		Utils.RegisterFunc(L, -2, "Event", _g_get_Event);
		Utils.RegisterFunc(L, -2, "filterSettings", _g_get_filterSettings);
		Utils.RegisterFunc(L, -2, "overrideMaterial", _g_get_overrideMaterial);
		Utils.RegisterFunc(L, -2, "overrideMaterialPassIndex", _g_get_overrideMaterialPassIndex);
		Utils.RegisterFunc(L, -2, "overrideDepthState", _g_get_overrideDepthState);
		Utils.RegisterFunc(L, -2, "depthCompareFunction", _g_get_depthCompareFunction);
		Utils.RegisterFunc(L, -2, "enableWrite", _g_get_enableWrite);
		Utils.RegisterFunc(L, -2, "stencilSettings", _g_get_stencilSettings);
		Utils.RegisterFunc(L, -2, "cameraSettings", _g_get_cameraSettings);
		Utils.RegisterFunc(L, -1, "passTag", _s_set_passTag);
		Utils.RegisterFunc(L, -1, "Event", _s_set_Event);
		Utils.RegisterFunc(L, -1, "filterSettings", _s_set_filterSettings);
		Utils.RegisterFunc(L, -1, "overrideMaterial", _s_set_overrideMaterial);
		Utils.RegisterFunc(L, -1, "overrideMaterialPassIndex", _s_set_overrideMaterialPassIndex);
		Utils.RegisterFunc(L, -1, "overrideDepthState", _s_set_overrideDepthState);
		Utils.RegisterFunc(L, -1, "depthCompareFunction", _s_set_depthCompareFunction);
		Utils.RegisterFunc(L, -1, "enableWrite", _s_set_enableWrite);
		Utils.RegisterFunc(L, -1, "stencilSettings", _s_set_stencilSettings);
		Utils.RegisterFunc(L, -1, "cameraSettings", _s_set_cameraSettings);
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
				RenderObjects.RenderObjectsSettings o = new RenderObjects.RenderObjectsSettings();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Experimental.Rendering.Universal.RenderObjects.RenderObjectsSettings constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_passTag(IntPtr L)
	{
		try
		{
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushstring(L, renderObjectsSettings.passTag);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_Event(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderObjectsSettings.Event);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_filterSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderObjectsSettings.filterSettings);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderObjectsSettings.overrideMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideMaterialPassIndex(IntPtr L)
	{
		try
		{
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, renderObjectsSettings.overrideMaterialPassIndex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideDepthState(IntPtr L)
	{
		try
		{
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderObjectsSettings.overrideDepthState);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_depthCompareFunction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderObjectsSettings.depthCompareFunction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableWrite(IntPtr L)
	{
		try
		{
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderObjectsSettings.enableWrite);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stencilSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderObjectsSettings.stencilSettings);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderObjectsSettings.cameraSettings);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_passTag(IntPtr L)
	{
		try
		{
			((RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).passTag = Lua.lua_tostring(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_Event(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderPassEvent v);
			renderObjectsSettings.Event = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_filterSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1)).filterSettings = (RenderObjects.FilterSettings)objectTranslator.GetObject(L, 2, typeof(RenderObjects.FilterSettings));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1)).overrideMaterial = (Material)objectTranslator.GetObject(L, 2, typeof(Material));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideMaterialPassIndex(IntPtr L)
	{
		try
		{
			((RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideMaterialPassIndex = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideDepthState(IntPtr L)
	{
		try
		{
			((RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideDepthState = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_depthCompareFunction(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderObjects.RenderObjectsSettings renderObjectsSettings = (RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CompareFunction v);
			renderObjectsSettings.depthCompareFunction = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableWrite(IntPtr L)
	{
		try
		{
			((RenderObjects.RenderObjectsSettings)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableWrite = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stencilSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1)).stencilSettings = (StencilStateData)objectTranslator.GetObject(L, 2, typeof(StencilStateData));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cameraSettings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((RenderObjects.RenderObjectsSettings)objectTranslator.FastGetCSObj(L, 1)).cameraSettings = (RenderObjects.CustomCameraSettings)objectTranslator.GetObject(L, 2, typeof(RenderObjects.CustomCameraSettings));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
