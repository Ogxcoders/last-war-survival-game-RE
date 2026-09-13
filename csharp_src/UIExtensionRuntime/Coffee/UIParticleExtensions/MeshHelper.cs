using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class MeshHelper
{
	private static readonly List<CombineInstanceEx> s_CachedInstance;

	private static int count;

	public static List<bool> activeMeshIndices { get; private set; }

	public static void Init()
	{
		activeMeshIndices = new List<bool>();
	}

	static MeshHelper()
	{
		s_CachedInstance = new List<CombineInstanceEx>(8);
		for (int i = 0; i < 8; i++)
		{
			s_CachedInstance.Add(new CombineInstanceEx());
		}
	}

	private static CombineInstanceEx Get(int index, long hash)
	{
		if (0 < count && s_CachedInstance[count - 1].hash == hash)
		{
			return s_CachedInstance[count - 1];
		}
		if (s_CachedInstance.Count <= count)
		{
			CombineInstanceEx item = new CombineInstanceEx();
			s_CachedInstance.Add(item);
		}
		CombineInstanceEx combineInstanceEx = s_CachedInstance[count];
		combineInstanceEx.hash = hash;
		if (combineInstanceEx.index != -1)
		{
			return combineInstanceEx;
		}
		combineInstanceEx.index = index;
		count++;
		return combineInstanceEx;
	}

	public static Mesh GetTemporaryMesh()
	{
		return MeshPool.Rent();
	}

	public static void Push(int index, long hash, Mesh mesh, Matrix4x4 transform)
	{
		if (mesh.vertexCount <= 0)
		{
			DiscardTemporaryMesh(mesh);
			return;
		}
		CombineInstanceEx combineInstanceEx = Get(index, hash);
		combineInstanceEx.Push(mesh, transform);
		activeMeshIndices[combineInstanceEx.index] = true;
	}

	public static void Clear()
	{
		count = 0;
		activeMeshIndices.Clear();
		foreach (CombineInstanceEx item in s_CachedInstance)
		{
			item.Clear();
		}
	}

	public static void CombineMesh(Mesh result)
	{
		if (count != 0)
		{
			for (int i = 0; i < count; i++)
			{
				s_CachedInstance[i].Combine();
			}
			CombineInstance[] array = CombineInstanceArrayPool.Get(s_CachedInstance, count);
			result.CombineMeshes(array, mergeSubMeshes: false, useMatrices: true);
			array.Clear();
			result.RecalculateBounds();
		}
	}

	public static void DiscardTemporaryMesh(Mesh mesh)
	{
		MeshPool.Return(mesh);
	}
}
