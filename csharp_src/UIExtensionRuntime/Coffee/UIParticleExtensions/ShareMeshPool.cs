using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class ShareMeshPool
{
	private static readonly Dictionary<string, ShareMeshItem> s_Pool = new Dictionary<string, ShareMeshItem>(512);

	public static void OnFrameBegin()
	{
		Dictionary<string, ShareMeshItem>.Enumerator enumerator = s_Pool.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ShareMeshItem value = enumerator.Current.Value;
			MeshPool.Return(value.mesh);
			ShareMeshItem.Return(value);
		}
		s_Pool?.Clear();
	}

	public static ShareMeshItem PushMesh(string tag, Mesh mesh, List<bool> meshIndices)
	{
		ShareMeshItem shareMeshItem = ShareMeshItem.Rent();
		shareMeshItem.mesh = mesh;
		shareMeshItem.meshIndices.AddRange(meshIndices);
		s_Pool.Add(tag, shareMeshItem);
		return shareMeshItem;
	}

	public static ShareMeshItem GetMesh(string tag)
	{
		if (s_Pool.TryGetValue(tag, out var value))
		{
			return value;
		}
		return null;
	}
}
