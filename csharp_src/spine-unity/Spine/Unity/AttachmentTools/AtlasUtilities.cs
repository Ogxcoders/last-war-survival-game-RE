using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Spine.Unity.AttachmentTools;

public static class AtlasUtilities
{
	private struct IntAndAtlasRegionKey
	{
		private int i;

		private AtlasRegion region;

		public IntAndAtlasRegionKey(int i, AtlasRegion region)
		{
			this.i = i;
			this.region = region;
		}

		public override int GetHashCode()
		{
			return (i.GetHashCode() * 23) ^ region.GetHashCode();
		}
	}

	internal const TextureFormat SpineTextureFormat = TextureFormat.RGBA32;

	internal const float DefaultMipmapBias = -0.5f;

	internal const bool UseMipMaps = false;

	internal const float DefaultScale = 0.01f;

	private const int NonrenderingRegion = -1;

	private static readonly Dictionary<AtlasRegion, int> existingRegions = new Dictionary<AtlasRegion, int>();

	private static readonly List<int> regionIndices = new List<int>();

	private static readonly List<AtlasRegion> originalRegions = new List<AtlasRegion>();

	private static readonly List<AtlasRegion> repackedRegions = new List<AtlasRegion>();

	private static List<Texture2D>[] texturesToPackAtParam = new List<Texture2D>[1];

	private static List<Attachment> inoutAttachments = new List<Attachment>();

	private static Dictionary<IntAndAtlasRegionKey, Texture2D> CachedRegionTextures = new Dictionary<IntAndAtlasRegionKey, Texture2D>();

	private static List<Texture2D> CachedRegionTexturesList = new List<Texture2D>();

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void Init()
	{
		ClearCache();
	}

	public static AtlasRegion ToAtlasRegion(this Texture2D t, Material materialPropertySource, float scale = 0.01f)
	{
		return t.ToAtlasRegion(materialPropertySource.shader, scale, materialPropertySource);
	}

	public static AtlasRegion ToAtlasRegion(this Texture2D t, Shader shader, float scale = 0.01f, Material materialPropertySource = null)
	{
		Material material = new Material(shader);
		if (materialPropertySource != null)
		{
			material.CopyPropertiesFromMaterial(materialPropertySource);
			material.shaderKeywords = materialPropertySource.shaderKeywords;
		}
		material.mainTexture = t;
		AtlasPage page = material.ToSpineAtlasPage();
		float num = t.width;
		float num2 = t.height;
		AtlasRegion obj = new AtlasRegion
		{
			name = t.name
		};
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(num, num2) * scale;
		obj.width = (int)num;
		obj.originalWidth = (int)num;
		obj.height = (int)num2;
		obj.originalHeight = (int)num2;
		obj.offsetX = num * (0.5f - InverseLerp(zero.x, vector.x, 0f));
		obj.offsetY = num2 * (0.5f - InverseLerp(zero.y, vector.y, 0f));
		obj.u = 0f;
		obj.v = 1f;
		obj.u2 = 1f;
		obj.v2 = 0f;
		obj.x = 0;
		obj.y = 0;
		obj.page = page;
		return obj;
	}

	public static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
	{
		return t.ToAtlasRegionPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource);
	}

	public static AtlasRegion ToAtlasRegionPMAClone(this Texture2D t, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null)
	{
		Material material = new Material(shader);
		if (materialPropertySource != null)
		{
			material.CopyPropertiesFromMaterial(materialPropertySource);
			material.shaderKeywords = materialPropertySource.shaderKeywords;
		}
		Texture2D clone = t.GetClone(textureFormat, mipmaps, linear: false, applyPMA: true);
		clone.name = t.name + "-pma-";
		material.name = t.name + shader.name;
		material.mainTexture = clone;
		AtlasPage page = material.ToSpineAtlasPage();
		AtlasRegion atlasRegion = clone.ToAtlasRegion(shader);
		atlasRegion.page = page;
		return atlasRegion;
	}

	public static AtlasPage ToSpineAtlasPage(this Material m)
	{
		AtlasPage atlasPage = new AtlasPage
		{
			rendererObject = m,
			name = m.name
		};
		Texture mainTexture = m.mainTexture;
		if (mainTexture != null)
		{
			atlasPage.width = mainTexture.width;
			atlasPage.height = mainTexture.height;
		}
		return atlasPage;
	}

	public static AtlasRegion ToAtlasRegion(this Sprite s, AtlasPage page)
	{
		if (page == null)
		{
			throw new ArgumentNullException("page", "page cannot be null. AtlasPage determines which texture region belongs and how it should be rendered. You can use material.ToSpineAtlasPage() to get a shareable AtlasPage from a Material, or use the sprite.ToAtlasRegion(material) overload.");
		}
		AtlasRegion atlasRegion = s.ToAtlasRegion();
		atlasRegion.page = page;
		return atlasRegion;
	}

	public static AtlasRegion ToAtlasRegion(this Sprite s, Material material)
	{
		AtlasRegion atlasRegion = s.ToAtlasRegion();
		atlasRegion.page = material.ToSpineAtlasPage();
		return atlasRegion;
	}

	public static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false)
	{
		return s.ToAtlasRegionPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource);
	}

	public static AtlasRegion ToAtlasRegionPMAClone(this Sprite s, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null)
	{
		Material material = new Material(shader);
		if (materialPropertySource != null)
		{
			material.CopyPropertiesFromMaterial(materialPropertySource);
			material.shaderKeywords = materialPropertySource.shaderKeywords;
		}
		Texture2D texture2D = s.ToTexture(textureFormat, mipmaps, linear: false, applyPMA: true);
		texture2D.name = s.name + "-pma-";
		material.name = texture2D.name + shader.name;
		material.mainTexture = texture2D;
		AtlasPage page = material.ToSpineAtlasPage();
		AtlasRegion atlasRegion = s.ToAtlasRegion(isolatedTexture: true);
		atlasRegion.page = page;
		return atlasRegion;
	}

	internal static AtlasRegion ToAtlasRegion(this Sprite s, bool isolatedTexture = false)
	{
		AtlasRegion atlasRegion = new AtlasRegion();
		atlasRegion.name = s.name;
		atlasRegion.index = -1;
		atlasRegion.degrees = ((s.packed && s.packingRotation != SpritePackingRotation.None) ? 90 : 0);
		Bounds bounds = s.bounds;
		Vector2 vector = bounds.min;
		Vector2 vector2 = bounds.max;
		Rect rect = s.rect.SpineUnityFlipRect(s.texture.height);
		atlasRegion.width = (int)rect.width;
		atlasRegion.originalWidth = (int)rect.width;
		atlasRegion.height = (int)rect.height;
		atlasRegion.originalHeight = (int)rect.height;
		atlasRegion.offsetX = rect.width * (0.5f - InverseLerp(vector.x, vector2.x, 0f));
		atlasRegion.offsetY = rect.height * (0.5f - InverseLerp(vector.y, vector2.y, 0f));
		if (isolatedTexture)
		{
			atlasRegion.u = 0f;
			atlasRegion.v = 1f;
			atlasRegion.u2 = 1f;
			atlasRegion.v2 = 0f;
			atlasRegion.x = 0;
			atlasRegion.y = 0;
		}
		else
		{
			Texture2D texture = s.texture;
			Rect rect2 = TextureRectToUVRect(s.textureRect, texture.width, texture.height);
			atlasRegion.u = rect2.xMin;
			atlasRegion.v = rect2.yMax;
			atlasRegion.u2 = rect2.xMax;
			atlasRegion.v2 = rect2.yMin;
			atlasRegion.x = (int)rect.x;
			atlasRegion.y = (int)rect.y;
		}
		return atlasRegion;
	}

	public static void GetRepackedAttachments(List<Attachment> sourceAttachments, List<Attachment> outputAttachments, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, string newAssetName = "Repacked Attachments", bool clearCache = false, bool useOriginalNonrenderables = true, int[] additionalTexturePropertyIDsToCopy = null, Texture2D[] additionalOutputTextures = null, TextureFormat[] additionalTextureFormats = null, bool[] additionalTextureIsLinear = null)
	{
		Shader shader = ((materialPropertySource == null) ? Shader.Find("Spine/Skeleton") : materialPropertySource.shader);
		GetRepackedAttachments(sourceAttachments, outputAttachments, shader, out outputMaterial, out outputTexture, maxAtlasSize, padding, textureFormat, mipmaps, newAssetName, materialPropertySource, clearCache, useOriginalNonrenderables, additionalTexturePropertyIDsToCopy, additionalOutputTextures, additionalTextureFormats, additionalTextureIsLinear);
	}

	public static void GetRepackedAttachments(List<Attachment> sourceAttachments, List<Attachment> outputAttachments, Shader shader, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, string newAssetName = "Repacked Attachments", Material materialPropertySource = null, bool clearCache = false, bool useOriginalNonrenderables = true, int[] additionalTexturePropertyIDsToCopy = null, Texture2D[] additionalOutputTextures = null, TextureFormat[] additionalTextureFormats = null, bool[] additionalTextureIsLinear = null)
	{
		if (sourceAttachments == null)
		{
			throw new ArgumentNullException("sourceAttachments");
		}
		if (outputAttachments == null)
		{
			throw new ArgumentNullException("outputAttachments");
		}
		outputTexture = null;
		if (additionalTexturePropertyIDsToCopy != null && additionalTextureIsLinear == null)
		{
			additionalTextureIsLinear = new bool[additionalTexturePropertyIDsToCopy.Length];
			for (int i = 0; i < additionalTextureIsLinear.Length; i++)
			{
				additionalTextureIsLinear[i] = true;
			}
		}
		existingRegions.Clear();
		regionIndices.Clear();
		int num = 1 + ((additionalTexturePropertyIDsToCopy != null) ? additionalTexturePropertyIDsToCopy.Length : 0);
		additionalOutputTextures = ((additionalTexturePropertyIDsToCopy == null) ? null : new Texture2D[additionalTexturePropertyIDsToCopy.Length]);
		if (texturesToPackAtParam.Length < num)
		{
			Array.Resize(ref texturesToPackAtParam, num);
		}
		for (int j = 0; j < num; j++)
		{
			if (texturesToPackAtParam[j] != null)
			{
				texturesToPackAtParam[j].Clear();
			}
			else
			{
				texturesToPackAtParam[j] = new List<Texture2D>();
			}
		}
		originalRegions.Clear();
		if (sourceAttachments != outputAttachments)
		{
			outputAttachments.Clear();
			outputAttachments.AddRange(sourceAttachments);
		}
		int num2 = 0;
		int k = 0;
		for (int count = sourceAttachments.Count; k < count; k++)
		{
			Attachment attachment = sourceAttachments[k];
			if (attachment is IHasRendererObject)
			{
				Attachment attachment2 = ((attachment is MeshAttachment meshAttachment) ? meshAttachment.NewLinkedMesh() : attachment.Copy());
				AtlasRegion atlasRegion = ((IHasRendererObject)attachment2).RendererObject as AtlasRegion;
				if (existingRegions.TryGetValue(atlasRegion, out var value))
				{
					regionIndices.Add(value);
				}
				else
				{
					originalRegions.Add(atlasRegion);
					for (int l = 0; l < num; l++)
					{
						Texture2D item = ((l == 0) ? atlasRegion.ToTexture(textureFormat, mipmaps) : atlasRegion.ToTexture((additionalTextureFormats != null && l - 1 < additionalTextureFormats.Length) ? additionalTextureFormats[l - 1] : textureFormat, mipmaps, additionalTexturePropertyIDsToCopy[l - 1], additionalTextureIsLinear[l - 1]));
						texturesToPackAtParam[l].Add(item);
					}
					existingRegions.Add(atlasRegion, num2);
					regionIndices.Add(num2);
					num2++;
				}
				outputAttachments[k] = attachment2;
			}
			else
			{
				outputAttachments[k] = (useOriginalNonrenderables ? attachment : attachment.Copy());
				regionIndices.Add(-1);
			}
		}
		Material material = new Material(shader);
		if (materialPropertySource != null)
		{
			material.CopyPropertiesFromMaterial(materialPropertySource);
			material.shaderKeywords = materialPropertySource.shaderKeywords;
		}
		material.name = newAssetName;
		Rect[] array = null;
		for (int m = 0; m < num; m++)
		{
			Texture2D texture2D = new Texture2D(maxAtlasSize, maxAtlasSize, (m > 0 && additionalTextureFormats != null && m - 1 < additionalTextureFormats.Length) ? additionalTextureFormats[m - 1] : textureFormat, mipmaps, m > 0 && additionalTextureIsLinear[m - 1]);
			texture2D.mipMapBias = -0.5f;
			List<Texture2D> list = texturesToPackAtParam[m];
			if (list.Count > 0)
			{
				Texture2D source = list[0];
				texture2D.CopyTextureAttributesFrom(source);
			}
			texture2D.name = newAssetName;
			Rect[] array2 = texture2D.PackTextures(list.ToArray(), padding, maxAtlasSize);
			if (m == 0)
			{
				array = array2;
				material.mainTexture = texture2D;
				outputTexture = texture2D;
			}
			else
			{
				material.SetTexture(additionalTexturePropertyIDsToCopy[m - 1], texture2D);
				additionalOutputTextures[m - 1] = texture2D;
			}
		}
		AtlasPage atlasPage = material.ToSpineAtlasPage();
		atlasPage.name = newAssetName;
		repackedRegions.Clear();
		int n = 0;
		for (int count2 = originalRegions.Count; n < count2; n++)
		{
			AtlasRegion referenceRegion = originalRegions[n];
			AtlasRegion item2 = UVRectToAtlasRegion(array[n], referenceRegion, atlasPage);
			repackedRegions.Add(item2);
		}
		int num3 = 0;
		for (int count3 = outputAttachments.Count; num3 < count3; num3++)
		{
			Attachment attachment3 = outputAttachments[num3];
			if (attachment3 is IHasRendererObject)
			{
				attachment3.SetRegion(repackedRegions[regionIndices[num3]]);
			}
		}
		if (clearCache)
		{
			ClearCache();
		}
		outputMaterial = material;
	}

	public static Skin GetRepackedSkin(this Skin o, string newName, Material materialPropertySource, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool useOriginalNonrenderables = true, bool clearCache = false, int[] additionalTexturePropertyIDsToCopy = null, Texture2D[] additionalOutputTextures = null, TextureFormat[] additionalTextureFormats = null, bool[] additionalTextureIsLinear = null)
	{
		return o.GetRepackedSkin(newName, materialPropertySource.shader, out outputMaterial, out outputTexture, maxAtlasSize, padding, textureFormat, mipmaps, materialPropertySource, clearCache, useOriginalNonrenderables, additionalTexturePropertyIDsToCopy, additionalOutputTextures, additionalTextureFormats, additionalTextureIsLinear);
	}

	public static Skin GetRepackedSkin(this Skin o, string newName, Shader shader, out Material outputMaterial, out Texture2D outputTexture, int maxAtlasSize = 1024, int padding = 2, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null, bool clearCache = false, bool useOriginalNonrenderables = true, int[] additionalTexturePropertyIDsToCopy = null, Texture2D[] additionalOutputTextures = null, TextureFormat[] additionalTextureFormats = null, bool[] additionalTextureIsLinear = null)
	{
		outputTexture = null;
		if (o == null)
		{
			throw new NullReferenceException("Skin was null");
		}
		_ = o.Attachments;
		Skin skin = new Skin(newName);
		skin.Bones.AddRange(o.Bones);
		skin.Constraints.AddRange(o.Constraints);
		inoutAttachments.Clear();
		foreach (Skin.SkinEntry attachment2 in o.Attachments)
		{
			inoutAttachments.Add(attachment2.Attachment);
		}
		GetRepackedAttachments(inoutAttachments, inoutAttachments, materialPropertySource, out outputMaterial, out outputTexture, maxAtlasSize, padding, textureFormat, mipmaps, newName, clearCache, useOriginalNonrenderables, additionalTexturePropertyIDsToCopy, additionalOutputTextures, additionalTextureFormats, additionalTextureIsLinear);
		int num = 0;
		foreach (Skin.SkinEntry attachment3 in o.Attachments)
		{
			Attachment attachment = inoutAttachments[num++];
			skin.SetAttachment(attachment3.SlotIndex, attachment3.Name, attachment);
		}
		return skin;
	}

	public static Sprite ToSprite(this AtlasRegion ar, float pixelsPerUnit = 100f)
	{
		return Sprite.Create(ar.GetMainTexture(), ar.GetUnityRect(), new Vector2(0.5f, 0.5f), pixelsPerUnit);
	}

	public static void ClearCache()
	{
		foreach (Texture2D cachedRegionTextures in CachedRegionTexturesList)
		{
			UnityEngine.Object.Destroy(cachedRegionTextures);
		}
		CachedRegionTextures.Clear();
		CachedRegionTexturesList.Clear();
	}

	public static Texture2D ToTexture(this AtlasRegion ar, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, int texturePropertyId = 0, bool linear = false, bool applyPMA = false)
	{
		IntAndAtlasRegionKey key = new IntAndAtlasRegionKey(texturePropertyId, ar);
		CachedRegionTextures.TryGetValue(key, out var value);
		if (value == null)
		{
			Texture2D source = ((texturePropertyId == 0) ? ar.GetMainTexture() : ar.GetTexture(texturePropertyId));
			Rect unityRect = ar.GetUnityRect();
			int width = (int)unityRect.width;
			int height = (int)unityRect.height;
			value = new Texture2D(width, height, textureFormat, mipmaps, linear)
			{
				name = ar.name
			};
			value.CopyTextureAttributesFrom(source);
			if (applyPMA)
			{
				CopyTextureApplyPMA(source, unityRect, value);
			}
			else
			{
				CopyTexture(source, unityRect, value);
			}
			CachedRegionTextures.Add(key, value);
			CachedRegionTexturesList.Add(value);
		}
		return value;
	}

	private static Texture2D ToTexture(this Sprite s, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool linear = false, bool applyPMA = false)
	{
		Texture2D texture = s.texture;
		Rect sourceRect = ((s.packed && s.packingMode != SpritePackingMode.Rectangle) ? new Rect
		{
			xMin = Math.Min(s.uv[0].x, s.uv[1].x) * (float)texture.width,
			xMax = Math.Max(s.uv[0].x, s.uv[1].x) * (float)texture.width,
			yMin = Math.Min(s.uv[0].y, s.uv[2].y) * (float)texture.height,
			yMax = Math.Max(s.uv[0].y, s.uv[2].y) * (float)texture.height
		} : s.textureRect);
		Texture2D texture2D = new Texture2D((int)sourceRect.width, (int)sourceRect.height, textureFormat, mipmaps, linear);
		texture2D.CopyTextureAttributesFrom(texture);
		if (applyPMA)
		{
			CopyTextureApplyPMA(texture, sourceRect, texture2D);
		}
		else
		{
			CopyTexture(texture, sourceRect, texture2D);
		}
		return texture2D;
	}

	private static Texture2D GetClone(this Texture2D t, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, bool linear = false, bool applyPMA = false)
	{
		Texture2D texture2D = new Texture2D(t.width, t.height, textureFormat, mipmaps, linear);
		texture2D.CopyTextureAttributesFrom(t);
		if (applyPMA)
		{
			CopyTextureApplyPMA(t, new Rect(0f, 0f, t.width, t.height), texture2D);
		}
		else
		{
			CopyTexture(t, new Rect(0f, 0f, t.width, t.height), texture2D);
		}
		return texture2D;
	}

	private static void CopyTexture(Texture2D source, Rect sourceRect, Texture2D destination)
	{
		if (SystemInfo.copyTextureSupport == CopyTextureSupport.None)
		{
			Color[] pixels = source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height);
			destination.SetPixels(pixels);
			destination.Apply();
		}
		else
		{
			Graphics.CopyTexture(source, 0, 0, (int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height, destination, 0, 0, 0, 0);
		}
	}

	private static void CopyTextureApplyPMA(Texture2D source, Rect sourceRect, Texture2D destination)
	{
		Color[] pixels = source.GetPixels((int)sourceRect.x, (int)sourceRect.y, (int)sourceRect.width, (int)sourceRect.height);
		int i = 0;
		for (int num = pixels.Length; i < num; i++)
		{
			Color color = pixels[i];
			float a = color.a;
			color.r *= a;
			color.g *= a;
			color.b *= a;
			pixels[i] = color;
		}
		destination.SetPixels(pixels);
		destination.Apply();
	}

	private static bool IsRenderable(Attachment a)
	{
		return a is IHasRendererObject;
	}

	private static Rect SpineUnityFlipRect(this Rect rect, int textureHeight)
	{
		rect.y = (float)textureHeight - rect.y - rect.height;
		return rect;
	}

	private static Rect GetUnityRect(this AtlasRegion region)
	{
		return region.GetSpineAtlasRect().SpineUnityFlipRect(region.page.height);
	}

	private static Rect GetUnityRect(this AtlasRegion region, int textureHeight)
	{
		return region.GetSpineAtlasRect().SpineUnityFlipRect(textureHeight);
	}

	private static Rect GetSpineAtlasRect(this AtlasRegion region, bool includeRotate = true)
	{
		if (includeRotate && (region.degrees == 90 || region.degrees == 270))
		{
			return new Rect(region.x, region.y, region.height, region.width);
		}
		return new Rect(region.x, region.y, region.width, region.height);
	}

	private static Rect UVRectToTextureRect(Rect uvRect, int texWidth, int texHeight)
	{
		uvRect.x *= texWidth;
		uvRect.width *= texWidth;
		uvRect.y *= texHeight;
		uvRect.height *= texHeight;
		return uvRect;
	}

	private static Rect TextureRectToUVRect(Rect textureRect, int texWidth, int texHeight)
	{
		textureRect.x = Mathf.InverseLerp(0f, texWidth, textureRect.x);
		textureRect.y = Mathf.InverseLerp(0f, texHeight, textureRect.y);
		textureRect.width = Mathf.InverseLerp(0f, texWidth, textureRect.width);
		textureRect.height = Mathf.InverseLerp(0f, texHeight, textureRect.height);
		return textureRect;
	}

	private static AtlasRegion UVRectToAtlasRegion(Rect uvRect, AtlasRegion referenceRegion, AtlasPage page)
	{
		Rect rect = UVRectToTextureRect(uvRect, page.width, page.height).SpineUnityFlipRect(page.height);
		int x = (int)rect.x;
		int y = (int)rect.y;
		int num;
		int num2;
		if (referenceRegion.degrees == 90 || referenceRegion.degrees == 270)
		{
			num = (int)rect.height;
			num2 = (int)rect.width;
		}
		else
		{
			num = (int)rect.width;
			num2 = (int)rect.height;
		}
		int originalWidth = Mathf.RoundToInt((float)num * ((float)referenceRegion.originalWidth / (float)referenceRegion.width));
		int originalHeight = Mathf.RoundToInt((float)num2 * ((float)referenceRegion.originalHeight / (float)referenceRegion.height));
		int num3 = Mathf.RoundToInt(referenceRegion.offsetX * ((float)num / (float)referenceRegion.width));
		int num4 = Mathf.RoundToInt(referenceRegion.offsetY * ((float)num2 / (float)referenceRegion.height));
		if (referenceRegion.degrees == 270)
		{
			num = (int)rect.width;
			num2 = (int)rect.height;
		}
		float xMin = uvRect.xMin;
		float xMax = uvRect.xMax;
		float yMax = uvRect.yMax;
		float yMin = uvRect.yMin;
		return new AtlasRegion
		{
			page = page,
			name = referenceRegion.name,
			u = xMin,
			u2 = xMax,
			v = yMax,
			v2 = yMin,
			index = -1,
			width = num,
			originalWidth = originalWidth,
			height = num2,
			originalHeight = originalHeight,
			offsetX = num3,
			offsetY = num4,
			x = x,
			y = y,
			rotate = referenceRegion.rotate,
			degrees = referenceRegion.degrees
		};
	}

	private static Texture2D GetMainTexture(this AtlasRegion region)
	{
		return (region.page.rendererObject as Material).mainTexture as Texture2D;
	}

	private static Texture2D GetTexture(this AtlasRegion region, string texturePropertyName)
	{
		return (region.page.rendererObject as Material).GetTexture(texturePropertyName) as Texture2D;
	}

	private static Texture2D GetTexture(this AtlasRegion region, int texturePropertyId)
	{
		return (region.page.rendererObject as Material).GetTexture(texturePropertyId) as Texture2D;
	}

	private static void CopyTextureAttributesFrom(this Texture2D destination, Texture2D source)
	{
		destination.filterMode = source.filterMode;
		destination.anisoLevel = source.anisoLevel;
		destination.wrapModeU = source.wrapModeU;
		destination.wrapModeV = source.wrapModeV;
		destination.wrapModeW = source.wrapModeW;
	}

	private static float InverseLerp(float a, float b, float value)
	{
		return (value - a) / (b - a);
	}
}
