using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.XR.WSA;

[NativeConditional("ENABLE_HOLOLENS_MODULE")]
[StaticAccessor("HolographicEmulation::HolographicEmulationManager::Get()", StaticAccessorType.Dot)]
[NativeHeader("Modules/VR/HoloLens/HolographicEmulation/HolographicEmulationManager.h")]
internal class HolographicAutomation
{
	private static SimulatedBody s_Body = new SimulatedBody();

	private static SimulatedHead s_Head = new SimulatedHead();

	private static SimulatedHand s_LeftHand = new SimulatedHand(Handedness.Left);

	private static SimulatedHand s_RightHand = new SimulatedHand(Handedness.Right);

	private static SimulatedSpatialController s_LeftController = new SimulatedSpatialController(Handedness.Left);

	private static SimulatedSpatialController s_RightController = new SimulatedSpatialController(Handedness.Right);

	public static SimulatedBody simulatedBody => s_Body;

	public static SimulatedHead simulatedHead => s_Head;

	public static SimulatedHand simulatedLeftHand => s_LeftHand;

	public static SimulatedHand simulatedRightHand => s_RightHand;

	public static SimulatedSpatialController simulatedLeftController => s_LeftController;

	public static SimulatedSpatialController simulatedRightController => s_RightController;

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void Initialize();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void Shutdown();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void LoadRoom(string id);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetEmulationMode(EmulationMode mode);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetPlaymodeInputType(PlaymodeInputType inputType);

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeName("ResetEmulationState")]
	internal static extern void Reset();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void PerformGesture(Handedness hand, SimulatedGesture gesture);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void PerformButtonPress(Handedness hand, SimulatedControllerPress buttonPress);

	[NativeConditional("ENABLE_HOLOLENS_MODULE", StubReturnStatement = "Vector3f::zero")]
	internal static Vector3 GetBodyPosition()
	{
		GetBodyPosition_Injected(out var ret);
		return ret;
	}

	internal static void SetBodyPosition(Vector3 position)
	{
		SetBodyPosition_Injected(ref position);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern float GetBodyRotation();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetBodyRotation(float degrees);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern float GetBodyHeight();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetBodyHeight(float degrees);

	[NativeConditional("ENABLE_HOLOLENS_MODULE", StubReturnStatement = "Vector3f::zero")]
	internal static Vector3 GetHeadRotation()
	{
		GetHeadRotation_Injected(out var ret);
		return ret;
	}

	internal static void SetHeadRotation(Vector3 degrees)
	{
		SetHeadRotation_Injected(ref degrees);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern float GetHeadDiameter();

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetHeadDiameter(float degrees);

	[NativeConditional("ENABLE_HOLOLENS_MODULE", StubReturnStatement = "Vector3f::zero")]
	internal static Vector3 GetHandPosition(Handedness hand)
	{
		GetHandPosition_Injected(hand, out var ret);
		return ret;
	}

	internal static void SetHandPosition(Handedness hand, Vector3 position)
	{
		SetHandPosition_Injected(hand, ref position);
	}

	[NativeConditional("ENABLE_HOLOLENS_MODULE", StubReturnStatement = "Quaternionf::identity()")]
	internal static Quaternion GetHandOrientation(Handedness hand)
	{
		GetHandOrientation_Injected(hand, out var ret);
		return ret;
	}

	internal static bool TrySetHandOrientation(Handedness hand, Quaternion orientation)
	{
		return TrySetHandOrientation_Injected(hand, ref orientation);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetHandActivated(Handedness hand);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void SetHandActivated(Handedness hand, bool activated);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetHandVisible(Handedness hand);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void EnsureHandVisible(Handedness hand);

	[NativeConditional("ENABLE_HOLOLENS_MODULE", StubReturnStatement = "Vector3f::zero")]
	internal static Vector3 GetControllerPosition(Handedness hand)
	{
		GetControllerPosition_Injected(hand, out var ret);
		return ret;
	}

	internal static bool TrySetControllerPosition(Handedness hand, Vector3 position)
	{
		return TrySetControllerPosition_Injected(hand, ref position);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetControllerActivated(Handedness hand);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TrySetControllerActivated(Handedness hand, bool activated);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool GetControllerVisible(Handedness hand);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryEnsureControllerVisible(Handedness hand);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetBodyPosition_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetBodyPosition_Injected(ref Vector3 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetHeadRotation_Injected(out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetHeadRotation_Injected(ref Vector3 degrees);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetHandPosition_Injected(Handedness hand, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void SetHandPosition_Injected(Handedness hand, ref Vector3 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetHandOrientation_Injected(Handedness hand, out Quaternion ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool TrySetHandOrientation_Injected(Handedness hand, ref Quaternion orientation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetControllerPosition_Injected(Handedness hand, out Vector3 ret);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool TrySetControllerPosition_Injected(Handedness hand, ref Vector3 position);
}
