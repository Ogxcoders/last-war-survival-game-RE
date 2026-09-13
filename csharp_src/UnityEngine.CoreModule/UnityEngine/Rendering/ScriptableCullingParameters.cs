using System;
using UnityEngine.Scripting;

namespace UnityEngine.Rendering;

[UsedByNativeCode]
public struct ScriptableCullingParameters : IEquatable<ScriptableCullingParameters>
{
	private int m_IsOrthographic;

	private LODParameters m_LODParameters;

	private const int k_MaximumCullingPlaneCount = 10;

	public static readonly int maximumCullingPlaneCount = 10;

	internal unsafe fixed byte m_CullingPlanes[160];

	private int m_CullingPlaneCount;

	private uint m_CullingMask;

	private ulong m_SceneMask;

	private const int k_LayerCount = 32;

	public static readonly int layerCount = 32;

	internal unsafe fixed float m_LayerFarCullDistances[32];

	private int m_LayerCull;

	private Matrix4x4 m_CullingMatrix;

	private Vector3 m_Origin;

	private float m_ShadowDistance;

	private CullingOptions m_CullingOptions;

	private ReflectionProbeSortingCriteria m_ReflectionProbeSortingCriteria;

	private CameraProperties m_CameraProperties;

	private float m_AccurateOcclusionThreshold;

	private int m_MaximumPortalCullingJobs;

	private const int k_CullingJobCountLowerLimit = 1;

	private const int k_CullingJobCountUpperLimit = 16;

	private Matrix4x4 m_StereoViewMatrix;

	private Matrix4x4 m_StereoProjectionMatrix;

	private float m_StereoSeparationDistance;

	private int m_maximumVisibleLights;

	public int maximumVisibleLights
	{
		get
		{
			return m_maximumVisibleLights;
		}
		set
		{
			m_maximumVisibleLights = value;
		}
	}

	public int cullingPlaneCount
	{
		get
		{
			return m_CullingPlaneCount;
		}
		set
		{
			if (value < 0 || value > 10)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "value", value, 10));
			}
			m_CullingPlaneCount = value;
		}
	}

	public bool isOrthographic
	{
		get
		{
			return Convert.ToBoolean(m_IsOrthographic);
		}
		set
		{
			m_IsOrthographic = Convert.ToInt32(value);
		}
	}

	public LODParameters lodParameters
	{
		get
		{
			return m_LODParameters;
		}
		set
		{
			m_LODParameters = value;
		}
	}

	public uint cullingMask
	{
		get
		{
			return m_CullingMask;
		}
		set
		{
			m_CullingMask = value;
		}
	}

	public Matrix4x4 cullingMatrix
	{
		get
		{
			return m_CullingMatrix;
		}
		set
		{
			m_CullingMatrix = value;
		}
	}

	public Vector3 origin
	{
		get
		{
			return m_Origin;
		}
		set
		{
			m_Origin = value;
		}
	}

	public float shadowDistance
	{
		get
		{
			return m_ShadowDistance;
		}
		set
		{
			m_ShadowDistance = value;
		}
	}

	public CullingOptions cullingOptions
	{
		get
		{
			return m_CullingOptions;
		}
		set
		{
			m_CullingOptions = value;
		}
	}

	public ReflectionProbeSortingCriteria reflectionProbeSortingCriteria
	{
		get
		{
			return m_ReflectionProbeSortingCriteria;
		}
		set
		{
			m_ReflectionProbeSortingCriteria = value;
		}
	}

	public CameraProperties cameraProperties
	{
		get
		{
			return m_CameraProperties;
		}
		set
		{
			m_CameraProperties = value;
		}
	}

	public Matrix4x4 stereoViewMatrix
	{
		get
		{
			return m_StereoViewMatrix;
		}
		set
		{
			m_StereoViewMatrix = value;
		}
	}

	public Matrix4x4 stereoProjectionMatrix
	{
		get
		{
			return m_StereoProjectionMatrix;
		}
		set
		{
			m_StereoProjectionMatrix = value;
		}
	}

	public float stereoSeparationDistance
	{
		get
		{
			return m_StereoSeparationDistance;
		}
		set
		{
			m_StereoSeparationDistance = value;
		}
	}

	public float accurateOcclusionThreshold
	{
		get
		{
			return m_AccurateOcclusionThreshold;
		}
		set
		{
			m_AccurateOcclusionThreshold = Mathf.Max(-1f, value);
		}
	}

	public int maximumPortalCullingJobs
	{
		get
		{
			return m_MaximumPortalCullingJobs;
		}
		set
		{
			if (value < 1 || value > 16)
			{
				throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be in range {2} to {3}", "maximumPortalCullingJobs", maximumPortalCullingJobs, 1, 16));
			}
			m_MaximumPortalCullingJobs = value;
		}
	}

	public static int cullingJobsLowerLimit => 1;

	public static int cullingJobsUpperLimit => 16;

	public unsafe float GetLayerCullingDistance(int layerIndex)
	{
		if (layerIndex < 0 || layerIndex >= 32)
		{
			throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "layerIndex", layerIndex, 32));
		}
		fixed (float* layerFarCullDistances = m_LayerFarCullDistances)
		{
			return layerFarCullDistances[layerIndex];
		}
	}

	public unsafe void SetLayerCullingDistance(int layerIndex, float distance)
	{
		if (layerIndex < 0 || layerIndex >= 32)
		{
			throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "layerIndex", layerIndex, 32));
		}
		fixed (float* layerFarCullDistances = m_LayerFarCullDistances)
		{
			layerFarCullDistances[layerIndex] = distance;
		}
	}

	public unsafe Plane GetCullingPlane(int index)
	{
		if (index < 0 || index >= cullingPlaneCount)
		{
			throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, cullingPlaneCount));
		}
		fixed (byte* cullingPlanes = m_CullingPlanes)
		{
			Plane* ptr = (Plane*)cullingPlanes;
			return ptr[index];
		}
	}

	public unsafe void SetCullingPlane(int index, Plane plane)
	{
		if (index < 0 || index >= cullingPlaneCount)
		{
			throw new ArgumentOutOfRangeException(string.Format("{0} was {1}, but must be at least 0 and less than {2}", "index", index, cullingPlaneCount));
		}
		fixed (byte* cullingPlanes = m_CullingPlanes)
		{
			Plane* ptr = (Plane*)cullingPlanes;
			ptr[index] = plane;
		}
	}

	public bool Equals(ScriptableCullingParameters other)
	{
		for (int i = 0; i < 32; i++)
		{
			if (!GetLayerCullingDistance(i).Equals(other.GetLayerCullingDistance(i)))
			{
				return false;
			}
		}
		for (int j = 0; j < cullingPlaneCount; j++)
		{
			if (!GetCullingPlane(j).Equals(other.GetCullingPlane(j)))
			{
				return false;
			}
		}
		return m_IsOrthographic == other.m_IsOrthographic && m_LODParameters.Equals(other.m_LODParameters) && m_CullingPlaneCount == other.m_CullingPlaneCount && m_CullingMask == other.m_CullingMask && m_SceneMask == other.m_SceneMask && m_LayerCull == other.m_LayerCull && m_CullingMatrix.Equals(other.m_CullingMatrix) && m_Origin.Equals(other.m_Origin) && m_ShadowDistance.Equals(other.m_ShadowDistance) && m_CullingOptions == other.m_CullingOptions && m_ReflectionProbeSortingCriteria == other.m_ReflectionProbeSortingCriteria && m_CameraProperties.Equals(other.m_CameraProperties) && m_AccurateOcclusionThreshold.Equals(other.m_AccurateOcclusionThreshold) && m_StereoViewMatrix.Equals(other.m_StereoViewMatrix) && m_StereoProjectionMatrix.Equals(other.m_StereoProjectionMatrix) && m_StereoSeparationDistance.Equals(other.m_StereoSeparationDistance) && m_maximumVisibleLights == other.m_maximumVisibleLights;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		return obj is ScriptableCullingParameters && Equals((ScriptableCullingParameters)obj);
	}

	public override int GetHashCode()
	{
		int num = m_IsOrthographic;
		num = (num * 397) ^ m_LODParameters.GetHashCode();
		num = (num * 397) ^ m_CullingPlaneCount;
		num = (num * 397) ^ (int)m_CullingMask;
		num = (num * 397) ^ m_SceneMask.GetHashCode();
		num = (num * 397) ^ m_LayerCull;
		num = (num * 397) ^ m_CullingMatrix.GetHashCode();
		num = (num * 397) ^ m_Origin.GetHashCode();
		num = (num * 397) ^ m_ShadowDistance.GetHashCode();
		num = (num * 397) ^ (int)m_CullingOptions;
		num = (num * 397) ^ (int)m_ReflectionProbeSortingCriteria;
		num = (num * 397) ^ m_CameraProperties.GetHashCode();
		num = (num * 397) ^ m_AccurateOcclusionThreshold.GetHashCode();
		num = (num * 397) ^ m_MaximumPortalCullingJobs.GetHashCode();
		num = (num * 397) ^ m_StereoViewMatrix.GetHashCode();
		num = (num * 397) ^ m_StereoProjectionMatrix.GetHashCode();
		num = (num * 397) ^ m_StereoSeparationDistance.GetHashCode();
		return (num * 397) ^ m_maximumVisibleLights;
	}

	public static bool operator ==(ScriptableCullingParameters left, ScriptableCullingParameters right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(ScriptableCullingParameters left, ScriptableCullingParameters right)
	{
		return !left.Equals(right);
	}
}
