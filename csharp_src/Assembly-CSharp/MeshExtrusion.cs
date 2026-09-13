using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshExtrusion : MonoBehaviour
{
	public Mesh originalMesh;

	private MeshFilter meshFilter;

	public float offsetValue = 1f;

	private float offsetValueMem = 1f;

	public int numberOfStacks = 1;

	private int numberOfStacksMem = 1;

	private int[] oldTri;

	private Vector3[] oldVert;

	private Vector3[] oldNorm;

	private Vector2[] oldUV;

	private Color[] oldCol;

	private List<int> triangles = new List<int>();

	private List<Vector3> vertexs = new List<Vector3>();

	private List<Vector2> uvs = new List<Vector2>();

	private List<Color> cols = new List<Color>();

	private void Awake()
	{
		CheckValues();
		BuildGeometry();
	}

	private void OnEnable()
	{
		CheckValues();
	}

	private void Update()
	{
		if (offsetValueMem != offsetValue || numberOfStacks != numberOfStacksMem)
		{
			ClearGeometry();
			BuildGeometry();
			offsetValueMem = offsetValue;
			numberOfStacksMem = numberOfStacks;
		}
	}

	private void CheckValues()
	{
		offsetValueMem = offsetValue;
		numberOfStacksMem = numberOfStacks;
		meshFilter = base.gameObject.GetComponent<MeshFilter>();
		oldTri = originalMesh.triangles;
		oldVert = originalMesh.vertices;
		oldNorm = originalMesh.normals;
		oldUV = originalMesh.uv;
	}

	private void ClearGeometry()
	{
		triangles.Clear();
		triangles.TrimExcess();
		vertexs.Clear();
		vertexs.TrimExcess();
		uvs.Clear();
		uvs.TrimExcess();
		cols.Clear();
		cols.TrimExcess();
	}

	private void BuildGeometry()
	{
		if (meshFilter == null)
		{
			meshFilter = base.gameObject.GetComponent<MeshFilter>();
		}
		Mesh mesh = new Mesh();
		meshFilter.mesh = mesh;
		int num = Mathf.Min(numberOfStacks, 100);
		for (int i = 0; i < num; i++)
		{
			int num2 = i * oldVert.Length;
			int num3 = 0;
			Vector3[] array = oldVert;
			foreach (Vector3 vector in array)
			{
				vertexs.Add(vector + oldNorm[num3] * offsetValue * 0.01f * i);
				uvs.Add(oldUV[num3]);
				cols.Add(new Color(1f * ((float)i / (float)(num - 1)), 1f * ((float)i / (float)(num - 1)), 1f * ((float)i / (float)(num - 1))));
				num3++;
			}
			num3 = 0;
			int[] array2 = oldTri;
			for (int j = 0; j < array2.Length; j++)
			{
				_ = array2[j];
				triangles.Add(oldTri[num3] + num2);
				num3++;
			}
		}
		mesh.vertices = vertexs.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.colors = cols.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}
}
