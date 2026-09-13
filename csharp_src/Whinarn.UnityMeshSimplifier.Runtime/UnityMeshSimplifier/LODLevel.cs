using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityMeshSimplifier;

[Serializable]
public struct LODLevel
{
	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("The screen relative height to use for the transition.")]
	private float screenRelativeTransitionHeight;

	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("The width of the cross-fade transition zone (proportion to the current LOD's whole length).")]
	private float fadeTransitionWidth;

	[SerializeField]
	[Range(0f, 1f)]
	[Tooltip("The desired quality for this level.")]
	private float quality;

	[SerializeField]
	[Tooltip("If all renderers and meshes under this level should be combined into one, where possible.")]
	private bool combineMeshes;

	[SerializeField]
	[Tooltip("If all sub-meshes should be combined into one, where possible.")]
	private bool combineSubMeshes;

	[SerializeField]
	[Tooltip("The renderers used in this level.")]
	private Renderer[] renderers;

	[SerializeField]
	[Tooltip("The skin quality to use for renderers on this level.")]
	private SkinQuality skinQuality;

	[SerializeField]
	[Tooltip("The shadow casting mode for renderers on this level.")]
	private ShadowCastingMode shadowCastingMode;

	[SerializeField]
	[Tooltip("If renderers on this level should receive shadows.")]
	private bool receiveShadows;

	[SerializeField]
	[Tooltip("The motion vector generation mode for renderers on this level.")]
	private MotionVectorGenerationMode motionVectorGenerationMode;

	[SerializeField]
	[Tooltip("If renderers on this level should use skinned motion vectors.")]
	private bool skinnedMotionVectors;

	[SerializeField]
	[Tooltip("The light probe usage for renderers on this level.")]
	private LightProbeUsage lightProbeUsage;

	[SerializeField]
	[Tooltip("The reflection probe usage for renderers on this level.")]
	private ReflectionProbeUsage reflectionProbeUsage;

	public float ScreenRelativeTransitionHeight
	{
		get
		{
			return screenRelativeTransitionHeight;
		}
		set
		{
			screenRelativeTransitionHeight = Mathf.Clamp01(value);
		}
	}

	public float FadeTransitionWidth
	{
		get
		{
			return fadeTransitionWidth;
		}
		set
		{
			fadeTransitionWidth = Mathf.Clamp01(value);
		}
	}

	public float Quality
	{
		get
		{
			return quality;
		}
		set
		{
			quality = Mathf.Clamp01(value);
		}
	}

	public bool CombineMeshes
	{
		get
		{
			return combineMeshes;
		}
		set
		{
			combineMeshes = value;
		}
	}

	public bool CombineSubMeshes
	{
		get
		{
			return combineSubMeshes;
		}
		set
		{
			combineSubMeshes = value;
		}
	}

	public Renderer[] Renderers
	{
		get
		{
			return renderers;
		}
		set
		{
			renderers = value;
		}
	}

	public SkinQuality SkinQuality
	{
		get
		{
			return skinQuality;
		}
		set
		{
			skinQuality = value;
		}
	}

	public ShadowCastingMode ShadowCastingMode
	{
		get
		{
			return shadowCastingMode;
		}
		set
		{
			shadowCastingMode = value;
		}
	}

	public bool ReceiveShadows
	{
		get
		{
			return receiveShadows;
		}
		set
		{
			receiveShadows = value;
		}
	}

	public MotionVectorGenerationMode MotionVectorGenerationMode
	{
		get
		{
			return motionVectorGenerationMode;
		}
		set
		{
			motionVectorGenerationMode = value;
		}
	}

	public bool SkinnedMotionVectors
	{
		get
		{
			return skinnedMotionVectors;
		}
		set
		{
			skinnedMotionVectors = value;
		}
	}

	public LightProbeUsage LightProbeUsage
	{
		get
		{
			return lightProbeUsage;
		}
		set
		{
			lightProbeUsage = value;
		}
	}

	public ReflectionProbeUsage ReflectionProbeUsage
	{
		get
		{
			return reflectionProbeUsage;
		}
		set
		{
			reflectionProbeUsage = value;
		}
	}

	public LODLevel(float screenRelativeTransitionHeight, float quality)
		: this(screenRelativeTransitionHeight, 0f, quality, combineMeshes: false, combineSubMeshes: false, null)
	{
	}

	public LODLevel(float screenRelativeTransitionHeight, float fadeTransitionWidth, float quality, bool combineMeshes, bool combineSubMeshes)
		: this(screenRelativeTransitionHeight, fadeTransitionWidth, quality, combineMeshes, combineSubMeshes, null)
	{
	}

	public LODLevel(float screenRelativeTransitionHeight, float fadeTransitionWidth, float quality, bool combineMeshes, bool combineSubMeshes, Renderer[] renderers)
	{
		this.screenRelativeTransitionHeight = Mathf.Clamp01(screenRelativeTransitionHeight);
		this.fadeTransitionWidth = fadeTransitionWidth;
		this.quality = Mathf.Clamp01(quality);
		this.combineMeshes = combineMeshes;
		this.combineSubMeshes = combineSubMeshes;
		this.renderers = renderers;
		skinQuality = SkinQuality.Auto;
		shadowCastingMode = ShadowCastingMode.On;
		receiveShadows = true;
		motionVectorGenerationMode = MotionVectorGenerationMode.Object;
		skinnedMotionVectors = true;
		lightProbeUsage = LightProbeUsage.BlendProbes;
		reflectionProbeUsage = ReflectionProbeUsage.BlendProbes;
	}
}
