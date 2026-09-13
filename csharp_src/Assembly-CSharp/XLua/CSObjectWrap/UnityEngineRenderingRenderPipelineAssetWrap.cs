using System;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingRenderPipelineAssetWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(RenderPipelineAsset);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 18, 0);
		Utils.RegisterFunc(L, -2, "renderingLayerMaskNames", _g_get_renderingLayerMaskNames);
		Utils.RegisterFunc(L, -2, "defaultMaterial", _g_get_defaultMaterial);
		Utils.RegisterFunc(L, -2, "autodeskInteractiveShader", _g_get_autodeskInteractiveShader);
		Utils.RegisterFunc(L, -2, "autodeskInteractiveTransparentShader", _g_get_autodeskInteractiveTransparentShader);
		Utils.RegisterFunc(L, -2, "autodeskInteractiveMaskedShader", _g_get_autodeskInteractiveMaskedShader);
		Utils.RegisterFunc(L, -2, "terrainDetailLitShader", _g_get_terrainDetailLitShader);
		Utils.RegisterFunc(L, -2, "terrainDetailGrassShader", _g_get_terrainDetailGrassShader);
		Utils.RegisterFunc(L, -2, "terrainDetailGrassBillboardShader", _g_get_terrainDetailGrassBillboardShader);
		Utils.RegisterFunc(L, -2, "defaultParticleMaterial", _g_get_defaultParticleMaterial);
		Utils.RegisterFunc(L, -2, "defaultLineMaterial", _g_get_defaultLineMaterial);
		Utils.RegisterFunc(L, -2, "defaultTerrainMaterial", _g_get_defaultTerrainMaterial);
		Utils.RegisterFunc(L, -2, "defaultUIMaterial", _g_get_defaultUIMaterial);
		Utils.RegisterFunc(L, -2, "defaultUIOverdrawMaterial", _g_get_defaultUIOverdrawMaterial);
		Utils.RegisterFunc(L, -2, "defaultUIETC1SupportedMaterial", _g_get_defaultUIETC1SupportedMaterial);
		Utils.RegisterFunc(L, -2, "default2DMaterial", _g_get_default2DMaterial);
		Utils.RegisterFunc(L, -2, "defaultShader", _g_get_defaultShader);
		Utils.RegisterFunc(L, -2, "defaultSpeedTree7Shader", _g_get_defaultSpeedTree7Shader);
		Utils.RegisterFunc(L, -2, "defaultSpeedTree8Shader", _g_get_defaultSpeedTree8Shader);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 1, 0, 0);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.Rendering.RenderPipelineAsset does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderingLayerMaskNames(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.renderingLayerMaskNames);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autodeskInteractiveShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.autodeskInteractiveShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autodeskInteractiveTransparentShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.autodeskInteractiveTransparentShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_autodeskInteractiveMaskedShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.autodeskInteractiveMaskedShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_terrainDetailLitShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.terrainDetailLitShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_terrainDetailGrassShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.terrainDetailGrassShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_terrainDetailGrassBillboardShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.terrainDetailGrassBillboardShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultParticleMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultParticleMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultLineMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultLineMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultTerrainMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultTerrainMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultUIMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultUIMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultUIOverdrawMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultUIOverdrawMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultUIETC1SupportedMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultUIETC1SupportedMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_default2DMaterial(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.default2DMaterial);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultShader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultSpeedTree7Shader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultSpeedTree7Shader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_defaultSpeedTree8Shader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAsset = (RenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, renderPipelineAsset.defaultSpeedTree8Shader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
