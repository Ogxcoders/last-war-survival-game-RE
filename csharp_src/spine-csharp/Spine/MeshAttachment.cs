using System;

namespace Spine;

public class MeshAttachment : VertexAttachment, IHasRendererObject
{
	internal float regionOffsetX;

	internal float regionOffsetY;

	internal float regionWidth;

	internal float regionHeight;

	internal float regionOriginalWidth;

	internal float regionOriginalHeight;

	private MeshAttachment parentMesh;

	internal float[] uvs;

	internal float[] regionUVs;

	internal int[] triangles;

	internal float r = 1f;

	internal float g = 1f;

	internal float b = 1f;

	internal float a = 1f;

	internal int hulllength;

	public int HullLength
	{
		get
		{
			return hulllength;
		}
		set
		{
			hulllength = value;
		}
	}

	public float[] RegionUVs
	{
		get
		{
			return regionUVs;
		}
		set
		{
			regionUVs = value;
		}
	}

	public float[] UVs
	{
		get
		{
			return uvs;
		}
		set
		{
			uvs = value;
		}
	}

	public int[] Triangles
	{
		get
		{
			return triangles;
		}
		set
		{
			triangles = value;
		}
	}

	public float R
	{
		get
		{
			return r;
		}
		set
		{
			r = value;
		}
	}

	public float G
	{
		get
		{
			return g;
		}
		set
		{
			g = value;
		}
	}

	public float B
	{
		get
		{
			return b;
		}
		set
		{
			b = value;
		}
	}

	public float A
	{
		get
		{
			return a;
		}
		set
		{
			a = value;
		}
	}

	public string Path { get; set; }

	public object RendererObject { get; set; }

	public float RegionU { get; set; }

	public float RegionV { get; set; }

	public float RegionU2 { get; set; }

	public float RegionV2 { get; set; }

	public int RegionDegrees { get; set; }

	public float RegionOffsetX
	{
		get
		{
			return regionOffsetX;
		}
		set
		{
			regionOffsetX = value;
		}
	}

	public float RegionOffsetY
	{
		get
		{
			return regionOffsetY;
		}
		set
		{
			regionOffsetY = value;
		}
	}

	public float RegionWidth
	{
		get
		{
			return regionWidth;
		}
		set
		{
			regionWidth = value;
		}
	}

	public float RegionHeight
	{
		get
		{
			return regionHeight;
		}
		set
		{
			regionHeight = value;
		}
	}

	public float RegionOriginalWidth
	{
		get
		{
			return regionOriginalWidth;
		}
		set
		{
			regionOriginalWidth = value;
		}
	}

	public float RegionOriginalHeight
	{
		get
		{
			return regionOriginalHeight;
		}
		set
		{
			regionOriginalHeight = value;
		}
	}

	public MeshAttachment ParentMesh
	{
		get
		{
			return parentMesh;
		}
		set
		{
			parentMesh = value;
			if (value != null)
			{
				bones = value.bones;
				vertices = value.vertices;
				worldVerticesLength = value.worldVerticesLength;
				regionUVs = value.regionUVs;
				triangles = value.triangles;
				HullLength = value.HullLength;
				Edges = value.Edges;
				Width = value.Width;
				Height = value.Height;
			}
		}
	}

	public int[] Edges { get; set; }

	public float Width { get; set; }

	public float Height { get; set; }

	public MeshAttachment(string name)
		: base(name)
	{
	}

	public void UpdateUVs()
	{
		float[] array = regionUVs;
		if (uvs == null || uvs.Length != array.Length)
		{
			uvs = new float[array.Length];
		}
		float[] array2 = uvs;
		float regionU = RegionU;
		float regionV = RegionV;
		float num = 0f;
		float num2 = 0f;
		if (RegionDegrees == 90)
		{
			float num3 = regionWidth / (RegionV2 - RegionV);
			float num4 = regionHeight / (RegionU2 - RegionU);
			regionU -= (RegionOriginalHeight - RegionOffsetY - RegionHeight) / num4;
			regionV -= (RegionOriginalWidth - RegionOffsetX - RegionWidth) / num3;
			num = RegionOriginalHeight / num4;
			num2 = RegionOriginalWidth / num3;
			int i = 0;
			for (int num5 = array2.Length; i < num5; i += 2)
			{
				array2[i] = regionU + array[i + 1] * num;
				array2[i + 1] = regionV + (1f - array[i]) * num2;
			}
		}
		else if (RegionDegrees == 180)
		{
			float num6 = regionWidth / (RegionU2 - RegionU);
			float num7 = regionHeight / (RegionV2 - RegionV);
			regionU -= (RegionOriginalWidth - RegionOffsetX - RegionWidth) / num6;
			regionV -= RegionOffsetY / num7;
			num = RegionOriginalWidth / num6;
			num2 = RegionOriginalHeight / num7;
			int j = 0;
			for (int num8 = array2.Length; j < num8; j += 2)
			{
				array2[j] = regionU + (1f - array[j]) * num;
				array2[j + 1] = regionV + (1f - array[j + 1]) * num2;
			}
		}
		else if (RegionDegrees == 270)
		{
			float num9 = regionWidth / (RegionU2 - RegionU);
			float num10 = regionHeight / (RegionV2 - RegionV);
			regionU -= RegionOffsetY / num9;
			regionV -= RegionOffsetX / num10;
			num = RegionOriginalHeight / num9;
			num2 = RegionOriginalWidth / num10;
			int k = 0;
			for (int num11 = array2.Length; k < num11; k += 2)
			{
				array2[k] = regionU + (1f - array[k + 1]) * num;
				array2[k + 1] = regionV + array[k] * num2;
			}
		}
		else
		{
			float num12 = regionWidth / (RegionU2 - RegionU);
			float num13 = regionHeight / (RegionV2 - RegionV);
			regionU -= RegionOffsetX / num12;
			regionV -= (RegionOriginalHeight - RegionOffsetY - RegionHeight) / num13;
			num = RegionOriginalWidth / num12;
			num2 = RegionOriginalHeight / num13;
			int l = 0;
			for (int num14 = array2.Length; l < num14; l += 2)
			{
				array2[l] = regionU + array[l] * num;
				array2[l + 1] = regionV + array[l + 1] * num2;
			}
		}
	}

	public override Attachment Copy()
	{
		if (parentMesh != null)
		{
			return NewLinkedMesh();
		}
		MeshAttachment meshAttachment = new MeshAttachment(base.Name);
		meshAttachment.RendererObject = RendererObject;
		meshAttachment.regionOffsetX = regionOffsetX;
		meshAttachment.regionOffsetY = regionOffsetY;
		meshAttachment.regionWidth = regionWidth;
		meshAttachment.regionHeight = regionHeight;
		meshAttachment.regionOriginalWidth = regionOriginalWidth;
		meshAttachment.regionOriginalHeight = regionOriginalHeight;
		meshAttachment.RegionDegrees = RegionDegrees;
		meshAttachment.RegionU = RegionU;
		meshAttachment.RegionV = RegionV;
		meshAttachment.RegionU2 = RegionU2;
		meshAttachment.RegionV2 = RegionV2;
		meshAttachment.Path = Path;
		meshAttachment.r = r;
		meshAttachment.g = g;
		meshAttachment.b = b;
		meshAttachment.a = a;
		CopyTo(meshAttachment);
		meshAttachment.regionUVs = new float[regionUVs.Length];
		Array.Copy(regionUVs, 0, meshAttachment.regionUVs, 0, regionUVs.Length);
		meshAttachment.uvs = new float[uvs.Length];
		Array.Copy(uvs, 0, meshAttachment.uvs, 0, uvs.Length);
		meshAttachment.triangles = new int[triangles.Length];
		Array.Copy(triangles, 0, meshAttachment.triangles, 0, triangles.Length);
		meshAttachment.HullLength = HullLength;
		if (Edges != null)
		{
			meshAttachment.Edges = new int[Edges.Length];
			Array.Copy(Edges, 0, meshAttachment.Edges, 0, Edges.Length);
		}
		meshAttachment.Width = Width;
		meshAttachment.Height = Height;
		return meshAttachment;
	}

	public MeshAttachment NewLinkedMesh()
	{
		MeshAttachment meshAttachment = new MeshAttachment(base.Name);
		meshAttachment.RendererObject = RendererObject;
		meshAttachment.regionOffsetX = regionOffsetX;
		meshAttachment.regionOffsetY = regionOffsetY;
		meshAttachment.regionWidth = regionWidth;
		meshAttachment.regionHeight = regionHeight;
		meshAttachment.regionOriginalWidth = regionOriginalWidth;
		meshAttachment.regionOriginalHeight = regionOriginalHeight;
		meshAttachment.RegionDegrees = RegionDegrees;
		meshAttachment.RegionU = RegionU;
		meshAttachment.RegionV = RegionV;
		meshAttachment.RegionU2 = RegionU2;
		meshAttachment.RegionV2 = RegionV2;
		meshAttachment.Path = Path;
		meshAttachment.r = r;
		meshAttachment.g = g;
		meshAttachment.b = b;
		meshAttachment.a = a;
		meshAttachment.deformAttachment = deformAttachment;
		meshAttachment.ParentMesh = ((parentMesh != null) ? parentMesh : this);
		meshAttachment.UpdateUVs();
		return meshAttachment;
	}
}
