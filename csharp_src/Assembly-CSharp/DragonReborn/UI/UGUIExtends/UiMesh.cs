using UnityEngine;
using UnityEngine.UI;

namespace DragonReborn.UI.UGUIExtends;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class UiMesh : MaskableGraphic
{
	[SerializeField]
	private Mesh _mesh;

	[SerializeField]
	private Vector3 _position;

	[SerializeField]
	private Vector3 _rotation;

	[SerializeField]
	private Vector3 _scale = Vector3.one;

	public override Texture mainTexture
	{
		get
		{
			if (material != null && material.mainTexture != null)
			{
				return material.mainTexture;
			}
			return Graphic.s_WhiteTexture;
		}
	}

	protected override void OnPopulateMesh(VertexHelper toFill)
	{
		toFill.Clear();
		if (!_mesh || !_mesh.isReadable)
		{
			return;
		}
		Quaternion quaternion = Quaternion.Euler(_rotation);
		int vertexCount = _mesh.vertexCount;
		Vector3[] vertices = _mesh.vertices;
		Vector3[] normals = _mesh.normals;
		Vector2[] uv = _mesh.uv;
		bool flag = vertices != null && vertices.Length != 0;
		bool flag2 = uv != null && uv.Length != 0;
		bool flag3 = normals != null && normals.Length != 0;
		for (int i = 0; i < vertexCount; i++)
		{
			UIVertex simpleVert = UIVertex.simpleVert;
			if (flag)
			{
				Vector3 position = quaternion * Vector3.Scale(vertices[i], _scale) + _position;
				simpleVert.position = position;
			}
			if (flag3)
			{
				simpleVert.normal = quaternion * Vector3.Scale(normals[i], _scale);
			}
			if (flag2)
			{
				simpleVert.uv0 = uv[i];
			}
			simpleVert.color = color;
			toFill.AddVert(simpleVert);
		}
		int[] triangles = _mesh.triangles;
		for (int j = 0; j < triangles.Length; j += 3)
		{
			toFill.AddTriangle(triangles[j], triangles[j + 1], triangles[j + 2]);
		}
	}
}
