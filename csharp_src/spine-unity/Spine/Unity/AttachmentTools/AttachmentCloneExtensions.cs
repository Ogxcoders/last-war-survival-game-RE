using UnityEngine;

namespace Spine.Unity.AttachmentTools;

public static class AttachmentCloneExtensions
{
	public static Attachment GetRemappedClone(this Attachment o, Sprite sprite, Material sourceMaterial, bool premultiplyAlpha = true, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false, bool pivotShiftsMeshUVCoords = true, bool useOriginalRegionScale = false)
	{
		AtlasRegion atlasRegion = (premultiplyAlpha ? sprite.ToAtlasRegionPMAClone(sourceMaterial) : sprite.ToAtlasRegion(new Material(sourceMaterial)
		{
			mainTexture = sprite.texture
		}));
		if (!pivotShiftsMeshUVCoords && o is MeshAttachment)
		{
			atlasRegion.offsetX = 0f;
			atlasRegion.offsetY = 0f;
		}
		float scale = 1f / sprite.pixelsPerUnit;
		if (useOriginalRegionScale && o is RegionAttachment regionAttachment)
		{
			scale = regionAttachment.Width / regionAttachment.RegionOriginalWidth;
		}
		return o.GetRemappedClone(atlasRegion, cloneMeshAsLinked, useOriginalRegionSize, scale);
	}

	public static Attachment GetRemappedClone(this Attachment o, AtlasRegion atlasRegion, bool cloneMeshAsLinked = true, bool useOriginalRegionSize = false, float scale = 0.01f)
	{
		if (o is RegionAttachment regionAttachment)
		{
			RegionAttachment regionAttachment2 = (RegionAttachment)regionAttachment.Copy();
			regionAttachment2.SetRegion(atlasRegion, updateOffset: false);
			if (!useOriginalRegionSize)
			{
				regionAttachment2.Width = (float)atlasRegion.width * scale;
				regionAttachment2.Height = (float)atlasRegion.height * scale;
			}
			regionAttachment2.UpdateOffset();
			return regionAttachment2;
		}
		if (o is MeshAttachment meshAttachment)
		{
			MeshAttachment obj = (cloneMeshAsLinked ? meshAttachment.NewLinkedMesh() : ((MeshAttachment)meshAttachment.Copy()));
			obj.SetRegion(atlasRegion);
			return obj;
		}
		return o.Copy();
	}
}
