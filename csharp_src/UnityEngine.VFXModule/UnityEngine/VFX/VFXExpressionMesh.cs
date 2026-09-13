using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[StaticAccessor("VFXExpressionMeshFunctions", StaticAccessorType.DoubleColon)]
[NativeType(Header = "Modules/VFX/Public/VFXExpressionMeshFunctions.h")]
[RequiredByNativeCode]
internal class VFXExpressionMesh
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int GetVertexStride(Mesh mesh);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern int GetChannelOffset(Mesh mesh, int channelIndex);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern float GetFloat(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride);

	internal static Vector2 GetFloat2(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride)
	{
		GetFloat2_Injected(mesh, vertexIndex, channelOffset, vertexStride, out var ret);
		return ret;
	}

	internal static Vector3 GetFloat3(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride)
	{
		GetFloat3_Injected(mesh, vertexIndex, channelOffset, vertexStride, out var ret);
		return ret;
	}

	internal static Vector4 GetFloat4(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride)
	{
		GetFloat4_Injected(mesh, vertexIndex, channelOffset, vertexStride, out var ret);
		return ret;
	}

	internal static Vector4 GetColor(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride)
	{
		GetColor_Injected(mesh, vertexIndex, channelOffset, vertexStride, out var ret);
		return ret;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetFloat2_Injected(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride, out Vector2 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetFloat3_Injected(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetFloat4_Injected(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride, out Vector4 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetColor_Injected(Mesh mesh, int vertexIndex, int channelOffset, int vertexStride, out Vector4 ret);
}
