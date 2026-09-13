using UnityEngine.Bindings;

namespace UnityEngine.XR;

[NativeType(Header = "Modules/XR/Subsystems/Display/XRDisplaySubsystemDescriptor.h")]
[NativeHeader("Modules/XR/XRPrefix.h")]
public struct XRMirrorViewBlitModeDesc
{
	public int blitMode;

	public string blitModeDesc;
}
