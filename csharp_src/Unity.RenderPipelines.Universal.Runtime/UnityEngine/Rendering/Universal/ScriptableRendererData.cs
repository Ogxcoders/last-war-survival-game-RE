using System.Collections.Generic;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering.Universal;

[MovedFrom("UnityEngine.Rendering.LWRP")]
public abstract class ScriptableRendererData : ScriptableObject
{
	[SerializeField]
	internal List<ScriptableRendererFeature> m_RendererFeatures = new List<ScriptableRendererFeature>(10);

	[SerializeField]
	internal List<long> m_RendererFeatureMap = new List<long>(10);

	internal bool isInvalidated { get; set; }

	public List<ScriptableRendererFeature> rendererFeatures => m_RendererFeatures;

	protected abstract ScriptableRenderer Create();

	public new void SetDirty()
	{
		isInvalidated = true;
	}

	internal ScriptableRenderer InternalCreateRenderer()
	{
		isInvalidated = false;
		return Create();
	}

	protected virtual void OnValidate()
	{
		SetDirty();
	}

	protected virtual void OnEnable()
	{
		SetDirty();
	}
}
