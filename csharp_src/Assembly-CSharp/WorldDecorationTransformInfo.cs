using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public struct WorldDecorationTransformInfo
{
	public Vector3 pos;

	public Vector3 rotation;

	public Vector3 scale;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static WorldDecorationTransformInfo Create(Vector3 pos, Vector3 rotation, Vector3 scale)
	{
		return new WorldDecorationTransformInfo
		{
			pos = pos,
			rotation = rotation,
			scale = scale
		};
	}
}
