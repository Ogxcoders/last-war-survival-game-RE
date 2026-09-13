using UnityEngine;
using UnityEngine.UI;

namespace TMPro;

public class TMP_SelectionCaret : MaskableGraphic
{
	public override void Cull(Rect clipRect, bool validRect)
	{
		if (validRect)
		{
			base.canvasRenderer.cull = false;
			CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
		}
		else
		{
			base.Cull(clipRect, validRect);
		}
	}

	protected override void UpdateGeometry()
	{
	}
}
