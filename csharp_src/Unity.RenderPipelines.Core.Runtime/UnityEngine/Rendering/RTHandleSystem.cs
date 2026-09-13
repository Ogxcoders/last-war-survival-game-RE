using System;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering;

public class RTHandleSystem : IDisposable
{
	internal enum ResizeMode
	{
		Auto,
		OnDemand
	}

	private bool m_HardwareDynamicResRequested;

	private bool m_ScaledRTSupportsMSAA;

	private MSAASamples m_ScaledRTCurrentMSAASamples = MSAASamples.None;

	private HashSet<RTHandle> m_AutoSizedRTs;

	private RTHandle[] m_AutoSizedRTsArray;

	private HashSet<RTHandle> m_ResizeOnDemandRTs;

	private RTHandleProperties m_RTHandleProperties;

	private int m_MaxWidths;

	private int m_MaxHeights;

	public RTHandleProperties rtHandleProperties => m_RTHandleProperties;

	public RTHandleSystem()
	{
		m_AutoSizedRTs = new HashSet<RTHandle>();
		m_ResizeOnDemandRTs = new HashSet<RTHandle>();
		m_MaxWidths = 1;
		m_MaxHeights = 1;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Initialize(int width, int height, bool scaledRTsupportsMSAA, MSAASamples scaledRTMSAASamples)
	{
		m_MaxWidths = width;
		m_MaxHeights = height;
		m_ScaledRTSupportsMSAA = scaledRTsupportsMSAA;
		m_ScaledRTCurrentMSAASamples = scaledRTMSAASamples;
		m_HardwareDynamicResRequested = DynamicResolutionHandler.instance.RequestsHardwareDynamicResolution();
	}

	public void Release(RTHandle rth)
	{
		rth?.Release();
	}

	internal void Remove(RTHandle rth)
	{
		m_AutoSizedRTs.Remove(rth);
	}

	public void SetReferenceSize(int width, int height, MSAASamples msaaSamples)
	{
		m_RTHandleProperties.previousViewportSize = m_RTHandleProperties.currentViewportSize;
		m_RTHandleProperties.previousRenderTargetSize = m_RTHandleProperties.currentRenderTargetSize;
		Vector2 vector = new Vector2(GetMaxWidth(), GetMaxHeight());
		width = Mathf.Max(width, 1);
		height = Mathf.Max(height, 1);
		bool flag = width > GetMaxWidth() || height > GetMaxHeight();
		bool flag2 = msaaSamples != m_ScaledRTCurrentMSAASamples;
		if (flag || flag2)
		{
			Resize(width, height, msaaSamples, flag, flag2);
		}
		m_RTHandleProperties.currentViewportSize = new Vector2Int(width, height);
		m_RTHandleProperties.currentRenderTargetSize = new Vector2Int(GetMaxWidth(), GetMaxHeight());
		if (m_RTHandleProperties.previousViewportSize.x == 0)
		{
			m_RTHandleProperties.previousViewportSize = m_RTHandleProperties.currentViewportSize;
			m_RTHandleProperties.previousRenderTargetSize = m_RTHandleProperties.currentRenderTargetSize;
			vector = new Vector2(GetMaxWidth(), GetMaxHeight());
		}
		if (DynamicResolutionHandler.instance.HardwareDynamicResIsEnabled() && m_HardwareDynamicResRequested)
		{
			m_RTHandleProperties.rtHandleScale = new Vector4(1f, 1f, 1f, 1f);
			return;
		}
		Vector2 vector2 = new Vector2(GetMaxWidth(), GetMaxHeight());
		Vector2 vector3 = m_RTHandleProperties.currentViewportSize / vector2;
		Vector2 vector4 = m_RTHandleProperties.previousViewportSize / vector;
		m_RTHandleProperties.rtHandleScale = new Vector4(vector3.x, vector3.y, vector4.x, vector4.y);
	}

	public void SetHardwareDynamicResolutionState(bool enableHWDynamicRes)
	{
		if (enableHWDynamicRes == m_HardwareDynamicResRequested || m_AutoSizedRTsArray == null)
		{
			return;
		}
		m_HardwareDynamicResRequested = enableHWDynamicRes;
		int i = 0;
		for (int num = m_AutoSizedRTsArray.Length; i < num; i++)
		{
			RTHandle rTHandle = m_AutoSizedRTsArray[i];
			RenderTexture rT = rTHandle.m_RT;
			if ((bool)rT)
			{
				rT.Release();
				rT.useDynamicScale = m_HardwareDynamicResRequested && rTHandle.m_EnableHWDynamicScale;
				rT.Create();
			}
		}
	}

	internal void SwitchResizeMode(RTHandle rth, ResizeMode mode)
	{
		if (!rth.useScaling)
		{
			return;
		}
		switch (mode)
		{
		case ResizeMode.OnDemand:
			m_AutoSizedRTs.Remove(rth);
			m_ResizeOnDemandRTs.Add(rth);
			break;
		case ResizeMode.Auto:
			if (m_ResizeOnDemandRTs.Contains(rth))
			{
				DemandResize(rth);
			}
			m_ResizeOnDemandRTs.Remove(rth);
			m_AutoSizedRTs.Add(rth);
			break;
		}
	}

	private void DemandResize(RTHandle rth)
	{
		RenderTexture rT = rth.m_RT;
		rth.referenceSize = new Vector2Int(m_MaxWidths, m_MaxHeights);
		Vector2Int scaledSize = rth.GetScaledSize(rth.referenceSize);
		scaledSize = Vector2Int.Max(Vector2Int.one, scaledSize);
		bool num = rT.width != scaledSize.x || rT.height != scaledSize.y;
		bool flag = rth.m_EnableMSAA && rT.antiAliasing != (int)m_ScaledRTCurrentMSAASamples;
		if (num || flag)
		{
			rT.Release();
			if (rth.m_EnableMSAA)
			{
				rT.antiAliasing = (int)m_ScaledRTCurrentMSAASamples;
			}
			rT.width = scaledSize.x;
			rT.height = scaledSize.y;
			rT.name = CoreUtils.GetRenderTargetAutoName(rT.width, rT.height, rT.volumeDepth, rT.format, rth.m_Name, rT.useMipMap, rth.m_EnableMSAA, m_ScaledRTCurrentMSAASamples);
			rT.Create();
		}
	}

	public int GetMaxWidth()
	{
		return m_MaxWidths;
	}

	public int GetMaxHeight()
	{
		return m_MaxHeights;
	}

	private void Dispose(bool disposing)
	{
		if (disposing)
		{
			Array.Resize(ref m_AutoSizedRTsArray, m_AutoSizedRTs.Count);
			m_AutoSizedRTs.CopyTo(m_AutoSizedRTsArray);
			int i = 0;
			for (int num = m_AutoSizedRTsArray.Length; i < num; i++)
			{
				RTHandle rth = m_AutoSizedRTsArray[i];
				Release(rth);
			}
			m_AutoSizedRTs.Clear();
			Array.Resize(ref m_AutoSizedRTsArray, m_ResizeOnDemandRTs.Count);
			m_ResizeOnDemandRTs.CopyTo(m_AutoSizedRTsArray);
			int j = 0;
			for (int num2 = m_AutoSizedRTsArray.Length; j < num2; j++)
			{
				RTHandle rth2 = m_AutoSizedRTsArray[j];
				Release(rth2);
			}
			m_ResizeOnDemandRTs.Clear();
			m_AutoSizedRTsArray = null;
		}
	}

	private void Resize(int width, int height, MSAASamples msaaSamples, bool sizeChanged, bool msaaSampleChanged)
	{
		m_MaxWidths = Math.Max(width, m_MaxWidths);
		m_MaxHeights = Math.Max(height, m_MaxHeights);
		m_ScaledRTCurrentMSAASamples = msaaSamples;
		Vector2Int vector2Int = new Vector2Int(m_MaxWidths, m_MaxHeights);
		Array.Resize(ref m_AutoSizedRTsArray, m_AutoSizedRTs.Count);
		m_AutoSizedRTs.CopyTo(m_AutoSizedRTsArray);
		int i = 0;
		for (int num = m_AutoSizedRTsArray.Length; i < num; i++)
		{
			RTHandle rTHandle = m_AutoSizedRTsArray[i];
			if (!(!sizeChanged && msaaSampleChanged) || rTHandle.m_EnableMSAA)
			{
				rTHandle.referenceSize = vector2Int;
				RenderTexture rT = rTHandle.m_RT;
				rT.Release();
				Vector2Int scaledSize = rTHandle.GetScaledSize(vector2Int);
				rT.width = Mathf.Max(scaledSize.x, 1);
				rT.height = Mathf.Max(scaledSize.y, 1);
				if (rTHandle.m_EnableMSAA)
				{
					rT.antiAliasing = (int)m_ScaledRTCurrentMSAASamples;
				}
				rT.name = CoreUtils.GetRenderTargetAutoName(rT.width, rT.height, rT.volumeDepth, rT.format, rTHandle.m_Name, rT.useMipMap, rTHandle.m_EnableMSAA, m_ScaledRTCurrentMSAASamples);
				rT.Create();
			}
		}
	}

	public RTHandle Alloc(int width, int height, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, MSAASamples msaaSamples = MSAASamples.None, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, string name = "")
	{
		bool flag = msaaSamples != MSAASamples.None;
		if (!flag && bindTextureMS)
		{
			Debug.LogWarning("RTHandle allocated without MSAA but with bindMS set to true, forcing bindMS to false.");
			bindTextureMS = false;
		}
		RenderTexture renderTexture;
		if (isShadowMap || depthBufferBits != DepthBits.None)
		{
			RenderTextureFormat format = ((!isShadowMap) ? RenderTextureFormat.Depth : RenderTextureFormat.Shadowmap);
			renderTexture = new RenderTexture(width, height, (int)depthBufferBits, format, RenderTextureReadWrite.Linear)
			{
				hideFlags = HideFlags.HideAndDontSave,
				volumeDepth = slices,
				filterMode = filterMode,
				wrapMode = wrapMode,
				dimension = dimension,
				enableRandomWrite = enableRandomWrite,
				useMipMap = useMipMap,
				autoGenerateMips = autoGenerateMips,
				anisoLevel = anisoLevel,
				mipMapBias = mipMapBias,
				antiAliasing = (int)msaaSamples,
				bindTextureMS = bindTextureMS,
				useDynamicScale = (m_HardwareDynamicResRequested && useDynamicScale),
				memorylessMode = memoryless,
				name = CoreUtils.GetRenderTargetAutoName(width, height, slices, format, name, useMipMap, flag, msaaSamples)
			};
		}
		else
		{
			renderTexture = new RenderTexture(width, height, (int)depthBufferBits, colorFormat)
			{
				hideFlags = HideFlags.HideAndDontSave,
				volumeDepth = slices,
				filterMode = filterMode,
				wrapMode = wrapMode,
				dimension = dimension,
				enableRandomWrite = enableRandomWrite,
				useMipMap = useMipMap,
				autoGenerateMips = autoGenerateMips,
				anisoLevel = anisoLevel,
				mipMapBias = mipMapBias,
				antiAliasing = (int)msaaSamples,
				bindTextureMS = bindTextureMS,
				useDynamicScale = (m_HardwareDynamicResRequested && useDynamicScale),
				memorylessMode = memoryless,
				name = CoreUtils.GetRenderTargetAutoName(width, height, slices, GraphicsFormatUtility.GetRenderTextureFormat(colorFormat), name, useMipMap, flag, msaaSamples)
			};
		}
		renderTexture.Create();
		RTHandle rTHandle = new RTHandle(this);
		rTHandle.SetRenderTexture(renderTexture);
		rTHandle.useScaling = false;
		rTHandle.m_EnableRandomWrite = enableRandomWrite;
		rTHandle.m_EnableMSAA = flag;
		rTHandle.m_EnableHWDynamicScale = useDynamicScale;
		rTHandle.m_Name = name;
		rTHandle.referenceSize = new Vector2Int(width, height);
		return rTHandle;
	}

	public RTHandle Alloc(Vector2 scaleFactor, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, bool enableMSAA = false, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, string name = "")
	{
		int num = Mathf.Max(Mathf.RoundToInt(scaleFactor.x * (float)GetMaxWidth()), 1);
		int num2 = Mathf.Max(Mathf.RoundToInt(scaleFactor.y * (float)GetMaxHeight()), 1);
		RTHandle rTHandle = AllocAutoSizedRenderTexture(num, num2, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, enableMSAA, bindTextureMS, useDynamicScale, memoryless, name);
		rTHandle.referenceSize = new Vector2Int(num, num2);
		rTHandle.scaleFactor = scaleFactor;
		return rTHandle;
	}

	public RTHandle Alloc(ScaleFunc scaleFunc, int slices = 1, DepthBits depthBufferBits = DepthBits.None, GraphicsFormat colorFormat = GraphicsFormat.R8G8B8A8_SRGB, FilterMode filterMode = FilterMode.Point, TextureWrapMode wrapMode = TextureWrapMode.Repeat, TextureDimension dimension = TextureDimension.Tex2D, bool enableRandomWrite = false, bool useMipMap = false, bool autoGenerateMips = true, bool isShadowMap = false, int anisoLevel = 1, float mipMapBias = 0f, bool enableMSAA = false, bool bindTextureMS = false, bool useDynamicScale = false, RenderTextureMemoryless memoryless = RenderTextureMemoryless.None, string name = "")
	{
		Vector2Int vector2Int = scaleFunc(new Vector2Int(GetMaxWidth(), GetMaxHeight()));
		int num = Mathf.Max(vector2Int.x, 1);
		int num2 = Mathf.Max(vector2Int.y, 1);
		RTHandle rTHandle = AllocAutoSizedRenderTexture(num, num2, slices, depthBufferBits, colorFormat, filterMode, wrapMode, dimension, enableRandomWrite, useMipMap, autoGenerateMips, isShadowMap, anisoLevel, mipMapBias, enableMSAA, bindTextureMS, useDynamicScale, memoryless, name);
		rTHandle.referenceSize = new Vector2Int(num, num2);
		rTHandle.scaleFunc = scaleFunc;
		return rTHandle;
	}

	private RTHandle AllocAutoSizedRenderTexture(int width, int height, int slices, DepthBits depthBufferBits, GraphicsFormat colorFormat, FilterMode filterMode, TextureWrapMode wrapMode, TextureDimension dimension, bool enableRandomWrite, bool useMipMap, bool autoGenerateMips, bool isShadowMap, int anisoLevel, float mipMapBias, bool enableMSAA, bool bindTextureMS, bool useDynamicScale, RenderTextureMemoryless memoryless, string name)
	{
		if (!enableMSAA && bindTextureMS)
		{
			Debug.LogWarning("RTHandle allocated without MSAA but with bindMS set to true, forcing bindMS to false.");
			bindTextureMS = false;
		}
		bool flag = m_ScaledRTSupportsMSAA && enableMSAA;
		if (!flag)
		{
			bindTextureMS = false;
		}
		bool flag2 = enableRandomWrite;
		if (flag && flag2)
		{
			Debug.LogWarning("RTHandle that is MSAA-enabled cannot allocate MSAA RT with 'enableRandomWrite = true'.");
			flag2 = false;
		}
		int antiAliasing = (int)((!flag) ? MSAASamples.None : m_ScaledRTCurrentMSAASamples);
		RenderTexture renderTexture;
		if (isShadowMap || depthBufferBits != DepthBits.None)
		{
			RenderTextureFormat format = ((!isShadowMap) ? RenderTextureFormat.Depth : RenderTextureFormat.Shadowmap);
			GraphicsFormat stencilFormat = ((!isShadowMap) ? GraphicsFormat.R8_UInt : GraphicsFormat.None);
			renderTexture = new RenderTexture(width, height, (int)depthBufferBits, format, RenderTextureReadWrite.Linear)
			{
				hideFlags = HideFlags.HideAndDontSave,
				volumeDepth = slices,
				filterMode = filterMode,
				wrapMode = wrapMode,
				dimension = dimension,
				enableRandomWrite = flag2,
				useMipMap = useMipMap,
				autoGenerateMips = autoGenerateMips,
				anisoLevel = anisoLevel,
				mipMapBias = mipMapBias,
				antiAliasing = antiAliasing,
				bindTextureMS = bindTextureMS,
				useDynamicScale = (m_HardwareDynamicResRequested && useDynamicScale),
				memorylessMode = memoryless,
				stencilFormat = stencilFormat,
				name = CoreUtils.GetRenderTargetAutoName(width, height, slices, GraphicsFormatUtility.GetRenderTextureFormat(colorFormat), name, useMipMap, flag, m_ScaledRTCurrentMSAASamples)
			};
		}
		else
		{
			renderTexture = new RenderTexture(width, height, (int)depthBufferBits, colorFormat)
			{
				hideFlags = HideFlags.HideAndDontSave,
				volumeDepth = slices,
				filterMode = filterMode,
				wrapMode = wrapMode,
				dimension = dimension,
				enableRandomWrite = flag2,
				useMipMap = useMipMap,
				autoGenerateMips = autoGenerateMips,
				anisoLevel = anisoLevel,
				mipMapBias = mipMapBias,
				antiAliasing = antiAliasing,
				bindTextureMS = bindTextureMS,
				useDynamicScale = (m_HardwareDynamicResRequested && useDynamicScale),
				memorylessMode = memoryless,
				name = CoreUtils.GetRenderTargetAutoName(width, height, slices, GraphicsFormatUtility.GetRenderTextureFormat(colorFormat), name, useMipMap, flag, m_ScaledRTCurrentMSAASamples)
			};
		}
		renderTexture.Create();
		RTHandle rTHandle = new RTHandle(this);
		rTHandle.SetRenderTexture(renderTexture);
		rTHandle.m_EnableMSAA = enableMSAA;
		rTHandle.m_EnableRandomWrite = enableRandomWrite;
		rTHandle.useScaling = true;
		rTHandle.m_EnableHWDynamicScale = useDynamicScale;
		rTHandle.m_Name = name;
		m_AutoSizedRTs.Add(rTHandle);
		return rTHandle;
	}

	public RTHandle Alloc(Texture texture)
	{
		RTHandle rTHandle = new RTHandle(this);
		rTHandle.SetTexture(texture);
		rTHandle.m_EnableMSAA = false;
		rTHandle.m_EnableRandomWrite = false;
		rTHandle.useScaling = false;
		rTHandle.m_EnableHWDynamicScale = false;
		rTHandle.m_Name = "";
		return rTHandle;
	}

	private static RTHandle Alloc(RTHandle tex)
	{
		Debug.LogError("Allocation a RTHandle from another one is forbidden.");
		return null;
	}

	internal string DumpRTInfo()
	{
		string text = "";
		Array.Resize(ref m_AutoSizedRTsArray, m_AutoSizedRTs.Count);
		m_AutoSizedRTs.CopyTo(m_AutoSizedRTsArray);
		int i = 0;
		for (int num = m_AutoSizedRTsArray.Length; i < num; i++)
		{
			RenderTexture rt = m_AutoSizedRTsArray[i].rt;
			text = $"{text}\nRT ({i})\t Format: {rt.format} W: {rt.width} H {rt.height}\n";
		}
		return text;
	}
}
