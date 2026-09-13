using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine.U2D;

[NativeHeader("Runtime/Graphics/Mesh/SpriteRenderer.h")]
[NativeHeader("Runtime/2D/Common/SpriteDataAccess.h")]
public static class SpriteRendererDataAccessExtensions
{
	internal unsafe static void SetDeformableBuffer(this SpriteRenderer spriteRenderer, NativeArray<byte> src)
	{
		if (spriteRenderer.sprite == null)
		{
			throw new ArgumentException($"spriteRenderer does not have a valid sprite set.");
		}
		if (src.Length != SpriteDataAccessExtensions.GetPrimaryVertexStreamSize(spriteRenderer.sprite))
		{
			throw new InvalidOperationException($"custom sprite vertex data size must match sprite asset's vertex data size {src.Length} {SpriteDataAccessExtensions.GetPrimaryVertexStreamSize(spriteRenderer.sprite)}");
		}
		SetDeformableBuffer(spriteRenderer, src.GetUnsafeReadOnlyPtr(), src.Length);
	}

	internal unsafe static void SetDeformableBuffer(this SpriteRenderer spriteRenderer, NativeArray<Vector3> src)
	{
		if (spriteRenderer.sprite == null)
		{
			throw new ArgumentException($"spriteRenderer does not have a valid sprite set.");
		}
		if (src.Length != spriteRenderer.sprite.GetVertexCount())
		{
			throw new InvalidOperationException($"The src length {src.Length} must match the vertex count of source Sprite {spriteRenderer.sprite.GetVertexCount()}.");
		}
		SetDeformableBuffer(spriteRenderer, src.GetUnsafeReadOnlyPtr(), src.Length);
	}

	internal unsafe static bool IsUsingDeformableBuffer(this SpriteRenderer spriteRenderer, IntPtr buffer)
	{
		return IsUsingDeformableBuffer(spriteRenderer, (void*)buffer);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void DeactivateDeformableBuffer([NotNull] this SpriteRenderer renderer);

	internal static void SetLocalAABB([NotNull] this SpriteRenderer renderer, Bounds aabb)
	{
		SetLocalAABB_Injected(renderer, ref aabb);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void SetDeformableBuffer([NotNull] SpriteRenderer spriteRenderer, void* src, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern bool IsUsingDeformableBuffer([NotNull] SpriteRenderer spriteRenderer, void* buffer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetLocalAABB_Injected(SpriteRenderer renderer, ref Bounds aabb);
}
