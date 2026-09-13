using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BF_MeshExtrusion : MonoBehaviour
{
	public Mesh originalMesh;

	private MeshFilter meshFilter;

	[Range(0.001f, 0.05f)]
	public float offsetValue = 0.01f;

	public Vector3 offsetVector = Vector3.zero;

	private float offsetValueMem = 1f;

	private Vector3 offsetVectorMem = Vector3.zero;

	[Range(1f, 20f)]
	public int numberOfStacks = 1;

	private int numberOfStacksMem = 1;

	private int[] oldTri;

	private Vector3[] oldVert;

	private Vector3[] oldNorm;

	private Vector2[] oldUV;

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
		if (offsetValueMem != offsetValue || numberOfStacks != numberOfStacksMem || offsetVectorMem != offsetVector)
		{
			ClearGeometry();
			BuildGeometry();
			offsetValueMem = offsetValue;
			offsetVectorMem = offsetVector;
			numberOfStacksMem = numberOfStacks;
		}
	}

	private void CheckValues()
	{
		offsetValueMem = offsetValue;
		offsetVectorMem = offsetVector;
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
		int num2 = 0;
		for (int num3 = num - 1; num3 >= 0; num3--)
		{
			int num4 = 0;
			float num5 = 0f;
			if (num > 1)
			{
				num5 = 1f * ((float)num3 / (float)(num - 1));
			}
			Vector3[] array = oldVert;
			foreach (Vector3 vector in array)
			{
				vertexs.Add(vector + oldNorm[num4] * offsetValue * num3 + offsetVectorMem * num3);
				uvs.Add(oldUV[num4]);
				cols.Add(new Color(num5, num5, num5));
				num4++;
			}
			num4 = 0;
			int[] array2 = oldTri;
			for (int i = 0; i < array2.Length; i++)
			{
				_ = array2[i];
				triangles.Add(oldTri[num4] + num2);
				num4++;
			}
			num2 += oldVert.Length;
		}
		mesh.vertices = vertexs.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();
		mesh.colors = cols.ToArray();
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize();
	}
}
