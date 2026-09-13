using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalForwardRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ForwardRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 4, 0, 0);
		Utils.RegisterFunc(L, -3, "Setup", _m_Setup);
		Utils.RegisterFunc(L, -3, "SetupLights", _m_SetupLights);
		Utils.RegisterFunc(L, -3, "SetupCullingParameters", _m_SetupCullingParameters);
		Utils.RegisterFunc(L, -3, "FinishRendering", _m_FinishRendering);
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
			if (Lua.lua_gettop(L) == 2 && objectTranslator.Assignable<ForwardRendererData>(L, 2))
			{
				ForwardRenderer o = new ForwardRenderer((ForwardRendererData)objectTranslator.GetObject(L, 2, typeof(ForwardRendererData)));
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.ForwardRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Setup(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ForwardRenderer forwardRenderer = (ForwardRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableRenderContext v);
			objectTranslator.Get(L, 3, out RenderingData v2);
			forwardRenderer.Setup(v, ref v2);
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
			ForwardRenderer forwardRenderer = (ForwardRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableRenderContext v);
			objectTranslator.Get(L, 3, out RenderingData v2);
			forwardRenderer.SetupLights(v, ref v2);
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
			ForwardRenderer forwardRenderer = (ForwardRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ScriptableCullingParameters v);
			objectTranslator.Get(L, 3, out CameraData v2);
			forwardRenderer.SetupCullingParameters(ref v, ref v2);
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
			ForwardRenderer forwardRenderer = (ForwardRenderer)objectTranslator.FastGetCSObj(L, 1);
			CommandBuffer cmd = (CommandBuffer)objectTranslator.GetObject(L, 2, typeof(CommandBuffer));
			forwardRenderer.FinishRendering(cmd);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
