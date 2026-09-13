using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal class ShareMeshItem
{
	private static readonly Stack<ShareMeshItem> s_Pool = new Stack<ShareMeshItem>(128);

	public Mesh mesh;

	public List<bool> meshIndices = new List<bool>(16);

	public static ShareMeshItem Rent()
	{
		if (0 < s_Pool.Count)
		{
			return s_Pool.Pop();
		}
		return new ShareMeshItem();
	}

	public static void Return(ShareMeshItem m)
	{
		if (!s_Pool.Contains(m))
		{
			m.mesh = null;
			m.meshIndices.Clear();
			s_Pool.Push(m);
		}
	}
}
