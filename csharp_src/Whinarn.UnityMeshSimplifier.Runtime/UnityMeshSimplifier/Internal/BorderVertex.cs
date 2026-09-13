using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier.Internal;

internal struct BorderVertex
{
	public int index;

	public int hash;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BorderVertex(int index, int hash)
	{
		this.index = index;
		this.hash = hash;
	}
}
