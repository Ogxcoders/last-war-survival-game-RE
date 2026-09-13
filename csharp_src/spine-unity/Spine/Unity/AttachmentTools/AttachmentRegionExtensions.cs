using System;
using UnityEngine;

namespace Spine.Unity.AttachmentTools;

public static class AttachmentRegionExtensions
{
	public static void SetRegion(this Attachment attachment, AtlasRegion region, bool updateOffset = true)
	{
		if (attachment is RegionAttachment attachment2)
		{
			attachment2.SetRegion(region, updateOffset);
		}
		if (attachment is MeshAttachment attachment3)
		{
			attachment3.SetRegion(region, updateOffset);
		}
	}

	public static void SetRegion(this RegionAttachment attachment, AtlasRegion region, bool updateOffset = true)
	{
		if (region == null)
		{
			throw new ArgumentNullException("region");
		}
		attachment.RendererObject = region;
		attachment.SetUVs(region.u, region.v, region.u2, region.v2, region.degrees);
		attachment.RegionOffsetX = region.offsetX;
		attachment.RegionOffsetY = region.offsetY;
		attachment.RegionWidth = region.width;
		attachment.RegionHeight = region.height;
		attachment.RegionOriginalWidth = region.originalWidth;
		attachment.RegionOriginalHeight = region.originalHeight;
		if (updateOffset)
		{
			attachment.UpdateOffset();
		}
	}

	public static void SetRegion(this MeshAttachment attachment, AtlasRegion region, bool updateUVs = true)
	{
		if (region == null)
		{
			throw new ArgumentNullException("region");
		}
		attachment.RendererObject = region;
		attachment.RegionU = region.u;
		attachment.RegionV = region.v;
		attachment.RegionU2 = region.u2;
		attachment.RegionV2 = region.v2;
		attachment.RegionDegrees = region.degrees;
		attachment.RegionOffsetX = region.offsetX;
		attachment.RegionOffsetY = region.offsetY;
		attachment.RegionWidth = region.width;
		attachment.RegionHeight = region.height;
		attachment.RegionOriginalWidth = region.originalWidth;
		attachment.RegionOriginalHeight = region.originalHeight;
		if (updateUVs)
		{
			attachment.UpdateUVs();
		}
	}

	public static RegionAttachment ToRegionAttachment(this Sprite sprite, Material material, float rotation = 0f)
	{
		return sprite.ToRegionAttachment(material.ToSpineAtlasPage(), rotation);
	}

	public static RegionAttachment ToRegionAttachment(this Sprite sprite, AtlasPage page, float rotation = 0f)
	{
		if (sprite == null)
		{
			throw new ArgumentNullException("sprite");
		}
		if (page == null)
		{
			throw new ArgumentNullException("page");
		}
		return sprite.ToAtlasRegion(page).ToRegionAttachment(scale: 1f / sprite.pixelsPerUnit, attachmentName: sprite.name, rotation: rotation);
	}

	public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Shader shader, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, Material materialPropertySource = null, float rotation = 0f)
	{
		if (sprite == null)
		{
			throw new ArgumentNullException("sprite");
		}
		if (shader == null)
		{
			throw new ArgumentNullException("shader");
		}
		return sprite.ToAtlasRegionPMAClone(shader, textureFormat, mipmaps, materialPropertySource).ToRegionAttachment(scale: 1f / sprite.pixelsPerUnit, attachmentName: sprite.name, rotation: rotation);
	}

	public static RegionAttachment ToRegionAttachmentPMAClone(this Sprite sprite, Material materialPropertySource, TextureFormat textureFormat = TextureFormat.RGBA32, bool mipmaps = false, float rotation = 0f)
	{
		return sprite.ToRegionAttachmentPMAClone(materialPropertySource.shader, textureFormat, mipmaps, materialPropertySource, rotation);
	}

	public static RegionAttachment ToRegionAttachment(this AtlasRegion region, string attachmentName, float scale = 0.01f, float rotation = 0f)
	{
		if (string.IsNullOrEmpty(attachmentName))
		{
			throw new ArgumentException("attachmentName can't be null or empty.", "attachmentName");
		}
		if (region == null)
		{
			throw new ArgumentNullException("region");
		}
		RegionAttachment regionAttachment = new RegionAttachment(attachmentName);
		regionAttachment.RendererObject = region;
		regionAttachment.SetUVs(region.u, region.v, region.u2, region.v2, region.degrees);
		regionAttachment.RegionOffsetX = region.offsetX;
		regionAttachment.RegionOffsetY = region.offsetY;
		regionAttachment.RegionWidth = region.width;
		regionAttachment.RegionHeight = region.height;
		regionAttachment.RegionOriginalWidth = region.originalWidth;
		regionAttachment.RegionOriginalHeight = region.originalHeight;
		regionAttachment.Path = region.name;
		regionAttachment.ScaleX = 1f;
		regionAttachment.ScaleY = 1f;
		regionAttachment.Rotation = rotation;
		regionAttachment.R = 1f;
		regionAttachment.G = 1f;
		regionAttachment.B = 1f;
		regionAttachment.A = 1f;
		regionAttachment.Width = regionAttachment.RegionOriginalWidth * scale;
		regionAttachment.Height = regionAttachment.RegionOriginalHeight * scale;
		regionAttachment.SetColor(Color.white);
		regionAttachment.UpdateOffset();
		return regionAttachment;
	}

	public static void SetScale(this RegionAttachment regionAttachment, Vector2 scale)
	{
		regionAttachment.ScaleX = scale.x;
		regionAttachment.ScaleY = scale.y;
	}

	public static void SetScale(this RegionAttachment regionAttachment, float x, float y)
	{
		regionAttachment.ScaleX = x;
		regionAttachment.ScaleY = y;
	}

	public static void SetPositionOffset(this RegionAttachment regionAttachment, Vector2 offset)
	{
		regionAttachment.X = offset.x;
		regionAttachment.Y = offset.y;
	}

	public static void SetPositionOffset(this RegionAttachment regionAttachment, float x, float y)
	{
		regionAttachment.X = x;
		regionAttachment.Y = y;
	}

	public static void SetRotation(this RegionAttachment regionAttachment, float rotation)
	{
		regionAttachment.Rotation = rotation;
	}
}
