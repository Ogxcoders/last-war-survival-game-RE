using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalUniversalAdditionalCameraDataWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UniversalAdditionalCameraData);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 18, 14);
		Utils.RegisterFunc(L, -3, "SetRenderer", _m_SetRenderer);
		Utils.RegisterFunc(L, -3, "OnBeforeSerialize", _m_OnBeforeSerialize);
		Utils.RegisterFunc(L, -3, "OnAfterDeserialize", _m_OnAfterDeserialize);
		Utils.RegisterFunc(L, -3, "OnDrawGizmos", _m_OnDrawGizmos);
		Utils.RegisterFunc(L, -2, "version", _g_get_version);
		Utils.RegisterFunc(L, -2, "renderShadows", _g_get_renderShadows);
		Utils.RegisterFunc(L, -2, "requiresDepthOption", _g_get_requiresDepthOption);
		Utils.RegisterFunc(L, -2, "requiresColorOption", _g_get_requiresColorOption);
		Utils.RegisterFunc(L, -2, "renderType", _g_get_renderType);
		Utils.RegisterFunc(L, -2, "cameraStack", _g_get_cameraStack);
		Utils.RegisterFunc(L, -2, "clearDepth", _g_get_clearDepth);
		Utils.RegisterFunc(L, -2, "requiresDepthTexture", _g_get_requiresDepthTexture);
		Utils.RegisterFunc(L, -2, "requiresColorTexture", _g_get_requiresColorTexture);
		Utils.RegisterFunc(L, -2, "scriptableRenderer", _g_get_scriptableRenderer);
		Utils.RegisterFunc(L, -2, "volumeLayerMask", _g_get_volumeLayerMask);
		Utils.RegisterFunc(L, -2, "volumeTrigger", _g_get_volumeTrigger);
		Utils.RegisterFunc(L, -2, "renderPostProcessing", _g_get_renderPostProcessing);
		Utils.RegisterFunc(L, -2, "antialiasing", _g_get_antialiasing);
		Utils.RegisterFunc(L, -2, "antialiasingQuality", _g_get_antialiasingQuality);
		Utils.RegisterFunc(L, -2, "stopNaN", _g_get_stopNaN);
		Utils.RegisterFunc(L, -2, "dithering", _g_get_dithering);
		Utils.RegisterFunc(L, -2, "disableRender", _g_get_disableRender);
		Utils.RegisterFunc(L, -1, "renderShadows", _s_set_renderShadows);
		Utils.RegisterFunc(L, -1, "requiresDepthOption", _s_set_requiresDepthOption);
		Utils.RegisterFunc(L, -1, "requiresColorOption", _s_set_requiresColorOption);
		Utils.RegisterFunc(L, -1, "renderType", _s_set_renderType);
		Utils.RegisterFunc(L, -1, "requiresDepthTexture", _s_set_requiresDepthTexture);
		Utils.RegisterFunc(L, -1, "requiresColorTexture", _s_set_requiresColorTexture);
		Utils.RegisterFunc(L, -1, "volumeLayerMask", _s_set_volumeLayerMask);
		Utils.RegisterFunc(L, -1, "volumeTrigger", _s_set_volumeTrigger);
		Utils.RegisterFunc(L, -1, "renderPostProcessing", _s_set_renderPostProcessing);
		Utils.RegisterFunc(L, -1, "antialiasing", _s_set_antialiasing);
		Utils.RegisterFunc(L, -1, "antialiasingQuality", _s_set_antialiasingQuality);
		Utils.RegisterFunc(L, -1, "stopNaN", _s_set_stopNaN);
		Utils.RegisterFunc(L, -1, "dithering", _s_set_dithering);
		Utils.RegisterFunc(L, -1, "disableRender", _s_set_disableRender);
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
				UniversalAdditionalCameraData o = new UniversalAdditionalCameraData();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.UniversalAdditionalCameraData constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetRenderer(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData obj = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			int renderer = Lua.xlua_tointeger(L, 2);
			obj.SetRenderer(renderer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnBeforeSerialize(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeforeSerialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnAfterDeserialize(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAfterDeserialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_OnDrawGizmos(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnDrawGizmos();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_version(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalAdditionalCameraData.version);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderShadows(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.renderShadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_requiresDepthOption(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.requiresDepthOption);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_requiresColorOption(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.requiresColorOption);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.renderType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraStack(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.cameraStack);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearDepth(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.clearDepth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_requiresDepthTexture(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.requiresDepthTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_requiresColorTexture(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.requiresColorTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scriptableRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.scriptableRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_volumeLayerMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.volumeLayerMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_volumeTrigger(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.volumeTrigger);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderPostProcessing(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.renderPostProcessing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_antialiasing(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, universalAdditionalCameraData.antialiasing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_antialiasingQuality(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalAdditionalCameraData.antialiasingQuality);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stopNaN(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.stopNaN);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_dithering(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.dithering);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_disableRender(IntPtr L)
	{
		try
		{
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalAdditionalCameraData.disableRender);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderShadows(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).renderShadows = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_requiresDepthOption(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraOverrideOption v);
			universalAdditionalCameraData.requiresDepthOption = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_requiresColorOption(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraOverrideOption v);
			universalAdditionalCameraData.requiresColorOption = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraRenderType v);
			universalAdditionalCameraData.renderType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_requiresDepthTexture(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).requiresDepthTexture = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_requiresColorTexture(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).requiresColorTexture = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_volumeLayerMask(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out LayerMask v);
			universalAdditionalCameraData.volumeLayerMask = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_volumeTrigger(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1)).volumeTrigger = (Transform)objectTranslator.GetObject(L, 2, typeof(Transform));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderPostProcessing(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).renderPostProcessing = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_antialiasing(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AntialiasingMode val);
			universalAdditionalCameraData.antialiasing = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_antialiasingQuality(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalAdditionalCameraData universalAdditionalCameraData = (UniversalAdditionalCameraData)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AntialiasingQuality v);
			universalAdditionalCameraData.antialiasingQuality = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stopNaN(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stopNaN = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_dithering(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).dithering = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_disableRender(IntPtr L)
	{
		try
		{
			((UniversalAdditionalCameraData)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).disableRender = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
