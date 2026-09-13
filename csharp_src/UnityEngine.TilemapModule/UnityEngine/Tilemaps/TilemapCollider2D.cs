using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps;

[NativeType(Header = "Modules/Tilemap/Public/TilemapCollider2D.h")]
[RequireComponent(typeof(Tilemap))]
public sealed class TilemapCollider2D : Collider2D
{
	public extern uint maximumTileChangeCount
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public extern float extrusionFactor
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}
}
