using System;
using UnityEngine;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineQualitySettingsWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(QualitySettings);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 6, 36, 33);
		Utils.RegisterFunc(L, -4, "IncreaseLevel", _m_IncreaseLevel_xlua_st_);
		Utils.RegisterFunc(L, -4, "DecreaseLevel", _m_DecreaseLevel_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetQualityLevel", _m_SetQualityLevel_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetRenderPipelineAssetAt", _m_GetRenderPipelineAssetAt_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetQualityLevel", _m_GetQualityLevel_xlua_st_);
		Utils.RegisterFunc(L, -2, "pixelLightCount", _g_get_pixelLightCount);
		Utils.RegisterFunc(L, -2, "shadows", _g_get_shadows);
		Utils.RegisterFunc(L, -2, "shadowProjection", _g_get_shadowProjection);
		Utils.RegisterFunc(L, -2, "shadowCascades", _g_get_shadowCascades);
		Utils.RegisterFunc(L, -2, "shadowDistance", _g_get_shadowDistance);
		Utils.RegisterFunc(L, -2, "shadowResolution", _g_get_shadowResolution);
		Utils.RegisterFunc(L, -2, "shadowmaskMode", _g_get_shadowmaskMode);
		Utils.RegisterFunc(L, -2, "shadowNearPlaneOffset", _g_get_shadowNearPlaneOffset);
		Utils.RegisterFunc(L, -2, "shadowCascade2Split", _g_get_shadowCascade2Split);
		Utils.RegisterFunc(L, -2, "shadowCascade4Split", _g_get_shadowCascade4Split);
		Utils.RegisterFunc(L, -2, "lodBias", _g_get_lodBias);
		Utils.RegisterFunc(L, -2, "anisotropicFiltering", _g_get_anisotropicFiltering);
		Utils.RegisterFunc(L, -2, "masterTextureLimit", _g_get_masterTextureLimit);
		Utils.RegisterFunc(L, -2, "maximumLODLevel", _g_get_maximumLODLevel);
		Utils.RegisterFunc(L, -2, "particleRaycastBudget", _g_get_particleRaycastBudget);
		Utils.RegisterFunc(L, -2, "softParticles", _g_get_softParticles);
		Utils.RegisterFunc(L, -2, "softVegetation", _g_get_softVegetation);
		Utils.RegisterFunc(L, -2, "vSyncCount", _g_get_vSyncCount);
		Utils.RegisterFunc(L, -2, "antiAliasing", _g_get_antiAliasing);
		Utils.RegisterFunc(L, -2, "asyncUploadTimeSlice", _g_get_asyncUploadTimeSlice);
		Utils.RegisterFunc(L, -2, "asyncUploadBufferSize", _g_get_asyncUploadBufferSize);
		Utils.RegisterFunc(L, -2, "asyncUploadPersistentBuffer", _g_get_asyncUploadPersistentBuffer);
		Utils.RegisterFunc(L, -2, "realtimeReflectionProbes", _g_get_realtimeReflectionProbes);
		Utils.RegisterFunc(L, -2, "billboardsFaceCameraPosition", _g_get_billboardsFaceCameraPosition);
		Utils.RegisterFunc(L, -2, "resolutionScalingFixedDPIFactor", _g_get_resolutionScalingFixedDPIFactor);
		Utils.RegisterFunc(L, -2, "renderPipeline", _g_get_renderPipeline);
		Utils.RegisterFunc(L, -2, "skinWeights", _g_get_skinWeights);
		Utils.RegisterFunc(L, -2, "streamingMipmapsActive", _g_get_streamingMipmapsActive);
		Utils.RegisterFunc(L, -2, "streamingMipmapsMemoryBudget", _g_get_streamingMipmapsMemoryBudget);
		Utils.RegisterFunc(L, -2, "streamingMipmapsMaxLevelReduction", _g_get_streamingMipmapsMaxLevelReduction);
		Utils.RegisterFunc(L, -2, "streamingMipmapsAddAllCameras", _g_get_streamingMipmapsAddAllCameras);
		Utils.RegisterFunc(L, -2, "streamingMipmapsMaxFileIORequests", _g_get_streamingMipmapsMaxFileIORequests);
		Utils.RegisterFunc(L, -2, "maxQueuedFrames", _g_get_maxQueuedFrames);
		Utils.RegisterFunc(L, -2, "names", _g_get_names);
		Utils.RegisterFunc(L, -2, "desiredColorSpace", _g_get_desiredColorSpace);
		Utils.RegisterFunc(L, -2, "activeColorSpace", _g_get_activeColorSpace);
		Utils.RegisterFunc(L, -1, "pixelLightCount", _s_set_pixelLightCount);
		Utils.RegisterFunc(L, -1, "shadows", _s_set_shadows);
		Utils.RegisterFunc(L, -1, "shadowProjection", _s_set_shadowProjection);
		Utils.RegisterFunc(L, -1, "shadowCascades", _s_set_shadowCascades);
		Utils.RegisterFunc(L, -1, "shadowDistance", _s_set_shadowDistance);
		Utils.RegisterFunc(L, -1, "shadowResolution", _s_set_shadowResolution);
		Utils.RegisterFunc(L, -1, "shadowmaskMode", _s_set_shadowmaskMode);
		Utils.RegisterFunc(L, -1, "shadowNearPlaneOffset", _s_set_shadowNearPlaneOffset);
		Utils.RegisterFunc(L, -1, "shadowCascade2Split", _s_set_shadowCascade2Split);
		Utils.RegisterFunc(L, -1, "shadowCascade4Split", _s_set_shadowCascade4Split);
		Utils.RegisterFunc(L, -1, "lodBias", _s_set_lodBias);
		Utils.RegisterFunc(L, -1, "anisotropicFiltering", _s_set_anisotropicFiltering);
		Utils.RegisterFunc(L, -1, "masterTextureLimit", _s_set_masterTextureLimit);
		Utils.RegisterFunc(L, -1, "maximumLODLevel", _s_set_maximumLODLevel);
		Utils.RegisterFunc(L, -1, "particleRaycastBudget", _s_set_particleRaycastBudget);
		Utils.RegisterFunc(L, -1, "softParticles", _s_set_softParticles);
		Utils.RegisterFunc(L, -1, "softVegetation", _s_set_softVegetation);
		Utils.RegisterFunc(L, -1, "vSyncCount", _s_set_vSyncCount);
		Utils.RegisterFunc(L, -1, "antiAliasing", _s_set_antiAliasing);
		Utils.RegisterFunc(L, -1, "asyncUploadTimeSlice", _s_set_asyncUploadTimeSlice);
		Utils.RegisterFunc(L, -1, "asyncUploadBufferSize", _s_set_asyncUploadBufferSize);
		Utils.RegisterFunc(L, -1, "asyncUploadPersistentBuffer", _s_set_asyncUploadPersistentBuffer);
		Utils.RegisterFunc(L, -1, "realtimeReflectionProbes", _s_set_realtimeReflectionProbes);
		Utils.RegisterFunc(L, -1, "billboardsFaceCameraPosition", _s_set_billboardsFaceCameraPosition);
		Utils.RegisterFunc(L, -1, "resolutionScalingFixedDPIFactor", _s_set_resolutionScalingFixedDPIFactor);
		Utils.RegisterFunc(L, -1, "renderPipeline", _s_set_renderPipeline);
		Utils.RegisterFunc(L, -1, "skinWeights", _s_set_skinWeights);
		Utils.RegisterFunc(L, -1, "streamingMipmapsActive", _s_set_streamingMipmapsActive);
		Utils.RegisterFunc(L, -1, "streamingMipmapsMemoryBudget", _s_set_streamingMipmapsMemoryBudget);
		Utils.RegisterFunc(L, -1, "streamingMipmapsMaxLevelReduction", _s_set_streamingMipmapsMaxLevelReduction);
		Utils.RegisterFunc(L, -1, "streamingMipmapsAddAllCameras", _s_set_streamingMipmapsAddAllCameras);
		Utils.RegisterFunc(L, -1, "streamingMipmapsMaxFileIORequests", _s_set_streamingMipmapsMaxFileIORequests);
		Utils.RegisterFunc(L, -1, "maxQueuedFrames", _s_set_maxQueuedFrames);
		Utils.EndClassRegister(typeFromHandle, L, translator);
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int __CreateInstance(IntPtr L)
	{
		return Lua.luaL_error(L, "UnityEngine.QualitySettings does not have a constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IncreaseLevel_xlua_st_(IntPtr L)
	{
		try
		{
			switch (Lua.lua_gettop(L))
			{
			case 0:
				QualitySettings.IncreaseLevel();
				return 0;
			case 1:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
				{
					QualitySettings.IncreaseLevel(Lua.lua_toboolean(L, 1));
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.QualitySettings.IncreaseLevel!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DecreaseLevel_xlua_st_(IntPtr L)
	{
		try
		{
			switch (Lua.lua_gettop(L))
			{
			case 0:
				QualitySettings.DecreaseLevel();
				return 0;
			case 1:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 1))
				{
					QualitySettings.DecreaseLevel(Lua.lua_toboolean(L, 1));
					return 0;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.QualitySettings.DecreaseLevel!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetQualityLevel_xlua_st_(IntPtr L)
	{
		try
		{
			int num = Lua.lua_gettop(L);
			if (num == 1 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1))
			{
				QualitySettings.SetQualityLevel(Lua.xlua_tointeger(L, 1));
				return 0;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
			{
				int index = Lua.xlua_tointeger(L, 1);
				bool applyExpensiveChanges = Lua.lua_toboolean(L, 2);
				QualitySettings.SetQualityLevel(index, applyExpensiveChanges);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.QualitySettings.SetQualityLevel!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetRenderPipelineAssetAt_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			RenderPipelineAsset renderPipelineAssetAt = QualitySettings.GetRenderPipelineAssetAt(Lua.xlua_tointeger(L, 1));
			objectTranslator.Push(L, renderPipelineAssetAt);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetQualityLevel_xlua_st_(IntPtr L)
	{
		try
		{
			int qualityLevel = QualitySettings.GetQualityLevel();
			Lua.xlua_pushinteger(L, qualityLevel);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelLightCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.pixelLightCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadows(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.shadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowProjection(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.shadowProjection);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowCascades(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.shadowCascades);
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
			Lua.lua_pushnumber(L, QualitySettings.shadowDistance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowResolution(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.shadowResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowmaskMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.shadowmaskMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowNearPlaneOffset(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, QualitySettings.shadowNearPlaneOffset);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowCascade2Split(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, QualitySettings.shadowCascade2Split);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_shadowCascade4Split(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).PushUnityEngineVector3(L, QualitySettings.shadowCascade4Split);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lodBias(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, QualitySettings.lodBias);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_anisotropicFiltering(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.anisotropicFiltering);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_masterTextureLimit(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.masterTextureLimit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maximumLODLevel(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.maximumLODLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_particleRaycastBudget(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.particleRaycastBudget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_softParticles(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.softParticles);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_softVegetation(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.softVegetation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_vSyncCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.vSyncCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_antiAliasing(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.antiAliasing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncUploadTimeSlice(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.asyncUploadTimeSlice);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncUploadBufferSize(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.asyncUploadBufferSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_asyncUploadPersistentBuffer(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.asyncUploadPersistentBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_realtimeReflectionProbes(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.realtimeReflectionProbes);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_billboardsFaceCameraPosition(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.billboardsFaceCameraPosition);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_resolutionScalingFixedDPIFactor(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, QualitySettings.resolutionScalingFixedDPIFactor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderPipeline(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.renderPipeline);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_skinWeights(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.skinWeights);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_streamingMipmapsActive(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.streamingMipmapsActive);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_streamingMipmapsMemoryBudget(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, QualitySettings.streamingMipmapsMemoryBudget);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_streamingMipmapsMaxLevelReduction(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.streamingMipmapsMaxLevelReduction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_streamingMipmapsAddAllCameras(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, QualitySettings.streamingMipmapsAddAllCameras);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_streamingMipmapsMaxFileIORequests(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.streamingMipmapsMaxFileIORequests);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxQueuedFrames(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, QualitySettings.maxQueuedFrames);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_names(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.names);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_desiredColorSpace(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.desiredColorSpace);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeColorSpace(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, QualitySettings.activeColorSpace);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pixelLightCount(IntPtr L)
	{
		try
		{
			QualitySettings.pixelLightCount = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadows(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ShadowQuality v);
			QualitySettings.shadows = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowProjection(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ShadowProjection v);
			QualitySettings.shadowProjection = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowCascades(IntPtr L)
	{
		try
		{
			QualitySettings.shadowCascades = Lua.xlua_tointeger(L, 1);
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
			QualitySettings.shadowDistance = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowResolution(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ShadowResolution v);
			QualitySettings.shadowResolution = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowmaskMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out ShadowmaskMode v);
			QualitySettings.shadowmaskMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowNearPlaneOffset(IntPtr L)
	{
		try
		{
			QualitySettings.shadowNearPlaneOffset = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowCascade2Split(IntPtr L)
	{
		try
		{
			QualitySettings.shadowCascade2Split = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_shadowCascade4Split(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out Vector3 val);
			QualitySettings.shadowCascade4Split = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lodBias(IntPtr L)
	{
		try
		{
			QualitySettings.lodBias = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_anisotropicFiltering(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out AnisotropicFiltering v);
			QualitySettings.anisotropicFiltering = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_masterTextureLimit(IntPtr L)
	{
		try
		{
			QualitySettings.masterTextureLimit = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maximumLODLevel(IntPtr L)
	{
		try
		{
			QualitySettings.maximumLODLevel = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_particleRaycastBudget(IntPtr L)
	{
		try
		{
			QualitySettings.particleRaycastBudget = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_softParticles(IntPtr L)
	{
		try
		{
			QualitySettings.softParticles = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_softVegetation(IntPtr L)
	{
		try
		{
			QualitySettings.softVegetation = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_vSyncCount(IntPtr L)
	{
		try
		{
			QualitySettings.vSyncCount = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_antiAliasing(IntPtr L)
	{
		try
		{
			QualitySettings.antiAliasing = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncUploadTimeSlice(IntPtr L)
	{
		try
		{
			QualitySettings.asyncUploadTimeSlice = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncUploadBufferSize(IntPtr L)
	{
		try
		{
			QualitySettings.asyncUploadBufferSize = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_asyncUploadPersistentBuffer(IntPtr L)
	{
		try
		{
			QualitySettings.asyncUploadPersistentBuffer = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_realtimeReflectionProbes(IntPtr L)
	{
		try
		{
			QualitySettings.realtimeReflectionProbes = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_billboardsFaceCameraPosition(IntPtr L)
	{
		try
		{
			QualitySettings.billboardsFaceCameraPosition = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_resolutionScalingFixedDPIFactor(IntPtr L)
	{
		try
		{
			QualitySettings.resolutionScalingFixedDPIFactor = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderPipeline(IntPtr L)
	{
		try
		{
			QualitySettings.renderPipeline = (RenderPipelineAsset)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(RenderPipelineAsset));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_skinWeights(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out SkinWeights v);
			QualitySettings.skinWeights = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_streamingMipmapsActive(IntPtr L)
	{
		try
		{
			QualitySettings.streamingMipmapsActive = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_streamingMipmapsMemoryBudget(IntPtr L)
	{
		try
		{
			QualitySettings.streamingMipmapsMemoryBudget = (float)Lua.lua_tonumber(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_streamingMipmapsMaxLevelReduction(IntPtr L)
	{
		try
		{
			QualitySettings.streamingMipmapsMaxLevelReduction = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_streamingMipmapsAddAllCameras(IntPtr L)
	{
		try
		{
			QualitySettings.streamingMipmapsAddAllCameras = Lua.lua_toboolean(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_streamingMipmapsMaxFileIORequests(IntPtr L)
	{
		try
		{
			QualitySettings.streamingMipmapsMaxFileIORequests = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_maxQueuedFrames(IntPtr L)
	{
		try
		{
			QualitySettings.maxQueuedFrames = Lua.xlua_tointeger(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
