using System;
using UnityEngine.Rendering.Universal;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineRenderingUniversalUniversalRenderPipelineAssetWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(UniversalRenderPipelineAsset);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 5, 59, 30);
		Utils.RegisterFunc(L, -3, "LoadBuiltinRendererData", _m_LoadBuiltinRendererData);
		Utils.RegisterFunc(L, -3, "GetRenderer", _m_GetRenderer);
		Utils.RegisterFunc(L, -3, "ToggleShadow", _m_ToggleShadow);
		Utils.RegisterFunc(L, -3, "OnBeforeSerialize", _m_OnBeforeSerialize);
		Utils.RegisterFunc(L, -3, "OnAfterDeserialize", _m_OnAfterDeserialize);
		Utils.RegisterFunc(L, -2, "scriptableRenderer", _g_get_scriptableRenderer);
		Utils.RegisterFunc(L, -2, "supportsCameraDepthTexture", _g_get_supportsCameraDepthTexture);
		Utils.RegisterFunc(L, -2, "allowSoftParticles", _g_get_allowSoftParticles);
		Utils.RegisterFunc(L, -2, "supportsCameraOpaqueTexture", _g_get_supportsCameraOpaqueTexture);
		Utils.RegisterFunc(L, -2, "opaqueDownsampling", _g_get_opaqueDownsampling);
		Utils.RegisterFunc(L, -2, "supportsTerrainHoles", _g_get_supportsTerrainHoles);
		Utils.RegisterFunc(L, -2, "supportsHDR", _g_get_supportsHDR);
		Utils.RegisterFunc(L, -2, "supportsPostProcess", _g_get_supportsPostProcess);
		Utils.RegisterFunc(L, -2, "doFxaaInUberPost", _g_get_doFxaaInUberPost);
		Utils.RegisterFunc(L, -2, "msaaSampleCount", _g_get_msaaSampleCount);
		Utils.RegisterFunc(L, -2, "renderScale", _g_get_renderScale);
		Utils.RegisterFunc(L, -2, "minRenderResolution", _g_get_minRenderResolution);
		Utils.RegisterFunc(L, -2, "maxRenderResolution", _g_get_maxRenderResolution);
		Utils.RegisterFunc(L, -2, "foldableResolution", _g_get_foldableResolution);
		Utils.RegisterFunc(L, -2, "foldableAspectRatio", _g_get_foldableAspectRatio);
		Utils.RegisterFunc(L, -2, "foldableRenderScale", _g_get_foldableRenderScale);
		Utils.RegisterFunc(L, -2, "mainLightRenderingMode", _g_get_mainLightRenderingMode);
		Utils.RegisterFunc(L, -2, "supportsMainLightShadows", _g_get_supportsMainLightShadows);
		Utils.RegisterFunc(L, -2, "mainLightShadowmapResolution", _g_get_mainLightShadowmapResolution);
		Utils.RegisterFunc(L, -2, "additionalLightsRenderingMode", _g_get_additionalLightsRenderingMode);
		Utils.RegisterFunc(L, -2, "maxAdditionalLightsCount", _g_get_maxAdditionalLightsCount);
		Utils.RegisterFunc(L, -2, "supportsAdditionalLightShadows", _g_get_supportsAdditionalLightShadows);
		Utils.RegisterFunc(L, -2, "additionalLightsShadowmapResolution", _g_get_additionalLightsShadowmapResolution);
		Utils.RegisterFunc(L, -2, "shadowDistance", _g_get_shadowDistance);
		Utils.RegisterFunc(L, -2, "shadowCascadeOption", _g_get_shadowCascadeOption);
		Utils.RegisterFunc(L, -2, "cascade2Split", _g_get_cascade2Split);
		Utils.RegisterFunc(L, -2, "cascade4Split", _g_get_cascade4Split);
		Utils.RegisterFunc(L, -2, "shadowDepthBias", _g_get_shadowDepthBias);
		Utils.RegisterFunc(L, -2, "shadowNormalBias", _g_get_shadowNormalBias);
		Utils.RegisterFunc(L, -2, "supportsSoftShadows", _g_get_supportsSoftShadows);
		Utils.RegisterFunc(L, -2, "supportsDynamicBatching", _g_get_supportsDynamicBatching);
		Utils.RegisterFunc(L, -2, "supportsMixedLighting", _g_get_supportsMixedLighting);
		Utils.RegisterFunc(L, -2, "shaderVariantLogLevel", _g_get_shaderVariantLogLevel);
		Utils.RegisterFunc(L, -2, "debugLevel", _g_get_debugLevel);
		Utils.RegisterFunc(L, -2, "useSRPBatcher", _g_get_useSRPBatcher);
		Utils.RegisterFunc(L, -2, "postProcessingFeatureSet", _g_get_postProcessingFeatureSet);
		Utils.RegisterFunc(L, -2, "colorGradingMode", _g_get_colorGradingMode);
		Utils.RegisterFunc(L, -2, "colorGradingLutSize", _g_get_colorGradingLutSize);
		Utils.RegisterFunc(L, -2, "useAdaptivePerformance", _g_get_useAdaptivePerformance);
		Utils.RegisterFunc(L, -2, "defaultMaterial", _g_get_defaultMaterial);
		Utils.RegisterFunc(L, -2, "defaultParticleMaterial", _g_get_defaultParticleMaterial);
		Utils.RegisterFunc(L, -2, "defaultLineMaterial", _g_get_defaultLineMaterial);
		Utils.RegisterFunc(L, -2, "defaultTerrainMaterial", _g_get_defaultTerrainMaterial);
		Utils.RegisterFunc(L, -2, "defaultUIMaterial", _g_get_defaultUIMaterial);
		Utils.RegisterFunc(L, -2, "defaultUIOverdrawMaterial", _g_get_defaultUIOverdrawMaterial);
		Utils.RegisterFunc(L, -2, "defaultUIETC1SupportedMaterial", _g_get_defaultUIETC1SupportedMaterial);
		Utils.RegisterFunc(L, -2, "default2DMaterial", _g_get_default2DMaterial);
		Utils.RegisterFunc(L, -2, "defaultShader", _g_get_defaultShader);
		Utils.RegisterFunc(L, -2, "autodeskInteractiveShader", _g_get_autodeskInteractiveShader);
		Utils.RegisterFunc(L, -2, "autodeskInteractiveTransparentShader", _g_get_autodeskInteractiveTransparentShader);
		Utils.RegisterFunc(L, -2, "autodeskInteractiveMaskedShader", _g_get_autodeskInteractiveMaskedShader);
		Utils.RegisterFunc(L, -2, "terrainDetailLitShader", _g_get_terrainDetailLitShader);
		Utils.RegisterFunc(L, -2, "terrainDetailGrassShader", _g_get_terrainDetailGrassShader);
		Utils.RegisterFunc(L, -2, "terrainDetailGrassBillboardShader", _g_get_terrainDetailGrassBillboardShader);
		Utils.RegisterFunc(L, -2, "defaultSpeedTree7Shader", _g_get_defaultSpeedTree7Shader);
		Utils.RegisterFunc(L, -2, "defaultSpeedTree8Shader", _g_get_defaultSpeedTree8Shader);
		Utils.RegisterFunc(L, -2, "enableFxDistortionInPost", _g_get_enableFxDistortionInPost);
		Utils.RegisterFunc(L, -2, "antialiasingMode", _g_get_antialiasingMode);
		Utils.RegisterFunc(L, -2, "antialiasingQuality", _g_get_antialiasingQuality);
		Utils.RegisterFunc(L, -1, "supportsCameraDepthTexture", _s_set_supportsCameraDepthTexture);
		Utils.RegisterFunc(L, -1, "allowSoftParticles", _s_set_allowSoftParticles);
		Utils.RegisterFunc(L, -1, "supportsCameraOpaqueTexture", _s_set_supportsCameraOpaqueTexture);
		Utils.RegisterFunc(L, -1, "supportsHDR", _s_set_supportsHDR);
		Utils.RegisterFunc(L, -1, "supportsPostProcess", _s_set_supportsPostProcess);
		Utils.RegisterFunc(L, -1, "doFxaaInUberPost", _s_set_doFxaaInUberPost);
		Utils.RegisterFunc(L, -1, "msaaSampleCount", _s_set_msaaSampleCount);
		Utils.RegisterFunc(L, -1, "renderScale", _s_set_renderScale);
		Utils.RegisterFunc(L, -1, "minRenderResolution", _s_set_minRenderResolution);
		Utils.RegisterFunc(L, -1, "maxRenderResolution", _s_set_maxRenderResolution);
		Utils.RegisterFunc(L, -1, "foldableResolution", _s_set_foldableResolution);
		Utils.RegisterFunc(L, -1, "foldableAspectRatio", _s_set_foldableAspectRatio);
		Utils.RegisterFunc(L, -1, "foldableRenderScale", _s_set_foldableRenderScale);
		Utils.RegisterFunc(L, -1, "supportsMainLightShadows", _s_set_supportsMainLightShadows);
		Utils.RegisterFunc(L, -1, "mainLightShadowmapResolution", _s_set_mainLightShadowmapResolution);
		Utils.RegisterFunc(L, -1, "maxAdditionalLightsCount", _s_set_maxAdditionalLightsCount);
		Utils.RegisterFunc(L, -1, "shadowDistance", _s_set_shadowDistance);
		Utils.RegisterFunc(L, -1, "shadowCascadeOption", _s_set_shadowCascadeOption);
		Utils.RegisterFunc(L, -1, "shadowDepthBias", _s_set_shadowDepthBias);
		Utils.RegisterFunc(L, -1, "shadowNormalBias", _s_set_shadowNormalBias);
		Utils.RegisterFunc(L, -1, "supportsDynamicBatching", _s_set_supportsDynamicBatching);
		Utils.RegisterFunc(L, -1, "shaderVariantLogLevel", _s_set_shaderVariantLogLevel);
		Utils.RegisterFunc(L, -1, "useSRPBatcher", _s_set_useSRPBatcher);
		Utils.RegisterFunc(L, -1, "postProcessingFeatureSet", _s_set_postProcessingFeatureSet);
		Utils.RegisterFunc(L, -1, "colorGradingMode", _s_set_colorGradingMode);
		Utils.RegisterFunc(L, -1, "colorGradingLutSize", _s_set_colorGradingLutSize);
		Utils.RegisterFunc(L, -1, "useAdaptivePerformance", _s_set_useAdaptivePerformance);
		Utils.RegisterFunc(L, -1, "enableFxDistortionInPost", _s_set_enableFxDistortionInPost);
		Utils.RegisterFunc(L, -1, "antialiasingMode", _s_set_antialiasingMode);
		Utils.RegisterFunc(L, -1, "antialiasingQuality", _s_set_antialiasingQuality);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 3, 0, 0);
		Utils.RegisterObject(L, translator, -4, "k_MinLutSize", 16);
		Utils.RegisterObject(L, translator, -4, "k_MaxLutSize", 65);
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
				UniversalRenderPipelineAsset o = new UniversalRenderPipelineAsset();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_LoadBuiltinRendererData(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<RendererType>(L, 2))
			{
				objectTranslator.Get(L, 2, out RendererType v);
				ScriptableRendererData o = universalRenderPipelineAsset.LoadBuiltinRendererData(v);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 1)
			{
				ScriptableRendererData o2 = universalRenderPipelineAsset.LoadBuiltinRendererData();
				objectTranslator.Push(L, o2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset.LoadBuiltinRendererData!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset obj = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			int index = Lua.xlua_tointeger(L, 2);
			ScriptableRenderer renderer = obj.GetRenderer(index);
			objectTranslator.Push(L, renderer);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ToggleShadow(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset obj = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			bool isToggle = Lua.lua_toboolean(L, 2);
			obj.ToggleShadow(isToggle);
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
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnBeforeSerialize();
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
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).OnAfterDeserialize();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scriptableRenderer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.scriptableRenderer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsCameraDepthTexture(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsCameraDepthTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowSoftParticles(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.allowSoftParticles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsCameraOpaqueTexture(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsCameraOpaqueTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_opaqueDownsampling(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.opaqueDownsampling);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsTerrainHoles(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsTerrainHoles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsHDR(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsHDR);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsPostProcess(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsPostProcess);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_doFxaaInUberPost(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.doFxaaInUberPost);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_msaaSampleCount(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.msaaSampleCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderScale(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.renderScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minRenderResolution(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.minRenderResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxRenderResolution(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.maxRenderResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_foldableResolution(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.foldableResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_foldableAspectRatio(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.foldableAspectRatio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_foldableRenderScale(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.foldableRenderScale);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainLightRenderingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.mainLightRenderingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsMainLightShadows(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsMainLightShadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_mainLightShadowmapResolution(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.mainLightShadowmapResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_additionalLightsRenderingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.additionalLightsRenderingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxAdditionalLightsCount(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.maxAdditionalLightsCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsAdditionalLightShadows(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsAdditionalLightShadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_additionalLightsShadowmapResolution(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.additionalLightsShadowmapResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowDistance(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.shadowDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowCascadeOption(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.shadowCascadeOption);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cascade2Split(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.cascade2Split);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cascade4Split(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, universalRenderPipelineAsset.cascade4Split);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowDepthBias(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.shadowDepthBias);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowNormalBias(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, universalRenderPipelineAsset.shadowNormalBias);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsSoftShadows(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsSoftShadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsDynamicBatching(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsDynamicBatching);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsMixedLighting(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.supportsMixedLighting);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shaderVariantLogLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.shaderVariantLogLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_debugLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.debugLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useSRPBatcher(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.useSRPBatcher);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_postProcessingFeatureSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.postProcessingFeatureSet);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorGradingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.colorGradingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_colorGradingLutSize(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, universalRenderPipelineAsset.colorGradingLutSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useAdaptivePerformance(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.useAdaptivePerformance);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultParticleMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultLineMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultTerrainMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultUIMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultUIOverdrawMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultUIETC1SupportedMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.default2DMaterial);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.autodeskInteractiveShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.autodeskInteractiveTransparentShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.autodeskInteractiveMaskedShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.terrainDetailLitShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.terrainDetailGrassShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.terrainDetailGrassBillboardShader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultSpeedTree7Shader);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.defaultSpeedTree8Shader);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_enableFxDistortionInPost(IntPtr L)
	{
		try
		{
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, universalRenderPipelineAsset.enableFxDistortionInPost);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_antialiasingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineRenderingUniversalAntialiasingMode(L, universalRenderPipelineAsset.antialiasingMode);
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, universalRenderPipelineAsset.antialiasingQuality);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportsCameraDepthTexture(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportsCameraDepthTexture = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowSoftParticles(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowSoftParticles = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportsCameraOpaqueTexture(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportsCameraOpaqueTexture = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportsHDR(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportsHDR = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportsPostProcess(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportsPostProcess = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_doFxaaInUberPost(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).doFxaaInUberPost = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_msaaSampleCount(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).msaaSampleCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderScale(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).renderScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_minRenderResolution(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).minRenderResolution = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxRenderResolution(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxRenderResolution = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_foldableResolution(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).foldableResolution = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_foldableAspectRatio(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).foldableAspectRatio = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_foldableRenderScale(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).foldableRenderScale = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportsMainLightShadows(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportsMainLightShadows = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_mainLightShadowmapResolution(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).mainLightShadowmapResolution = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxAdditionalLightsCount(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).maxAdditionalLightsCount = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowDistance(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shadowDistance = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowCascadeOption(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ShadowCascadesOption v);
			universalRenderPipelineAsset.shadowCascadeOption = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowDepthBias(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shadowDepthBias = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowNormalBias(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).shadowNormalBias = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_supportsDynamicBatching(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).supportsDynamicBatching = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shaderVariantLogLevel(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ShaderVariantLogLevel v);
			universalRenderPipelineAsset.shaderVariantLogLevel = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useSRPBatcher(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useSRPBatcher = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_postProcessingFeatureSet(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out PostProcessingFeatureSet v);
			universalRenderPipelineAsset.postProcessingFeatureSet = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorGradingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out ColorGradingMode v);
			universalRenderPipelineAsset.colorGradingMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_colorGradingLutSize(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).colorGradingLutSize = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useAdaptivePerformance(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useAdaptivePerformance = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_enableFxDistortionInPost(IntPtr L)
	{
		try
		{
			((UniversalRenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).enableFxDistortionInPost = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_antialiasingMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AntialiasingMode val);
			universalRenderPipelineAsset.antialiasingMode = val;
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
			UniversalRenderPipelineAsset universalRenderPipelineAsset = (UniversalRenderPipelineAsset)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out AntialiasingQuality v);
			universalRenderPipelineAsset.antialiasingQuality = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
