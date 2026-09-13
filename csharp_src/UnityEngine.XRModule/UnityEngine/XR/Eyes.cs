using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[NativeHeader("Modules/XR/XRPrefix.h")]
[NativeHeader("XRScriptingClasses.h")]
[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
[RequiredByNativeCode]
[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
[NativeConditional("ENABLE_VR")]
public struct Eyes : IEquatable<Eyes>
{
	private ulong m_DeviceId;

	private uint m_FeatureIndex;

	internal ulong deviceId => m_DeviceId;

	internal uint featureIndex => m_FeatureIndex;

	public bool TryGetLeftEyePosition(out Vector3 position)
	{
		return Eyes_TryGetEyePosition(this, EyeSide.Left, out position);
	}

	public bool TryGetRightEyePosition(out Vector3 position)
	{
		return Eyes_TryGetEyePosition(this, EyeSide.Right, out position);
	}

	public bool TryGetLeftEyeRotation(out Quaternion rotation)
	{
		return Eyes_TryGetEyeRotation(this, EyeSide.Left, out rotation);
	}

	public bool TryGetRightEyeRotation(out Quaternion rotation)
	{
		return Eyes_TryGetEyeRotation(this, EyeSide.Right, out rotation);
	}

	private static bool Eyes_TryGetEyePosition(Eyes eyes, EyeSide chirality, out Vector3 position)
	{
		return Eyes_TryGetEyePosition_Injected(ref eyes, chirality, out position);
	}

	private static bool Eyes_TryGetEyeRotation(Eyes eyes, EyeSide chirality, out Quaternion rotation)
	{
		return Eyes_TryGetEyeRotation_Injected(ref eyes, chirality, out rotation);
	}

	public bool TryGetFixationPoint(out Vector3 fixationPoint)
	{
		return Eyes_TryGetFixationPoint(this, out fixationPoint);
	}

	private static bool Eyes_TryGetFixationPoint(Eyes eyes, out Vector3 fixationPoint)
	{
		return Eyes_TryGetFixationPoint_Injected(ref eyes, out fixationPoint);
	}

	public bool TryGetLeftEyeOpenAmount(out float openAmount)
	{
		return Eyes_TryGetEyeOpenAmount(this, EyeSide.Left, out openAmount);
	}

	public bool TryGetRightEyeOpenAmount(out float openAmount)
	{
		return Eyes_TryGetEyeOpenAmount(this, EyeSide.Right, out openAmount);
	}

	private static bool Eyes_TryGetEyeOpenAmount(Eyes eyes, EyeSide chirality, out float openAmount)
	{
		return Eyes_TryGetEyeOpenAmount_Injected(ref eyes, chirality, out openAmount);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Eyes))
		{
			return false;
		}
		return Equals((Eyes)obj);
	}

	public bool Equals(Eyes other)
	{
		return deviceId == other.deviceId && featureIndex == other.featureIndex;
	}

	public override int GetHashCode()
	{
		return deviceId.GetHashCode() ^ (featureIndex.GetHashCode() << 1);
	}

	public static bool operator ==(Eyes a, Eyes b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(Eyes a, Eyes b)
	{
		return !(a == b);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Eyes_TryGetEyePosition_Injected(ref Eyes eyes, EyeSide chirality, out Vector3 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Eyes_TryGetEyeRotation_Injected(ref Eyes eyes, EyeSide chirality, out Quaternion rotation);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Eyes_TryGetFixationPoint_Injected(ref Eyes eyes, out Vector3 fixationPoint);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Eyes_TryGetEyeOpenAmount_Injected(ref Eyes eyes, EyeSide chirality, out float openAmount);
}
