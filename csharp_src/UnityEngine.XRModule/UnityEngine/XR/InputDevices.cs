using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[StructLayout(LayoutKind.Sequential)]
[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
[NativeConditional("ENABLE_VR")]
[UsedByNativeCode]
public class InputDevices
{
	private static List<InputDevice> s_InputDeviceList;

	public static event Action<InputDevice> deviceConnected;

	public static event Action<InputDevice> deviceDisconnected;

	public static event Action<InputDevice> deviceConfigChanged;

	public static InputDevice GetDeviceAtXRNode(XRNode node)
	{
		ulong deviceIdAtXRNode = InputTracking.GetDeviceIdAtXRNode(node);
		return new InputDevice(deviceIdAtXRNode);
	}

	public static void GetDevicesAtXRNode(XRNode node, List<InputDevice> inputDevices)
	{
		if (inputDevices == null)
		{
			throw new ArgumentNullException("inputDevices");
		}
		List<ulong> list = new List<ulong>();
		InputTracking.GetDeviceIdsAtXRNode_Internal(node, list);
		inputDevices.Clear();
		foreach (ulong item2 in list)
		{
			InputDevice item = new InputDevice(item2);
			if (item.isValid)
			{
				inputDevices.Add(item);
			}
		}
	}

	public static void GetDevices(List<InputDevice> inputDevices)
	{
		if (inputDevices == null)
		{
			throw new ArgumentNullException("inputDevices");
		}
		inputDevices.Clear();
		GetDevices_Internal(inputDevices);
	}

	[Obsolete("This API has been marked as deprecated and will be removed in future versions. Please use InputDevices.GetDevicesWithCharacteristics instead.")]
	public static void GetDevicesWithRole(InputDeviceRole role, List<InputDevice> inputDevices)
	{
		if (inputDevices == null)
		{
			throw new ArgumentNullException("inputDevices");
		}
		if (s_InputDeviceList == null)
		{
			s_InputDeviceList = new List<InputDevice>();
		}
		GetDevices_Internal(s_InputDeviceList);
		inputDevices.Clear();
		foreach (InputDevice s_InputDevice in s_InputDeviceList)
		{
			if (s_InputDevice.role == role)
			{
				inputDevices.Add(s_InputDevice);
			}
		}
	}

	public static void GetDevicesWithCharacteristics(InputDeviceCharacteristics desiredCharacteristics, List<InputDevice> inputDevices)
	{
		if (inputDevices == null)
		{
			throw new ArgumentNullException("inputDevices");
		}
		if (s_InputDeviceList == null)
		{
			s_InputDeviceList = new List<InputDevice>();
		}
		GetDevices_Internal(s_InputDeviceList);
		inputDevices.Clear();
		foreach (InputDevice s_InputDevice in s_InputDeviceList)
		{
			if ((s_InputDevice.characteristics & desiredCharacteristics) == desiredCharacteristics)
			{
				inputDevices.Add(s_InputDevice);
			}
		}
	}

	[RequiredByNativeCode]
	private static void InvokeConnectionEvent(ulong deviceId, ConnectionChangeType change)
	{
		switch (change)
		{
		case ConnectionChangeType.Connected:
			if (InputDevices.deviceConnected != null)
			{
				InputDevices.deviceConnected(new InputDevice(deviceId));
			}
			break;
		case ConnectionChangeType.Disconnected:
			if (InputDevices.deviceDisconnected != null)
			{
				InputDevices.deviceDisconnected(new InputDevice(deviceId));
			}
			break;
		case ConnectionChangeType.ConfigChange:
			if (InputDevices.deviceConfigChanged != null)
			{
				InputDevices.deviceConfigChanged(new InputDevice(deviceId));
			}
			break;
		}
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void GetDevices_Internal([NotNull] List<InputDevice> inputDevices);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool SendHapticImpulse(ulong deviceId, uint channel, float amplitude, float duration);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool SendHapticBuffer(ulong deviceId, uint channel, [NotNull] byte[] buffer);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetHapticCapabilities(ulong deviceId, out HapticCapabilities capabilities);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern void StopHaptics(ulong deviceId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureUsages(ulong deviceId, [NotNull] List<InputFeatureUsage> featureUsages);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_bool(ulong deviceId, string usage, out bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_UInt32(ulong deviceId, string usage, out uint value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_float(ulong deviceId, string usage, out float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_Vector2f(ulong deviceId, string usage, out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_Vector3f(ulong deviceId, string usage, out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_Quaternionf(ulong deviceId, string usage, out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_Custom(ulong deviceId, string usage, [Out] byte[] value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValueAtTime_bool(ulong deviceId, string usage, long time, out bool value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValueAtTime_UInt32(ulong deviceId, string usage, long time, out uint value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValueAtTime_float(ulong deviceId, string usage, long time, out float value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValueAtTime_Vector2f(ulong deviceId, string usage, long time, out Vector2 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValueAtTime_Vector3f(ulong deviceId, string usage, long time, out Vector3 value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValueAtTime_Quaternionf(ulong deviceId, string usage, long time, out Quaternion value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_XRHand(ulong deviceId, string usage, out Hand value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_XRBone(ulong deviceId, string usage, out Bone value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool TryGetFeatureValue_XREyes(ulong deviceId, string usage, out Eyes value);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern bool IsDeviceValid(ulong deviceId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string GetDeviceName(ulong deviceId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string GetDeviceManufacturer(ulong deviceId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern string GetDeviceSerialNumber(ulong deviceId);

	[MethodImpl(MethodImplOptions.InternalCall)]
	internal static extern InputDeviceCharacteristics GetDeviceCharacteristics(ulong deviceId);

	internal static InputDeviceRole GetDeviceRole(ulong deviceId)
	{
		InputDeviceCharacteristics deviceCharacteristics = GetDeviceCharacteristics(deviceId);
		if ((deviceCharacteristics & (InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice)) == (InputDeviceCharacteristics.HeadMounted | InputDeviceCharacteristics.TrackedDevice))
		{
			return InputDeviceRole.Generic;
		}
		if ((deviceCharacteristics & (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Left)) == (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Left))
		{
			return InputDeviceRole.LeftHanded;
		}
		if ((deviceCharacteristics & (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Right)) == (InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.Right))
		{
			return InputDeviceRole.RightHanded;
		}
		if ((deviceCharacteristics & InputDeviceCharacteristics.Controller) == InputDeviceCharacteristics.Controller)
		{
			return InputDeviceRole.GameController;
		}
		if ((deviceCharacteristics & (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.TrackingReference)) == (InputDeviceCharacteristics.TrackedDevice | InputDeviceCharacteristics.TrackingReference))
		{
			return InputDeviceRole.TrackingReference;
		}
		if ((deviceCharacteristics & InputDeviceCharacteristics.TrackedDevice) == InputDeviceCharacteristics.TrackedDevice)
		{
			return InputDeviceRole.HardwareTracker;
		}
		return InputDeviceRole.Unknown;
	}
}
