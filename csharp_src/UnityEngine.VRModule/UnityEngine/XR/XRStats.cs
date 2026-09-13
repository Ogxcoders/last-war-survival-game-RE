using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.XR;

[NativeConditional("ENABLE_VR")]
public static class XRStats
{
	[Obsolete("gpuTimeLastFrame is deprecated. Use XRStats.TryGetGPUTimeLastFrame instead.", false)]
	public static float gpuTimeLastFrame
	{
		get
		{
			if (TryGetGPUTimeLastFrame(out var result))
			{
				return result;
			}
			return 0f;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern bool TryGetGPUTimeLastFrame(out float gpuTimeLastFrame);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern bool TryGetDroppedFrameCount(out int droppedFrameCount);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[StaticAccessor("GetIVRDevice()", StaticAccessorType.ArrowWithDefaultReturnIfNull)]
	public static extern bool TryGetFramePresentCount(out int framePresentCount);
}
