using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace FibMatrix.Rendering;

public static class OverlayBackground
{
	private struct LayerMaskBackup
	{
		public int layer;

		public uint renderingLayerMask;
	}

	public const string IntensityPropertyName = "_Intensity";

	public static readonly int IntensityPropertyId = Shader.PropertyToID("_Intensity");

	private static bool m_ClearDepthOld = true;

	public const string LayerName = "RenderFeature";

	private static int m_DefaultLayer = -1;

	private static int m_RenderFeatureLayer = m_DefaultLayer;

	public const string RenderingLayerName = "OverlayBackground";

	private static uint m_DefaultMask = uint.MaxValue;

	private static uint m_OverlayBackgroundMask = m_DefaultMask;

	private static OverlayBackgroundRendererFeature m_Feature;

	private static Dictionary<Renderer, LayerMaskBackup> m_RendererLayerDictionary = new Dictionary<Renderer, LayerMaskBackup>(16);

	private static List<Renderer> m_RendererBuffer = new List<Renderer>(16);

	internal static Tween m_Tween;

	public static Material OverrideMaterial
	{
		get
		{
			if (Feature != null)
			{
				return Feature.overrideMaterial;
			}
			return null;
		}
		set
		{
			if (Feature != null)
			{
				Feature.overrideMaterial = value;
			}
		}
	}

	public static float Intensity
	{
		get
		{
			Material overrideMaterial = OverrideMaterial;
			if (overrideMaterial != null)
			{
				return overrideMaterial.GetFloat(IntensityPropertyId);
			}
			return 0f;
		}
		set
		{
			Material overrideMaterial = OverrideMaterial;
			if (overrideMaterial != null)
			{
				overrideMaterial.SetFloat(IntensityPropertyId, value);
			}
		}
	}

	public static int RenderFeatureLayer
	{
		get
		{
			if (m_RenderFeatureLayer == m_DefaultLayer)
			{
				m_RenderFeatureLayer = LayerMask.NameToLayer("RenderFeature");
			}
			return m_RenderFeatureLayer;
		}
	}

	public static uint OverlayBackgroundMask
	{
		get
		{
			if (m_OverlayBackgroundMask == m_DefaultMask)
			{
				m_OverlayBackgroundMask = (uint)RenderingLayerMask.NameToMask("OverlayBackground");
				if (m_OverlayBackgroundMask == 0)
				{
					Debug.LogError("OverlayBackgroundMask == 0");
				}
			}
			return m_OverlayBackgroundMask;
		}
	}

	internal static OverlayBackgroundRendererFeature Feature
	{
		get
		{
			if (m_Feature == null)
			{
				m_Feature = RenderQualitySetting.ScriptableRenderer.GetRendererFeature<OverlayBackgroundRendererFeature>();
			}
			return m_Feature;
		}
	}

	public static void Register(GameObject go)
	{
		go.GetComponentsInChildren(includeInactive: true, m_RendererBuffer);
		Register(m_RendererBuffer);
		m_RendererBuffer.Clear();
	}

	public static void Register(List<Renderer> renderers)
	{
		if (renderers == null)
		{
			return;
		}
		foreach (Renderer renderer in renderers)
		{
			Register(renderer);
		}
	}

	public static void Register(Renderer renderer)
	{
		if (!m_RendererLayerDictionary.ContainsKey(renderer) || renderer.renderingLayerMask != OverlayBackgroundMask)
		{
			m_RendererLayerDictionary[renderer] = new LayerMaskBackup
			{
				layer = renderer.gameObject.layer,
				renderingLayerMask = renderer.renderingLayerMask
			};
			renderer.gameObject.layer = RenderFeatureLayer;
			renderer.renderingLayerMask = OverlayBackgroundMask;
		}
	}

	public static void ReRegister()
	{
		foreach (KeyValuePair<Renderer, LayerMaskBackup> item in m_RendererLayerDictionary)
		{
			if (item.Key != null)
			{
				item.Key.renderingLayerMask = OverlayBackgroundMask;
			}
		}
	}

	public static void Deregister(GameObject go)
	{
		if (go == null)
		{
			Debug.LogError("null gameobject");
		}
		go.GetComponentsInChildren(includeInactive: true, m_RendererBuffer);
		Deregister(m_RendererBuffer);
		m_RendererBuffer.Clear();
	}

	public static void Deregister(List<Renderer> renderers)
	{
		if (renderers == null)
		{
			return;
		}
		foreach (Renderer renderer in renderers)
		{
			Deregister(renderer);
		}
	}

	public static void Deregister(Renderer renderer)
	{
		if (m_RendererLayerDictionary.TryGetValue(renderer, out var value))
		{
			if (renderer != null)
			{
				renderer.gameObject.layer = value.layer;
				renderer.renderingLayerMask = value.renderingLayerMask;
			}
			m_RendererLayerDictionary.Remove(renderer);
		}
	}

	public static void Deregister(bool clear = true)
	{
		foreach (KeyValuePair<Renderer, LayerMaskBackup> item in m_RendererLayerDictionary)
		{
			if (item.Key != null)
			{
				item.Key.gameObject.layer = item.Value.layer;
				item.Key.renderingLayerMask = item.Value.renderingLayerMask;
			}
		}
		if (clear)
		{
			m_RendererLayerDictionary.Clear();
		}
	}

	public static bool Tween(float intensity, bool clearDepth, bool active)
	{
		if (Feature != null)
		{
			if (Feature.overrideMaterial != null)
			{
				Feature.overrideMaterial.SetFloat(IntensityPropertyId, intensity);
			}
			Feature.clearDepth = clearDepth;
			Feature.SetActive(active);
			return true;
		}
		return false;
	}

	public static Tween Tween(float target, float during, bool active, bool clearDepth, Action<bool, bool> completeCallback = null)
	{
		return Tween(Intensity, target, during, active, clearDepth, compele: true, deregister: true, completeCallback);
	}

	public static Tween Tween(float target, float during, bool active, bool clearDepth, bool compele, Action<bool, bool> completeCallback = null)
	{
		return Tween(Intensity, target, during, active, clearDepth, compele, deregister: true, completeCallback);
	}

	public static Tween Tween(float target, float during, bool active, bool clearDepth, bool compele, bool deregister, Action<bool, bool> completeCallback = null)
	{
		return Tween(Intensity, target, during, active, clearDepth, compele, deregister, completeCallback);
	}

	public static Tween Tween(float start, float target, float during, bool active, bool clearDepth, bool compele, bool deregister, Action<bool, bool> completeCallback)
	{
		if (Feature == null)
		{
			return null;
		}
		Intensity = start;
		if (m_Tween != null && !m_Tween.IsComplete() && compele)
		{
			m_Tween.Complete(withCallbacks: false);
			completeCallback?.Invoke(active, arg2: false);
		}
		m_Tween = DOTween.To(() => Intensity, delegate(float value)
		{
			Intensity = value;
		}, target, during).OnStart(delegate
		{
			if (active)
			{
				m_ClearDepthOld = Feature.clearDepth;
				Feature.clearDepth = clearDepth;
				Feature.SetActive(active: true);
			}
		}).OnUpdate(delegate
		{
		})
			.OnComplete(delegate
			{
				if (!active)
				{
					Feature.clearDepth = m_ClearDepthOld;
					Feature.SetActive(active: false);
					if (deregister)
					{
						Deregister();
					}
				}
				m_Tween = null;
				completeCallback?.Invoke(active, arg2: true);
			});
		return m_Tween;
	}
}
