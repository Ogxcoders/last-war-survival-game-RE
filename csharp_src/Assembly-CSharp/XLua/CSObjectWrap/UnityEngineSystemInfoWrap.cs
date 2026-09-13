using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using XLua.LuaDLL;

namespace XLua.CSObjectWrap;

public class UnityEngineSystemInfoWrap
{
	public static void __Register(IntPtr L)
	{
		ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
		Type typeFromHandle = typeof(SystemInfo);
		Utils.BeginObjectRegister(typeFromHandle, L, translator, 0, 0, 0, 0);
		Utils.EndObjectRegister(typeFromHandle, L, translator, null, null, null, null, null);
		Utils.BeginClassRegister(typeFromHandle, L, __CreateInstance, 9, 74, 0);
		Utils.RegisterFunc(L, -4, "SupportsRenderTextureFormat", _m_SupportsRenderTextureFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "SupportsBlendingOnRenderTextureFormat", _m_SupportsBlendingOnRenderTextureFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "SupportsTextureFormat", _m_SupportsTextureFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "SupportsVertexAttributeFormat", _m_SupportsVertexAttributeFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "IsFormatSupported", _m_IsFormatSupported_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetCompatibleFormat", _m_GetCompatibleFormat_xlua_st_);
		Utils.RegisterFunc(L, -4, "GetGraphicsFormat", _m_GetGraphicsFormat_xlua_st_);
		Utils.RegisterObject(L, translator, -4, "unsupportedIdentifier", "n/a");
		Utils.RegisterFunc(L, -2, "batteryLevel", _g_get_batteryLevel);
		Utils.RegisterFunc(L, -2, "batteryStatus", _g_get_batteryStatus);
		Utils.RegisterFunc(L, -2, "operatingSystem", _g_get_operatingSystem);
		Utils.RegisterFunc(L, -2, "operatingSystemFamily", _g_get_operatingSystemFamily);
		Utils.RegisterFunc(L, -2, "processorType", _g_get_processorType);
		Utils.RegisterFunc(L, -2, "processorFrequency", _g_get_processorFrequency);
		Utils.RegisterFunc(L, -2, "processorCount", _g_get_processorCount);
		Utils.RegisterFunc(L, -2, "systemMemorySize", _g_get_systemMemorySize);
		Utils.RegisterFunc(L, -2, "deviceUniqueIdentifier", _g_get_deviceUniqueIdentifier);
		Utils.RegisterFunc(L, -2, "deviceName", _g_get_deviceName);
		Utils.RegisterFunc(L, -2, "deviceModel", _g_get_deviceModel);
		Utils.RegisterFunc(L, -2, "supportsAccelerometer", _g_get_supportsAccelerometer);
		Utils.RegisterFunc(L, -2, "supportsGyroscope", _g_get_supportsGyroscope);
		Utils.RegisterFunc(L, -2, "supportsVibration", _g_get_supportsVibration);
		Utils.RegisterFunc(L, -2, "supportsAudio", _g_get_supportsAudio);
		Utils.RegisterFunc(L, -2, "deviceType", _g_get_deviceType);
		Utils.RegisterFunc(L, -2, "graphicsMemorySize", _g_get_graphicsMemorySize);
		Utils.RegisterFunc(L, -2, "graphicsDeviceName", _g_get_graphicsDeviceName);
		Utils.RegisterFunc(L, -2, "graphicsDeviceVendor", _g_get_graphicsDeviceVendor);
		Utils.RegisterFunc(L, -2, "graphicsDeviceID", _g_get_graphicsDeviceID);
		Utils.RegisterFunc(L, -2, "graphicsDeviceVendorID", _g_get_graphicsDeviceVendorID);
		Utils.RegisterFunc(L, -2, "graphicsDeviceType", _g_get_graphicsDeviceType);
		Utils.RegisterFunc(L, -2, "graphicsUVStartsAtTop", _g_get_graphicsUVStartsAtTop);
		Utils.RegisterFunc(L, -2, "graphicsDeviceVersion", _g_get_graphicsDeviceVersion);
		Utils.RegisterFunc(L, -2, "graphicsShaderLevel", _g_get_graphicsShaderLevel);
		Utils.RegisterFunc(L, -2, "graphicsMultiThreaded", _g_get_graphicsMultiThreaded);
		Utils.RegisterFunc(L, -2, "renderingThreadingMode", _g_get_renderingThreadingMode);
		Utils.RegisterFunc(L, -2, "hasHiddenSurfaceRemovalOnGPU", _g_get_hasHiddenSurfaceRemovalOnGPU);
		Utils.RegisterFunc(L, -2, "hasDynamicUniformArrayIndexingInFragmentShaders", _g_get_hasDynamicUniformArrayIndexingInFragmentShaders);
		Utils.RegisterFunc(L, -2, "supportsShadows", _g_get_supportsShadows);
		Utils.RegisterFunc(L, -2, "supportsRawShadowDepthSampling", _g_get_supportsRawShadowDepthSampling);
		Utils.RegisterFunc(L, -2, "supportsMotionVectors", _g_get_supportsMotionVectors);
		Utils.RegisterFunc(L, -2, "supports3DTextures", _g_get_supports3DTextures);
		Utils.RegisterFunc(L, -2, "supports2DArrayTextures", _g_get_supports2DArrayTextures);
		Utils.RegisterFunc(L, -2, "supports3DRenderTextures", _g_get_supports3DRenderTextures);
		Utils.RegisterFunc(L, -2, "supportsCubemapArrayTextures", _g_get_supportsCubemapArrayTextures);
		Utils.RegisterFunc(L, -2, "copyTextureSupport", _g_get_copyTextureSupport);
		Utils.RegisterFunc(L, -2, "supportsComputeShaders", _g_get_supportsComputeShaders);
		Utils.RegisterFunc(L, -2, "supportsGeometryShaders", _g_get_supportsGeometryShaders);
		Utils.RegisterFunc(L, -2, "supportsTessellationShaders", _g_get_supportsTessellationShaders);
		Utils.RegisterFunc(L, -2, "supportsInstancing", _g_get_supportsInstancing);
		Utils.RegisterFunc(L, -2, "supportsHardwareQuadTopology", _g_get_supportsHardwareQuadTopology);
		Utils.RegisterFunc(L, -2, "supports32bitsIndexBuffer", _g_get_supports32bitsIndexBuffer);
		Utils.RegisterFunc(L, -2, "supportsSparseTextures", _g_get_supportsSparseTextures);
		Utils.RegisterFunc(L, -2, "supportedRenderTargetCount", _g_get_supportedRenderTargetCount);
		Utils.RegisterFunc(L, -2, "supportsSeparatedRenderTargetsBlend", _g_get_supportsSeparatedRenderTargetsBlend);
		Utils.RegisterFunc(L, -2, "supportedRandomWriteTargetCount", _g_get_supportedRandomWriteTargetCount);
		Utils.RegisterFunc(L, -2, "supportsMultisampledTextures", _g_get_supportsMultisampledTextures);
		Utils.RegisterFunc(L, -2, "supportsMultisampleAutoResolve", _g_get_supportsMultisampleAutoResolve);
		Utils.RegisterFunc(L, -2, "supportsTextureWrapMirrorOnce", _g_get_supportsTextureWrapMirrorOnce);
		Utils.RegisterFunc(L, -2, "usesReversedZBuffer", _g_get_usesReversedZBuffer);
		Utils.RegisterFunc(L, -2, "npotSupport", _g_get_npotSupport);
		Utils.RegisterFunc(L, -2, "maxTextureSize", _g_get_maxTextureSize);
		Utils.RegisterFunc(L, -2, "maxCubemapSize", _g_get_maxCubemapSize);
		Utils.RegisterFunc(L, -2, "maxComputeBufferInputsVertex", _g_get_maxComputeBufferInputsVertex);
		Utils.RegisterFunc(L, -2, "maxComputeBufferInputsFragment", _g_get_maxComputeBufferInputsFragment);
		Utils.RegisterFunc(L, -2, "maxComputeBufferInputsGeometry", _g_get_maxComputeBufferInputsGeometry);
		Utils.RegisterFunc(L, -2, "maxComputeBufferInputsDomain", _g_get_maxComputeBufferInputsDomain);
		Utils.RegisterFunc(L, -2, "maxComputeBufferInputsHull", _g_get_maxComputeBufferInputsHull);
		Utils.RegisterFunc(L, -2, "maxComputeBufferInputsCompute", _g_get_maxComputeBufferInputsCompute);
		Utils.RegisterFunc(L, -2, "maxComputeWorkGroupSize", _g_get_maxComputeWorkGroupSize);
		Utils.RegisterFunc(L, -2, "maxComputeWorkGroupSizeX", _g_get_maxComputeWorkGroupSizeX);
		Utils.RegisterFunc(L, -2, "maxComputeWorkGroupSizeY", _g_get_maxComputeWorkGroupSizeY);
		Utils.RegisterFunc(L, -2, "maxComputeWorkGroupSizeZ", _g_get_maxComputeWorkGroupSizeZ);
		Utils.RegisterFunc(L, -2, "supportsAsyncCompute", _g_get_supportsAsyncCompute);
		Utils.RegisterFunc(L, -2, "supportsGraphicsFence", _g_get_supportsGraphicsFence);
		Utils.RegisterFunc(L, -2, "supportsAsyncGPUReadback", _g_get_supportsAsyncGPUReadback);
		Utils.RegisterFunc(L, -2, "supportsRayTracing", _g_get_supportsRayTracing);
		Utils.RegisterFunc(L, -2, "supportsSetConstantBuffer", _g_get_supportsSetConstantBuffer);
		Utils.RegisterFunc(L, -2, "minConstantBufferOffsetAlignment", _g_get_minConstantBufferOffsetAlignment);
		Utils.RegisterFunc(L, -2, "hasMipMaxLevel", _g_get_hasMipMaxLevel);
		Utils.RegisterFunc(L, -2, "supportsMipStreaming", _g_get_supportsMipStreaming);
		Utils.RegisterFunc(L, -2, "usesLoadStoreActions", _g_get_usesLoadStoreActions);
		Utils.RegisterFunc(L, -2, "supportsStoreAndResolveAction", _g_get_supportsStoreAndResolveAction);
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
				SystemInfo o = new SystemInfo();
				objectTranslator.Push(L, o);
				return 1;
			}
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return Lua.luaL_error(L, "invalid arguments to UnityEngine.SystemInfo constructor!");
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportsRenderTextureFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out RenderTextureFormat val);
			bool value = SystemInfo.SupportsRenderTextureFormat(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportsBlendingOnRenderTextureFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out RenderTextureFormat val);
			bool value = SystemInfo.SupportsBlendingOnRenderTextureFormat(val);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportsTextureFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out TextureFormat v);
			bool value = SystemInfo.SupportsTextureFormat(v);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_SupportsVertexAttributeFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Get(L, 1, out VertexAttributeFormat v);
			int dimension = Lua.xlua_tointeger(L, 2);
			bool value = SystemInfo.SupportsVertexAttributeFormat(v, dimension);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_IsFormatSupported_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out GraphicsFormat v);
			objectTranslator.Get(L, 2, out FormatUsage v2);
			bool value = SystemInfo.IsFormatSupported(v, v2);
			Lua.lua_pushboolean(L, value);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetCompatibleFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out GraphicsFormat v);
			objectTranslator.Get(L, 2, out FormatUsage v2);
			GraphicsFormat compatibleFormat = SystemInfo.GetCompatibleFormat(v, v2);
			objectTranslator.Push(L, compatibleFormat);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _m_GetGraphicsFormat_xlua_st_(IntPtr L)
	{
		try
		{
			ObjectTranslator objectTranslator = ObjectTranslatorPool.Instance.Find(L);
			objectTranslator.Get(L, 1, out DefaultFormat v);
			GraphicsFormat graphicsFormat = SystemInfo.GetGraphicsFormat(v);
			objectTranslator.Push(L, graphicsFormat);
			return 1;
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_batteryLevel(IntPtr L)
	{
		try
		{
			Lua.lua_pushnumber(L, SystemInfo.batteryLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_batteryStatus(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.batteryStatus);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_operatingSystem(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.operatingSystem);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_operatingSystemFamily(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.operatingSystemFamily);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_processorType(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.processorType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_processorFrequency(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.processorFrequency);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_processorCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.processorCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_systemMemorySize(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.systemMemorySize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deviceUniqueIdentifier(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.deviceUniqueIdentifier);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deviceName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.deviceName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deviceModel(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.deviceModel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsAccelerometer(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsAccelerometer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsGyroscope(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsGyroscope);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsVibration(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsVibration);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsAudio(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsAudio);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_deviceType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.deviceType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsMemorySize(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.graphicsMemorySize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsDeviceName(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.graphicsDeviceName);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsDeviceVendor(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.graphicsDeviceVendor);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsDeviceID(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.graphicsDeviceID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsDeviceVendorID(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.graphicsDeviceVendorID);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsDeviceType(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.graphicsDeviceType);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsUVStartsAtTop(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.graphicsUVStartsAtTop);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsDeviceVersion(IntPtr L)
	{
		try
		{
			Lua.lua_pushstring(L, SystemInfo.graphicsDeviceVersion);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsShaderLevel(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.graphicsShaderLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_graphicsMultiThreaded(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.graphicsMultiThreaded);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_renderingThreadingMode(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.renderingThreadingMode);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasHiddenSurfaceRemovalOnGPU(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.hasHiddenSurfaceRemovalOnGPU);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasDynamicUniformArrayIndexingInFragmentShaders(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.hasDynamicUniformArrayIndexingInFragmentShaders);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsShadows(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsShadows);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsRawShadowDepthSampling(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsRawShadowDepthSampling);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsMotionVectors(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsMotionVectors);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supports3DTextures(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supports3DTextures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supports2DArrayTextures(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supports2DArrayTextures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supports3DRenderTextures(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supports3DRenderTextures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsCubemapArrayTextures(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsCubemapArrayTextures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_copyTextureSupport(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.copyTextureSupport);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsComputeShaders(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsComputeShaders);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsGeometryShaders(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsGeometryShaders);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsTessellationShaders(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsTessellationShaders);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsInstancing(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsInstancing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsHardwareQuadTopology(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsHardwareQuadTopology);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supports32bitsIndexBuffer(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supports32bitsIndexBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsSparseTextures(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsSparseTextures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportedRenderTargetCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.supportedRenderTargetCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsSeparatedRenderTargetsBlend(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsSeparatedRenderTargetsBlend);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportedRandomWriteTargetCount(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.supportedRandomWriteTargetCount);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsMultisampledTextures(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.supportsMultisampledTextures);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsMultisampleAutoResolve(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsMultisampleAutoResolve);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsTextureWrapMirrorOnce(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.supportsTextureWrapMirrorOnce);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_usesReversedZBuffer(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.usesReversedZBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_npotSupport(IntPtr L)
	{
		try
		{
			ObjectTranslatorPool.Instance.Find(L).Push(L, SystemInfo.npotSupport);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxTextureSize(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxTextureSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxCubemapSize(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxCubemapSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeBufferInputsVertex(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeBufferInputsVertex);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeBufferInputsFragment(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeBufferInputsFragment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeBufferInputsGeometry(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeBufferInputsGeometry);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeBufferInputsDomain(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeBufferInputsDomain);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeBufferInputsHull(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeBufferInputsHull);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeBufferInputsCompute(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeBufferInputsCompute);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeWorkGroupSize(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeWorkGroupSize);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeWorkGroupSizeX(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeWorkGroupSizeX);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeWorkGroupSizeY(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeWorkGroupSizeY);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_maxComputeWorkGroupSizeZ(IntPtr L)
	{
		try
		{
			Lua.xlua_pushinteger(L, SystemInfo.maxComputeWorkGroupSizeZ);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsAsyncCompute(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsAsyncCompute);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsGraphicsFence(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsGraphicsFence);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsAsyncGPUReadback(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsAsyncGPUReadback);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsRayTracing(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsRayTracing);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsSetConstantBuffer(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsSetConstantBuffer);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_minConstantBufferOffsetAlignment(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.minConstantBufferOffsetAlignment);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_hasMipMaxLevel(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.hasMipMaxLevel);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsMipStreaming(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsMipStreaming);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_usesLoadStoreActions(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.usesLoadStoreActions);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}

	[MonoPInvokeCallback(typeof(lua_CSFunction))]
	private static int _g_get_supportsStoreAndResolveAction(IntPtr L)
	{
		try
		{
			Lua.lua_pushboolean(L, SystemInfo.supportsStoreAndResolveAction);
		}
		catch (Exception ex)
		{
			return Lua.luaL_error(L, "c# exception:" + ex);
		}
		return 1;
	}
}
