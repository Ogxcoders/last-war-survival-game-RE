using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityMeshSimplifier.Internal;

namespace UnityMeshSimplifier;

public sealed class MeshSimplifier
{
	private const int TriangleEdgeCount = 3;

	private const int TriangleVertexCount = 3;

	private const double DoubleEpsilon = 0.001;

	private static readonly int UVChannelCount = MeshUtils.UVChannelCount;

	private SimplificationOptions simplificationOptions = SimplificationOptions.Default;

	private bool verbose;

	private int subMeshCount;

	private int[] subMeshOffsets;

	private ResizableArray<Triangle> triangles;

	private ResizableArray<Vertex> vertices;

	private ResizableArray<Ref> refs;

	private ResizableArray<Vector3> vertNormals;

	private ResizableArray<Vector4> vertTangents;

	private UVChannels<Vector2> vertUV2D;

	private UVChannels<Vector3> vertUV3D;

	private UVChannels<Vector4> vertUV4D;

	private ResizableArray<Color> vertColors;

	private ResizableArray<BoneWeight> vertBoneWeights;

	private ResizableArray<BlendShapeContainer> blendShapes;

	private Matrix4x4[] bindposes;

	private readonly double[] errArr = new double[3];

	private readonly int[] attributeIndexArr = new int[3];

	private readonly HashSet<Triangle> triangleHashSet1 = new HashSet<Triangle>();

	private readonly HashSet<Triangle> triangleHashSet2 = new HashSet<Triangle>();

	public SimplificationOptions SimplificationOptions
	{
		get
		{
			return simplificationOptions;
		}
		set
		{
			simplificationOptions = value;
		}
	}

	[Obsolete("Use the 'MeshSimplifier.PreserveBorderEdges' property instead.", false)]
	public bool PreserveBorders
	{
		get
		{
			return PreserveBorderEdges;
		}
		set
		{
			PreserveBorderEdges = value;
		}
	}

	public bool PreserveBorderEdges
	{
		get
		{
			return simplificationOptions.PreserveBorderEdges;
		}
		set
		{
			simplificationOptions.PreserveBorderEdges = value;
		}
	}

	[Obsolete("Use the 'MeshSimplifier.PreserveUVSeamEdges' property instead.", false)]
	public bool PreserveSeams
	{
		get
		{
			return PreserveUVSeamEdges;
		}
		set
		{
			PreserveUVSeamEdges = value;
		}
	}

	public bool PreserveUVSeamEdges
	{
		get
		{
			return simplificationOptions.PreserveUVSeamEdges;
		}
		set
		{
			simplificationOptions.PreserveUVSeamEdges = value;
		}
	}

	[Obsolete("Use the 'MeshSimplifier.PreserveUVFoldoverEdges' property instead.", false)]
	public bool PreserveFoldovers
	{
		get
		{
			return PreserveUVFoldoverEdges;
		}
		set
		{
			PreserveUVFoldoverEdges = value;
		}
	}

	public bool PreserveUVFoldoverEdges
	{
		get
		{
			return simplificationOptions.PreserveUVFoldoverEdges;
		}
		set
		{
			simplificationOptions.PreserveUVFoldoverEdges = value;
		}
	}

	public bool PreserveSurfaceCurvature
	{
		get
		{
			return simplificationOptions.PreserveSurfaceCurvature;
		}
		set
		{
			simplificationOptions.PreserveSurfaceCurvature = value;
		}
	}

	public bool EnableSmartLink
	{
		get
		{
			return simplificationOptions.EnableSmartLink;
		}
		set
		{
			simplificationOptions.EnableSmartLink = value;
		}
	}

	public int MaxIterationCount
	{
		get
		{
			return simplificationOptions.MaxIterationCount;
		}
		set
		{
			simplificationOptions.MaxIterationCount = value;
		}
	}

	public double Agressiveness
	{
		get
		{
			return simplificationOptions.Agressiveness;
		}
		set
		{
			simplificationOptions.Agressiveness = value;
		}
	}

	public bool Verbose
	{
		get
		{
			return verbose;
		}
		set
		{
			verbose = value;
		}
	}

	public double VertexLinkDistance
	{
		get
		{
			return simplificationOptions.VertexLinkDistance;
		}
		set
		{
			simplificationOptions.VertexLinkDistance = ((value > double.Epsilon) ? value : double.Epsilon);
		}
	}

	public double VertexLinkDistanceSqr
	{
		get
		{
			return simplificationOptions.VertexLinkDistance * simplificationOptions.VertexLinkDistance;
		}
		set
		{
			simplificationOptions.VertexLinkDistance = Math.Sqrt(value);
		}
	}

	public Vector3[] Vertices
	{
		get
		{
			int length = vertices.Length;
			Vector3[] array = new Vector3[length];
			Vertex[] data = vertices.Data;
			for (int i = 0; i < length; i++)
			{
				array[i] = (Vector3)data[i].p;
			}
			return array;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			bindposes = null;
			vertices.Resize(value.Length);
			Vertex[] data = vertices.Data;
			for (int i = 0; i < value.Length; i++)
			{
				data[i] = new Vertex(i, value[i]);
			}
		}
	}

	public int SubMeshCount => subMeshCount;

	public int BlendShapeCount
	{
		get
		{
			if (blendShapes == null)
			{
				return 0;
			}
			return blendShapes.Length;
		}
	}

	public Vector3[] Normals
	{
		get
		{
			if (vertNormals == null)
			{
				return null;
			}
			return vertNormals.Data;
		}
		set
		{
			InitializeVertexAttribute(value, ref vertNormals, "normals");
		}
	}

	public Vector4[] Tangents
	{
		get
		{
			if (vertTangents == null)
			{
				return null;
			}
			return vertTangents.Data;
		}
		set
		{
			InitializeVertexAttribute(value, ref vertTangents, "tangents");
		}
	}

	public Vector2[] UV1
	{
		get
		{
			return GetUVs2D(0);
		}
		set
		{
			SetUVs(0, value);
		}
	}

	public Vector2[] UV2
	{
		get
		{
			return GetUVs2D(1);
		}
		set
		{
			SetUVs(1, value);
		}
	}

	public Vector2[] UV3
	{
		get
		{
			return GetUVs2D(2);
		}
		set
		{
			SetUVs(2, value);
		}
	}

	public Vector2[] UV4
	{
		get
		{
			return GetUVs2D(3);
		}
		set
		{
			SetUVs(3, value);
		}
	}

	public Vector2[] UV5
	{
		get
		{
			return GetUVs2D(4);
		}
		set
		{
			SetUVs(4, value);
		}
	}

	public Vector2[] UV6
	{
		get
		{
			return GetUVs2D(5);
		}
		set
		{
			SetUVs(5, value);
		}
	}

	public Vector2[] UV7
	{
		get
		{
			return GetUVs2D(6);
		}
		set
		{
			SetUVs(6, value);
		}
	}

	public Vector2[] UV8
	{
		get
		{
			return GetUVs2D(7);
		}
		set
		{
			SetUVs(7, value);
		}
	}

	public Color[] Colors
	{
		get
		{
			if (vertColors == null)
			{
				return null;
			}
			return vertColors.Data;
		}
		set
		{
			InitializeVertexAttribute(value, ref vertColors, "colors");
		}
	}

	public BoneWeight[] BoneWeights
	{
		get
		{
			if (vertBoneWeights == null)
			{
				return null;
			}
			return vertBoneWeights.Data;
		}
		set
		{
			InitializeVertexAttribute(value, ref vertBoneWeights, "boneWeights");
		}
	}

	public MeshSimplifier()
	{
		triangles = new ResizableArray<Triangle>(0);
		vertices = new ResizableArray<Vertex>(0);
		refs = new ResizableArray<Ref>(0);
	}

	public MeshSimplifier(Mesh mesh)
		: this()
	{
		if (mesh != null)
		{
			Initialize(mesh);
		}
	}

	private void InitializeVertexAttribute<T>(T[] attributeValues, ref ResizableArray<T> attributeArray, string attributeName)
	{
		if (attributeValues != null && attributeValues.Length == vertices.Length)
		{
			if (attributeArray == null)
			{
				attributeArray = new ResizableArray<T>(attributeValues.Length, attributeValues.Length);
			}
			else
			{
				attributeArray.Resize(attributeValues.Length);
			}
			T[] data = attributeArray.Data;
			Array.Copy(attributeValues, 0, data, 0, attributeValues.Length);
			return;
		}
		if (attributeValues != null && attributeValues.Length != 0)
		{
			Debug.LogErrorFormat("Failed to set vertex attribute '{0}' with {1} length of array, when {2} was needed.", attributeName, attributeValues.Length, vertices.Length);
		}
		attributeArray = null;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static double VertexError(ref SymmetricMatrix q, double x, double y, double z)
	{
		return q.m0 * x * x + 2.0 * q.m1 * x * y + 2.0 * q.m2 * x * z + 2.0 * q.m3 * x + q.m4 * y * y + 2.0 * q.m5 * y * z + 2.0 * q.m6 * y + q.m7 * z * z + 2.0 * q.m8 * z + q.m9;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private double CurvatureError(ref Vertex vert0, ref Vertex vert1)
	{
		double magnitude = (vert0.p - vert1.p).Magnitude;
		HashSet<Triangle> hashSet = triangleHashSet1;
		hashSet.Clear();
		GetTrianglesContainingVertex(ref vert0, hashSet);
		GetTrianglesContainingVertex(ref vert1, hashSet);
		HashSet<Triangle> hashSet2 = triangleHashSet2;
		hashSet2.Clear();
		GetTrianglesContainingBothVertices(ref vert0, ref vert1, hashSet2);
		double num = 0.0;
		foreach (Triangle item in hashSet)
		{
			double num2 = 0.0;
			Vector3d lhs = item.n;
			foreach (Triangle item2 in hashSet2)
			{
				Vector3d rhs = item2.n;
				double num3 = Vector3d.Dot(ref lhs, ref rhs);
				if (num3 > num2)
				{
					num2 = num3;
				}
			}
			if (num2 > num)
			{
				num = num2;
			}
		}
		return magnitude * num;
	}

	private double CalculateError(ref Vertex vert0, ref Vertex vert1, out Vector3d result)
	{
		SymmetricMatrix q = vert0.q + vert1.q;
		bool flag = vert0.borderEdge && vert1.borderEdge;
		double num = 0.0;
		double num2 = q.Determinant1();
		if (num2 != 0.0 && !flag)
		{
			result = new Vector3d(-1.0 / num2 * q.Determinant2(), 1.0 / num2 * q.Determinant3(), -1.0 / num2 * q.Determinant4());
			double num3 = 0.0;
			if (simplificationOptions.PreserveSurfaceCurvature)
			{
				num3 = CurvatureError(ref vert0, ref vert1);
			}
			num = VertexError(ref q, result.x, result.y, result.z) + num3;
		}
		else
		{
			Vector3d p = vert0.p;
			Vector3d p2 = vert1.p;
			Vector3d vector3d = (p + p2) * 0.5;
			double num4 = VertexError(ref q, p.x, p.y, p.z);
			double num5 = VertexError(ref q, p2.x, p2.y, p2.z);
			double num6 = VertexError(ref q, vector3d.x, vector3d.y, vector3d.z);
			if (num4 < num5)
			{
				if (num4 < num6)
				{
					num = num4;
					result = p;
				}
				else
				{
					num = num6;
					result = vector3d;
				}
			}
			else if (num5 < num6)
			{
				num = num5;
				result = p2;
			}
			else
			{
				num = num6;
				result = vector3d;
			}
		}
		return num;
	}

	private static void CalculateBarycentricCoords(ref Vector3d point, ref Vector3d a, ref Vector3d b, ref Vector3d c, out Vector3 result)
	{
		Vector3 vector = (Vector3)(b - a);
		Vector3 vector2 = (Vector3)(c - a);
		Vector3 lhs = (Vector3)(point - a);
		float num = Vector3.Dot(vector, vector);
		float num2 = Vector3.Dot(vector, vector2);
		float num3 = Vector3.Dot(vector2, vector2);
		float num4 = Vector3.Dot(lhs, vector);
		float num5 = Vector3.Dot(lhs, vector2);
		float num6 = num * num3 - num2 * num2;
		float num7 = (num3 * num4 - num2 * num5) / num6;
		float num8 = (num * num5 - num2 * num4) / num6;
		float x = 1f - num7 - num8;
		result = new Vector3(x, num7, num8);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector4 NormalizeTangent(Vector4 tangent)
	{
		Vector3 vector = new Vector3(tangent.x, tangent.y, tangent.z);
		vector.Normalize();
		return new Vector4(vector.x, vector.y, vector.z, tangent.w);
	}

	private bool Flipped(ref Vector3d p, int i0, int i1, ref Vertex v0, bool[] deleted)
	{
		int tcount = v0.tcount;
		Ref[] data = refs.Data;
		Triangle[] data2 = triangles.Data;
		Vertex[] data3 = vertices.Data;
		for (int j = 0; j < tcount; j++)
		{
			Ref obj = data[v0.tstart + j];
			if (data2[obj.tid].deleted)
			{
				continue;
			}
			int tvertex = obj.tvertex;
			int num = data2[obj.tid][(tvertex + 1) % 3];
			int num2 = data2[obj.tid][(tvertex + 2) % 3];
			if (num == i1 || num2 == i1)
			{
				deleted[j] = true;
				continue;
			}
			Vector3d lhs = data3[num].p - p;
			lhs.Normalize();
			Vector3d rhs = data3[num2].p - p;
			rhs.Normalize();
			if (Math.Abs(Vector3d.Dot(ref lhs, ref rhs)) > 0.999)
			{
				return true;
			}
			Vector3d.Cross(ref lhs, ref rhs, out var result);
			result.Normalize();
			deleted[j] = false;
			if (Vector3d.Dot(ref result, ref data2[obj.tid].n) < 0.2)
			{
				return true;
			}
		}
		return false;
	}

	private void UpdateTriangles(int i0, int ia0, ref Vertex v, ResizableArray<bool> deleted, ref int deletedTriangles)
	{
		int tcount = v.tcount;
		Triangle[] data = triangles.Data;
		Vertex[] data2 = vertices.Data;
		for (int j = 0; j < tcount; j++)
		{
			Ref item = refs[v.tstart + j];
			int tid = item.tid;
			Triangle triangle = data[tid];
			if (triangle.deleted)
			{
				continue;
			}
			if (deleted[j])
			{
				data[tid].deleted = true;
				deletedTriangles++;
				continue;
			}
			triangle[item.tvertex] = i0;
			if (ia0 != -1)
			{
				triangle.SetAttributeIndex(item.tvertex, ia0);
			}
			triangle.dirty = true;
			triangle.err0 = CalculateError(ref data2[triangle.v0], ref data2[triangle.v1], out var result);
			triangle.err1 = CalculateError(ref data2[triangle.v1], ref data2[triangle.v2], out result);
			triangle.err2 = CalculateError(ref data2[triangle.v2], ref data2[triangle.v0], out result);
			triangle.err3 = MathHelper.Min(triangle.err0, triangle.err1, triangle.err2);
			data[tid] = triangle;
			refs.Add(item);
		}
	}

	private void InterpolateVertexAttributes(int dst, int i0, int i1, int i2, ref Vector3 barycentricCoord)
	{
		if (vertNormals != null)
		{
			vertNormals[dst] = Vector3.Normalize(vertNormals[i0] * barycentricCoord.x + vertNormals[i1] * barycentricCoord.y + vertNormals[i2] * barycentricCoord.z);
		}
		if (vertTangents != null)
		{
			vertTangents[dst] = NormalizeTangent(vertTangents[i0] * barycentricCoord.x + vertTangents[i1] * barycentricCoord.y + vertTangents[i2] * barycentricCoord.z);
		}
		if (vertUV2D != null)
		{
			for (int j = 0; j < UVChannelCount; j++)
			{
				ResizableArray<Vector2> resizableArray = vertUV2D[j];
				if (resizableArray != null)
				{
					resizableArray[dst] = resizableArray[i0] * barycentricCoord.x + resizableArray[i1] * barycentricCoord.y + resizableArray[i2] * barycentricCoord.z;
				}
			}
		}
		if (vertUV3D != null)
		{
			for (int k = 0; k < UVChannelCount; k++)
			{
				ResizableArray<Vector3> resizableArray2 = vertUV3D[k];
				if (resizableArray2 != null)
				{
					resizableArray2[dst] = resizableArray2[i0] * barycentricCoord.x + resizableArray2[i1] * barycentricCoord.y + resizableArray2[i2] * barycentricCoord.z;
				}
			}
		}
		if (vertUV4D != null)
		{
			for (int l = 0; l < UVChannelCount; l++)
			{
				ResizableArray<Vector4> resizableArray3 = vertUV4D[l];
				if (resizableArray3 != null)
				{
					resizableArray3[dst] = resizableArray3[i0] * barycentricCoord.x + resizableArray3[i1] * barycentricCoord.y + resizableArray3[i2] * barycentricCoord.z;
				}
			}
		}
		if (vertColors != null)
		{
			vertColors[dst] = vertColors[i0] * barycentricCoord.x + vertColors[i1] * barycentricCoord.y + vertColors[i2] * barycentricCoord.z;
		}
		if (blendShapes != null)
		{
			for (int m = 0; m < blendShapes.Length; m++)
			{
				blendShapes[m].InterpolateVertexAttributes(dst, i0, i1, i2, ref barycentricCoord);
			}
		}
	}

	private bool AreUVsTheSame(int channel, int indexA, int indexB)
	{
		if (vertUV2D != null)
		{
			ResizableArray<Vector2> resizableArray = vertUV2D[channel];
			if (resizableArray != null)
			{
				Vector2 vector = resizableArray[indexA];
				Vector2 vector2 = resizableArray[indexB];
				return vector == vector2;
			}
		}
		if (vertUV3D != null)
		{
			ResizableArray<Vector3> resizableArray2 = vertUV3D[channel];
			if (resizableArray2 != null)
			{
				Vector3 vector3 = resizableArray2[indexA];
				Vector3 vector4 = resizableArray2[indexB];
				return vector3 == vector4;
			}
		}
		if (vertUV4D != null)
		{
			ResizableArray<Vector4> resizableArray3 = vertUV4D[channel];
			if (resizableArray3 != null)
			{
				Vector4 vector5 = resizableArray3[indexA];
				Vector4 vector6 = resizableArray3[indexB];
				return vector5 == vector6;
			}
		}
		return false;
	}

	private void RemoveVertexPass(int startTrisCount, int targetTrisCount, double threshold, ResizableArray<bool> deleted0, ResizableArray<bool> deleted1, ref int deletedTris)
	{
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		Vertex[] data2 = vertices.Data;
		bool preserveBorderEdges = PreserveBorderEdges;
		bool preserveUVSeamEdges = PreserveUVSeamEdges;
		bool preserveUVFoldoverEdges = PreserveUVFoldoverEdges;
		for (int i = 0; i < length; i++)
		{
			if (data[i].dirty || data[i].deleted || data[i].err3 > threshold)
			{
				continue;
			}
			data[i].GetErrors(errArr);
			data[i].GetAttributeIndices(attributeIndexArr);
			for (int j = 0; j < 3; j++)
			{
				if (errArr[j] > threshold)
				{
					continue;
				}
				int num = (j + 1) % 3;
				int num2 = data[i][j];
				int num3 = data[i][num];
				if (data2[num2].borderEdge != data2[num3].borderEdge || data2[num2].uvSeamEdge != data2[num3].uvSeamEdge || data2[num2].uvFoldoverEdge != data2[num3].uvFoldoverEdge || (preserveBorderEdges && data2[num2].borderEdge) || (preserveUVSeamEdges && data2[num2].uvSeamEdge) || (preserveUVFoldoverEdges && data2[num2].uvFoldoverEdge))
				{
					continue;
				}
				CalculateError(ref data2[num2], ref data2[num3], out var result);
				deleted0.Resize(data2[num2].tcount);
				deleted1.Resize(data2[num3].tcount);
				if (Flipped(ref result, num2, num3, ref data2[num2], deleted0.Data) || Flipped(ref result, num3, num2, ref data2[num3], deleted1.Data))
				{
					continue;
				}
				int num4 = (j + 2) % 3;
				int num5 = data[i][num4];
				CalculateBarycentricCoords(ref result, ref data2[num2].p, ref data2[num3].p, ref data2[num5].p, out var result2);
				data2[num2].p = result;
				data2[num2].q += data2[num3].q;
				int num6 = attributeIndexArr[j];
				int i2 = attributeIndexArr[num];
				int i3 = attributeIndexArr[num4];
				InterpolateVertexAttributes(num6, num6, i2, i3, ref result2);
				if (data2[num2].uvSeamEdge)
				{
					num6 = -1;
				}
				int length2 = refs.Length;
				UpdateTriangles(num2, num6, ref data2[num2], deleted0, ref deletedTris);
				UpdateTriangles(num2, num6, ref data2[num3], deleted1, ref deletedTris);
				int num7 = refs.Length - length2;
				if (num7 <= data2[num2].tcount)
				{
					if (num7 > 0)
					{
						Ref[] data3 = refs.Data;
						Array.Copy(data3, length2, data3, data2[num2].tstart, num7);
					}
				}
				else
				{
					data2[num2].tstart = length2;
				}
				data2[num2].tcount = num7;
				break;
			}
			if (startTrisCount - deletedTris <= targetTrisCount)
			{
				break;
			}
		}
	}

	private void UpdateMesh(int iteration)
	{
		Triangle[] data = triangles.Data;
		Vertex[] data2 = vertices.Data;
		int num = triangles.Length;
		int length = vertices.Length;
		if (iteration > 0)
		{
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				if (!data[i].deleted)
				{
					if (num2 != i)
					{
						data[num2] = data[i];
						data[num2].index = num2;
					}
					num2++;
				}
			}
			triangles.Resize(num2);
			data = triangles.Data;
			num = num2;
		}
		UpdateReferences();
		if (iteration != 0)
		{
			return;
		}
		Ref[] data3 = refs.Data;
		List<int> list = new List<int>(8);
		List<int> list2 = new List<int>(8);
		int num3 = 0;
		for (int j = 0; j < length; j++)
		{
			data2[j].borderEdge = false;
			data2[j].uvSeamEdge = false;
			data2[j].uvFoldoverEdge = false;
		}
		int num4 = 0;
		double num5 = double.MaxValue;
		double num6 = double.MinValue;
		bool enableSmartLink = EnableSmartLink;
		double vertexLinkDistanceSqr = VertexLinkDistanceSqr;
		for (int k = 0; k < length; k++)
		{
			int tstart = data2[k].tstart;
			int tcount = data2[k].tcount;
			list.Clear();
			list2.Clear();
			num3 = 0;
			for (int l = 0; l < tcount; l++)
			{
				int tid = data3[tstart + l].tid;
				for (int m = 0; m < 3; m++)
				{
					int n = 0;
					int num7;
					for (num7 = data[tid][m]; n < num3 && list2[n] != num7; n++)
					{
					}
					if (n == num3)
					{
						list.Add(1);
						list2.Add(num7);
						num3++;
					}
					else
					{
						int index = n;
						int value = list[index] + 1;
						list[index] = value;
					}
				}
			}
			for (int num8 = 0; num8 < num3; num8++)
			{
				if (list[num8] != 1)
				{
					continue;
				}
				int num7 = list2[num8];
				data2[num7].borderEdge = true;
				num4++;
				if (enableSmartLink)
				{
					if (data2[num7].p.x < num5)
					{
						num5 = data2[num7].p.x;
					}
					if (data2[num7].p.x > num6)
					{
						num6 = data2[num7].p.x;
					}
				}
			}
		}
		if (enableSmartLink)
		{
			BorderVertex[] array = new BorderVertex[num4];
			int num9 = 0;
			double num10 = num6 - num5;
			for (int num11 = 0; num11 < length; num11++)
			{
				if (data2[num11].borderEdge)
				{
					int hash = (int)(((data2[num11].p.x - num5) / num10 * 2.0 - 1.0) * 2147483647.0);
					array[num9] = new BorderVertex(num11, hash);
					num9++;
				}
			}
			Array.Sort(array, 0, num9, BorderVertexComparer.instance);
			int num12 = Math.Max((int)(Math.Sqrt(vertexLinkDistanceSqr) / num10 * 2147483647.0), 1);
			for (int num13 = 0; num13 < num9; num13++)
			{
				int index2 = array[num13].index;
				if (index2 == -1)
				{
					continue;
				}
				Vector3d p = data2[index2].p;
				for (int num14 = num13 + 1; num14 < num9; num14++)
				{
					int index3 = array[num14].index;
					if (index3 == -1)
					{
						continue;
					}
					if (array[num14].hash - array[num13].hash > num12)
					{
						break;
					}
					Vector3d p2 = data2[index3].p;
					double num15 = (p.x - p2.x) * (p.x - p2.x);
					double num16 = (p.y - p2.y) * (p.y - p2.y);
					double num17 = (p.z - p2.z) * (p.z - p2.z);
					if (num15 + num16 + num17 <= vertexLinkDistanceSqr)
					{
						array[num14].index = -1;
						data2[index2].borderEdge = false;
						data2[index3].borderEdge = false;
						if (AreUVsTheSame(0, index2, index3))
						{
							data2[index2].uvFoldoverEdge = true;
							data2[index3].uvFoldoverEdge = true;
						}
						else
						{
							data2[index2].uvSeamEdge = true;
							data2[index3].uvSeamEdge = true;
						}
						int tcount2 = data2[index3].tcount;
						int tstart2 = data2[index3].tstart;
						for (int num18 = 0; num18 < tcount2; num18++)
						{
							Ref obj = data3[tstart2 + num18];
							data[obj.tid][obj.tvertex] = index2;
						}
					}
				}
			}
			UpdateReferences();
		}
		for (int num19 = 0; num19 < length; num19++)
		{
			data2[num19].q = default(SymmetricMatrix);
		}
		for (int num20 = 0; num20 < num; num20++)
		{
			int v = data[num20].v0;
			int v2 = data[num20].v1;
			int v3 = data[num20].v2;
			Vector3d rhs = data2[v].p;
			Vector3d p3 = data2[v2].p;
			Vector3d p4 = data2[v3].p;
			Vector3d lhs = p3 - rhs;
			Vector3d rhs2 = p4 - rhs;
			Vector3d.Cross(ref lhs, ref rhs2, out var result);
			result.Normalize();
			data[num20].n = result;
			SymmetricMatrix symmetricMatrix = new SymmetricMatrix(result.x, result.y, result.z, 0.0 - Vector3d.Dot(ref result, ref rhs));
			data2[v].q += symmetricMatrix;
			data2[v2].q += symmetricMatrix;
			data2[v3].q += symmetricMatrix;
		}
		for (int num21 = 0; num21 < num; num21++)
		{
			Triangle triangle = data[num21];
			data[num21].err0 = CalculateError(ref data2[triangle.v0], ref data2[triangle.v1], out var result2);
			data[num21].err1 = CalculateError(ref data2[triangle.v1], ref data2[triangle.v2], out result2);
			data[num21].err2 = CalculateError(ref data2[triangle.v2], ref data2[triangle.v0], out result2);
			data[num21].err3 = MathHelper.Min(data[num21].err0, data[num21].err1, data[num21].err2);
		}
	}

	private void UpdateReferences()
	{
		int length = triangles.Length;
		int length2 = vertices.Length;
		Triangle[] data = triangles.Data;
		Vertex[] data2 = vertices.Data;
		for (int i = 0; i < length2; i++)
		{
			data2[i].tstart = 0;
			data2[i].tcount = 0;
		}
		for (int j = 0; j < length; j++)
		{
			data2[data[j].v0].tcount++;
			data2[data[j].v1].tcount++;
			data2[data[j].v2].tcount++;
		}
		int num = 0;
		for (int k = 0; k < length2; k++)
		{
			data2[k].tstart = num;
			num += data2[k].tcount;
			data2[k].tcount = 0;
		}
		refs.Resize(num);
		Ref[] data3 = refs.Data;
		for (int l = 0; l < length; l++)
		{
			int v = data[l].v0;
			int v2 = data[l].v1;
			int v3 = data[l].v2;
			int tstart = data2[v].tstart;
			int num2 = data2[v].tcount++;
			int tstart2 = data2[v2].tstart;
			int num3 = data2[v2].tcount++;
			int tstart3 = data2[v3].tstart;
			int num4 = data2[v3].tcount++;
			data3[tstart + num2].Set(l, 0);
			data3[tstart2 + num3].Set(l, 1);
			data3[tstart3 + num4].Set(l, 2);
		}
	}

	private void CompactMesh()
	{
		int num = 0;
		Vertex[] data = vertices.Data;
		int length = vertices.Length;
		for (int i = 0; i < length; i++)
		{
			data[i].tcount = 0;
		}
		Vector3[] array = ((vertNormals != null) ? vertNormals.Data : null);
		Vector4[] array2 = ((vertTangents != null) ? vertTangents.Data : null);
		Vector2[][] array3 = ((vertUV2D != null) ? vertUV2D.Data : null);
		Vector3[][] array4 = ((vertUV3D != null) ? vertUV3D.Data : null);
		Vector4[][] array5 = ((vertUV4D != null) ? vertUV4D.Data : null);
		Color[] array6 = ((vertColors != null) ? vertColors.Data : null);
		BoneWeight[] array7 = ((vertBoneWeights != null) ? vertBoneWeights.Data : null);
		BlendShapeContainer[] array8 = ((blendShapes != null) ? blendShapes.Data : null);
		int num2 = -1;
		subMeshOffsets = new int[subMeshCount];
		Triangle[] data2 = triangles.Data;
		int length2 = triangles.Length;
		for (int j = 0; j < length2; j++)
		{
			Triangle triangle = data2[j];
			if (triangle.deleted)
			{
				continue;
			}
			if (triangle.va0 != triangle.v0)
			{
				int va = triangle.va0;
				int v = triangle.v0;
				data[va].p = data[v].p;
				if (array7 != null)
				{
					array7[va] = array7[v];
				}
				triangle.v0 = triangle.va0;
			}
			if (triangle.va1 != triangle.v1)
			{
				int va2 = triangle.va1;
				int v2 = triangle.v1;
				data[va2].p = data[v2].p;
				if (array7 != null)
				{
					array7[va2] = array7[v2];
				}
				triangle.v1 = triangle.va1;
			}
			if (triangle.va2 != triangle.v2)
			{
				int va3 = triangle.va2;
				int v3 = triangle.v2;
				data[va3].p = data[v3].p;
				if (array7 != null)
				{
					array7[va3] = array7[v3];
				}
				triangle.v2 = triangle.va2;
			}
			int num3 = num++;
			data2[num3] = triangle;
			data2[num3].index = num3;
			data[triangle.v0].tcount = 1;
			data[triangle.v1].tcount = 1;
			data[triangle.v2].tcount = 1;
			if (triangle.subMeshIndex > num2)
			{
				for (int k = num2 + 1; k < triangle.subMeshIndex; k++)
				{
					subMeshOffsets[k] = num3;
				}
				subMeshOffsets[triangle.subMeshIndex] = num3;
				num2 = triangle.subMeshIndex;
			}
		}
		length2 = num;
		for (int l = num2 + 1; l < subMeshCount; l++)
		{
			subMeshOffsets[l] = length2;
		}
		triangles.Resize(length2);
		data2 = triangles.Data;
		num = 0;
		for (int m = 0; m < length; m++)
		{
			Vertex vertex = data[m];
			if (vertex.tcount <= 0)
			{
				continue;
			}
			data[m].tstart = num;
			if (num != m)
			{
				data[num].index = num;
				data[num].p = vertex.p;
				if (array != null)
				{
					array[num] = array[m];
				}
				if (array2 != null)
				{
					array2[num] = array2[m];
				}
				if (array3 != null)
				{
					for (int n = 0; n < UVChannelCount; n++)
					{
						Vector2[] array9 = array3[n];
						if (array9 != null)
						{
							array9[num] = array9[m];
						}
					}
				}
				if (array4 != null)
				{
					for (int num4 = 0; num4 < UVChannelCount; num4++)
					{
						Vector3[] array10 = array4[num4];
						if (array10 != null)
						{
							array10[num] = array10[m];
						}
					}
				}
				if (array5 != null)
				{
					for (int num5 = 0; num5 < UVChannelCount; num5++)
					{
						Vector4[] array11 = array5[num5];
						if (array11 != null)
						{
							array11[num] = array11[m];
						}
					}
				}
				if (array6 != null)
				{
					array6[num] = array6[m];
				}
				if (array7 != null)
				{
					array7[num] = array7[m];
				}
				if (array8 != null)
				{
					for (int num6 = 0; num6 < blendShapes.Length; num6++)
					{
						array8[num6].MoveVertexElement(num, m);
					}
				}
			}
			num++;
		}
		for (int num7 = 0; num7 < length2; num7++)
		{
			Triangle triangle2 = data2[num7];
			triangle2.v0 = data[triangle2.v0].tstart;
			triangle2.v1 = data[triangle2.v1].tstart;
			triangle2.v2 = data[triangle2.v2].tstart;
			data2[num7] = triangle2;
		}
		length = num;
		vertices.Resize(length);
		if (array != null)
		{
			vertNormals.Resize(length, trimExess: true);
		}
		if (array2 != null)
		{
			vertTangents.Resize(length, trimExess: true);
		}
		if (array3 != null)
		{
			vertUV2D.Resize(length, trimExess: true);
		}
		if (array4 != null)
		{
			vertUV3D.Resize(length, trimExess: true);
		}
		if (array5 != null)
		{
			vertUV4D.Resize(length, trimExess: true);
		}
		if (array6 != null)
		{
			vertColors.Resize(length, trimExess: true);
		}
		if (array7 != null)
		{
			vertBoneWeights.Resize(length, trimExess: true);
		}
		if (array8 != null)
		{
			for (int num8 = 0; num8 < blendShapes.Length; num8++)
			{
				array8[num8].Resize(length);
			}
		}
	}

	private void CalculateSubMeshOffsets()
	{
		int num = -1;
		subMeshOffsets = new int[subMeshCount];
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		for (int i = 0; i < length; i++)
		{
			Triangle triangle = data[i];
			if (triangle.subMeshIndex > num)
			{
				for (int j = num + 1; j < triangle.subMeshIndex; j++)
				{
					subMeshOffsets[j] = i;
				}
				subMeshOffsets[triangle.subMeshIndex] = i;
				num = triangle.subMeshIndex;
			}
		}
		for (int k = num + 1; k < subMeshCount; k++)
		{
			subMeshOffsets[k] = length;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void GetTrianglesContainingVertex(ref Vertex vert, HashSet<Triangle> tris)
	{
		int tcount = vert.tcount;
		int tstart = vert.tstart;
		for (int i = tstart; i < tstart + tcount; i++)
		{
			tris.Add(triangles[refs[i].tid]);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void GetTrianglesContainingBothVertices(ref Vertex vert0, ref Vertex vert1, HashSet<Triangle> tris)
	{
		int tcount = vert0.tcount;
		int tstart = vert0.tstart;
		for (int i = tstart; i < tstart + tcount; i++)
		{
			int tid = refs[i].tid;
			Triangle item = triangles[tid];
			if (vertices[item.v0].index == vert1.index || vertices[item.v1].index == vert1.index || vertices[item.v2].index == vert1.index)
			{
				tris.Add(item);
			}
		}
	}

	public int[][] GetAllSubMeshTriangles()
	{
		int[][] array = new int[subMeshCount][];
		for (int i = 0; i < subMeshCount; i++)
		{
			array[i] = GetSubMeshTriangles(i);
		}
		return array;
	}

	public int[] GetSubMeshTriangles(int subMeshIndex)
	{
		if (subMeshIndex < 0)
		{
			throw new ArgumentOutOfRangeException("subMeshIndex", "The sub-mesh index is negative.");
		}
		if (subMeshOffsets == null)
		{
			CalculateSubMeshOffsets();
		}
		if (subMeshIndex >= subMeshOffsets.Length)
		{
			throw new ArgumentOutOfRangeException("subMeshIndex", "The sub-mesh index is greater than or equals to the sub mesh count.");
		}
		if (subMeshOffsets.Length != subMeshCount)
		{
			throw new InvalidOperationException("The sub-mesh triangle offsets array is not the same size as the count of sub-meshes. This should not be possible to happen.");
		}
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		int num = subMeshOffsets[subMeshIndex];
		if (num >= length)
		{
			return new int[0];
		}
		int num2 = ((subMeshIndex + 1 < subMeshCount) ? subMeshOffsets[subMeshIndex + 1] : length);
		int num3 = num2 - num;
		if (num3 < 0)
		{
			num3 = 0;
		}
		int[] array = new int[num3 * 3];
		for (int i = num; i < num2; i++)
		{
			Triangle triangle = data[i];
			int num4 = (i - num) * 3;
			array[num4] = triangle.v0;
			array[num4 + 1] = triangle.v1;
			array[num4 + 2] = triangle.v2;
		}
		return array;
	}

	public void ClearSubMeshes()
	{
		subMeshCount = 0;
		subMeshOffsets = null;
		triangles.Resize(0);
	}

	public void AddSubMeshTriangles(int[] triangles)
	{
		if (triangles == null)
		{
			throw new ArgumentNullException("triangles");
		}
		if (triangles.Length % 3 != 0)
		{
			throw new ArgumentException("The index array length must be a multiple of 3 in order to represent triangles.", "triangles");
		}
		int subMeshIndex = subMeshCount++;
		int length = this.triangles.Length;
		int num = triangles.Length / 3;
		this.triangles.Resize(this.triangles.Length + num);
		Triangle[] data = this.triangles.Data;
		for (int i = 0; i < num; i++)
		{
			int num2 = i * 3;
			int v = triangles[num2];
			int v2 = triangles[num2 + 1];
			int v3 = triangles[num2 + 2];
			int num3 = length + i;
			data[num3] = new Triangle(num3, v, v2, v3, subMeshIndex);
		}
	}

	public void AddSubMeshTriangles(int[][] triangles)
	{
		if (triangles == null)
		{
			throw new ArgumentNullException("triangles");
		}
		int num = 0;
		for (int i = 0; i < triangles.Length; i++)
		{
			if (triangles[i] == null)
			{
				throw new ArgumentException($"The index array at index {i} is null.");
			}
			if (triangles[i].Length % 3 != 0)
			{
				throw new ArgumentException($"The index array length at index {i} must be a multiple of 3 in order to represent triangles.", "triangles");
			}
			num += triangles[i].Length / 3;
		}
		int num2 = this.triangles.Length;
		this.triangles.Resize(this.triangles.Length + num);
		Triangle[] data = this.triangles.Data;
		for (int j = 0; j < triangles.Length; j++)
		{
			int subMeshIndex = subMeshCount++;
			int[] array = triangles[j];
			int num3 = array.Length / 3;
			for (int k = 0; k < num3; k++)
			{
				int num4 = k * 3;
				int v = array[num4];
				int v2 = array[num4 + 1];
				int v3 = array[num4 + 2];
				int num5 = num2 + k;
				data[num5] = new Triangle(num5, v, v2, v3, subMeshIndex);
			}
			num2 += num3;
		}
	}

	public Vector2[] GetUVs2D(int channel)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (vertUV2D != null && vertUV2D[channel] != null)
		{
			return vertUV2D[channel].Data;
		}
		return null;
	}

	public Vector3[] GetUVs3D(int channel)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (vertUV3D != null && vertUV3D[channel] != null)
		{
			return vertUV3D[channel].Data;
		}
		return null;
	}

	public Vector4[] GetUVs4D(int channel)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (vertUV4D != null && vertUV4D[channel] != null)
		{
			return vertUV4D[channel].Data;
		}
		return null;
	}

	public void GetUVs(int channel, List<Vector2> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs == null)
		{
			throw new ArgumentNullException("uvs");
		}
		uvs.Clear();
		if (vertUV2D != null && vertUV2D[channel] != null)
		{
			Vector2[] data = vertUV2D[channel].Data;
			if (data != null)
			{
				uvs.AddRange(data);
			}
		}
	}

	public void GetUVs(int channel, List<Vector3> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs == null)
		{
			throw new ArgumentNullException("uvs");
		}
		uvs.Clear();
		if (vertUV3D != null && vertUV3D[channel] != null)
		{
			Vector3[] data = vertUV3D[channel].Data;
			if (data != null)
			{
				uvs.AddRange(data);
			}
		}
	}

	public void GetUVs(int channel, List<Vector4> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs == null)
		{
			throw new ArgumentNullException("uvs");
		}
		uvs.Clear();
		if (vertUV4D != null && vertUV4D[channel] != null)
		{
			Vector4[] data = vertUV4D[channel].Data;
			if (data != null)
			{
				uvs.AddRange(data);
			}
		}
	}

	public void SetUVs(int channel, Vector2[] uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Length != 0)
		{
			if (vertUV2D == null)
			{
				vertUV2D = new UVChannels<Vector2>();
			}
			int num = uvs.Length;
			ResizableArray<Vector2> resizableArray = vertUV2D[channel];
			if (resizableArray != null)
			{
				resizableArray.Resize(num);
			}
			else
			{
				resizableArray = new ResizableArray<Vector2>(num, num);
				vertUV2D[channel] = resizableArray;
			}
			Vector2[] data = resizableArray.Data;
			uvs.CopyTo(data, 0);
		}
		else if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
		if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
	}

	public void SetUVs(int channel, Vector3[] uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Length != 0)
		{
			if (vertUV3D == null)
			{
				vertUV3D = new UVChannels<Vector3>();
			}
			int num = uvs.Length;
			ResizableArray<Vector3> resizableArray = vertUV3D[channel];
			if (resizableArray != null)
			{
				resizableArray.Resize(num);
			}
			else
			{
				resizableArray = new ResizableArray<Vector3>(num, num);
				vertUV3D[channel] = resizableArray;
			}
			Vector3[] data = resizableArray.Data;
			uvs.CopyTo(data, 0);
		}
		else if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
		if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
	}

	public void SetUVs(int channel, Vector4[] uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Length != 0)
		{
			if (vertUV4D == null)
			{
				vertUV4D = new UVChannels<Vector4>();
			}
			int num = uvs.Length;
			ResizableArray<Vector4> resizableArray = vertUV4D[channel];
			if (resizableArray != null)
			{
				resizableArray.Resize(num);
			}
			else
			{
				resizableArray = new ResizableArray<Vector4>(num, num);
				vertUV4D[channel] = resizableArray;
			}
			Vector4[] data = resizableArray.Data;
			uvs.CopyTo(data, 0);
		}
		else if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
		if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
	}

	public void SetUVs(int channel, List<Vector2> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			if (vertUV2D == null)
			{
				vertUV2D = new UVChannels<Vector2>();
			}
			int count = uvs.Count;
			ResizableArray<Vector2> resizableArray = vertUV2D[channel];
			if (resizableArray != null)
			{
				resizableArray.Resize(count);
			}
			else
			{
				resizableArray = new ResizableArray<Vector2>(count, count);
				vertUV2D[channel] = resizableArray;
			}
			Vector2[] data = resizableArray.Data;
			uvs.CopyTo(data, 0);
		}
		else if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
		if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
	}

	public void SetUVs(int channel, List<Vector3> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			if (vertUV3D == null)
			{
				vertUV3D = new UVChannels<Vector3>();
			}
			int count = uvs.Count;
			ResizableArray<Vector3> resizableArray = vertUV3D[channel];
			if (resizableArray != null)
			{
				resizableArray.Resize(count);
			}
			else
			{
				resizableArray = new ResizableArray<Vector3>(count, count);
				vertUV3D[channel] = resizableArray;
			}
			Vector3[] data = resizableArray.Data;
			uvs.CopyTo(data, 0);
		}
		else if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
		if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
	}

	public void SetUVs(int channel, List<Vector4> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			if (vertUV4D == null)
			{
				vertUV4D = new UVChannels<Vector4>();
			}
			int count = uvs.Count;
			ResizableArray<Vector4> resizableArray = vertUV4D[channel];
			if (resizableArray != null)
			{
				resizableArray.Resize(count);
			}
			else
			{
				resizableArray = new ResizableArray<Vector4>(count, count);
				vertUV4D[channel] = resizableArray;
			}
			Vector4[] data = resizableArray.Data;
			uvs.CopyTo(data, 0);
		}
		else if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
		if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
	}

	public void SetUVsAuto(int channel, List<Vector4> uvs)
	{
		if (channel < 0 || channel >= UVChannelCount)
		{
			throw new ArgumentOutOfRangeException("channel");
		}
		if (uvs != null && uvs.Count > 0)
		{
			int usedUVComponents = MeshUtils.GetUsedUVComponents(uvs);
			if (usedUVComponents <= 2)
			{
				Vector2[] uvs2 = MeshUtils.ConvertUVsTo2D(uvs);
				SetUVs(channel, uvs2);
			}
			else if (usedUVComponents == 3)
			{
				Vector3[] uvs3 = MeshUtils.ConvertUVsTo3D(uvs);
				SetUVs(channel, uvs3);
			}
			else
			{
				SetUVs(channel, uvs);
			}
			return;
		}
		if (vertUV2D != null)
		{
			vertUV2D[channel] = null;
		}
		if (vertUV3D != null)
		{
			vertUV3D[channel] = null;
		}
		if (vertUV4D != null)
		{
			vertUV4D[channel] = null;
		}
	}

	public BlendShape[] GetAllBlendShapes()
	{
		if (blendShapes == null)
		{
			return null;
		}
		BlendShape[] array = new BlendShape[blendShapes.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = blendShapes[i].ToBlendShape();
		}
		return array;
	}

	public BlendShape GetBlendShape(int blendShapeIndex)
	{
		if (blendShapes == null || blendShapeIndex < 0 || blendShapeIndex >= blendShapes.Length)
		{
			throw new ArgumentOutOfRangeException("blendShapeIndex");
		}
		return blendShapes[blendShapeIndex].ToBlendShape();
	}

	public void ClearBlendShapes()
	{
		if (blendShapes != null)
		{
			blendShapes.Clear();
			blendShapes = null;
		}
	}

	public void AddBlendShape(BlendShape blendShape)
	{
		BlendShapeFrame[] frames = blendShape.Frames;
		if (frames == null || frames.Length == 0)
		{
			throw new ArgumentException("The frames cannot be null or empty.", "blendShape");
		}
		if (blendShapes == null)
		{
			blendShapes = new ResizableArray<BlendShapeContainer>(4, 0);
		}
		BlendShapeContainer item = new BlendShapeContainer(blendShape);
		blendShapes.Add(item);
	}

	public void AddBlendShapes(BlendShape[] blendShapes)
	{
		if (blendShapes == null)
		{
			throw new ArgumentNullException("blendShapes");
		}
		if (this.blendShapes == null)
		{
			this.blendShapes = new ResizableArray<BlendShapeContainer>(Math.Max(4, blendShapes.Length), 0);
		}
		for (int i = 0; i < blendShapes.Length; i++)
		{
			BlendShapeFrame[] frames = blendShapes[i].Frames;
			if (frames == null || frames.Length == 0)
			{
				throw new ArgumentException($"The frames of blend shape at index {i} cannot be null or empty.", "blendShapes");
			}
			BlendShapeContainer item = new BlendShapeContainer(blendShapes[i]);
			this.blendShapes.Add(item);
		}
	}

	public void Initialize(Mesh mesh)
	{
		if (mesh == null)
		{
			throw new ArgumentNullException("mesh");
		}
		Vertices = mesh.vertices;
		Normals = mesh.normals;
		Tangents = mesh.tangents;
		Colors = mesh.colors;
		BoneWeights = mesh.boneWeights;
		bindposes = mesh.bindposes;
		for (int i = 0; i < UVChannelCount; i++)
		{
			List<Vector4> meshUVs = MeshUtils.GetMeshUVs(mesh, i);
			SetUVsAuto(i, meshUVs);
		}
		BlendShape[] meshBlendShapes = MeshUtils.GetMeshBlendShapes(mesh);
		if (meshBlendShapes != null && meshBlendShapes.Length != 0)
		{
			AddBlendShapes(meshBlendShapes);
		}
		ClearSubMeshes();
		int num = mesh.subMeshCount;
		int[][] array = new int[num][];
		for (int j = 0; j < num; j++)
		{
			array[j] = mesh.GetTriangles(j);
		}
		AddSubMeshTriangles(array);
	}

	public void SimplifyMesh(float quality)
	{
		quality = Mathf.Clamp01(quality);
		int deletedTris = 0;
		ResizableArray<bool> deleted = new ResizableArray<bool>(20);
		ResizableArray<bool> deleted2 = new ResizableArray<bool>(20);
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		int num = length;
		_ = vertices.Data;
		int num2 = Mathf.RoundToInt((float)length * quality);
		int maxIterationCount = MaxIterationCount;
		double agressiveness = Agressiveness;
		for (int i = 0; i < maxIterationCount; i++)
		{
			if (num - deletedTris <= num2)
			{
				break;
			}
			if (i % 5 == 0)
			{
				UpdateMesh(i);
				data = triangles.Data;
				length = triangles.Length;
				_ = vertices.Data;
			}
			for (int j = 0; j < length; j++)
			{
				data[j].dirty = false;
			}
			double num3 = 1E-09 * Math.Pow(i + 3, agressiveness);
			if (verbose)
			{
				Debug.LogFormat("iteration {0} - triangles {1} threshold {2}", i, num - deletedTris, num3);
			}
			RemoveVertexPass(num, num2, num3, deleted, deleted2, ref deletedTris);
		}
		CompactMesh();
		if (verbose)
		{
			Debug.LogFormat("Finished simplification with triangle count {0}", triangles.Length);
		}
	}

	public void SimplifyMeshLossless()
	{
		int deletedTris = 0;
		ResizableArray<bool> deleted = new ResizableArray<bool>(0);
		ResizableArray<bool> deleted2 = new ResizableArray<bool>(0);
		Triangle[] data = triangles.Data;
		int length = triangles.Length;
		int startTrisCount = length;
		_ = vertices.Data;
		for (int i = 0; i < 9999; i++)
		{
			UpdateMesh(i);
			data = triangles.Data;
			length = triangles.Length;
			_ = vertices.Data;
			for (int j = 0; j < length; j++)
			{
				data[j].dirty = false;
			}
			double threshold = 0.001;
			if (verbose)
			{
				Debug.LogFormat("Lossless iteration {0} - triangles {1}", i, length);
			}
			RemoveVertexPass(startTrisCount, 0, threshold, deleted, deleted2, ref deletedTris);
			if (deletedTris <= 0)
			{
				break;
			}
			deletedTris = 0;
		}
		CompactMesh();
		if (verbose)
		{
			Debug.LogFormat("Finished simplification with triangle count {0}", triangles.Length);
		}
	}

	public Mesh ToMesh()
	{
		Vector3[] array = Vertices;
		Vector3[] normals = Normals;
		Vector4[] tangents = Tangents;
		Color[] colors = Colors;
		BoneWeight[] boneWeights = BoneWeights;
		int[][] allSubMeshTriangles = GetAllSubMeshTriangles();
		BlendShape[] allBlendShapes = GetAllBlendShapes();
		List<Vector2>[] array2 = null;
		List<Vector3>[] array3 = null;
		List<Vector4>[] array4 = null;
		if (vertUV2D != null)
		{
			array2 = new List<Vector2>[UVChannelCount];
			for (int i = 0; i < UVChannelCount; i++)
			{
				if (vertUV2D[i] != null)
				{
					List<Vector2> list = new List<Vector2>(array.Length);
					GetUVs(i, list);
					array2[i] = list;
				}
			}
		}
		if (vertUV3D != null)
		{
			array3 = new List<Vector3>[UVChannelCount];
			for (int j = 0; j < UVChannelCount; j++)
			{
				if (vertUV3D[j] != null)
				{
					List<Vector3> list2 = new List<Vector3>(array.Length);
					GetUVs(j, list2);
					array3[j] = list2;
				}
			}
		}
		if (vertUV4D != null)
		{
			array4 = new List<Vector4>[UVChannelCount];
			for (int k = 0; k < UVChannelCount; k++)
			{
				if (vertUV4D[k] != null)
				{
					List<Vector4> list3 = new List<Vector4>(array.Length);
					GetUVs(k, list3);
					array4[k] = list3;
				}
			}
		}
		return MeshUtils.CreateMesh(array, allSubMeshTriangles, normals, tangents, colors, boneWeights, array2, array3, array4, bindposes, allBlendShapes);
	}
}
