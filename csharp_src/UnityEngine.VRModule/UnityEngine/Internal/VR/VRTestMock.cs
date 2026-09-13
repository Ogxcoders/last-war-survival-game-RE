using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.XR;

namespace UnityEngine.Internal.VR;

[NativeHeader("Modules/VR/Test/VRTestMock.bindings.h")]
[StaticAccessor("VRTestMockBindings", StaticAccessorType.DoubleColon)]
public static class VRTestMock
{
	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void Reset();

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void AddTrackedDevice(XRNode nodeType);

	public static void UpdateTrackedDevice(XRNode nodeType, Vector3 position, Quaternion rotation)
	{
		UpdateTrackedDevice_Injected(nodeType, ref position, ref rotation);
	}

	public static void UpdateLeftEye(Vector3 position, Quaternion rotation)
	{
		UpdateLeftEye_Injected(ref position, ref rotation);
	}

	public static void UpdateRightEye(Vector3 position, Quaternion rotation)
	{
		UpdateRightEye_Injected(ref position, ref rotation);
	}

	public static void UpdateCenterEye(Vector3 position, Quaternion rotation)
	{
		UpdateCenterEye_Injected(ref position, ref rotation);
	}

	public static void UpdateHead(Vector3 position, Quaternion rotation)
	{
		UpdateHead_Injected(ref position, ref rotation);
	}

	public static void UpdateLeftHand(Vector3 position, Quaternion rotation)
	{
		UpdateLeftHand_Injected(ref position, ref rotation);
	}

	public static void UpdateRightHand(Vector3 position, Quaternion rotation)
	{
		UpdateRightHand_Injected(ref position, ref rotation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void AddController(string controllerName);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void UpdateControllerAxis(string controllerName, int axis, float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	public static extern void UpdateControllerButton(string controllerName, int button, bool pressed);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateTrackedDevice_Injected(XRNode nodeType, ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateLeftEye_Injected(ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateRightEye_Injected(ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateCenterEye_Injected(ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateHead_Injected(ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateLeftHand_Injected(ref Vector3 position, ref Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void UpdateRightHand_Injected(ref Vector3 position, ref Quaternion rotation);
}
