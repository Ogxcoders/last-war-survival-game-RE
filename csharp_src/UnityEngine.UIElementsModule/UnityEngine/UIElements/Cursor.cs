using System;
using System.Collections.Generic;

namespace UnityEngine.UIElements;

public struct Cursor : IEquatable<Cursor>
{
	public Texture2D texture { get; set; }

	public Vector2 hotspot { get; set; }

	internal int defaultCursorId { get; set; }

	public override bool Equals(object obj)
	{
		return obj is Cursor && Equals((Cursor)obj);
	}

	public bool Equals(Cursor other)
	{
		return EqualityComparer<Texture2D>.Default.Equals(texture, other.texture) && hotspot.Equals(other.hotspot) && defaultCursorId == other.defaultCursorId;
	}

	public override int GetHashCode()
	{
		int num = 1500536833;
		num = num * -1521134295 + EqualityComparer<Texture2D>.Default.GetHashCode(texture);
		num = num * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(hotspot);
		return num * -1521134295 + defaultCursorId.GetHashCode();
	}

	public static bool operator ==(Cursor style1, Cursor style2)
	{
		return style1.Equals(style2);
	}

	public static bool operator !=(Cursor style1, Cursor style2)
	{
		return !(style1 == style2);
	}

	public override string ToString()
	{
		return $"texture={texture}, hotspot={hotspot}";
	}
}
