using System.Runtime.CompilerServices;
using UnityEngine;

namespace UnityMeshSimplifier.Internal;

internal class BlendShapeFrameContainer
{
	private readonly float frameWeight;

	private readonly ResizableArray<Vector3> deltaVertices;

	private readonly ResizableArray<Vector3> deltaNormals;

	private readonly ResizableArray<Vector3> deltaTangents;

	public BlendShapeFrameContainer(BlendShapeFrame frame)
	{
		frameWeight = frame.FrameWeight;
		deltaVertices = new ResizableArray<Vector3>(frame.DeltaVertices);
		deltaNormals = new ResizableArray<Vector3>(frame.DeltaNormals);
		deltaTangents = new ResizableArray<Vector3>(frame.DeltaTangents);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void MoveVertexElement(int dst, int src)
	{
		deltaVertices[dst] = deltaVertices[src];
		deltaNormals[dst] = deltaNormals[src];
		deltaTangents[dst] = deltaTangents[src];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void InterpolateVertexAttributes(int dst, int i0, int i1, int i2, ref Vector3 barycentricCoord)
	{
		deltaVertices[dst] = deltaVertices[i0] * barycentricCoord.x + deltaVertices[i1] * barycentricCoord.y + deltaVertices[i2] * barycentricCoord.z;
		deltaNormals[dst] = Vector3.Normalize(deltaNormals[i0] * barycentricCoord.x + deltaNormals[i1] * barycentricCoord.y + deltaNormals[i2] * barycentricCoord.z);
		deltaTangents[dst] = Vector3.Normalize(deltaTangents[i0] * barycentricCoord.x + deltaTangents[i1] * barycentricCoord.y + deltaTangents[i2] * barycentricCoord.z);
	}

	public void Resize(int length, bool trimExess = false)
	{
		deltaVertices.Resize(length, trimExess);
		deltaNormals.Resize(length, trimExess);
		deltaTangents.Resize(length, trimExess);
	}

	public BlendShapeFrame ToBlendShapeFrame()
	{
		Vector3[] array = deltaVertices.ToArray();
		Vector3[] array2 = deltaNormals.ToArray();
		Vector3[] array3 = deltaTangents.ToArray();
		return new BlendShapeFrame(frameWeight, array, array2, array3);
	}
}
