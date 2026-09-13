using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[CreateAssetMenu(menuName = "Spine/SkeletonData Modifiers/Blend Mode Materials", order = 200)]
public class BlendModeMaterialsAsset : SkeletonDataModifierAsset
{
	private class AtlasMaterialCache : IDisposable
	{
		private readonly Dictionary<KeyValuePair<AtlasPage, Material>, AtlasPage> cache = new Dictionary<KeyValuePair<AtlasPage, Material>, AtlasPage>();

		public AtlasRegion CloneAtlasRegionWithMaterial(AtlasRegion originalRegion, Material materialTemplate)
		{
			AtlasRegion atlasRegion = originalRegion.Clone();
			atlasRegion.page = GetAtlasPageWithMaterial(originalRegion.page, materialTemplate);
			return atlasRegion;
		}

		private AtlasPage GetAtlasPageWithMaterial(AtlasPage originalPage, Material materialTemplate)
		{
			if (originalPage == null)
			{
				throw new ArgumentNullException("originalPage");
			}
			AtlasPage value = null;
			KeyValuePair<AtlasPage, Material> key = new KeyValuePair<AtlasPage, Material>(originalPage, materialTemplate);
			cache.TryGetValue(key, out value);
			if (value == null)
			{
				value = originalPage.Clone();
				Material material = originalPage.rendererObject as Material;
				value.rendererObject = new Material(materialTemplate)
				{
					name = material.name + " " + materialTemplate.name,
					mainTexture = material.mainTexture
				};
				cache.Add(key, value);
			}
			return value;
		}

		public void Dispose()
		{
			cache.Clear();
		}
	}

	public Material multiplyMaterialTemplate;

	public Material screenMaterialTemplate;

	public Material additiveMaterialTemplate;

	public bool applyAdditiveMaterial = true;

	public override void Apply(SkeletonData skeletonData)
	{
		ApplyMaterials(skeletonData, multiplyMaterialTemplate, screenMaterialTemplate, additiveMaterialTemplate, applyAdditiveMaterial);
	}

	public static void ApplyMaterials(SkeletonData skeletonData, Material multiplyTemplate, Material screenTemplate, Material additiveTemplate, bool includeAdditiveSlots)
	{
		if (skeletonData == null)
		{
			throw new ArgumentNullException("skeletonData");
		}
		using AtlasMaterialCache atlasMaterialCache = new AtlasMaterialCache();
		List<Skin.SkinEntry> list = new List<Skin.SkinEntry>();
		SlotData[] items = skeletonData.Slots.Items;
		int i = 0;
		for (int count = skeletonData.Slots.Count; i < count; i++)
		{
			SlotData slotData = items[i];
			if (slotData.BlendMode == BlendMode.Normal || (!includeAdditiveSlots && slotData.BlendMode == BlendMode.Additive))
			{
				continue;
			}
			list.Clear();
			foreach (Skin skin in skeletonData.Skins)
			{
				skin.GetAttachments(i, list);
			}
			Material material = null;
			switch (slotData.BlendMode)
			{
			case BlendMode.Multiply:
				material = multiplyTemplate;
				break;
			case BlendMode.Screen:
				material = screenTemplate;
				break;
			case BlendMode.Additive:
				material = additiveTemplate;
				break;
			}
			if (material == null)
			{
				continue;
			}
			foreach (Skin.SkinEntry item in list)
			{
				if (item.Attachment is IHasRendererObject hasRendererObject)
				{
					hasRendererObject.RendererObject = atlasMaterialCache.CloneAtlasRegionWithMaterial((AtlasRegion)hasRendererObject.RendererObject, material);
				}
			}
		}
	}
}
