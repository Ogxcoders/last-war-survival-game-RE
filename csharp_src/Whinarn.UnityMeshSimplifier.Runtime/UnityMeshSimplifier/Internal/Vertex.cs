using System;
using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier.Internal;

internal struct Vertex : IEquatable<Vertex>
{
	public int index;

	public Vector3d p;

	public int tstart;

	public int tcount;

	public SymmetricMatrix q;

	public bool borderEdge;

	public bool uvSeamEdge;

	public bool uvFoldoverEdge;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Vertex(int index, Vector3d p)
	{
		this.index = index;
		this.p = p;
		tstart = 0;
		tcount = 0;
		q = default(SymmetricMatrix);
		borderEdge = true;
		uvSeamEdge = false;
		uvFoldoverEdge = false;
	}

	public override int GetHashCode()
	{
		return index;
	}

	public override bool Equals(object obj)
	{
		if (obj is Vertex vertex)
		{
			return index == vertex.index;
		}
		return false;
	}

	public bool Equals(Vertex other)
	{
		return index == other.index;
	}
}
