namespace UnityEngine.UIElements;

internal struct Spacing
{
	public float left;

	public float top;

	public float right;

	public float bottom;

	public float horizontal => left + right;

	public float vertical => top + bottom;

	public Spacing(float left, float top, float right, float bottom)
	{
		this.left = left;
		this.top = top;
		this.right = right;
		this.bottom = bottom;
	}

	public static Rect operator +(Rect r, Spacing a)
	{
		r.x -= a.left;
		r.y -= a.top;
		r.width += a.horizontal;
		r.height += a.vertical;
		return r;
	}

	public static Rect operator -(Rect r, Spacing a)
	{
		r.x += a.left;
		r.y += a.top;
		r.width = Mathf.Max(0f, r.width - a.horizontal);
		r.height = Mathf.Max(0f, r.height - a.vertical);
		return r;
	}
}
