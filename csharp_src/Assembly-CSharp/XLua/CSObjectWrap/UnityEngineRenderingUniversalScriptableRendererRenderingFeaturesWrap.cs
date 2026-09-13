using System;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalScriptableRendererRenderingFeaturesWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ScriptableRenderer.RenderingFeatures);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 1, 1);
		Utils.RegisterFunc(L, -2, "cameraStacking", _g_get_cameraStacking);
		Utils.RegisterFunc(L, -1, "cameraStacking", _s_set_cameraStacking);
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
				ScriptableRenderer.RenderingFeatures o = new ScriptableRenderer.RenderingFeatures();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.ScriptableRenderer.RenderingFeatures constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraStacking(IntPtr L)
	{
		try
		{
			ScriptableRenderer.RenderingFeatures renderingFeatures = (ScriptableRenderer.RenderingFeatures)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, renderingFeatures.cameraStacking);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cameraStacking(IntPtr L)
	{
		try
		{
			((ScriptableRenderer.RenderingFeatures)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cameraStacking = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
