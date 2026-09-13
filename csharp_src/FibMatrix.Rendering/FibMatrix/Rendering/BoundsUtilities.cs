using System.Collections.Generic;
using UnityEngine;

namespace FibMatrix.Rendering;

public static class BoundsUtilities
{
	public static Bounds CalculateBounds(List<GameObject> gos)
	{
		if (gos == null || gos.Count == 0)
		{
			return default(Bounds);
		}
		Bounds src = CalculateBounds(gos[0]);
		for (int i = 1; i < gos.Count; i++)
		{
			EncapsulateMixMax(ref src, CalculateBounds(gos[i]));
		}
		return src;
	}

	public static Bounds CalculateBounds(GameObject go)
	{
		if (go == null)
		{
			return default(Bounds);
		}
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		if (componentsInChildren == null)
		{
			return default(Bounds);
		}
		Bounds src = componentsInChildren[0].bounds;
		for (int i = 1; i < componentsInChildren.Length; i++)
		{
			EncapsulateMixMax(ref src, componentsInChildren[i].bounds);
		}
		return src;
	}

	public static Bounds ReCalculateBounds(Mesh mesh)
	{
		Bounds src = new Bounds(mesh.vertices[0], Vector3.zero);
		for (int i = 1; i < mesh.vertices.Length; i++)
		{
			EncapsulateMixMax(ref src, mesh.vertices[i]);
		}
		mesh.bounds = src;
		return src;
	}

	public static Bounds EncapsulateMixMax(ref Bounds src, Bounds tgt)
	{
		src.min = Vector3.Min(src.min, tgt.min);
		src.max = Vector3.Max(src.max, tgt.max);
		return src;
	}

	public static Bounds EncapsulateMixMax(ref Bounds src, Vector3 tgt)
	{
		src.min = Vector3.Min(src.min, tgt);
		src.max = Vector3.Max(src.max, tgt);
		return src;
	}
}
