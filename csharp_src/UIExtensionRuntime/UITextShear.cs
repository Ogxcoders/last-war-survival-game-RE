using UnityEngine;
using UnityEngine.UI;

public class UITextShear : BaseMeshEffect
{
	public float kx;

	public float ky;

	public override void ModifyMesh(VertexHelper vh)
	{
		UIVertex vertex = default(UIVertex);
		for (int i = 0; i < vh.currentVertCount; i++)
		{
			vh.PopulateUIVertex(ref vertex, i);
			Vector3 position = vertex.position;
			vertex.position.x = position.x + position.y * kx;
			vertex.position.y = position.y + position.x * ky;
			vh.SetUIVertex(vertex, i);
		}
	}
}
