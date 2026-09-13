using System;
using UnityEngine.Bindings;

namespace UnityEngine.TextCore;

[VisibleToOtherModules(new string[] { "UnityEngine.UIElementsModule" })]
internal struct MeshInfo
{
	private static readonly Color32 k_DefaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public int vertexCount;

	public Vector3[] vertices;

	public Vector2[] uvs0;

	public Vector2[] uvs2;

	public Color32[] colors32;

	public int[] triangles;

	public Material material;

	public MeshInfo(int size)
	{
		material = null;
		size = Mathf.Min(size, 16383);
		int num = size * 4;
		int num2 = size * 6;
		vertexCount = 0;
		vertices = new Vector3[num];
		uvs0 = new Vector2[num];
		uvs2 = new Vector2[num];
		colors32 = new Color32[num];
		triangles = new int[num2];
		int num3 = 0;
		int num4 = 0;
		while (num4 / 4 < size)
		{
			for (int i = 0; i < 4; i++)
			{
				vertices[num4 + i] = Vector3.zero;
				uvs0[num4 + i] = Vector2.zero;
				uvs2[num4 + i] = Vector2.zero;
				colors32[num4 + i] = k_DefaultColor;
			}
			triangles[num3] = num4;
			triangles[num3 + 1] = num4 + 1;
			triangles[num3 + 2] = num4 + 2;
			triangles[num3 + 3] = num4 + 2;
			triangles[num3 + 4] = num4 + 3;
			triangles[num3 + 5] = num4;
			num4 += 4;
			num3 += 6;
		}
	}

	internal void ResizeMeshInfo(int size)
	{
		size = Mathf.Min(size, 16383);
		int newSize = size * 4;
		int newSize2 = size * 6;
		int num = vertices.Length / 4;
		Array.Resize(ref vertices, newSize);
		Array.Resize(ref uvs0, newSize);
		Array.Resize(ref uvs2, newSize);
		Array.Resize(ref colors32, newSize);
		Array.Resize(ref triangles, newSize2);
		for (int i = num; i < size; i++)
		{
			int num2 = i * 4;
			int num3 = i * 6;
			triangles[num3] = num2;
			triangles[1 + num3] = 1 + num2;
			triangles[2 + num3] = 2 + num2;
			triangles[3 + num3] = 2 + num2;
			triangles[4 + num3] = 3 + num2;
			triangles[5 + num3] = num2;
		}
	}

	internal void Clear(bool uploadChanges)
	{
		if (vertices != null)
		{
			Array.Clear(vertices, 0, vertices.Length);
			vertexCount = 0;
		}
	}

	internal void ClearUnusedVertices()
	{
		int num = vertices.Length - vertexCount;
		if (num > 0)
		{
			Array.Clear(vertices, vertexCount, num);
		}
	}

	internal void ClearUnusedVertices(int startIndex)
	{
		int num = vertices.Length - startIndex;
		if (num > 0)
		{
			Array.Clear(vertices, startIndex, num);
		}
	}

	internal void SortGeometry(VertexSortingOrder order)
	{
		if (order == VertexSortingOrder.Normal || order != VertexSortingOrder.Reverse)
		{
			return;
		}
		int num = vertexCount / 4;
		for (int i = 0; i < num; i++)
		{
			int num2 = i * 4;
			int num3 = (num - i - 1) * 4;
			if (num2 < num3)
			{
				SwapVertexData(num2, num3);
			}
		}
	}

	internal void SwapVertexData(int src, int dst)
	{
		Vector3 vector = vertices[dst];
		vertices[dst] = vertices[src];
		vertices[src] = vector;
		vector = vertices[dst + 1];
		vertices[dst + 1] = vertices[src + 1];
		vertices[src + 1] = vector;
		vector = vertices[dst + 2];
		vertices[dst + 2] = vertices[src + 2];
		vertices[src + 2] = vector;
		vector = vertices[dst + 3];
		vertices[dst + 3] = vertices[src + 3];
		vertices[src + 3] = vector;
		Vector2 vector2 = uvs0[dst];
		uvs0[dst] = uvs0[src];
		uvs0[src] = vector2;
		vector2 = uvs0[dst + 1];
		uvs0[dst + 1] = uvs0[src + 1];
		uvs0[src + 1] = vector2;
		vector2 = uvs0[dst + 2];
		uvs0[dst + 2] = uvs0[src + 2];
		uvs0[src + 2] = vector2;
		vector2 = uvs0[dst + 3];
		uvs0[dst + 3] = uvs0[src + 3];
		uvs0[src + 3] = vector2;
		vector2 = uvs2[dst];
		uvs2[dst] = uvs2[src];
		uvs2[src] = vector2;
		vector2 = uvs2[dst + 1];
		uvs2[dst + 1] = uvs2[src + 1];
		uvs2[src + 1] = vector2;
		vector2 = uvs2[dst + 2];
		uvs2[dst + 2] = uvs2[src + 2];
		uvs2[src + 2] = vector2;
		vector2 = uvs2[dst + 3];
		uvs2[dst + 3] = uvs2[src + 3];
		uvs2[src + 3] = vector2;
		Color32 color = colors32[dst];
		colors32[dst] = colors32[src];
		colors32[src] = color;
		color = colors32[dst + 1];
		colors32[dst + 1] = colors32[src + 1];
		colors32[src + 1] = color;
		color = colors32[dst + 2];
		colors32[dst + 2] = colors32[src + 2];
		colors32[src + 2] = color;
		color = colors32[dst + 3];
		colors32[dst + 3] = colors32[src + 3];
		colors32[src + 3] = color;
	}
}
