using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine;

[NativeHeader("Modules/Grid/Public/GridMarshalling.h")]
[RequireComponent(typeof(Transform))]
[NativeType(Header = "Modules/Grid/Public/Grid.h")]
public sealed class Grid : GridLayout
{
	public new Vector3 cellSize
	{
		[FreeFunction("GridBindings::GetCellSize", HasExplicitThis = true)]
		get
		{
			get_cellSize_Injected(out var ret);
			return ret;
		}
		[FreeFunction("GridBindings::SetCellSize", HasExplicitThis = true)]
		set
		{
			set_cellSize_Injected(ref value);
		}
	}

	public new Vector3 cellGap
	{
		[FreeFunction("GridBindings::GetCellGap", HasExplicitThis = true)]
		get
		{
			get_cellGap_Injected(out var ret);
			return ret;
		}
		[FreeFunction("GridBindings::SetCellGap", HasExplicitThis = true)]
		set
		{
			set_cellGap_Injected(ref value);
		}
	}

	public new extern CellLayout cellLayout
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public new extern CellSwizzle cellSwizzle
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public Vector3 GetCellCenterLocal(Vector3Int position)
	{
		return CellToLocalInterpolated(position + GetLayoutCellCenter());
	}

	public Vector3 GetCellCenterWorld(Vector3Int position)
	{
		return LocalToWorld(CellToLocalInterpolated(position + GetLayoutCellCenter()));
	}

	[FreeFunction("GridBindings::CellSwizzle")]
	public static Vector3 Swizzle(CellSwizzle swizzle, Vector3 position)
	{
		Swizzle_Injected(swizzle, ref position, out var ret);
		return ret;
	}

	[FreeFunction("GridBindings::InverseCellSwizzle")]
	public static Vector3 InverseSwizzle(CellSwizzle swizzle, Vector3 position)
	{
		InverseSwizzle_Injected(swizzle, ref position, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_cellSize_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_cellSize_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void get_cellGap_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[SpecialName]
	private extern void set_cellGap_Injected(ref Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void Swizzle_Injected(CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InverseSwizzle_Injected(CellSwizzle swizzle, ref Vector3 position, out Vector3 ret);
}
