using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.VFX;

[StaticAccessor("GetVFXManager()", StaticAccessorType.Dot)]
[RequiredByNativeCode]
[NativeHeader("Modules/VFX/Public/VFXManager.h")]
public static class VFXManager
{
	public static extern float fixedTimeStep
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	public static extern float maxDeltaTime
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	internal static extern string renderPipeSettingsPath
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern VisualEffect[] GetComponents();

	public static void ProcessCamera(Camera cam)
	{
		PrepareCamera(cam);
		ProcessCameraCommand(cam, null);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void PrepareCamera(Camera cam);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void ProcessCameraCommand(Camera cam, CommandBuffer cmd);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern VFXCameraBufferTypes IsCameraBufferNeeded(Camera cam);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void SetCameraBuffer(Camera cam, VFXCameraBufferTypes type, Texture buffer, int x, int y, int width, int height);
}
