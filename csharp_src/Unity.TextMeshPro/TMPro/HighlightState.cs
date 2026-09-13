using UnityEngine;

namespace TMPro;

public struct HighlightState
{
	public Color32 color;

	public TMP_Offset padding;

	public HighlightState(Color32 color, TMP_Offset padding)
	{
		this.color = color;
		this.padding = padding;
	}

	public static bool operator ==(HighlightState lhs, HighlightState rhs)
	{
		if (lhs.color.Compare(rhs.color))
		{
			return lhs.padding == rhs.padding;
		}
		return false;
	}

	public static bool operator !=(HighlightState lhs, HighlightState rhs)
	{
		return !(lhs == rhs);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public bool Equals(HighlightState other)
	{
		return base.Equals((object)other);
	}
}
