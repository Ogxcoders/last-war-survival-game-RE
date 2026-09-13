using UnityEngine;

namespace TMPro;

public struct Extents
{
	internal static Extents zero = new Extents(Vector2.zero, Vector2.zero);

	internal static Extents uninitialized = new Extents(new Vector2(32767f, 32767f), new Vector2(-32767f, -32767f));

	public Vector2 min;

	public Vector2 max;

	public Extents(Vector2 min, Vector2 max)
	{
		this.min = min;
		this.max = max;
	}

	public override string ToString()
	{
		return "Min (" + min.x.ToString("f2") + ", " + min.y.ToString("f2") + ")   Max (" + max.x.ToString("f2") + ", " + max.y.ToString("f2") + ")";
	}
}
