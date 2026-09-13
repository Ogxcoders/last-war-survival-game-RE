namespace UnityEngine.UIElements;

internal class RepaintData
{
	public Matrix4x4 currentOffset { get; set; } = Matrix4x4.identity;

	public Vector2 mousePosition { get; set; }

	public Rect currentWorldClip { get; set; }

	public Event repaintEvent { get; set; }
}
