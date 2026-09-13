using UnityEngine;
using UnityEngine.UI;

namespace Spine.Unity;

[RequireComponent(typeof(CanvasRenderer))]
public class SkeletonSubmeshGraphic : MaskableGraphic
{
	public override void SetMaterialDirty()
	{
	}

	public override void SetVerticesDirty()
	{
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
