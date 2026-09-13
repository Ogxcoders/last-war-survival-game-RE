using System;
using Unity.Collections;

namespace UnityEngine.UIElements;

public class MeshWriteData
{
	internal NativeSlice<Vertex> m_Vertices;

	internal NativeSlice<ushort> m_Indices;

	internal Rect m_UVRegion;

	internal int currentIndex;

	internal int currentVertex;

	public int vertexCount => m_Vertices.Length;

	public int indexCount => m_Indices.Length;

	public Rect uvRegion => m_UVRegion;

	internal MeshWriteData()
	{
	}

	public void SetNextVertex(Vertex vertex)
	{
		m_Vertices[currentVertex++] = vertex;
	}

	public void SetNextIndex(ushort index)
	{
		m_Indices[currentIndex++] = index;
	}

	public void SetAllVertices(Vertex[] vertices)
	{
		if (currentVertex == 0)
		{
			m_Vertices.CopyFrom(vertices);
			currentVertex = m_Vertices.Length;
			return;
		}
		throw new InvalidOperationException("SetAllVertices may not be called after using SetNextVertex");
	}

	public void SetAllIndices(ushort[] indices)
	{
		if (currentIndex == 0)
		{
			m_Indices.CopyFrom(indices);
			currentIndex = m_Indices.Length;
			return;
		}
		throw new InvalidOperationException("SetAllIndices may not be called after using SetNextIndex");
	}

	internal void Reset(NativeSlice<Vertex> vertices, NativeSlice<ushort> indices)
	{
		m_Vertices = vertices;
		m_Indices = indices;
		m_UVRegion = new Rect(0f, 0f, 1f, 1f);
		currentIndex = (currentVertex = 0);
	}

	internal void Reset(NativeSlice<Vertex> vertices, NativeSlice<ushort> indices, Rect uvRegion)
	{
		m_Vertices = vertices;
		m_Indices = indices;
		m_UVRegion = uvRegion;
		currentIndex = (currentVertex = 0);
	}
}
