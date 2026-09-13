using System;
using FibMatrix.Rendering;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class FibMatrixRenderingCustomRenderObjectsFeatureWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(CustomRenderObjectsFeature);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 1, 1);
		Utils.RegisterFunc(L, -3, "Create", _m_Create);
		Utils.RegisterFunc(L, -3, "AddRenderPasses", _m_AddRenderPasses);
		Utils.RegisterFunc(L, -2, "settings", _g_get_settings);
		Utils.RegisterFunc(L, -1, "settings", _s_set_settings);
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
				CustomRenderObjectsFeature o = new CustomRenderObjectsFeature();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to FibMatrix.Rendering.CustomRenderObjectsFeature constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create(IntPtr L)
	{
		try
		{
			((CustomRenderObjectsFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Create();
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
			CustomRenderObjectsFeature customRenderObjectsFeature = (CustomRenderObjectsFeature)objectTranslator.FastGetCSObj(L, 1);
			ScriptableRenderer renderer = (ScriptableRenderer)objectTranslator.GetObject(L, 2, typeof(ScriptableRenderer));
			objectTranslator.Get(L, 3, out RenderingData v);
			customRenderObjectsFeature.AddRenderPasses(renderer, ref v);
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
	private static int _g_get_settings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			CustomRenderObjectsFeature customRenderObjectsFeature = (CustomRenderObjectsFeature)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, customRenderObjectsFeature.settings);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_settings(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((CustomRenderObjectsFeature)objectTranslator.FastGetCSObj(L, 1)).settings = (CustomRenderObjectsFeature.RenderObjectsSettings)objectTranslator.GetObject(L, 2, typeof(CustomRenderObjectsFeature.RenderObjectsSettings));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
