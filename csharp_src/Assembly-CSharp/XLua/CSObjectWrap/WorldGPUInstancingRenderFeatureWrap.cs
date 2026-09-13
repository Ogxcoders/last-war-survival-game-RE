using System;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class WorldGPUInstancingRenderFeatureWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(WorldGPUInstancingRenderFeature);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 6, 0, 0);
		Utils.RegisterFunc(L, -3, "Create", _m_Create);
		Utils.RegisterFunc(L, -3, "AddRenderPasses", _m_AddRenderPasses);
		Utils.RegisterFunc(L, -3, "AddRenderer", _m_AddRenderer);
		Utils.RegisterFunc(L, -3, "RemoveRenderer", _m_RemoveRenderer);
		Utils.RegisterFunc(L, -3, "ClearAllPass", _m_ClearAllPass);
		Utils.RegisterFunc(L, -3, "Description", _m_Description);
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
				WorldGPUInstancingRenderFeature o = new WorldGPUInstancingRenderFeature();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to WorldGPUInstancingRenderFeature constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Create(IntPtr L)
	{
		try
		{
			((WorldGPUInstancingRenderFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Create();
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
			WorldGPUInstancingRenderFeature worldGPUInstancingRenderFeature = (WorldGPUInstancingRenderFeature)objectTranslator.FastGetCSObj(L, 1);
			ScriptableRenderer renderer = (ScriptableRenderer)objectTranslator.GetObject(L, 2, typeof(ScriptableRenderer));
			objectTranslator.Get(L, 3, out RenderingData v);
			worldGPUInstancingRenderFeature.AddRenderPasses(renderer, ref v);
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
	private static int _m_AddRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldGPUInstancingRenderFeature worldGPUInstancingRenderFeature = (WorldGPUInstancingRenderFeature)objectTranslator.FastGetCSObj(L, 1);
			IWorldGPUInstancingRenderer renderer = (IWorldGPUInstancingRenderer)objectTranslator.GetObject(L, 2, typeof(IWorldGPUInstancingRenderer));
			worldGPUInstancingRenderFeature.AddRenderer(renderer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			WorldGPUInstancingRenderFeature worldGPUInstancingRenderFeature = (WorldGPUInstancingRenderFeature)objectTranslator.FastGetCSObj(L, 1);
			IWorldGPUInstancingRenderer renderer = (IWorldGPUInstancingRenderer)objectTranslator.GetObject(L, 2, typeof(IWorldGPUInstancingRenderer));
			worldGPUInstancingRenderFeature.RemoveRenderer(renderer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ClearAllPass(IntPtr L)
	{
		try
		{
			((WorldGPUInstancingRenderFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ClearAllPass();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Description(IntPtr L)
	{
		try
		{
			string str = ((WorldGPUInstancingRenderFeature)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Description();
			Lua.lua_pushstring(L, str);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}
}
