using System;
using System.Collections.Generic;

namespace Spine;

public abstract class SkeletonLoader
{
	protected class LinkedMesh
	{
		internal string parent;

		internal string skin;

		internal int slotIndex;

		internal MeshAttachment mesh;

		internal bool inheritDeform;

		public LinkedMesh(MeshAttachment mesh, string skin, int slotIndex, string parent, bool inheritDeform)
		{
			this.mesh = mesh;
			this.skin = skin;
			this.slotIndex = slotIndex;
			this.parent = parent;
			this.inheritDeform = inheritDeform;
		}
	}

	protected readonly AttachmentLoader attachmentLoader;

	protected float scale = 1f;

	protected readonly List<LinkedMesh> linkedMeshes = new List<LinkedMesh>();

	public float Scale
	{
		get
		{
			return scale;
		}
		set
		{
			if (scale == 0f)
			{
				throw new ArgumentNullException("scale", "scale cannot be 0.");
			}
			scale = value;
		}
	}

	public SkeletonLoader(params Atlas[] atlasArray)
	{
		attachmentLoader = new AtlasAttachmentLoader(atlasArray);
	}

	public SkeletonLoader(AttachmentLoader attachmentLoader)
	{
		if (attachmentLoader == null)
		{
			throw new ArgumentNullException("attachmentLoader", "attachmentLoader cannot be null.");
		}
		this.attachmentLoader = attachmentLoader;
	}

	public abstract SkeletonData ReadSkeletonData(string path);
}
