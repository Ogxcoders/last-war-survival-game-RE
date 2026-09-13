using System.Runtime.CompilerServices;

namespace UnityMeshSimplifier.Internal;

internal struct Ref
{
	public int tid;

	public int tvertex;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Set(int tid, int tvertex)
	{
		this.tid = tid;
		this.tvertex = tvertex;
	}
}
