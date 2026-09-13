using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityMeshSimplifier;

public static class MeshUtils
{
	public static readonly int UVChannelCount = 8;

	public static Mesh CreateMesh(Vector3[] vertices, int[][] indices, Vector3[] normals, Vector4[] tangents, Color[] colors, BoneWeight[] boneWeights, List<Vector2>[] uvs, Matrix4x4[] bindposes, BlendShape[] blendShapes)
	{
		return CreateMesh(vertices, indices, normals, tangents, colors, boneWeights, uvs, null, null, bindposes, blendShapes);
	}

	public static Mesh CreateMesh(Vector3[] vertices, int[][] indices, Vector3[] normals, Vector4[] tangents, Color[] colors, BoneWeight[] boneWeights, List<Vector4>[] uvs, Matrix4x4[] bindposes, BlendShape[] blendShapes)
	{
		return CreateMesh(vertices, indices, normals, tangents, colors, boneWeights, null, null, uvs, bindposes, blendShapes);
	}

	public static Mesh CreateMesh(Vector3[] vertices, int[][] indices, Vector3[] normals, Vector4[] tangents, Color[] colors, BoneWeight[] boneWeights, List<Vector2>[] uvs2D, List<Vector3>[] uvs3D, List<Vector4>[] uvs4D, Matrix4x4[] bindposes, BlendShape[] blendShapes)
	{
		if (vertices == null)
		{
			throw new ArgumentNullException("vertices");
		}
		if (indices == null)
		{
			throw new ArgumentNullException("indices");
		}
		Mesh mesh = new Mesh();
		int num = indices.Length;
		IndexFormat indexFormat;
		Vector2Int[] subMeshIndexMinMax = GetSubMeshIndexMinMax(indices, out indexFormat);
		mesh.indexFormat = indexFormat;
		if (bindposes != null && bindposes.Length != 0)
		{
			mesh.bindposes = bindposes;
		}
		mesh.subMeshCount = num;
		mesh.vertices = vertices;
		if (normals != null && normals.Length != 0)
		{
			mesh.normals = normals;
		}
		if (tangents != null && tangents.Length != 0)
		{
			mesh.tangents = tangents;
		}
		if (colors != null && colors.Length != 0)
		{
			mesh.colors = colors;
		}
		if (boneWeights != null && boneWeights.Length != 0)
		{
			mesh.boneWeights = boneWeights;
		}
		if (uvs2D != null)
		{
			for (int i = 0; i < uvs2D.Length; i++)
			{
				if (uvs2D[i] != null && uvs2D[i].Count > 0)
				{
					mesh.SetUVs(i, uvs2D[i]);
				}
			}
		}
		if (uvs3D != null)
		{
			for (int j = 0; j < uvs3D.Length; j++)
			{
				if (uvs3D[j] != null && uvs3D[j].Count > 0)
				{
					mesh.SetUVs(j, uvs3D[j]);
				}
			}
		}
		if (uvs4D != null)
		{
			for (int k = 0; k < uvs4D.Length; k++)
			{
				if (uvs4D[k] != null && uvs4D[k].Count > 0)
				{
					mesh.SetUVs(k, uvs4D[k]);
				}
			}
		}
		if (blendShapes != null)
		{
			ApplyMeshBlendShapes(mesh, blendShapes);
		}
		for (int l = 0; l < num; l++)
		{
			int[] array = indices[l];
			Vector2Int vector2Int = subMeshIndexMinMax[l];
			if (indexFormat == IndexFormat.UInt16 && vector2Int.y > 65535)
			{
				int x = vector2Int.x;
				for (int m = 0; m < array.Length; m++)
				{
					array[m] -= x;
				}
				mesh.SetTriangles(array, l, calculateBounds: false, x);
			}
			else
			{
				mesh.SetTriangles(array, l, calculateBounds: false, 0);
			}
		}
		mesh.RecalculateBounds();
		return mesh;
	}

	public static BlendShape[] GetMeshBlendShapes(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		int vertexCount = mesh.vertexCount;
		int blendShapeCount = mesh.blendShapeCount;
		if (blendShapeCount == 0)
		{
			return null;
		}
		BlendShape[] array = new BlendShape[blendShapeCount];
		for (int i = 0; i < blendShapeCount; i++)
		{
			string blendShapeName = mesh.GetBlendShapeName(i);
			int blendShapeFrameCount = mesh.GetBlendShapeFrameCount(i);
			BlendShapeFrame[] array2 = new BlendShapeFrame[blendShapeFrameCount];
			for (int j = 0; j < blendShapeFrameCount; j++)
			{
				float blendShapeFrameWeight = mesh.GetBlendShapeFrameWeight(i, j);
				Vector3[] deltaVertices = new Vector3[vertexCount];
				Vector3[] deltaNormals = new Vector3[vertexCount];
				Vector3[] deltaTangents = new Vector3[vertexCount];
				mesh.GetBlendShapeFrameVertices(i, j, deltaVertices, deltaNormals, deltaTangents);
				array2[j] = new BlendShapeFrame(blendShapeFrameWeight, deltaVertices, deltaNormals, deltaTangents);
			}
			array[i] = new BlendShape(blendShapeName, array2);
		}
		return array;
	}

	public static void ApplyMeshBlendShapes(Mesh mesh, BlendShape[] blendShapes)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		mesh.ClearBlendShapes();
		if (blendShapes == null || blendShapes.Length == 0)
		{
			return;
		}
		for (int i = 0; i < blendShapes.Length; i++)
		{
			string shapeName = blendShapes[i].ShapeName;
			BlendShapeFrame[] frames = blendShapes[i].Frames;
			if (frames != null)
			{
				for (int j = 0; j < frames.Length; j++)
				{
					mesh.AddBlendShapeFrame(shapeName, frames[j].FrameWeight, frames[j].DeltaVertices, frames[j].DeltaNormals, frames[j].DeltaTangents);
				}
			}
		}
	}

	public static List<Vector4>[] GetMeshUVs(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		List<Vector4>[] array = new List<Vector4>[UVChannelCount];
		for (int i = 0; i < UVChannelCount; i++)
		{
			array[i] = GetMeshUVs(mesh, i);
		}
		return array;
	}

	public static List<Vector4> GetMeshUVs(Mesh mesh, int channel)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		List<Vector4> list = new List<Vector4>(mesh.vertexCount);
		mesh.GetUVs(channel, list);
		return list;
	}

	public static int GetUsedUVComponents(List<Vector4> uvs)
	{
		if (uvs == null || uvs.Count == 0)
		{
			return 0;
		}
		int num = 0;
		foreach (Vector4 uv in uvs)
		{
			if (num < 1 && uv.x != 0f)
			{
				num = 1;
			}
			if (num < 2 && uv.y != 0f)
			{
				num = 2;
			}
			if (num < 3 && uv.z != 0f)
			{
				num = 3;
			}
			if (num < 4 && uv.w != 0f)
			{
				num = 4;
				break;
			}
		}
		return num;
	}

	public static Vector2[] ConvertUVsTo2D(List<Vector4> uvs)
	{
		if (uvs == null)
		{
			return null;
		}
		Vector2[] array = new Vector2[uvs.Count];
		for (int i = 0; i < array.Length; i++)
		{
			Vector4 vector = uvs[i];
			array[i] = new Vector2(vector.x, vector.y);
		}
		return array;
	}

	public static Vector3[] ConvertUVsTo3D(List<Vector4> uvs)
	{
		if (uvs == null)
		{
			return null;
		}
		Vector3[] array = new Vector3[uvs.Count];
		for (int i = 0; i < array.Length; i++)
		{
			Vector4 vector = uvs[i];
			array[i] = new Vector3(vector.x, vector.y, vector.z);
		}
		return array;
	}

	public static Vector2Int[] GetSubMeshIndexMinMax(int[][] indices, out IndexFormat indexFormat)
	{
		if (indices == null)
		{
			throw new ArgumentNullException("indices");
		}
		Vector2Int[] array = new Vector2Int[indices.Length];
		indexFormat = IndexFormat.UInt16;
		for (int i = 0; i < indices.Length; i++)
		{
			GetIndexMinMax(indices[i], out var minIndex, out var maxIndex);
			array[i] = new Vector2Int(minIndex, maxIndex);
			if (maxIndex - minIndex > 65535)
			{
				indexFormat = IndexFormat.UInt32;
			}
		}
		return array;
	}

	private static void GetIndexMinMax(int[] indices, out int minIndex, out int maxIndex)
	{
		if (indices == null || indices.Length == 0)
		{
			minIndex = (maxIndex = 0);
			return;
		}
		minIndex = int.MaxValue;
		maxIndex = int.MinValue;
		for (int i = 0; i < indices.Length; i++)
		{
			if (indices[i] < minIndex)
			{
				minIndex = indices[i];
			}
			if (indices[i] > maxIndex)
			{
				maxIndex = indices[i];
			}
		}
	}
}
