using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR;

[UsedByNativeCode]
[NativeType(Header = "Modules/XR/Subsystems/Display/XRDisplaySubsystemDescriptor.h")]
public class XRDisplaySubsystemDescriptor : IntegratedSubsystemDescriptor<XRDisplaySubsystem>
{
	[NativeConditional("ENABLE_XR")]
	public extern bool disablesLegacyVr
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_XR")]
	[NativeMethod("TryGetAvailableMirrorModeCount")]
	public extern int GetAvailableMirrorBlitModeCount();

	[MethodImpl(MethodImplOptions.InternalCall)]
	[NativeConditional("ENABLE_XR")]
	[NativeMethod("TryGetMirrorModeByIndex")]
	public extern void GetMirrorBlitModeByIndex(int index, out XRMirrorViewBlitModeDesc mode);
}
