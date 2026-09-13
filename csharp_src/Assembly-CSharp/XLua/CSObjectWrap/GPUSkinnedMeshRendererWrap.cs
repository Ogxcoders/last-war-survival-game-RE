using System;
using System.Collections.Generic;
using UnityEngine;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class GPUSkinnedMeshRendererWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(GPUSkinnedMeshRenderer);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 1, 8, 5);
		Utils.RegisterFunc(L, -3, "ResetMaterial", _m_ResetMaterial);
		Utils.RegisterFunc(L, -2, "SharedMeshId", _g_get_SharedMeshId);
		Utils.RegisterFunc(L, -2, "materialPropertyBlockController", _g_get_materialPropertyBlockController);
		Utils.RegisterFunc(L, -2, "enableInstancing", _g_get_enableInstancing);
		Utils.RegisterFunc(L, -2, "info", _g_get_info);
		Utils.RegisterFunc(L, -2, "animator", _g_get_animator);
		Utils.RegisterFunc(L, -2, "meshRenderer", _g_get_meshRenderer);
		Utils.RegisterFunc(L, -2, "meshFilter", _g_get_meshFilter);
		Utils.RegisterFunc(L, -2, "simpleAnimation", _g_get_simpleAnimation);
		Utils.RegisterFunc(L, -1, "info", _s_set_info);
		Utils.RegisterFunc(L, -1, "animator", _s_set_animator);
		Utils.RegisterFunc(L, -1, "meshRenderer", _s_set_meshRenderer);
		Utils.RegisterFunc(L, -1, "meshFilter", _s_set_meshFilter);
		Utils.RegisterFunc(L, -1, "simpleAnimation", _s_set_simpleAnimation);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 2, 7, 5);
		Utils.RegisterFunc(L, -4, "UpdateAllActiveGpuSkinRenderer", _m_UpdateAllActiveGpuSkinRenderer_xlua_st_);
		Utils.RegisterFunc(L, -2, "crossFadeEnabled", _g_get_crossFadeEnabled);
		Utils.RegisterFunc(L, -2, "crossFadeInterruptEnabled", _g_get_crossFadeInterruptEnabled);
		Utils.RegisterFunc(L, -2, "activeRendererCount", _g_get_activeRendererCount);
		Utils.RegisterFunc(L, -2, "totalRendererCount", _g_get_totalRendererCount);
		Utils.RegisterFunc(L, -2, "s_ActiveRenderers", _g_get_s_ActiveRenderers);
		Utils.RegisterFunc(L, -2, "s_SimpleAnimRendererMap", _g_get_s_SimpleAnimRendererMap);
		Utils.RegisterFunc(L, -2, "s_AllNonSimpleAnimGpuSkinRenderers", _g_get_s_AllNonSimpleAnimGpuSkinRenderers);
		Utils.RegisterFunc(L, -1, "crossFadeEnabled", _s_set_crossFadeEnabled);
		Utils.RegisterFunc(L, -1, "crossFadeInterruptEnabled", _s_set_crossFadeInterruptEnabled);
		Utils.RegisterFunc(L, -1, "s_ActiveRenderers", _s_set_s_ActiveRenderers);
		Utils.RegisterFunc(L, -1, "s_SimpleAnimRendererMap", _s_set_s_SimpleAnimRendererMap);
		Utils.RegisterFunc(L, -1, "s_AllNonSimpleAnimGpuSkinRenderers", _s_set_s_AllNonSimpleAnimGpuSkinRenderers);
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
				GPUSkinnedMeshRenderer o = new GPUSkinnedMeshRenderer();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to GPUSkinnedMeshRenderer constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetMaterial(IntPtr L)
	{
		try
		{
			((GPUSkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetMaterial();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_UpdateAllActiveGpuSkinRenderer_xlua_st_(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer.UpdateAllActiveGpuSkinRenderer();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_crossFadeEnabled(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GPUSkinnedMeshRenderer.crossFadeEnabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_crossFadeInterruptEnabled(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, GPUSkinnedMeshRenderer.crossFadeInterruptEnabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_SharedMeshId(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, gPUSkinnedMeshRenderer.SharedMeshId);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_materialPropertyBlockController(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinnedMeshRenderer.materialPropertyBlockController);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableInstancing(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, gPUSkinnedMeshRenderer.enableInstancing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeRendererCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GPUSkinnedMeshRenderer.activeRendererCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_totalRendererCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, GPUSkinnedMeshRenderer.totalRendererCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_info(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinnedMeshRenderer.info);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_animator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinnedMeshRenderer.animator);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinnedMeshRenderer.meshRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_meshFilter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinnedMeshRenderer.meshFilter);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_simpleAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			GPUSkinnedMeshRenderer gPUSkinnedMeshRenderer = (GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, gPUSkinnedMeshRenderer.simpleAnimation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_s_ActiveRenderers(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GPUSkinnedMeshRenderer.s_ActiveRenderers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_s_SimpleAnimRendererMap(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GPUSkinnedMeshRenderer.s_SimpleAnimRendererMap);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_s_AllNonSimpleAnimGpuSkinRenderers(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, GPUSkinnedMeshRenderer.s_AllNonSimpleAnimGpuSkinRenderers);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_crossFadeEnabled(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer.crossFadeEnabled = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_crossFadeInterruptEnabled(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer.crossFadeInterruptEnabled = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_info(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).info = (GPUSkinnedMeshInfo)objectTranslator.GetObject(L, 2, typeof(GPUSkinnedMeshInfo));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_animator(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).animator = (Animator)objectTranslator.GetObject(L, 2, typeof(Animator));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).meshRenderer = (MeshRenderer)objectTranslator.GetObject(L, 2, typeof(MeshRenderer));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_meshFilter(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).meshFilter = (MeshFilter)objectTranslator.GetObject(L, 2, typeof(MeshFilter));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_simpleAnimation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((GPUSkinnedMeshRenderer)objectTranslator.FastGetCSObj(L, 1)).simpleAnimation = (SimpleAnimation)objectTranslator.GetObject(L, 2, typeof(SimpleAnimation));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_s_ActiveRenderers(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer.s_ActiveRenderers = (Dictionary<int, HashSet<GPUSkinnedMeshRenderer>>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<int, HashSet<GPUSkinnedMeshRenderer>>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_s_SimpleAnimRendererMap(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer.s_SimpleAnimRendererMap = (Dictionary<SimpleAnimation, HashSet<GPUSkinnedMeshRenderer>>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Dictionary<SimpleAnimation, HashSet<GPUSkinnedMeshRenderer>>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_s_AllNonSimpleAnimGpuSkinRenderers(IntPtr L)
	{
		try
		{
			GPUSkinnedMeshRenderer.s_AllNonSimpleAnimGpuSkinRenderers = (HashSet<GPUSkinnedMeshRenderer>)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(HashSet<GPUSkinnedMeshRenderer>));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
