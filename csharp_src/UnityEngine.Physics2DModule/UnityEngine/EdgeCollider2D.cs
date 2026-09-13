using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Physics2D/Public/EdgeCollider2D.h")]
public sealed class EdgeCollider2D : Collider2D
{
	public extern float edgeRadius
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern int edgeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern int pointCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	public extern Vector2[] points
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public extern void Reset();
}
