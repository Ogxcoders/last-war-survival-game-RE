using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Experimental.Rendering.Universal.LibTessDotNet;

namespace UnityEngine.Experimental.Rendering.Universal;

internal class ShadowUtility
{
	internal struct Edge : IComparable<Edge>
	{
		public int vertexIndex0;

		public int vertexIndex1;

		public Vector4 tangent;

		private bool compareReversed;

		public void AssignVertexIndices(int vi0, int vi1)
		{
			vertexIndex0 = vi0;
			vertexIndex1 = vi1;
			compareReversed = vi0 > vi1;
		}

		public int Compare(Edge a, Edge b)
		{
			int num = (a.compareReversed ? a.vertexIndex1 : a.vertexIndex0);
			int num2 = (a.compareReversed ? a.vertexIndex0 : a.vertexIndex1);
			int num3 = (b.compareReversed ? b.vertexIndex1 : b.vertexIndex0);
			int num4 = (b.compareReversed ? b.vertexIndex0 : b.vertexIndex1);
			int num5 = num - num3;
			int result = num2 - num4;
			if (num5 == 0)
			{
				return result;
			}
			return num5;
		}

		public int CompareTo(Edge edgeToCompare)
		{
			return Compare(this, edgeToCompare);
		}
	}

	private static Edge CreateEdge(int triangleIndexA, int triangleIndexB, List<Vector3> vertices, List<int> triangles)
	{
		Edge result = default(Edge);
		result.AssignVertexIndices(triangles[triangleIndexA], triangles[triangleIndexB]);
		Vector3 vector = vertices[result.vertexIndex0];
		vector.z = 0f;
		Vector3 vector2 = vertices[result.vertexIndex1];
		vector2.z = 0f;
		Vector3 rhs = Vector3.Normalize(vector2 - vector);
		result.tangent = Vector3.Cross(-Vector3.forward, rhs);
		return result;
	}

	private static void PopulateEdgeArray(List<Vector3> vertices, List<int> triangles, List<Edge> edges)
	{
		for (int i = 0; i < triangles.Count; i += 3)
		{
			edges.Add(CreateEdge(i, i + 1, vertices, triangles));
			edges.Add(CreateEdge(i + 1, i + 2, vertices, triangles));
			edges.Add(CreateEdge(i + 2, i, vertices, triangles));
		}
	}

	private static bool IsOutsideEdge(int edgeIndex, List<Edge> edgesToProcess)
	{
		int num = edgeIndex - 1;
		int num2 = edgeIndex + 1;
		int count = edgesToProcess.Count;
		Edge edge = edgesToProcess[edgeIndex];
		if (num < 0 || edge.CompareTo(edgesToProcess[edgeIndex - 1]) != 0)
		{
			if (num2 < count)
			{
				return edge.CompareTo(edgesToProcess[edgeIndex + 1]) != 0;
			}
			return true;
		}
		return false;
	}

	private static void SortEdges(List<Edge> edgesToProcess)
	{
		edgesToProcess.Sort();
	}

	private static void CreateShadowTriangles(List<Vector3> vertices, List<Color> colors, List<int> triangles, List<Vector4> tangents, List<Edge> edges)
	{
		for (int i = 0; i < edges.Count; i++)
		{
			if (IsOutsideEdge(i, edges))
			{
				Edge edge = edges[i];
				tangents[edge.vertexIndex1] = -edge.tangent;
				int count = vertices.Count;
				vertices.Add(vertices[edge.vertexIndex0]);
				colors.Add(colors[edge.vertexIndex0]);
				tangents.Add(-edge.tangent);
				triangles.Add(edge.vertexIndex0);
				triangles.Add(count);
				triangles.Add(edge.vertexIndex1);
			}
		}
	}

	private static object InterpCustomVertexData(Vec3 position, object[] data, float[] weights)
	{
		return data[0];
	}

	private static void InitializeTangents(int tangentsToAdd, List<Vector4> tangents)
	{
		for (int i = 0; i < tangentsToAdd; i++)
		{
			tangents.Add(Vector4.zero);
		}
	}

	public static void GenerateShadowMesh(Mesh mesh, Vector3[] shapePath)
	{
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		List<Vector4> list3 = new List<Vector4>();
		List<Color> list4 = new List<Color>();
		int num = shapePath.Length;
		ContourVertex[] array = new ContourVertex[2 * num];
		for (int i = 0; i < num; i++)
		{
			Color color = new Color(shapePath[i].x, shapePath[i].y, shapePath[i].x, shapePath[i].y);
			int num2 = (i + 1) % num;
			array[2 * i] = new ContourVertex
			{
				Position = new Vec3
				{
					X = shapePath[i].x,
					Y = shapePath[i].y,
					Z = 0f
				},
				Data = color
			};
			color = new Color(shapePath[i].x, shapePath[i].y, shapePath[num2].x, shapePath[num2].y);
			Vector2 vector = 0.5f * (shapePath[i] + shapePath[num2]);
			array[2 * i + 1] = new ContourVertex
			{
				Position = new Vec3
				{
					X = vector.x,
					Y = vector.y,
					Z = 0f
				},
				Data = color
			};
		}
		Tess tess = new Tess();
		tess.AddContour(array, ContourOrientation.Original);
		tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3, InterpCustomVertexData);
		int[] collection = tess.Elements.Select((int result) => result).ToArray();
		Vector3[] collection2 = tess.Vertices.Select((ContourVertex v) => new Vector3(v.Position.X, v.Position.Y, 0f)).ToArray();
		Color[] collection3 = tess.Vertices.Select((ContourVertex v) => new Color(((Color)v.Data).r, ((Color)v.Data).g, ((Color)v.Data).b, ((Color)v.Data).a)).ToArray();
		list.AddRange(collection2);
		list2.AddRange(collection);
		list4.AddRange(collection3);
		InitializeTangents(list.Count, list3);
		List<Edge> list5 = new List<Edge>();
		PopulateEdgeArray(list, list2, list5);
		SortEdges(list5);
		CreateShadowTriangles(list, list4, list2, list3, list5);
		Color[] colors = list4.ToArray();
		Vector3[] vertices = list.ToArray();
		int[] triangles = list2.ToArray();
		Vector4[] tangents = list3.ToArray();
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.tangents = tangents;
		mesh.colors = colors;
	}
}
