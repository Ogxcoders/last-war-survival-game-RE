using System;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity;

[Serializable]
public class BlendModeMaterials
{
	[Serializable]
	public class ReplacementMaterial
	{
		public string pageName;

		public Material material;
	}

	[SerializeField]
	[HideInInspector]
	protected bool requiresBlendModeMaterials;

	public bool applyAdditiveMaterial;

	public List<ReplacementMaterial> additiveMaterials = new List<ReplacementMaterial>();

	public List<ReplacementMaterial> multiplyMaterials = new List<ReplacementMaterial>();

	public List<ReplacementMaterial> screenMaterials = new List<ReplacementMaterial>();

	public bool RequiresBlendModeMaterials
	{
		get
		{
			return requiresBlendModeMaterials;
		}
		set
		{
			requiresBlendModeMaterials = value;
		}
	}

	public BlendMode BlendModeForMaterial(Material material)
	{
		foreach (ReplacementMaterial multiplyMaterial in multiplyMaterials)
		{
			if (multiplyMaterial.material == material)
			{
				return BlendMode.Multiply;
			}
		}
		foreach (ReplacementMaterial additiveMaterial in additiveMaterials)
		{
			if (additiveMaterial.material == material)
			{
				return BlendMode.Additive;
			}
		}
		foreach (ReplacementMaterial screenMaterial in screenMaterials)
		{
			if (screenMaterial.material == material)
			{
				return BlendMode.Screen;
			}
		}
		return BlendMode.Normal;
	}

	public void ApplyMaterials(SkeletonData skeletonData)
	{
		if (skeletonData == null)
		{
			throw new ArgumentNullException("skeletonData");
		}
		if (!requiresBlendModeMaterials)
		{
			return;
		}
		List<Skin.SkinEntry> list = new List<Skin.SkinEntry>();
		SlotData[] items = skeletonData.Slots.Items;
		int i = 0;
		for (int count = skeletonData.Slots.Count; i < count; i++)
		{
			SlotData slotData = items[i];
			if (slotData.BlendMode == BlendMode.Normal || (!applyAdditiveMaterial && slotData.BlendMode == BlendMode.Additive))
			{
				continue;
			}
			List<ReplacementMaterial> list2 = null;
			switch (slotData.BlendMode)
			{
			case BlendMode.Multiply:
				list2 = multiplyMaterials;
				break;
			case BlendMode.Screen:
				list2 = screenMaterials;
				break;
			case BlendMode.Additive:
				list2 = additiveMaterials;
				break;
			}
			if (list2 == null)
			{
				continue;
			}
			list.Clear();
			foreach (Skin skin in skeletonData.Skins)
			{
				skin.GetAttachments(i, list);
			}
			foreach (Skin.SkinEntry item in list)
			{
				if (item.Attachment is IHasRendererObject hasRendererObject)
				{
					hasRendererObject.RendererObject = CloneAtlasRegionWithMaterial((AtlasRegion)hasRendererObject.RendererObject, list2);
				}
			}
		}
	}

	protected AtlasRegion CloneAtlasRegionWithMaterial(AtlasRegion originalRegion, List<ReplacementMaterial> replacementMaterials)
	{
		AtlasRegion atlasRegion = originalRegion.Clone();
		Material rendererObject = null;
		foreach (ReplacementMaterial replacementMaterial in replacementMaterials)
		{
			if (replacementMaterial.pageName == originalRegion.page.name)
			{
				rendererObject = replacementMaterial.material;
				break;
			}
		}
		AtlasPage atlasPage = originalRegion.page.Clone();
		atlasPage.rendererObject = rendererObject;
		atlasRegion.page = atlasPage;
		return atlasRegion;
	}
}
