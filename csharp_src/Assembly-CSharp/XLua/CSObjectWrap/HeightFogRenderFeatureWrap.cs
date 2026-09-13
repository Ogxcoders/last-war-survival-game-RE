using System;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class HeightFogRenderFeatureWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(HeightFogRenderFeature);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 2, 1, 1);
		Utils.RegisterFunc(L, -3, "Create", _m_Create);
		Utils.RegisterFunc(L, -3, "AddRenderPasses", _m_AddRenderPasses);
		Utils.RegisterFunc(L, -2, "fogSetting", _g_get_fogSetting);
		Utils.RegisterFunc(L, -1, "fogSetting", _s_set_fogSetting);
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
				HeightFogRenderFeature o = new HeightFogRenderFeature();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to HeightFogRenderFeature constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create(IntPtr L)
	{
		try
		{
			((HeightFogRenderFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Create();
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
			HeightFogRenderFeature heightFogRenderFeature = (HeightFogRenderFeature)objectTranslator.FastGetCSObj(L, 1);
			ScriptableRenderer renderer = (ScriptableRenderer)objectTranslator.GetObject(L, 2, typeof(ScriptableRenderer));
			objectTranslator.Get(L, 3, out RenderingData v);
			heightFogRenderFeature.AddRenderPasses(renderer, ref v);
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
	private static int _g_get_fogSetting(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			HeightFogRenderFeature heightFogRenderFeature = (HeightFogRenderFeature)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, heightFogRenderFeature.fogSetting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fogSetting(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((HeightFogRenderFeature)objectTranslator.FastGetCSObj(L, 1)).fogSetting = (HeightFogSettings)objectTranslator.GetObject(L, 2, typeof(HeightFogSettings));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
