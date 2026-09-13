using UnityEngine;
using UnityEngine.Rendering;

namespace UnityMeshSimplifier;

[AddComponentMenu("Rendering/LOD Generator Helper")]
public sealed class LODGeneratorHelper : MonoBehaviour
{
	[SerializeField]
	[Tooltip("The fade mode used by the created LOD group.")]
	private LODFadeMode fadeMode;

	[SerializeField]
	[Tooltip("If the cross-fading should be animated by time.")]
	private bool animateCrossFading;

	[SerializeField]
	[Tooltip("If the renderers under this game object and any children should be automatically collected.")]
	private bool autoCollectRenderers = true;

	[SerializeField]
	[Tooltip("The simplification options.")]
	private SimplificationOptions simplificationOptions = SimplificationOptions.Default;

	[SerializeField]
	[Tooltip("The path within the project to save the generated assets. Leave this empty to use the default path.")]
	private string saveAssetsPath = string.Empty;

	[SerializeField]
	[Tooltip("The LOD levels.")]
	private LODLevel[] levels;

	[SerializeField]
	private bool isGenerated;

	public LODFadeMode FadeMode
	{
		get
		{
			return fadeMode;
		}
		set
		{
			fadeMode = value;
		}
	}

	public bool AnimateCrossFading
	{
		get
		{
			return animateCrossFading;
		}
		set
		{
			animateCrossFading = value;
		}
	}

	public bool AutoCollectRenderers
	{
		get
		{
			return autoCollectRenderers;
		}
		set
		{
			autoCollectRenderers = value;
		}
	}

	public SimplificationOptions SimplificationOptions
	{
		get
		{
			return simplificationOptions;
		}
		set
		{
			simplificationOptions = value;
		}
	}

	public string SaveAssetsPath
	{
		get
		{
			return saveAssetsPath;
		}
		set
		{
			saveAssetsPath = value;
		}
	}

	public LODLevel[] Levels
	{
		get
		{
			return levels;
		}
		set
		{
			levels = value;
		}
	}

	public bool IsGenerated => isGenerated;

	private void Reset()
	{
		fadeMode = LODFadeMode.None;
		animateCrossFading = false;
		autoCollectRenderers = true;
		simplificationOptions = SimplificationOptions.Default;
		levels = new LODLevel[3]
		{
			new LODLevel(0.5f, 1f)
			{
				CombineMeshes = false,
				CombineSubMeshes = false,
				SkinQuality = SkinQuality.Auto,
				ShadowCastingMode = ShadowCastingMode.On,
				ReceiveShadows = true,
				SkinnedMotionVectors = true,
				LightProbeUsage = LightProbeUsage.BlendProbes,
				ReflectionProbeUsage = ReflectionProbeUsage.BlendProbes
			},
			new LODLevel(0.17f, 0.65f)
			{
				CombineMeshes = true,
				CombineSubMeshes = false,
				SkinQuality = SkinQuality.Auto,
				ShadowCastingMode = ShadowCastingMode.On,
				ReceiveShadows = true,
				SkinnedMotionVectors = true,
				LightProbeUsage = LightProbeUsage.BlendProbes,
				ReflectionProbeUsage = ReflectionProbeUsage.Simple
			},
			new LODLevel(0.02f, 0.4225f)
			{
				CombineMeshes = true,
				CombineSubMeshes = true,
				SkinQuality = SkinQuality.Bone2,
				ShadowCastingMode = ShadowCastingMode.Off,
				ReceiveShadows = false,
				SkinnedMotionVectors = false,
				LightProbeUsage = LightProbeUsage.Off,
				ReflectionProbeUsage = ReflectionProbeUsage.Off
			}
		};
	}
}
