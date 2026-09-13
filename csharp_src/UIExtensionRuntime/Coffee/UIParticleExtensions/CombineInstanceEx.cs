using System.Collections.Generic;
using UnityEngine;

namespace Coffee.UIParticleExtensions;

internal class CombineInstanceEx
{
	private int count;

	public long hash = -1L;

	public int index = -1;

	private readonly List<CombineInstance> combineInstances = new List<CombineInstance>(32);

	public Mesh mesh;

	public Matrix4x4 transform;

	public void Combine()
	{
		switch (count)
		{
		case 0:
			return;
		case 1:
			mesh = combineInstances[0].mesh;
			transform = combineInstances[0].transform;
			return;
		}
		CombineInstance[] array = CombineInstanceArrayPool.Get(combineInstances);
		mesh = MeshPool.Rent();
		mesh.CombineMeshes(array, mergeSubMeshes: true, useMatrices: true);
		transform = Matrix4x4.identity;
		array.Clear();
	}

	public void Clear()
	{
		for (int i = 0; i < combineInstances.Count; i++)
		{
			CombineInstance value = combineInstances[i];
			MeshPool.Return(value.mesh);
			value.mesh = null;
			combineInstances[i] = value;
		}
		combineInstances.Clear();
		MeshPool.Return(mesh);
		mesh = null;
		count = 0;
		hash = -1L;
		index = -1;
	}

	public void Push(Mesh mesh, Matrix4x4 transform)
	{
		combineInstances.Add(new CombineInstance
		{
			mesh = mesh,
			transform = transform
		});
		count++;
	}
}
