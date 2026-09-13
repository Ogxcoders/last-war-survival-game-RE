using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class ParticleSystemExtensions
{
	public static void SortForRendering(this List<ParticleSystem> self, Transform transform, bool sortByMaterial)
	{
		self.Sort(delegate(ParticleSystem a, ParticleSystem b)
		{
			Transform transform2 = transform;
			ParticleSystemRenderer component = a.GetComponent<ParticleSystemRenderer>();
			ParticleSystemRenderer component2 = b.GetComponent<ParticleSystemRenderer>();
			Material material = component.sharedMaterial ?? component.trailMaterial;
			Material material2 = component2.sharedMaterial ?? component2.trailMaterial;
			if (!material && !material2)
			{
				return 0;
			}
			if (!material)
			{
				return -1;
			}
			if (!material2)
			{
				return 1;
			}
			if (sortByMaterial)
			{
				return material.GetInstanceID() - material2.GetInstanceID();
			}
			if (material.renderQueue != material2.renderQueue)
			{
				return material.renderQueue - material2.renderQueue;
			}
			if (component.sortingLayerID != component2.sortingLayerID)
			{
				return component.sortingLayerID - component2.sortingLayerID;
			}
			if (component.sortingOrder != component2.sortingOrder)
			{
				return component.sortingOrder - component2.sortingOrder;
			}
			Transform transform3 = a.transform;
			Transform transform4 = b.transform;
			float num = transform2.InverseTransformPoint(transform3.position).z + component.sortingFudge;
			float num2 = transform2.InverseTransformPoint(transform4.position).z + component2.sortingFudge;
			return (!Mathf.Approximately(num, num2)) ? ((int)Mathf.Sign(num2 - num)) : ((int)Mathf.Sign(GetIndex(self, a) - GetIndex(self, b)));
		});
	}

	private static int GetIndex(IList<ParticleSystem> list, UnityEngine.Object ps)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].GetInstanceID() == ps.GetInstanceID())
			{
				return i;
			}
		}
		return 0;
	}

	public static long GetMaterialHash(this ParticleSystem self, bool trail)
	{
		if (!self)
		{
			return 0L;
		}
		ParticleSystemRenderer component = self.GetComponent<ParticleSystemRenderer>();
		Material material = (trail ? component.trailMaterial : component.sharedMaterial);
		if (!material)
		{
			return 0L;
		}
		Texture2D texture2D = (trail ? null : self.GetTextureForSprite());
		return ((long)material.GetHashCode() << 32) + (texture2D ? texture2D.GetHashCode() : 0);
	}

	public static Texture2D GetTextureForSprite(this ParticleSystem self)
	{
		if (!self)
		{
			return null;
		}
		ParticleSystem.TextureSheetAnimationModule textureSheetAnimation = self.textureSheetAnimation;
		if (!textureSheetAnimation.enabled || textureSheetAnimation.mode != ParticleSystemAnimationMode.Sprites)
		{
			return null;
		}
		for (int i = 0; i < textureSheetAnimation.spriteCount; i++)
		{
			Sprite sprite = textureSheetAnimation.GetSprite(i);
			if ((bool)sprite)
			{
				return sprite.GetActualTexture();
			}
		}
		return null;
	}

	public static void Exec(this List<ParticleSystem> self, Action<ParticleSystem> action)
	{
		self.RemoveAll((ParticleSystem p) => !p);
		self.ForEach(action);
	}
}
