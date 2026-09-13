namespace UnityEngine.UI;

public class Empty4Raycast : MaskableGraphic
{
	protected Empty4Raycast()
	{
		base.useLegacyMeshGeneration = false;
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
	}
}
