using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[UsedByNativeCode]
public struct ShadowDrawingSettings : IEquatable<ShadowDrawingSettings>
{
	private CullingResults m_CullingResults;

	private int m_LightIndex;

	private int m_UseRenderingLayerMaskTest;

	private ShadowSplitData m_SplitData;

	public CullingResults cullingResults
	{
		get
		{
			return m_CullingResults;
		}
		set
		{
			m_CullingResults = value;
		}
	}

	public int lightIndex
	{
		get
		{
			return m_LightIndex;
		}
		set
		{
			m_LightIndex = value;
		}
	}

	public bool useRenderingLayerMaskTest
	{
		get
		{
			return m_UseRenderingLayerMaskTest != 0;
		}
		set
		{
			m_UseRenderingLayerMaskTest = (value ? 1 : 0);
		}
	}

	public ShadowSplitData splitData
	{
		get
		{
			return m_SplitData;
		}
		set
		{
			m_SplitData = value;
		}
	}

	public ShadowDrawingSettings(CullingResults cullingResults, int lightIndex)
	{
		m_CullingResults = cullingResults;
		m_LightIndex = lightIndex;
		m_UseRenderingLayerMaskTest = 0;
		m_SplitData = default(ShadowSplitData);
		m_SplitData.shadowCascadeBlendCullingFactor = 1f;
	}

	public bool Equals(ShadowDrawingSettings other)
	{
		return m_CullingResults.Equals(other.m_CullingResults) && m_LightIndex == other.m_LightIndex && m_SplitData.Equals(other.m_SplitData) && m_UseRenderingLayerMaskTest.Equals(other.m_UseRenderingLayerMaskTest);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is ShadowDrawingSettings && Equals((ShadowDrawingSettings)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = m_CullingResults.GetHashCode();
		hashCode = (hashCode * 397) ^ m_LightIndex;
		hashCode = (hashCode * 397) ^ m_UseRenderingLayerMaskTest;
		return (hashCode * 397) ^ m_SplitData.GetHashCode();
	}

	public static bool operator ==(ShadowDrawingSettings left, ShadowDrawingSettings right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ShadowDrawingSettings left, ShadowDrawingSettings right)
	{
		return !left.Equals(right);
	}
}
