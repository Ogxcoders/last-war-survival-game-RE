using System;

namespace Spine;

public class RegionAttachment : Attachment, IHasRendererObject
{
	public const int BLX = 0;

	public const int BLY = 1;

	public const int ULX = 2;

	public const int ULY = 3;

	public const int URX = 4;

	public const int URY = 5;

	public const int BRX = 6;

	public const int BRY = 7;

	internal float x;

	internal float y;

	internal float rotation;

	internal float scaleX = 1f;

	internal float scaleY = 1f;

	internal float width;

	internal float height;

	internal float regionOffsetX;

	internal float regionOffsetY;

	internal float regionWidth;

	internal float regionHeight;

	internal float regionOriginalWidth;

	internal float regionOriginalHeight;

	internal float[] offset = new float[8];

	internal float[] uvs = new float[8];

	internal float r = 1f;

	internal float g = 1f;

	internal float b = 1f;

	internal float a = 1f;

	public float X
	{
		get
		{
			return x;
		}
		set
		{
			x = value;
		}
	}

	public float Y
	{
		get
		{
			return y;
		}
		set
		{
			y = value;
		}
	}

	public float Rotation
	{
		get
		{
			return rotation;
		}
		set
		{
			rotation = value;
		}
	}

	public float ScaleX
	{
		get
		{
			return scaleX;
		}
		set
		{
			scaleX = value;
		}
	}

	public float ScaleY
	{
		get
		{
			return scaleY;
		}
		set
		{
			scaleY = value;
		}
	}

	public float Width
	{
		get
		{
			return width;
		}
		set
		{
			width = value;
		}
	}

	public float Height
	{
		get
		{
			return height;
		}
		set
		{
			height = value;
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

	public float[] Offset => offset;

	public float[] UVs => uvs;

	public RegionAttachment(string name)
		: base(name)
	{
	}

	public void UpdateOffset()
	{
		float num = width / regionOriginalWidth * scaleX;
		float num2 = height / regionOriginalHeight * scaleY;
		float num3 = (0f - width) / 2f * scaleX + regionOffsetX * num;
		float num4 = (0f - height) / 2f * scaleY + regionOffsetY * num2;
		float num5 = num3 + regionWidth * num;
		float num6 = num4 + regionHeight * num2;
		float num7 = MathUtils.CosDeg(rotation);
		float num8 = MathUtils.SinDeg(rotation);
		float num9 = x;
		float num10 = y;
		float num11 = num3 * num7 + num9;
		float num12 = num3 * num8;
		float num13 = num4 * num7 + num10;
		float num14 = num4 * num8;
		float num15 = num5 * num7 + num9;
		float num16 = num5 * num8;
		float num17 = num6 * num7 + num10;
		float num18 = num6 * num8;
		float[] array = offset;
		array[0] = num11 - num14;
		array[1] = num13 + num12;
		array[2] = num11 - num18;
		array[3] = num17 + num12;
		array[4] = num15 - num18;
		array[5] = num17 + num16;
		array[6] = num15 - num14;
		array[7] = num13 + num16;
	}

	public void SetUVs(float u, float v, float u2, float v2, int degrees)
	{
		float[] array = uvs;
		if (degrees == 90)
		{
			array[4] = u;
			array[5] = v2;
			array[6] = u;
			array[7] = v;
			array[0] = u2;
			array[1] = v;
			array[2] = u2;
			array[3] = v2;
		}
		else
		{
			array[2] = u;
			array[3] = v2;
			array[4] = u;
			array[5] = v;
			array[6] = u2;
			array[7] = v;
			array[0] = u2;
			array[1] = v2;
		}
	}

	public void ComputeWorldVertices(Bone bone, float[] worldVertices, int offset, int stride = 2)
	{
		float[] array = this.offset;
		float worldX = bone.worldX;
		float worldY = bone.worldY;
		float num = bone.a;
		float num2 = bone.b;
		float c = bone.c;
		float d = bone.d;
		float num3 = array[6];
		float num4 = array[7];
		worldVertices[offset] = num3 * num + num4 * num2 + worldX;
		worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
		offset += stride;
		num3 = array[0];
		num4 = array[1];
		worldVertices[offset] = num3 * num + num4 * num2 + worldX;
		worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
		offset += stride;
		num3 = array[2];
		num4 = array[3];
		worldVertices[offset] = num3 * num + num4 * num2 + worldX;
		worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
		offset += stride;
		num3 = array[4];
		num4 = array[5];
		worldVertices[offset] = num3 * num + num4 * num2 + worldX;
		worldVertices[offset + 1] = num3 * c + num4 * d + worldY;
	}

	public override Attachment Copy()
	{
		RegionAttachment regionAttachment = new RegionAttachment(base.Name);
		regionAttachment.RendererObject = RendererObject;
		regionAttachment.regionOffsetX = regionOffsetX;
		regionAttachment.regionOffsetY = regionOffsetY;
		regionAttachment.regionWidth = regionWidth;
		regionAttachment.regionHeight = regionHeight;
		regionAttachment.regionOriginalWidth = regionOriginalWidth;
		regionAttachment.regionOriginalHeight = regionOriginalHeight;
		regionAttachment.Path = Path;
		regionAttachment.x = x;
		regionAttachment.y = y;
		regionAttachment.scaleX = scaleX;
		regionAttachment.scaleY = scaleY;
		regionAttachment.rotation = rotation;
		regionAttachment.width = width;
		regionAttachment.height = height;
		Array.Copy(uvs, 0, regionAttachment.uvs, 0, 8);
		Array.Copy(offset, 0, regionAttachment.offset, 0, 8);
		regionAttachment.r = r;
		regionAttachment.g = g;
		regionAttachment.b = b;
		regionAttachment.a = a;
		return regionAttachment;
	}
}
