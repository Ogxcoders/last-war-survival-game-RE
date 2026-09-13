using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[ExecuteAlways]
[HelpURL("http://esotericsoftware.com/spine-unity#SkeletonGraphicCustomMaterials")]
public class SkeletonGraphicCustomMaterials : MonoBehaviour
{
	[Serializable]
	public struct AtlasMaterialOverride : IEquatable<AtlasMaterialOverride>
	{
		public bool overrideEnabled;

		public Texture originalTexture;

		public Material replacementMaterial;

		public bool Equals(AtlasMaterialOverride other)
		{
			if (overrideEnabled == other.overrideEnabled && originalTexture == other.originalTexture)
			{
				return replacementMaterial == other.replacementMaterial;
			}
			return false;
		}
	}

	[Serializable]
	public struct AtlasTextureOverride : IEquatable<AtlasTextureOverride>
	{
		public bool overrideEnabled;

		public Texture originalTexture;

		public Texture replacementTexture;

		public bool Equals(AtlasTextureOverride other)
		{
			if (overrideEnabled == other.overrideEnabled && originalTexture == other.originalTexture)
			{
				return replacementTexture == other.replacementTexture;
			}
			return false;
		}
	}

	public SkeletonGraphic skeletonGraphic;

	[SerializeField]
	protected List<AtlasMaterialOverride> customMaterialOverrides = new List<AtlasMaterialOverride>();

	[SerializeField]
	protected List<AtlasTextureOverride> customTextureOverrides = new List<AtlasTextureOverride>();

	private void SetCustomMaterialOverrides()
	{
		if (skeletonGraphic == null)
		{
			Debug.LogError("skeletonGraphic == null");
			return;
		}
		for (int i = 0; i < customMaterialOverrides.Count; i++)
		{
			AtlasMaterialOverride atlasMaterialOverride = customMaterialOverrides[i];
			if (atlasMaterialOverride.overrideEnabled)
			{
				skeletonGraphic.CustomMaterialOverride[atlasMaterialOverride.originalTexture] = atlasMaterialOverride.replacementMaterial;
			}
		}
	}

	private void RemoveCustomMaterialOverrides()
	{
		if (skeletonGraphic == null)
		{
			Debug.LogError("skeletonGraphic == null");
			return;
		}
		for (int i = 0; i < customMaterialOverrides.Count; i++)
		{
			AtlasMaterialOverride atlasMaterialOverride = customMaterialOverrides[i];
			if (skeletonGraphic.CustomMaterialOverride.TryGetValue(atlasMaterialOverride.originalTexture, out var value) && !(value != atlasMaterialOverride.replacementMaterial))
			{
				skeletonGraphic.CustomMaterialOverride.Remove(atlasMaterialOverride.originalTexture);
			}
		}
	}

	private void SetCustomTextureOverrides()
	{
		if (skeletonGraphic == null)
		{
			Debug.LogError("skeletonGraphic == null");
			return;
		}
		for (int i = 0; i < customTextureOverrides.Count; i++)
		{
			AtlasTextureOverride atlasTextureOverride = customTextureOverrides[i];
			if (atlasTextureOverride.overrideEnabled)
			{
				skeletonGraphic.CustomTextureOverride[atlasTextureOverride.originalTexture] = atlasTextureOverride.replacementTexture;
			}
		}
	}

	private void RemoveCustomTextureOverrides()
	{
		if (skeletonGraphic == null)
		{
			Debug.LogError("skeletonGraphic == null");
			return;
		}
		for (int i = 0; i < customTextureOverrides.Count; i++)
		{
			AtlasTextureOverride atlasTextureOverride = customTextureOverrides[i];
			if (skeletonGraphic.CustomTextureOverride.TryGetValue(atlasTextureOverride.originalTexture, out var value) && !(value != atlasTextureOverride.replacementTexture))
			{
				skeletonGraphic.CustomTextureOverride.Remove(atlasTextureOverride.originalTexture);
			}
		}
	}

	private void OnEnable()
	{
		if (skeletonGraphic == null)
		{
			skeletonGraphic = GetComponent<SkeletonGraphic>();
		}
		if (skeletonGraphic == null)
		{
			Debug.LogError("skeletonGraphic == null");
			return;
		}
		skeletonGraphic.Initialize(overwrite: false);
		SetCustomMaterialOverrides();
		SetCustomTextureOverrides();
	}

	private void OnDisable()
	{
		if (skeletonGraphic == null)
		{
			Debug.LogError("skeletonGraphic == null");
			return;
		}
		RemoveCustomMaterialOverrides();
		RemoveCustomTextureOverrides();
	}
}
