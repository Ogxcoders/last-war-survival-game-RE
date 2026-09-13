using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.U2D;

[StructLayout(LayoutKind.Sequential)]
[RequiredByNativeCode]
[NativeHeader("Runtime/2D/Renderer/SpriteRendererGroup.h")]
internal class SpriteRendererGroup
{
	public unsafe static void AddRenderers(NativeArray<SpriteIntermediateRendererInfo> renderers)
	{
		AddRenderers(renderers.GetUnsafeReadOnlyPtr(), renderers.Length);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private unsafe static extern void AddRenderers(void* renderers, int count);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Clear();
}
