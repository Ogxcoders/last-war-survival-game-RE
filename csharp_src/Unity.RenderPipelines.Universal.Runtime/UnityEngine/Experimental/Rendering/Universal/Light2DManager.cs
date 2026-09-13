using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.Rendering.Universal;

internal class Light2DManager : IDisposable
{
	private const int k_BlendStyleCount = 4;

	private static Light2DManager s_Instance = new Light2DManager();

	private Light2DManager m_PrevInstance;

	private List<Light2D>[] m_Lights;

	private CullingGroup m_CullingGroup;

	private BoundingSphere[] m_BoundingSpheres;

	internal static List<Light2D>[] lights => s_Instance.m_Lights;

	internal static CullingGroup cullingGroup
	{
		get
		{
			return s_Instance.m_CullingGroup;
		}
		set
		{
			s_Instance.m_CullingGroup = value;
		}
	}

	internal static BoundingSphere[] boundingSpheres
	{
		get
		{
			return s_Instance.m_BoundingSpheres;
		}
		set
		{
			s_Instance.m_BoundingSpheres = value;
		}
	}

	internal static bool GetGlobalColor(int sortingLayerIndex, int blendStyleIndex, out Color color)
	{
		bool flag = false;
		color = Color.black;
		List<Light2D> list = s_Instance.m_Lights[blendStyleIndex];
		for (int i = 0; i < list.Count; i++)
		{
			Light2D light2D = list[i];
			if (light2D.lightType == Light2D.LightType.Global && light2D.IsLitLayer(sortingLayerIndex))
			{
				if (true)
				{
					color = light2D.color * light2D.intensity;
					return true;
				}
				if (!flag)
				{
					color = light2D.color * light2D.intensity;
					flag = true;
				}
			}
		}
		return flag;
	}

	internal static bool ContainsDuplicateGlobalLight(int sortingLayerIndex, int blendStyleIndex)
	{
		int num = 0;
		List<Light2D> list = s_Instance.m_Lights[blendStyleIndex];
		for (int i = 0; i < list.Count; i++)
		{
			Light2D light2D = list[i];
			if (light2D.lightType == Light2D.LightType.Global && light2D.IsLitLayer(sortingLayerIndex))
			{
				if (num > 0)
				{
					return true;
				}
				num++;
			}
		}
		return false;
	}

	internal Light2DManager()
	{
		m_PrevInstance = s_Instance;
		s_Instance = this;
		m_Lights = new List<Light2D>[4];
		for (int i = 0; i < m_Lights.Length; i++)
		{
			m_Lights[i] = new List<Light2D>();
		}
	}

	public void Dispose()
	{
		s_Instance = m_PrevInstance;
	}
}
