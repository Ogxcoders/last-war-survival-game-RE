using System;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalScriptableRendererFeatureWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(ScriptableRendererFeature);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 3, 1, 0);
		Utils.RegisterFunc(L, -3, "Create", _m_Create);
		Utils.RegisterFunc(L, -3, "AddRenderPasses", _m_AddRenderPasses);
		Utils.RegisterFunc(L, -3, "SetActive", _m_SetActive);
		Utils.RegisterFunc(L, -2, "isActive", _g_get_isActive);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Rendering.Universal.ScriptableRendererFeature does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create(IntPtr L)
	{
		try
		{
			((ScriptableRendererFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Create();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddRenderPasses(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			ScriptableRendererFeature scriptableRendererFeature = (ScriptableRendererFeature)objectTranslator.FastGetCSObj(L, 1);
			ScriptableRenderer renderer = (ScriptableRenderer)objectTranslator.GetObject(L, 2, typeof(ScriptableRenderer));
			objectTranslator.Get(L, 3, out RenderingData v);
			scriptableRendererFeature.AddRenderPasses(renderer, ref v);
			objectTranslator.Push(L, v);
			objectTranslator.Update(L, 3, v);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetActive(IntPtr L)
	{
		try
		{
			ScriptableRendererFeature obj = (ScriptableRendererFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool active = Lua.lua_toboolean(L, 2);
			obj.SetActive(active);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_isActive(IntPtr L)
	{
		try
		{
			ScriptableRendererFeature scriptableRendererFeature = (ScriptableRendererFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, scriptableRendererFeature.isActive);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
