using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine;

[UsedByNativeCode]
public struct CharacterInfo
{
	public int index;

	[Obsolete("CharacterInfo.uv is deprecated. Use uvBottomLeft, uvBottomRight, uvTopRight or uvTopLeft instead.")]
	public Rect uv;

	[Obsolete("CharacterInfo.vert is deprecated. Use minX, maxX, minY, maxY instead.")]
	public Rect vert;

	[Obsolete("CharacterInfo.width is deprecated. Use advance instead.")]
	[NativeName("advance")]
	public float width;

	public int size;

	public FontStyle style;

	[Obsolete("CharacterInfo.flipped is deprecated. Use uvBottomLeft, uvBottomRight, uvTopRight or uvTopLeft instead, which will be correct regardless of orientation.")]
	public bool flipped;

	public int advance
	{
		get
		{
			return (int)Math.Round(width, MidpointRounding.AwayFromZero);
		}
		set
		{
			width = value;
		}
	}

	public int glyphWidth
	{
		get
		{
			return (int)vert.width;
		}
		set
		{
			vert.width = value;
		}
	}

	public int glyphHeight
	{
		get
		{
			return (int)(0f - vert.height);
		}
		set
		{
			float height = vert.height;
			vert.height = -value;
			vert.y += height - vert.height;
		}
	}

	public int bearing
	{
		get
		{
			return (int)vert.x;
		}
		set
		{
			vert.x = value;
		}
	}

	public int minY
	{
		get
		{
			return (int)(vert.y + vert.height);
		}
		set
		{
			vert.height = (float)value - vert.y;
		}
	}

	public int maxY
	{
		get
		{
			return (int)vert.y;
		}
		set
		{
			float y = vert.y;
			vert.y = value;
			vert.height += y - vert.y;
		}
	}

	public int minX
	{
		get
		{
			return (int)vert.x;
		}
		set
		{
			float x = vert.x;
			vert.x = value;
			vert.width += x - vert.x;
		}
	}

	public int maxX
	{
		get
		{
			return (int)(vert.x + vert.width);
		}
		set
		{
			vert.width = (float)value - vert.x;
		}
	}

	internal Vector2 uvBottomLeftUnFlipped
	{
		get
		{
			return new Vector2(uv.x, uv.y);
		}
		set
		{
			Vector2 vector = uvTopRightUnFlipped;
			uv.x = value.x;
			uv.y = value.y;
			uv.width = vector.x - uv.x;
			uv.height = vector.y - uv.y;
		}
	}

	internal Vector2 uvBottomRightUnFlipped
	{
		get
		{
			return new Vector2(uv.x + uv.width, uv.y);
		}
		set
		{
			Vector2 vector = uvTopRightUnFlipped;
			uv.width = value.x - uv.x;
			uv.y = value.y;
			uv.height = vector.y - uv.y;
		}
	}

	internal Vector2 uvTopRightUnFlipped
	{
		get
		{
			return new Vector2(uv.x + uv.width, uv.y + uv.height);
		}
		set
		{
			uv.width = value.x - uv.x;
			uv.height = value.y - uv.y;
		}
	}

	internal Vector2 uvTopLeftUnFlipped
	{
		get
		{
			return new Vector2(uv.x, uv.y + uv.height);
		}
		set
		{
			Vector2 vector = uvTopRightUnFlipped;
			uv.x = value.x;
			uv.height = value.y - uv.y;
			uv.width = vector.x - uv.x;
		}
	}

	public Vector2 uvBottomLeft
	{
		get
		{
			return uvBottomLeftUnFlipped;
		}
		set
		{
			uvBottomLeftUnFlipped = value;
		}
	}

	public Vector2 uvBottomRight
	{
		get
		{
			return flipped ? uvTopLeftUnFlipped : uvBottomRightUnFlipped;
		}
		set
		{
			if (flipped)
			{
				uvTopLeftUnFlipped = value;
			}
			else
			{
				uvBottomRightUnFlipped = value;
			}
		}
	}

	public Vector2 uvTopRight
	{
		get
		{
			return uvTopRightUnFlipped;
		}
		set
		{
			uvTopRightUnFlipped = value;
		}
	}

	public Vector2 uvTopLeft
	{
		get
		{
			return flipped ? uvBottomRightUnFlipped : uvTopLeftUnFlipped;
		}
		set
		{
			if (flipped)
			{
				uvBottomRightUnFlipped = value;
			}
			else
			{
				uvTopLeftUnFlipped = value;
			}
		}
	}
}
