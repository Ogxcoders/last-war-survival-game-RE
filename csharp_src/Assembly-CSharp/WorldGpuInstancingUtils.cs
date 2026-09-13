using UnityEngine;

public static class WorldGpuInstancingUtils
{
	public static Mesh CreateNormalMesh()
	{
		Mesh mesh = new Mesh();
		mesh.name = "mesh";
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(1f, 0f, 1f)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 1, 2, 3 };
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateTangents();
		mesh.RecalculateNormals();
		return mesh;
	}

	public static Mesh CreateXZMesh(float scale, float anchorX, float anchorZ)
	{
		Mesh mesh = new Mesh();
		mesh.name = "mesh";
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f + anchorX, 0f, 0f + anchorZ) * scale,
			new Vector3(1f + anchorX, 0f, 0f + anchorZ) * scale,
			new Vector3(0f + anchorX, 0f, 1f + anchorZ) * scale,
			new Vector3(1f + anchorX, 0f, 1f + anchorZ) * scale
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 1, 2, 3 };
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateTangents();
		mesh.RecalculateNormals();
		return mesh;
	}

	public static Mesh CreateNormalMeshVertical()
	{
		Mesh mesh = new Mesh();
		mesh.name = "mesh";
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(1f, 0f, 1f)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 0f),
			new Vector2(0f, 1f)
		};
		int[] triangles = new int[6] { 0, 2, 1, 1, 2, 3 };
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
		mesh.RecalculateTangents();
		mesh.RecalculateNormals();
		return mesh;
	}
}
