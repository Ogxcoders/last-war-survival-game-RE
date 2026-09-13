using System.Collections.Generic;

namespace UnityEngine.UIElements.UIR;

internal class DrawParams
{
	internal static readonly Rect k_UnlimitedRect = new Rect(-100000f, -100000f, 200000f, 200000f);

	internal Rect viewport;

	internal Matrix4x4 projection;

	internal readonly Stack<ViewTransform> view = new Stack<ViewTransform>(8);

	internal readonly Stack<Rect> scissor = new Stack<Rect>(8);

	public void Reset(Rect _viewport, Matrix4x4 _projection)
	{
		viewport = _viewport;
		projection = _projection;
		view.Clear();
		view.Push(new ViewTransform
		{
			transform = Matrix4x4.identity,
			clipRect = k_UnlimitedRect.ToVector4()
		});
		scissor.Clear();
		scissor.Push(k_UnlimitedRect);
	}
}
