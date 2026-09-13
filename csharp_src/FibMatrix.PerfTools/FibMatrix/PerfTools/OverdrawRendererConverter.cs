using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FibMatrix.PerfTools;

public class OverdrawRendererConverter
{
	private int m_OMRendererIndexInAsset = -1;

	private int m_LastRendererIndexInAsset = -1;

	private FieldInfo m_RenderDataListFieldInfo;

	private FieldInfo m_DefaultRendererIndexFieldInfo;

	public OverdrawRendererConverter()
	{
		Type typeFromHandle = typeof(UniversalRenderPipelineAsset);
		m_RenderDataListFieldInfo = typeFromHandle.GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
		if (m_RenderDataListFieldInfo == null)
		{
			Debug.LogError("m_RendererDataList not found");
		}
		m_DefaultRendererIndexFieldInfo = typeFromHandle.GetField("m_DefaultRendererIndex", BindingFlags.Instance | BindingFlags.NonPublic);
		if (m_DefaultRendererIndexFieldInfo == null)
		{
			Debug.LogError("m_DefaultRendererIndex not found");
		}
		m_LastRendererIndexInAsset = -1;
		m_OMRendererIndexInAsset = GetOverdrawRendererIndexInAsset();
		if (m_OMRendererIndexInAsset < 0)
		{
			Debug.LogError("overdraw monitor urp renderer init failed");
		}
	}

	public bool IsValid()
	{
		UniversalRenderPipelineAsset asset = UniversalRenderPipeline.asset;
		if (asset == null)
		{
			return false;
		}
		if (m_RenderDataListFieldInfo == null)
		{
			return false;
		}
		if (m_RenderDataListFieldInfo.GetValue(asset) is ScriptableRendererData[] array && array.Length > m_OMRendererIndexInAsset && m_OMRendererIndexInAsset >= 0)
		{
			return array[m_OMRendererIndexInAsset] != null;
		}
		return false;
	}

	public void Convert()
	{
		if (IsValid())
		{
			m_LastRendererIndexInAsset = (int)m_DefaultRendererIndexFieldInfo.GetValue(UniversalRenderPipeline.asset);
			m_DefaultRendererIndexFieldInfo.SetValue(UniversalRenderPipeline.asset, m_OMRendererIndexInAsset);
		}
	}

	public void Restore()
	{
		if (IsValid())
		{
			m_DefaultRendererIndexFieldInfo.SetValue(UniversalRenderPipeline.asset, m_LastRendererIndexInAsset);
		}
	}

	public static bool IsUseUniversalRenderPipeline()
	{
		if (!(UniversalRenderPipeline.asset == null))
		{
			return true;
		}
		return false;
	}

	private int GetOverdrawRendererIndexInAsset()
	{
		return -1;
	}
}
