using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorldTerrain : MonoBehaviour
{
	private const float MeshGridSize = 1.8101934f;

	private Mesh mesh;

	private MeshFilter filter;

	private MeshRenderer meshRenderer;

	[SerializeField]
	private int sortingOrder;

	private int[] triangles = new int[54]
	{
		0, 2, 3, 0, 3, 1, 1, 7, 5, 1,
		3, 7, 5, 6, 4, 5, 7, 6, 2, 11,
		3, 2, 9, 11, 3, 15, 7, 3, 11, 15,
		7, 13, 6, 7, 15, 13, 8, 10, 9, 9,
		10, 11, 11, 14, 15, 11, 10, 14, 12, 15,
		14, 12, 13, 15
	};

	public void Create(int tileRange)
	{
		filter.mesh = CreateMesh(tileRange);
	}

	private void Awake()
	{
		filter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.sortingLayerName = "FreeBuildGround";
		meshRenderer.sortingOrder = sortingOrder;
	}

	private Mesh CreateMesh(int tileRange)
	{
		Mesh obj = new Mesh();
		float num = (float)(tileRange + 1) * 1.8101934f;
		List<Vector3> list = new List<Vector3>(16);
		List<Color> list2 = new List<Color>(16);
		list.Add(new Vector3(0f, 0f, 0f));
		list.Add(new Vector3(1.8101934f, 0f, 0f));
		list.Add(new Vector3(0f, 1.8101934f, 0f));
		list.Add(new Vector3(1.8101934f, 1.8101934f, 0f));
		list.Add(new Vector3(num, 0f, 0f));
		list.Add(new Vector3(num - 1.8101934f, 0f, 0f));
		list.Add(new Vector3(num, 1.8101934f, 0f));
		list.Add(new Vector3(num - 1.8101934f, 1.8101934f, 0f));
		list.Add(new Vector3(0f, num, 0f));
		list.Add(new Vector3(0f, num - 1.8101934f, 0f));
		list.Add(new Vector3(1.8101934f, num, 0f));
		list.Add(new Vector3(1.8101934f, num - 1.8101934f, 0f));
		list.Add(new Vector3(num, num, 0f));
		list.Add(new Vector3(num, num - 1.8101934f, 0f));
		list.Add(new Vector3(num - 1.8101934f, num, 0f));
		list.Add(new Vector3(num - 1.8101934f, num - 1.8101934f, 0f));
		obj.SetVertices(list);
		obj.SetTriangles(triangles, 0);
		obj.SetUVs(0, list.Select((Vector3 v) => new Vector2(v.x / 10.24f, v.y / 10.24f)).ToList());
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 1f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 1f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 1f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 0f));
		list2.Add(new Color(1f, 1f, 1f, 1f));
		obj.SetColors(list2);
		return obj;
	}
}
