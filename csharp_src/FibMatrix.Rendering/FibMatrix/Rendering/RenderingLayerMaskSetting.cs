using System;
using System.Collections.Generic;
using FibMatrix.BaseUtils;
using UnityEngine;

namespace FibMatrix.Rendering;

public class RenderingLayerMaskSetting : FibSingletonCfgBase, ISerializationCallbackReceiver
{
	[Serializable]
	public class RenderingLayerMaskItem
	{
		[SerializeField]
		public int layer;

		public string name;

		public string description;

		public RenderingLayerMaskItem()
		{
			layer = 0;
			name = string.Empty;
			description = string.Empty;
		}

		private void OnValueChanged()
		{
			name = name.Trim();
			RenderingLayerMask.Register(layer, name);
		}
	}

	private static RenderingLayerMaskSetting s_Inst;

	[SerializeField]
	private List<RenderingLayerMaskItem> m_RenderingLayerMasks;

	public static RenderingLayerMaskSetting Instance
	{
		get
		{
			if (s_Inst == null)
			{
				s_Inst = FibSingletonCfgBase.LoadOrCreate<RenderingLayerMaskSetting>(runtimeAsset: true);
			}
			return s_Inst;
		}
	}

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void RefreshRenderingLayerMaskSetting()
	{
		if (Instance == null)
		{
			Debug.LogError("RenderingLayerMaskSetting not initialize probably");
		}
	}

	private RenderingLayerMaskSetting()
	{
	}

	public void OnBeforeSerialize()
	{
		if (m_RenderingLayerMasks == null)
		{
			m_RenderingLayerMasks = new List<RenderingLayerMaskItem>(RenderingLayerMask.OverrideRenderingLayerCount);
			for (int i = 0; i < RenderingLayerMask.OverrideRenderingLayerCount; i++)
			{
				m_RenderingLayerMasks.Add(new RenderingLayerMaskItem
				{
					layer = i
				});
			}
		}
		m_RenderingLayerMasks[0].name = "Default";
		m_RenderingLayerMasks[0].description = "Do Not Modify!";
	}

	public void OnAfterDeserialize()
	{
		for (int i = 0; i < RenderingLayerMask.OverrideRenderingLayerCount; i++)
		{
			RenderingLayerMask.Register(m_RenderingLayerMasks[i].layer, m_RenderingLayerMasks[i].name);
		}
	}
}
