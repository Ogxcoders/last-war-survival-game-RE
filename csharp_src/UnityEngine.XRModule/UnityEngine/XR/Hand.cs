using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[NativeHeader("Modules/XR/Subsystems/Input/Public/XRInputDevices.h")]
[NativeHeader("XRScriptingClasses.h")]
[NativeHeader("Modules/XR/XRPrefix.h")]
[RequiredByNativeCode]
[NativeConditional("ENABLE_VR")]
[StaticAccessor("XRInputDevices::Get()", StaticAccessorType.Dot)]
public struct Hand : IEquatable<Hand>
{
	private ulong m_DeviceId;

	private uint m_FeatureIndex;

	internal ulong deviceId => m_DeviceId;

	internal uint featureIndex => m_FeatureIndex;

	public bool TryGetRootBone(out Bone boneOut)
	{
		return Hand_TryGetRootBone(this, out boneOut);
	}

	private static bool Hand_TryGetRootBone(Hand hand, out Bone boneOut)
	{
		return Hand_TryGetRootBone_Injected(ref hand, out boneOut);
	}

	public bool TryGetFingerBones(HandFinger finger, List<Bone> bonesOut)
	{
		if (bonesOut == null)
		{
			throw new ArgumentNullException("bonesOut");
		}
		return Hand_TryGetFingerBonesAsList(this, finger, bonesOut);
	}

	private static bool Hand_TryGetFingerBonesAsList(Hand hand, HandFinger finger, [NotNull] List<Bone> bonesOut)
	{
		return Hand_TryGetFingerBonesAsList_Injected(ref hand, finger, bonesOut);
	}

	public override bool Equals(object obj)
	{
		if (!(obj is Hand))
		{
			return false;
		}
		return Equals((Hand)obj);
	}

	public bool Equals(Hand other)
	{
		return deviceId == other.deviceId && featureIndex == other.featureIndex;
	}

	public override int GetHashCode()
	{
		return deviceId.GetHashCode() ^ (featureIndex.GetHashCode() << 1);
	}

	public static bool operator ==(Hand a, Hand b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(Hand a, Hand b)
	{
		return !(a == b);
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Hand_TryGetRootBone_Injected(ref Hand hand, out Bone boneOut);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern bool Hand_TryGetFingerBonesAsList_Injected(ref Hand hand, HandFinger finger, List<Bone> bonesOut);
}
