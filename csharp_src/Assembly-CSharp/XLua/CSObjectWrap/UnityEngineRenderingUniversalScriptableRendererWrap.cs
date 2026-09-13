using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalScriptableRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ScriptableRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 8, 4, 1);
		Utils.RegisterFunc(L, -3, "Dispose", _m_Dispose);
		Utils.RegisterFunc(L, -3, "ConfigureCameraTarget", _m_ConfigureCameraTarget);
		Utils.RegisterFunc(L, -3, "Setup", _m_Setup);
		Utils.RegisterFunc(L, -3, "SetupLights", _m_SetupLights);
		Utils.RegisterFunc(L, -3, "SetupCullingParameters", _m_SetupCullingParameters);
		Utils.RegisterFunc(L, -3, "FinishRendering", _m_FinishRendering);
		Utils.RegisterFunc(L, -3, "Execute", _m_Execute);
		Utils.RegisterFunc(L, -3, "EnqueuePass", _m_EnqueuePass);
		Utils.RegisterFunc(L, -2, "cameraColorTarget", _g_get_cameraColorTarget);
		Utils.RegisterFunc(L, -2, "cameraDepth", _g_get_cameraDepth);
		Utils.RegisterFunc(L, -2, "rendererFeatures", _g_get_rendererFeatures);
		Utils.RegisterFunc(L, -2, "supportedRenderingFeatures", _g_get_supportedRenderingFeatures);
		Utils.RegisterFunc(L, -1, "supportedRenderingFeatures", _s_set_supportedRenderingFeatures);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 0, 0);
		Utils.RegisterFunc(L, -4, "SetCameraMatrices", _m_SetCameraMatrices_xlua_st_);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Rendering.Universal.ScriptableRenderer does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetCameraMatrices_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CommandBuffer cmd = (CommandBuffer)objectTranslator.GetObject(L, 1, typeof(CommandBuffer));
			objectTranslator.Get(L, 2, out CameraData v);
			bool setInverseMatrices = Lua.lua_toboolean(L, 3);
			ScriptableRenderer.SetCameraMatrices(cmd, ref v, setInverseMatrices);
			objectTranslator.Push(L, v);
			objectTranslator.Update(L, 2, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Dispose(IntPtr L)
	{
		try
		{
			((ScriptableRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Dispose();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ConfigureCameraTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderTargetIdentifier v);
			objectTranslator.Get(L, 3, out RenderTargetIdentifier v2);
			scriptableRenderer.ConfigureCameraTarget(v, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Setup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableRenderContext v);
			objectTranslator.Get(L, 3, out RenderingData v2);
			scriptableRenderer.Setup(v, ref v2);
			objectTranslator.Push(L, v2);
			objectTranslator.Update(L, 3, v2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupLights(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableRenderContext v);
			objectTranslator.Get(L, 3, out RenderingData v2);
			scriptableRenderer.SetupLights(v, ref v2);
			objectTranslator.Push(L, v2);
			objectTranslator.Update(L, 3, v2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupCullingParameters(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableCullingParameters v);
			objectTranslator.Get(L, 3, out CameraData v2);
			scriptableRenderer.SetupCullingParameters(ref v, ref v2);
			objectTranslator.Push(L, v);
			objectTranslator.Update(L, 2, v);
			objectTranslator.Push(L, v2);
			objectTranslator.Update(L, 3, v2);
			return 2;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FinishRendering(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			CommandBuffer cmd = (CommandBuffer)objectTranslator.GetObject(L, 2, typeof(CommandBuffer));
			scriptableRenderer.FinishRendering(cmd);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Execute(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableRenderContext v);
			objectTranslator.Get(L, 3, out RenderingData v2);
			scriptableRenderer.Execute(v, ref v2);
			objectTranslator.Push(L, v2);
			objectTranslator.Update(L, 3, v2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_EnqueuePass(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			ScriptableRenderPass pass = (ScriptableRenderPass)objectTranslator.GetObject(L, 2, typeof(ScriptableRenderPass));
			scriptableRenderer.EnqueuePass(pass);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraColorTarget(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scriptableRenderer.cameraColorTarget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraDepth(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scriptableRenderer.cameraDepth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rendererFeatures(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scriptableRenderer.rendererFeatures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportedRenderingFeatures(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRenderer scriptableRenderer = (ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, scriptableRenderer.supportedRenderingFeatures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportedRenderingFeatures(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((ScriptableRenderer)objectTranslator.FastGetCSObj(L, 1)).supportedRenderingFeatures = (ScriptableRenderer.RenderingFeatures)objectTranslator.GetObject(L, 2, typeof(ScriptableRenderer.RenderingFeatures));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
