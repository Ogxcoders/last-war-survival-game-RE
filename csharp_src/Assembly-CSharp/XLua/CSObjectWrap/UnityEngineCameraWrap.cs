using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineCameraWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(Camera);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 51, 57, 44);
		Utils.RegisterFunc(L, -3, "Reset", _m_Reset);
		Utils.RegisterFunc(L, -3, "ResetTransparencySortSettings", _m_ResetTransparencySortSettings);
		Utils.RegisterFunc(L, -3, "ResetAspect", _m_ResetAspect);
		Utils.RegisterFunc(L, -3, "ResetCullingMatrix", _m_ResetCullingMatrix);
		Utils.RegisterFunc(L, -3, "SetReplacementShader", _m_SetReplacementShader);
		Utils.RegisterFunc(L, -3, "ResetReplacementShader", _m_ResetReplacementShader);
		Utils.RegisterFunc(L, -3, "GetGateFittedFieldOfView", _m_GetGateFittedFieldOfView);
		Utils.RegisterFunc(L, -3, "GetGateFittedLensShift", _m_GetGateFittedLensShift);
		Utils.RegisterFunc(L, -3, "SetTargetBuffers", _m_SetTargetBuffers);
		Utils.RegisterFunc(L, -3, "ResetWorldToCameraMatrix", _m_ResetWorldToCameraMatrix);
		Utils.RegisterFunc(L, -3, "ResetProjectionMatrix", _m_ResetProjectionMatrix);
		Utils.RegisterFunc(L, -3, "CalculateObliqueMatrix", _m_CalculateObliqueMatrix);
		Utils.RegisterFunc(L, -3, "WorldToScreenPoint", _m_WorldToScreenPoint);
		Utils.RegisterFunc(L, -3, "WorldToViewportPoint", _m_WorldToViewportPoint);
		Utils.RegisterFunc(L, -3, "ViewportToWorldPoint", _m_ViewportToWorldPoint);
		Utils.RegisterFunc(L, -3, "ScreenToWorldPoint", _m_ScreenToWorldPoint);
		Utils.RegisterFunc(L, -3, "ScreenToViewportPoint", _m_ScreenToViewportPoint);
		Utils.RegisterFunc(L, -3, "ViewportToScreenPoint", _m_ViewportToScreenPoint);
		Utils.RegisterFunc(L, -3, "ViewportPointToRay", _m_ViewportPointToRay);
		Utils.RegisterFunc(L, -3, "ScreenPointToRay", _m_ScreenPointToRay);
		Utils.RegisterFunc(L, -3, "CalculateFrustumCorners", _m_CalculateFrustumCorners);
		Utils.RegisterFunc(L, -3, "GetStereoNonJitteredProjectionMatrix", _m_GetStereoNonJitteredProjectionMatrix);
		Utils.RegisterFunc(L, -3, "GetStereoViewMatrix", _m_GetStereoViewMatrix);
		Utils.RegisterFunc(L, -3, "CopyStereoDeviceProjectionMatrixToNonJittered", _m_CopyStereoDeviceProjectionMatrixToNonJittered);
		Utils.RegisterFunc(L, -3, "GetStereoProjectionMatrix", _m_GetStereoProjectionMatrix);
		Utils.RegisterFunc(L, -3, "SetStereoProjectionMatrix", _m_SetStereoProjectionMatrix);
		Utils.RegisterFunc(L, -3, "ResetStereoProjectionMatrices", _m_ResetStereoProjectionMatrices);
		Utils.RegisterFunc(L, -3, "SetStereoViewMatrix", _m_SetStereoViewMatrix);
		Utils.RegisterFunc(L, -3, "ResetStereoViewMatrices", _m_ResetStereoViewMatrices);
		Utils.RegisterFunc(L, -3, "RenderToCubemap", _m_RenderToCubemap);
		Utils.RegisterFunc(L, -3, "Render", _m_Render);
		Utils.RegisterFunc(L, -3, "RenderWithShader", _m_RenderWithShader);
		Utils.RegisterFunc(L, -3, "RenderDontRestore", _m_RenderDontRestore);
		Utils.RegisterFunc(L, -3, "CopyFrom", _m_CopyFrom);
		Utils.RegisterFunc(L, -3, "RemoveCommandBuffers", _m_RemoveCommandBuffers);
		Utils.RegisterFunc(L, -3, "RemoveAllCommandBuffers", _m_RemoveAllCommandBuffers);
		Utils.RegisterFunc(L, -3, "AddCommandBuffer", _m_AddCommandBuffer);
		Utils.RegisterFunc(L, -3, "AddCommandBufferAsync", _m_AddCommandBufferAsync);
		Utils.RegisterFunc(L, -3, "RemoveCommandBuffer", _m_RemoveCommandBuffer);
		Utils.RegisterFunc(L, -3, "GetCommandBuffers", _m_GetCommandBuffers);
		Utils.RegisterFunc(L, -3, "TryGetCullingParameters", _m_TryGetCullingParameters);
		Utils.RegisterFunc(L, -3, "DOAspect", _m_DOAspect);
		Utils.RegisterFunc(L, -3, "DOColor", _m_DOColor);
		Utils.RegisterFunc(L, -3, "DOFarClipPlane", _m_DOFarClipPlane);
		Utils.RegisterFunc(L, -3, "DOFieldOfView", _m_DOFieldOfView);
		Utils.RegisterFunc(L, -3, "DONearClipPlane", _m_DONearClipPlane);
		Utils.RegisterFunc(L, -3, "DOOrthoSize", _m_DOOrthoSize);
		Utils.RegisterFunc(L, -3, "DOPixelRect", _m_DOPixelRect);
		Utils.RegisterFunc(L, -3, "DORect", _m_DORect);
		Utils.RegisterFunc(L, -3, "DOShakePosition", _m_DOShakePosition);
		Utils.RegisterFunc(L, -3, "DOShakeRotation", _m_DOShakeRotation);
		Utils.RegisterFunc(L, -2, "nearClipPlane", _g_get_nearClipPlane);
		Utils.RegisterFunc(L, -2, "farClipPlane", _g_get_farClipPlane);
		Utils.RegisterFunc(L, -2, "fieldOfView", _g_get_fieldOfView);
		Utils.RegisterFunc(L, -2, "renderingPath", _g_get_renderingPath);
		Utils.RegisterFunc(L, -2, "actualRenderingPath", _g_get_actualRenderingPath);
		Utils.RegisterFunc(L, -2, "allowHDR", _g_get_allowHDR);
		Utils.RegisterFunc(L, -2, "allowMSAA", _g_get_allowMSAA);
		Utils.RegisterFunc(L, -2, "allowDynamicResolution", _g_get_allowDynamicResolution);
		Utils.RegisterFunc(L, -2, "forceIntoRenderTexture", _g_get_forceIntoRenderTexture);
		Utils.RegisterFunc(L, -2, "orthographicSize", _g_get_orthographicSize);
		Utils.RegisterFunc(L, -2, "orthographic", _g_get_orthographic);
		Utils.RegisterFunc(L, -2, "opaqueSortMode", _g_get_opaqueSortMode);
		Utils.RegisterFunc(L, -2, "transparencySortMode", _g_get_transparencySortMode);
		Utils.RegisterFunc(L, -2, "transparencySortAxis", _g_get_transparencySortAxis);
		Utils.RegisterFunc(L, -2, "depth", _g_get_depth);
		Utils.RegisterFunc(L, -2, "aspect", _g_get_aspect);
		Utils.RegisterFunc(L, -2, "velocity", _g_get_velocity);
		Utils.RegisterFunc(L, -2, "cullingMask", _g_get_cullingMask);
		Utils.RegisterFunc(L, -2, "eventMask", _g_get_eventMask);
		Utils.RegisterFunc(L, -2, "layerCullSpherical", _g_get_layerCullSpherical);
		Utils.RegisterFunc(L, -2, "cameraType", _g_get_cameraType);
		Utils.RegisterFunc(L, -2, "overrideSceneCullingMask", _g_get_overrideSceneCullingMask);
		Utils.RegisterFunc(L, -2, "layerCullDistances", _g_get_layerCullDistances);
		Utils.RegisterFunc(L, -2, "useOcclusionCulling", _g_get_useOcclusionCulling);
		Utils.RegisterFunc(L, -2, "cullingMatrix", _g_get_cullingMatrix);
		Utils.RegisterFunc(L, -2, "backgroundColor", _g_get_backgroundColor);
		Utils.RegisterFunc(L, -2, "clearFlags", _g_get_clearFlags);
		Utils.RegisterFunc(L, -2, "depthTextureMode", _g_get_depthTextureMode);
		Utils.RegisterFunc(L, -2, "clearStencilAfterLightingPass", _g_get_clearStencilAfterLightingPass);
		Utils.RegisterFunc(L, -2, "usePhysicalProperties", _g_get_usePhysicalProperties);
		Utils.RegisterFunc(L, -2, "sensorSize", _g_get_sensorSize);
		Utils.RegisterFunc(L, -2, "lensShift", _g_get_lensShift);
		Utils.RegisterFunc(L, -2, "focalLength", _g_get_focalLength);
		Utils.RegisterFunc(L, -2, "gateFit", _g_get_gateFit);
		Utils.RegisterFunc(L, -2, "rect", _g_get_rect);
		Utils.RegisterFunc(L, -2, "pixelRect", _g_get_pixelRect);
		Utils.RegisterFunc(L, -2, "pixelWidth", _g_get_pixelWidth);
		Utils.RegisterFunc(L, -2, "pixelHeight", _g_get_pixelHeight);
		Utils.RegisterFunc(L, -2, "scaledPixelWidth", _g_get_scaledPixelWidth);
		Utils.RegisterFunc(L, -2, "scaledPixelHeight", _g_get_scaledPixelHeight);
		Utils.RegisterFunc(L, -2, "targetTexture", _g_get_targetTexture);
		Utils.RegisterFunc(L, -2, "activeTexture", _g_get_activeTexture);
		Utils.RegisterFunc(L, -2, "targetDisplay", _g_get_targetDisplay);
		Utils.RegisterFunc(L, -2, "cameraToWorldMatrix", _g_get_cameraToWorldMatrix);
		Utils.RegisterFunc(L, -2, "worldToCameraMatrix", _g_get_worldToCameraMatrix);
		Utils.RegisterFunc(L, -2, "projectionMatrix", _g_get_projectionMatrix);
		Utils.RegisterFunc(L, -2, "nonJitteredProjectionMatrix", _g_get_nonJitteredProjectionMatrix);
		Utils.RegisterFunc(L, -2, "useJitteredProjectionMatrixForTransparentRendering", _g_get_useJitteredProjectionMatrixForTransparentRendering);
		Utils.RegisterFunc(L, -2, "previousViewProjectionMatrix", _g_get_previousViewProjectionMatrix);
		Utils.RegisterFunc(L, -2, "scene", _g_get_scene);
		Utils.RegisterFunc(L, -2, "stereoEnabled", _g_get_stereoEnabled);
		Utils.RegisterFunc(L, -2, "stereoSeparation", _g_get_stereoSeparation);
		Utils.RegisterFunc(L, -2, "stereoConvergence", _g_get_stereoConvergence);
		Utils.RegisterFunc(L, -2, "areVRStereoViewMatricesWithinSingleCullTolerance", _g_get_areVRStereoViewMatricesWithinSingleCullTolerance);
		Utils.RegisterFunc(L, -2, "stereoTargetEye", _g_get_stereoTargetEye);
		Utils.RegisterFunc(L, -2, "stereoActiveEye", _g_get_stereoActiveEye);
		Utils.RegisterFunc(L, -2, "commandBufferCount", _g_get_commandBufferCount);
		Utils.RegisterFunc(L, -1, "nearClipPlane", _s_set_nearClipPlane);
		Utils.RegisterFunc(L, -1, "farClipPlane", _s_set_farClipPlane);
		Utils.RegisterFunc(L, -1, "fieldOfView", _s_set_fieldOfView);
		Utils.RegisterFunc(L, -1, "renderingPath", _s_set_renderingPath);
		Utils.RegisterFunc(L, -1, "allowHDR", _s_set_allowHDR);
		Utils.RegisterFunc(L, -1, "allowMSAA", _s_set_allowMSAA);
		Utils.RegisterFunc(L, -1, "allowDynamicResolution", _s_set_allowDynamicResolution);
		Utils.RegisterFunc(L, -1, "forceIntoRenderTexture", _s_set_forceIntoRenderTexture);
		Utils.RegisterFunc(L, -1, "orthographicSize", _s_set_orthographicSize);
		Utils.RegisterFunc(L, -1, "orthographic", _s_set_orthographic);
		Utils.RegisterFunc(L, -1, "opaqueSortMode", _s_set_opaqueSortMode);
		Utils.RegisterFunc(L, -1, "transparencySortMode", _s_set_transparencySortMode);
		Utils.RegisterFunc(L, -1, "transparencySortAxis", _s_set_transparencySortAxis);
		Utils.RegisterFunc(L, -1, "depth", _s_set_depth);
		Utils.RegisterFunc(L, -1, "aspect", _s_set_aspect);
		Utils.RegisterFunc(L, -1, "cullingMask", _s_set_cullingMask);
		Utils.RegisterFunc(L, -1, "eventMask", _s_set_eventMask);
		Utils.RegisterFunc(L, -1, "layerCullSpherical", _s_set_layerCullSpherical);
		Utils.RegisterFunc(L, -1, "cameraType", _s_set_cameraType);
		Utils.RegisterFunc(L, -1, "overrideSceneCullingMask", _s_set_overrideSceneCullingMask);
		Utils.RegisterFunc(L, -1, "layerCullDistances", _s_set_layerCullDistances);
		Utils.RegisterFunc(L, -1, "useOcclusionCulling", _s_set_useOcclusionCulling);
		Utils.RegisterFunc(L, -1, "cullingMatrix", _s_set_cullingMatrix);
		Utils.RegisterFunc(L, -1, "backgroundColor", _s_set_backgroundColor);
		Utils.RegisterFunc(L, -1, "clearFlags", _s_set_clearFlags);
		Utils.RegisterFunc(L, -1, "depthTextureMode", _s_set_depthTextureMode);
		Utils.RegisterFunc(L, -1, "clearStencilAfterLightingPass", _s_set_clearStencilAfterLightingPass);
		Utils.RegisterFunc(L, -1, "usePhysicalProperties", _s_set_usePhysicalProperties);
		Utils.RegisterFunc(L, -1, "sensorSize", _s_set_sensorSize);
		Utils.RegisterFunc(L, -1, "lensShift", _s_set_lensShift);
		Utils.RegisterFunc(L, -1, "focalLength", _s_set_focalLength);
		Utils.RegisterFunc(L, -1, "gateFit", _s_set_gateFit);
		Utils.RegisterFunc(L, -1, "rect", _s_set_rect);
		Utils.RegisterFunc(L, -1, "pixelRect", _s_set_pixelRect);
		Utils.RegisterFunc(L, -1, "targetTexture", _s_set_targetTexture);
		Utils.RegisterFunc(L, -1, "targetDisplay", _s_set_targetDisplay);
		Utils.RegisterFunc(L, -1, "worldToCameraMatrix", _s_set_worldToCameraMatrix);
		Utils.RegisterFunc(L, -1, "projectionMatrix", _s_set_projectionMatrix);
		Utils.RegisterFunc(L, -1, "nonJitteredProjectionMatrix", _s_set_nonJitteredProjectionMatrix);
		Utils.RegisterFunc(L, -1, "useJitteredProjectionMatrixForTransparentRendering", _s_set_useJitteredProjectionMatrixForTransparentRendering);
		Utils.RegisterFunc(L, -1, "scene", _s_set_scene);
		Utils.RegisterFunc(L, -1, "stereoSeparation", _s_set_stereoSeparation);
		Utils.RegisterFunc(L, -1, "stereoConvergence", _s_set_stereoConvergence);
		Utils.RegisterFunc(L, -1, "stereoTargetEye", _s_set_stereoTargetEye);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 8, 7, 3);
		Utils.RegisterFunc(L, -4, "CalculateProjectionMatrixFromPhysicalProperties", _m_CalculateProjectionMatrixFromPhysicalProperties_xlua_st_);
		Utils.RegisterFunc(L, -4, "FocalLengthToFieldOfView", _m_FocalLengthToFieldOfView_xlua_st_);
		Utils.RegisterFunc(L, -4, "FieldOfViewToFocalLength", _m_FieldOfViewToFocalLength_xlua_st_);
		Utils.RegisterFunc(L, -4, "HorizontalToVerticalFieldOfView", _m_HorizontalToVerticalFieldOfView_xlua_st_);
		Utils.RegisterFunc(L, -4, "VerticalToHorizontalFieldOfView", _m_VerticalToHorizontalFieldOfView_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetAllCameras", _m_GetAllCameras_xlua_st_);
		Utils.RegisterFunc(L, -4, "SetupCurrent", _m_SetupCurrent_xlua_st_);
		Utils.RegisterFunc(L, -2, "main", _g_get_main);
		Utils.RegisterFunc(L, -2, "current", _g_get_current);
		Utils.RegisterFunc(L, -2, "allCamerasCount", _g_get_allCamerasCount);
		Utils.RegisterFunc(L, -2, "allCameras", _g_get_allCameras);
		Utils.RegisterFunc(L, -2, "onPreCull", _g_get_onPreCull);
		Utils.RegisterFunc(L, -2, "onPreRender", _g_get_onPreRender);
		Utils.RegisterFunc(L, -2, "onPostRender", _g_get_onPostRender);
		Utils.RegisterFunc(L, -1, "onPreCull", _s_set_onPreCull);
		Utils.RegisterFunc(L, -1, "onPreRender", _s_set_onPreRender);
		Utils.RegisterFunc(L, -1, "onPostRender", _s_set_onPostRender);
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
				Camera o = new Camera();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Reset(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Reset();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetTransparencySortSettings(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetTransparencySortSettings();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetAspect(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetAspect();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetCullingMatrix(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetCullingMatrix();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetReplacementShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			Shader shader = (Shader)objectTranslator.GetObject(L, 2, typeof(Shader));
			string replacementTag = Lua.lua_tostring(L, 3);
			camera.SetReplacementShader(shader, replacementTag);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetReplacementShader(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetReplacementShader();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGateFittedFieldOfView(IntPtr L)
	{
		try
		{
			float gateFittedFieldOfView = ((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).GetGateFittedFieldOfView();
			Lua.lua_pushnumber(L, gateFittedFieldOfView);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGateFittedLensShift(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Vector2 gateFittedLensShift = ((Camera)objectTranslator.FastGetCSObj(L, 1)).GetGateFittedLensShift();
			objectTranslator.PushUnityEngineVector2(L, gateFittedLensShift);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetTargetBuffers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 3 && objectTranslator.Assignable<RenderBuffer>(L, 2) && objectTranslator.Assignable<RenderBuffer>(L, 3))
			{
				objectTranslator.Get(L, 2, out RenderBuffer v);
				objectTranslator.Get(L, 3, out RenderBuffer v2);
				camera.SetTargetBuffers(v, v2);
				return 0;
			}
			if (num == 3 && objectTranslator.Assignable<RenderBuffer[]>(L, 2) && objectTranslator.Assignable<RenderBuffer>(L, 3))
			{
				RenderBuffer[] colorBuffer = (RenderBuffer[])objectTranslator.GetObject(L, 2, typeof(RenderBuffer[]));
				objectTranslator.Get(L, 3, out RenderBuffer v3);
				camera.SetTargetBuffers(colorBuffer, v3);
				return 0;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.SetTargetBuffers!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetWorldToCameraMatrix(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetWorldToCameraMatrix();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetProjectionMatrix(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetProjectionMatrix();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateObliqueMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector4 val);
			Matrix4x4 matrix4x = camera.CalculateObliqueMatrix(val);
			objectTranslator.Push(L, matrix4x);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToScreenPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				Vector3 val2 = camera.WorldToScreenPoint(val);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val4);
				Vector3 val5 = camera.WorldToScreenPoint(val3, val4);
				objectTranslator.PushUnityEngineVector3(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.WorldToScreenPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_WorldToViewportPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				Vector3 val2 = camera.WorldToViewportPoint(val);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val4);
				Vector3 val5 = camera.WorldToViewportPoint(val3, val4);
				objectTranslator.PushUnityEngineVector3(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.WorldToViewportPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ViewportToWorldPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				Vector3 val2 = camera.ViewportToWorldPoint(val);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val4);
				Vector3 val5 = camera.ViewportToWorldPoint(val3, val4);
				objectTranslator.PushUnityEngineVector3(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.ViewportToWorldPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenToWorldPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				Vector3 val2 = camera.ScreenToWorldPoint(val);
				objectTranslator.PushUnityEngineVector3(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val4);
				Vector3 val5 = camera.ScreenToWorldPoint(val3, val4);
				objectTranslator.PushUnityEngineVector3(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.ScreenToWorldPoint!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenToViewportPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = camera.ScreenToViewportPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ViewportToScreenPoint(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			Vector3 val2 = camera.ViewportToScreenPoint(val);
			objectTranslator.PushUnityEngineVector3(L, val2);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ViewportPointToRay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				Ray val2 = camera.ViewportPointToRay(val);
				objectTranslator.PushUnityEngineRay(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val4);
				Ray val5 = camera.ViewportPointToRay(val3, val4);
				objectTranslator.PushUnityEngineRay(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.ViewportPointToRay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ScreenPointToRay(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Vector3>(L, 2))
			{
				objectTranslator.Get(L, 2, out Vector3 val);
				Ray val2 = camera.ScreenPointToRay(val);
				objectTranslator.PushUnityEngineRay(L, val2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Vector3>(L, 2) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 3))
			{
				objectTranslator.Get(L, 2, out Vector3 val3);
				objectTranslator.Get(L, 3, out Camera.MonoOrStereoscopicEye val4);
				Ray val5 = camera.ScreenPointToRay(val3, val4);
				objectTranslator.PushUnityEngineRay(L, val5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.ScreenPointToRay!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateFrustumCorners(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			float z = (float)Lua.lua_tonumber(L, 3);
			objectTranslator.Get(L, 4, out Camera.MonoOrStereoscopicEye val);
			Vector3[] outCorners = (Vector3[])objectTranslator.GetObject(L, 5, typeof(Vector3[]));
			camera.CalculateFrustumCorners(v, z, val, outCorners);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CalculateProjectionMatrixFromPhysicalProperties_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && objectTranslator.Assignable<Camera.GateFitParameters>(L, 6))
			{
				float focalLength = (float)Lua.lua_tonumber(L, 1);
				objectTranslator.Get(L, 2, out Vector2 val);
				objectTranslator.Get(L, 3, out Vector2 val2);
				float nearClip = (float)Lua.lua_tonumber(L, 4);
				float farClip = (float)Lua.lua_tonumber(L, 5);
				objectTranslator.Get(L, 6, out Camera.GateFitParameters v);
				Camera.CalculateProjectionMatrixFromPhysicalProperties(out var output, focalLength, val, val2, nearClip, farClip, v);
				objectTranslator.Push(L, output);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 1) && objectTranslator.Assignable<Vector2>(L, 2) && objectTranslator.Assignable<Vector2>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float focalLength2 = (float)Lua.lua_tonumber(L, 1);
				objectTranslator.Get(L, 2, out Vector2 val3);
				objectTranslator.Get(L, 3, out Vector2 val4);
				float nearClip2 = (float)Lua.lua_tonumber(L, 4);
				float farClip2 = (float)Lua.lua_tonumber(L, 5);
				Camera.CalculateProjectionMatrixFromPhysicalProperties(out var output2, focalLength2, val3, val4, nearClip2, farClip2);
				objectTranslator.Push(L, output2);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.CalculateProjectionMatrixFromPhysicalProperties!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FocalLengthToFieldOfView_xlua_st_(IntPtr L)
	{
		try
		{
			float focalLength = (float)Lua.lua_tonumber(L, 1);
			float sensorSize = (float)Lua.lua_tonumber(L, 2);
			float num = Camera.FocalLengthToFieldOfView(focalLength, sensorSize);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_FieldOfViewToFocalLength_xlua_st_(IntPtr L)
	{
		try
		{
			float fieldOfView = (float)Lua.lua_tonumber(L, 1);
			float sensorSize = (float)Lua.lua_tonumber(L, 2);
			float num = Camera.FieldOfViewToFocalLength(fieldOfView, sensorSize);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_HorizontalToVerticalFieldOfView_xlua_st_(IntPtr L)
	{
		try
		{
			float horizontalFieldOfView = (float)Lua.lua_tonumber(L, 1);
			float aspectRatio = (float)Lua.lua_tonumber(L, 2);
			float num = Camera.HorizontalToVerticalFieldOfView(horizontalFieldOfView, aspectRatio);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_VerticalToHorizontalFieldOfView_xlua_st_(IntPtr L)
	{
		try
		{
			float verticalFieldOfView = (float)Lua.lua_tonumber(L, 1);
			float aspectRatio = (float)Lua.lua_tonumber(L, 2);
			float num = Camera.VerticalToHorizontalFieldOfView(verticalFieldOfView, aspectRatio);
			Lua.lua_pushnumber(L, num);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStereoNonJitteredProjectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.StereoscopicEye val);
			Matrix4x4 stereoNonJitteredProjectionMatrix = camera.GetStereoNonJitteredProjectionMatrix(val);
			objectTranslator.Push(L, stereoNonJitteredProjectionMatrix);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStereoViewMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.StereoscopicEye val);
			Matrix4x4 stereoViewMatrix = camera.GetStereoViewMatrix(val);
			objectTranslator.Push(L, stereoViewMatrix);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyStereoDeviceProjectionMatrixToNonJittered(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.StereoscopicEye val);
			camera.CopyStereoDeviceProjectionMatrixToNonJittered(val);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetStereoProjectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.StereoscopicEye val);
			Matrix4x4 stereoProjectionMatrix = camera.GetStereoProjectionMatrix(val);
			objectTranslator.Push(L, stereoProjectionMatrix);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStereoProjectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.StereoscopicEye val);
			objectTranslator.Get(L, 3, out Matrix4x4 v);
			camera.SetStereoProjectionMatrix(val, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetStereoProjectionMatrices(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetStereoProjectionMatrices();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetStereoViewMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.StereoscopicEye val);
			objectTranslator.Get(L, 3, out Matrix4x4 v);
			camera.SetStereoViewMatrix(val, v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_ResetStereoViewMatrices(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).ResetStereoViewMatrices();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetAllCameras_xlua_st_(IntPtr L)
	{
		try
		{
			int allCameras = Camera.GetAllCameras((Camera[])ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Camera[])));
			Lua.xlua_pushinteger(L, allCameras);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RenderToCubemap(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 2 && objectTranslator.Assignable<Cubemap>(L, 2))
			{
				Cubemap cubemap = (Cubemap)objectTranslator.GetObject(L, 2, typeof(Cubemap));
				bool value = camera.RenderToCubemap(cubemap);
				Lua.lua_pushboolean(L, value);
				return 1;
			}
			if (num == 2 && objectTranslator.Assignable<RenderTexture>(L, 2))
			{
				RenderTexture cubemap2 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				bool value2 = camera.RenderToCubemap(cubemap2);
				Lua.lua_pushboolean(L, value2);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<Cubemap>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				Cubemap cubemap3 = (Cubemap)objectTranslator.GetObject(L, 2, typeof(Cubemap));
				int faceMask = Lua.xlua_tointeger(L, 3);
				bool value3 = camera.RenderToCubemap(cubemap3, faceMask);
				Lua.lua_pushboolean(L, value3);
				return 1;
			}
			if (num == 3 && objectTranslator.Assignable<RenderTexture>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				RenderTexture cubemap4 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				int faceMask2 = Lua.xlua_tointeger(L, 3);
				bool value4 = camera.RenderToCubemap(cubemap4, faceMask2);
				Lua.lua_pushboolean(L, value4);
				return 1;
			}
			if (num == 4 && objectTranslator.Assignable<RenderTexture>(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && objectTranslator.Assignable<Camera.MonoOrStereoscopicEye>(L, 4))
			{
				RenderTexture cubemap5 = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
				int faceMask3 = Lua.xlua_tointeger(L, 3);
				objectTranslator.Get(L, 4, out Camera.MonoOrStereoscopicEye val);
				bool value5 = camera.RenderToCubemap(cubemap5, faceMask3, val);
				Lua.lua_pushboolean(L, value5);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.RenderToCubemap!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_Render(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).Render();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RenderWithShader(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			Shader shader = (Shader)objectTranslator.GetObject(L, 2, typeof(Shader));
			string replacementTag = Lua.lua_tostring(L, 3);
			camera.RenderWithShader(shader, replacementTag);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RenderDontRestore(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RenderDontRestore();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SetupCurrent_xlua_st_(IntPtr L)
	{
		try
		{
			Camera.SetupCurrent((Camera)ObjectTranslatorPool.Instance.Find(L).GetObject(L, 1, typeof(Camera)));
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_CopyFrom(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			Camera other = (Camera)objectTranslator.GetObject(L, 2, typeof(Camera));
			camera.CopyFrom(other);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCommandBuffers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraEvent v);
			camera.RemoveCommandBuffers(v);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveAllCommandBuffers(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).RemoveAllCommandBuffers();
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCommandBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraEvent v);
			CommandBuffer buffer = (CommandBuffer)objectTranslator.GetObject(L, 3, typeof(CommandBuffer));
			camera.AddCommandBuffer(v, buffer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_AddCommandBufferAsync(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraEvent v);
			CommandBuffer buffer = (CommandBuffer)objectTranslator.GetObject(L, 3, typeof(CommandBuffer));
			objectTranslator.Get(L, 4, out ComputeQueueType v2);
			camera.AddCommandBufferAsync(v, buffer, v2);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_RemoveCommandBuffer(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraEvent v);
			CommandBuffer buffer = (CommandBuffer)objectTranslator.GetObject(L, 3, typeof(CommandBuffer));
			camera.RemoveCommandBuffer(v, buffer);
			return 0;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCommandBuffers(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraEvent v);
			CommandBuffer[] commandBuffers = camera.GetCommandBuffers(v);
			objectTranslator.Push(L, commandBuffers);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_TryGetCullingParameters(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			switch (Lua.lua_gettop(L))
			{
			case 1:
			{
				ScriptableCullingParameters cullingParameters2;
				bool value2 = camera.TryGetCullingParameters(out cullingParameters2);
				Lua.lua_pushboolean(L, value2);
				objectTranslator.Push(L, cullingParameters2);
				return 2;
			}
			case 2:
				if (LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 2))
				{
					bool stereoAware = Lua.lua_toboolean(L, 2);
					ScriptableCullingParameters cullingParameters;
					bool value = camera.TryGetCullingParameters(stereoAware, out cullingParameters);
					Lua.lua_pushboolean(L, value);
					objectTranslator.Push(L, cullingParameters);
					return 2;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.TryGetCullingParameters!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOAspect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOAspect(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			TweenerCore<Color, Color, ColorOptions> o = ShortcutExtensions.DOColor(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: val);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFarClipPlane(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOFarClipPlane(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOFieldOfView(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOFieldOfView(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DONearClipPlane(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DONearClipPlane(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOOrthoSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			float endValue = (float)Lua.lua_tonumber(L, 2);
			float duration = (float)Lua.lua_tonumber(L, 3);
			TweenerCore<float, float, FloatOptions> o = target.DOOrthoSize(endValue, duration);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOPixelRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			TweenerCore<Rect, Rect, RectOptions> o = ShortcutExtensions.DOPixelRect(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DORect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			TweenerCore<Rect, Rect, RectOptions> o = ShortcutExtensions.DORect(duration: (float)Lua.lua_tonumber(L, 3), target: target, endValue: v);
			objectTranslator.Push(L, o);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOShakePosition(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration = (float)Lua.lua_tonumber(L, 2);
				float strength = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float randomness = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut = Lua.lua_toboolean(L, 6);
				Tweener o = target.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration2 = (float)Lua.lua_tonumber(L, 2);
				float strength2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float randomness2 = (float)Lua.lua_tonumber(L, 5);
				Tweener o2 = target.DOShakePosition(duration2, strength2, vibrato2, randomness2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration3 = (float)Lua.lua_tonumber(L, 2);
				float strength3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				Tweener o3 = target.DOShakePosition(duration3, strength3, vibrato3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float duration4 = (float)Lua.lua_tonumber(L, 2);
				float strength4 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = target.DOShakePosition(duration4, strength4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float duration5 = (float)Lua.lua_tonumber(L, 2);
				Tweener o5 = target.DOShakePosition(duration5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration6 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				int vibrato4 = Lua.xlua_tointeger(L, 4);
				float randomness3 = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut2 = Lua.lua_toboolean(L, 6);
				Tweener o6 = target.DOShakePosition(duration6, val, vibrato4, randomness3, fadeOut2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				int vibrato5 = Lua.xlua_tointeger(L, 4);
				float randomness4 = (float)Lua.lua_tonumber(L, 5);
				Tweener o7 = target.DOShakePosition(duration7, val2, vibrato5, randomness4);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				int vibrato6 = Lua.xlua_tointeger(L, 4);
				Tweener o8 = target.DOShakePosition(duration8, val3, vibrato6);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				float duration9 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val4);
				Tweener o9 = target.DOShakePosition(duration9, val4);
				objectTranslator.Push(L, o9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.DOShakePosition!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_DOShakeRotation(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera target = (Camera)objectTranslator.FastGetCSObj(L, 1);
			int num = Lua.lua_gettop(L);
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration = (float)Lua.lua_tonumber(L, 2);
				float strength = (float)Lua.lua_tonumber(L, 3);
				int vibrato = Lua.xlua_tointeger(L, 4);
				float randomness = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut = Lua.lua_toboolean(L, 6);
				Tweener o = target.DOShakeRotation(duration, strength, vibrato, randomness, fadeOut);
				objectTranslator.Push(L, o);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration2 = (float)Lua.lua_tonumber(L, 2);
				float strength2 = (float)Lua.lua_tonumber(L, 3);
				int vibrato2 = Lua.xlua_tointeger(L, 4);
				float randomness2 = (float)Lua.lua_tonumber(L, 5);
				Tweener o2 = target.DOShakeRotation(duration2, strength2, vibrato2, randomness2);
				objectTranslator.Push(L, o2);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration3 = (float)Lua.lua_tonumber(L, 2);
				float strength3 = (float)Lua.lua_tonumber(L, 3);
				int vibrato3 = Lua.xlua_tointeger(L, 4);
				Tweener o3 = target.DOShakeRotation(duration3, strength3, vibrato3);
				objectTranslator.Push(L, o3);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 3))
			{
				float duration4 = (float)Lua.lua_tonumber(L, 2);
				float strength4 = (float)Lua.lua_tonumber(L, 3);
				Tweener o4 = target.DOShakeRotation(duration4, strength4);
				objectTranslator.Push(L, o4);
				return 1;
			}
			if (num == 2 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2))
			{
				float duration5 = (float)Lua.lua_tonumber(L, 2);
				Tweener o5 = target.DOShakeRotation(duration5);
				objectTranslator.Push(L, o5);
				return 1;
			}
			if (num == 6 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5) && LuaTypes.LUA_TBOOLEAN == Lua.lua_type(L, 6))
			{
				float duration6 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val);
				int vibrato4 = Lua.xlua_tointeger(L, 4);
				float randomness3 = (float)Lua.lua_tonumber(L, 5);
				bool fadeOut2 = Lua.lua_toboolean(L, 6);
				Tweener o6 = target.DOShakeRotation(duration6, val, vibrato4, randomness3, fadeOut2);
				objectTranslator.Push(L, o6);
				return 1;
			}
			if (num == 5 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 5))
			{
				float duration7 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val2);
				int vibrato5 = Lua.xlua_tointeger(L, 4);
				float randomness4 = (float)Lua.lua_tonumber(L, 5);
				Tweener o7 = target.DOShakeRotation(duration7, val2, vibrato5, randomness4);
				objectTranslator.Push(L, o7);
				return 1;
			}
			if (num == 4 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3) && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 4))
			{
				float duration8 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val3);
				int vibrato6 = Lua.xlua_tointeger(L, 4);
				Tweener o8 = target.DOShakeRotation(duration8, val3, vibrato6);
				objectTranslator.Push(L, o8);
				return 1;
			}
			if (num == 3 && LuaTypes.LUA_TNUMBER == Lua.lua_type(L, 2) && objectTranslator.Assignable<Vector3>(L, 3))
			{
				float duration9 = (float)Lua.lua_tonumber(L, 2);
				objectTranslator.Get(L, 3, out Vector3 val4);
				Tweener o9 = target.DOShakeRotation(duration9, val4);
				objectTranslator.Push(L, o9);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.Camera.DOShakeRotation!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nearClipPlane(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.nearClipPlane);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_farClipPlane(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.farClipPlane);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_fieldOfView(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.fieldOfView);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderingPath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.renderingPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_actualRenderingPath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.actualRenderingPath);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowHDR(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.allowHDR);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowMSAA(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.allowMSAA);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allowDynamicResolution(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.allowDynamicResolution);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_forceIntoRenderTexture(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.forceIntoRenderTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orthographicSize(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.orthographicSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_orthographic(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.orthographic);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_opaqueSortMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.opaqueSortMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transparencySortMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.transparencySortMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_transparencySortAxis(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, camera.transparencySortAxis);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_depth(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.depth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_aspect(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.aspect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_velocity(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector3(L, camera.velocity);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cullingMask(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.cullingMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_eventMask(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.eventMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layerCullSpherical(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.layerCullSpherical);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.cameraType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_overrideSceneCullingMask(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushuint64(L, camera.overrideSceneCullingMask);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_layerCullDistances(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.layerCullDistances);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useOcclusionCulling(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.useOcclusionCulling);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cullingMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.cullingMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_backgroundColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineColor(L, camera.backgroundColor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.clearFlags);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_depthTextureMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.depthTextureMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_clearStencilAfterLightingPass(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.clearStencilAfterLightingPass);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_usePhysicalProperties(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.usePhysicalProperties);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_sensorSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, camera.sensorSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_lensShift(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineVector2(L, camera.lensShift);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_focalLength(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.focalLength);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_gateFit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineCameraGateFitMode(L, camera.gateFit);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_rect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.rect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.pixelRect);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelWidth(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.pixelWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_pixelHeight(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.pixelHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scaledPixelWidth(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.scaledPixelWidth);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scaledPixelHeight(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.scaledPixelHeight);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.targetTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_activeTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.activeTexture);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_targetDisplay(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.targetDisplay);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_cameraToWorldMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.cameraToWorldMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_worldToCameraMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.worldToCameraMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_projectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.projectionMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_nonJitteredProjectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.nonJitteredProjectionMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_useJitteredProjectionMatrixForTransparentRendering(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.useJitteredProjectionMatrixForTransparentRendering);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_previousViewProjectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.previousViewProjectionMatrix);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_main(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Camera.main);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_current(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Camera.current);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_scene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.scene);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stereoEnabled(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.stereoEnabled);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stereoSeparation(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.stereoSeparation);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stereoConvergence(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushnumber(L, camera.stereoConvergence);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_areVRStereoViewMatricesWithinSingleCullTolerance(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.lua_pushboolean(L, camera.areVRStereoViewMatricesWithinSingleCullTolerance);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stereoTargetEye(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Push(L, camera.stereoTargetEye);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_stereoActiveEye(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.PushUnityEngineCameraMonoOrStereoscopicEye(L, camera.stereoActiveEye);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allCamerasCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, Camera.allCamerasCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_allCameras(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Camera.allCameras);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_commandBufferCount(IntPtr L)
	{
		try
		{
			Camera camera = (Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1);
			Lua.xlua_pushinteger(L, camera.commandBufferCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPreCull(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Camera.onPreCull);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPreRender(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Camera.onPreRender);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_onPostRender(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, Camera.onPostRender);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nearClipPlane(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).nearClipPlane = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_farClipPlane(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).farClipPlane = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_fieldOfView(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).fieldOfView = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_renderingPath(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out RenderingPath v);
			camera.renderingPath = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowHDR(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowHDR = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowMSAA(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowMSAA = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_allowDynamicResolution(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).allowDynamicResolution = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_forceIntoRenderTexture(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).forceIntoRenderTexture = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orthographicSize(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).orthographicSize = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_orthographic(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).orthographic = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_opaqueSortMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out OpaqueSortMode v);
			camera.opaqueSortMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_transparencySortMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out TransparencySortMode v);
			camera.transparencySortMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_transparencySortAxis(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector3 val);
			camera.transparencySortAxis = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_depth(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).depth = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_aspect(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).aspect = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cullingMask(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).cullingMask = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_eventMask(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).eventMask = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_layerCullSpherical(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).layerCullSpherical = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cameraType(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraType v);
			camera.cameraType = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_overrideSceneCullingMask(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).overrideSceneCullingMask = Lua.lua_touint64(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_layerCullDistances(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Camera)objectTranslator.FastGetCSObj(L, 1)).layerCullDistances = (float[])objectTranslator.GetObject(L, 2, typeof(float[]));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useOcclusionCulling(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useOcclusionCulling = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_cullingMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Matrix4x4 v);
			camera.cullingMatrix = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_backgroundColor(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Color val);
			camera.backgroundColor = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearFlags(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out CameraClearFlags v);
			camera.clearFlags = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_depthTextureMode(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out DepthTextureMode v);
			camera.depthTextureMode = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_clearStencilAfterLightingPass(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).clearStencilAfterLightingPass = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_usePhysicalProperties(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).usePhysicalProperties = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_sensorSize(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			camera.sensorSize = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_lensShift(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Vector2 val);
			camera.lensShift = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_focalLength(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).focalLength = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_gateFit(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Camera.GateFitMode val);
			camera.gateFit = val;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_rect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			camera.rect = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_pixelRect(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Rect v);
			camera.pixelRect = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetTexture(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			((Camera)objectTranslator.FastGetCSObj(L, 1)).targetTexture = (RenderTexture)objectTranslator.GetObject(L, 2, typeof(RenderTexture));
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_targetDisplay(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).targetDisplay = Lua.xlua_tointeger(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_worldToCameraMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Matrix4x4 v);
			camera.worldToCameraMatrix = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_projectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Matrix4x4 v);
			camera.projectionMatrix = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_nonJitteredProjectionMatrix(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Matrix4x4 v);
			camera.nonJitteredProjectionMatrix = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_useJitteredProjectionMatrixForTransparentRendering(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).useJitteredProjectionMatrixForTransparentRendering = Lua.lua_toboolean(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_scene(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out Scene v);
			camera.scene = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stereoSeparation(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stereoSeparation = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stereoConvergence(IntPtr L)
	{
		try
		{
			((Camera)ObjectTranslatorPool.Instance.Find(L).FastGetCSObj(L, 1)).stereoConvergence = (float)Lua.lua_tonumber(L, 2);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_stereoTargetEye(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			Camera camera = (Camera)objectTranslator.FastGetCSObj(L, 1);
			objectTranslator.Get(L, 2, out StereoTargetEyeMask v);
			camera.stereoTargetEye = v;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPreCull(IntPtr L)
	{
		try
		{
			Camera.onPreCull = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Camera.CameraCallback>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPreRender(IntPtr L)
	{
		try
		{
			Camera.onPreRender = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Camera.CameraCallback>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _s_set_onPostRender(IntPtr L)
	{
		try
		{
			Camera.onPostRender = ObjectTranslatorPool.Instance.Find(L).GetDelegate<Camera.CameraCallback>(L, 1);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 0;
	}
}
