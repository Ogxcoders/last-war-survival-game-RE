using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal static class MeshPool
{
	private static readonly Stack<Mesh> s_Pool;

	public static void Init()
	{
	}

	static MeshPool()
	{
		s_Pool = new Stack<Mesh>();
		for (int i = 0; i < 32; i++)
		{
			Mesh mesh = new Mesh();
			mesh.MarkDynamic();
			s_Pool.Push(mesh);
		}
	}

	public static Mesh Rent()
	{
		Mesh mesh;
		while (0 < s_Pool.Count)
		{
			mesh = s_Pool.Pop();
			if ((bool)mesh)
			{
				return mesh;
			}
		}
		mesh = new Mesh();
		mesh.MarkDynamic();
		return mesh;
	}

	public static void Return(Mesh mesh)
	{
		if ((bool)mesh && !s_Pool.Contains(mesh))
		{
			mesh.Clear(keepVertexLayout: false);
			s_Pool.Push(mesh);
		}
	}
}
