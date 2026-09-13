using System;
using UnityEngine;
using UnityEngine.UI;

public class CircleText : BaseMeshEffect
{
	public int radius = 100;

	public float spaceCoff = 1f;

	public override void ModifyMesh(VertexHelper vh)
	{
		if (IsActive() && radius != 0)
		{
			UIVertex vertex = default(UIVertex);
			UIVertex vertex2 = default(UIVertex);
			UIVertex vertex3 = default(UIVertex);
			UIVertex vertex4 = default(UIVertex);
			for (int i = 0; i < vh.currentVertCount / 4; i++)
			{
				vh.PopulateUIVertex(ref vertex, i * 4);
				vh.PopulateUIVertex(ref vertex2, i * 4 + 1);
				vh.PopulateUIVertex(ref vertex3, i * 4 + 2);
				vh.PopulateUIVertex(ref vertex4, i * 4 + 3);
				Vector3 vector = Vector3.Lerp(vertex.position, vertex3.position, 0.5f);
				Matrix4x4 matrix4x = Matrix4x4.TRS(vector * -1f, Quaternion.identity, Vector3.one);
				float num = MathF.PI / 2f - vector.x * spaceCoff / (float)radius;
				Vector3 pos = new Vector3(Mathf.Cos(num), Mathf.Sin(num), 0f) * radius;
				Matrix4x4 matrix4x2 = Matrix4x4.TRS(q: Quaternion.Euler(0f, 0f, num * 180f / MathF.PI - 90f), pos: Vector3.zero, s: Vector3.one);
				Matrix4x4 matrix4x3 = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one) * matrix4x2 * matrix4x;
				vertex.position = matrix4x3.MultiplyPoint(vertex.position);
				vertex2.position = matrix4x3.MultiplyPoint(vertex2.position);
				vertex3.position = matrix4x3.MultiplyPoint(vertex3.position);
				vertex4.position = matrix4x3.MultiplyPoint(vertex4.position);
				vertex.position.y = vertex.position.y - (float)radius + vector.y;
				vertex2.position.y = vertex2.position.y - (float)radius + vector.y;
				vertex3.position.y = vertex3.position.y - (float)radius + vector.y;
				vertex4.position.y = vertex4.position.y - (float)radius + vector.y;
				vh.SetUIVertex(vertex, i * 4);
				vh.SetUIVertex(vertex2, i * 4 + 1);
				vh.SetUIVertex(vertex3, i * 4 + 2);
				vh.SetUIVertex(vertex4, i * 4 + 3);
			}
		}
	}
}
