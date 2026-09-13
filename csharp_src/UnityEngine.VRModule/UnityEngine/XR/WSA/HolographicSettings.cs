using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.XR.WSA;

[StaticAccessor("HolographicSettings::GetInstance()", StaticAccessorType.Dot)]
[NativeHeader("Modules/VR/HoloLens/HolographicSettings.h")]
public class HolographicSettings
{
	public enum HolographicReprojectionMode
	{
		PositionAndOrientation,
		OrientationOnly,
		Disabled
	}

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static bool IsDisplayOpaque => true;

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	[NativeConditional("ENABLE_HOLOLENS_MODULE")]
	public static extern bool IsContentProtectionEnabled
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall)]
		set;
	}

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static HolographicReprojectionMode ReprojectionMode
	{
		get
		{
			return HolographicReprojectionMode.Disabled;
		}
		set
		{
		}
	}

	[Obsolete("Support for toggling latent frame presentation has been removed, and IsLatentFramePresentation will always return true", false)]
	public static bool IsLatentFramePresentation => true;

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static void SetFocusPointForFrame(Vector3 position)
	{
		InternalSetFocusPointForFrameP(position);
	}

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static void SetFocusPointForFrame(Vector3 position, Vector3 normal)
	{
		InternalSetFocusPointForFramePN(position, normal);
	}

	[Obsolete("Support for built-in VR will be removed in Unity 2020.1. Please update to the new Unity XR Plugin System. More information about the new XR Plugin System can be found at https://docs.unity3d.com/2019.3/Documentation/Manual/XR.html.", false)]
	public static void SetFocusPointForFrame(Vector3 position, Vector3 normal, Vector3 velocity)
	{
		InternalSetFocusPointForFramePNV(position, normal, velocity);
	}

	[NativeName("SetFocusPointForFrame")]
	[NativeConditional("ENABLE_HOLOLENS_MODULE")]
	private static void InternalSetFocusPointForFrameP(Vector3 position)
	{
		InternalSetFocusPointForFrameP_Injected(ref position);
	}

	[NativeName("SetFocusPointForFrame")]
	[NativeConditional("ENABLE_HOLOLENS_MODULE")]
	private static void InternalSetFocusPointForFramePN(Vector3 position, Vector3 normal)
	{
		InternalSetFocusPointForFramePN_Injected(ref position, ref normal);
	}

	[NativeConditional("ENABLE_HOLOLENS_MODULE")]
	[NativeName("SetFocusPointForFrame")]
	private static void InternalSetFocusPointForFramePNV(Vector3 position, Vector3 normal, Vector3 velocity)
	{
		InternalSetFocusPointForFramePNV_Injected(ref position, ref normal, ref velocity);
	}

	[Obsolete("Support for toggling latent frame presentation has been removed", true)]
	public static void ActivateLatentFramePresentation(bool activated)
	{
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetFocusPointForFrameP_Injected(ref Vector3 position);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetFocusPointForFramePN_Injected(ref Vector3 position, ref Vector3 normal);

	[MethodImpl(MethodImplOptions.InternalCall)]
	private static extern void InternalSetFocusPointForFramePNV_Injected(ref Vector3 position, ref Vector3 normal, ref Vector3 velocity);
}
